using System;
using System.Web.UI;

namespace Green.Apple.Management
{
    public partial class Utenti : GREEN_BasePage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (((clsSession)Session["GreenApple"]).IDUtente == "0") 
                return;

            Control myUserControl;
            string action = "Green_Admin_Utenti.ascx";
            myUserControl = LoadControl(clsCostanti.RootUserControls + action);
            phContent1.Controls.Add(myUserControl);
        }
    }
}