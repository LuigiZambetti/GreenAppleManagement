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
    public partial class Green_Admin_DatiGA : Green_BaseUserControl
    {

        #region DECLARATIONS

        private string sql = "";
        clsPagining objPagining = new clsPagining();
        #endregion

        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            lblError.Text = "";
            lblError.Visible = false;
            if (!IsPostBack)
            {
                CaricaDati();
            }
        }
        
        protected void lnkAggiorna_Click(object sender, EventArgs e)
        {
            bool ShowError = false;

            clsParameter pParameter = new clsParameter();
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@RAGSOC", txtRAGSOC.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@INDIRIZZO", txtINDIRIZZO.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CAP", txtCAP.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@LOCALITA", txtLOCALITA.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NAZIONE", txtNAZIONE.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PIVA", txtPIVA.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@AliquotaIVA", txtIVAAPPLICATA.Text, SqlDbType.NVarChar, ParameterDirection.Input));

            

            sql = "UPDATE Admin_DatiAnagrifici "
            + " SET RAGSOC = @RAGSOC "
            + ",INDIRIZZO = @INDIRIZZO "
            + ",CAP = @CAP "
            + ",LOCALITA = @LOCALITA "
            + ",NAZIONE = @NAZIONE "
            + ",AliquotaIVA = @AliquotaIVA "
            + ",PIVA = @PIVA ";
            
            clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

            //togliere automatismo per IVA
            //sql = "UPDATE fornitori "
            //+ " SET forperciva = @AliquotaIVA ";

            //clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

            sql = "UPDATE clienti "
            + " SET cliperciva = @AliquotaIVA ";

            clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);


            CaricaDati();

        }
        
        protected void lnkCerca_Click(object sender, EventArgs e)
        {
            CaricaDati();
        }
        #endregion

        #region FUNCTIONS
        private void CaricaDati()
        {
            sql = "SELECT top 1 * FROM Admin_DatiAnagrifici";
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            if (DTResult != null)
            {
                if (DTResult.Rows.Count > 0)
                {
                    txtRAGSOC.Text = "" + DTResult.Rows[0]["RAGSOC"];
                    txtINDIRIZZO.Text = "" + DTResult.Rows[0]["INDIRIZZO"];
                    txtCAP.Text = "" + DTResult.Rows[0]["CAP"];
                    txtLOCALITA.Text = "" + DTResult.Rows[0]["LOCALITA"];
                    txtNAZIONE.Text = "" + DTResult.Rows[0]["NAZIONE"];
                    txtPIVA.Text = "" + DTResult.Rows[0]["PIVA"];
                    txtIVAAPPLICATA.Text = "" + DTResult.Rows[0]["AliquotaIVA"];
                }
            }
        }
        #endregion        
    }
}