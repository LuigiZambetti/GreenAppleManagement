using System;
using System.Web.UI;

namespace Green.Apple.Management
{
    public partial class pages_Home : GREEN_BasePage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if(Session["GreenApple"]==null) 
                Session["GreenApple"] = new clsSession();

            Control myUserControl;
            string action = "Green_Home_Cruscotto.ascx";
            myUserControl = LoadControl(clsCostanti.RootUserControls + action);
            phContent1.Controls.Add(myUserControl);
        }
    }
}