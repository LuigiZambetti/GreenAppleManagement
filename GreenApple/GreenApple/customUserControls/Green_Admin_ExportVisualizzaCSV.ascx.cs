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
using System.Text;
using System.IO;
using System.Linq;

namespace Green.Apple.Management
{
    public partial class Green_Admin_ExportVisualizzaCSV : Green_BaseUserControl
    {
        #region DECLARATIONS

        protected string NOMEOGGETTO = "Nome Oggetto";
        protected string CODICEOGGETTO = "Codice Oggetto";
        protected string MEX_NOMEOGGETTO = "l'oggetto";
        protected string TABLENAME = "TABELLA";
        protected string COLUMNID = "ID";
        protected string COLUMNOME = "Nome";
        protected string COLUMRAGSOC = "Ragione Sociale";
        protected string COLUMNMODIFICATA = "Modificata";
        protected int COL_ESPORTA = 4;
        protected bool CheckUnivocita = true;
        protected bool CheckOrder = true;
        protected bool NvarcharID = false;
        protected bool NvarcharPOS = false;

        private string sql = "";
        clsPagining objPagining = new clsPagining();
        #endregion

        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            //COL_ESPORTA = 4;

            //TABLENAME = "Export_Clienti";
            //CheckUnivocita = true;
            //CheckOrder = true;
            //NvarcharID = true;


            //NOMEOGGETTO = "Anagrafica Cliente";
            //CODICEOGGETTO = "Codice";
            //MEX_NOMEOGGETTO = "la Anagrafica Cliente";
            //COLUMNID = "ID";


            //gridData.DataKeyField = COLUMNID;

            //((BoundColumn)gridData.Columns[0]).DataField = COLUMNID;
            //((BoundColumn)gridData.Columns[0]).HeaderText = "ID";

            //((BoundColumn)gridData.Columns[1]).DataField = COLUMNOME;
            //((BoundColumn)gridData.Columns[1]).HeaderText = COLUMNOME;

            //((BoundColumn)gridData.Columns[2]).DataField = COLUMRAGSOC;
            //((BoundColumn)gridData.Columns[2]).HeaderText = COLUMRAGSOC;

            //((BoundColumn)gridData.Columns[3]).DataField = COLUMNMODIFICATA;
            //((BoundColumn)gridData.Columns[3]).HeaderText = COLUMNMODIFICATA;


            //objPagining.CaricaPagining(phPagine, gridData);
            //objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            //lblError.Text = "";
            //lblError.Visible = false;
            if (!IsPostBack)
            {
                //if (((clsSession)Session["GreenApple"]).AzioneCorrente != clsSession.AzioniPagina.Modifica)
                //    ModificaVisible = false;
                CaricaDati();
            }
        }
      

        
        #endregion

        #region FUNCTIONS
        private void CaricaDati()
        {

            DirectoryInfo d = new DirectoryInfo(HttpContext.Current.Server.MapPath("~\\export"));
            FileInfo[] Files = d.GetFiles("*.csv"); 
            string str = @"<table style='width: 100%;'>
                            ";

            foreach (FileInfo file in Files)
            {
                str += @"<tr><td><a href='../export/" + file.Name + "' target='_blank'>" + file.Name + "</a></td></tr>";
            }

            str += @"</table>";

            litTABLE.Text = str;

        }

       
        #endregion

        #region PROPERTY
      
        #endregion
    }
}