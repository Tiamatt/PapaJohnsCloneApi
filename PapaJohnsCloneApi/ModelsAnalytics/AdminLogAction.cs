using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class AdminLogAction
    {
        public AdminLogAction()
        {
            AdminLog = new HashSet<AdminLog>();
        }

        public int AdminLogActionId { get; set; }
        public string Name { get; set; }

        public ICollection<AdminLog> AdminLog { get; set; }
    }
}
