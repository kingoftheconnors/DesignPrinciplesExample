using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    /// <summary>
    /// Work Area Domain Object for calculating lineItem area-specific details
    /// </summary>
    public class WorkArea
    {
        public string Name { get; set; }
        public string WorkAreaCode { get; set; }
        public bool HasRule { get; set; }
    }
}
