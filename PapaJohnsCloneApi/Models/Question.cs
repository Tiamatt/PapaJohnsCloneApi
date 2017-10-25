using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.Models
{
    public partial class Question
    {
        public Question()
        {
            ItemAllowedQuestions = new HashSet<ItemAllowedQuestions>();
            ItemPrice = new HashSet<ItemPrice>();
            ItemSelectedQuestion = new HashSet<ItemSelectedQuestion>();
        }

        public int Id { get; set; }
        public int QuestionCategoryId { get; set; }
        public string Name { get; set; }

        public QuestionCategory QuestionCategory { get; set; }
        public ICollection<ItemAllowedQuestions> ItemAllowedQuestions { get; set; }
        public ICollection<ItemPrice> ItemPrice { get; set; }
        public ICollection<ItemSelectedQuestion> ItemSelectedQuestion { get; set; }
    }
}
