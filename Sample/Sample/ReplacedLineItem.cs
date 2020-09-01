namespace Sample
{
    /// <summary>
    /// Basic info data row for rows replaced by a LineItem. These items are used as a list in the LineItem definition
    /// </summary>
    public class ReplacedLineItem
    {
        public ReplacedLineItem(int optionItemId, int lineItemId)
        {
            OptionItemId = optionItemId;
            LineItemId = lineItemId;
        }

        //Constructor for EF
        private ReplacedLineItem()
        {
        }

        public int OptionItemId { get; private set; }
        public int LineItemId { get; private set; }

        public virtual LineItem OptionItem { get; set; }
        public virtual LineItem LineItem { get; set; }
    }
}
