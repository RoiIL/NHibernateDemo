using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernateDemo
{
    public class Customer
    {
        public Customer()
        {
            Orders = new HashSet<Order>();
        }

        public virtual Guid Id { get; set; }
        public virtual string FirstName { get; set; }
        public virtual string LastName { get; set; }

        public virtual  ISet<Order> Orders { get; set; }
    }
}
