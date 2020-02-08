using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Threading;
using System.IO;
using System.Text;
using Microsoft.Reporting.WebForms;
using System.Globalization;

namespace Green.Apple.Management
{
    public partial class Print : GREEN_BasePage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (Request.QueryString.Count == 5)
                {
                    string NUMERO = Request.QueryString["NUMERO"].ToString();
                    string ANNO = Request.QueryString["ANNO"].ToString();
                    string TIPO = Request.QueryString["TIPO"].ToString();
                    string DCID = Request.QueryString["DCID"].ToString();
                    string CAMBIO = Request.QueryString["CAMBIO"].ToString();

                    switch (TIPO)
                    {
                        case "FATTURA":
                            {
                                DataTable DTTesta = new DataTable();
                                string sql = @"select documclienti.*,ModalitaPagamento.* from documclienti 
                                inner join clienti on clienti.clicodice = documclienti.dccliente
                                left join ModalitaPagamento on documclienti.DCPagam = ModalitaPagamento.Codice
                                WHERE DCID = " + DCID + " ";
                                //sql += " AND DCANNO = '" + ANNO + "' ";
                                clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "TESTA", ref DTTesta, true);

                                DataTable DTCliente = new DataTable();
                                //sql = "SELECT * FROM CLIENTI WHERE CLICODICE= '" + DTTesta.Rows[0]["DCcliente"].ToString() + "' ";

                                // tratto il messaggio IVA 
                                sql = @"SELECT CLI.*, FA.Messaggio FROM ( 
                                    SELECT C.* , CASE WHEN C.CliNazione <> 'ITALIA' THEN 'ESTERO' ELSE CliNazione END AS Nazione
                                    FROM CLIENTI C WHERE CLICODICE= '" + DTTesta.Rows[0]["DCcliente"].ToString() + "' " +
                                        @") AS CLI
                                    INNER JOIN FattureAlert FA ON CLI.GaCalcoloIVA = FA.IVA AND CLI.Nazione = FA.Nazione";

                                clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "TESTA", ref DTCliente, true);

                                DataTable DTRighe = new DataTable();
                                //Carico le righe
                                sql = "select * from prestazioni ";
                                sql += " WHERE PRfattura = " + DCID;
                                clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RIGHE", ref DTRighe, true);
                                if (DTRighe.Rows.Count == 0)
                                {
                                    if ("" + DTTesta.Rows[0]["DCnumeroprestazione"] != "")
                                    {
                                        //Carico con Vecchia modalità
                                        sql = "select * from prestazioni WHERE PRnumero = " + DTTesta.Rows[0]["DCnumeroprestazione"].ToString();
                                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RIGHE", ref DTRighe, true);
                                    }
                                }

                                //sql = "SELECT * FROM Prestazioni WHERE PRFATTURA = '" + NUMERO + "' ";
                                //sql += " AND YEAR(PRdatafattura) = '" + ANNO + "' ";
                                //clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RIGHE", ref DTRighe, true);

                                DTTesta.TableName = "FATTURA";
                                DTRighe.TableName = "RIGHE";
                                DTCliente.TableName = "CLIENTE";

                                string tracciato_filename = "";

                                LocalReport localReport = new LocalReport();

                                string ReportFile = "";

                                string DCTIPO = "FATTURA";
                                if (DTTesta != null)
                                {
                                    if (DTTesta.Rows.Count > 0)
                                    {
                                        DCTIPO = DTTesta.Rows[0]["DCTIPO"].ToString();
                                    }
                                }

                                switch (DCTIPO)
                                {
                                    case "FATTURA":
                                        {
                                            if (CAMBIO == "NO")
                                            {
                                                ReportFile = "CLIENTI_FATTURA.rdlc";
                                                tracciato_filename = "FATTURA_" + ANNO + "_" + NUMERO + ".pdf";
                                            }
                                            else
                                            {
                                                ReportFile = "CLIENTI_FATTURA_CAMBIO.rdlc";
                                                tracciato_filename = "FATTURA_" + ANNO + "_" + NUMERO + "_Cambio_Euro.pdf";
                                            }

                                            break;
                                        }
                                    case "NOTA DI CREDITO":
                                        {
                                            if (CAMBIO == "NO")
                                            {
                                                ReportFile = "CLIENTI_FATTURA.rdlc";
                                                tracciato_filename = "NOTA_DI_CREDITO_" + ANNO + "_" + NUMERO + ".pdf";
                                            }
                                            else
                                            {
                                                ReportFile = "CLIENTI_FATTURA_CAMBIO.rdlc";
                                                tracciato_filename = "NOTA_DI_CREDITO_" + ANNO + "_" + NUMERO + "_Cambio_Euro.pdf";
                                            }

                                            break;
                                        }

                                }

                                localReport.ReportPath = Server.MapPath("../report") + "\\" + ReportFile;

                                ReportDataSource reportDataSource = new ReportDataSource("CLIENTI_FATTURA_Righe_Documclienti", DTTesta);
                                localReport.DataSources.Add(reportDataSource);

                                ReportDataSource reportDataSource2 = new ReportDataSource("CLIENTI_FATTURA_Righe_Prestazioni", DTRighe);
                                localReport.DataSources.Add(reportDataSource2);

                                ReportDataSource reportDataSource3 = new ReportDataSource("CLIENTI_FATTURA_Righe_Clienti", DTCliente);
                                localReport.DataSources.Add(reportDataSource3);

                                string reportType = "PDF";
                                string mimeType;
                                string encoding;
                                string fileNameExtension;
                                string deviceInfo =
                                "<DeviceInfo>" +
                                "  <OutputFormat>PDF</OutputFormat>" +
                                "  <PageWidth>8.5in</PageWidth>" +
                                "  <PageHeight>11in</PageHeight>" +
                                "  <MarginTop>0.5in</MarginTop>" +
                                "  <MarginLeft>1in</MarginLeft>" +
                                "  <MarginRight>1in</MarginRight>" +
                                "  <MarginBottom>0.5in</MarginBottom>" +
                                "</DeviceInfo>";

                                Warning[] warnings;
                                string[] streams;
                                byte[] renderedBytes;

                                Thread.CurrentThread.CurrentCulture = new CultureInfo("IT-it");

                                renderedBytes = localReport.Render(
                                    reportType,
                                    deviceInfo,
                                    out mimeType,
                                    out encoding,
                                    out fileNameExtension,
                                    out streams,
                                    out warnings);

                                this.Response.Clear();
                                this.Response.AddHeader("Content-Type", "Application/pdf");
                                this.Response.AddHeader("Content-Disposition", "attachment; filename=" + tracciato_filename + "; size=" + renderedBytes.Length.ToString());
                                this.Response.Flush();
                                this.Response.BinaryWrite(renderedBytes);
                                this.Response.Flush();
                                //this.Response.Close(); // non funziona con la nuova versione di Chrome v53
                                //this.Response.End();
                                break;
                            }
                        case "NOTULA":
                        case "FORNITORE":
                            {
                                DataTable DTRighe = new DataTable();
                                //Carico le righe
                                string sql = "select ";
                                sql += clsDB.DatePartGG_MM_AAAA("DFdata", "DATA", false) + ",";
                                sql += " * from documfornitori inner join fornitori on documfornitori.DFfornitore = fornitori.forcodice ";
                                sql += " WHERE documfornitori.DCId= " + DCID;
                                clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RIGHE", ref DTRighe, true);

                                DTRighe.TableName = "FATTURA";

                                string tracciato_filename = "";

                                LocalReport localReport = new LocalReport();

                                string ReportFile = "";
                                ReportFile = "FORNITORI_FATTURA.rdlc";
                                if (TIPO == "NOTULA") ReportFile = "FORNITORI_NOTULA.rdlc";

                                tracciato_filename = "FATTURA_FORNITORE_" + DTRighe.Rows[0]["DFnumero"] + ".pdf";
                                if (TIPO == "NOTULA") tracciato_filename = "NOTULA_FORNITORE_" + DTRighe.Rows[0]["DFnumero"] + ".pdf";

                                localReport.ReportPath = Server.MapPath("../report") + "\\" + ReportFile;

                                ReportDataSource reportDataSource = new ReportDataSource("CLIENTI_FATTURA_Righe_Documfornitori", DTRighe);
                                localReport.DataSources.Add(reportDataSource);

                                sql = "select top 1 * from Admin_DatiAnagrifici";
                                DataTable DTAnagrafici = new DataTable();
                                clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "ANAGRAFICI", ref DTAnagrafici, true);

                                ReportDataSource reportDataSource2 = new ReportDataSource("CLIENTI_FATTURA_Righe_Anagrafici", DTAnagrafici);
                                localReport.DataSources.Add(reportDataSource2);

                                string reportType = "PDF";
                                string mimeType;
                                string encoding;
                                string fileNameExtension;
                                string deviceInfo =
                                "<DeviceInfo>" +
                                "  <OutputFormat>PDF</OutputFormat>" +
                                "  <PageWidth>8.5in</PageWidth>" +
                                "  <PageHeight>11in</PageHeight>" +
                                "  <MarginTop>0.5in</MarginTop>" +
                                "  <MarginLeft>1in</MarginLeft>" +
                                "  <MarginRight>1in</MarginRight>" +
                                "  <MarginBottom>0.5in</MarginBottom>" +
                                "</DeviceInfo>";

                                Warning[] warnings;
                                string[] streams;
                                byte[] renderedBytes;

                                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("IT-it");

                                renderedBytes = localReport.Render(
                                    reportType,
                                    deviceInfo,
                                    out mimeType,
                                    out encoding,
                                    out fileNameExtension,
                                    out streams,
                                    out warnings);

                                this.Response.Clear();
                                this.Response.AddHeader("Content-Type", "Application/pdf");
                                this.Response.AddHeader("Content-Disposition", "attachment; filename=" + tracciato_filename + "; size=" + renderedBytes.Length.ToString());
                                this.Response.Flush();
                                this.Response.BinaryWrite(renderedBytes);
                                this.Response.Flush();
                                //this.Response.Close(); // non funziona con la nuova versione di Chrome v53
                                this.Response.End();
                                break;
                            }
                    }
                }
            }
            catch (Exception ex)
            {

                System.IO.StreamWriter stream = new System.IO.StreamWriter(Server.MapPath("log/log_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt"));
                stream.WriteLine("");
                stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " ERROR");
                stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " Message: " + ex.Message);
                stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " StackTrace: " + ex.StackTrace);
                stream.Close();
            }
        }
    }
}
