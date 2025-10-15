using CLRIQTR.Data.Repositories.Interfaces;
using CLRIQTR.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace CLRIQTR.Data.Repositories.Implementations
{
    public class LookupRepository : ILookupRepository
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

        public List<LabMast> GetLabs()
        {
            var list = new List<LabMast>();
            const string sql = "SELECT labcode, labname FROM labmast ORDER BY labcode";

            using (var conn = new MySqlConnection(_connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var lab = new LabMast();

                        // Safely handle potential NULL values from the database
                        if (reader["labcode"] != DBNull.Value)
                        {
                            lab.LabCode = Convert.ToInt32(reader["labcode"]);
                        }

                        if (reader["labname"] != DBNull.Value)
                        {
                            lab.LabName = reader["labname"].ToString();
                        }

                        list.Add(lab);
                    }
                }
            }
            return list;
        }

        public List<DesMast> GetDesignations()
        {
            var list = new List<DesMast>();
            const string sql = "SELECT desid, desdesc FROM desmast ORDER BY desdesc";

            using (var conn = new MySqlConnection(_connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new DesMast
                        {
                            DesId = Convert.ToInt32(reader["desid"]),
                            DesDesc = reader["desdesc"] as string
                        });
                    }
                }
            }
            return list;
        }

        // LINQ IMPLEMENTATION (ALTERNATIVE - COMMENTED)
        /*
        public List<LabMast> GetLabs_LINQ()
        {
            using (var context = new MyDbContext())
            {
                return context.LabMasts.OrderBy(l => l.LabCode).ToList();
            }
        }
        */

        public void Dispose()
        {
        }
    }
}