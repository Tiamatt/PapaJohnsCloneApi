using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.Models
{
    public partial class ItemPrice
    {
        public int Id { get; set; }
        public int? ItemId { get; set; }
        public int ItemCategoryId { get; set; }
        public int? QuestionIdSize { get; set; }
        public decimal BasicPrice { get; set; }
        public decimal? PricePerTopping { get; set; }

        public Item Item { get; set; }
        public ItemCategory ItemCategory { get; set; }
        public Question QuestionIdSizeNavigation { get; set; }
    }
}
