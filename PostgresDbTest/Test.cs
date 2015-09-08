using NUnit.Framework;
using Npgsql;
using Dapper;
using System;
using System.Data;
using System.IO;

namespace PostgresDbTest
{
    [TestFixture]
    public class Test
    {
        private const string DB_NAME = "xxx_test";

        NpgsqlConnection connection;
        
        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            CreateDb();

            connection = new NpgsqlConnection(ConnectionString(DB_NAME));
            connection.Open();

            connection.Execute(Sql("BuildTables.txt"));
        }

        [Test]
        public void TestCase()
        {
            Assert.AreEqual(1,1);
        }


        private void CreateDb()
        {
            using (var conn = new NpgsqlConnection(ConnectionString("postgres")))
            {
                conn.Open();

                conn.Execute($"drop database if exists {DB_NAME}");
                conn.Execute($"CREATE DATABASE {DB_NAME} WITH TEMPLATE = template0 OWNER = postgres ENCODING = 'UTF8'");
                conn.Close();
            }
        }

        private string ConnectionString(string dbName)
        {
            return $"Server=localhost;Port=5432;User Id=postgres;Password=;Database={dbName};Encoding=UNICODE";
        }

        private string Sql(string filename)
        {
            var assembly = GetType().Assembly;
            var resource = GetType().Namespace + "." + filename;

            using (var resourceStream = assembly.GetManifestResourceStream(resource))
            using (var resourceReader = new StreamReader(resourceStream))
            {
                return resourceReader.ReadToEnd();
            }
        }
    }
}
