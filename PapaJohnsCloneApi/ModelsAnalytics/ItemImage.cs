using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class ItemImage
    {
        public Guid ItemImageId { get; set; }
        public Guid ItemId { get; set; }
        public string Src { get; set; }
        public bool IsMain { get; set; }
        public int? Size { get; set; }
        public string ImageType { get; set; }

        public Item Item { get; set; }
    }
}
