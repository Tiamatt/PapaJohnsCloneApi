using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.Models
{
    public partial class Topping
    {
        public Topping()
        {
            ItemSelectedToppings = new HashSet<ItemSelectedToppings>();
        }

        public int Id { get; set; }
        public int ToppingCategoryId { get; set; }
        public string Name { get; set; }

        public ToppingCategory ToppingCategory { get; set; }
        public ICollection<ItemSelectedToppings> ItemSelectedToppings { get; set; }
    }
}
