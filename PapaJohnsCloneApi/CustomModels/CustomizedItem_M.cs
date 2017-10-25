using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModels
{
    public class CustomizedItem_M
    {
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public int ItemCategoryId { get; set; }
        public List<Question_M> QuestionList { get; set; }
        public List<Topping_M> ToppingList { get; set; }

        public CustomizedItem_M()
        {
            this.QuestionList = new List<Question_M>();
            this.ToppingList = new List<Topping_M>();
        }
}
}
