using System;
using System.Collections.Generic;
using System.Text;

namespace NHibernateDemo
{
    public class Order
    {
        public virtual Guid Id { get; set; }
        public virtual DateTime Ordered { get; set; }

        public virtual Customer Customer { get; set; }

        public override string ToString()
        {
            return string.Format("Order Id: {0}, Id");
        }
    }
}
