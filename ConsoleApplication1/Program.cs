﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperLinq;

namespace ConsoleApplication1
{
    class Program
    {
       

        public static void CreateDB()
        {
            using (SqlConnection connection =
                        new SqlConnection(@"Data Source=(localdb)\v11.0;Integrated Security=True"))
            {
                connection.Open();

                using (var command = connection.CreateCommand())
                {
                    command.CommandType = CommandType.Text;
                    command.CommandText =
                        "IF NOT EXISTS ( SELECT [name] FROM sys.databases WHERE [name] = 'ORMTests' ) CREATE DATABASE ORMTests";
                    command.ExecuteNonQuery();

                    connection.ChangeDatabase("ORMTests");

                    command.CommandType = CommandType.Text;
                    command.CommandText = "IF EXISTS ( SELECT [name] FROM sys.tables WHERE [name] = 'Person' ) DROP Table Person";
                    command.ExecuteNonQuery();

                    command.CommandText =
                    "CREATE TABLE [dbo].[Person] (" +
                    "[Id]   INT NOT NULL," +
                    "[Name] NVARCHAR (255)   NOT NULL," +
                    "[Balance] FLOAT         NOT NULL," +
                    "[Age]  INT              NOT NULL);";
                    command.ExecuteNonQuery();

                    command.CommandText = "INSERT INTO [dbo].[Person] VALUES (1, 'Doron',1000, 29)";
                    command.ExecuteNonQuery();
                }
            }
        }

        static void Main(string[] args)
        {
            CreateDB();
            

            SqlConnection connection = new SqlConnection();
            var query = connection.Queryable<Person>().Where(p => p.Age > 13);

            foreach (var p in query)
            {
                Console.WriteLine(p);
            }
        }
    }
}
