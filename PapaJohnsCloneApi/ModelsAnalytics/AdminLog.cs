using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class AdminLog
    {
        public int AdminLogId { get; set; }
        public string TableName { get; set; }
        public string FieldName { get; set; }
        public string FieldValue { get; set; }
        public int AdminLogActionId { get; set; }
        public string Notes { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime Timestamp { get; set; }

        public AdminLogAction AdminLogAction { get; set; }
        public Employee Employee { get; set; }
    }
}
