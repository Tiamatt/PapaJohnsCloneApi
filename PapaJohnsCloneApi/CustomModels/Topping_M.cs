using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModels
{
    public class Topping_M
    {
        public int Id { get; set; }
        public int ToppingCategoryId { get; set; }
        public string Name { get; set; }
    }
}
