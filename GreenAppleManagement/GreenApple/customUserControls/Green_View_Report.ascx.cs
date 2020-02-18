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
using System.IO;

namespace Green.Apple.Management
{
    public partial class Green_View_Report : Green_BaseUserControl
    {

        #region DECLARATIONS

        private string sql = "";
        clsPagining objPagining = new clsPagining();

        #endregion

        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            ViewState["IDReport"] = Request.QueryString["IDReport"].ToString();

            clsFunctions.AssegnaEventoCalendar(imgDataInizio, txtDataInizio, false);
            clsFunctions.AssegnaEventoCalendar(imgDataFine, txtDataFine, false);

            objPagining.CaricaPagining(phPagine, gridData);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            
            if (!IsPostBack)
            {
                ViewState["QUERYREPORT"] = "";

                DateTime DataFineMese = Convert.ToDateTime("01/" + DateTime.Today.AddMonths(1).Month.ToString().PadLeft(2, '0') + "/" + DateTime.Today.Year);
                DataFineMese = DataFineMese.AddDays(-1);
                txtDataInizio.Text = "01/" + DateTime.Today.Month.ToString().PadLeft(2, '0') + "/" + DateTime.Today.Year;
                txtDataFine.Text = DataFineMese.Day.ToString().PadLeft(2, '0') + "/" + DataFineMese.Month.ToString().PadLeft(2, '0') + "/" + DataFineMese.Year;
       
                CaricaDati();
                CaricaInEvidenza();
            }
        }
        protected void objPagining_PageChange(object sender, clsPagining.CustomPaginingArgs e)
        {
            gridData.SelectedIndex = -1;
            gridData.EditItemIndex = -1;
            gridData.CurrentPageIndex = e.NewPage;
            CaricaDati();
        }
        
        #endregion

        #region FUNCTIONS
        private void CaricaDati()
        {
            string QueryReport = "";
            sql = "SELECT * FROM ADMIN_REPORT WHERE IDREPORT = " + ViewState["IDReport"].ToString();
            DataTable DTQuery = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "QUERY", ref DTQuery, true);

            if (DTQuery.Rows.Count > 0)
            {
                lblTitle.Text = DTQuery.Rows[0]["Nome"].ToString();
                lblDescrizione.Text = DTQuery.Rows[0]["Descrizione"].ToString();

                QueryReport = DTQuery.Rows[0]["QUERY"].ToString();

                bool ExportTracciato = false;
                try
                {
                    ExportTracciato = Convert.ToBoolean(DTQuery.Rows[0]["ExportFile"].ToString());
                }
                catch { }

                ROWEsporta.Visible = ExportTracciato;

                try
                {
                    gridData.PageSize = int.Parse(DTQuery.Rows[0]["RowPerPage"].ToString());
                }
                catch { }

                if ("" + DTQuery.Rows[0]["DataColumn"] != "")
                {
                    RWDATE.Visible = true;
                }
                else
                {
                    RWDATE.Visible = false;
                }

                CaricaReport("" + DTQuery.Rows[0]["DataColumn"].ToString(), QueryReport);
            }
        }

        private void CaricaReport(string ColData, string QueryReport)
        {
            DataTable DTResult = new DataTable();

            if (RWDATE.Visible == true)
            {
                string TempQuery = QueryReport.ToUpper();

                if (TempQuery.Contains("WHERE"))
                {
                    string PartialQuery = "";
                    PartialQuery = " WHERE " + ColData + " >= CONVERT(datetime,'" + txtDataInizio.Text + "',105) ";
                    PartialQuery += " AND " + ColData + " <= CONVERT(datetime,'" + txtDataFine.Text + "',105) AND ";

                    TempQuery = TempQuery.Replace("WHERE", PartialQuery);
                    TempQuery = TempQuery.Replace("@@DATA@@", "");
                    QueryReport = TempQuery;
                }
                else
                {
                    string PartialQuery = "";
                    PartialQuery = " WHERE " + ColData + " >= CONVERT(datetime,'" + txtDataInizio.Text + "',105) ";
                    PartialQuery += " AND " + ColData + " <= CONVERT(datetime,'" + txtDataFine.Text + "',105)  ";
                   
                    //TempQuery += PartialQuery;
                    QueryReport = TempQuery.Replace("@@DATA@@",PartialQuery);
                }
            }

            ViewState["QUERYREPORT"] = QueryReport;

            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, QueryReport, "RESULT", ref DTResult, true);
            try
            {
                if (gridData.CurrentPageIndex >= ((double)DTResult.Rows.Count / (double)gridData.PageSize))
                {
                    gridData.CurrentPageIndex -= 1;
                }
            }
            catch { }

            if (DTResult != null)
            {
                if (DTResult.Rows.Count>0)
                {
                    gridData.DataSource = DTResult;
                    gridData.DataBind();
                }
            }

            objPagining.CaricaPagining(phPagine, gridData);
        }

        private void CaricaInEvidenza()
        {
            DataTable DTResult = new DataTable();
            string sql = "SELECT * FROM ADMIN_Report ";
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
        #endregion        
        
        protected void lnkFiltra_Click(object sender, EventArgs e)
        {
            CaricaDati();
        }
        protected void lnkEsporta_Click(object sender, EventArgs e)
        {
            //Export del Tracciato
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, ViewState["QUERYREPORT"].ToString(), "RESULT", ref DTResult, true);

            MemoryStream memoryStream = new MemoryStream();
            TextWriter tw = new StreamWriter(memoryStream);  
               
            for (int r = 0; r < DTResult.Rows.Count; r++)
            {
                string strLine = "";
                for (int c = 0; c < DTResult.Columns.Count; c++)
                {
                    if (strLine != "") strLine += ";";
                    strLine += DTResult.Rows[r][c].ToString();
                }
                tw.WriteLine(strLine);
            }

            tw.Close();

            string tracciato_filename = "TRACCIATO.txt";

            //Convert the memorystream to an array of bytes.
            byte[] byteArray = memoryStream.ToArray();
            memoryStream.Flush();
            memoryStream.Close();
            Response.Clear();
            Response.AddHeader("Content-Disposition", "attachment; filename=" + tracciato_filename + "");
            Response.AddHeader("Content-Length", byteArray.Length.ToString());
            Response.ContentType = "application/octet-stream";
            Response.BinaryWrite(byteArray);
        }
    }
}