using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using wcfcleaners;

namespace TessituraCleaners
{
    public partial class LoginScanner : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            lblErr.Visible = false;
            //var dBind = new DefaultBindings();
            //dBind.InitializeServiceClient();
            ////_client = DefaultBindings.Client;
        }

        protected void LoginClick(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtPinCode.Text)) return;

            var s = new Service1();

            var user = s.GetOperatorName(txtPinCode.Text);

            if (string.IsNullOrEmpty(user))
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
 
                Response.Redirect("./Work.aspx");
                lblErr.Visible = false;
            }
        }
    }
}