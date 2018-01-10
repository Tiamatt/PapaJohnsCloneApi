using System;
using System.Collections.Generic;

namespace PapaJohnsCloneApi.ModelsAnalytics
{
    public partial class Customer
    {
        public Customer()
        {
            ItemDetail = new HashSet<ItemDetail>();
        }

        public Guid CustomerId { get; set; }
        public string Email { get; set; }
        public DateTime RegistrationDate { get; set; }

        public ICollection<ItemDetail> ItemDetail { get; set; }
    }
}
