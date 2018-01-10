using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModelsAnalytics
{
    public class TextValueCheckedModel
    {
        public string valueStr{ get; set; }
        public int valueNum { get; set; }
        public string text { get; set; }
        public string extraText { get; set; }
        public bool isChecked { get; set; }
    }
}
