using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Text;
using System.Threading;
using TessituraCleaners.SinoticoREST;
using System.Text.RegularExpressions;
using wcfcleaners;
using System.Web;
using System.IO;
using System.Net.Mail;
using System.Web.UI.HtmlControls;

public partial class Work : System.Web.UI.Page
{
    public string ConString = "Data Source = 192.168.96.17; Initial Catalog = Sinotico; user=sa;password=onlyouolimpias;";
    //public string ConString = "Data Source = DESKTOP-RBJRI9H\\SQLEXPRESS; Initial Catalog = Sinotico; Integrated Security=SSPI;";
    //private const string ConString = "data source=QUANT\\QUANTNEW;Initial Catalog=Sinotico;Integrated Security=SSPI;";
    
    //public string ConString = "Data Source = localhost; Initial Catalog = Sinotico; Integrated Security=SSPI;";


    private List<Label> _listOfMachines = new List<Label>();
    private List<Label> _listOfLines = new List<Label>();
    private List<Label> _listOfBlocks = new List<Label>();
    private List<Cleaners> _listOfCleaners = new List<Cleaners>();
    private Dictionary<Label, Color> _totalEff = new Dictionary<Label, Color>();
    private static DateTime _fromDate1;

    private static DateTime Get_from_date()
    {
        return _fromDate1;
    }
    private static void Set_from_date(DateTime value)
    {
        _fromDate1 = value;
    }

    private static DateTime _toDate1;

    private static DateTime Get_to_date()
    {
        return _toDate1;
    }

    private static void Set_to_date(DateTime value)
    {
        _toDate1 = value;
    }


    private static DateTime GetStartJob { get; set; }


    //private TessituraCleaners.SinoticoREST.Service1Client _client = new Service1Client();
    //private wcfcleaners.Service1 _client1 = new Service1();
    private ServiceReference1.Service1Client _client1 = new ServiceReference1.Service1Client();
    //jel mozemo ponovo da probamo? naravno, hoces kroz debug? dada

    private Timer t = null;
    private void ServiceTimer(Object o)
    {
        _client1.UpdateOperation3(DateTime.Now, IdJob, null, 0);
    }

    public int GetWeekNumber()
    {
        CultureInfo ciCurr = CultureInfo.CurrentCulture;
        int weekNum = ciCurr.Calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        return weekNum;
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Page.Form.Attributes.Add("enctype", "multipart/form-data");

        panel_home_labels.Visible = true;
        panel_home.Visible = true;
        panel_alarm.Visible = false;
        panel_cqlayout.Visible = false;
        panel_quality.Visible = false;
        panel_email.Visible = false;
        panel_image.Visible = false;
        panel_myjob.Visible = false;
        panel_operatori.Visible = false;
        

        lblJobType.Text = Request.QueryString["JobType"];
        lblName.Text = Request.QueryString["Name"];
        lblOpId.Text = Request.QueryString["OpId"];

        if (Request.QueryString["JobType"] == null)
        {
            Response.Redirect("./Login.aspx");
        }

        //btnStop.Enabled = false;
        //btnConfirm.Enabled = false;
        btnAlarm.Enabled = false;
        lblnote.Enabled = false;
        btnStop.BackColor = Color.Crimson;

        System.Net.ServicePointManager.Expect100Continue = false;

        
             if (!Page.IsPostBack)
            { 

                lblJobType.Text = Request.QueryString["JobType"];
                lblName.Text = Request.QueryString["Name"];
                lblOpId.Text = Request.QueryString["OpId"];
                //startDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
                //endDate.Text = DateTime.Now.ToString("yyyy-MM-dd");



                if (Request.QueryString["JobType"] == "CQ")
                {
                Grid_table.DataSource = _client1.GetAlmTable("cquality");
                Grid_table.DataBind();
                gv_myJob.DataSource = _client1.MachineTable(lblOpId.Text, DateTime.Now, _client1.GetShift());
                    gv_myJob.DataBind();
                }
                else if (Request.QueryString["JobType"] == "PULIZIA ORDINARIA")
                {
                    Grid_table.DataSource = _client1.GetAlmTable("cleaning");
                    Grid_table.DataBind();

                    gv_myJob.DataSource = _client1.MachineTable(lblOpId.Text, DateTime.Now, _client1.GetShift());
                    gv_myJob.DataBind();
                }
                else if (Request.QueryString["JobType"] == "OPERATORI")
                {
                    panel_operatori.Visible = true;
                btnAlarm.Visible = false;
                panel_home_labels.Visible = false;
                panel_home.Visible = true;
                    //Grid_table.DataSource = _client1.GetAlmTable("cleaning");
                    //Grid_table.DataBind();

                //gv_myJob.DataSource = _client1.MachineTable(lblOpId.Text, DateTime.Now, _client1.GetShift());
                //gv_myJob.DataBind();
            }

                //CallProcedures("auto");
            }
            //ALARMA CLICK ENABLE WHEN FINISH
            btnAlarm.Click += (s, g) =>
            {
                if (string.IsNullOrEmpty(lblnote.Text))
                    _client1.ActivateAlarmNote(IdJob, null);
                else
                    _client1.ActivateAlarmNote(IdJob, lblnote.Text);

                btnStop.Enabled = true;
                lblnote.Enabled = true;
            };
             
        
    }

    protected void Page_Error(object sender, EventArgs e)
    {
        Exception ex = Server.GetLastError();
        txt_error.Text = ex.ToString() + " " + Server.GetLastError().ToString();
        Server.ClearError();
    }
    
    private System.Timers.Timer _t = new System.Timers.Timer(); //probaj sa ovim, mislim da nece  sa thread-om
    private static long IdJob { get; set; }

    //STOP CLICK
    protected void btnStop_Click(object sender, EventArgs e)
    { 
        txtMachina.Enabled = true;
        lblnote.Text = string.Empty;
        btnLogout.Enabled = true;
        btnConfirm.Enabled = true;
        lblnote.Enabled = false;
        btnAlarm.Enabled = false;
        lbl_error.Text = ""; 


       


         if (Request.QueryString["JobType"] == "OPERATORI")
        {
            if (Request.Cookies["UserID"] != null)
            {
                var value = Request.Cookies["UserID"].Value;
                long.TryParse(value, out var v);
                _client1.EndOperatorsJob(v);

                //SHOW PANEL OPERATORI
                panel_home.Visible = true;
                panel_home_labels.Visible = false;
                panel_operatori.Visible = true;
                btnConfirm.Enabled = true;
            } else
            {
                op_error.InnerText  = "Your job is not started jet.";

            }
        }
        else
        {
            if (txtMachina.Text != string.Empty)
            {
                btnStop.Enabled = true;
                //Timer1.Enabled = false;
                if (Request.QueryString["JobType"] == "PULIZIA ORDINARIA")
                {
                    var str = "";
                    for (var i = 1; i <= 2; i++)
                    {
                        str += tester.Text.Split(' ')[i] + " ";
                    }

                    DateTime.TryParse(str.TrimEnd(), out var start);
                    var tempo = _client1.GetTimeStampInMM(start, DateTime.Now);
                    _client1.UpdateOperation3(DateTime.Now, Convert.ToInt64(tester.Text.Split(' ')[0]), lblnote.Text, Convert.ToInt32(tempo));
                    var ctrl = (Button)sender;
                    var ctrlTxt = ctrl.Text;
                    ctrl.BackColor = Color.Crimson;


                }

                if (Request.QueryString["JobType"] == "CQ")
                {
                    int.TryParse(lbl_machina.Text, out var mac);
                    int.TryParse(lblOpId.Text, out var opid);
                    int.TryParse(lbl_progressivo.Text, out var prog);
                    DateTime.TryParse(lblstartdate.Text, out var _sdate);
                    //ovde
                    _client1.InsertQualityRecord(mac, txt_commessa.Text, lbl_articolo.Text, lbl_componente.Text, lbl_taglia.Text, txt_colore.Text, txt_cotta.Text,
                                                 prog, txt_note.Text, txt_aghi.Text, txt_ranghi.Text, txt_lunghezza.Text, txt_langhezza.Text,
                                                 txt_elasticita.Text, txt_altezza.Text, txt_catanelle.Text, txt_sx.Text, txt_cartellino.Text, txt_infilatiura.Text,
                                                 txt_cottafillato.Text, _sdate, DateTime.Now, opid, _client1.GetShift(), txt_esito.Text, Request.QueryString["Name"]);
                }

                txtMachina.Text = "";
            }
            else
            {
                btnStop.Enabled = false;
                lbl_error.Text = "Your job is not started jet.";
            }
        }




    }

    //START CLICK 
    private List<string> listEmails = new List<string>();
    private StringBuilder eb = new StringBuilder();
    private DateTime _startDate = new DateTime();
    //long OperatorIdJob;

    protected void btnConfig_Click(object sender, EventArgs e)
    {
        tester.Text = string.Empty;

        txt_commessa.Text = string.Empty;
        txt_cotta.Text = string.Empty;
        txt_aghi.Text = string.Empty;
        txt_ranghi.Text = string.Empty;
        txt_langhezza.Text = string.Empty;
        txt_lunghezza.Text = string.Empty;
        txt_elasticita.Text = string.Empty;
        txt_altezza.Text = string.Empty;
        txt_catanelle.Text = string.Empty;
        txt_cimose.Text = string.Empty;
        txt_sx.Text = string.Empty;
        txt_note.Text = string.Empty;
        txt_esito.Text = string.Empty;
        txt_cartellino.Text = string.Empty;
        txt_infilatiura.Text = string.Empty;
        txt_cottafillato.Text = string.Empty;
        lbl_error.Text = "";
        op_error.InnerText = "";

        if (Request.QueryString["JobType"] == "OPERATORI")
        {
            if (txt_linea.Text.Contains("LINEA")) {
            var _machines = _client1.GetLineMachines(txt_linea.Text);
                double sum = 0;
                int count = 0;
                int itemcount = 0;
            foreach (var item in _machines)
            {
                itemcount++;
                var span = new HtmlGenericControl("span");
                span.InnerHtml = "" + item.Code + "<br/>" + item.Efficiency + "";

                if (item.Efficiency > 92)  {
                    span.Attributes["class"] = "nonExpense green";
                } else if (item.Efficiency > 85 && item.Efficiency <= 92)
                {
                    span.Attributes["class"] = "nonExpense yellow";
                }
                else
                {
                    span.Attributes["class"] = "nonExpense red";
                }
                if (itemcount <= 7)
                    {
                        pnl_operator_machines.Controls.Add(span);

                    }
                    else
                    {
                        pnl_operator_machines_second.Controls.Add(span);

                    }
                    sum += item.Efficiency;
                count++;
            }

            var hsum = new HtmlGenericControl("h4");
            hsum.Attributes["class"] = "sum";
            double totale = sum / count;
            hsum.InnerText = "Totale: " + Math.Round(totale,1).ToString() + "%";
                pnl_operator_machines_second.Controls.Add(hsum);

            panel_home.Visible = true;
            panel_home_labels.Visible = false;
            panel_operatori.Visible = true;
            btnConfirm.Enabled = false;

            string fullName = Request.QueryString["Name"];
            string line = txt_linea.Text;
            var job = _client1.StartOperatorsJob(fullName, line);
            HttpCookie userIdCookie = new HttpCookie("UserID");
            userIdCookie.Value = job.Id.ToString();
            Response.Cookies.Add(userIdCookie);
            }
            else
            {
                op_error.InnerText = "Error: Linie Value not correct! it must be example LINEA1";
                panel_home.Visible = true;
                panel_home_labels.Visible = false;
                panel_operatori.Visible = true;
            }
            
        } else
        {
            if (txtMachina.Text != string.Empty)
            {
                lbl_error.Text = string.Empty;
                txtMachina.Enabled = false;
                btnConfirm.Enabled = false;
                btnStop.Enabled = true;
                btnLogout.Enabled = false;
                lblnote.Enabled = true;
                btnAlarm.Enabled = true;

                //Timer1.Enabled = true;

                var ctrl = (Button)sender;
                var ctrlTxt = ctrl.Text;
                var ddi = org.SelectedItem.Value;
                if (string.IsNullOrEmpty(txtMachina.Text)) return;
                int.TryParse(txtMachina.Text, out var mac);
                if (mac < 1 || mac > 210) return;
                _startDate = DateTime.Now;
                lblstartdate.Text = _startDate.ToString();
                var shift = _client1.GetShift();
                var type = org.SelectedItem.Value.ToString();

                GetStartJob = DateTime.Now;

                if (Request.QueryString["JobType"] == "CQ")
                {
                    var progNum = _client1.GetProgressiveNumber(_startDate, shift, mac, lblOpId.Text, type, "cquality");
                    if (string.IsNullOrEmpty(progNum.ToString())) progNum = 1;
                    lblProgressivo.Text = progNum.ToString();

                    IdJob = _client1.GetJobId("cquality") + 1;
                    DataTable date = new DataTable();
                    date = _client1.LoadMachineInformations(Convert.ToInt32(txtMachina.Text));

                    lbl_machina.Text = date.Rows[0][0].ToString();
                    lbl_articolo.Text = date.Rows[0][1].ToString();
                    lbl_componente.Text = date.Rows[0][2].ToString();
                    lbl_taglia.Text = date.Rows[0][3].ToString();
                    txt_colore.Text = date.Rows[0][4].ToString();
                    lbl_progressivo.Text = lblProgressivo.Text;

                }
                else if (Request.QueryString["JobType"] == "PULIZIA ORDINARIA")
                {
                    var progNum = _client1.GetProgressiveNumber(_startDate, shift, mac, lblName.Text, type, "cleaning");
                    if (string.IsNullOrEmpty(progNum.ToString())) progNum = 1;
                    lblProgressivo.Text = progNum.ToString();

                    IdJob = _client1.GetJobId("cleaning") + 1;

                    _client1.InsertNewOperation(_startDate, shift, lblName.Text, DateTime.Now, DateTime.Now, mac, string.Empty, ddi, lblnote.Text, DateTime.Now, progNum, 1, "0");

                }
                else if (Request.QueryString["JobType"] == "PULIZIA FRONTURE")
                {
                    var progNum = _client1.GetProgressiveNumber(_startDate, shift, mac, lblName.Text, type, "cleaning");
                    if (string.IsNullOrEmpty(progNum.ToString())) progNum = 1;
                    lblProgressivo.Text = progNum.ToString();

                    IdJob = _client1.GetJobId("cleaning") + 1;

                    _client1.InsertNewOperation(_startDate, shift, lblName.Text, DateTime.Now, DateTime.Now, mac, string.Empty, "Pulizia Fronture", lblnote.Text, DateTime.Now, progNum, 1, "0");

                }

                tester.Text = IdJob.ToString() + " " + GetStartJob.ToString("yyyy/MM/dd HH:mm:ss tt", CultureInfo.InvariantCulture);


                panel_quality.Visible = true;
                if (lblJobType.Text == "CQ")
                {
                    panel_quality.Visible = true;
                    btnAlarm.Visible = false;
                    panel_home_labels.Visible = false;
                    panel_operatori.Visible = false;

                }
                else if (lblJobType.Text == "OPERATORI")
                {
                    panel_quality.Visible = false;
                    btnAlarm.Visible = false;
                    panel_home_labels.Visible = false;
                    pnlhome.Visible = true;
                    panel_operatori.Visible = true;
                }
                else
                {
                    panel_quality.Visible = false;
                    btnAlarm.Visible = true;
                    panel_home_labels.Visible = true;
                    panel_operatori.Visible = false;
                }


            }
            else
            {
                btnStop.Enabled = false;
                lbl_error.Text = "You must enter Machine number!";
            }

        }




    }
     
    //LOGOUT CLICK
    public void LogOut(object sender, EventArgs e)
    {
        Session?.Abandon();
        Response.Redirect("./Login.aspx");
    }

    private void CreateObjectGroups()
    {
        _listOfMachines = new List<Label>();
        _listOfLines = new List<Label>();
        _listOfBlocks = new List<Label>();

        foreach (var groupBox in new[] { mainBody, Table1, Table2 })  //Goes through all sub-groups
        {
            // Collects all machines to their list
            foreach (TableRow tr in groupBox.Rows)
            {
                foreach (TableCell tc in tr.Cells)
                {
                    foreach (var machine in (from lbl in tc.Controls.OfType<Label>() //groupBox.Controls.OfType<Label>()
                                             where lbl.ID.Substring(0, 1) == "p"
                                             select lbl).ToList())
                    {
                        //Set machine listeners

                        _listOfMachines.Add(machine);
                    }
                    // Collects all of the lines in their list    
                    foreach (var line in (from lbl in tc.Controls.OfType<Label>()
                                          where lbl.ID.Substring(0, 1) == "l"
                                          select lbl).ToList())
                    {
                        _listOfLines.Add(line);
                    }
                    // Collects all blocks in their list
                    foreach (var block in (from lbl in tc.Controls.OfType<Label>()
                                           where lbl.ID.Substring(0, 1) == "s"
                                           select lbl).ToList())
                    {
                        _listOfBlocks.Add(block);
                    }
                }
            }
        }
    }


    private string _cleanersType = string.Empty;
    private string _mod = string.Empty;
    private DataTable _tbl_machines = new DataTable();
    private List<CleanedMachines> _cleaned_machines = new List<CleanedMachines>();
    private List<Cleaners> _list_of_cleaners = new List<Cleaners>();
    private Dictionary<int, int> _cleaners_per_machine = new Dictionary<int, int>();
    private Dictionary<Label, Color> _currentColors = new Dictionary<Label, Color>();
    private void CallProcedures(string mode)
    {
        _tbl_machines = new DataTable();
        CreateObjectGroups();

        foreach (var listGroup in new[] { _listOfMachines, _listOfBlocks, _listOfLines })
        {
            foreach (var label in listGroup.OfType<Label>())
            {
                label.Text = "";
            }
        }
        var lstShift = new List<string>();
        lstShift.Add("NIGHT");
        lstShift.Add("MORNING");
        lstShift.Add("AFTERNOON");
        var fromDate = new DateTime();
        var toDate = new DateTime();
        string shift = string.Empty;
        var sdate = DateTime.Now;
        var edate = DateTime.Now;
        DateTime.TryParse(startDate.Text, out sdate);
        DateTime.TryParse(endDate.Text, out edate);
        if (sdate == DateTime.MinValue) sdate = DateTime.Now;
        else DateTime.ParseExact(startDate.Text.ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        if (edate == DateTime.MinValue) edate = DateTime.Now;
        else DateTime.ParseExact(endDate.Text.ToString(), "yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture);
        startDate.Text = sdate.ToString("yyyy-MM-dd");
        endDate.Text = edate.ToString("yyyy-MM-dd");
        shift = _client1.GetShift();
        var selIdx = turno.SelectedIndex;
        var lIdx = lstShift.IndexOf(shift);

        if (lblturno.Text == "True")
        {
            turno.SelectedIndex = lIdx;
        }
        else
        {
            if (selIdx == lIdx)
            {
                turno.SelectedIndex = lIdx;
            }
        }
        lblturno.Text = "False";
        var selShift = lstShift.ElementAt(Convert.ToInt32(turno.SelectedValue) - 1);
        if (shift != selShift)
        {
            shift = selShift;
        }
        fromDate = sdate; 
        toDate = edate; 

        var type = org.SelectedItem.Value;
        bobitester.Text = type + "  " + shift + "  " + fromDate.ToString();
        
        var _dataSet = new DataSet();
        using (var con = new SqlConnection(ConString))
        {
            var cmd = new SqlCommand("getcleanersession", con)
            {
                CommandType = CommandType.StoredProcedure
            };
            cmd.Parameters.Add("@fromDate", SqlDbType.DateTime).Value = new DateTime(fromDate.Year, fromDate.Month, fromDate.Day);
            cmd.Parameters.Add("@toDate", SqlDbType.DateTime).Value = new DateTime(toDate.Year, toDate.Month, toDate.Day);
            cmd.Parameters.Add("@shift", SqlDbType.NVarChar).Value = ","+shift+","; //charindex oke moze sada 
            cmd.Parameters.Add("@type", SqlDbType.NVarChar).Value = org.SelectedItem.Value;

            var sd = DateTime.Parse(startDate.Text);
            var ed = DateTime.Parse(endDate.Text);

            cmd.Parameters.Add("@oneDayOnly", SqlDbType.Bit).Value = ed.Equals(sd)? true : false;        
            con.Open();
            var da = new SqlDataAdapter(cmd);
            var ds = new DataSet();
            da.Fill(_dataSet);
            da.Dispose();
        }
        _list_of_cleaners.Clear();

        if (_mod == "cleaner")
        {
            if (_cleanersType == "cquality")
            {
                _cleaned_machines.Clear();

                foreach (DataRow row in _dataSet.Tables[3].Rows)
                {
                    _list_of_cleaners.Add(new Cleaners(row[0].ToString(), row[1].ToString(), Convert.ToInt32(row[2])));
                }
                foreach (DataRow row in _dataSet.Tables[4].Rows)
                {
                    int.TryParse(row[0].ToString(), out var idm);
                    int.TryParse(row[1].ToString(), out var num_of_cleaners);
                    _cleaners_per_machine.Add(idm, num_of_cleaners);
                }
            }
            else
            {
                foreach(DataRow row in _dataSet.Tables[0].Rows)
                        {
                    _list_of_cleaners.Add(new Cleaners(row[0].ToString(), row[1].ToString(), Convert.ToInt32(row[2])));
                }
                foreach (DataRow row in _dataSet.Tables[1].Rows)
                {
                    int.TryParse(row[0].ToString(), out var idm);
                    int.TryParse(row[1].ToString(), out var num_of_cleaners);
                    _cleaners_per_machine.Add(idm, num_of_cleaners);
                }
            }
        }
        else
        {
            _cleaned_machines.Clear();
            using (var con = new SqlConnection(ConString))
            {
                var cmd = new SqlCommand("get_data", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                cmd.Parameters.Add("@finesse", SqlDbType.VarChar).Value = ",3,7,14,";
                cmd.Parameters.Add("@from_date", SqlDbType.DateTime).Value = startDate.Text;
                cmd.Parameters.Add("@to_date", SqlDbType.DateTime).Value = endDate.Text;
                cmd.Parameters.Add("@shift", SqlDbType.VarChar).Value = "," + shift + ",";
                cmd.Parameters.Add("@file_name", SqlDbType.VarChar).Value = string.Empty;
                con.Open();
                var dr = cmd.ExecuteReader();            
                _tbl_machines.Load(dr);
                con.Close();
                dr.Close();
            }
            //GetMachineAlarms();
            //if (_machine_alarms.Count >= 1)
            //    foreach (var mac in _list_of_currents)
            //    {
            //        if (machine_alarms.ContainsKey(mac.MachineNumber)) mac.Alarm = machine_alarms[mac.MachineNumber];
            //        else mac.Alarm = false;
            //    }
        }

        foreach (var machine in _listOfMachines)
        {
            machine.BackColor = Color.LightGray;
            machine.Text = default(string);

            if (_mod != "cleaner")
            {
                foreach (DataRow row in _tbl_machines.Rows)
                {
                    if (machine.ID.Remove(0, 1) != row[0].ToString()) continue;

                    var ferme = TimeSpan.FromMinutes(Convert.ToDouble(row[4]));
                    var tempStd = TimeSpan.FromMinutes(Convert.ToDouble(row[3]));

                    var strFerme = ferme.ToString(@"h\:mm");
                    var strTempoStandard = tempStd.ToString(@"h\:mm");
                    var temp = string.Empty;
                    var eff = string.Concat(Math.Round(Convert.ToDouble(row[1]), 0).ToString(), "  ");
                    machine.Text =  eff;
                    //switch (_mod)
                    //{
                    //    case "eff":
                    //        var eff = string.Concat(Math.Round(Convert.ToDouble(row[1]), 0).ToString(), "  ");
                    //        machine.Text = eff;
                    //        //machine.Font = _fontBig;
                    //        break;
                    //    //case "tempStd":
                    //    //    machine.Text = strTempoStandard;
                    //    //    //machine.Font = _fontSmall;
                    //    //    break;
                    //    //case "ferme":
                    //    //    machine.Text = strFerme;
                    //    //    //machine.Font = _fontBig;
                    //    //    break;
                    //    //case "qty":
                    //    //    machine.Text = row[2].ToString() + "  ";
                    //    //    //machine.Font = _fontBig;
                    //    //    break;
                    //}


                    //machine.BackColor =
                    //    !string.IsNullOrEmpty(machine.Text)
                    //    ? machine.BackColor = Color.DimGray :
                    machine.BackColor = GetEfficiencyColor(row[1].ToString());
                     
                }

            }
            else
            {
                var days = (Get_to_date().Subtract(Get_from_date())).TotalDays;
                if (days == 0)
                {
                    foreach (var item in _list_of_cleaners)
                    {
                        var macId = item.Machine;
                        var initial = item.NameInitials;
                        var prod = item.Prodgen;

                        if (machine.ID.Remove(0, 1) != macId) continue;

                        machine.Text = initial + "/" + prod.ToString();
                        //machine.Font = _fontBig;
                        machine.BackColor = Color.FromArgb(54, 180, 86);
                    }
                }
                else
                {
                    foreach (var item in _list_of_cleaners)
                    {
                        var macId = item.Machine;
                        var prod = item.Prodgen;
                        var num_of_cleaners = string.Empty;

                        if (machine.ID.Remove(0, 1) != macId) continue;
                        
                        if (_cleaners_per_machine.ContainsKey(int.Parse(macId)))
                            num_of_cleaners = _cleaners_per_machine[int.Parse(macId)].ToString();

                        var oldProd = 0;
                        if (machine.Text != string.Empty)
                        {
                            int.TryParse(machine.Text.Split('/')[1], out oldProd);
                            prod += oldProd;
                        }

                        machine.Text =  num_of_cleaners + "/" + prod.ToString();
                        //machine.Font = _fontBig;
                        machine.BackColor = Color.FromArgb(54, 180, 86);
                    }
                }
            }
        }
        GetTotals();
        foreach (var kvp in _totalEff)
        {
            kvp.Key.BackColor = kvp.Value;
        }
        foreach (var mchn in _listOfMachines)
        {
            if (!string.IsNullOrEmpty(mchn.Text)) continue;
            mchn.Text = "";
        }
    }

    private static Color GetEfficiencyColor(string text)
    {
        var color = default(Color);
        double.TryParse(text, out var eff);

        if (eff == 0)
            color = Color.LightGray;
        else if
            (eff > 0 && eff <= 85.0) color = Color.FromArgb(253, 129, 127);
        else if
            (eff > 85.0 && eff <= 90.0) color = Color.FromArgb(254, 215, 1);
        else if
            (eff > 90.0) color = Color.FromArgb(54, 214, 87);
        else
            color = Color.LightGray;

        return color;
    }

    private const int MaxLine = 14;
    private void GetTotals()
    {
 

        var machineRange = new[] { 1, 14 };     //starts from line 1
        var count = 1;
        var curLineNumber = 1;

        if (_mod == "eff" || _mod == "cleaner" || _mod == "temperature" ||
            _mod == "rammendi" || _mod == "scarti")
        {
            _totalEff = new Dictionary<Label, Color>();
            _totalEff.Clear();
        }

        for (var m = 1; m <= 210; m++)
        {
            count++;

            if (count < MaxLine) continue;

            foreach (var line in _listOfLines.Where(p => p.ID == "l" + curLineNumber.ToString()))
            {
                line.Text = GetMachinesDataInRange(_listOfMachines, machineRange[0], machineRange[1], 0, false).Trim('=');
                //false => is line

                if (_mod == "eff")
                {
                    _totalEff.Add(line, GetEfficiencyColor(line.Text.Trim('%'))); 
                }
             
                else if (_mod == "cleaner")
                {
                    _totalEff.Add(line, Color.FromArgb(54, 215, 86)); 
                }
                
            }

            count = 1;
            machineRange = new int[] { machineRange[1] + 1, machineRange[1] + MaxLine };

            curLineNumber++;
        }

        s1.Text = GetMachinesDataInRange(_listOfMachines, 1, 70, 1, true);
        s2.Text = GetMachinesDataInRange(
            _listOfMachines, 71, 140, 1, true);
        s3.Text = GetMachinesDataInRange(
            _listOfMachines, 141, 210, 1, true);
        total.Text =  GetMachinesDataInRange(
            _listOfMachines, 1, 210, 1, false).Trim('=');

        switch (_mod)
        {
            case "eff":
                _totalEff.Add(s1,
                    GetEfficiencyColor(s1.Text.Trim('=', '%'),
                        Color.FromArgb(235, 235, 235)));
                _totalEff.Add(s2,
                    GetEfficiencyColor(s2.Text.Trim('=', '%'),
                        Color.FromArgb(235, 235, 235)));
                _totalEff.Add(s3,
                    GetEfficiencyColor(s3.Text.Trim('=', '%'),
                        Color.FromArgb(235, 235, 235)));

                _totalEff.Add(total,
                    GetEfficiencyColor(total.Text.Split('%')[0],
                        Color.LightGray));
                break;
            case "cleaner":
                _totalEff.Add(s1, Color.FromArgb(54, 215, 86));
                _totalEff.Add(s2, Color.FromArgb(54, 215, 86));
                _totalEff.Add(s3, Color.FromArgb(54, 215, 86));
                _totalEff.Add(total, Color.FromArgb(54, 215, 86));
                break;
        }
    }

    private static Color GetEfficiencyColor(string text, Color optColor)
    {
        var color = default(Color);
        double.TryParse(text, out var eff);
        if (eff == 0)
            color = optColor;
        else if
            (eff > 0 && eff <= 85.0) color = Color.FromArgb(253, 129, 127);
        else if
            (eff > 85.0 && eff <= 90.0) color = Color.FromArgb(254, 215, 1);
        else if
            (eff > 90.0) color = Color.FromArgb(54, 214, 87);
        else
            color = optColor;

        return color;
    }

    private Dictionary<string, int> _dictCleaners = new Dictionary<string, int>();
    private string GetMachinesDataInRange(IEnumerable<Label> lst, int fromMac, int toMac,int round, bool isBlock)
    {
        //string tmpStr;
        //var value = 0.0;

        //_dictCleaners = new Dictionary<string, int>();

        //foreach (var item in lst)
        //{
        //    var machineRegNumb = Convert.ToInt32(item.ID.Remove(0, 1));
        //    if (machineRegNumb < fromMac || machineRegNumb > toMac) continue;

        //    if (item.Text != string.Empty)
        //    {
        //        int.TryParse(item.Text.Split('/')[1], out var jVal);
        //        value += jVal;
        //        var nameInit = item.Text.Split('/')[0];

        //        if (!_dictCleaners.ContainsKey(nameInit))
        //            _dictCleaners.Add(nameInit, jVal);
        //        else
        //            _dictCleaners[nameInit] += jVal;
        //    }
        //}

        //var days = (Get_to_date().Subtract(Get_from_date())).TotalDays;

        //if (Math.Abs(days) < 0 && isBlock)
        //{
        //    var sb = new StringBuilder();
        //    foreach (var k in _dictCleaners)
        //    {
        //        sb.Append(k.Key
        //                  + "=" +
        //                  (days + 1).ToString(CultureInfo.CurrentCulture)
        //                  + "/" +
        //                  k.Value.ToString(CultureInfo.CurrentCulture) + "   ");
        //    }

        //    tmpStr = sb.ToString();
        //}
        //else
        //{
        //    tmpStr = (days + 1).ToString(CultureInfo.InvariantCulture)
        //             + "/" +
        //             value.ToString(CultureInfo.InvariantCulture);
        //}

        //return tmpStr.ToString(CultureInfo.CurrentCulture);
        var tmpStr = "";
        var value = 0.0;
        var counter = 0;
        int totMachineData = 0;

        _dictCleaners = new Dictionary<string, int>();

        foreach (var item in lst)
        {
            if (item.BackColor == Color.DimGray
                 && item.BackColor == Color.LightGray) continue;
            if (item.Text == string.Empty) continue;
            var machineRegNumb = Convert.ToInt32(item.ID.Remove(0, 1));
            if (machineRegNumb < fromMac || machineRegNumb > toMac) continue;
            counter++;

            switch (_mod)
            {
                //case "tempStd":
                //    if (!item.Text.Contains(":")) continue;
                //    int.TryParse(item.Text.Split(':')[0], out int hours);
                //    int.TryParse(item.Text.Split(':')[1], out int minutes);
                //    tmpStr = CumulateHHmm(hours, minutes);
                //    break;
                case "cleaner":
                    if (item.Text != string.Empty)
                    {
                        int.TryParse(item.Text.Split('/')[1], out var jVal);
                        value += Convert.ToDouble(jVal);
                        var nameInit = item.Text.Split('/')[0];

                        if (!_dictCleaners.ContainsKey(nameInit))
                            _dictCleaners.Add(nameInit, jVal);
                        else
                            _dictCleaners[nameInit] += jVal;
                    }

                    break;
                default:
                    {
                        if (_mod != "velo" && _mod != "cleaner" && _mod != "temperature")
                        {
                            double.TryParse(item.Text, out var result);
                            value += result;
                        }
                        else
                            value = 0;

                        break;
                    }
            }
        }
        //hours = 0; minutes = 0;

        switch (_mod)
        {
            case "eff":
                if (counter != 0)
                    tmpStr = "=" + Math.Round(value / counter, round).ToString() + "%";
                break;
            case "qty":
                tmpStr = value.ToString();
                break;
            case "cleaner":
                var days = (Get_to_date().Subtract(Get_from_date())).TotalDays;

                if (days == 0 && isBlock)
                {
                    var sb = new StringBuilder();

                    foreach (var k in _dictCleaners)
                    {
                        totMachineData += k.Value;
                        sb.AppendLine(k.Key.ToString() + "=" + (days + 1).ToString(CultureInfo.CurrentCulture)
                                      + "/" + k.Value.ToString(CultureInfo.CurrentCulture));
                    }

                    sb.AppendLine("Tot.=" + (days + 1).ToString(CultureInfo.CurrentCulture) + "/" + totMachineData.ToString());
                    tmpStr = sb.ToString();
                }
                else
                {
                    tmpStr = (days + 1).ToString(CultureInfo.CurrentCulture) + "/" + value.ToString(CultureInfo.CurrentCulture);
                }
                break;
            case "rammendi":
                if (counter != 0)
                    tmpStr = "=" + value.ToString() + "%";
                break;
            case "scarti":
                if (counter != 0)
                    tmpStr = "=" + value.ToString() + "%";
                break;
            case "temperature":
                value = 0;
                break;
        }
        return tmpStr.ToString(CultureInfo.CurrentCulture);
    }
    //TEXT MACHINE CHANGED
    protected void lblmachine_TextChanged(object sender, EventArgs e)
    {
        //ClientScript.RegisterStartupScript(GetType(), "Javascript", "javascript:initSomething(); ", true);

        //btnConfirm.Enabled = true;


    }
    protected void btnAlarm_Click(object sender, EventArgs e)
    {
        tester.Text = IdJob.ToString() +" " + GetStartJob.ToString("yyyy/MM/dd HH:mm:ss tt", CultureInfo.InvariantCulture);

    }
    protected void table_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            LinkButton objDelete = e.Row.Cells[0].Controls[0] as LinkButton;

             objDelete.Attributes.Add("class", "btn_delete_click");

         }
    }
     
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        string ID = Grid_table.DataKeys[e.RowIndex].Value.ToString();
        long l1;
        long.TryParse(ID, out l1);

        if (Request.QueryString["JobType"] == "PULIZIA ORDINARIA")
        {
            _client1.DeactivateAlarm(l1, "cleaning", lblName.Text);
            Grid_table.DataSource = _client1.GetAlmTable("cquality");
            Grid_table.DataBind();

        }
        else if (Request.QueryString["JobType"] == "CQ")
        {
            _client1.DeactivateAlarm(l1, "cquality", lblName.Text);
            Grid_table.DataSource = _client1.GetAlmTable("cquality");
            Grid_table.DataBind();
        }

          
        ClientScript.RegisterClientScriptBlock(GetType(), "Javascript", "javascript:initAllarms(); ", true);

        Grid_table.DataSource = _client1.GetAlmTable("cquality");
        Grid_table.DataBind();

        panel_home_labels.Visible = true;
        panel_home.Visible = false;
        panel_alarm.Visible = true;
        panel_cqlayout.Visible = false;
        panel_quality.Visible = false;

        idhome.Attributes.Remove("class");
        id_layout_cq_eff.Attributes.Remove("class");
        id_layout_cq_pulizia.Attributes.Remove("class");
        id_layout_cq_cq.Attributes.Remove("class");
        id_layout_cq_alarms.Attributes.Add("class", "active");
        id_layout_pul_pul.Attributes.Remove("class");
        id_layout_pul_alarms.Attributes.Remove("class");


    }
    protected void Timer1_Tick(object sender, EventArgs e)
    {
        btnConfirm.Enabled = false;
        btnStop.Enabled = true;
        btnLogout.Enabled = false;
        lblnote.Enabled = true;
        btnAlarm.Enabled = true;

        var start = Convert.ToDateTime(tester.Text.Split(' ')[1] + " " +
            tester.Text.Split(' ')[2] + " " + tester.Text.Split(' ')[3]);
        var tempo = _client1.GetTimeStampInMM(start, DateTime.Now);
        _client1.UpdateOperation3(DateTime.Now, IdJob, "", Convert.ToInt32(tempo));
    }

    protected void aggiorna_Click(object sender, EventArgs e)
    {
        _mod = lblMod.Text;
        _cleanersType = lblCleanersType.Text;
        CallProcedures(_mod);
    }
     
    protected void getAlm_Click(object sender, EventArgs e)
    {
        //if (Request.QueryString["JobType"] == "CQ")
        //{ 
        //    Grid_table.DataSource = _client1.GetAlmTable("cquality");
        //    Grid_table.DataBind();

        //} else if (Request.QueryString["JobType"] == "PULIZIA ORDINARIA")
        //{
           
        //    Grid_table.DataSource = _client1.GetAlmTable("cleaning");
        //    Grid_table.DataBind();

        //}

    }
 

    protected void aggiorna_eff_Click(object sender, EventArgs e)
    {
        _mod = "eff";
        CallProcedures(_mod);

        panel_home_labels.Visible = true;
        panel_home.Visible = false;
        panel_alarm.Visible = false;
        panel_cqlayout.Visible = true;
        panel_quality.Visible = false;
        idhome.Attributes.Remove("class");
    }

    protected void aggiorna_pul_Click(object sender, EventArgs e)
    {
        _cleanersType = "PULIZIA ORDINARIA";
        _mod = "cleaner";
        CallProcedures(_mod);

        panel_home_labels.Visible = true;
        panel_home.Visible = false;
        panel_alarm.Visible = false;
        panel_cqlayout.Visible = true;
        panel_quality.Visible = false;
    }

    protected void aggiorna_cq_Click(object sender, EventArgs e)
    {
        _cleanersType = "cquality";
        _mod = "cleaner";
        CallProcedures(_mod);

        panel_home_labels.Visible = true;
        panel_home.Visible = false;
        panel_alarm.Visible = false;
        panel_cqlayout.Visible = true;
        panel_quality.Visible = false;
    }

    protected void button_home_OnClick(object sender, EventArgs e)
    {
        panel_home_labels.Visible = true;
        panel_home.Visible = true;
        panel_alarm.Visible = false;
        panel_cqlayout.Visible = false;
        panel_quality.Visible = false;
        panel_myjob.Visible = false;

        panel_email.Visible = false;

        idhome.Attributes.Add("class", "active");
        id_layout_cq_eff.Attributes.Remove("class");
        id_layout_cq_pulizia.Attributes.Remove("class");
        id_layout_cq_cq.Attributes.Remove("class");
        id_layout_cq_alarms.Attributes.Remove("class");
        id_layout_pul_pul.Attributes.Remove("class");
        id_layout_pul_alarms.Attributes.Remove("class");
        id_myJob.Attributes.Remove("class");


        if (txtMachina.Enabled == false && lblJobType.Text=="CQ")
        {
            panel_quality.Visible = true;
        }


       
    }

    protected void button_lay_eff_OnClick(object sender, EventArgs e)
    {
        _mod = "eff";
        CallProcedures(_mod);

        panel_home_labels.Visible = true;
        panel_home.Visible = false;
        panel_alarm.Visible = false;
        panel_cqlayout.Visible = true;
        panel_quality.Visible = false;
        panel_myjob.Visible = false;

        panel_email.Visible = false;

        aggiorna_eff.Visible = true;
        aggiorna_cq.Visible = false;
        aggiorna_pul.Visible = false;

        idhome.Attributes.Remove("class");
        id_layout_cq_eff.Attributes.Add("class", "active");
        id_layout_cq_pulizia.Attributes.Remove("class");
        id_layout_cq_cq.Attributes.Remove("class");
        id_layout_cq_alarms.Attributes.Remove("class");
        id_layout_pul_pul.Attributes.Remove("class");
        id_layout_pul_alarms.Attributes.Remove("class");
        id_myJob.Attributes.Remove("class");

    }

    protected void button_lay_pul_OnClick(object sender, EventArgs e)
    {
        _cleanersType = "PULIZIA ORDINARIA";
        _mod = "cleaner";
        CallProcedures(_mod);

        panel_home_labels.Visible = true;
        panel_home.Visible = false;
        panel_alarm.Visible = false;
        panel_cqlayout.Visible = true;
        panel_quality.Visible = false;
        panel_myjob.Visible = false;

        panel_email.Visible = false;

        aggiorna_eff.Visible = false;
        aggiorna_cq.Visible = false;
        aggiorna_pul.Visible = true;

        idhome.Attributes.Remove("class");
        id_layout_cq_eff.Attributes.Remove("class");
        id_layout_cq_pulizia.Attributes.Add("class", "active");
        id_layout_cq_cq.Attributes.Remove("class");
        id_layout_cq_alarms.Attributes.Remove("class");
        id_layout_pul_pul.Attributes.Remove("class");
        id_layout_pul_alarms.Attributes.Remove("class");
        id_myJob.Attributes.Remove("class");

    }

    protected void button_lay_cq_OnClick(object sender, EventArgs e)
    {
        _cleanersType = "cquality";
        _mod = "cleaner";
        CallProcedures(_mod);


        panel_home_labels.Visible = true;
        panel_home.Visible = false;
        panel_alarm.Visible = false;
        panel_cqlayout.Visible = true;
        panel_quality.Visible = false;
        panel_myjob.Visible = false;

        panel_email.Visible = false;

        aggiorna_eff.Visible = false;
        aggiorna_cq.Visible = true;
        aggiorna_pul.Visible = false;

        idhome.Attributes.Remove("class");
        id_layout_cq_eff.Attributes.Remove("class");
        id_layout_cq_pulizia.Attributes.Remove("class");
        id_layout_cq_cq.Attributes.Add("class", "active");
        id_layout_cq_alarms.Attributes.Remove("class");
        id_layout_pul_pul.Attributes.Remove("class");
        id_layout_pul_alarms.Attributes.Remove("class");
        id_myJob.Attributes.Remove("class");

    }

    protected void button_cq_alarms_OnClick(object sender, EventArgs e)
    {

        Grid_table.DataSource = _client1.GetAlmTable("cquality");
        Grid_table.DataBind();

        panel_home_labels.Visible = true;
        panel_home.Visible = false;
        panel_alarm.Visible = true;
        panel_cqlayout.Visible = false;
        panel_quality.Visible = false;
        panel_myjob.Visible = false;

        panel_email.Visible = false;

        idhome.Attributes.Remove("class");
        id_layout_cq_eff.Attributes.Remove("class");
        id_layout_cq_pulizia.Attributes.Remove("class");
        id_layout_cq_cq.Attributes.Remove("class");
        id_layout_cq_alarms.Attributes.Add("class", "active");
        id_layout_pul_pul.Attributes.Remove("class");
        id_layout_pul_alarms.Attributes.Remove("class");
        id_myJob.Attributes.Remove("class");

       
            Grid_table.DataSource = _client1.GetAlmTable("cquality");
            Grid_table.DataBind();
       
         

    }

    protected void button_pul_alarms_OnClick(object sender, EventArgs e)
    {
        panel_home_labels.Visible = true;
        panel_home.Visible = false;
        panel_alarm.Visible = true;
        panel_cqlayout.Visible = false;
        panel_quality.Visible = false;
        panel_myjob.Visible = false;

        panel_email.Visible = false;

        idhome.Attributes.Remove("class");
        id_layout_cq_eff.Attributes.Remove("class");
        id_layout_cq_pulizia.Attributes.Remove("class");
        id_layout_cq_cq.Attributes.Remove("class");
        id_layout_cq_alarms.Attributes.Remove("class");
        id_layout_pul_pul.Attributes.Remove("class");
        id_layout_pul_alarms.Attributes.Add("class", "active");
        id_myJob.Attributes.Remove("class");

        
            Grid_table.DataSource = _client1.GetAlmTable("cleaning");
            Grid_table.DataBind();
  
    }

    protected void button_mail_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        panel_email.Visible = true;
    }

    protected void button_closeMail_Click(object sender, EventArgs e)
    {
        panel_email.Visible = false;
    }

    protected void button_sendMail_Click(object sender, EventArgs e)
    { 
        SendEmail();
    }

    private void InsertPicToDb(HttpPostedFile htPost)
    {
        using (var c = new SqlConnection(ConString))
        {
            int length = htPost.ContentLength;
            byte[] imagebyt = new byte[length];
            HttpPostedFile img = htPost;
            img.InputStream.Read(imagebyt, 0, length);
            SqlCommand cmd = new SqlCommand("insert into imgresource(idjob,img) values (@param1,@image)", c);
            cmd.Parameters.Add("@param1", SqlDbType.BigInt).Value = IdJob;
            cmd.Parameters.Add("@image", SqlDbType.VarBinary).Value = imagebyt;
            c.Open();
            cmd.ExecuteNonQuery();
            c.Close();
        }
    }
     
    private void SendEmail()
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("noreply@olimpias.rs", "CQ Notify - New Message - '" + DateTime.Now.ToShortDateString() + "'");
        mail.To.Add(txt_mailTo.Text);
        mail.Subject = txt_subject.Text;
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("<span>"+txt_mailMessage.Text+"</span>");
        //sb.AppendLine("<table style='font-family: arial;'>");
        //sb.AppendLine("<tr>");
        //sb.AppendLine("<td colspan='2' style='font-weight:600;color: #293d61;font-size: 14pt;'> ASSENTEISMO GIORNALIERO </td>");
        //sb.AppendLine("<td colspan='1' style='color: #334c79;'> -Oliknit </td>'");
        //sb.AppendLine("<td colspan='2' style='float:right;text-align:right;font-size: 8pt;font-weight: 600;'> Anno 2019 </td>");
        //sb.AppendLine("</tr>");
        //sb.AppendLine("<tr style='background: #cecece;line-height: 22px;'>");
        //sb.AppendLine("<td colspan='2'></td>");
        //sb.AppendLine("<td colspan='1'></td>");
        //sb.AppendLine("<td colspan='2' style='font-size:10pt;color:red;text-align:right;padding-right:5px;font-weight:600;'>" + DateTime.Now.ToShortDateString() + "</td>");
        //sb.AppendLine("</tr>");
        //sb.AppendLine("<tr style='line-height:30px;background: #f0fafd;'>");
        //sb.AppendLine("<td colspan='2' style='font-weight: 600;color:red;padding-left: 5px;vertical-align: middle;'> REPARTO </td>");
        //sb.AppendLine("<td colspan='1' style='color:red;font-size: 11pt;font-weight: 600;padding-left: 5px;padding-right: 5px;'>% assenteismo</td>");
        //sb.AppendLine("<td colspan='2' style='color:red;font-size: 11pt;font-weight: 600;padding-left: 5px;padding-right: 5px;'>nr persone assenti</td>");
        //sb.AppendLine("</tr>");
        //sb.AppendLine("<tr style='background:#d6ebfb;line-height: 25px;'>");
        //sb.AppendLine("<td colspan='2' style='padding-left: 5px;font-weight: 600;'>Confezione A </td>");
        //sb.AppendLine("<td colspan='1' style='text-align:center; font-weight: 600;'>" + _perConfA + "%</td>");
        //sb.AppendLine("<td colspan='2' style='text-align:center; font-weight: 600;'>" + _assConfA + "</td>");
        //sb.AppendLine("</tr>");
        //sb.AppendLine("<tr style='line-height: 25px;background: #f0fafd;'>");
        //sb.AppendLine("<td colspan='2' style='padding-left: 5px;font-weight: 600;'>Confezione B</td>");
        //sb.AppendLine("<td colspan='1' style='text-align:center; font-weight: 600;'>" + _perConfB + "%</td>");
        //sb.AppendLine("<td colspan='2' style='text-align:center; font-weight: 600;'>" + _assConfB + "</td>");
        //sb.AppendLine("</tr>");
        //sb.AppendLine("<tr style='background: #d6ebfb;line-height: 25px;'>");
        //sb.AppendLine("<td colspan='2' style='padding-left: 5px;font-weight: 600;'> Stiro </td>");
        //sb.AppendLine("<td colspan='1' style='text-align:center;font-weight: 600;'>" + _perStiro + "%</td>");
        //sb.AppendLine("<td colspan='2' style='text-align:center;font-weight: 600;'>" + _assStiro + "</td>");
        //sb.AppendLine("</tr>");
        //sb.AppendLine("<tr style='line-height: 25px;background: #f0fafd;'>");
        //sb.AppendLine("<td colspan='2' style='padding-left: 5px;font-weight: 600;'>Amministrazione</td>");
        //sb.AppendLine("<td colspan='1' style='text-align:center; font-weight: 600;'>" + _perAmmini + "%</td>");
        //sb.AppendLine("<td colspan='2' style='text-align:center; font-weight: 600;'>" + _assAmmini + "</td>");
        //sb.AppendLine("</tr>");
        //sb.AppendLine("<tr style='line-height:30px;background:#acd7f7;'>");
        //sb.AppendLine("<td colspan='3' style='padding-left:5px;font-weight: 600;'>Totale</td>");
        //sb.AppendLine("<td colspan='2' style='text-align:center;font-weight: 600;'>" + _totale + "</td>");
        //sb.AppendLine("</tr>");
        //sb.AppendLine("</table>");
        sb.AppendLine("</body>");
        sb.AppendLine("</html>");

        try
        {
            //foreach (HttpPostedFile postedFile in file_attach.PostedFiles)
            //{
            //    string fileName = Path.GetFileName(postedFile.FileName);
            //    postedFile.SaveAs(Server.MapPath("~/Images/Upload/Images/") + IdJob + "_" + fileName);
            //    InsertPicToDb(postedFile);

            //    //mail.Attachments.Add(file_attach);
            //}

            if (file_attach.HasFile)
            {
                foreach (HttpPostedFile file in file_attach.PostedFiles)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Images/Upload/Images/Email") + fileName);
                    mail.Attachments.Add(new Attachment(file.InputStream, fileName));
                }
            }

        }
        catch (Exception)
        {

        }


        mail.Body = sb.ToString();
        mail.IsBodyHtml = true;
        SmtpClient smtp = new SmtpClient("mail.olimpias.it");
        smtp.Port = 25;
        smtp.Send(mail);
        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Email sent.');", true);

    }


    private void SendEmailStart()
    {

       
        

        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("noreply@olimpias.rs", "CQ Notify - From: '"+ lblName.Text + "' - '" + DateTime.Now.ToShortDateString() + "'");
         
        var allEmails = eb.ToString().TrimEnd(',').Split(',');
        mail.To.Add("mpanic@olimpias.rs");
        
        foreach (var item in allEmails)
        {
            mail.CC.Add(item);
        }
         

        mail.Subject = "CQ Report";
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("<table>");
        //1
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Machina:</td>");
        sb.AppendLine("<td>"+ lbl_machina.Text + "</td>");
        sb.AppendLine("</tr>");
        //2
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Commessa:</td>");
        sb.AppendLine("<td>" + txt_commessa.Text + "</td>");
        sb.AppendLine("</tr>");
        //3
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Articolo:</td>");
        sb.AppendLine("<td>" + lbl_articolo.Text + "</td>");
        sb.AppendLine("</tr>");
        //4
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Componente:</td>");
        sb.AppendLine("<td>" + lbl_componente.Text + "</td>");
        sb.AppendLine("</tr>");
        //5
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Taglia:</td>");
        sb.AppendLine("<td>" + lbl_taglia.Text + "</td>");
        sb.AppendLine("</tr>");
        //6
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Colore:</td>");
        sb.AppendLine("<td>" + txt_colore.Text + "</td>");
        sb.AppendLine("</tr>");
        //7
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Cotta:</td>");
        sb.AppendLine("<td>" + txt_cotta.Text + "</td>");
        sb.AppendLine("</tr>");
        //8
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Progressivo:</td>");
        sb.AppendLine("<td>" + lbl_progressivo.Text + "</td>");
        sb.AppendLine("</tr>");
        //9 BR Elasticita
        sb.AppendLine("<tr>");
        sb.AppendLine("<td><b>Elasticita</b></td>");
        sb.AppendLine("</tr>");

        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Aghi:</td>");
        sb.AppendLine("<td>" + txt_aghi.Text + "</td>");
        sb.AppendLine("</tr>");
        //10
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Ranghi:</td>");
        sb.AppendLine("<td>" + txt_ranghi.Text + "</td>");
        sb.AppendLine("</tr>");
        //11 br Missure
        sb.AppendLine("<tr>");
        sb.AppendLine("<td><b>Misure</b></td>");
        sb.AppendLine("</tr>");

        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Lunghezza:</td>");
        sb.AppendLine("<td>" + txt_lunghezza.Text + "</td>");
        sb.AppendLine("</tr>");
        //12
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Langhezza:</td>");
        sb.AppendLine("<td>" + txt_langhezza.Text + "</td>");
        sb.AppendLine("</tr>");
        //13 br Bordo/Polso/Collo
        sb.AppendLine("<tr>");
        sb.AppendLine("<td><b>Bordo/Polso/Collo</b></td>");
        sb.AppendLine("</tr>");

        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Elasticita:</td>");
        sb.AppendLine("<td>" + txt_elasticita.Text + "</td>");
        sb.AppendLine("</tr>");
        //14
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Altezza:</td>");
        sb.AppendLine("<td>" + txt_altezza.Text + "</td>");
        sb.AppendLine("</tr>");
        //15
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Catanelle:</td>");
        sb.AppendLine("<td>" + txt_catanelle.Text + "</td>");
        sb.AppendLine("</tr>");
        //16
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Cimose:</td>");
        sb.AppendLine("<td>" + txt_cimose.Text + "</td>");
        sb.AppendLine("</tr>");
        //17
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Sx:</td>");
        sb.AppendLine("<td>" + txt_sx.Text + "</td>");
        sb.AppendLine("</tr>");
        //18
        sb.AppendLine("<tr>");
        sb.AppendLine("<td>Note:</td>");
        sb.AppendLine("<td>" + txt_note.Text + "</td>");
        sb.AppendLine("</tr>");

        sb.AppendLine("</table>");

        sb.AppendLine("</body>");
        sb.AppendLine("</html>");


        try
        { 
            if (Attach_to_mail.HasFile)
            {
                foreach (HttpPostedFile postedFile in Attach_to_mail.PostedFiles)
                {
                    string fileName = Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(Server.MapPath("~/Images/Upload/Images/") + IdJob + "_" + fileName);
                    InsertPicToDb(postedFile);
                }

                foreach (HttpPostedFile file in Attach_to_mail.PostedFiles)
                {
                    string fileName = Path.GetFileName(file.FileName);
                    file.SaveAs(Server.MapPath("~/Images/Upload/Images/Email") + fileName);
                    mail.Attachments.Add(new Attachment(file.InputStream, fileName));
                }
            }

        }
        catch (Exception)
        {

        }

        mail.Body = sb.ToString();
        mail.IsBodyHtml = true;
        SmtpClient smtp = new SmtpClient("mail.olimpias.it");
        smtp.Port = 25;
        smtp.Send(mail);
        ClientScript.RegisterStartupScript(this.GetType(), "alert", "alert('Email sent.');", true);

    }

    protected void cb_esito_CheckedChanged(object sender, EventArgs e)
    {
        if (cb_esito.Checked == true)
        { 
            txt_esito.Text = "OK";
            panel_quality.Visible = true;
            btnAlarm.Visible = false;
            panel_home_labels.Visible = false;
        }
        else
        { 
            txt_esito.Text = "";
            panel_quality.Visible = true;
            btnAlarm.Visible = false;
            panel_home_labels.Visible = false;
        }
    }

    protected void btn_closeImage_Click(object sender, EventArgs e)
    { 
        panel_image.Visible = false;
        panel_home_labels.Visible = false;
        panel_quality.Visible = true;
    }

    protected void btn_sendImage_Click(object sender, EventArgs e)
    {
        try
        {
            foreach (HttpPostedFile postedFile in File_image.PostedFiles)
            {
                string fileName = Path.GetFileName(postedFile.FileName);
                postedFile.SaveAs(Server.MapPath("~/Images/Upload/Images/") + IdJob + "_" + fileName);

                InsertPicToDb(postedFile);
            }
            panel_image.Visible = false;
            panel_home_labels.Visible = false;
            panel_quality.Visible = true;
        }
        catch (Exception)
        {
            lbl_error_image.Text = "Please start Job First, then you can upload images.";
        }
    }

    protected void button_image_Click(object sender, System.Web.UI.ImageClickEventArgs e)
    {
        panel_image.Visible = true;
    }

    protected void button_myJob_Click(object sender, EventArgs e)
    {
        panel_home_labels.Visible = false;
        panel_home.Visible = false;
        panel_alarm.Visible = false;
        panel_cqlayout.Visible = false;
        panel_quality.Visible = false;
        panel_myjob.Visible = true;

        panel_email.Visible = false;

        idhome.Attributes.Remove("class");
        id_layout_cq_eff.Attributes.Remove("class");
        id_layout_cq_pulizia.Attributes.Remove("class");
        id_layout_cq_cq.Attributes.Remove("class");
        id_layout_cq_alarms.Attributes.Remove("class");
        id_layout_pul_pul.Attributes.Remove("class");
        id_layout_pul_alarms.Attributes.Remove("class");
        id_myJob.Attributes.Add("class", "active");

        if (Request.QueryString["JobType"] == "CQ")
        {
        
            gv_myJob.DataSource = _client1.MachineTable(lblOpId.Text, DateTime.Now, _client1.GetShift());
            gv_myJob.DataBind();
        }
        else if (Request.QueryString["JobType"] == "PULIZIA ORDINARIA")
        {
      
            gv_myJob.DataSource = _client1.MachineTable(lblOpId.Text, DateTime.Now, _client1.GetShift());
            gv_myJob.DataBind();
        }
    }

    protected void btn_emailToSend_Click(object sender, EventArgs e)
    {

        var ds = _client1.GetEmailInfo();
        eb = new StringBuilder();

        foreach (DataRow data in ds.Tables[0].Rows)
        {
            eb.Append(data.ItemArray.GetValue(1).ToString() + ",");
        }


        if (eb.Length > 0) { 
        SendEmailStart();
        }

        if (Request.QueryString["JobType"] == "CQ")
        {
            panel_quality.Visible = true;
            btnAlarm.Visible = false;
            panel_home_labels.Visible = false;
        }

        }

    protected void ddp_mail_list_TextChanged(object sender, EventArgs e)
    {
    

        if (lblJobType.Text == "CQ")
        {
            panel_quality.Visible = true;
            btnAlarm.Visible = false;
            panel_home_labels.Visible = false;
        }
        else
        {
            panel_quality.Visible = false;
            btnAlarm.Visible = true;
            panel_home_labels.Visible = true;
        }

    }

    protected void load_machines_Click(object sender, EventArgs e)
    {
       // var _machines;


       var _machines = _client1.GetLineMachines(txt_linea.Text);

      

        foreach (var item in _machines)
        {
            var span = new HtmlGenericControl("span");
            span.Attributes["class"] = "nonExpense";
            span.InnerHtml = ""+item.Code+"<br/>"+item.Efficiency+"";
            panel_operatori.Controls.Add(span);
        }

        panel_home.Visible = true;
        panel_home_labels.Visible = false;
        panel_operatori.Visible = true;

        //GetLineMachines
    }
}


public class CleanedMachines
{
    public int MachineNumber { get; set; }
    public int CleanedHours { get; set; }
    public DateTime EventDate { get; set; }
    public CleanedMachines() { }
    public CleanedMachines(int macNum, int clndHrs, DateTime evDate)
    {
        MachineNumber = macNum;
        CleanedHours = clndHrs;
        EventDate = evDate;
    }
}

public class Cleaners
{
    public string Machine { get; set; }
    public string NameInitials { get; set; }
    public int Prodgen { get; set; }
    public Cleaners(string machine, string nameinit, int prodgen)
    {
        Machine = machine;
        NameInitials = nameinit;
        Prodgen = prodgen;

    }
}
  
 