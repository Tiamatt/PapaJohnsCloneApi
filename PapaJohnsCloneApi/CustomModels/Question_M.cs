using PapaJohnsCloneApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PapaJohnsCloneApi.CustomModels
{
    public class Question_M
    {
        public int QuestionCategoryId { get; set; }
        public string QuestionCategoryName { get; set; }
        public List<IdName_M> AllowedQuestionList { get; set; }
        public int SelectedQuestionId { get; set; }

        public Question_M()
        {
            this.AllowedQuestionList = new List<IdName_M>();
        }
    }
}
