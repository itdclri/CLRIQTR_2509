using CLRIQTR.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Linq;

namespace CLRIQTR.Repositories
{
    public class AdminRepository
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;

        public TentativeReportViewModel GetAllTentativeData()
        {
            return new TentativeReportViewModel
            {
                TypeI = GetTypeIQuarterData(),
                TypeII = GetTypeIIQuarterData(),
                TypeIISC = GetTypeIISCQuarterData(),
                TypeIII = GetTypeIIIQuarterData(),
                TypeIIISC = GetTypeIIISCQuarterData(),
                TypeIIIST = GetTypeIIISTQuarterData(),
                TypeIV = GetTypeIVQuarterData(),
                TypeIVSC = GetTypeIVSCQuarterData(),
                TypeIVST = GetTypeIVSTQuarterData(),
                TypeV = GetTypeVQuarterData(),
                TypeSA = GetTypeSAQuarterData()
            };
        }

        public List<TentativeReportModel> GetTypeIQuarterData()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT a.qtrappno, a.empno, 
                       TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) as empname,
                       b.designation, b.doj, b.category, a.labcode, a.toe, a.ownhouse  
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
                WHERE a.appstatus = 'C' AND a.toe = 'I' 
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = GetDesignationDescription(reader["designation"].ToString()),
                            LabInstitute = GetLabName(reader["labcode"].ToString()),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = reader["category"].ToString(),
                            QuarterType = "I",

                            OwnHouse = reader["ownhouse"].ToString()
                        });
                    }
                }
            }
            return results;
        }

        public List<TentativeReportModel> GetTypeIIQuarterData()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT a.qtrappno, a.empno, 
                       TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) as empname,
                       b.designation, b.doj, b.category, a.labcode, a.toe, a.ownhouse, b.dob  ,r.remarks
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
                left join qtradminremarks r on r.empno=a.empno
                WHERE a.appstatus = 'C' AND a.toe = 'II' 
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y'), STR_TO_DATE(b.dob, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var category = reader["category"].ToString();
                        var dob = reader["dob"].ToString();
                        var oh = reader["ownhouse"].ToString();
                        var remarks = reader["remarks"].ToString();


                        if (category == "OBC" || category == "General" || category == "Gen" || category == "GEN" || category == "EWS")
                        {
                            category = "";
                        }

                        //if (empno == "11066")
                        //{
                        //    category = "Rule 10.1 applies";
                        //}

                        //var allowedEmpNos = new[] { "11061", "11060", "11065", "11066", "11068", "11069", "11070", "11072", "11073", "11085", "11088", "11091", "11092", "11097", "11098" };
                        //if (!allowedEmpNos.Contains(empno))
                        //{
                        //    dob = "";
                        //}

                        category += " " + remarks;

                        if (oh == "O")
                        {
                            category += "*";
                        }

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = GetDesignationDescription(reader["designation"].ToString()),
                            LabInstitute = GetLabName(reader["labcode"].ToString()),
                            DateOfJoining = reader["doj"].ToString(),
                            DateOfBirth = dob,
                            Remarks = category,
                            QuarterType = "II",
                            OwnHouse = oh
                        });
                    }
                }
            }
            return results;
        }

        public List<TentativeReportModel> GetTypeIISCQuarterData()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT a.qtrappno, a.empno, 
                       TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) as empname,
                       b.designation, b.doj, b.category, a.labcode, a.toe, a.ownhouse  ,r.remarks
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
                left join qtradminremarks r on r.empno=a.empno
                WHERE a.appstatus = 'C' AND a.toe = 'II' AND b.category = 'SC'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var category = reader["category"].ToString();
                        var oh = reader["ownhouse"].ToString();
                        var remarks = reader["remarks"].ToString();

                        category += " " + remarks;

                        if (oh == "O")
                        {
                            category += "*";
                        }

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = GetDesignationDescription(reader["designation"].ToString()),
                            LabInstitute = GetLabName(reader["labcode"].ToString()),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = category,
                            QuarterType = "II SC",
                            OwnHouse = oh
                        });
                    }
                }
            }
            return results;
        }

        public List<TentativeReportModel> GetTypeIIIQuarterData()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT a.qtrappno, a.empno, 
                       TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) as empname,
                       b.designation, b.doj, b.category, a.labcode, a.toe, b.dob, a.ownhouse  ,r.remarks
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
                left join qtradminremarks r on r.empno=a.empno
                WHERE a.appstatus = 'C' AND a.toe = 'III' AND a.eqtrtypesel <> 'NA'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y'), STR_TO_DATE(b.dob, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var category = reader["category"].ToString();
                        var dob = reader["dob"].ToString();
                        var oh = reader["ownhouse"].ToString();
                        var remarks = reader["remarks"].ToString();


                        if (category == "OBC" || category == "General" || category == "Gen" || category == "GEN" || category == "EWS")
                        {
                            category = "";
                        }

                        category += " " + remarks;

                        //if (empno == "10569")
                        //{
                        //    category += "*";
                        //}

                        //if (empno == "10998" || empno == "10996")
                        //{
                        //    // Keep dob as is
                        //}
                        //else
                        //{
                        //    dob = "";
                        //}

                        //if (empno == "2060025" || empno == "10962" || empno == "15025")
                        //{
                        //    category += "\n(Rule 10.1 applies)";
                        //}

                        if (oh == "O")
                        {
                            category += "*";
                        }

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = GetDesignationDescription(reader["designation"].ToString()),
                            LabInstitute = GetLabName(reader["labcode"].ToString()),
                            DateOfJoining = reader["doj"].ToString(),
                            DateOfBirth = dob,
                            Remarks = category,
                            QuarterType = "III",
                            OwnHouse = oh
                        });
                    }
                }
            }
            return results;
        }

        public List<TentativeReportModel> GetTypeIIISCQuarterData()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT a.qtrappno, a.empno, 
                       TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) as empname,
                       b.designation, b.doj, b.category, a.labcode, a.toe, a.ownhouse  ,r.remarks
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
 left join qtradminremarks r on r.empno=a.empno
                WHERE a.appstatus = 'C' AND a.toe = 'III' AND a.eqtrtypesel <> 'NA' AND b.category = 'SC'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var category = reader["category"].ToString();
                        var oh = reader["ownhouse"].ToString();
                        var remarks = reader["remarks"].ToString();
                        //if (empno == "2060025")
                        //{
                        //    category += "\n(Rule 10.1 applies)";
                        //}

                        //if (empno == "10569")
                        //{
                        //    category += "*";
                        //}
                        category += " " + remarks;
                        if (oh == "O")
                        {
                            category += "*";
                        }

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = GetDesignationDescription(reader["designation"].ToString()),
                            LabInstitute = GetLabName(reader["labcode"].ToString()),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = category,
                            QuarterType = "III SC",
                            OwnHouse = oh
                        });
                    }
                }
            }
            return results;
        }

        public List<TentativeReportModel> GetTypeIIISTQuarterData()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT a.qtrappno, a.empno, 
                       TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) as empname,
                       b.designation, b.doj, b.category, b.labcode, a.toe, a.ownhouse  ,r.remarks
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
 left join qtradminremarks r on r.empno=a.empno
                WHERE a.appstatus = 'C' AND b.category = 'ST' AND a.eqtrtypesel <> 'NA' AND a.toe = 'III'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var category = reader["category"].ToString();
                        var oh = reader["ownhouse"].ToString();
                        var remarks = reader["remarks"].ToString();
                        //if (empno == "2060025")
                        //{
                        //    category += "\n(Rule 10.1 applies)";
                        //}
                        category += " " + remarks;
                        if (oh == "O")
                        {
                            category += "*";
                        }

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = GetDesignationDescription(reader["designation"].ToString()),
                            LabInstitute = GetLabName(reader["labcode"].ToString()),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = category,
                            QuarterType = "III ST",
                            OwnHouse = oh
                        });
                    }
                }
            }
            return results;
        }

        public List<TentativeReportModel> GetTypeIVQuarterData()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT a.qtrappno, a.empno, 
                       TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) as empname,
                       b.designation, b.doj, b.category, b.labcode, a.toe, a.lowertypesel, a.ownhouse  ,r.remarks
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
 left join qtradminremarks r on r.empno=a.empno
                WHERE a.appstatus = 'C' AND 
                      ((a.toe = 'IV' AND a.lowertypesel = 'IV') OR 
                       (a.toe = 'IV' AND a.lowertypesel = 'NA') OR 
                       (a.toe = 'V' AND a.lowertypesel = 'I'))
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var category = reader["category"].ToString();
                        var oh = reader["ownhouse"].ToString();
                        var remarks = reader["remarks"].ToString();
                        if (category == "OBC" || category == "General" || category == "Gen" || category == "GEN" || category == "EWS")
                        {
                            category = "";
                        }
                        category += " " + remarks;
                        //if (empno == "10926")
                        //{
                        //    category += "\n(Rule 10.1 applies)";
                        //}

                        if (oh == "O")
                        {
                            category += "*";
                        }

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = GetDesignationDescription(reader["designation"].ToString()),
                            LabInstitute = GetLabName(reader["labcode"].ToString()),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = category,
                            QuarterType = "IV",
                            OwnHouse = oh
                        });
                    }
                }
            }
            return results;
        }

        public List<TentativeReportModel> GetTypeIVSCQuarterData()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT a.qtrappno, a.empno, 
                       TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) as empname,
                       b.designation, b.doj, b.category, b.labcode, a.toe, a.lowertypesel, a.ownhouse  ,r.remarks
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
 left join qtradminremarks r on r.empno=a.empno
                WHERE a.appstatus = 'C' AND 
                      ((a.toe = 'IV' AND a.lowertypesel IN ('I','NA','IV')) OR 
                       (a.toe = 'V' AND a.lowertypesel IN ('I','NA','IV'))) 
                      AND b.category = 'SC'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var category = reader["category"].ToString();
                        var oh = reader["ownhouse"].ToString();
                        var remarks = reader["remarks"].ToString();
                        //if (empno == "10887")
                        //{
                        //    category += "\n(Rule 10.1 applies)";
                        //}
                        category += " " + remarks;
                        if (oh == "O")
                        {
                            category += "*";
                        }

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = GetDesignationDescription(reader["designation"].ToString()),
                            LabInstitute = GetLabName(reader["labcode"].ToString()),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = category,
                            QuarterType = "IV SC",
                            OwnHouse = oh
                        });
                    }
                }
            }
            return results;
        }

        public List<TentativeReportModel> GetTypeIVSTQuarterData()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT a.qtrappno, a.empno, 
                       TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) as empname,
                       b.designation, b.doj, b.category, a.labcode, a.toe, a.ownhouse  ,r.remarks
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno  left join qtradminremarks r on r.empno=a.empno
                WHERE a.appstatus = 'C' AND a.toe = 'IV' AND a.lowertypesel IN ('I','NA','IV') AND b.category = 'ST'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var category = reader["category"].ToString();
                        var oh = reader["ownhouse"].ToString();
                        var remarks = reader["remarks"].ToString();
                        if (oh == "O")
                        {
                            category += "*";
                        }
                        category += " " + remarks;
                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = GetDesignationDescription(reader["designation"].ToString()),
                            LabInstitute = GetLabName(reader["labcode"].ToString()),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = category,
                            QuarterType = "IV ST",
                            OwnHouse = oh
                        });
                    }
                }
            }
            return results;
        }

        public List<TentativeReportModel> GetTypeVQuarterData()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT a.qtrappno, a.empno, b.empname, b.designation, b.dop, b.category, b.labcode, a.toe, b.paylvl, a.ownhouse  ,r.remarks
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno  left join qtradminremarks r on r.empno=a.empno
                WHERE a.appstatus = 'C' AND a.toe = 'V' AND a.lowertypesel IN ('NA','V','I','NI')
                ORDER BY b.paylvl DESC, STR_TO_DATE(b.dop, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var oh = reader["ownhouse"].ToString();
                        var category = "";

                        if (oh == "O")
                        {
                            category = "*";
                        }

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString().Contains(".") ?
                                   reader["empname"].ToString().Substring(reader["empname"].ToString().IndexOf(".") + 1).Trim() :
                                   reader["empname"].ToString(),
                            Designation = GetDesignationDescription(reader["designation"].ToString()),
                            LabInstitute = GetLabName(reader["labcode"].ToString()),
                            PayLevel = reader["paylvl"].ToString(),
                            PriorityDate = reader["dop"].ToString(),
                            Remarks = category,
                            QuarterType = "V",
                            OwnHouse = oh
                        });
                    }
                }
            }
            return results;
        }

        public List<TentativeReportModel> GetTypeSAQuarterData()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT a.saqtrappno, a.empno, 
                       TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) as empname,
                       b.designation, b.doj, b.category, a.labcode, a.toe, a.ownhouse  
                FROM saeqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
                JOIN saorder c ON c.des = b.designation
                WHERE a.appstatus = 'C'
                ORDER BY c.orderit, STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var category = reader["category"].ToString();
                        var oh = reader["ownhouse"].ToString();

                        if (category == "OBC" || category == "General" || category == "SC" || category == "ST" || category == "Gen" || category == "GEN" || category == "EWS")
                        {
                            category = "";
                        }

                        if (oh == "O")
                        {
                            category += "*";
                        }

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = GetDesignationDescription(reader["designation"].ToString()),
                            LabInstitute = GetLabName(reader["labcode"].ToString()),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = category,
                            QuarterType = "SA",
                            OwnHouse = oh
                        });
                    }
                }
            }
            return results;
        }

        private string GetLabName(string labCode)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT labname FROM labmast WHERE labcode = @labCode";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@labCode", labCode);

                conn.Open();
                return cmd.ExecuteScalar()?.ToString() ?? string.Empty;
            }
        }

        private string GetDesignationDescription(string desId)
        {
            using (var conn = new MySqlConnection(connectionString))
            {
                string query = "SELECT desdesc FROM desmast WHERE desid = @desId";
                var cmd = new MySqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@desId", desId);

                conn.Open();
                return cmd.ExecuteScalar()?.ToString() ?? string.Empty;
            }
        }


        // Main method to aggregate all data for the Final Report PDF.
        public TentativeReportViewModel GetAllFinalData()
        {
            return new TentativeReportViewModel
            {
                TypeI = GetTypeIQuarterDataFinal(),
                TypeII = GetTypeIIQuarterDataFinal(),
                TypeIISC = GetTypeIISCQuarterDataFinal(),
                TypeIII = GetTypeIIIQuarterDataFinal(),
                TypeIIISC = GetTypeIIISCQuarterDataFinal(),
                TypeIIIST = GetTypeIIISTQuarterDataFinal(),
                TypeIV = GetTypeIVQuarterDataFinal(),
                TypeIVSC = GetTypeIVSCQuarterDataFinal(),
                TypeIVST = GetTypeIVSTQuarterDataFinal(),
                TypeV = GetTypeVQuarterDataFinal(),
                TypeSA = GetTypeSAQuarterDataFinal()
            };
        }




        private List<TentativeReportModel> GetTypeIQuarterDataFinal()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT 
                    a.empno, TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) AS empname, b.doj, b.category, a.ownhouse,
                    l.labname, d.desdesc
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
                LEFT JOIN labmast l ON a.labcode = l.labcode
                LEFT JOIN desmast d ON b.designation = d.desid
                WHERE a.appstatus = 'C' AND a.toe = 'I' 
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = reader["desdesc"].ToString(),
                            LabInstitute = reader["labname"].ToString(),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = reader["category"].ToString()
                        });
                    }
                }
            }
            return results;
        }

        private List<TentativeReportModel> GetTypeIIQuarterDataFinal()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT 
                    a.empno, TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) AS empname, b.doj, b.category, a.ownhouse,
                    l.labname, d.desdesc ,r.remarks
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
                LEFT JOIN labmast l ON a.labcode = l.labcode
                LEFT JOIN desmast d ON b.designation = d.desid  left join qtradminremarks r on r.empno=a.empno
                WHERE a.appstatus = 'C' AND a.toe = 'II' 
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var remarks = reader["category"].ToString();
                        var remarks2 = reader["remarks"].ToString();
                        // Java Logic: Clear non-reserved categories
                        var generalCategories = new[] { "OBC", "General", "Gen", "GEN", "EWS" };
                        if (generalCategories.Contains(remarks))
                        {
                            remarks = "";
                        }
                        remarks2 += " " + remarks2; 
                        //// Java Logic: Special rule for empno 11029
                        //if (empno == "11029")
                        //{
                        //    remarks = "Rule 10.1 applies";
                        //}

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = reader["desdesc"].ToString(),
                            LabInstitute = reader["labname"].ToString(),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = remarks
                        });
                    }
                }
            }
            return results;
        }

        private List<TentativeReportModel> GetTypeIISCQuarterDataFinal()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT 
                    a.empno, TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) AS empname, b.doj, b.category, a.ownhouse,
                    l.labname, d.desdesc ,r.remarks
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
                LEFT JOIN labmast l ON a.labcode = l.labcode
                LEFT JOIN desmast d ON b.designation = d.desid left join qtradminremarks r on r.empno=a.empno
                WHERE a.appstatus = 'C' AND a.toe = 'II' AND b.category = 'SC'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var remarks = reader["category"].ToString();
                        var remarks2 = reader["remarks"].ToString();
                        remarks2 += " " + remarks2;

                        //// Java Logic: Special rule for empno 10707
                        //if (empno == "10707")
                        //{
                        //    remarks += "\n(Rule 10.1 applies)";
                        //}

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = reader["desdesc"].ToString(),
                            LabInstitute = reader["labname"].ToString(),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = remarks
                        });
                    }
                }
            }
            return results;
        }

        private List<TentativeReportModel> GetTypeIIIQuarterDataFinal()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT 
                    a.empno, TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) AS empname, b.doj, b.category, b.dob, a.ownhouse,
                    l.labname, d.desdesc
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
                LEFT JOIN labmast l ON a.labcode = l.labcode
                LEFT JOIN desmast d ON b.designation = d.desid
                WHERE a.appstatus = 'C' AND a.toe = 'III' AND a.eqtrtypesel <> 'NA'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y'), STR_TO_DATE(b.dob, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var remarks = reader["category"].ToString();
                        var dob = reader["dob"].ToString();
                        var ownHouse = reader["ownhouse"].ToString();

                        // Java Logic: Clear non-reserved categories
                        var generalCategories = new[] { "OBC", "General", "Gen", "GEN", "EWS" };
                        if (generalCategories.Contains(remarks))
                        {
                            remarks = "";
                        }

                        // Java Logic: Handle DOB visibility
                        //if (empno != "10998" && empno != "10996")
                        //{
                        //    dob = "";
                        //}

                        //// Java Logic: Add special remarks
                        //if (empno == "10569") remarks += "*";
                        //if (empno == "10707") remarks += "\n(Rule 10.1 applies)";
                        //if (ownHouse == "O") remarks += "*";


                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = reader["desdesc"].ToString(),
                            LabInstitute = reader["labname"].ToString(),
                            DateOfJoining = reader["doj"].ToString(),
                            DateOfBirth = dob,
                            Remarks = remarks.Trim()
                        });
                    }
                }
            }
            return results;
        }

        private List<TentativeReportModel> GetTypeIIISCQuarterDataFinal()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT
                    a.empno, TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) AS empname, b.doj, b.category, a.ownhouse,
                    l.labname, d.desdesc
                FROM eqtrapply a
                JOIN empmast b ON a.empno = b.empno
                LEFT JOIN labmast l ON a.labcode = l.labcode
                LEFT JOIN desmast d ON b.designation = d.desid
                WHERE a.appstatus = 'C' AND a.toe = 'III' AND a.eqtrtypesel <> 'NA' AND b.category = 'SC'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var remarks = reader["category"].ToString();
                        var ownHouse = reader["ownhouse"].ToString();

                        if (empno == "10707") remarks += "\n(Rule 10.1 applies)";
                        if (empno == "10569") remarks += "*";
                        if (ownHouse == "O") remarks += "*";

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = reader["desdesc"].ToString(),
                            LabInstitute = reader["labname"].ToString(),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = remarks.Trim()
                        });
                    }
                }
            }
            return results;
        }

        private List<TentativeReportModel> GetTypeIIISTQuarterDataFinal()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT
                    a.empno, TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) AS empname, b.doj, b.category, a.ownhouse,
                    l.labname, d.desdesc
                FROM eqtrapply a
                JOIN empmast b ON a.empno = b.empno
                LEFT JOIN labmast l ON a.labcode = l.labcode
                LEFT JOIN desmast d ON b.designation = d.desid
                WHERE a.appstatus = 'C' AND b.category = 'ST' AND a.eqtrtypesel <> 'NA' AND a.toe = 'III'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var remarks = reader["category"].ToString();
                        var ownHouse = reader["ownhouse"].ToString();

                        if (empno == "10707") remarks += "\n(Rule 10.1 applies)";
                        if (ownHouse == "O") remarks += "*";

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = reader["desdesc"].ToString(),
                            LabInstitute = reader["labname"].ToString(),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = remarks.Trim()
                        });
                    }
                }
            }
            return results;
        }

        private List<TentativeReportModel> GetTypeIVQuarterDataFinal()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT 
                    a.empno, TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) AS empname, b.doj, b.category, a.ownhouse,
                    l.labname, d.desdesc
                FROM eqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
                LEFT JOIN labmast l ON b.labcode = l.labcode
                LEFT JOIN desmast d ON b.designation = d.desid
                WHERE a.appstatus = 'C' AND 
                      ((a.toe = 'IV' AND a.lowertypesel = 'IV') OR 
                       (a.toe = 'IV' AND a.lowertypesel = 'NA') OR 
                       (a.toe = 'V' AND a.lowertypesel = 'I'))
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var remarks = reader["category"].ToString();
                        var ownHouse = reader["ownhouse"].ToString();

                        var generalCategories = new[] { "OBC", "General", "Gen", "GEN", "EWS" };
                        if (generalCategories.Contains(remarks))
                        {
                            remarks = "";
                        }

                        if (empno == "10887") remarks += "\n(Rule 10.1 applies)";
                        if (empno == "5056") remarks = "Rule 10.1 applies";
                        if (ownHouse == "O") remarks += "*";

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = reader["desdesc"].ToString(),
                            LabInstitute = reader["labname"].ToString(),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = remarks.Trim()
                        });
                    }
                }
            }
            return results;
        }

        private List<TentativeReportModel> GetTypeIVSCQuarterDataFinal()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT
                    a.empno, TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) AS empname, b.doj, b.category, a.ownhouse,
                    l.labname, d.desdesc
                FROM eqtrapply a
                JOIN empmast b ON a.empno = b.empno
                LEFT JOIN labmast l ON b.labcode = l.labcode
                LEFT JOIN desmast d ON b.designation = d.desid
                WHERE a.appstatus = 'C' 
                  AND a.toe IN ('IV', 'V') 
                  AND a.lowertypesel IN ('I', 'NA', 'IV') 
                  AND b.category = 'SC'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var empno = reader["empno"].ToString();
                        var remarks = reader["category"].ToString();
                        var ownHouse = reader["ownhouse"].ToString();

                        if (empno == "10887") remarks += "\n(Rule 10.1 applies)";
                        if (ownHouse == "O") remarks += "*";

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = reader["desdesc"].ToString(),
                            LabInstitute = reader["labname"].ToString(),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = remarks.Trim()
                        });
                    }
                }
            }
            return results;
        }

        private List<TentativeReportModel> GetTypeIVSTQuarterDataFinal()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT
                    a.empno, TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) AS empname, b.doj, b.category, a.ownhouse,
                    l.labname, d.desdesc
                FROM eqtrapply a
                JOIN empmast b ON a.empno = b.empno
                LEFT JOIN labmast l ON a.labcode = l.labcode
                LEFT JOIN desmast d ON b.designation = d.desid
                WHERE a.appstatus = 'C' 
                  AND a.toe IN ('IV', 'V') 
                  AND a.lowertypesel IN ('I', 'NA', 'IV') 
                  AND b.category = 'ST'
                ORDER BY STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var remarks = reader["category"].ToString();
                        var ownHouse = reader["ownhouse"].ToString();
                        if (ownHouse == "O") remarks += "*";

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = reader["desdesc"].ToString(),
                            LabInstitute = reader["labname"].ToString(),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = remarks.Trim()
                        });
                    }
                }
            }
            return results;
        }

        private List<TentativeReportModel> GetTypeVQuarterDataFinal()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT
                    b.empname, b.dop, b.paylvl, a.ownhouse,
                    l.labname, d.desdesc
                FROM eqtrapply a
                JOIN empmast b ON a.empno = b.empno
                LEFT JOIN labmast l ON b.labcode = l.labcode
                LEFT JOIN desmast d ON b.designation = d.desid
                WHERE a.appstatus = 'C' AND a.toe = 'V' AND a.lowertypesel IN ('NA','V','I','NI')
                ORDER BY b.paylvl DESC, STR_TO_DATE(b.dop, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var ownHouse = reader["ownhouse"].ToString();
                        var remarks = ownHouse == "O" ? "*" : "";

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = reader["desdesc"].ToString(),
                            LabInstitute = reader["labname"].ToString(),
                            PayLevel = reader["paylvl"].ToString(),
                            PriorityDate = reader["dop"].ToString(),
                            Remarks = remarks
                        });
                    }
                }
            }
            return results;
        }

        private List<TentativeReportModel> GetTypeSAQuarterDataFinal()
        {
            var results = new List<TentativeReportModel>();
            var query = @"
                SELECT
                    TRIM(SUBSTR(b.empname, INSTR(b.empname, '.') + 1)) AS empname, b.doj, a.ownhouse, b.category,
                    l.labname, d.desdesc
                FROM saeqtrapply a 
                JOIN empmast b ON a.empno = b.empno 
                JOIN saorder c ON c.des = b.designation
                LEFT JOIN labmast l ON a.labcode = l.labcode
                LEFT JOIN desmast d ON b.designation = d.desid
                WHERE a.appstatus = 'C'
                ORDER BY c.orderit, STR_TO_DATE(b.doj, '%d-%m-%Y')";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    int sNo = 1;
                    while (reader.Read())
                    {
                        var ownHouse = reader["ownhouse"].ToString();
                        var remarks = ownHouse == "O" ? "*" : "";

                        // In SA, all categories are cleared for the remarks column

                        results.Add(new TentativeReportModel
                        {
                            SNo = sNo++,
                            Name = reader["empname"].ToString(),
                            Designation = reader["desdesc"].ToString(),
                            LabInstitute = reader["labname"].ToString(),
                            DateOfJoining = reader["doj"].ToString(),
                            Remarks = remarks
                        });
                    }
                }
            }
            return results;
        }




        public List<AdminLogin> GetEmployee(string empNo = null, string empName = null)
        {
            var list = new List<AdminLogin>();
            var sql = @"SELECT e.empno, e.empname, e.category , r.remarks FROM empmast e 
                INNER JOIN desmast d ON e.designation = d.desid 
                INNER JOIN labmast l ON e.labcode = l.labcode
                LEFT JOIN QTRUPD q ON e.empno=q.empno 
                LEFT JOIN QTRADMINREMARKS R on e.empno=r.empno
                WHERE e.empno LIKE @empno AND e.empname LIKE @empname
                ORDER BY e.empname";

            using (var conn = new MySqlConnection(connectionString))
            using (var cmd = new MySqlCommand(sql, conn))
            {
                
                cmd.Parameters.AddWithValue("@empno", $"%{empNo}%");
                cmd.Parameters.AddWithValue("@empName", $"%{empName}%");

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

        public AdminLogin MapEmployeeFromReaderForTable(MySqlDataReader reader)
        {
            return new AdminLogin
            {
                EmpNo = reader["empno"] != DBNull.Value ? reader["empno"].ToString() : null,
                EmpName = reader["empname"] != DBNull.Value ? reader["empname"].ToString() : null,
                Category = reader["category"] != DBNull.Value ? reader["category"].ToString() : null,
                Remarks = reader["remarks"] != DBNull.Value ? reader["remarks"].ToString() : null
            };
        }

        public void InsertOrUpdateAdminRemark(string empNo, string remarks)
        {

            using (var conn = new MySqlConnection(connectionString))
            {
                try
                {
                    conn.Open();

                    var checkSql = "SELECT COUNT(1) FROM qtradminremarks WHERE empno = @empno";
                    long recordExists = 0;
                    using (var checkCmd = new MySqlCommand(checkSql, conn))
                    {
                        checkCmd.Parameters.AddWithValue("@empno", empNo);
                        recordExists = (long)checkCmd.ExecuteScalar();
                    }

                    string finalSql;
                    if (recordExists > 0)
                    {
                        finalSql = "UPDATE qtradminremarks SET remarks = @remarks WHERE empno = @empno";
                    }
                    else
                    {
                        finalSql = "INSERT INTO qtradminremarks (empno, remarks) VALUES (@empno, @remarks)";
                    }

                    using (var cmd = new MySqlCommand(finalSql, conn))
                    {
                        cmd.Parameters.AddWithValue("@empno", empNo);
                        cmd.Parameters.AddWithValue("@remarks", remarks);
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine($"A database error occurred: {ex.Message}");
                    throw; 
                }
            }
        }


        //public void UpdateRoomRemark(string qtrNo, string remarks)
        //{
        //    // Ensure you have a 'connectionString' variable available in this class
        //    using (var conn = new MySqlConnection(connectionString))
        //    {
        //        try
        //        {
        //            conn.Open();

        //            // The SQL statement now directly updates the record.
        //            // It will affect rows where QtrNo matches, and do nothing if no match is found.
        //            string sql = "UPDATE qtrupd SET rem = @remarks WHERE QtrNo = @qtrNo";

        //            using (var cmd = new MySqlCommand(sql, conn))
        //            {
        //                // Add parameters to prevent SQL injection
        //                cmd.Parameters.AddWithValue("@qtrNo", qtrNo);
        //                cmd.Parameters.AddWithValue("@remarks", remarks);

        //                // Execute the command
        //                cmd.ExecuteNonQuery();
        //            }
        //        }
        //        catch (MySqlException ex)
        //        {
        //            // Log the database-specific error and re-throw to be handled by the controller.
        //            Console.WriteLine($"A database error occurred: {ex.Message}");
        //            throw;
        //        }
        //    }
        //}



    }
}