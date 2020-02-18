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
    public partial class Green_Home_Cruscotto : Green_BaseUserControl
    {

        #region DECLARATIONS
        clsPagining objPaginingSearch = new clsPagining();
        #endregion

        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e); 

            if (((clsSession)Session["GreenApple"]).IDUtente != "0")
            {
                CaricaNews();
                CaricaReport();
                lblTitolo2.Text = "GREEN APPLE MANAGEMENT - Reportistica";
                pnlLogin.Visible = false;
            }
            else
            { 
                //Carico Login
                lblTitolo2.Text = "";
                pnlLogin.Visible = true;
            }
            
        }
        protected void objPaginingSearch_PageChange(object sender, clsPagining.CustomPaginingArgs e)
        {
            gridReport.SelectedIndex = -1;
            gridReport.EditItemIndex = -1;
            gridReport.CurrentPageIndex = e.NewPage;
            CaricaReport();
        }
        
        #endregion

        #region FUNCTIONS

        private void CaricaReport()
        {
            DataTable DTResult = new DataTable();
            lblTitolo2.Text = "Green Apple Management - Reportistica";

            string sql = "SELECT *,'<img src=''../customLayout/images/ReportHome.gif''>' AS ReportHome FROM ADMIN_Report ";
            sql += " INNER JOIN ADMIN_REPORTGRUPPI ON ADMIN_Report.IdReport=ADMIN_REPORTGRUPPI.IdReport ";
            sql += " INNER JOIN ADMIN_UTENTIGRUPPI ON ADMIN_REPORTGRUPPI.IDGRUPPO=ADMIN_UTENTIGRUPPI.IDGRUPPO ";
            sql += " WHERE ADMIN_UTENTIGRUPPI.IDUTENTE = " + ((clsSession)Session["GreenApple"]).IDUtente;
            sql += " ORDER BY POSIZIONE";
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            DTResult.Columns.Add("LINK");

            if (DTResult.Rows.Count > 0)
            {
                for (int i = 0; i < DTResult.Rows.Count; i++)
                {
                    DTResult.Rows[i]["LINK"] = "<a href='ReportViewer.aspx?IDReport=" + DTResult.Rows[i]["IDREPORT"].ToString() + "'>" + DTResult.Rows[i]["NOME"].ToString() + "</a>";
                }

                gridReport.DataSource = DTResult;
                gridReport.DataBind();
            }
            else
            {
                gridReport.DataSource = null;
                gridReport.DataBind();
            }
        }

        private void CaricaNews()
        {
            //***********************************************
            //******************************* CONFIGURATION**
            //***********************************************
            DataTable DTConfig = new DataTable();
            bool VisibleCloseNewsHomePage = false;
            string sqlConfig = "SELECT * FROM ADMIN_CONFIGURATION ";
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sqlConfig, "RESULT", ref DTConfig, true);
            if (DTConfig.Rows.Count > 0)
            {
                VisibleCloseNewsHomePage = Convert.ToBoolean(DTConfig.Rows[0]["VisibleCloseNewsHomePage"].ToString());
            }


            DataTable DTResult = new DataTable();
            string sql = "SELECT *,CASE WHEN ISNULL(Chiusa,0)=0 THEN '<img src=''../customLayout/images/NewsOpen.gif''>' ELSE '<img src=''../customLayout/images/NewsClose.gif''>' END AS NewsChiusa FROM ADMIN_NEWS ";
            if (VisibleCloseNewsHomePage == false)
            {
                sql += " WHERE Chiusa = 0 ";
            }
            sql += " ORDER BY POSIZIONE ";
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            if (DTResult.Rows.Count > 0)
            {
                gridNews.DataSource = DTResult;
                gridNews.DataBind();
            }
            else
            {
                gridNews.DataSource = null;
                gridNews.DataBind();
            }
        }

        #endregion
        protected void lnkACCEDI_Click(object sender, EventArgs e)
        {
            //Provo Login
            string sql = "SELECT * FROM ADMIN_UTENTI WHERE LOGIN ='" + txtLogin.Text + "' ";
            sql += " AND PASSWORD ='" + txtPassword.Text + "'";

            DataTable DTUtente = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "UTENTE", ref DTUtente, true);
            if (DTUtente.Rows.Count > 0)
            {
                ((clsSession)Session["GreenApple"]).Login = txtLogin.Text;
                ((clsSession)Session["GreenApple"]).Password = txtPassword.Text;
                ((clsSession)Session["GreenApple"]).IDUtente = DTUtente.Rows[0]["IDUtente"].ToString();

                Response.Redirect("Home.aspx");
            }
            else
            {
                lblErrorLogin.Text = "Credenziali di accesso non valide, si prega di riprovare.";
            }
        }
}
}