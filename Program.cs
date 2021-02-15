using System;
using System.Linq;
using System.Reflection;
using HibernatingRhinos.Profiler.Appender.NHibernate;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.ConfigurationSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Linq;
using NHibernate.Linq.ReWriters;
using NHibernate.Type;

namespace NHibernateDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            NHibernateProfilerBootstrapper.PreStart();
            NHibernateProfiler.Initialize();

            Console.WriteLine("Here we go!");
            var config = new Configuration();

            // Using configuration file (hibernate.cfg.xml):
            config.Configure();

            // Using code for configuration:
            //config.DataBaseIntegration(x =>
            //{
            //    // Integrated Security: false - needs User ID and Password, SSPI - uses the Windows account credentials
            //    x.ConnectionString = "Server=192.168.68.103;Database=NHibernateDemo;User ID=SA;Password=1Secure*Password1;Persist Security Info=True;Integrated Security=false;";
            //    x.Driver<SqlClientDriver>();
            //    x.Dialect<MsSql2008Dialect>();
            //});
            //config.AddAssembly(Assembly.GetExecutingAssembly());

            var sessionFactory = config.BuildSessionFactory();

            #region SaveToDatabase

            //using var saveSession = sessionFactory.OpenSession();
            //using var saveTx = saveSession.BeginTransaction();

            //var newCustomer = new Customer
            //{
            //    FirstName = "David",
            //    LastName = "Jones",
            //    Orders =
            //    {
            //        new Order
            //        {
            //            Ordered = DateTime.Now
            //        },
            //        new Order
            //        {
            //            Ordered = DateTime.Now
            //        }
            //    }
            //};

            //saveSession.Save(newCustomer);
            //saveTx.Commit();

            #endregion

            #region GetFromDatabase

            // LINQ
            using var session = sessionFactory.OpenSession();
            using var tx = session.BeginTransaction();
            var query = from customer in session.Query<Customer>()
                        where customer.LastName.Length > 3
                        orderby customer.LastName
                        select customer;

            // Fetch on the query to avoid round trip to the database
            var reloaded = query.Fetch(x => x.Orders).ToList();
            foreach (var customer in reloaded)
            {
                Console.WriteLine("LINQ: ");
                Console.WriteLine("{0} {1}", customer.FirstName, customer.LastName);
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine(order.Id);
                }
            }

            // Native SQL
            var sql = @"
                select *
                from dbo.Customer";
            var sqlQuery = session.CreateSQLQuery(sql).AddEntity(typeof(Customer)).List<Customer>();

            foreach (var customer in sqlQuery)
            {
                Console.WriteLine("Native SQL: ");
                Console.WriteLine("{0} {1}", customer.FirstName, customer.LastName);
                foreach (var order in customer.Orders)
                {
                    Console.WriteLine(order.Id);
                }
            }


            tx.Commit();

            // Console.ReadLine();

            #endregion

        }
    }
}

