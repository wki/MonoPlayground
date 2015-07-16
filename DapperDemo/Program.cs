using System;
using Dapper;
using Npgsql;
using System.Data;
using System.Linq;

namespace DapperDemo
{
    class MainClass
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Postgres Demo");
            PostgresQuery();
        }

        public static void PostgresQuery()
        {
            using (var conn = new NpgsqlConnection("Server=localhost;Port=5432;User Id=postgres;Password=;Database=hausverwaltung;Encoding=UNICODE"))
            {
                conn.Open();
                // IDbTransaction transaction = conn.BeginTransaction();

                var persons = conn.Query<Person>("select * from person order by person_id");

                persons.ToList()
                    .ForEach(p => Console.WriteLine("Person #{0} is {1}", p.PersonId, p.Name));

//                foreach (var person in persons)
//                {
//                    Console.WriteLine("Person #{0} is {1}", person.PersonId, person.Name);
//                }

                // transaction.Rollback();
            }
        }
    }

    class Person 
    {
        // needed for mapping DB key to reasonable name
        private int person_id;

        public int PersonId { get { return person_id; } set { person_id = value; } }
        public string Name { get; set; }
        public string Shortname { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Phone { get; set; }
    }

}
