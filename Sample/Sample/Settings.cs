using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    /// <summary>
    /// Globally available Singleton settings manager
    /// </summary>
    public class Settings
    {
        private Settings()
        {
        }

        private static Settings _Instance;

        public static Settings Instance
        {
            get
            {
                if (_Instance == null)
                {
                    _Instance = new Settings();
                }
                return _Instance;
            }
        }

        public decimal TotalLaborSurchargePercent = (decimal)0.1215;

        public decimal AtticBlowSqftBase = 1000;
    }
}
