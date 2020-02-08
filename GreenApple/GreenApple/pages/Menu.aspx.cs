using System;
using System.Web.UI;

namespace Green.Apple.Management
{
    public partial class Menu : GREEN_BasePage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            Control myUserControl;
            string action = "Green_Admin_Menu.ascx";
            myUserControl = LoadControl(clsCostanti.RootUserControls + action);
            phContent1.Controls.Add(myUserControl);
        }
    }
}