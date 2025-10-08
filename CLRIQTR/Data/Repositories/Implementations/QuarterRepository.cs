using CLRIQTR.Data.Repositories.Interfaces;
using CLRIQTR.Models;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq.Expressions;

namespace CLRIQTR.Data.Repositories.Implementations
{
    public class QuarterRepository : IQuarterRepository, IDisposable
    {
        private readonly string _connectionString;
        private MySqlConnection _connection;
        private bool _disposed = false;

        public QuarterRepository()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["MySqlConn"].ConnectionString;
        }

        #region Core Database Operations

        private MySqlConnection GetConnection()
        {
            if (_connection == null || _connection.State != ConnectionState.Open)
            {
                _connection = new MySqlConnection(_connectionString);
                _connection.Open();
            }
            return _connection;
        }

        private T ExecuteQuery<T>(string sql, Func<MySqlCommand, T> operation, params MySqlParameter[] parameters)
        {
            using (var cmd = new MySqlCommand(sql, GetConnection()))
            {
                if (parameters != null && parameters.Length > 0)
                {
                    cmd.Parameters.AddRange(parameters);
                }

                return operation(cmd);
            }
        }

        private int ExecuteNonQuery(string sql, params MySqlParameter[] parameters)
        {
            return ExecuteQuery(sql, cmd => cmd.ExecuteNonQuery(), parameters);
        }

        #endregion

        #region Quarter Operations

        public List<QtrTypeMaster> GetAllQuarterTypes()
        {
            const string sql = @"SELECT qid, qtrdesc, qtrtype FROM qtr_type_master ORDER BY qtrdesc";

            return ExecuteQuery(sql, cmd =>
            {
                var list = new List<QtrTypeMaster>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new QtrTypeMaster
                        {
                            qid = Convert.ToInt32(reader["qid"]),
                            qtrdesc = reader["qtrdesc"].ToString(),
                            qtrtype = reader["qtrtype"].ToString()
                        });
                    }
                }
                return list;
            });
        }

        public List<QtrMaster> GetQuarterDetailsByType(string qtrType)
        {
            const string sql = @"SELECT qtr_id, qtr_desc, qtr_no, qtr_type, qtr_full_label 
                                 FROM qtr_master 
                                 WHERE qtr_type = @qtrtype 
                                 ORDER BY qtr_no";

            return ExecuteQuery(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@qtrtype", qtrType);
                var list = new List<QtrMaster>();

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        list.Add(new QtrMaster
                        {
                            qtr_id = Convert.ToInt32(reader["qtr_id"]),
                            qtr_desc = reader["qtr_desc"] as string,
                            qtr_no = reader["qtr_no"] as string,
                            qtr_type = reader["qtr_type"] as string,
                            qtr_full_label = reader["qtr_full_label"] as string
                        });
                    }
                }
                return list;
            });
        }

        public QtrUpd GetQuarterByEmployee(string empNo)
        {
            const string sql = @"SELECT * FROM qtrupd WHERE empno = @empno";

            return ExecuteQuery(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@empno", empNo);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapQtrUpdFromReader(reader);
                    }
                }
                return null;
            });
        }

        public QtrUpd GetQuarterByQtrNo(string qtrNo)
        {
            const string sql = @"SELECT * FROM qtrupd WHERE qtrno = @qtrno";

            return ExecuteQuery(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@qtrno", qtrNo);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapQtrUpdFromReader(reader);
                    }
                }
                return null;
            });
        }

        public QtrUpd GetQuarterByPart(string part)
        {
            const string sql = @"SELECT * FROM qtrupd 
                         WHERE qtrno2 = @part OR qtrno3 = @part or qtrno4= @part";

            return ExecuteQuery(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@part", part);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return MapQtrUpdFromReader(reader);
                    }
                }
                return null;
            });
        }

        public void UpdateQuarterStatus(QtrUpd quarter)
        {
            string sql = null;
            Debug.WriteLine((quarter.qtrno));

            try
            {
                if (quarter.qtrstatus == "O")
                {
                    Debug.WriteLine("Update - V100");

                    sql = @"UPDATE qtrupd 
                    SET qtrstatus = @qtrstatus, 
                        occdate = @occdate, 
                        FNAN = @fnan,
                        qtrno1 = @qtrno1,
                        qtrno2 = @qtrno2,
                        qtrno3 = @qtrno3,
                        qtr_count = @qtr_count,
                        qtrtype = @qtrtype,
                        rem = @rem
                    WHERE empno = @empno";


                    if (sql != null)
                    {
                        var parameters = new MySqlParameter[]
                        {
                new MySqlParameter("@qtrstatus", (object)quarter.qtrstatus ?? DBNull.Value),
                new MySqlParameter("@occdate", (object)quarter.occdate ?? DBNull.Value),
                new MySqlParameter("@fnan", (object)quarter.FNAN ?? DBNull.Value),
                new MySqlParameter("@qtrno", (object)quarter.qtrno ?? DBNull.Value),
                new MySqlParameter("@qtrno1", (object)quarter.qtrno1 ?? DBNull.Value),
                new MySqlParameter("@qtrno2", (object)quarter.qtrno2 ?? DBNull.Value),
                new MySqlParameter("@qtrno3", (object)quarter.qtrno3 ?? DBNull.Value),
                new MySqlParameter("@qtr_count", (object)quarter.qtr_count ?? DBNull.Value),
                new MySqlParameter("@qtrtype", (object)quarter.qtrtype ?? DBNull.Value),
                new MySqlParameter("@rem", (object)quarter.rem ?? DBNull.Value),
                    new MySqlParameter("@empno", (object)quarter.empno ?? DBNull.Value)

                        };

                        Debug.WriteLine("SQL Query:");
                        Debug.WriteLine(sql);
                        Debug.WriteLine("Parameters:");
                        foreach (var p in parameters)
                        {
                            Debug.WriteLine($"{p.ParameterName} = {p.Value}");
                        }

                        int rowsAffected = ExecuteNonQuery(sql, parameters);
                        Debug.WriteLine($"Rows affected: {rowsAffected}");
                    }
                    else
                    {
                        Debug.WriteLine("No matching status condition found for update.");
                    }
                }
                else if (quarter.qtrstatus == "V")
                {
                    Debug.WriteLine("Update - V111");
                    Debug.WriteLine((quarter.qtrno));

                    quarter.occdate = DateTime.Now;

                    sql = @"UPDATE qtrupd 
                    SET qtrstatus = @qtrstatus, 
                        occdate = @occdate, 
                        FNAN = @fnan,
                        rem = @rem,
                        empno = NULL,
                        labcode=NULL
                    WHERE empno = @empno";

                    if (sql != null)
                    {
                        var parameters = new MySqlParameter[]
                        {
                new MySqlParameter("@qtrstatus", (object)quarter.qtrstatus ?? DBNull.Value),
                new MySqlParameter("@occdate", (object)quarter.occdate ?? DBNull.Value),
                new MySqlParameter("@fnan", (object)quarter.FNAN ?? DBNull.Value),
                new MySqlParameter("@qtrno", (object)quarter.qtrno ?? DBNull.Value),
                new MySqlParameter("@qtrno1", (object)quarter.qtrno1 ?? DBNull.Value),
                new MySqlParameter("@qtrno2", (object)quarter.qtrno2 ?? DBNull.Value),
                new MySqlParameter("@qtrno3", (object)quarter.qtrno3 ?? DBNull.Value),
                new MySqlParameter("@qtr_count", (object)quarter.qtr_count ?? DBNull.Value),
                new MySqlParameter("@qtrtype", (object)quarter.qtrtype ?? DBNull.Value),
                new MySqlParameter("@rem", (object)quarter.rem ?? DBNull.Value),
                    new MySqlParameter("@empno", (object)quarter.empno ?? DBNull.Value),
                                        new MySqlParameter("@labcode", (object)quarter.labcode ?? DBNull.Value)


                        };

                        Debug.WriteLine("SQL Query:");
                        Debug.WriteLine(sql);
                        Debug.WriteLine("Parameters:");
                        foreach (var p in parameters)
                        {
                            Debug.WriteLine($"{p.ParameterName} = {p.Value}");
                        }

                        int rowsAffected = ExecuteNonQuery(sql, parameters);
                        Debug.WriteLine($"Row affected: {rowsAffected}");
                    }
                    else
                    {
                        Debug.WriteLine("No matching status condition found for update.");
                    }


                }

               
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occurred in UpdateQuarterStatus:");
                Debug.WriteLine(ex.ToString());
            }
        }


        public void UpdateVacant(QtrUpd quarter)
        {
            string sql = null;
            Debug.WriteLine((quarter.qtrno));

            try
            {
                
                    Debug.WriteLine("Update - V11111");
                    Debug.WriteLine((quarter.qtrno));

                    quarter.occdate = DateTime.Now;

                    sql = @"UPDATE qtrupd 
                    SET qtrstatus = 'V', 
                        occdate = @occdate, 
                        FNAN = @fnan,
                        rem = @rem,
                        empno = NULL,
                        labcode=NULL
                    WHERE qtrno = @qtroldno";

                    if (sql != null)
                    {
                        var parameters = new MySqlParameter[]
                        {
                new MySqlParameter("@qtrstatus", (object)quarter.qtrstatus ?? DBNull.Value),
                new MySqlParameter("@occdate", (object)quarter.occdate ?? DBNull.Value),
                new MySqlParameter("@fnan", (object)quarter.FNAN ?? DBNull.Value),
                new MySqlParameter("@qtroldno", (object)quarter.qtroldno ?? DBNull.Value),
                new MySqlParameter("@qtrno1", (object)quarter.qtrno1 ?? DBNull.Value),
                new MySqlParameter("@qtrno2", (object)quarter.qtrno2 ?? DBNull.Value),
                new MySqlParameter("@qtrno3", (object)quarter.qtrno3 ?? DBNull.Value),
                new MySqlParameter("@qtr_count", (object)quarter.qtr_count ?? DBNull.Value),
                new MySqlParameter("@qtrtype", (object)quarter.qtrtype ?? DBNull.Value),
                new MySqlParameter("@rem", (object)quarter.rem ?? DBNull.Value),
                    new MySqlParameter("@empno", (object)quarter.empno ?? DBNull.Value),
                                        new MySqlParameter("@labcode", (object)quarter.labcode ?? DBNull.Value),
                                                            


                        };

                       

                        int rowsAffected = ExecuteNonQuery(sql, parameters);
                        Debug.WriteLine($"Row affected: {rowsAffected}");
                    }
                    else
                    {
                        Debug.WriteLine("No matching status condition found for update.");
                    }


                


            }
            catch (Exception ex)
            {
                Debug.WriteLine("Exception occurred in UpdateQuarterStatus:");
                Debug.WriteLine(ex.ToString());
            }
        }



        public void InsertQuarter(QtrUpd quarter)
        {
            const string sql = @"
    INSERT INTO qtrupd 
    (qtrno, empno, qtrstatus, labcode, occdate, qtrtype, rem, 
     restype, resname, qtrno1, qtrno2, qtrno3, qtr_count, FNAN)
    VALUES 
    (@qtrno, @empno, @qtrstatus, @labcode, @occdate, @qtrtype, @rem,
     @restype, @resname, @qtrno1, @qtrno2, @qtrno3, @qtr_count, @fnan)
    ON DUPLICATE KEY UPDATE
        empno = VALUES(empno),
        qtrstatus = VALUES(qtrstatus),
        labcode = VALUES(labcode),
        occdate = VALUES(occdate),
        qtrtype = VALUES(qtrtype),
        rem = VALUES(rem),
        restype = VALUES(restype),
        resname = VALUES(resname),
        qtrno1 = VALUES(qtrno1),
        qtrno2 = VALUES(qtrno2),
        qtrno3 = VALUES(qtrno3),
        qtr_count = VALUES(qtr_count),
        FNAN = VALUES(FNAN);";

            ExecuteNonQuery(sql,
                new MySqlParameter("@qtrno", (object)quarter.qtrno ?? DBNull.Value),
                new MySqlParameter("@empno", quarter.empno),
                new MySqlParameter("@qtrstatus", (object)quarter.qtrstatus ?? DBNull.Value),
                new MySqlParameter("@labcode", (object)quarter.labcode ?? DBNull.Value),
                new MySqlParameter("@occdate", (object)quarter.occdate ?? DBNull.Value),
                new MySqlParameter("@qtrtype", (object)quarter.qtrtype ?? DBNull.Value),
                new MySqlParameter("@rem", (object)quarter.rem ?? DBNull.Value),
                new MySqlParameter("@restype", (object)quarter.restype ?? DBNull.Value),
                new MySqlParameter("@resname", (object)quarter.resname ?? DBNull.Value),
                new MySqlParameter("@qtrno1", (object)quarter.qtrno1 ?? DBNull.Value),
                new MySqlParameter("@qtrno2", (object)quarter.qtrno2 ?? DBNull.Value),
                new MySqlParameter("@qtrno3", (object)quarter.qtrno3 ?? DBNull.Value),
                new MySqlParameter("@qtr_count", (object)quarter.qtr_count ?? DBNull.Value),
                new MySqlParameter("@fnan", (object)quarter.FNAN ?? DBNull.Value)
            );
        }

        public bool IsQtrNoAvailable(string qtrNo, string currentEmpNo = null)
        {
            const string sql = @"SELECT COUNT(*) FROM qtrupd 
                         WHERE qtrno = @qtrno 
                         AND (empno != @currentEmpNo OR @currentEmpNo IS NULL)
                         AND qtrstatus = 'O'";

            return ExecuteQuery(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@qtrno", qtrNo);
                cmd.Parameters.AddWithValue("@currentEmpNo", (object)currentEmpNo ?? DBNull.Value);

                var count = Convert.ToInt32(cmd.ExecuteScalar());
                return count == 0;
            });
        }

        #endregion

        #region Occupancy Operations

        public List<dynamic> GetPartsWithOccupancyByDesc(string qtrdesc, string currentEmpNo)
        {

            Debug.WriteLine($"Received qtrdesc: {qtrdesc}, currentEmpNo: {currentEmpNo}");

            const string sql = @"
                SELECT 
    qm.qtr_no as PartNumber,
    CASE WHEN qm.status = 'O' THEN 1 ELSE 0 END as Occupied,  
    qm.empno as OccupiedBy,
    CASE WHEN qm.empno = @currentEmpNo THEN 1 ELSE 0 END as IsCurrentUser
FROM qtr_master qm
WHERE qm.qtr_desc =@qtrdesc
ORDER BY qm.qtr_no";

            return ExecuteQuery(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@qtrdesc", qtrdesc);
                cmd.Parameters.AddWithValue("@currentEmpNo", (object)currentEmpNo ?? DBNull.Value);

                var results = new List<dynamic>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new
                        {
                            PartNumber = reader["PartNumber"].ToString(),
                            Occupied = Convert.ToBoolean(reader["Occupied"]),
                            OccupiedBy = reader["OccupiedBy"] as string,
                            IsCurrentUser = Convert.ToBoolean(reader["IsCurrentUser"])
                        });
                    }
                }
                return results;
            });
        }

        #endregion

        #region Helper Methods

        private QtrUpd MapQtrUpdFromReader(MySqlDataReader reader)
        {
            DateTime? occdate = null;

            if (reader["occdate"] != DBNull.Value)
            {
                string occdateValue = reader["occdate"].ToString();

                // Try multiple date formats
                string[] formats = { "yyyy-MM-dd HH:mm:ss", "dd.MM.yyyy", "yyyy-MM-dd", "MM/dd/yyyy" };

                if (DateTime.TryParseExact(occdateValue, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime dt))
                {
                    occdate = dt;
                }
                else if (DateTime.TryParse(occdateValue, out dt))
                {
                    occdate = dt;
                }
            }

            return new QtrUpd
            {
                qtrno = reader["qtrno"] as string,
                empno = reader["empno"] as string,
                qtrstatus = reader["qtrstatus"] as string,
                labcode = reader["labcode"] as string,
                occdate = occdate,
                qtrtype = reader["qtrtype"] as string,
                rem = reader["rem"] as string,
                restype = reader["restype"] as string,
                resname = reader["resname"] as string,
                qtrno1 = reader["qtrno1"] as string,
                qtrno2 = reader["qtrno2"] as string,
                qtrno3 = reader["qtrno3"] as string,
                qtr_count = reader["qtr_count"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["qtr_count"]),
                FNAN = reader["FNAN"] as string
            };
        }

        #endregion

        #region IDisposable Implementation

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_connection != null)
                    {
                        _connection.Dispose();
                        _connection = null;
                    }
                }
                _disposed = true;
            }
        }

        ~QuarterRepository()
        {
            Dispose(false);
        }


        public List<dynamic> GetPartsWithOccupancy(string qtrtype, string currentEmpNo)
        {
            const string sql = @"
        SELECT 
    qm.qtr_no AS PartNumber,
    CASE 
        WHEN MAX(CASE WHEN qu.qtrstatus = 'O' THEN 1 ELSE 0 END) = 1 THEN 1 
        ELSE 0 
    END AS Occupied,
    MAX(CASE WHEN qu.qtrstatus = 'O' THEN qu.empno ELSE NULL END) AS OccupiedBy,
    MAX(CASE WHEN qu.qtrstatus = 'O' AND qu.empno = @currentEmpNo THEN 1 ELSE 0 END) AS IsCurrentUser
FROM qtr_master qm
LEFT JOIN qtrupd qu 
    ON (
        qu.qtrno2 = qm.qtr_no OR 
        qu.qtrno3 = qm.qtr_no OR 
        qu.qtrno4 = qm.qtr_no
    )
    AND qm.qtr_type = qu.qtrtype
WHERE qm.qtr_type = @qtrtype
GROUP BY qm.qtr_no
ORDER BY qm.qtr_no";

            return ExecuteQuery(sql, cmd =>
            {
                cmd.Parameters.AddWithValue("@qtrtype", qtrtype);
                cmd.Parameters.AddWithValue("@currentEmpNo", (object)currentEmpNo ?? DBNull.Value);

                var results = new List<dynamic>();
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        results.Add(new
                        {
                            PartNumber = reader["PartNumber"].ToString(),
                            Occupied = Convert.ToBoolean(reader["Occupied"]),
                            OccupiedBy = reader["OccupiedBy"] as string,
                            IsCurrentUser = Convert.ToBoolean(reader["IsCurrentUser"])
                        });
                    }
                }
                return results;
            });
        }

        #endregion
    }
}