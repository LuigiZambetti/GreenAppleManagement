using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Green.Apple.Management
{
    public partial class Utenti : GREEN_BasePage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (((clsSession)Session["GreenApple"]).IDUtente == "0") return;

            
            System.Web.UI.Control myUserControl;
            string action = "Green_Admin_Utenti.ascx";
            myUserControl = LoadControl(clsCostanti.RootUserControls + action);
            this.phContent1.Controls.Add(myUserControl);
        }
    }
}