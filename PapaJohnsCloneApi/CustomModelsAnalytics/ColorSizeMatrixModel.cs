using PapaJohnsCloneApi.ModelsAnalytics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModelsAnalytics
{
    public class ColorSizeMatrixModel
    {
        public List<SizeModel> sizes { get; set; }
        public List<ColorModel> colors { get; set; }
        public List<List<int?>> quantities { get; set; }

        public ColorSizeMatrixModel()
        {
            this.sizes = new List<SizeModel>();
            this.colors = new List<ColorModel>();
            this.quantities = new List<List<int?>>();
        }
    }
}
