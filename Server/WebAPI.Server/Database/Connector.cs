using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using WebAPI.Server.Models;

namespace WebAPI.Server.Database
{
    public class Connector
    {
        // api/Parts
        /// <summary>
        /// Makes connection to DB. Iterates through the DB file creating parts objects
        /// that satisfy the query conditions and adding them to a list then returns this list.
        /// </summary>
        /// <returns>A list of parts that was created from the DB on Willie's Server</returns>
        public List<Part> Get(String query)
        {
            var list = new List<Part>();
            try
            {
                var connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=E:\MUPS_2015\Parts\McPartsFinal-03 Edition.mdb;";

                using (var connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    var command = new OleDbCommand(query, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        var count = 0;
                        while (reader.Read() && count < 25)
                        {
                            var partName = reader.GetString(reader.GetOrdinal("PartName"));
                            var year = reader.GetString(reader.GetOrdinal("YR"));
                            var make = reader.GetString(reader.GetOrdinal("Make"));
                            var model = make;
                            if (make != null && make.Trim().Length > 0)
                            {
                                var makeChar = make.Substring(0, 1);
                                switch (makeChar)
                                {
                                    case "H":
                                        make = "Honda";
                                        break;
                                    case "K":
                                        make = "Kawasaki";
                                        break;
                                    case "Y":
                                        make = "Yamaha";
                                        break;
                                    case "S":
                                        make = "Suzuki";
                                        break;
                                }
                            }
                            if (model != null && model.Trim().Length >= 2)
                            {
                                model = model.Trim().Substring(2);
                            }
                            var price = reader.GetString(reader.GetOrdinal("Price"));
                            var location = reader.GetString(reader.GetOrdinal("Location"));
                            var pkParts = reader.GetInt32(reader.GetOrdinal("pkParts"));
                            var interchange = reader.GetString(reader.GetOrdinal("Interchange"));
                            list.Add(new Part
                            {
                                PartName = partName.Trim(),
                                Year = year.Trim(),
                                Make = make.Trim(),
                                Model = model,
                                Price = price.Trim(),
                                Location = location.Trim(),
                                PKParts = pkParts,
                                Interchange = interchange
                            });

                            count++;
                        }
                    }

                    return list;

                }

                Console.WriteLine("It worked!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("It didn't work!");
                Console.WriteLine(ex.Message);

                return new List<Part> { new Part { PartName = ex.Message, Make = ex.Source } };
            }
        }

        // api/Parts
        /// <summary>
        /// Makes connection to DB. Inserts an item into the database satisfying
        /// the insert statement.
        /// </summary>
        /// <returns>A boolean value indicating whether the insert was successful.</returns>
        public bool Insert(String insertStatement)
        {
            try
            {
                var connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=E:\MUPS_2015\Parts\McPartsFinal-03 Edition.mdb;";

                using (var connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    var command = new OleDbCommand(insertStatement, connection);
                    command.ExecuteNonQuery();

                    return true;
                }

                Console.WriteLine("It worked!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("It didn't work!");
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        // api/Parts
        /// <summary>
        /// Makes connection to DB. Deletes an item from the database that satisfies the delete statement.
        /// </summary>
        /// <returns>A boolean value indicating whether the delete was successful.</returns>
        public bool Delete(String deleteStatement)
        {
            try
            {
                var connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=E:\MUPS_2015\Parts\McPartsFinal-03 Edition.mdb;";

                using (var connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    var command = new OleDbCommand(deleteStatement, connection);
                    command.ExecuteNonQuery();

                    return true;
                }

                Console.WriteLine("It worked!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("It didn't work!");
                Console.WriteLine(ex.Message);

                return false;
            }
        }

        // api/Parts
        /// <summary>
        /// Makes connection to DB. Iterates through the DB file creating list
        /// of all years for which there is at least one part for the selected make.
        /// </summary>
        /// <returns>A list of valid years for the selected make.</returns>
        public List<string> GetYearSpinner(String query)
        {
            var list = new List<string>();
            try
            {
                var connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=E:\MUPS_2015\Parts\McPartsFinal-03 Edition.mdb;";

                using (var connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    var command = new OleDbCommand(query, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        var count = 0;
                        while (reader.Read())
                        {
                            var year = reader.GetString(reader.GetOrdinal("YR"));
                            list.Add(year.Trim());

                            count++;
                        }
                    }

                    return list;

                }

                Console.WriteLine("It worked!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("It didn't work!");
                Console.WriteLine(ex.Message);

                return new List<string> { "" };
            }
        }

        // api/Parts
        /// <summary>
        /// Makes connection to DB. Iterates through the DB file creating list
        /// of all part types available for the selected year and make.
        /// </summary>
        /// <returns>A list of valid part names for the selected year and make.</returns>
        public List<string> GetPartNameSpinner(String query)
        {
            var list = new List<string>();
            try
            {
                var connectionString = @"Provider=Microsoft.ACE.OLEDB.12.0; Data Source=E:\MUPS_2015\Parts\McPartsFinal-03 Edition.mdb;";

                using (var connection = new OleDbConnection(connectionString))
                {
                    connection.Open();
                    var command = new OleDbCommand(query, connection);
                    using (var reader = command.ExecuteReader())
                    {
                        var count = 0;
                        while (reader.Read())
                        {
                            var partName = reader.GetString(reader.GetOrdinal("PartName"));
                            var hyphenIndex = partName.Trim().IndexOf("-"); // Added to fix -xx issue in search results display
				            partName = partName.Substring(0,hyphenIndex)
                            list.Add(partName);

                            count++;
                        }
                    }

                    return list;

                }

                Console.WriteLine("It worked!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("It didn't work!");
                Console.WriteLine(ex.Message);

                return new List<string> { "" };
            }
        }
    }
}