using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class Color
    {
        public Color()
        {
            ItemDetail = new HashSet<ItemDetail>();
        }

        public int ColorId { get; set; }
        public string Name { get; set; }
        public string HexCode { get; set; }

        public ICollection<ItemDetail> ItemDetail { get; set; }
    }
}
