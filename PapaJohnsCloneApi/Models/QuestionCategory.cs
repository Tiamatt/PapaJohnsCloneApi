using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.Models
{
    public partial class QuestionCategory
    {
        public QuestionCategory()
        {
            ItemSelectedQuestion = new HashSet<ItemSelectedQuestion>();
            Question = new HashSet<Question>();
        }

        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<ItemSelectedQuestion> ItemSelectedQuestion { get; set; }
        public ICollection<Question> Question { get; set; }
    }
}
