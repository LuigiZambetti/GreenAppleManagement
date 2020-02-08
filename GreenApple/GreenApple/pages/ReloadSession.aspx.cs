using System;
using System.Web;

namespace Green.Apple.Management
{
    public partial class ReloadSession : GREEN_BasePage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);
            lblInfo.Text = "";
        }
        protected void lnkRefresh_Click(object sender, EventArgs e)
        {
            try
            {
                lblInfo.Text = "Session : " + ((clsSession)HttpContext.Current.Session["GreenApple"]).AzioneCorrente + "<BR>" + DateTime.Now.Day.ToString() + "/" + DateTime.Now.Month.ToString() + "/" + DateTime.Now.Year.ToString() + " " + DateTime.Now.Hour.ToString() + ":" + DateTime.Now.Minute.ToString() + ":" + DateTime.Now.Second.ToString();
            }
            catch
            {
                lblInfo.Text = "Sessione ancora non esistente";
            }
        }
    }
}
