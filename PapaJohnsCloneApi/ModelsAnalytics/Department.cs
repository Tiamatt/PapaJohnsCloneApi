using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class Department
    {
        public Department()
        {
            Employee = new HashSet<Employee>();
        }

        public int DepartmentId { get; set; }
        public string Name { get; set; }

        public ICollection<Employee> Employee { get; set; }
    }
}
