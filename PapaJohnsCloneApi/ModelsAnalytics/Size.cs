using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class Size
    {
        public Size()
        {
            ItemDetail = new HashSet<ItemDetail>();
        }

        public int SizeId { get; set; }
        public string Sizing { get; set; }
        public string UsaSizing { get; set; }
        public string UkSizing { get; set; }
        public string EuropeSizing { get; set; }
        public string JapanSizing { get; set; }
        public string AustraliaSizing { get; set; }

        public ICollection<ItemDetail> ItemDetail { get; set; }
    }
}
