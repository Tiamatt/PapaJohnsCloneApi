using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class ErrorLogType
    {
        public ErrorLogType()
        {
            ErrorLog = new HashSet<ErrorLog>();
        }

        public int ErrorLogTypeId { get; set; }
        public string Name { get; set; }

        public ICollection<ErrorLog> ErrorLog { get; set; }
    }
}
