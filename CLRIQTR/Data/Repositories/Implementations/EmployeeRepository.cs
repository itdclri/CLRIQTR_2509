using CLRIQTR.Data.Repositories.Interfaces;
using CLRIQTR.Models;
using MySql.Data.MySqlClient;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using Dapper;

namespace CLRIQTR.Data.Repositories.Implementations
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly string _connStr = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

        public EmpMastTest GetEmployeeByEmpNo(string empNo)
        {
            const string sql = @"SELECT * FROM empmast e 
                               left JOIN desmast d ON e.designation = d.desid 
                                left JOIN labmast l ON e.labcode = l.labcode  
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
                         left JOIN desmast d ON e.Designation = d.DesId
                         left JOIN labmast l ON e.LabCode = l.LabCode
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
               left JOIN desmast d ON e.designation = d.desid 
               INNER JOIN labmast l ON e.labcode = l.labcode
               LEFT JOIN QTRUPD q ON e.empno=q.empno
               WHERE e.labcode = @labcode";

        

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
                //DesDesc = reader["desdesc"] as string,
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
            
            cmd.Parameters.AddWithValue("@empno", e.EmpNo);
            cmd.Parameters.AddWithValue("@empname", (object)e.EmpName ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@labcode", e.LabCode);

            
            cmd.Parameters.AddWithValue("@gender",
                (e.Gender?.ToUpper() == "M" || e.Gender?.ToUpper() == "F") ? e.Gender.ToUpper() : (object)DBNull.Value);

            cmd.Parameters.AddWithValue("@paylvl", (object)e.PayLvl ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@designation", (object)e.Designation ?? DBNull.Value);

            
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
            cmd.Parameters.AddWithValue("@email", (object)e.Email ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@empgroup", (object)e.EmpGroup ?? DBNull.Value);
            cmd.Parameters.AddWithValue("@grade", (object)e.Grade ?? DBNull.Value);

          
            cmd.Parameters.AddWithValue("@active",
                (e.Active?.ToUpper() == "Y" || e.Active?.ToUpper() == "N") ? e.Active.ToUpper() : "N");

            
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
           
        }


        public EmpDepDtls GetFamilyDetailsByEmpNo(string empNo)
        {
            EmpDepDtls details = null;
            const string sql = "SELECT * FROM empdepdtls WHERE empno = @empno";

            using (var conn = new MySqlConnection(_connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@empno", empNo);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        details = new EmpDepDtls
                        {
                            EmpNo = reader["empno"].ToString(),
                            FatherName = reader["f1"] != DBNull.Value ? reader["f1"].ToString() : null,
                            MotherName = reader["m1"] != DBNull.Value ? reader["m1"].ToString() : null,
                            WifeName = reader["w1"] != DBNull.Value ? reader["w1"].ToString() : null,
                            HusbandName = reader["h1"] != DBNull.Value ? reader["h1"].ToString() : null,
                            Son1 = reader["s1"] != DBNull.Value ? reader["s1"].ToString() : null,
                            Son2 = reader["s2"] != DBNull.Value ? reader["s2"].ToString() : null,
                            Son3 = reader["s3"] != DBNull.Value ? reader["s3"].ToString() : null,
                            Daughter1 = reader["d1"] != DBNull.Value ? reader["d1"].ToString() : null,
                            Daughter2 = reader["d2"] != DBNull.Value ? reader["d2"].ToString() : null,
                            Daughter3 = reader["d3"] != DBNull.Value ? reader["d3"].ToString() : null,
                        };
                    }
                }
            }
            return details;
        }

        public void UpsertFamilyDetails(EmpDepDtls details)
        {
            
            const string sql = @"
INSERT INTO empdepdtls (empno, f1, m1, w1, h1, s1, s2, s3, d1, d2, d3,depslno,deprel,depage,depname)
VALUES (@empno, @f1, @m1, @w1, @h1, @s1, @s2, @s3, @d1, @d2, @d3, 1, 'family', 0, '')
ON DUPLICATE KEY UPDATE
    f1 = VALUES(f1), m1 = VALUES(m1), w1 = VALUES(w1), h1 = VALUES(h1),
    s1 = VALUES(s1), s2 = VALUES(s2), s3 = VALUES(s3),
    d1 = VALUES(d1), d2 = VALUES(d2), d3 = VALUES(d3);";

            using (var conn = new MySqlConnection(_connStr))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                cmd.Parameters.AddWithValue("@empno", details.EmpNo);
                cmd.Parameters.AddWithValue("@f1", (object)details.FatherName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@m1", (object)details.MotherName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@w1", (object)details.WifeName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@h1", (object)details.HusbandName ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@s1", (object)details.Son1 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@s2", (object)details.Son2 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@s3", (object)details.Son3 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@d1", (object)details.Daughter1 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@d2", (object)details.Daughter2 ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@d3", (object)details.Daughter3 ?? DBNull.Value);

                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }


        public IEnumerable<DependentTypeDto> GetAllDependentTypes()
        {
            string sql = "SELECT depid, depdesc FROM empdepmast ORDER BY depdesc;";

            using (var connection = new MySqlConnection(_connStr))
            {
                var dbData = connection.Query<EmpDepMast>(sql).ToList();

                return dbData.Select(d => new DependentTypeDto
                {
                    Id = d.depid,
                    TypeName = d.depdesc
                }).ToList();
            }
        }

        public void AddDependent(EmpDependentDetail dependent)
        {
            string sql = "INSERT INTO empdep (empno, depid, depname) VALUES (@EmpNo, @DepId, @DepName);";
            Debug.WriteLine("Family");

            using (var connection = new MySqlConnection(_connStr))
            {
                connection.Execute(sql, dependent);
            }
        }

       
        public void UpdateDependents(string empNo, List<DependentInputModel> dependents)
        {
            using (var connection = new MySqlConnection(_connStr))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        string deleteSql = "DELETE FROM empdep WHERE empno = @EmpNo;";
                        connection.Execute(deleteSql, new { EmpNo = empNo }, transaction);

                        if (dependents != null && dependents.Any())
                        {
                            string insertSql = "INSERT INTO empdep (empno, depid, depname) VALUES (@EmpNo, @DependentTypeId, @Name);";

                            foreach (var dependent in dependents)
                            {
                                if (dependent.DependentTypeId > 0 && !string.IsNullOrWhiteSpace(dependent.Name))
                                {
                                    connection.Execute(insertSql, new
                                    {
                                        EmpNo = empNo, 
                                        dependent.DependentTypeId,
                                        dependent.Name
                                    }, transaction);
                                }
                            }
                        }

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw; 
                    }
                }
            }
        }

       

        public List<DependentInputModel> GetDependentsByEmpNo(string empNo)
        {
            string sql = @"SELECT 
                       Id, 
                       depid AS DependentTypeId, 
                       depname AS Name 
                   FROM empdep 
                   WHERE empno = @EmpNo;";

            using (var connection = new MySqlConnection(_connStr))
            {
                return connection.Query<DependentInputModel>(sql, new { EmpNo = empNo }).ToList();
            }
        }

        public DependentInputModel AddDependent(DependentInputModel dependent)
        {
            string sql = @"INSERT INTO empdep (empno, depid, depname) 
                   VALUES (@EmpNo, @DependentTypeId, @Name);
                   SELECT LAST_INSERT_ID();"; // MySQL specific: get the new ID
            using (var connection = new MySqlConnection(_connStr))
            {
                var newId = connection.QuerySingle<int>(sql, dependent);
                dependent.Id = newId;
                return dependent;
            }
        }

        public void UpdateDependent(DependentInputModel dependent)
        {
            string sql = @"UPDATE empdep SET depid = @DependentTypeId, depname = @Name 
                   WHERE Id = @Id;";
            using (var connection = new MySqlConnection(_connStr))
            {
                connection.Execute(sql, dependent);
            }
        }

        public void DeleteDependent(int id)
        {
            string sql = "DELETE FROM empdep WHERE Id = @Id;";
            using (var connection = new MySqlConnection(_connStr))
            {
                connection.Execute(sql, new { Id = id });
            }
        }

        public IEnumerable<CompletedApplicationViewModel> GetAllCompletedApplications()
        {
            var sql = @"
        -- Part 1: Get completed apps from 'eqtrapply' and join matching 'sa' apps.
        SELECT 
            CONCAT(
                eq.qtrappno, 
                IF(sa.saqtrappno IS NOT NULL, ' & ', ''), 
                IFNULL(sa.saqtrappno, '')
            ) AS qtrappno,
            eq.empno,
            e.empname,
            d.desdesc,
            -- Convert the string to a date here
            STR_TO_DATE(eq.doa, '%d/%m/%Y') AS doa, 
            'Completed' AS appstatus
        FROM 
            eqtrapply eq
        INNER JOIN empmast e ON e.empno = eq.empno
        LEFT JOIN desmast d ON d.desid = e.designation
        LEFT JOIN saeqtrapply sa ON sa.empno = eq.empno AND sa.appstatus = 'C'
        WHERE eq.appstatus = 'C'

        UNION

        -- Part 2: Get completed apps from 'saeqtrapply' that DO NOT exist in 'eqtrapply'.
        SELECT 
            sa.saqtrappno AS qtrappno,
            sa.empno,
            e.empname,
            d.desdesc,
            -- Also convert the string to a date here
            STR_TO_DATE(sa.doa, '%d/%m/%Y') AS doa,
            'Completed' AS appstatus
        FROM 
            saeqtrapply sa
        INNER JOIN empmast e ON e.empno = sa.empno
        LEFT JOIN desmast d ON d.desid = e.designation
        LEFT JOIN eqtrapply eq ON eq.empno = sa.empno
        WHERE sa.appstatus = 'C' AND eq.empno IS NULL;";

            using (var connection = new MySqlConnection(_connStr))
            {
                return connection.Query<CompletedApplicationViewModel>(sql).ToList();
            }
        }


    }
}