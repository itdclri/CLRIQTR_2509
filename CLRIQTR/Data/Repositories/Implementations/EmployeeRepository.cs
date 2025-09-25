using CLRIQTR.Data.Repositories.Interfaces;
using CLRIQTR.Models;
using MySql.Data.MySqlClient;
using Mysqlx.Crud;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;

namespace CLRIQTR.Data.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

        public EmpMastTest GetEmployeeByEmpNo(string empNo)
        {
            const string sql = @"SELECT * FROM empmast e 
                                INNER JOIN desmast d ON e.designation = d.desid 
                                INNER JOIN labmast l ON e.labcode = l.labcode  
                                WHERE empno = @empno";

            using (var conn = new MySqlConnection(_connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@empno", empNo);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapEmployeeFromReader(reader);
                    }
                }
            }
            return null;
        }


        public EmpMastTest GetEmployeeByNoForView(string empNo)
        {
            const string sql = @"SELECT e.*, d.DesDesc, l.LabName,q.qtrno,q.occdate,q.qtrtype,q.qtrstatus
                         FROM empmast e
                         INNER JOIN desmast d ON e.Designation = d.DesId
                         INNER JOIN labmast l ON e.LabCode = l.LabCode
                         left JOIN qtrupd q ON q.empno = e.empno 
                         WHERE e.EmpNo = @empNo";

            using (var conn = new MySqlConnection(_connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@empNo", empNo);
                conn.Open();

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapEmployeeFromReaderForView(reader);
                    }
                }
            }

            return null;
        }


        public List<EmpMastTest> GetEmployeesByLab(int labCode, string empNo = null, string empName = null, string designation = null,string status=null)
        {
            var list = new List<EmpMastTest>();
            var sql = @"SELECT * FROM empmast e 
               INNER JOIN desmast d ON e.designation = d.desid 
               INNER JOIN labmast l ON e.labcode = l.labcode
               LEFT JOIN QTRUPD q ON e.empno=q.empno
               WHERE e.labcode = @labcode";

        

            // Build dynamic WHERE clause
            if (!string.IsNullOrEmpty(empNo))
                sql += " AND e.empno LIKE @empno";
            if (!string.IsNullOrEmpty(empName))
                sql += " AND e.empname LIKE @empname";
            if (!string.IsNullOrEmpty(designation))
                sql += " AND e.designation = @designation ";
            if (!string.IsNullOrEmpty(status))
            {
                if (status == "Occupied")
                {
                    status = "O";
                    sql += " AND q.qtrstatus  = @status";
                }
                else {
                sql += " AND q.qtrstatus is null";
            }
            }
            sql += " order by e.empname ";
            
            using (var conn = new MySqlConnection(_connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@labcode", labCode);

                if (!string.IsNullOrEmpty(empNo))
                    cmd.Parameters.AddWithValue("@empno", $"%{empNo}%");
                if (!string.IsNullOrEmpty(empName))
                    cmd.Parameters.AddWithValue("@empname", $"%{empName}%");
                if (!string.IsNullOrEmpty(designation))
                    cmd.Parameters.AddWithValue("@designation", designation);
                if (!string.IsNullOrEmpty(status))
                    cmd.Parameters.AddWithValue("@status", status);

                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(MapEmployeeFromReaderForTable(reader));
                    }
                }
            }
            return list;
        }

        public void AddEmployee(EmpMastTest employee, string enteredIp)
        {
            const string sql = @"
    INSERT INTO empmast (
        empno, empname, labcode, gender, paylvl, designation,
        dob, dob_dt, doj, doj_dt, basicpay, category, 
        dop, dop_dt, dor, dor_dt, email,
        empgroup, grade, active, phy, checked, chkdtte, mobilenumber,
        entereddate, enteredip
    ) VALUES (
        @empno, @empname, @labcode, @gender, @paylvl, @designation,
        @dob, @dob_dt, @doj, @doj_dt, @basicpay, @category,
        @dop, @dop_dt, @dor, @dor_dt, @email,
        @empgroup, @grade, @active, @phy, @checked, @chkdtte, @mobilenumber,
        @entereddate, @enteredip
    )";


            try
            {
                using (var conn = new MySqlConnection(_connStr))
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    AddEmployeeParameters(cmd, employee, enteredIp);
                    conn.Open();

                    // Debug output
                    Debug.WriteLine("Executing SQL: " + sql);
                    foreach (MySqlParameter param in cmd.Parameters)
                    {
                        Debug.WriteLine($"{param.ParameterName}: {param.Value}");
                    }

                    int rowsAffected = cmd.ExecuteNonQuery();
                    Debug.WriteLine($"Rows affected: {rowsAffected}");
                }
            }
            catch (MySqlException ex)
            {
                Debug.WriteLine($"MySQL Error ({ex.Number}): {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public void UpdateEmployee(EmpMastTest employee)
        {
            const string sql = @"
    UPDATE empmast SET
        empname = @empname, labcode = @labcode, gender = @gender, 
        paylvl = @paylvl, designation = @designation,
        dob = @dob, dob_dt = @dob_dt, doj = @doj, doj_dt = @doj_dt,
        basicpay = @basicpay, category = @category, 
        dop = @dop, dop_dt = @dop_dt, dor = @dor, dor_dt = @dor_dt,
        email = @email, empgroup = @empgroup, grade = @grade,
        active = @active, phy = @phy, checked = @checked, 
        chkdtte = @chkdtte, mobilenumber = @mobilenumber
    WHERE empno = @empno";


            try
            {
                using (var conn = new MySqlConnection(_connStr))
                using (var cmd = new MySqlCommand(sql, conn))
                {
                    AddEmployeeParameters(cmd, employee, null);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error updating employee: {ex.Message}");
                throw;
            }
        }

        public void DeactivateEmployee(string empNo)
        {
            const string sql = @"UPDATE empmast SET active = 'N' WHERE empno = @empno";

            using (var conn = new MySqlConnection(_connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@empno", empNo);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        private EmpMastTest MapEmployeeFromReaderForView(MySqlDataReader reader)
        {
            return new EmpMastTest
            {
                EmpNo = reader["empno"].ToString(),
                EmpName = reader["empname"] as string,
                LabCode = Convert.ToInt32(reader["labcode"]),
                LabName = reader["labname"] as string,
                Gender = reader["gender"] as string,
                PayLvl = reader["paylvl"] as string,
                Designation = reader["designation"] as string,
                DesDesc = reader["desdesc"] as string,
                DOB = reader["dob"] as string,
                DOJ = reader["doj"] as string,
                BasicPay = reader["basicpay"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["basicpay"]),
                Category = reader["category"] as string,
                DOP = reader["dop"] as string,
                DOR = reader["dor"] as string,
                Email = reader["email"] as string,
                EmpGroup = reader["empgroup"] as string,
                Grade = reader["grade"] as string,
                Active = reader["active"] as string,
                Phy = reader["phy"] as string,
                Checked = reader["checked"] as string,
                ChkDtte = reader["chkdtte"] as string,
                MobileNumber = reader["mobilenumber"] as string,
                DOB_dt = reader["dob_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["dob_dt"]),
                DOJ_dt = reader["doj_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["doj_dt"]),
                DOP_dt = reader["dop_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["dop_dt"]),
                DOR_dt = reader["dor_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["dor_dt"]),
                //CatNew = reader["catnew"] as string,
                QtrNo = reader["qtrno"] as string,
                OccDate = reader["occdate"] as string,
                QtrType = reader["qtrtype"] as string,
                QtrStatus = reader["qtrstatus"] as string,



            };
        }

        private EmpMastTest MapEmployeeFromReader(MySqlDataReader reader)
        {
            return new EmpMastTest
            {
                EmpNo = reader["empno"].ToString(),
                EmpName = reader["empname"] as string,
                LabCode = Convert.ToInt32(reader["labcode"]),
                LabName = reader["labname"] as string,
                Gender = reader["gender"] as string,
                PayLvl = reader["paylvl"] as string,
                Designation = reader["designation"] as string,
                DesDesc = reader["desdesc"] as string,
                DOB = reader["dob"] as string,
                DOJ = reader["doj"] as string,
                BasicPay = reader["basicpay"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["basicpay"]),
                Category = reader["category"] as string,
                DOP = reader["dop"] as string,
                DOR = reader["dor"] as string,
                Email = reader["email"] as string,
                EmpGroup = reader["empgroup"] as string,
                Grade = reader["grade"] as string,
                Active = reader["active"] as string,
                Phy = reader["phy"] as string,
                Checked = reader["checked"] as string,
                ChkDtte = reader["chkdtte"] as string,
                MobileNumber = reader["mobilenumber"] as string,
                DOB_dt = reader["dob_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["dob_dt"]),
                DOJ_dt = reader["doj_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["doj_dt"]),
                DOP_dt = reader["dop_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["dop_dt"]),
                DOR_dt = reader["dor_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["dor_dt"]),
                //QtrStatus = reader["qtrstatus"] as string
                //CatNew = reader["catnew"] as string,


            };
        }

        private EmpMastTest MapEmployeeFromReaderForTable(MySqlDataReader reader)
        {
            return new EmpMastTest
            {
                EmpNo = reader["empno"].ToString(),
                EmpName = reader["empname"] as string,
                LabCode = Convert.ToInt32(reader["labcode"]),
                LabName = reader["labname"] as string,
                Gender = reader["gender"] as string,
                PayLvl = reader["paylvl"] as string,
                Designation = reader["designation"] as string,
                DesDesc = reader["desdesc"] as string,
                DOB = reader["dob"] as string,
                DOJ = reader["doj"] as string,
                BasicPay = reader["basicpay"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["basicpay"]),
                Category = reader["category"] as string,
                DOP = reader["dop"] as string,
                DOR = reader["dor"] as string,
                Email = reader["email"] as string,
                EmpGroup = reader["empgroup"] as string,
                Grade = reader["grade"] as string,
                Active = reader["active"] as string,
                Phy = reader["phy"] as string,
                Checked = reader["checked"] as string,
                ChkDtte = reader["chkdtte"] as string,
                MobileNumber = reader["mobilenumber"] as string,
                DOB_dt = reader["dob_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["dob_dt"]),
                DOJ_dt = reader["doj_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["doj_dt"]),
                DOP_dt = reader["dop_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["dop_dt"]),
                DOR_dt = reader["dor_dt"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(reader["dor_dt"]),
                QtrStatus = reader["qtrstatus"] as string
                //CatNew = reader["catnew"] as string,


            };
        }

        private void AddEmployeeParameters(MySqlCommand cmd, EmpMastTest e, string enteredIp)
        {
            // Basic parameters
            cmd.Parameters.AddWithValue("@empno", e.EmpNo);
            cmd.Parameters.AddWithValue("@empname", (object)e.EmpName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@labcode", e.LabCode);

            // Gender - ensure M or F
            cmd.Parameters.AddWithValue("@gender",
                (e.Gender?.ToUpper() == "M" || e.Gender?.ToUpper() == "F") ? e.Gender.ToUpper() : (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@paylvl", (object)e.PayLvl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@designation", (object)e.Designation ?? DBNull.Value);

            // String date columns
            cmd.Parameters.AddWithValue("@dob", (object)e.DOB ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@doj", (object)e.DOJ ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@dop", (object)e.DOP ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@dor", (object)e.DOR ?? DBNull.Value);

            // Date columns - use proper MySQL date format (yyyy-MM-dd)
            cmd.Parameters.AddWithValue("@dob_dt", e.DOB_dt.HasValue ?
                e.DOB_dt.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@doj_dt", e.DOJ_dt.HasValue ?
                e.DOJ_dt.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@dop_dt", e.DOP_dt.HasValue ?
                e.DOP_dt.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@dor_dt", e.DOR_dt.HasValue ?
                e.DOR_dt.Value.ToString("yyyy-MM-dd") : (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@basicpay", (object)e.BasicPay ?? DBNull.Value);
            //cmd.Parameters.AddWithValue("@category", (object)e.Category ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@email", (object)e.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@empgroup", (object)e.EmpGroup ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@grade", (object)e.Grade ?? DBNull.Value);

            // Active - ensure Y or N
            cmd.Parameters.AddWithValue("@active",
                (e.Active?.ToUpper() == "Y" || e.Active?.ToUpper() == "N") ? e.Active.ToUpper() : "N");

            // Phy (Handicapped) - ensure Y or N
            cmd.Parameters.AddWithValue("@phy",
                (e.Phy?.ToUpper() == "Y" || e.Phy?.ToUpper() == "N") ? e.Phy.ToUpper() : "N");

            cmd.Parameters.AddWithValue("@checked", (object)e.Checked ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@chkdtte", (object)e.ChkDtte ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@mobilenumber", (object)e.MobileNumber ?? DBNull.Value);

            if (enteredIp != null)
            {
                cmd.Parameters.AddWithValue("@entereddate", DateTime.UtcNow.ToString("yyyy-MM-dd"));
                cmd.Parameters.AddWithValue("@enteredip", enteredIp);
            }

            string newCat = e.Category;
            if (!string.IsNullOrEmpty(e.Phy) && e.Phy.ToUpper() == "Y" && !string.IsNullOrEmpty(e.Category))
            {
                newCat = e.Category + "-PwD";
            }

            cmd.Parameters.AddWithValue("@category", (object)e.Category ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@newcat", (object)newCat ?? DBNull.Value);

        }

        public void Dispose()
        {
            // Cleanup if needed
        }
    }
}