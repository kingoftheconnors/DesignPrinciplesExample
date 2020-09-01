using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    /// <summary>
    /// Default values class for building new LineItem objects. A LineItem present is defined for each MaterialRecord, which
    /// is used to build the new LineItem.
    /// </summary>
    public class LineItemPreset
    {
        public LineItemPreset(string description, decimal? yield, decimal? laborRate, decimal? discount, decimal? markUp,
                                    decimal? taxRate, bool taxable, bool surchargeExempt, Material material)
        {
            Description = description;
            Yield = yield;
            LaborRate = laborRate ?? material.DefaultLaborRate;
            Discount = discount;
            MarkUp = markUp ?? material.DefaultMarkup;
            TaxRate = taxRate ?? .1M;
            Taxable = taxable;
            SurchargeExempt = surchargeExempt;
        }

        public LineItemPreset(Material material)
        {
            LaborRate = material.DefaultLaborRate;
            MarkUp = material.DefaultMarkup;
        }

        //private constructor for EF
        private LineItemPreset()
        {
        }

        public string Description { get; set; }

        // Pricing variables
        public decimal? Yield { get; private set; }
        public decimal? LaborRate { get; private set; }
        public decimal? Discount { get; private set; }
        public decimal? MarkUp { get; private set; }
        public decimal? TaxRate { get; private set; } = .1M;

        // Pricing flags
        public bool Taxable { get; private set; } = true;
        public bool SurchargeExempt { get; private set; } = false;
    }
}
