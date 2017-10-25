using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.Models
{
    public partial class ItemCategory
    {
        public ItemCategory()
        {
            Item = new HashSet<Item>();
            ItemPrice = new HashSet<ItemPrice>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Item> Item { get; set; }
        public ICollection<ItemPrice> ItemPrice { get; set; }
    }
}
