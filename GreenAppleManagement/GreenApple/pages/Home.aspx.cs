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

    public partial class pages_Home : GREEN_BasePage
    {
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if(Session["GreenApple"]==null) Session["GreenApple"] = new clsSession();

            System.Web.UI.Control myUserControl;
            string action = "Green_Home_Cruscotto.ascx";
            myUserControl = LoadControl(clsCostanti.RootUserControls + action);
            this.phContent1.Controls.Add(myUserControl);
            
        }
    }
}