using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModels
{
    public class Item_M
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ItemCategoryId { get; set; }
        public string ItemCategoryName { get; set; }
        public decimal? Price { get; set; }
    }
}
