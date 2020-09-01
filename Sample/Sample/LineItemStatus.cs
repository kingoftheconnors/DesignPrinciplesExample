using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    /// <summary>
    /// State object to define the current state-specific behavior of a LineItem
    /// </summary>
    public class LineItemStatus
    {
        public ILineItemState state = new LineItemStatusNormal();
        public LineItem item;

        public bool IsOptionItem { get => state.IsOptionItem; }
        public string Type { get => "NotApplicable"; }

        /// <summary>
        /// Marks this LineItem as replaced by another lineItem. This will add a new ReplacedLineItem object to the replacer's "ReplacedLineItems" instance variable
        /// </summary>
        public void MarkReplaced()
        {
            if (Type == "Normal" && !item.MaterialRecord.IsVentChute())
            {
                state = new LineItemStatusReplaced();
            }
        }

        /// <summary>
        /// Unmarks this LineItem as replaced by another lineItem. This will remove the ReplacedLineItem object from the original replacer's "ReplacedLineItems" instance variable
        /// </summary>
        public void MarkNotReplaced()
        {
            if (Type == "Replaced")
            {
                state = new LineItemStatusNormal();
            }
        }
    }
}
