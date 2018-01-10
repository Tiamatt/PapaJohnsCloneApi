using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModelsAnalytics
{
    public class ItemImageModel
    {
        public Guid? itemImageId { get; set; }
        public Guid? itemId { get; set; }
        public string itemName { get; set; }
        public string src { get; set; }
        public bool? isMain { get; set; }
        public int? size { get; set; }
        public string imageType { get; set; }
    }
}
