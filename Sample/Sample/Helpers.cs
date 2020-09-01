using System;

namespace Sample
{
    public class Helpers
    {
        /// <summary>
        /// Helper method to round any number UP to a certain number of decimals.
        /// e.g., 2.3526 => 2.36 when places=2
        /// </summary>
        /// <param name="input">Decimal number to round up</param>
        /// <param name="places">Decimal places to keep in the rounded number</param>
        /// <returns></returns>
        public static decimal RoundUp(decimal input, int places)
        {
            decimal multiplier = Convert.ToDecimal(Math.Pow(10, Convert.ToDouble(places)));
            return Math.Ceiling(input * multiplier) / multiplier;
        }
    }
}
