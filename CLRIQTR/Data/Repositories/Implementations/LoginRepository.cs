using CLRIQTR.Data.Repositories.Interfaces;
using CLRIQTR.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;

namespace CLRIQTR.Data.Repositories.Implementations
{
    public class LoginRepository : ILoginRepository
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

        public EmpMastTest ValidateLogin(string empNo, string password, int labCode)
        {
            const string sql = @"
                                  SELECT empno, empname, labcode, password 
                                 FROM adminlogin 
                                 WHERE empno = @EmpNo AND password = @Password AND labcode = @LabCode";

            using (var conn = new MySqlConnection(_connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@empno", empNo);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@labcode", labCode);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new EmpMastTest
                        {
                            EmpNo = reader["empno"].ToString(),
                            EmpName = reader["empname"] as string,
                            LabCode = Convert.ToInt32(reader["labcode"]),
                           
                            Password = reader["password"].ToString()


                        };
                    }
                }
            }
            return null;
        }

        public List<LabMast> GetAllLabs()
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
                        list.Add(new LabMast
                        {
                            LabCode = Convert.ToInt32(reader["labcode"]),
                            LabName = reader["labname"].ToString()
                        });
                    }
                }
            }
            return list;
        }

        // LINQ IMPLEMENTATION (ALTERNATIVE - COMMENTED)
        /*
        public EmpMastTest ValidateLogin_LINQ(string empNo, int labCode)
        {
            using (var context = new MyDbContext())
            {
                return context.EmpMastTests.FirstOrDefault(e => 
                    e.EmpNo == empNo && 
                    e.LabCode == labCode && 
                    (e.Active == null || e.Active.ToUpper() != "N"));
            }
        }
        */

        public void Dispose()
        {
        }
    }
}