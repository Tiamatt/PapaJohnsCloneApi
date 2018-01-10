using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class ErrorLog
    {
        public int ErrorLogId { get; set; }
        public string Namespace { get; set; }
        public string Class { get; set; }
        public string Method { get; set; }
        public string MethodParams { get; set; }
        public int ErrorLogTypeId { get; set; }
        public string ShortComment { get; set; }
        public string DetailedComment { get; set; }
        public string Exception { get; set; }
        public Guid EmployeeId { get; set; }
        public DateTime Timestamp { get; set; }

        public Employee Employee { get; set; }
        public ErrorLogType ErrorLogType { get; set; }
    }
}
