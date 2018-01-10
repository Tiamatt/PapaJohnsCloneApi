using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class Category
    {
        public Category()
        {
            Item = new HashSet<Item>();
        }

        public int CategoryId { get; set; }
        public string Name { get; set; }

        public ICollection<Item> Item { get; set; }
    }
}
