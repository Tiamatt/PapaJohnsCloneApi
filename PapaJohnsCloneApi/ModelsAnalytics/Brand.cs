using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class Brand
    {
        public Brand()
        {
            Item = new HashSet<Item>();
        }

        public int BrandId { get; set; }
        public string Name { get; set; }

        public ICollection<Item> Item { get; set; }
    }
}
