using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    /// <summary>
    /// Material specifics domain object. Includes instance-specific financial and quality data for a material. This class uses Hierarchy-Per-Table
    /// functionality in Entity Framework to apply data to models with material-specific domain logic. These can be several records of differing manufacturers
    /// selling the same base material. To add a new material, simply add a new materialRecord subclass and define its specific inherited functionality.
    /// </summary>
    public class MaterialRecord
    {
        protected MaterialRecord()
        {
        }

        public decimal Price { get; private set; }
        public bool Priority { get; private set; } = false;
        public string SKU { get; private set; }
        public string Manufacturer { get; private set; }
        public int LineItemPresetId { get; private set; }
        public LineItemPreset LineItemPreset { get; private set; }
        public int MaterialId { get; private set; }
        public Material Material { get; private set; }
        public DateTime CreatedDate { get; private set; }

        /// <summary>
        /// Tester method to check if a material is, in fact, a Vent Chute
        /// </summary>
        /// <returns>Flag signifying whether the material is a vent chute</returns>
        public bool IsVentChute() => Material?.Id == (int)MaterialEnum.VentChute;

        /// <summary>
        /// Getter method that calculates the amount of that material that can be stored per container. For some materials,
        /// this is a function of the amount of material being ordered overall and the Yield of the construction project
        /// </summary>
        /// <param name="Quantity">Amount of material being ordered</param>
        /// <param name="Yield">Expected annual return for the construction firm making the order</param>
        /// <returns>Amount of material for a container</returns>
        public virtual decimal GetAmountPerContainer(decimal Quantity, decimal? Yield) => 0;

        /// <summary>
        /// Getter method that calculates the number of containers needed to fit a certain amount of the current material
        /// </summary>
        /// <param name="Quantity">Amount of material being ordered</param>
        /// <param name="Yield">Expected annual return for the construction firm making the order</param>
        /// <returns>Number of containers needed</returns>
        public virtual decimal GetNumContainersRequired(decimal Quantity, decimal? Yield) => 0;

        /// <summary>
        /// Getter method for the catalog name of this item
        /// </summary>
        /// <param name="Quantity">Amount of material being ordered</param>
        /// <param name="Yield">Expected annual return for the construction firm making the order</param>
        /// <returns>Number of containers needed</returns>
        public virtual string GetLineItemName(string workAreaName) { return $"{workAreaName} {Material.Name}"; }

        /// <summary>
        /// Getter method for the cost of the material based on the quantity required
        /// </summary>
        /// <param name="Quantity">Amount of material being ordered</param>
        /// <param name="Yield">Expected annual return for the construction firm making the order</param>
        /// <returns></returns>
        public virtual decimal GetMaterialCost(decimal Quantity, decimal? Yield) { throw new InvalidOperationException("Invalid Material Type"); }

        /// <summary>
        /// Calculator method for the total cost of the material including tax rate, cost of labor, and product markup
        /// </summary>
        /// <param name="Quantity">Amount of material being ordered</param>
        /// <param name="Yield">Expected annual return for the construction firm making the order</param>
        /// <param name="MarkUp">MarkUp percentage of the product</param>
        /// <param name="LaborRate">Labor rate required to install the material</param>
        /// <param name="TaxRate">Tax rate of the local government</param>
        /// <returns></returns>
        public virtual decimal Total(decimal Quantity, decimal? Yield, decimal MarkUp, decimal LaborRate, decimal TaxRate)
        {
            decimal markup = MarkUp != 0 ? MarkUp : 1;
            decimal adjustedLaborRate = (LaborRate * Quantity) / markup;
            decimal adjustedMaterialCost = GetMaterialCost(Quantity, Yield) / markup;
            decimal tax = GetMaterialCost(Quantity, Yield) * TaxRate;
            decimal laborRateSurcharge = (LaborRate * Quantity * Settings.Instance.TotalLaborSurchargePercent);
            return Helpers.RoundUp(adjustedMaterialCost + adjustedLaborRate + tax + laborRateSurcharge, 2);
        }

        /// <summary>
        /// Calculator method for the total projit for the manufacturer gained from selling the material
        /// </summary>
        /// <param name="Quantity">Amount of material being ordered</param>
        /// <param name="Yield">Expected annual return for the construction firm making the order</param>
        /// <param name="MarkUp">MarkUp percentage of the product</param>
        /// <param name="LaborRate">Labor rate required to install the material</param>
        /// <param name="TaxRate">Tax rate of the local government</param>
        /// <param name="TotalLabor">Total amount of labor required to install the material</param>
        /// <returns></returns>
        public virtual decimal Profit(decimal Quantity, decimal? Yield, decimal MarkUp, decimal LaborRate, decimal TaxRate, decimal TotalLabor)
        {
            return Total(Quantity, Yield, MarkUp, LaborRate, TaxRate) - (TotalLabor + GetMaterialCost(Quantity, Yield)) - (GetMaterialCost(Quantity, Yield) * TaxRate) - (TotalLabor * Settings.Instance.TotalLaborSurchargePercent);
        }
    }

    /// <summary>
    /// Material specifics class for records for selling Foam-Material items
    /// </summary>
    public class MaterialFoamRecord : MaterialRecord
    {
        public decimal? RValuePerInch { get; internal set; }
        public RValue RValue { get; private set; }
        public virtual decimal? ItemRValue { get; internal set; }
        public decimal? Depth { get; internal set; }

        public override decimal GetAmountPerContainer(decimal Quantity, decimal? Yield) => (Quantity * Depth.Value) / Yield.Value;
        public override decimal GetNumContainersRequired(decimal Quantity, decimal? Yield) => GetAmountPerContainer(Quantity, Yield);
        public override string GetLineItemName(string BaseDescription) { return $"R{Math.Round(ItemRValue ?? 0, 0)} {Material.Name} {Math.Round(Depth ?? 0, 2)}in."; }
        public override decimal GetMaterialCost(decimal Quantity, decimal? Yield) { return Price * GetAmountPerContainer(Quantity, Yield); }
    }

    /// <summary>
    /// Material specifics class for records for selling Attic-Blowout-Material items
    /// </summary>
    public class MaterialAtticblowRecord : MaterialRecord
    {
        public decimal? RValuePerInch { get; internal set; }
        public RValue RValue { get; private set; }
        public virtual decimal? ItemRValue { get; internal set; }
        public decimal? Depth { get; internal set; }

        public override decimal GetAmountPerContainer(decimal Quantity, decimal? Yield) => Settings.Instance.AtticBlowSqftBase / RValue.GetRValue((RValueEnum)(ItemRValue ?? 0));
        public override decimal GetNumContainersRequired(decimal Quantity, decimal? Yield) => Quantity / GetAmountPerContainer(Quantity, Yield);
        public override string GetLineItemName(string BaseDescription) { return $"R{Math.Round(ItemRValue ?? 0, 0)} {BaseDescription}"; }
        public override decimal GetMaterialCost(decimal Quantity, decimal? Yield) { return Price * GetNumContainersRequired(Quantity, Yield); }
    }

    /// <summary>
    /// Material specifics class for records for selling Fiberglass-Material items
    /// </summary>
    public class MaterialFiberglassRecord : MaterialRecord
    {
        public decimal? AmountPerContainer { get; private set; }

        public override decimal GetNumContainersRequired(decimal Quantity, decimal? Yield) => Quantity / GetAmountPerContainer(Quantity, Yield);
        public override decimal GetAmountPerContainer(decimal Quantity, decimal? Yield) => AmountPerContainer.Value;
        public override decimal GetMaterialCost(decimal Quantity, decimal? Yield) { return Price * Quantity; }
    }

    /// <summary>
    /// Material specifics class for records for selling Accessory items
    /// </summary>
    public class MaterialAccessoryRecord : MaterialRecord
    {
        public override decimal GetNumContainersRequired(decimal Quantity, decimal? Yield) => 1;
        public override decimal GetAmountPerContainer(decimal Quantity, decimal? Yield) => 1;
        public override decimal GetMaterialCost(decimal Quantity, decimal? Yield) { return Price * Quantity; }
    }

    /// <summary>
    /// Material specifics class for records for selling Paint-Material items
    /// </summary>
    public class MaterialPaintRecord : MaterialRecord
    {
        public override decimal GetNumContainersRequired(decimal Quantity, decimal? Yield) => Quantity / GetAmountPerContainer(Quantity, Yield);
        public override decimal GetAmountPerContainer(decimal Quantity, decimal? Yield) => Yield.Value;
        public override decimal GetMaterialCost(decimal Quantity, decimal? Yield) { return Price * (Quantity / Yield.Value); }
    }

    /// <summary>
    /// Material specifics class for records for selling Fees
    /// </summary>
    public class MaterialFeeRecord : MaterialRecord
    {
        public override decimal GetMaterialCost(decimal Quantity, decimal? Yield) { return 0; }
        public override decimal Total(decimal Quantity, decimal? Yield, decimal MarkUp, decimal LaborRate, decimal TaxRate) { return Price + LaborRate; }
        public override decimal Profit(decimal Quantity, decimal? Yield, decimal MarkUp, decimal LaborRate, decimal TaxRate, decimal TotalLabor) { return Total(Quantity, Yield, MarkUp, LaborRate, TaxRate); }
    }

    /// <summary>
    /// Material specifics class for records for selling Material-Removal services
    /// </summary>
    public class MaterialRemovalRecord : MaterialRecord
    {
        public override decimal GetMaterialCost(decimal Quantity, decimal? Yield) { return 0; }
        public override decimal Profit(decimal Quantity, decimal? Yield, decimal MarkUp, decimal LaborRate, decimal TaxRate, decimal TotalLabor)
        {
            return Total(Quantity, Yield, MarkUp, LaborRate, TaxRate) - TotalLabor - -(TotalLabor * Settings.Instance.TotalLaborSurchargePercent);
        }
    }
}
