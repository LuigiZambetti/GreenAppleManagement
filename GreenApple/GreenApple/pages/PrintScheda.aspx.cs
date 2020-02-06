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
    public partial class PrintScheda : GREEN_BasePage
    {
        protected void Page_Load(object sender, System.EventArgs e)
        {
            if (Request.QueryString.Count == 3)
            {
                
                //stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " START");
                string FORCODICE = Request.QueryString["FORCODICE"].ToString();
                string ANNO = Request.QueryString["ANNO"].ToString();
                string MESE = Request.QueryString["MESE"].ToString();

                string ForRagSoc = "";
                try
                {
                    //System.IO.StreamWriter stream = new System.IO.StreamWriter(Server.MapPath("log/log_" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt"));
                
                    DataTable DTLista = new DataTable();
                    //string sql = @"select ISNULL(DCNumero,'0') as DCNumero,ISNULL(DCAnno,'0') AS DCAnno,Lista_CategoriaServizi.csdescrizione as CATEGORIA,Lista_PrestazioniTipi.descrizione AS TIPO,'' AS FATTURA, '' AS DATAFATTURA, ";
                    string sql = @"select DCNumeroCompleto,ISNULL(DCAnno,'0') AS DCAnno,Lista_CategoriaServizi.csdescrizione as CATEGORIA,Lista_PrestazioniTipi.descrizione AS TIPO,'' AS FATTURA, '' AS DATAFATTURA, ";
                    sql += clsDB.DatePartGG_MM_AAAA("srdatainizio", "DATAINIZIO", false) + ",";
                    sql += clsDB.DatePartGG_MM_AAAA("srdatafine", "DATAFINE", false) + ",";
                        sql += @" clienti.cliragsoc, fornitori.*,servizi.* from fornitori
                        inner join servizi on servizi.SROperatore=fornitori.forcodice
                        left join prestazioni on servizi.srnumeroprestazione=prestazioni.prnumero
                        left join documclienti on prestazioni.prfattura=documclienti.dcid
                        left join Lista_PrestazioniTipi on servizi.SRtiposervizio = Lista_PrestazioniTipi.idtipo
                        inner join clienti on servizi.srcliente=clienti.clicodice
                        left join Lista_CategoriaServizi on servizi.SRcategoria=Lista_CategoriaServizi.cscodice
                        where 1=1 "; //SRfatturare = 1 and SRFatturato = 0

                    sql += " AND ";
                    sql += " ( ";
                    sql += "     (SRAnnoFatt = " + ANNO + "  AND SRMeseFatt = " + MESE + ")";
	                sql += "     OR ";
                    sql += "     (SRAnnoFatt is null and (DATEPART(yyyy,SRdatainizio) = " + ANNO + " AND DATEPART(mm,SRdatainizio) = " + MESE + ")) ";
                    sql += " ) ";

                    sql += " and forcodice = " + FORCODICE;
                    sql += " order by SRAnnoFatt,SRMeseFatt,SRDataInizio,SRDataFine";
                   
                    clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "LISTA", ref DTLista, true);

                    //stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " Select 1 ok");
                    //era : inner join Lista_PrestazioniTipi on servizi.SRtiposervizio = Lista_PrestazioniTipi.idtipo
                    //AGGIUNTA APPLICATIVA DI IMPORTO SFILATE

                    sql = " select isnull(sum(dfimportosfilate),0) as dfimportosfilate from documfornitori ";
                    sql += " where 1=1 ";
                    sql += " AND DFFornitore = " + FORCODICE;
                    sql += " AND ";
                    sql += " ( ";
                    sql += " (DFAnnoFatt = " + ANNO + "  AND DFMeseFatt = " + MESE + ") ";
                    sql += " OR ";
                    sql += " (DFAnnoFatt is null and (DATEPART(yyyy,DFData) = " + ANNO + " AND DATEPART(mm,DFData) = " + MESE + ")) ";
                    sql += " ) ";
                    DataTable DTImpSfilate = new DataTable();
                    clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "IMPSFILATE", ref DTImpSfilate, true);
                    //stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " Select 2 ok");

                    if (DTImpSfilate != null)
                    {
                        if (DTImpSfilate.Rows.Count > 0)
                        { 
                            //Aggiungo la riga a DTLista
                            //Ns.Rif= SF
                            //Inizio= campo vuoto
                            //Fine  = campo vuoto
                            //Cliente= Sfilate
                            //GG= campo vuoto
                            //Importo=importo sfilate

                            double CheckSfilate = Convert.ToDouble("" + DTImpSfilate.Rows[0]["dfimportosfilate"]);
                            if (!string.IsNullOrEmpty(CheckSfilate.ToString()) && CheckSfilate != 0)
                            {
                                DataRow newRow = DTLista.NewRow();
                                newRow["DCNumeroCompleto"] = "0/00";
                                newRow["DCAnno"] = "0";
                                newRow["DATAINIZIO"] = "";
                                newRow["DATAFINE"] = "";
                                newRow["cliragsoc"] = "Sfilate";
                                newRow["CATEGORIA"] = "";
                                newRow["SRnumgiorni"] = System.DBNull.Value;
                                newRow["SRImponibile"] = Convert.ToDouble("" + DTImpSfilate.Rows[0]["dfimportosfilate"]);
                                newRow["SRCompensoaggiunto"] = "0";
                                DTLista.Rows.Add(newRow);
                            }
                        }
                    }
                
                    //stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " newRow ok");

                    for (int i = 0; i < DTLista.Rows.Count; i++)
                    {
                        ForRagSoc = DTLista.Rows[i]["ForRagSoc"].ToString();

                        if (DTLista.Rows[i]["SRCODICE"].ToString() != "")
                        {
                            if (bool.Parse(DTLista.Rows[i]["SRpagato"].ToString()) == false)
                            {
                                sql = @"UPDATE Servizi
                                SET
                                SRfatturare = 1, SRfatturato = 1, SRpagato = 0
                                WHERE SRcodice = " + DTLista.Rows[i]["SRCODICE"].ToString() + @"
                               ";

                                clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                            }
                        }
                    }
                
                    //stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " update Servizi ok");

                    DTLista.TableName = "LISTA";
                    
                    string tracciato_filename = "";
                
                    //stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " Start report");
                    LocalReport localReport = new LocalReport();

                    string ReportFile = "";
                    ReportFile = "SCHEDA_FORNITORE.rdlc";
                    tracciato_filename = ANNO + "_" + MESE.PadLeft(2, '0') + "_" + ForRagSoc.Replace("&", "_").Replace("+", "_").Replace(" ", "_") + "_SCHEDA_FORNITORE.pdf";
                    
                    localReport.ReportPath = Server.MapPath("../report") + "\\" + ReportFile;

                    ReportDataSource reportDataSource = new ReportDataSource("CLIENTI_FATTURA_Righe_SchedaFornitore", DTLista);
                    localReport.DataSources.Add(reportDataSource);

                    //sql = "select top 1 * from prestazioni";
                    //DataTable DTDocumClienti = new DataTable();

                    ReportDataSource reportDataSource1 = new ReportDataSource("CLIENTI_FATTURA_Righe_Documclienti");//, null);
                    localReport.DataSources.Add(reportDataSource1);

                    ReportDataSource reportDataSource2 = new ReportDataSource("CLIENTI_FATTURA_Righe_Prestazioni");//, null);
                    localReport.DataSources.Add(reportDataSource2);

                    ReportDataSource reportDataSource3 = new ReportDataSource("CLIENTI_FATTURA_Righe_Clienti");//, null);
                    localReport.DataSources.Add(reportDataSource3);

                    ReportDataSource reportDataSource4 = new ReportDataSource("CLIENTI_FATTURA_Righe_Anagrafici");//, null);
                    localReport.DataSources.Add(reportDataSource4);
                      
                    //sql = "select top 1 * from Admin_DatiAnagrifici";
                    //DataTable DTAnagrafici = new DataTable();
                    //ReportDataSource reportDataSource4 = new ReportDataSource("CLIENTI_FATTURA_Righe_Anagrafici", DTAnagrafici);
                    //localReport.DataSources.Add(reportDataSource4);
                      
                    //stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " ReportDataSource ok");
                    

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
                
                    //stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " Report configuration ok");
                    renderedBytes = localReport.Render(
                        reportType,
                        deviceInfo,
                        out mimeType,
                        out encoding,
                        out fileNameExtension,
                        out streams,
                        out warnings);
                
                    //stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " Report render ok");

                    this.Response.Clear();
                    this.Response.AddHeader("Content-Type", "Application/pdf");
                    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + tracciato_filename + "; size=" + renderedBytes.Length.ToString());
                    this.Response.Flush();
                    this.Response.BinaryWrite(renderedBytes);
                    this.Response.Flush();
                    //this.Response.Close();
                    //this.Response.End();
                    //stream.WriteLine(DateTime.Now.ToString("yyyyMMdd hh:mm:ss") + " pdf creation ok");

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
}
