using System;
using System.Web.UI;

namespace Green.Apple.Management
{
    public partial class ModalitaPagamento : GREEN_BasePage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (((clsSession)Session["GreenApple"]).IDUtente == "0") 
                return;

            Control myUserControl;
            string action = "Green_Admin_ModalitaPagamento.ascx";
            myUserControl = LoadControl(clsCostanti.RootUserControls + action);
            phContent1.Controls.Add(myUserControl);
        }
    }
}