using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class Gender
    {
        public Gender()
        {
            Item = new HashSet<Item>();
        }

        public int GenderId { get; set; }
        public string Name { get; set; }

        public ICollection<Item> Item { get; set; }
    }
}
