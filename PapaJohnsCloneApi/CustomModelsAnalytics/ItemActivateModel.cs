using PapaJohnsCloneApi.EnumsAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModelsAnalytics
{
    public class ItemActivateModel
    {
        public string itemId { get; set; }
        public string itemName { get; set; }
        public bool isItemActive { get; set; }
        public List<ItemActivityEnum> itemActivities { get; set; }

    }
}
