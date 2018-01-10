using PapaJohnsCloneApi.EnumsAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModelsAnalytics
{
    public class ItemViewModel
    {
        public ItemModel item { get; set; }
        public List<ItemImageModel> itemImages { get; set; }
    }
}
