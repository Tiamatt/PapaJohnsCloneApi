using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.Models
{
    public partial class ToppingCategory
    {
        public ToppingCategory()
        {
            Topping = new HashSet<Topping>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<Topping> Topping { get; set; }
    }
}
