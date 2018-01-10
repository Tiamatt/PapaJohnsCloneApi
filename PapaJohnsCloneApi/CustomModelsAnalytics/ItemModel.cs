using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModelsAnalytics
{
    // Angular models-> Item.model.ts
    public class ItemModel
    {
        public Guid itemId { get; set; }  // NULL [uniqueidentifier] 
        public string name { get; set; }    // NOT NULL [nvarchar](50)
        public string description { get; set; } //  NOT NULL [nvarchar](2000)
        public int genderId { get; set; } //  NOT NULL [int] 
        public string genderName { get; set; } 
        public int categoryId { get; set; } //  NOT NULL [int]
        public string categoryName { get; set; }
        public int brandId { get; set; } //  NOT NULL [int] 
        public string brandName { get; set; }
        public decimal price { get; set; } //  NOT NULL [decimal](10, 2)
        public bool isActive { get; set; } // NOT NULL
    }
}
