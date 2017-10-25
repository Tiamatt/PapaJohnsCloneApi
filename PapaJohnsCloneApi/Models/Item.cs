using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.Models
{
    public partial class Item
    {
        public Item()
        {
            ItemAllowedQuestions = new HashSet<ItemAllowedQuestions>();
            ItemPrice = new HashSet<ItemPrice>();
            ItemSelectedQuestion = new HashSet<ItemSelectedQuestion>();
            ItemSelectedToppings = new HashSet<ItemSelectedToppings>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int ItemCategoryId { get; set; }
        public bool? IsDisabled { get; set; }
        public bool? IsSpecial { get; set; }

        public ItemCategory ItemCategory { get; set; }
        public ICollection<ItemAllowedQuestions> ItemAllowedQuestions { get; set; }
        public ICollection<ItemPrice> ItemPrice { get; set; }
        public ICollection<ItemSelectedQuestion> ItemSelectedQuestion { get; set; }
        public ICollection<ItemSelectedToppings> ItemSelectedToppings { get; set; }
    }
}
