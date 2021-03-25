using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Email : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SendEmail();
    }

    private void SendEmail()
    {
        MailMessage mail = new MailMessage();
        mail.From = new MailAddress("noreply@olimpias.rs", "CQ Notify - '" + DateTime.Now.ToShortDateString() + "'");
        mail.To.Add("pnikolic@olimpias.rs");
        mail.Subject = "Subject Text";
        StringBuilder sb = new StringBuilder();

        sb.AppendLine("<html>");
        sb.AppendLine("<head>");
        sb.AppendLine("</head>");
        sb.AppendLine("<body>");
        sb.AppendLine("<span>test mail</span>");
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

        mail.Body = sb.ToString();
        mail.IsBodyHtml = true;
        SmtpClient smtp = new SmtpClient("mail.olimpias.it");
        smtp.Port = 25;
        smtp.Send(mail);
    }
}