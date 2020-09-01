using System;
using System.Collections.Generic;
using System.Text;

namespace Sample
{
    /// <summary>
    /// State object to define the current state-specific behavior of a LineItem
    /// </summary>
    public interface ILineItemState
    {
        public bool IsOptionItem { get; }
        public string Type { get; }

    }

    public class LineItemStatusNotApplicable : ILineItemState
    {
        public bool IsOptionItem { get => false; }
        public string Type { get => "NotApplicable"; }

    }

    public class LineItemStatusError : ILineItemState
    {
        public bool IsOptionItem { get => false; }
        public string Type { get => "Error"; }

    }
    public class LineItemStatusNormal : ILineItemState
    {
        public bool IsOptionItem { get => false; }
        public string Type { get => "Normal"; }

    }
    public class LineItemStatusReplaced : ILineItemState
    {
        public bool IsOptionItem { get => false; }
        public string Type { get => "Replaced"; }

    }
    public class LineItemStatusOptionItem : ILineItemState
    {
        public bool IsOptionItem { get => true; }
        public string Type { get => "OptionItem"; }

    }
    public class LineItemStatusAcceptedOptionItem : ILineItemState
    {
        public bool IsOptionItem { get => true; }
        public string Type { get => "AcceptedOptionItem"; }

    }
    public class LineItemStatusRejectedOptionItem : ILineItemState
    {
        public bool IsOptionItem { get => true; }
        public string Type { get => "RejectedOptionItem"; }

    }
}
