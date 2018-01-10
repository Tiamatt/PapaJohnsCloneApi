using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class PriceRange
    {
        public int PriceRangeId { get; set; }
        public decimal Min { get; set; }
        public decimal Max { get; set; }
    }
}
