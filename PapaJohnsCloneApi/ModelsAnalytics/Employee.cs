using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class Employee
    {
        public Employee()
        {
            AdminLog = new HashSet<AdminLog>();
            ErrorLog = new HashSet<ErrorLog>();
        }

        public Guid EmployeeId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime HireDate { get; set; }
        public DateTime? TerminationDate { get; set; }
        public int PositionId { get; set; }
        public int DepartmentId { get; set; }
        public bool? IsActive { get; set; }
        public string Email { get; set; }

        public Department Department { get; set; }
        public Position Position { get; set; }
        public ICollection<AdminLog> AdminLog { get; set; }
        public ICollection<ErrorLog> ErrorLog { get; set; }
    }
}
