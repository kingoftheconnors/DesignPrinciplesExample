using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    /// <summary>
    /// Database-details class for Foam and Blowout insulation rating
    /// </summary>
    public class RValue
    {
        public decimal? R1 { get; set; }
        public decimal? R11 { get; set; }
        public decimal? R13 { get; set; }
        public decimal? R19 { get; set; }
        public decimal? R22 { get; set; }
        public decimal? R26 { get; set; }
        public decimal? R30 { get; set; }
        public decimal? R38 { get; set; }
        public decimal? R44 { get; set; }
        public decimal? R49 { get; set; }
        public decimal? R85 { get; set; }
        public decimal? R127 { get; set; }

        public decimal GetRValue(RValueEnum rvalue)
        {
            var returnValue = rvalue switch
            {
                RValueEnum.R1 => R1 ?? 1,
                RValueEnum.R11 => R11 ?? 1,
                RValueEnum.R13 => R13 ?? 1,
                RValueEnum.R19 => R19 ?? 1,
                RValueEnum.R22 => R22 ?? 1,
                RValueEnum.R26 => R26 ?? 1,
                RValueEnum.R30 => R30 ?? 1,
                RValueEnum.R38 => R38 ?? 1,
                RValueEnum.R44 => R44 ?? 1,
                RValueEnum.R49 => R49 ?? 1,
                RValueEnum.R85 => R85 ?? 1,
                RValueEnum.R127 => R127 ?? 1,
                _ => throw new InvalidOperationException("Invalid RValue selected"),
            };
            return returnValue == 0 ? 1 : returnValue;
        }
    }
}
