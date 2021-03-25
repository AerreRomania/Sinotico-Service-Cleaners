using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using wcfcleaners.Model;

namespace wcfcleaners
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        #region Cleaners

        public DataTable GetAlmTable(string tablename)
        {
            string q = string.Empty;
            if (tablename == "cleaning") q = "select id,machine,shift,operator,startclean,endclean,note,type,prognum from cleaning where alm='true'";
            else q = "select c.id,machine,shift,a.fullname,eventdate,endt,note_alarm,progressivo from cquality c " +
                     "inner join ( select id, fullname from operators )a on a.Id = operator_id " +
                     "where note_alarm <> '' and note_alarm is not null order by machine";

            DataTable tmpdt = new DataTable("AlarmTable");
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                tmpdt.Load(dr);
                con.Close();
                dr.Close();
            }
            foreach (DataColumn c in tmpdt.Columns)
            {
                var firstChar = c.ColumnName.Substring(0, 1).ToUpper();
                var rest = c.ColumnName.Remove(0, 1);
                c.ColumnName = string.Concat(firstChar, rest);
            }
            return tmpdt;
        }
        public DataTable GetGeneralTable()
        {
            //return string.Format("You entered: {0}", value);
            DataTable dt = new DataTable("GeneralTable");
            var todayDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            const string q = "select * from cleaning where convert(date,eventdate,101)=@param1";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.DateTime).Value = todayDate;
                con.Open();
                var dr = cmd.ExecuteReader();
                dt.Load(dr);
                con.Close();
                dr.Close();
            }
            return dt;
        }
        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }
        public string[] GetOperatorName(string code)
        {
            var str = "";
            var strType = "";
            var opId = "";
            var q = "select a.fullname, b.jobname, a.Id from operators a left join jobtypes b on a.jobtype_id = b.id where code='" + code + "'";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        str = dr[0].ToString();
                        strType = dr[1].ToString();
                        opId = dr[2].ToString();
                    }
                        
                con.Close();
                dr.Close();
            }
            return new string[] { str, strType,opId};
        }
        public int GetProgressiveNumber(DateTime evDate, string cShift, int mac, string operatorx, string type, string tablename)
        {
            var i = 0;
            var newEventDate = new DateTime(evDate.Year, evDate.Month, evDate.Day);
            string q = string.Empty;

            if (tablename == "cleaning") q = "select prognum from cleaning where convert(date,eventdate,101)= '" + newEventDate.Date.ToString("yyyy-MM-dd") + "' " +
                 "and shift='" + cShift + "' and machine='" + mac + "' and operator='" + operatorx
                     + "' and type='" + type + "'";
            else q = "select progressivo from cquality where convert(date,eventdate,101)= '" + newEventDate.Date.ToString("yyyy-MM-dd") + "' " +
               "and shift='" + cShift + "' and machine='" + mac.ToString() + "' and operator_id='" + operatorx
                   + "'";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();

                if (!dr.HasRows)
                    i = 1;
                else
                    while (dr.Read())
                    {
                        int.TryParse(dr[0].ToString(), out var j);
                        i = j + 1;
                    }
            }
            return i;
        }
        public void InsertNewOperation(DateTime evDate, string cShift, string operatorName, DateTime cStart, DateTime cEnd,
            int machine, string reason, string cType, string note, DateTime dateLoad, int progNum, int tempoMin, string ptPrec)
        {
            var q = "insert into cleaning (eventdate,shift,operator,startclean,endclean,machine,reason,type," +
                             "note,dateload,prognum,tempomin,ptprec)" +
                             " values (@param1,@param2,@param3,@param4,@param5,@param6,@param7," +
                             "@param8,@param9,@param10,@param11,@param12,@param13)";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.DateTime).Value = evDate;
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = cShift;
                cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = operatorName;
                cmd.Parameters.Add("@param4", SqlDbType.DateTime).Value = cStart;
                cmd.Parameters.Add("@param5", SqlDbType.DateTime).Value = cEnd;
                cmd.Parameters.Add("@param6", SqlDbType.Int).Value = machine;
                cmd.Parameters.Add("@param7", SqlDbType.NVarChar).Value = reason;
                cmd.Parameters.Add("@param8", SqlDbType.NVarChar).Value = cType;
                cmd.Parameters.Add("@param9", SqlDbType.NVarChar).Value = note;
                cmd.Parameters.Add("@param10", SqlDbType.DateTime).Value = dateLoad;
                cmd.Parameters.Add("@param11", SqlDbType.Int).Value = progNum;
                cmd.Parameters.Add("@param12", SqlDbType.Int).Value = tempoMin;
                cmd.Parameters.Add("@param13", SqlDbType.NVarChar).Value = ptPrec;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public long GetJobId(string table)
        {
            var q = "select max(id) from " + table;
            long id = 0;
            using (var c = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, c);
                c.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                {
                    while (dr.Read())
                    {
                        id = Convert.ToInt64(dr[0].ToString());
                    }
                }
                c.Close();
            }
            return id;
        }
        public void UpdateOperation(DateTime endclean, long id)
        {
            var q = "update cleaning set endclean=@param1 where " +
                    "id=@param2";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.DateTime).Value = endclean;
                cmd.Parameters.Add("@param2", SqlDbType.BigInt).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void UpdateOperation1(DateTime endclean, long id, string note)
        {
            var q = "update cleaning set endclean=@param1,note=@param2 where " +
                    "id=@param3";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.DateTime).Value = endclean;
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = note;
                cmd.Parameters.Add("@param3", SqlDbType.BigInt).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void UpdateOperation2(DateTime endclean, long id, int timeStamp)
        {
            var q = "update cleaning set endclean=@param1,tempomin=@param2 where " +
                    "id=@param3";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.DateTime).Value = endclean;
                cmd.Parameters.Add("@param2", SqlDbType.Int).Value = timeStamp;
                cmd.Parameters.Add("@param3", SqlDbType.BigInt).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void UpdateOperation3(DateTime endclean, long id, string note, int timeStamp)
        {
            var q = "update cleaning set endclean=@param1,note=@param2,tempomin=@param3 where " +
                    "id=@param4";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.DateTime).Value = endclean;
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = note;
                cmd.Parameters.Add("@param3", SqlDbType.Int).Value = timeStamp;
                cmd.Parameters.Add("@param4", SqlDbType.BigInt).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void UpdateOperationManually(int id, string note)
        {
            const string q = "update cleaning set note=@param1 where id=@param2";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);

                //end clean
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = note;
                //note
                cmd.Parameters.Add("@param2", SqlDbType.Int).Value = id;
            }
        }
        public void ActivateAlarm(long id)
        {
            const string q = "update cleaning set alm='true' where id=@param1";

            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.BigInt).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void ActivateAlarmNote(long id, string note)
        {
            const string q = "update cleaning set alm='true',note=@param1 where id=@param2";

            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.NVarChar).Value = note;
                cmd.Parameters.Add("@param2", SqlDbType.BigInt).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public void DeactivateAlarm(long id, string tablename, string user)
        {
            string q = string.Empty;
            if (tablename == "cleaning") q = "update cleaning set alm='0',deleteuser=@param0 where id=@param1";
            else q = "update cquality set note_alarm=@paramEmpty,deleteuser=@param0 where id = @param1";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                if (tablename != "cleaning")
                {
                    cmd.Parameters.Add("@paramEmpty", SqlDbType.NVarChar).Value = string.Empty;
                }
                cmd.Parameters.Add("@param0", SqlDbType.NVarChar).Value = user;
                cmd.Parameters.Add("@param1", SqlDbType.BigInt).Value = id;
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
        public string GetShift()
        {
            var tmpShift = "";
            // Use to compare with specific time-spans
            var curTime = DateTime.Now.TimeOfDay;
            var nightShiftStart = new TimeSpan(23, 0, 0);
            var nightShiftEnd = new TimeSpan(7, 0, 0);
            var morningShiftStart = new TimeSpan(7, 0, 0);
            var morningShiftEnd = new TimeSpan(15, 0, 0);
            var afternShiftStart = new TimeSpan(15, 0, 0);
            var afternShiftEnd = new TimeSpan(23, 0, 0);
            // Recognize shifts using current times
            if (nightShiftStart > nightShiftEnd && curTime > nightShiftStart || curTime < nightShiftEnd)
                tmpShift = "NIGHT";
            else if (curTime > morningShiftStart && curTime < morningShiftEnd)
                tmpShift = "MORNING";
            else if (curTime > afternShiftStart && curTime < afternShiftEnd)
                tmpShift = "AFTERNOON";
            return tmpShift;
        }

        public double GetTimeStampInMM(DateTime start, DateTime end)
        {
            var startTimeSpan = new TimeSpan(start.Day, start.Hour, start.Minute, start.Second, start.Millisecond);
            var endTimeSpan = new TimeSpan(end.Day, end.Hour, end.Minute, end.Second, end.Millisecond);
            return endTimeSpan.Subtract(startTimeSpan).TotalMinutes;
        }
        public bool HasError()
        {
            return false;
        }

        #endregion Cleaners

        #region ControlQuality

        public DataTable MachineTable(string oper_id, DateTime evDate, string shift)
        {
            var tmpTable = new DataTable("MachineTable");
            string q = "select machine as Macchina,Articolo,Commessa,Componente,Taglia,Colore,Cotta,progressivo as Progr,Esito " +
                       "from cquality " +
                       "where convert(date,eventdate, 101) ='" + evDate.ToString("MM/dd/yyyy") + "'" +
                       " and operator_id ='" + oper_id + "' and shift='" + shift + "'";
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                tmpTable.Load(dr);
                con.Close();
                dr.Close();
            }
                return tmpTable;
        }
        public DataTable LoadMachineInformations(int mac)
        {
            DataTable cqTable = new DataTable("CqTable");

            cqTable.Columns.Add("macchina");
            cqTable.Columns.Add("articolo");
            cqTable.Columns.Add("componente");
            cqTable.Columns.Add("taglia");
            cqTable.Columns.Add("colore");

            string q = "select [Machine Registration No.], [File Name], [partname], [sizename], [colorname] " +
                       "from csv_temporary " +
                       "where [Machine Registration No.]='" + mac.ToString()+"'";
            var newRow = cqTable.NewRow();
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                con.Open();
                var dr = cmd.ExecuteReader();
                if(dr.HasRows)
                {                    
                    while(dr.Read())
                    {
                        newRow[0] = dr[0].ToString();
                        newRow[1] = dr[1].ToString();
                        newRow[2] = dr[2].ToString();
                        newRow[3] = dr[3].ToString();
                        newRow[4] = dr[4].ToString();
                    }
                }
                cqTable.Rows.Add(newRow);
                con.Close();
                dr.Close();
            }

            return cqTable;
        }
        public void InsertQualityRecord(int macc, string comm, string art, string comp, string tag, string col, string cotta, int prog,
                                         string noteA, string ag, string rang, string lung, string lang, string elastic,
                                        string alt, string cat, string sx, string cart, string infil, string cottaFil,
                                        DateTime startEv, DateTime endEv, int operatId, string shift, string esi, string fullname)
        {
            var q = "insert into cquality (machine,commessa,articolo,componente,taglia,colore,cotta,progressivo," +
                             "note_alarm,aghi,ranghi,lunghezza, langhezza, elasticita, altezza," +
                             "catanelle, sx, cartellino_scheda, infilatura, cotta_filo, eventdate, endt, operator_id, shift, esito, OperatorFullName)" +
                             " values (@param1,@param2,@param3,@param4,@param5,@param6,@param7," +
                             "@param8,@param10,@param11,@param12,@param13," +
                             "@param14,@param15,@param16,@param17,@param18,@param19," +
                             "@param20,@param21,@param22,@param23,@param24,@param25,@param26,@param27)";

            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@param1", SqlDbType.Int).Value = macc;
                cmd.Parameters.Add("@param2", SqlDbType.NVarChar).Value = comm;
                cmd.Parameters.Add("@param3", SqlDbType.NVarChar).Value = art;
                cmd.Parameters.Add("@param4", SqlDbType.NVarChar).Value = comp;
                cmd.Parameters.Add("@param5", SqlDbType.NVarChar).Value = tag;
                cmd.Parameters.Add("@param6", SqlDbType.NVarChar).Value = col;
                cmd.Parameters.Add("@param7", SqlDbType.NVarChar).Value = cotta;
                cmd.Parameters.Add("@param8", SqlDbType.Int).Value = prog; 
                cmd.Parameters.Add("@param10", SqlDbType.NVarChar).Value = noteA;
                cmd.Parameters.Add("@param11", SqlDbType.NVarChar).Value = ag;
                cmd.Parameters.Add("@param12", SqlDbType.NVarChar).Value = rang;
                cmd.Parameters.Add("@param13", SqlDbType.NVarChar).Value = lung;
                cmd.Parameters.Add("@param14", SqlDbType.NVarChar).Value = lang;
                cmd.Parameters.Add("@param15", SqlDbType.NVarChar).Value = elastic;
                cmd.Parameters.Add("@param16", SqlDbType.NVarChar).Value = alt;
                cmd.Parameters.Add("@param17", SqlDbType.NVarChar).Value = cat;
                cmd.Parameters.Add("@param18", SqlDbType.NVarChar).Value = sx;
                cmd.Parameters.Add("@param19", SqlDbType.NVarChar).Value = cart;
                cmd.Parameters.Add("@param20", SqlDbType.NVarChar).Value = infil;
                cmd.Parameters.Add("@param21", SqlDbType.NVarChar).Value = cottaFil;
                cmd.Parameters.Add("@param22", SqlDbType.DateTime).Value = startEv;
                cmd.Parameters.Add("@param23", SqlDbType.DateTime).Value = endEv;
                cmd.Parameters.Add("@param24", SqlDbType.Int).Value = operatId;
                cmd.Parameters.Add("@param25", SqlDbType.NVarChar).Value = shift;
                cmd.Parameters.Add("@param26", SqlDbType.NVarChar).Value = esi;
                cmd.Parameters.Add("@param27", SqlDbType.NVarChar).Value = fullname;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public DataSet GetEmailInfo()
        {
            var ds = new DataSet("maildict");
            var q = "select fullname as name,email as mail from operators where jobtype_id=5";
            using (var c = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, c);
                var da = new SqlDataAdapter(cmd);
                da.Fill(ds);
                da.Dispose();
            }
            return ds;
        }
        #endregion ControlQuality

        #region Operators

        public Operator LoginOperator(string pin)
        {
            string q = "select CodAngajat,Angajat,Mansione,Id from Angajati where CodAngajat=@Pin";
            Operator _operator = null;
            using (var con = new SqlConnection(Config.ConnStringOY))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@Pin", SqlDbType.NVarChar).Value = pin;
                con.Open();
                var dr = cmd.ExecuteReader();
                if(dr.HasRows)
                    while(dr.Read())
                    {
                        int.TryParse(dr[3].ToString(), out var id);
                        _operator = new Operator();
                        _operator.Pin = dr[0].ToString();
                        _operator.FullName = dr[1].ToString();
                        _operator.JobType = dr[2].ToString();
                        _operator.Id = id;
                        _operator.Shift = GetShift();
                    }
                con.Close();
                dr.Close();
            }
            return _operator;
        }

        public OperatorsJob StartOperatorsJob(string FullName, string Line)
        {
            var q = "insert into OperatorsJob (FullName,Line,StartTime,EndTime,Shift)" +
                                       " output INSERTED.ID" +
                                       " values (@FullName,@Line,@StartTime,@EndTime,@Shift)";
            long id = 0;
            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@FullName", SqlDbType.NVarChar).Value = FullName;
                cmd.Parameters.Add("@Line", SqlDbType.NVarChar).Value = Line;
                cmd.Parameters.Add("@StartTime", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = DBNull.Value;
                cmd.Parameters.Add("@Shift", SqlDbType.NVarChar).Value = GetShift();

                con.Open();
                var obj = cmd.ExecuteScalar();
                con.Close();
                long.TryParse(obj.ToString(), out id);
            }

            OperatorsJob job = new OperatorsJob();
            job.FullName = FullName;
            job.Line = Line;
            job.StartTime = DateTime.Now;
            job.EndTime = DateTime.MinValue;
            job.Shift = GetShift();

            if (id != 0)
            {
                job.Id = id;
                return job;
            } 
            else
                return null;
        }

        public void EndOperatorsJob(long jobId)
        {
            var q = "update OperatorsJob set EndTime = @EndTime where Id = @Id";

            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@EndTime", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@Id", SqlDbType.BigInt).Value = jobId;

                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }

        public List<Machine> GetLineMachines(string line)
        {
            string q = "select Descriere,Active from Masini where Linie=@Line and IdSector=7";
            List<Machine> _machines = new List<Machine>();
            using (var con = new SqlConnection(Config.ConnStringOY))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@Line", SqlDbType.NVarChar).Value = line;
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        var machine = new Machine();
                        machine.Code = dr[0].ToString();
                        bool.TryParse(dr[1].ToString(), out var isActive);
                        machine.IsActive = isActive;
                        _machines.Add(machine);
                    }
                con.Close();
                dr.Close();
            }

            q = "select [Machine Registration No.],round(sum(cast([Knit time] as float)) / sum(cast([Elapsed time] as float)) * 100,1)"
                + " from extendview where convert(date, [Workshift End Date Time], 101) = convert(date, @CurrentDate, 101) and"
                + " [Workshift Name]=@Shift and charindex(+ ',' + cast([Machine Registration No.] as nvarchar(20)) + ',',@Machines ) > 0"
                + " group by [Machine Registration No.]";

            if (_machines.Count <= 0)
                return null;

            string machines = string.Empty;
            foreach(var machine in _machines)
            {
                machines += "," + machine.Code;
            }
            machines += ",";

            using (var con = new SqlConnection(Config.ConnString))
            {
                var cmd = new SqlCommand(q, con);
                cmd.Parameters.Add("@CurrentDate", SqlDbType.DateTime).Value = DateTime.Now;
                cmd.Parameters.Add("@Shift", SqlDbType.NVarChar).Value = GetShift();
                cmd.Parameters.Add("@Machines", SqlDbType.NVarChar).Value = machines;
                con.Open();
                var dr = cmd.ExecuteReader();
                if (dr.HasRows)
                    while (dr.Read())
                    {
                        var mac = (from m in _machines
                                   where m.Code == dr[0].ToString()
                                   select m).SingleOrDefault();
                        if (mac == null)
                            continue;
                        double.TryParse(dr[1].ToString(), out var efficiency);
                        mac.Efficiency = efficiency;
                    }
                con.Close();
                dr.Close();
            }
                return _machines;
        }

        #endregion Operators
    }
}
