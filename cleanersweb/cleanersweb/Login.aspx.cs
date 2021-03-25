using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wcfcleaners;
public partial class Login : System.Web.UI.Page
{
    private ServiceReference1.Service1Client s = new ServiceReference1.Service1Client();
   // private wcfcleaners.Service1 s = new wcfcleaners.Service1();

    protected void Page_Load(object sender, EventArgs e)
    {

        lblErr.Visible = false;

        ClearCacheItems();
    }
    public void ClearCacheItems()
    {
        List<string> keys = new List<string>();
        IDictionaryEnumerator enumerator = Cache.GetEnumerator();

        while (enumerator.MoveNext())
            keys.Add(enumerator.Key.ToString());

        for (int i = 0; i < keys.Count; i++)
            Cache.Remove(keys[i]);
    }
    public static string _operatorName = string.Empty;
    public static string _jobType = string.Empty;
    public static string _operatorId = string.Empty;

    protected void LoginClick(object sender, EventArgs e)
    {
        if (string.IsNullOrEmpty(txtPinCode.Text)) return;

        //var s = new Service1();
        s = new ServiceReference1.Service1Client();
        var user = s.LoginOperator(txtPinCode.Text);

        _operatorName = user.FullName;
        _jobType = user.JobType;
        _operatorId = user.Id.ToString();

        if (string.IsNullOrEmpty(_operatorName))
        {
            lblErr.Visible = true;
        }
        else
        {
            Session["USER"] = user;
            HttpCookie myCookie = new HttpCookie("MyTestCookie");
            DateTime now = DateTime.Now;

            // Set the cookie value.
            myCookie.Value = now.ToString();
            // Set the cookie expiration date.
            myCookie.Expires = now.AddMinutes(3600);

            // Add the cookie.
            Response.Cookies.Add(myCookie);

            Response.Redirect("./Work.aspx?JobType=" + _jobType + "&Name=" + _operatorName + "&OpId=" + _operatorId);
            lblErr.Visible = false;
        }
    } 
}
