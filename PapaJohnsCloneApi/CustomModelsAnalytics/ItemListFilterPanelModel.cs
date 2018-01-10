using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModelsAnalytics
{
    public class ItemListFilterPanelModel
    {
        public bool isShowPartialName_txt { get; set; }
        public string partialName { get; set; }
        public bool isShowActive_ddl { get; set; }
        public int active { get; set; }  // 0 is "All", 1 is "Active", 2 is "Not Active"
        public bool isShowGender_ddl { get; set; }
        public int gender { get; set; } // 0 is "All", 1 is "Women", 2 is "Men"
        public bool isShowCategory_chb { get; set; }
        public int[] category { get; set; }
        public bool isShowBrand_chb { get; set; }
        public int[] brand { get; set; }
    }
}
