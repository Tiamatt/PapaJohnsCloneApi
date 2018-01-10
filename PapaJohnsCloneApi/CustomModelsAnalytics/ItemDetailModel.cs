using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModelsAnalytics
{
    public class ItemDetailModel
    {
        public Guid itemDetailId { get; set; }
        public Guid itemId { get; set; }
        public string itemName { get; set; }
        public int sizeId { get; set; }
        public string sizeName { get; set; }
        public int colorId { get; set; }
        public string colorName { get; set; }
        public int quantity { get; set; }
        public int itemActionId { get; set; }
        public string itemActionName { get; set; }
        public Guid? customerId { get; set; }
        public string customerEmail { get; set; }
    }
}
