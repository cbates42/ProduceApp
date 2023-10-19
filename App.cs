//referenced my CSVEngine for StreamReader and StreamWriter
//referenced https://stackoverflow.com/questions/814548/how-to-replace-a-string-in-a-sql-server-table-column for how to replace
//referenced https://stackoverflow.com/questions/19370088/how-to-add-plus-one-1-to-a-sql-server-column-in-a-sql-query for how to raise all prices
//referenced https://stackoverflow.com/questions/17418258/datetime-format-to-sql-format-using-c-sharp for how to format datetimes

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;

namespace ProduceApp
{
    internal class App
    {
        public App() 
        {
            // on construction, reads the Produce.txt
            Read();
        }
        public void Read()
        {
            //build sqlconnection
            SqlConnectionStringBuilder mySqlConBldr = new SqlConnectionStringBuilder();
            mySqlConBldr["server"] = @"(localdb)\MSSQLLocalDB";
            mySqlConBldr["Trusted_Connection"] = true;
            mySqlConBldr["Integrated Security"] = "SSPI";
            mySqlConBldr["Initial Catalog"] = "PROG260FA23";
            string sqlConStr = mySqlConBldr.ToString();

            //get path to produce.txt
            var path = Directory.GetCurrentDirectory() + @"\Produce.txt";

            //open the streamreader to read the text
              using (StreamReader sr = new StreamReader(path)) 
              {
                //skipping the first line
                var line = sr.ReadLine();
                line = sr.ReadLine();

                while(line != null)
                {
                    //split at the pipes
                    var stats = line.Split('|');

                    //turn it into a datetime
                    DateTime.TryParse(stats[4], out DateTime currentsellby);
                    //turn into a float
                    float.TryParse(stats[2], out float currentPrice);

                    //create an instance of produce to store the info
                    Produce currentProduce = new Produce(stats[0].ToString(), stats[1].ToString(), currentPrice, stats[3].ToString(), currentsellby);

                    //connect to the table
                    using(SqlConnection conn = new SqlConnection(sqlConStr))
                    {
                        conn.Open();
                        //insert data into the table
                        string inlineSQL = $"INSERT [dbo].[Grocer] ([Name], [Location], [Price], [UoM], [SellByDate]) " +
     $"VALUES ('{currentProduce.name}', '{currentProduce.location}', '{currentProduce.price}', '{currentProduce.uom}', '{currentProduce.sellbydate:yyyy-MM-dd}')";
                        using (var command = new SqlCommand(inlineSQL, conn))
                        {
                            var query = command.ExecuteNonQuery();
                        }
                        conn.Close();
                    }
                    //read next line
                    line = sr.ReadLine();
                }
             //run the method to make changes to the table
              Update();
              }    
        }

        public void CreateOutput()
        {
            SqlConnectionStringBuilder mySqlConBldr = new SqlConnectionStringBuilder();
            mySqlConBldr["server"] = @"(localdb)\MSSQLLocalDB";
            mySqlConBldr["Trusted_Connection"] = true;
            mySqlConBldr["Integrated Security"] = "SSPI";
            mySqlConBldr["Initial Catalog"] = "PROG260FA23";
            string sqlConStr = mySqlConBldr.ToString();

            using (SqlConnection conn = new SqlConnection(sqlConStr))
            {
                conn.Open();
                string inlineSQL = @"Select * from Grocer";
                using(var command = new SqlCommand(inlineSQL, conn))
                {
                    var reader = command.ExecuteReader();
                    while(reader.Read())
                    {

                        using(StreamWriter sw = new StreamWriter(Directory.GetCurrentDirectory() + @"\UpdatedProduce.txt", true))
                        {
                            //Write the updated Produce.txt!
                            sw.Write($"{reader.GetValue(0)}|{reader.GetValue(1)}|{reader.GetValue(2)}|{reader.GetValue(3)}|{reader.GetValue(4)}");
                        }

                    }
                    reader.Close();
                }
                conn.Close();
            }
        }

        public void Update()
        {
            //run all changes 
            DeleteExpired();
            Increase();
            UpdateLoc();
            CreateOutput();
        }

        public void DeleteExpired()
        {
            //build string, deleting expired first so there's less to be run later
            SqlConnectionStringBuilder mySqlConBldr = new SqlConnectionStringBuilder();
            mySqlConBldr["server"] = @"(localdb)\MSSQLLocalDB";
            mySqlConBldr["Trusted_Connection"] = true;
            mySqlConBldr["Integrated Security"] = "SSPI";
            mySqlConBldr["Initial Catalog"] = "PROG260FA23";
            string sqlConStr = mySqlConBldr.ToString();

            using (SqlConnection conn = new SqlConnection(sqlConStr))
            {
                //delete all from the table that are from days that have expired
                conn.Open();
                string inlineSQL = $"DELETE from Grocer WHERE SellByDate < '{DateTime.Now:yyyy-MM-dd}'";
                using (var command = new SqlCommand(inlineSQL, conn))
                {
                   var query = command.ExecuteNonQuery();
                   Console.WriteLine("Expired produce deleted");
                }
                conn.Close();
            }
        }

        public void Increase()
        {
            SqlConnectionStringBuilder mySqlConBldr = new SqlConnectionStringBuilder();
            mySqlConBldr["server"] = @"(localdb)\MSSQLLocalDB";
            mySqlConBldr["Trusted_Connection"] = true;
            mySqlConBldr["Integrated Security"] = "SSPI";
            mySqlConBldr["Initial Catalog"] = "PROG260FA23";
            string sqlConStr = mySqlConBldr.ToString();

            using (SqlConnection conn = new SqlConnection(sqlConStr))
            {
                //raises all prices by one
                conn.Open();
                string inlineSQL = $"UPDATE Grocer SET Price = Price + 1";
                using (var command = new SqlCommand(inlineSQL, conn))
                {
                    var query = command.ExecuteNonQuery();
                    Console.WriteLine("Prices increased");
                }
                conn.Close();
            }
        }
        
        public void UpdateLoc()
        {
            SqlConnectionStringBuilder mySqlConBldr = new SqlConnectionStringBuilder();
            mySqlConBldr["server"] = @"(localdb)\MSSQLLocalDB";
            mySqlConBldr["Trusted_Connection"] = true;
            mySqlConBldr["Integrated Security"] = "SSPI";
            mySqlConBldr["Initial Catalog"] = "PROG260FA23";
            string sqlConStr = mySqlConBldr.ToString();

            using (SqlConnection conn = new SqlConnection(sqlConStr))
            {
                conn.Open();
                //replaces all with an F location to a Z location
                string inlineSQL = "UPDATE Grocer SET Location = REPLACE(Location, 'F', 'Z')";
                using (var command = new SqlCommand(inlineSQL, conn))
                {
                    var query = command.ExecuteNonQuery();
                    Console.WriteLine("Locations updated");
                }
                conn.Close();
            }
        }
   
    }
}


