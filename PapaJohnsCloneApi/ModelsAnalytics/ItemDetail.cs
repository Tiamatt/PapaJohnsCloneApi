using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class ItemDetail
    {
        public Guid ItemDetailId { get; set; }
        public Guid ItemId { get; set; }
        public int SizeId { get; set; }
        public int ColorId { get; set; }
        public int Quantity { get; set; }
        public int ItemActionId { get; set; }
        public Guid? CustomerId { get; set; }

        public Color Color { get; set; }
        public Customer Customer { get; set; }
        public Item Item { get; set; }
        public ItemAction ItemAction { get; set; }
        public Size Size { get; set; }
    }
}
