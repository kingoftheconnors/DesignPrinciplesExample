using System;
using System.Collections.Generic;
using System.Linq;

namespace Sample
{
    /// <summary>
    /// Order information for a quantity of material goods ordered for construction purposes.
    /// Keeps track of order status and based financial information.
    /// </summary>
    public abstract class LineItem
    {
        public int EstimateId { get; internal set; }
        public string Description { get; internal set; }

        // Pricing variables
        public decimal Quantity { get; internal set; } = 0;
        public decimal? Yield { get; internal set; } = 0;
        public decimal LaborRate { get; internal set; } = 0;
        public decimal Discount { get; internal set; } = 0;
        public decimal MarkUp { get; internal set; } = 0;
        public decimal TaxRate { get; internal set; } = .1M;
        public int? MaterialRecordId { get; internal set; }
        public MaterialRecord MaterialRecord { get; internal set; } // Bought material. Includes material's base price and container information
        public decimal TaxAmount => MaterialRecord.GetMaterialCost(Quantity, Yield) * TaxRate;

        public decimal UnitPrice => MaterialRecord.Total(Quantity, Yield, MarkUp, LaborRate, TaxRate) / (Quantity == 0 ? 1 : Quantity);
        public decimal TotalLabor => LaborRate * Quantity;

        // Pricing flags
        public bool Taxable { get; internal set; } = true;
        public bool SurchargeExempt { get; internal set; } = false;
        public bool DoNotShowOnEstimate { get; internal set; } = false;
        public bool DoNotShowOnWorkOrder { get; internal set; } = false;

        // External Domain Object Variables
        public int? StatusId { get; internal set; }
        public LineItemStatus Status;
        public int? WorkAreaId { get; internal set; }
        public WorkArea WorkArea { get; internal set; }
        public List<ReplacedLineItem> ReplacedLineItems { get; internal set; }
        public LineItemCategoryEnum LineItemCategory { get; internal set; }
        public int? WorkOrderId { get; internal set; }

        // Line Item Classification Flags
        public bool IsSlopedCeilingLineItem => WorkAreaId == (int)WorkAreaEnum.SlopedCeiling && !MaterialRecord.IsVentChute();
        public bool AffectsSlopedCeilingLineItems => IsSlopedCeilingLineItem || (ReplacedLineItems?.Where(l => l.LineItem?.IsSlopedCeilingLineItem ?? false).Any() ?? false);

        /// <summary>
        /// Edit order function to update the quantity of items ordered and the taxRate of the order.
        /// The way the quantity of items ordered is calculated depends on the subclass.
        /// </summary>
        /// <param name="containersRequired">The new number of required containers</param>
        /// <param name="taxRate">The new tax rate</param>
        public void UpdateEstimatedFields(decimal containersRequired, decimal taxRate)
        {
            UpdateEstimatedQuantityForNewContainersRequired(containersRequired);
            TaxRate = taxRate;
        }

        protected abstract void UpdateEstimatedQuantityForNewContainersRequired(decimal containersRequired);

        /// <summary>
        /// Tests if two lineItem records match. Assumes that if two IDs
        /// are equal, all values inside those records are also equal
        /// </summary>
        /// <param name="lineItem">Line item to compare against this one for equality</param>
        /// <returns>True or false if the two line items are the same item</returns>
        public bool IsMatch(LineItem lineItem)
        {
            return lineItem.Quantity == Quantity
                   && lineItem.MaterialRecordId == MaterialRecordId
                   && lineItem.WorkAreaId == WorkAreaId
                   && lineItem.Status.Type == Status.Type
                   && lineItem.Taxable == Taxable
                   && lineItem.Yield == Yield;
        }
    }
}
