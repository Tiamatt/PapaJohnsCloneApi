using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class Item
    {
        public Item()
        {
            ItemDetail = new HashSet<ItemDetail>();
            ItemImage = new HashSet<ItemImage>();
        }

        public Guid ItemId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public int GenderId { get; set; }
        public decimal Price { get; set; }
        public bool IsActive { get; set; }

        public Brand Brand { get; set; }
        public Category Category { get; set; }
        public Gender Gender { get; set; }
        public ICollection<ItemDetail> ItemDetail { get; set; }
        public ICollection<ItemImage> ItemImage { get; set; }
    }
}
