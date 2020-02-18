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
using System.Collections.Generic;
using System.Linq;
using FattureElettroniche;
/*
 * EDIT:
 ** Gestione iva 0%
 ** copia di GAnote della tabella Clienti in Documclienti
 */
namespace Green.Apple.Management
{
    public partial class Green_View_Fattura : Green_BaseUserControl
    {

        #region DECLARATIONS

        private string sql = "";
        clsPagining objPagining = new clsPagining();
        protected int COL_ELIMINA = 0;
        DataTable DTRigheFattura = new DataTable();
        string IDSelezionati = "";

        #endregion

        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            clsFunctions.AssegnaEventoCalendar(imgtxtDataRiferimento, txtDataRiferimento, false);
            clsFunctions.AssegnaEventoCalendar(imgtxtDataPagamento, txtDataPagamento, false);

            clsFunctions.AssegnaEventoCalendar(imgtxtFiltroFine, txtFiltroFine, false);
            clsFunctions.AssegnaEventoCalendar(imgtxtFiltroInizio, txtFiltroInizio, false);


            COL_ELIMINA = 3;

            gridData.DataKeyField = "DCid";
            gridPrestazioni.DataKeyField = "PRNumero";
            gridPrestazioniLibere.DataKeyField = "PRNumero";

            objPagining.CaricaPagining(phPagine, gridData);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            lblError.Text = "";
            lblError.Visible = false;

            if (!IsPostBack)
            {
                ViewState["TIPOCERCA"] = "";
                CaricaComboAnnoMese();
                txtFiltroInizio.Text = DateTime.Now.AddMonths(-3).ToShortDateString(); // ho impostato di base gli ultimi 3 mesi con .AddMonths(-3)
                txtFiltroFine.Text = DateTime.Now.ToShortDateString();

                txtFiltro.Enabled = true;
                cboCliente.Enabled = true;
                lnkCercaCLIENTE.Enabled = true;

                IDSelezionati = "";
                divRigheFattura.Visible = false;

                if (((clsSession)Session["GreenApple"]).AzioneCorrente != clsSession.AzioniPagina.Inserimento
                    && ((clsSession)Session["GreenApple"]).AzioneCorrente != clsSession.AzioniPagina.Modifica)
                    ModificaVisible = false;
                gridData.Visible = true;
                CaricaDati(ViewState["TIPOCERCA"].ToString());

                if (Request.QueryString["IDPrestazione"] != null)
                {

                    //CARICO FATTURA CON PRESTAZIONE SELEZIONATA
                    lnkInserisci_Click(new object(), new EventArgs());

                    //lnkCercaCLIENTE_Click(new object(), new EventArgs());
                    CaricaCampiCliente(true);

                    sql = "select * from prestazioni where PRnumero = " + Request.QueryString["IDPrestazione"].ToString();
                    DataTable DTResult = new DataTable();
                    clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                    if (DTResult != null)
                    {
                        if (DTResult.Rows.Count > 0)
                        {
                            cboCliente.SelectedValue = DTResult.Rows[0]["PRcliente"].ToString();
                            CambioCliente();

                            CaricaPrestazioniCollegate_SIMULATA(Request.QueryString["IDPrestazione"].ToString());

                        }
                    }


                    txtFiltro.Enabled = false;
                    cboCliente.Enabled = false;
                    lnkCercaCLIENTE.Enabled = false;

                }
                if (Request.QueryString["DCId"] != null)
                {
                    txtFiltro.Enabled = false;
                    cboCliente.Enabled = false;
                    lnkCercaCLIENTE.Enabled = false;

                    //CHIAMATA DA PRESTAZIONE GIA' INSERITA
                    ModificaFattura(Request.QueryString["DCId"].ToString());

                }
            }
        }

        protected void objPagining_PageChange(object sender, clsPagining.CustomPaginingArgs e)
        {
            gridData.SelectedIndex = -1;
            gridData.EditItemIndex = -1;
            gridData.CurrentPageIndex = e.NewPage;
            CaricaDati(ViewState["TIPOCERCA"].ToString());
        }
        protected void gridData_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "MODIFICA":
                    {
                        txtFiltro.Enabled = false;
                        cboCliente.Enabled = false;
                        lnkCercaCLIENTE.Enabled = false;

                        gridData.SelectedIndex = e.Item.ItemIndex;

                        string FATTURASEL = gridData.DataKeys[e.Item.ItemIndex].ToString();
                        ModificaFattura(FATTURASEL);

                        break;
                    }
                case "ELIMINA":
                    {
                        //ELIMINO FATTURA
                        string DCID = gridData.DataKeys[e.Item.ItemIndex].ToString();
                        sql = "SELECT * FROM DOCUMCLIENTI ";
                        sql += " WHERE DCid = '" + DCID + "' ";
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        if (DTResult.Rows.Count > 0)
                        {
                            sql = " UPDATE Servizi SET SRfatclianno =NULL, SRfatturato=0, SRfatturare=0, SRdatafatturacli = NULL, SRfatturacli = NULL WHERE SRNumeroPrestazione IN  (select PRNumero from Prestazioni Where PRFattura = " + DCID + ") ";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                            //TROVATA FATTURA
                            sql = " UPDATE Prestazioni SET PRFattura=NULL,PRFatturato=0,PRFatturare=0, PRdatafattura=NULL, PRpagata=0 WHERE PRNumero IN (select PRNumero from Prestazioni Where PRFattura = " + DCID + ") ";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                            sql = "DELETE FROM DOCUMCLIENTI ";
                            sql += " WHERE DCid = '" + DCID + "' ";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                            sql = @"DELETE FROM [dbo].[EXPORT_Documclienti]
                                WHERE DCnumeroCompleto IN (
                                    SELECT DCnumeroCompleto 
                                    FROM [dbo].[EXPORT_Documclienti]
                                    WHERE DCid = " + DCID + ")";

                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                        }

                        CaricaDati(ViewState["TIPOCERCA"].ToString());

                        break;
                    }
                case "PRINT":
                    {
                        clsParameter pParameter = new clsParameter();

                        string DCID = gridData.DataKeys[e.Item.ItemIndex].ToString();
                        sql = "SELECT * FROM DOCUMCLIENTI ";
                        sql += " WHERE DCid = '" + DCID + "' ";
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        if (DTResult.Rows.Count > 0)
                        {
                            DataRow myRow = DTResult.Rows[0];

                            string URL = "Print.aspx?CAMBIO=NO&DCID=" + DCID + "&NUMERO=" + myRow["DCnumero"].ToString() + "&ANNO=" + myRow["DCanno"].ToString() + "&TIPO=FATTURA";
                            ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("PRINTReport")).Attributes.Add("src", URL);
                        }

                        break;
                    }
                case "PRINT_VALUTA":
                    {
                        clsParameter pParameter = new clsParameter();

                        string DCID = gridData.DataKeys[e.Item.ItemIndex].ToString();
                        sql = "SELECT * FROM DOCUMCLIENTI ";
                        sql += " WHERE DCid = '" + DCID + "' ";
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        if (DTResult.Rows.Count > 0)
                        {
                            DataRow myRow = DTResult.Rows[0];

                            string URL = "Print.aspx?CAMBIO=SI&DCID=" + DCID + "&NUMERO=" + myRow["DCnumero"].ToString() + "&ANNO=" + myRow["DCanno"].ToString() + "&TIPO=FATTURA";
                            ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("PRINTReport")).Attributes.Add("src", URL);
                        }

                        break;
                    }
                case "PRINT_EFATTURA":
                    {
                        string DCID = gridData.DataKeys[e.Item.ItemIndex].ToString();
                        if(DCID != string.Empty)
                        {
                            try 
                            { 
                                FatturaResult xml = FattureElettroniche.FattureElettroniche.CreateFatturaOrdinaria(int.Parse(DCID));


                                if(xml.errors.Count() > 0)
                                {
                                    string alertstring = "Errore nei dati del cliente: " + xml.errors.Aggregate((x, y) => x + ", " + y);
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('" + alertstring + "')", true);
                                }
                                else 
                                {
                                    SaveEFatturaXmlOnDB(DCID, xml.xml);

                                    byte[] xmlToBytes = System.Text.Encoding.ASCII.GetBytes(xml.xml); ;

                                    this.Response.Clear();
                                    this.Response.AddHeader("Content-Type", "application/xml");
                                    this.Response.AddHeader("Content-Disposition", "attachment; filename=" + xml.filename + "; size=" + xmlToBytes.Length.ToString());
                                    this.Response.Flush();
                                    this.Response.BinaryWrite(xmlToBytes);
                                    this.Response.Flush();
                                    this.Response.End();
                                }
                            }
                            catch(Exception exEFatt)
                            {
                                if (exEFatt.Message == "Non è stato possibile creare la fattura, dati sul DB mancanti") 
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Dati incompleti sul DB')", true);
                            
                            }
                            
                        }
                        
                        break;
                    }
            }
        }

        private void ModificaFattura(string FATTURASEL)
        {
            lnkAggiornaRiga.Enabled = true;
            lnkCloseRiga.Enabled = true;
            lnkAggiorna.Enabled = true;
            lnkInserisciRiga.Enabled = true;

            divRigheFattura.Visible = false;
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;

            sql = "select * from documclienti WHERE DCid=" + FATTURASEL;
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            if (DTResult.Rows.Count > 0)
            {
                //lnkCercaCLIENTE_Click(new object(), new EventArgs());
                CaricaCampiCliente(false);

                DataRow DTRow;
                DTRow = DTResult.Rows[0];

                cboCliente.SelectedValue = DTRow["DCcliente"].ToString();
                CambioCliente();
                lblPostaRAGSOC.Text = DTRow["DCpostaRagSoc"].ToString();
                try
                {
                    txtDataRiferimento.Text = Convert.ToDateTime(DTRow["DCdata"].ToString()).ToShortDateString();
                }
                catch { }

                try
                {
                    txtDataPagamento.Text = Convert.ToDateTime(DTRow["DCdatapag1"].ToString()).ToShortDateString();
                }
                catch { }

                try
                {
                    cboPagamento.SelectedValue = DTRow["DCpagam"].ToString();
                }
                catch { }

                txtNumeroFatt.Text = "" + DTRow["DCNumero"];
                txtNumeroFattCompleto.Text = "" + DTRow["DCNumeroCompleto"]; 
                lblPostaINDIRIZZO.Text = DTRow["DCpostaIndirizzo"].ToString();
                lblPostaLOCALITA.Text = DTRow["DCpostaLocalita"].ToString();
                lblPostaPROVINCIA.Text = DTRow["DCpostaProv"].ToString();
                lblPostaCAP.Text = DTRow["DCpostaCAP"].ToString();
                lblPostaNAZIONE.Text = DTRow["DCpostaNazione"].ToString();
                txtCliBanca.Text = DTRow["DCBanca"].ToString();
                txtCliPiazza.Text = DTRow["DCPiazza"].ToString();
                txtCliSwift.Text = DTRow["DCSwift"].ToString();
                txtDESCR_FATT.Text = DTRow["DCdescfat"].ToString();
                txtDESCR_SPESE.Text = DTRow["DCdescspese"].ToString();
                lblC_DCpercIVA.Text = DTRow["DCpercIVA"].ToString();
                if (lblC_DCpercIVA.Text == "") lblC_DCpercIVA.Text = "20";
                lblC_DCpercdiritti.Text = DTRow["DCpercdiritti"].ToString();

                txtManualPercSconto.Text = "0";
                if ("" + DTRow["DCSconto"] != "")
                {
                    txtManualPercSconto.Text = "" + DTRow["DCSconto"];
                }
                else
                {
                    txtManualPercSconto.Text = "0";
                }


                txtManualPercDiritti.Text = lblC_DCpercdiritti.Text;

                lblVALUTA_CAMBIO.Text = "" + DTRow["Cambio"];
                if (lblVALUTA_CAMBIO.Text != "") lblVALUTA_CAMBIO.Text = " Cambio Applicato : " + lblVALUTA_CAMBIO.Text;

                if (lblC_DCpercIVA.Text == "") lblC_DCpercIVA.Text = "0";

                lblC_DCimponibile.Text = DTRow["DCimponibile"].ToString();
                lblC_DCspese.Text = DTRow["DCspese"].ToString();
                lblC_DCdiritti.Text = DTRow["DCdiritti"].ToString();
                lblC_DCIVA.Text = DTRow["DCIVA"].ToString();
                lblC_DCtrattenute.Text = DTRow["DCtrattenute"].ToString();
                lblC_DCanticipi.Text = DTRow["DCanticipi"].ToString();
                lblC_DCtotale.Text = DTRow["DCtotale"].ToString();

                for (int i = 0; i < cboValuta.Items.Count; i++)
                {
                    if (cboValuta.Items[i].Value == DTRow["DCvaluta"].ToString())
                    {
                        cboValuta.SelectedIndex = i;
                        break;
                    }
                }

                if ("" + DTRow["DCPagata"] != "")
                {
                    bool res = false;
                    if (bool.TryParse("" + DTRow["DCPagata"], out res))
                    {
                        chkPagata.Checked = Convert.ToBoolean(DTRow["DCPagata"]);
                    }
                    else
                    {
                        chkPagata.Checked = false;
                    }
                }

                if ("" + DTRow["DCTipo"] == "FATTURA")
                {
                    chkFATTURA.Checked = true;
                    chkNOTA.Checked = false;
                }
                else
                {
                    chkFATTURA.Checked = false;
                    chkNOTA.Checked = true;
                }

                CaricaPrestazioniCollegate(DTRow["DCdata"].ToString(), DTRow["DCid"].ToString(), DTRow["DCnumeroprestazione"].ToString());
            }


            ModificaVisible = true;
            gridData.Visible = false;

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
        }
        private void CaricaPrestazioniCollegate_SIMULATA(string DCnumeroprestazione)
        {
            //Carico le righe collegate
            sql = "SELECT (select count(*) from servizi where srnumeroprestazione = prestazioni.prnumero) AS NumServizi, ";
            sql += " ISNULL(ModalitaPagamento.Descrizione,'------') AS PAGAMENTO, ";
            sql += @"CASE 
                            WHEN 
                            PRfatturare = 0 and PRfatturato = 0 and PRpagata = 0
                            THEN 'Da Fatturare'
                            WHEN 
                            PRfatturare = 1 and PRfatturato = 0 and PRpagata = 0
                            THEN 'Da Fatturare'
                            WHEN 
                            PRfatturato = 1  and PRpagata = 0
                            THEN 'Fatturata'
                            WHEN 
                            PRpagata = 1 and PRfatturato = 1  
                            THEN 'Pagata'
                            END AS COLORE, ";
            //sql += " ISNULL(CAST(documclienti.DCanno as nvarchar(255)),'') as ANNO,ISNULL(CAST(documclienti.DCnumero as nvarchar(255)),'') as NUMERO, ISNULL(PRvaluta,'EUR') as VALUTA, ";
            sql += " ISNULL(CAST(documclienti.DCanno as nvarchar(255)),'') as ANNO, documclienti.DCnumeroCompleto as NUMERO, ISNULL(PRvaluta,'EUR') as VALUTA, ";
            sql += clsDB.DatePartGG_MM_AAAA("PRdatainizio", "DATAINIZIO", false) + ",";
            sql += clsDB.DatePartGG_MM_AAAA("PRdatafine", "DATAFINE", false) + ",";
            sql += clsDB.DatePartGG_MM_AAAA("PRdatafattura", "DATAFATTURA", false) + ",";
            sql += " isnull(Lista_CategoriaServizi.CSdescrizione,'------') as CATDESCR, Clienti.cliragsoc AS GREEN_Cliente , prestazioni.* FROM prestazioni ";
            sql += " inner join clienti on prestazioni.PRcliente=clienti.clicodice ";
            sql += " left join Lista_CategoriaServizi on Lista_CategoriaServizi.CScodice = prestazioni.PRcategoria ";
            sql += " left join documclienti on documclienti.DCid = prestazioni.PRfattura ";
            sql += " left join ModalitaPagamento on ModalitaPagamento.Codice = prestazioni.PRcondpag ";
            sql += " WHERE PRnumero = " + DCnumeroprestazione;
            sql += " ORDER BY PRnumero DESC ";


            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTRigheFattura, true);

            gridPrestazioni.DataSource = DTRigheFattura;
            gridPrestazioni.DataBind();

            for (int i = 0; i <= gridPrestazioni.Items.Count - 1; i++)
            {
                ((LinkButton)gridPrestazioni.Items[i].Cells[1].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole deselezionare la prestazione selezionata ?')){}else{return false}");
            }

            CalcolaTotaliFattura_SIMULATA(DCnumeroprestazione);

        }

        private void CaricaPrestazioniCollegate(string DCdata, string DCid, string DCnumeroprestazione)
        {
            //Carico le righe collegate
            sql = "SELECT (select count(*) from servizi where srnumeroprestazione = prestazioni.prnumero) AS NumServizi, ";
            sql += " ISNULL(ModalitaPagamento.Descrizione,'------') AS PAGAMENTO, ";
            sql += @"CASE 
                            WHEN 
                            PRfatturare = 0 and PRfatturato = 0 and PRpagata = 0
                            THEN 'Da Fatturare'
                            WHEN 
                            PRfatturare = 1 and PRfatturato = 0 and PRpagata = 0
                            THEN 'Da Fatturare'
                            WHEN 
                            PRfatturato = 1  and PRpagata = 0
                            THEN 'Fatturata'
                            WHEN 
                            PRpagata = 1 and PRfatturato = 1  
                            THEN 'Pagata'
                            END AS COLORE, ";
            //sql += " ISNULL(CAST(documclienti.DCanno as nvarchar(255)),'') as ANNO,ISNULL(CAST(documclienti.DCnumero as nvarchar(255)),'') as NUMERO, ISNULL(PRvaluta,'EUR') as VALUTA, ";
            sql += " ISNULL(CAST(documclienti.DCanno as nvarchar(255)),'') as ANNO,documclienti.DCnumeroCompleto as NUMERO, ISNULL(PRvaluta,'EUR') as VALUTA, ";
            sql += clsDB.DatePartGG_MM_AAAA("PRdatainizio", "DATAINIZIO", false) + ",";
            sql += clsDB.DatePartGG_MM_AAAA("PRdatafine", "DATAFINE", false) + ",";
            sql += clsDB.DatePartGG_MM_AAAA("PRdatafattura", "DATAFATTURA", false) + ",";
            sql += " isnull(Lista_CategoriaServizi.CSdescrizione,'------') as CATDESCR, Clienti.cliragsoc AS GREEN_Cliente , prestazioni.* FROM prestazioni ";
            sql += " inner join clienti on prestazioni.PRcliente=clienti.clicodice ";
            sql += " left join Lista_CategoriaServizi on Lista_CategoriaServizi.CScodice = prestazioni.PRcategoria ";
            sql += " left join documclienti on documclienti.DCid = prestazioni.PRfattura ";
            sql += " left join ModalitaPagamento on ModalitaPagamento.Codice = prestazioni.PRcondpag ";
            sql += " WHERE PRfattura = " + DCid;
            sql += " ORDER BY PRnumero DESC ";


            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTRigheFattura, true);
            if (DTRigheFattura.Rows.Count == 0)
            {
                if ("" + DCnumeroprestazione != "")
                {
                    //Carico con Vecchia modalità
                    sql = "SELECT (select count(*) from servizi where srnumeroprestazione = prestazioni.prnumero) AS NumServizi, ";
                    sql += " ISNULL(ModalitaPagamento.Descrizione,'------') AS PAGAMENTO, ";
                    sql += @"CASE 
                                    WHEN 
                                    PRfatturare = 0 and PRfatturato = 0 and PRpagata = 0
                                    THEN 'Da Fatturare'
                                    WHEN 
                                    PRfatturare = 1 and PRfatturato = 0 and PRpagata = 0
                                    THEN 'Da Fatturare'
                                    WHEN 
                                    PRfatturato = 1  and PRpagata = 0
                                    THEN 'Fatturata'
                                    WHEN 
                                    PRpagata = 1 and PRfatturato = 1  
                                    THEN 'Pagata'
                                    END AS COLORE, ";
                    //sql += " ISNULL(CAST(documclienti.DCanno as nvarchar(255)),'') as ANNO,ISNULL(CAST(documclienti.DCnumero as nvarchar(255)),'') as NUMERO, ISNULL(PRvaluta,'EUR') as VALUTA, ";
                    sql += " ISNULL(CAST(documclienti.DCanno as nvarchar(255)),'') as ANNO,documclienti.DCnumeroCompleto as NUMERO, ISNULL(PRvaluta,'EUR') as VALUTA, ";
                    sql += clsDB.DatePartGG_MM_AAAA("PRdatainizio", "DATAINIZIO", false) + ",";
                    sql += clsDB.DatePartGG_MM_AAAA("PRdatafine", "DATAFINE", false) + ",";
                    sql += clsDB.DatePartGG_MM_AAAA("PRdatafattura", "DATAFATTURA", false) + ",";
                    sql += " isnull(Lista_CategoriaServizi.CSdescrizione,'------') as CATDESCR, Clienti.cliragsoc AS GREEN_Cliente , prestazioni.* FROM prestazioni ";
                    sql += " inner join clienti on prestazioni.PRcliente=clienti.clicodice ";
                    sql += " left join Lista_CategoriaServizi on Lista_CategoriaServizi.CScodice = prestazioni.PRcategoria ";
                    sql += " left join documclienti on documclienti.DCid = prestazioni.PRfattura ";
                    sql += " left join ModalitaPagamento on ModalitaPagamento.Codice = prestazioni.PRcondpag ";
                    sql += " WHERE PRnumero = " + DCnumeroprestazione;
                    sql += " ORDER BY PRnumero DESC ";

                    clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTRigheFattura, true);
                }
            }

            gridPrestazioni.DataSource = DTRigheFattura;
            gridPrestazioni.DataBind();

            for (int i = 0; i <= gridPrestazioni.Items.Count - 1; i++)
            {
                ((LinkButton)gridPrestazioni.Items[i].Cells[1].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole deselezionare la prestazione selezionata ?')){}else{return false}");
            }

            CalcolaTotaliFattura();
        }

        protected void lnkInserisci_Click(object sender, EventArgs e)
        {
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            gridData.SelectedIndex = -1;
            ModificaVisible = true;
            gridData.Visible = false;

            lnkAggiornaRiga.Enabled = false;
            lnkCloseRiga.Enabled = false;

            txtFiltro.Enabled = true;
            cboCliente.Enabled = true;
            lnkCercaCLIENTE.Enabled = true;

            txtDataRiferimento.Text = DateTime.Today.ToShortDateString();
            txtDataPagamento.Text = DateTime.Today.ToShortDateString();
            chkFATTURA.Checked = true;
            chkNOTA.Checked = false;

            string ANNO = Convert.ToDateTime(txtDataRiferimento.Text).Year.ToString();
            string NUMERO = "";

            txtNumeroFatt.Text = "";

            //recupero il numero fattura maggiore dell'anno in corso 
//            sql = @"SELECT 
//	                    max
//	                    (
//		                    cast
//		                    (
//			                    substring(
//				                    dcnumerocompleto, 1, CHARINDEX('/', DCnumeroCompleto)-1
//			                    )
//		                    as int)
//	                    ) + 1  as NUMERO 
//	                FROM [dbo].[Documclienti] 
//                    where 
//                        CHARINDEX('/', DCnumeroCompleto) > 0 
//                        AND
//                        substring(
//				            dcnumerocompleto, CHARINDEX('/', DCnumeroCompleto)+1, LEN(dcnumerocompleto)
//			            ) = " + ANNO.Substring(2);
            sql = @"SELECT 
	                    max
	                    (
			                dcnumero
	                    ) + 1  as NUMERO 
	                FROM [dbo].[Documclienti] 
                    where 
                        CHARINDEX('/', DCnumeroCompleto) > 0 
                        AND
                        substring(
				            dcnumerocompleto, CHARINDEX('/', DCnumeroCompleto)+1, LEN(dcnumerocompleto)
			            ) = " + ANNO.Substring(2);

            DataTable DTNumero = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "NUMEROFATTURA", ref DTNumero, true);
            if (DTNumero.Rows.Count > 0)
            {
                NUMERO = DTNumero.Rows[0]["NUMERO"].ToString();
                txtNumeroFatt.Text = NUMERO;
                txtNumeroFattCompleto.Text = NUMERO + "/" + ANNO.Substring(2, 2); 
            }

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            this.Page.Validate();
        }

        protected void lnkAnnulla_Click(object sender, EventArgs e)
        {
            gridData.SelectedIndex = -1;
            ModificaVisible = false;
            gridData.Visible = true;

            if (Request.QueryString["DCId"] != null)
            {
                Response.Redirect("Fattura.aspx");
            }
        }

        protected void lnkAggiorna_Click(object sender, EventArgs e)
        {
            bool ShowError = false;
            string Nomenclatura = "";

            string sql = "select * from clienti where clicodice = " + cboCliente.SelectedValue + " ";
            DataTable DTcliente = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CLIENTEFATTURA", ref DTcliente, true);
            if (DTcliente.Rows.Count > 0)
            {
                Nomenclatura = "" + DTcliente.Rows[0]["Nomenclatura"];
            }

            switch (((clsSession)Session["GreenApple"]).AzioneCorrente)
            {
                case clsSession.AzioniPagina.Inserimento:
                    {
                        // il campo numero completo fattura ora è readonly e il numero è generato automaticamente, non è più necessario questo controllo

                        //sql = "SELECT * FROM documclienti WHERE DCANNO='" + Convert.ToDateTime(txtDataRiferimento.Text).Year.ToString() + "' AND DCNUMEROCOMPLETO = " + txtNumeroFattCompleto.Text;
                        //DataTable DTControllo = new DataTable();
                        //clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CONTROLLO", ref DTControllo, true);
                        //if (DTControllo.Rows.Count > 0)
                        //{
                        //    //ESISTE GIA' ERROE
                        //    ShowError = true;
                        //    lblError.Text = "Esite già una Fattura con lo stesso numero per lo stesso anno.";
                        //    lblError.Visible = true;
                        //    return;
                        //}

                        string ANNO = Convert.ToDateTime(txtDataRiferimento.Text).Year.ToString();

                        string NUMERO = txtNumeroFatt.Text;
                        int res = 0;
                        if (!int.TryParse(NUMERO, out res))
                        {
                            NUMERO = "0";
                        }

                        string NUMEROCOMPLETO = txtNumeroFattCompleto.Text;
                        if(NUMEROCOMPLETO.Split('/')[0].Length > 5)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Il numero fattura può avere al massimo 5 caratteri, altrimenti la fattura elettronica non potrà essere valida.')", true);
                            return;
                        }
                            

                        clsParameter pParameter = new clsParameter();
                        string CAP_INS = lblPostaCAP.Text.Trim();
                        if (CAP_INS.Trim() == "") CAP_INS = "0";

                        /*salva GAnote dalla tabella dbo.Clienti - START */
                        string CliGAnote, CliDataDicIntenti;
                        //Dictionary<string,string> clientiRet = new Dictionary<string,string>();
                        string clientiRet;

                        sql = "SELECT GAnote FROM dbo.Clienti WHERE Clicodice = " + cboCliente.SelectedValue;
                        CliGAnote = clsDB.ExecuteQueryScalar(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                        sql = "SELECT CliDataDicIntenti FROM dbo.Clienti WHERE Clicodice = " + cboCliente.SelectedValue;
                        CliDataDicIntenti = clsDB.ExecuteQueryScalar(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        if (CliGAnote == "") CliGAnote = null;
                        if (CliDataDicIntenti == "") CliDataDicIntenti = null;
                        //DEBUG START
                        System.IO.StreamWriter stream = new System.IO.StreamWriter(Server.MapPath("log/View_Fattura.txt"));
                        stream.WriteLine("CliDataDicIntenti: " + CliDataDicIntenti);

                        stream.Close();
                        if (CliDataDicIntenti != null && CliDataDicIntenti != "")
                        {
                            DateTime dataCliDataDicIntenti = DateTime.ParseExact(CliDataDicIntenti, "dd/MM/yyyy h.mm.ss", System.Globalization.CultureInfo.InvariantCulture);
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliDataDicIntenti", dataCliDataDicIntenti, SqlDbType.DateTime, ParameterDirection.Input));
                        }
                        else
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliDataDicIntenti", CliDataDicIntenti, SqlDbType.NVarChar, ParameterDirection.Input));
                        }
                            
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCnoteCliente", CliGAnote, SqlDbType.NVarChar, ParameterDirection.Input));
                        //DEBUG END
                        /*salva GAnote dalla tabella dbo.Clienti - END */

                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpagam", cboPagamento.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCcliente", cboCliente.SelectedValue, SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaRagSoc", lblPostaRAGSOC.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaIndirizzo", lblPostaINDIRIZZO.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaLocalita", lblPostaLOCALITA.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaProv", lblPostaPROVINCIA.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaCAP", CAP_INS, SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaNazione", lblPostaNAZIONE.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCBanca", txtCliBanca.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCPiazza", txtCliPiazza.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCSwift", txtCliSwift.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));

                        string DCTIPO = "FATTURA";
                        if (chkFATTURA.Checked) DCTIPO = "FATTURA";
                        if (chkNOTA.Checked) DCTIPO = "NOTA DI CREDITO";

                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCnumero", NUMERO, SqlDbType.SmallInt, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCnumeroCompleto", NUMEROCOMPLETO, SqlDbType.VarChar, ParameterDirection.Input)); 
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCanno", ANNO, SqlDbType.SmallInt, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCtipo", DCTIPO, SqlDbType.NVarChar, ParameterDirection.Input));
                        //DEBUG START
                        //stream.WriteLine("");
                        //stream.WriteLine("txtDataRiferimento: " + txtDataRiferimento.Text.ToString());
                        //stream.WriteLine("txtDataPagamento: " + txtDataPagamento.Text.ToString());
                        //stream.Close();
                        DateTime dateDataRiferimento = DateTime.ParseExact(txtDataRiferimento.Text.ToString().Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        DateTime dateDataPagamento = DateTime.ParseExact(txtDataPagamento.Text.ToString().Trim(), "dd/MM/yyyy", System.Globalization.CultureInfo.InvariantCulture);
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCdata", dateDataRiferimento, SqlDbType.DateTime, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCdatapag1", dateDataPagamento, SqlDbType.DateTime, ParameterDirection.Input));
                        //DEBUG END
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCimponibile", lblC_DCimponibile.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCspese", lblC_DCspese.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCdiritti", lblC_DCdiritti.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCIVA", lblC_DCIVA.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCtotale", lblC_DCtotale.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCtrattenute", lblC_DCtrattenute.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpercIVA", lblC_DCpercIVA.Text.Trim(), SqlDbType.Int, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpercdiritti", txtManualPercDiritti.Text.Trim(), SqlDbType.Int, ParameterDirection.Input));

                        double res1 = 0;
                        if (double.TryParse(txtManualPercSconto.Text, out res1))
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCSconto", txtManualPercSconto.Text, SqlDbType.Float, ParameterDirection.Input));
                        }
                        else
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCSconto", 0, SqlDbType.Float, ParameterDirection.Input));
                        }

                        if (double.TryParse(lblImportoSconto.Text, out res1))
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCScontato", lblImportoSconto.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        }
                        else
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCScontato", 0, SqlDbType.Float, ParameterDirection.Input));
                        }

                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DClingua", "IT", SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCanticipi", lblC_DCanticipi.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCdescfat", txtDESCR_FATT.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCdescspese", txtDESCR_SPESE.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Nomenclatura", Nomenclatura, SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCvaluta", cboValuta.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCPagata", chkPagata.Checked, SqlDbType.Bit, ParameterDirection.Input));

                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCid", 0, SqlDbType.Int, ParameterDirection.Output));

                        sql = @"INSERT INTO DOCUMCLIENTI 
                            (
                                Nomenclatura,DCSconto,DCScontato
                                ,DCvaluta
                                ,DCcliente
                                ,DCpostaRagSoc
                                ,DCpostaIndirizzo
                                ,DCpostaLocalita
                                ,DCpostaProv
                                ,DCpostaCAP
                                ,DCpostaNazione
                                ,DCBanca
                                ,DCPiazza,DCSwift
                                ,DCnumero
                                ,DCnumeroCompleto
                                ,DCanno
                                ,DCtipo
                                ,DCdata
                                ,DCdatapag1
                                ,DCimponibile
                                ,DCspese
                                ,DCdiritti
                                ,DCIVA
                                ,DCtotale
                                ,DCtrattenute
                                ,DCpercIVA
                                ,DCpercdiritti
                                ,DClingua
                                ,DCanticipi
                                ,DCdescfat
                                ,DCdescspese,DCpagam
                                ,DCnoteCliente -- FC 20180313 aggiunto
                                ,CliDataDicIntenti -- FC 20180313 aggiunto
                            ) 
                            VALUES(
                                @Nomenclatura,@DCSconto,@DCScontato
                                ,@DCvaluta
                                ,@DCcliente
                                ,@DCpostaRagSoc
                                ,@DCpostaIndirizzo
                                ,@DCpostaLocalita
                                ,@DCpostaProv
                                ,@DCpostaCAP
                                ,@DCpostaNazione
                                ,@DCBanca
                                ,@DCPiazza,@DCSwift
                                ,@DCnumero
                                ,@DCnumeroCompleto
                                ,@DCanno
                                ,@DCtipo
                                ,@DCdata
                                ,@DCdatapag1
                                ,@DCimponibile
                                ,@DCspese
                                ,@DCdiritti
                                ,@DCIVA
                                ,@DCtotale
                                ,@DCtrattenute
                                ,@DCpercIVA
                                ,@DCpercdiritti
                                ,@DClingua
                                ,@DCanticipi
                                ,@DCdescfat
                                ,@DCdescspese,@DCpagam
                                ,@DCnoteCliente -- FC 20180313 aggiunto
                                ,@CliDataDicIntenti -- FC 20180313 aggiunto
                                ); 
                                SET @DCid = @@IDENTITY";

                        Dictionary<string, string> Ret = new Dictionary<string, string>();

                        Ret = clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                        string FATTURASEL;
                        //stream.WriteLine("DCid: " + String.Join(", ", Ret.Keys.ToArray()) + " " + String.Join(",", Ret.Values.ToArray()));
                        //stream.Close();
                        FATTURASEL = Ret["@DCid"].ToString();

                        //try
                        //{
                        //}
                        //catch (Exception ex)
                        //{
                        //    System.IO.StreamWriter stream = new System.IO.StreamWriter(Server.MapPath("log/View_Fattura.txt"));
                        //    stream.WriteLine("");
                        //    stream.WriteLine(" ERROR");
                        //    stream.WriteLine( " Message: " + ex.Message);
                        //    stream.WriteLine(" StackTrace: " + ex.StackTrace);
                        //    stream.WriteLine("DCid: " + String.Join(", ", Ret.Keys.ToArray()));
                        //    stream.Close();
                        //    return;
                        //}


                        sql = " UPDATE ADMIN_PROGRESSIVI SET NUMERO = " + NUMERO + "  WHERE TIPO = 'FATTURA' AND ANNO='" + ANNO + "' ";
                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        if (chkPagata.Checked == true)
                        {
                            sql = " UPDATE Prestazioni SET PRpagata=1 WHERE PRNumero IN (select PRNumero from Prestazioni Where PRFattura = " + FATTURASEL + ") ";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                            /* 
                             * Imposta il flag SRfatturare a 1 se la prestazione risulta pagata e la categoria è configurata come "Rifferita".
                             * FatturaRifferita è pari ad 1 per le sfilate.
                             */
                            sql = @"UPDATE Servizi 
                                        SET SRfatturare=1
                                        WHERE SRnumeroprestazione IN (select PRNumero from Prestazioni Where PRFattura = " + FATTURASEL + @") 
                                              AND SRcategoria in (select CSCodice from Lista_CategoriaServizi where FatturaRifferita = 1)";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        }
                        else
                        {
                            sql = " UPDATE Prestazioni SET PRpagata=0 WHERE PRNumero IN (select PRNumero from Prestazioni Where PRFattura = " + FATTURASEL + ") ";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                            /* 
                             * Imposta il flag SRfatturare a 0 se la prestazione risulta pagata e la categoria è configurata come NON "Rifferita".
                             * FatturaRifferita è pari a 0 per tutte le prestazioni, tranne le sfilate.
                             */
                            sql = @"UPDATE Servizi
                                            SET SRfatturare=0
                                            WHERE SRnumeroprestazione IN (select PRNumero from Prestazioni Where PRFattura = " + FATTURASEL + @")
                                                AND SRcategoria in (select CSCodice from Lista_CategoriaServizi where FatturaRifferita = 0)";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                        }

                        sql = "SELECT * FROM Lista_Valuta where Codice ='" + cboValuta.SelectedValue + "' ";
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CAMBIO", ref DTResult, true);

                        if (DTResult.Rows.Count > 0)
                        {
                            sql = "UPDATE documclienti SET Cambio = " + DTResult.Rows[0]["Cambio"].ToString().Replace(",", ".") + " WHERE DCid = " + FATTURASEL + " ";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                        }

                        if (Request.QueryString["IDPrestazione"] != null)
                        {
                            string SEL = Request.QueryString["IDPrestazione"].ToString();

                            sql = " UPDATE Prestazioni SET PRannoDoc ='" + Convert.ToDateTime(txtDataRiferimento.Text).Year.ToString() + "', PRcondpag = '" + cboPagamento.SelectedValue + "', PRfatturato=1, PRfatturare=0, PRdatafattura = CONVERT(datetime,'" + txtDataRiferimento.Text + "',105), PRFattura = " + Ret["@DCid"].ToString() + " WHERE PRNumero = " + SEL + " ";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                            /* 
                             * Imposta il flag SRfatturare a 1 se la Query string contiene un valore di IDPrestazione e la categoria è configurata come NON "Rifferita".
                             */
                            sql = @"UPDATE Servizi
                                        SET SRfatturare=1
                                        WHERE SRnumeroprestazione = " + SEL + @"
                                            AND SRcategoria in (select CSCodice from Lista_CategoriaServizi where FatturaRifferita = 0)";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                        }


                        // Inserisco la nuova fattura nella tabella EXPORT_Documclienti
                        sql = @"DELETE FROM [dbo].[EXPORT_Documclienti]
                                WHERE DCnumeroCompleto IN (
                                    SELECT DCnumeroCompleto 
                                    FROM [dbo].[EXPORT_Documclienti]
                                    WHERE DCid = " + FATTURASEL + ")";

                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        sql = @"INSERT INTO [dbo].[EXPORT_Documclienti]
                                       ([DCid]
                                       ,[DCnumeroCompleto]
                                       ,[DCtipo]
                                       ,[DCdata]
                                       ,[DCcliente]
                                       ,[DCimponibile]
                                       ,[DCspese]
                                       ,[DCdiritti]
                                       ,[DCIVA]
                                       ,[DCtotale]
                                       ,[PRcategoria]
                                       ,[DCpagam]
                                       ,[Esportata]
                                       ,[Modificata], [DCPiazza])
                                 SELECT 
	                             dcid,
	                             [DCnumeroCompleto] AS NumeroFattura
                                  ,[DCtipo] AS FatturaNotaCredito
                                  ,CAST([DCdata] AS DATE) AS DataFattura
                                  ,[DCcliente] AS CodiceCliente
                                  ,[DCimponibile] AS ImponibileLordo
	                              ,[DCspese] AS Spese
	                              ,[DCdiritti] AS Diritti
                                  ,[DCIVA] AS ImportoIVA
                                  ,[DCtotale] AS ImportoTotale
	                              ,CASE
									WHEN C.CliNazione = 'ITALIA' THEN P.PRcategoria ELSE 'X'
									END AS CategoriaServizio
                                  , [DCpagam] AS TipoPagamento
	                              , 0 
	                              , 0, REPLACE([DCPiazza],'IBAN','') as IBAN
                              FROM [dbo].[Documclienti] F
                              LEFT OUTER JOIN [dbo].[Prestazioni] P ON P.PRfattura = F.DCid
                              INNER JOIN dbo.Clienti C ON C.Clicodice = F.DCcliente
                              WHERE DCid = " + FATTURASEL;

                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        // ------------------------------------------------------------

                        if (Request.QueryString["IDPrestazione"] != null)
                        {
                            Response.Redirect("Fattura.aspx");
                        }

                        if (Request.QueryString["DCId"] != null)
                        {
                            Response.Redirect("Fattura.aspx");
                        }
                        CaricaDati(ViewState["TIPOCERCA"].ToString());
                        ModificaVisible = false;
                        gridData.SelectedIndex = -1;
                        gridData.Visible = true;
                        break;
                    }
                case clsSession.AzioniPagina.Modifica:
                    {
                        ShowError = false;
                        string FATTURASEL = "";
                        if (Request.QueryString["DCId"] != null)
                        {
                            FATTURASEL = Request.QueryString["DCId"].ToString();
                        }
                        else
                        {
                            FATTURASEL = gridData.DataKeys[gridData.SelectedIndex].ToString();
                        }

                        // Non ha senso questo controllo, impedisce di fare gli UPDATE! E' ovvio che esista già la fattura, non sto facendo un INSERT
                        //sql = "SELECT * FROM documclienti WHERE DCID <> " + FATTURASEL + " AND DCANNO='" + Convert.ToDateTime(txtDataRiferimento.Text).Year.ToString() + "' AND DCNUMERO = " + txtNumeroFattCompleto.Text;
                        //DataTable DTControllo = new DataTable();
                        //clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CONTROLLO", ref DTControllo, true);
                        //if (DTControllo.Rows.Count > 0)
                        //{
                        //    //ESISTE GIA' ERROE
                        //    ShowError = true;
                        //    lblError.Text = "Esite già una Fattura con lo stesso numero per lo stesso anno.";
                        //    lblError.Visible = true;
                        //    return;
                        //}

                        sql = "select * from documclienti WHERE DCid=" + FATTURASEL;
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        string ANNO = Convert.ToDateTime(DTResult.Rows[0]["DCdata"].ToString()).Year.ToString();
                        string NUMERO = DTResult.Rows[0]["DCnumero"].ToString();

                        NUMERO = txtNumeroFatt.Text;
                        int res = 0;
                        if (!int.TryParse(NUMERO, out res))
                        {
                            NUMERO = "0";
                        }

                        
                        string NUMEROCOMPLETO = txtNumeroFattCompleto.Text;
                        if (NUMEROCOMPLETO.Split('/')[0].Length > 5)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Il numero fattura può avere al massimo 5 caratteri, altrimenti la fattura elettronica non potrà essere valida.')", true);
                            return;
                        }

                        clsParameter pParameter = new clsParameter();
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCid", FATTURASEL, SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpagam", cboPagamento.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCcliente", cboCliente.SelectedValue, SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaRagSoc", lblPostaRAGSOC.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaIndirizzo", lblPostaINDIRIZZO.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaLocalita", lblPostaLOCALITA.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaProv", lblPostaPROVINCIA.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaCAP", lblPostaCAP.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpostaNazione", lblPostaNAZIONE.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCBanca", txtCliBanca.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCPiazza", txtCliPiazza.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCSwift", txtCliSwift.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));

                        string DCTIPO = "FATTURA";
                        if (chkFATTURA.Checked) DCTIPO = "FATTURA";
                        if (chkNOTA.Checked) DCTIPO = "NOTA DI CREDITO";

                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCnumero", NUMERO, SqlDbType.SmallInt, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCnumeroCompleto", NUMEROCOMPLETO, SqlDbType.VarChar, ParameterDirection.Input)); 
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCanno", ANNO, SqlDbType.SmallInt, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCtipo", DCTIPO, SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCdata", txtDataRiferimento.Text.Trim(), SqlDbType.DateTime, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCdatapag1", txtDataPagamento.Text.Trim(), SqlDbType.DateTime, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCimponibile", lblC_DCimponibile.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCspese", lblC_DCspese.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCdiritti", lblC_DCdiritti.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCIVA", lblC_DCIVA.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCtotale", lblC_DCtotale.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCtrattenute", lblC_DCtrattenute.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpercIVA", lblC_DCpercIVA.Text.Trim(), SqlDbType.Int, ParameterDirection.Input));

                        double res2 = 0;
                        if (double.TryParse(txtManualPercSconto.Text, out res2))
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCSconto", txtManualPercSconto.Text, SqlDbType.Float, ParameterDirection.Input));
                        }
                        else
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCSconto", 0, SqlDbType.Float, ParameterDirection.Input));
                        }

                        if (double.TryParse(lblImportoSconto.Text, out res2))
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCScontato", lblImportoSconto.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        }
                        else
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCScontato", 0, SqlDbType.Float, ParameterDirection.Input));
                        }

                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCpercdiritti", txtManualPercDiritti.Text.Trim(), SqlDbType.Int, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DClingua", "IT", SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCanticipi", lblC_DCanticipi.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCdescfat", txtDESCR_FATT.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCdescspese", txtDESCR_SPESE.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Nomenclatura", Nomenclatura, SqlDbType.NVarChar, ParameterDirection.Input));

                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCPagata", chkPagata.Checked, SqlDbType.Bit, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCvaluta", cboValuta.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));



                        sql = @"UPDATE DOCUMCLIENTI 
                            SET DCcliente = @DCcliente
                            ,DCSconto = @DCSconto
                            ,DCScontato = @DCScontato
                            ,DCvaluta = @DCvaluta
                            ,DCPagata = @DCPagata
                            ,Nomenclatura = @Nomenclatura
                            ,DCpostaRagSoc = @DCpostaRagSoc
                            ,DCpostaIndirizzo = @DCpostaIndirizzo
                            ,DCpostaLocalita = @DCpostaLocalita
                            ,DCpostaProv = @DCpostaProv
                            ,DCpostaCAP = @DCpostaCAP
                            ,DCpostaNazione = @DCpostaNazione
                            ,DCBanca = @DCBanca
                            ,DCPiazza = @DCPiazza
                            ,DCSwift = @DCSwift
                            ,DCnumero = @DCnumero
                            ,DCnumeroCompleto = @DCnumeroCompleto
                            ,DCanno = @DCanno
                            ,DCtipo = @DCtipo
                            ,DCdata = @DCdata
                            ,DCdatapag1 = @DCdatapag1
                            ,DCimponibile = @DCimponibile
                            ,DCspese = @DCspese
                            ,DCdiritti = @DCdiritti
                            ,DCIVA = @DCIVA
                            ,DCtotale = @DCtotale
                            ,DCtrattenute = @DCtrattenute
                            ,DCpercIVA = @DCpercIVA
                            ,DCpercdiritti = @DCpercdiritti
                            ,DClingua = @DClingua
                            ,DCanticipi = @DCanticipi
                            ,DCdescfat = @DCdescfat
                            ,DCdescspese = @DCdescspese
                            ,DCpagam = @DCpagam
                            WHERE DCid = @DCid ";

                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "SELECT * FROM Lista_Valuta where Codice ='" + cboValuta.SelectedValue + "' ";
                        DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CAMBIO", ref DTResult, true);

                        if (DTResult.Rows.Count > 0)
                        {
                            sql = "UPDATE documclienti SET Cambio = " + DTResult.Rows[0]["Cambio"].ToString().Replace(",", ".") + " WHERE DCid = " + FATTURASEL + " ";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                        }

                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);


                        //SE PAGATA AGGIORNO CAMPI FIGLI
                        if (chkPagata.Checked == true)
                        {
                            string SEL = FATTURASEL;

                            sql = " UPDATE Prestazioni SET PRpagata=1 WHERE PRNumero IN (select PRNumero from Prestazioni Where PRFattura = " + FATTURASEL + ") ";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                            //sql = " UPDATE Servizi SET SRfatturare=1 WHERE SRnumeroprestazione IN (select PRNumero from Prestazioni Where PRFattura = " + FATTURASEL + ") AND SRcategoria in (select CSCodice from Lista_CategoriaServizi where FatturaRifferita = 1)";
                            sql = @" UPDATE Servizi SET SRfatturare=1 WHERE SRnumeroprestazione IN (
                                    select PRNumero from Prestazioni Where PRFattura = " + FATTURASEL + @"
                                    AND PRcategoria in (select CSCodice from Lista_CategoriaServizi where FatturaRifferita = 1))";
                            
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        }
                        else
                        {
                            string SEL = FATTURASEL;

                            sql = " UPDATE Prestazioni SET PRpagata=0 WHERE PRNumero IN (select PRNumero from Prestazioni Where PRFattura = " + FATTURASEL + ") ";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                            // - con questa istruzione, se modifico la fattura e il Servizio era DA FATTURARE, il servizio diventa erroneamente ESEGUITO (e non DA FATTURARE)
                            //sql = " UPDATE Servizi SET SRfatturare=0, SRfatturato=0 WHERE SRnumeroprestazione IN (select PRNumero from Prestazioni Where PRFattura = " + FATTURASEL + ") AND SRcategoria in (select CSCodice from Lista_CategoriaServizi where FatturaRifferita = 0)";
                            //clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                            //se il servizio è una sfilata, la fattura non pagata deve propagarsi al fornitore, che non potrà più fatturare (NON da fatturare)
                            sql = @" UPDATE Servizi SET SRfatturare=0 WHERE SRnumeroprestazione IN (
                                    select PRNumero from Prestazioni Where PRFattura = " + FATTURASEL + @"
                                    AND PRcategoria in (select CSCodice from Lista_CategoriaServizi where FatturaRifferita = 1))";

                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                        }

                        // Aggiorno la tabella EXPORT_DocumClienti (in realtà cancello e poi reinserisco da Documclienti, per avere i dati aggiornati) e metto Modificata = true se è già stata esportata
                        pParameter = new clsParameter();
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@modificata", 0, SqlDbType.Int, ParameterDirection.Output));
                        sql = @"DECLARE @num AS int

                                SELECT @num = COUNT(*) FROM dbo.EXPORT_Documclienti WHERE Esportata = 1 AND DCid = " + FATTURASEL;

                        sql += @" IF @num > 0
                                BEGIN
	                                SET @modificata = 1
                                END
                                ELSE
                                BEGIN
	                                SET @modificata = 0
                                END";

                        Dictionary<string, string> Ret = new Dictionary<string, string>();

                        Ret = clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        string MODIFICATA = Ret["@modificata"].ToString();


                        sql = @"DELETE FROM [dbo].[EXPORT_Documclienti]
                                WHERE DCnumeroCompleto IN (
                                    SELECT DCnumeroCompleto 
                                    FROM [dbo].[EXPORT_Documclienti]
                                    WHERE DCid = " + FATTURASEL + ")";

                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        sql = @"INSERT INTO [dbo].[EXPORT_Documclienti]
                                       ([DCid]
                                       ,[DCnumeroCompleto]
                                       ,[DCtipo]
                                       ,[DCdata]
                                       ,[DCcliente]
                                       ,[DCimponibile]
                                       ,[DCspese]
                                       ,[DCdiritti]
                                       ,[DCIVA]
                                       ,[DCtotale]
                                       ,[PRcategoria]
                                       ,[DCpagam]
                                       ,[Esportata]
                                       ,[Modificata]
                                        , [DCPiazza])
                                 SELECT 
	                             dcid,
	                             [DCnumeroCompleto] AS NumeroFattura
                                  ,[DCtipo] AS FatturaNotaCredito
                                  ,CAST([DCdata] AS DATE) AS DataFattura
                                  ,[DCcliente] AS CodiceCliente
                                  ,[DCimponibile] AS ImponibileLordo
	                              ,[DCspese] AS Spese
	                              ,[DCdiritti] AS Diritti
                                  ,[DCIVA] AS ImportoIVA
                                  ,[DCtotale] AS ImportoTotale
                                  ,CASE
									WHEN C.CliNazione = 'ITALIA' THEN P.PRcategoria ELSE 'X'
									END AS CategoriaServizio
                                  , [DCpagam] AS TipoPagamento
	                              , 0 
	                              , " + MODIFICATA;
                        sql += @", REPLACE([DCPiazza],'IBAN','') as IBAN FROM [dbo].[Documclienti] F
                              LEFT OUTER JOIN [dbo].[Prestazioni] P ON P.PRfattura = F.DCid
                              INNER JOIN dbo.Clienti C ON C.Clicodice = F.DCcliente
                              WHERE DCid = " + FATTURASEL;

                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        // ----------------------------------

                        if (Request.QueryString["DCId"] != null)
                        {
                            Response.Redirect("Fattura.aspx");
                        }
                        CaricaDati(ViewState["TIPOCERCA"].ToString());
                        ModificaVisible = false;
                        gridData.SelectedIndex = -1;
                        gridData.Visible = true;

                        break;
                    }
            }
            if (ShowError)
            {
                lblError.Text = "Esite già una Fattura.";
                lblError.Visible = true;
            }
        }

        protected void lnkCerca_Click(object sender, EventArgs e)
        {
            gridData.CurrentPageIndex = 0;

            CaricaDati(ViewState["TIPOCERCA"].ToString());
        }
        #endregion

        #region FUNCTIONS
        private void CaricaDati()
        {
            CaricaDati("");
        }
        private void CaricaDati(string TIPO)
        {
            //Carico Valuta
            sql = @"select * from Lista_Valuta ORDER BY POSIZIONE ";
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cboValuta.Items.Clear();
            ListItem myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Selezionare una valuta --";
            cboValuta.Items.Add(myItem);

            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["Codice"];
                myItem.Text = "" + DTResult.Rows[i]["Codice"] + " - " + DTResult.Rows[i]["Descrizione"];
                cboValuta.Items.Add(myItem);
            }

            cboValuta.SelectedIndex = 0;

            cboPagamento.Items.Clear();
            myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Selezionare modalità pagamento --";
            cboPagamento.Items.Add(myItem);

            sql = "SELECT * FROM ModalitaPagamento order by Codice";
            DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);
            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["Codice"];
                myItem.Text = "" + DTResult.Rows[i]["Codice"] + " - " + DTResult.Rows[i]["descrizione"];
                cboPagamento.Items.Add(myItem);
            }
            cboPagamento.SelectedIndex = 0;

            //***************************************************************************************


            //            sql = @" 
            //                SELECT '<BR>' as PRESTAZIONICOLLEGATE,ModalitaPagamento.Descrizione AS PAGAMENTO, Clienti.cliragsoc AS GREEN_Cliente , Documclienti.* FROM Documclienti
            //                inner join clienti on Documclienti.DCCliente=clienti.clicodice
            //                left join ModalitaPagamento on ModalitaPagamento.Codice=Documclienti.DCpagam
            //            ";

            sql = "SET ARITHABORT ON SELECT stuff( " +
                            "( " +
           "select cast('<BR>' as varchar(max)) " +
           "+ 'Prestazione : <a href=\"Prestazioni.aspx?FilterNumero='" +
           "+ cast(COALESCE(prestazioni.PRnumero,0) as varchar(max))" +
           "+ '\">' + cast(COALESCE(prestazioni.PRnumero,0) as varchar(max)) + '</a><BR>' " +
           "from prestazioni " +
           "WHERE prestazioni.PRfattura = Documclienti.dcid  " +
           "for xml path('') , root('MyString'), type " +
            ").value('/MyString[1]','varchar(max)') " +
            ", 1, 0, '') " +
           "as PRESTAZIONICOLLEGATE,ModalitaPagamento.Descrizione AS PAGAMENTO, Clienti.cliragsoc AS GREEN_Cliente , Documclienti.* FROM Documclienti " +
           "inner join clienti on Documclienti.DCCliente=clienti.clicodice " +
           "left join ModalitaPagamento on ModalitaPagamento.Codice=Documclienti.DCpagam ";




            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " WHERE (DCpostaRagSoc like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR DCpostaIndirizzo like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR DCpostaLocalita like '%" + txtFiltroLibero.Text.Trim() + "%' ";

                int Res = 0;
                if (Int32.TryParse(txtFiltroLibero.Text, out Res))
                {
                    sql += " OR DCNumeroPrestazione = " + txtFiltroLibero.Text.Trim() + " ";
                    sql += " OR DCid = " + txtFiltroLibero.Text.Trim() + " ";
                }

                sql += " OR Clienti.cliragsoc like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                //sql += " OR DCnumero like '%" + txtFiltroLibero.Text.Trim() + "%' )";
                sql += " OR DCnumeroCompleto like '%" + txtFiltroLibero.Text.Trim() + "%' )";
            }
            if (TIPO == "MESEANNO")
            {
                if (txtFiltroLibero.Text.Trim().Length > 0)
                {
                    if (cboMese.SelectedIndex > 0)
                    {
                        sql += " AND DATEPART(mm,DCdata) = " + cboMese.SelectedValue + " AND DATEPART(yyyy,DCdata) = " + cboAnno.SelectedValue + " ";
                    }
                    else
                    {
                        sql += " AND DATEPART(yyyy,DCdata) = " + cboAnno.SelectedValue + " ";
                    }
                }
                else
                {
                    if (cboMese.SelectedIndex > 0)
                    {
                        sql += " WHERE 1=1 AND DATEPART(mm,DCdata) = " + cboMese.SelectedValue + " AND DATEPART(yyyy,DCdata) = " + cboAnno.SelectedValue + " ";
                    }
                    else
                    {
                        sql += " WHERE 1=1 AND DATEPART(yyyy,DCdata) = " + cboAnno.SelectedValue + " ";
                    }
                }
            }
            else if (TIPO == "DATE")
            {
                if (txtFiltroLibero.Text.Trim().Length > 0)
                {
                    sql += " AND DCdata >= CONVERT(datetime,'" + txtFiltroInizio.Text + "',105) ";
                    sql += " AND DCdata <= CONVERT(datetime,'" + txtFiltroFine.Text + "',105)  ";
                }
                else
                {
                    sql += " WHERE 1=1 AND DCdata >= CONVERT(datetime,'" + txtFiltroInizio.Text + "',105) ";
                    sql += " AND DCdata <= CONVERT(datetime,'" + txtFiltroFine.Text + "',105)  ";
                }
            }
            else // aggiungo questo ELSE, così all'apertura iniziale della pagina cerca sempre per range di date (di default ho impostato gli ultimi 3 mesi, vedi riga 56), a meno che non si stia filtrando per campo libero
            {
                if (txtFiltroLibero.Text.Trim().Length == 0)
                {
                    sql += " WHERE 1=1 AND DCdata >= CONVERT(datetime,'" + txtFiltroInizio.Text + "',105) ";

                    //elimino il limite superiore di data: alcune fatture possono avere date posteriori a quella odierna.
                    //sql += " AND DCdata <= CONVERT(datetime,'" + txtFiltroFine.Text + "',105)  ";
                }
            }

            sql += " ORDER BY DCID DESC ";
            DTResult = new DataTable();

            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            //string sqlCollegate = "select PRNUMERO, prfattura from prestazioni";
            //DataTable DTCollegate = new DataTable();
            //clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sqlCollegate, "RESULT", ref DTCollegate, true);

            //var resultsFatture = from myRowFattura in DTResult.AsEnumerable()
            //                     select myRowFattura;

            //foreach (var rowFattura in resultsFatture)
            //{
            //    var resultsColl = from myRowCollegata in DTCollegate.AsEnumerable()
            //                      where myRowCollegata["prfattura"].ToString() == rowFattura["DCId"].ToString()
            //                      select myRowCollegata;

            //    foreach (var row in resultsColl)
            //    {
            //        string LinkPrest = "";
            //        LinkPrest = "Prestazione : <a href='Prestazioni.aspx?FilterNumero=" + row["PRNUMERO"].ToString() + "'>" + row["PRNUMERO"].ToString() + "</a><BR>";
            //        rowFattura["PRESTAZIONICOLLEGATE"] += LinkPrest;
            //    }
            //}

            //for (int i = 0; i < DTResult.Rows.Count; i++)
            //{
            //    sql = "select PRNUMERO from prestazioni where prfattura = " + DTResult.Rows[i]["DCId"].ToString();
            //    DataTable DTCollegate = new DataTable();
            //    clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTCollegate, true);
            //    for (int y = 0; y < DTCollegate.Rows.Count; y++)
            //    {
            //        string LinkPrest="";
            //        LinkPrest = "Prestazione : " + "<a href='Prestazioni.aspx?FilterNumero=" + DTCollegate.Rows[y]["PRNUMERO"].ToString() + "'>" + DTCollegate.Rows[y]["PRNUMERO"].ToString() + "</a><BR>";
            //        DTResult.Rows[i]["PRESTAZIONICOLLEGATE"] += LinkPrest;
            //    }
            //}

            try
            {
                if (gridData.CurrentPageIndex >= ((double)DTResult.Rows.Count / (double)gridData.PageSize))
                {
                    gridData.CurrentPageIndex -= 1;
                }
            }
            catch { }

            gridData.DataSource = DTResult;
            gridData.DataBind();
            for (int i = 0; i <= gridData.Items.Count - 1; i++)
            {

            }

            for (int i = 0; i <= gridData.Items.Count - 1; i++)
            {
                ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole davvero eliminare la Fattura selezionata/a ?')){}else{return false}");
            }
            objPagining.CaricaPagining(phPagine, gridData);
        }
        #endregion

        #region PROPERTY
        private bool ModificaVisible
        {
            get
            {
                return TBLModifica.Visible;
            }
            set
            {
                TBLModifica.Visible = value; gridData.Enabled = !value; phPagine.Visible = !value; lnkInserisci.Enabled = !value;
                for (int i = 0; i <= gridData.Items.Count - 1; i++)
                {
                    ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Visible = !value;
                }
                string pstrToolTip = "";
                if (value)
                {
                    pstrToolTip = "Modalità di Modifica/Inserimento attiva. Prima di procedere confermare o annullare l'operazione.";
                }
                else
                {
                    ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Dettaglio;
                }
                gridData.ToolTip = pstrToolTip;
                lnkInserisci.ToolTip = pstrToolTip;
            }
        }
        #endregion

        protected void lnkCaricaDATI_Click(object sender, EventArgs e)
        {
            CambioCliente();
        }


        private void CaricaCampiCliente(bool onlyInUso)
        {
            //Carico clienti in Combo

            if (onlyInUso)
            {
                if (txtFiltro.Text != "")
                {
                    sql = @"SELECT * FROM CLIENTI WHERE (cliragsoc like '%" + txtFiltro.Text + "%' or cliprenome like '%" + txtFiltro.Text + "%') AND CliInUso = 1 "; /* aggiunto "AND" mancante */
                    sql += " ORDER BY cliragsoc ";
                }
                else
                {
                    sql = @"SELECT * FROM CLIENTI WHERE CliInUso = 1";
                    sql += " ORDER BY cliragsoc ";
                }
            }
            else
            {
                if (txtFiltro.Text != "")
                {
                    sql = @"SELECT * FROM CLIENTI WHERE (cliragsoc like '%" + txtFiltro.Text + "%' or cliprenome like '%" + txtFiltro.Text + "%') ";
                    sql += " ORDER BY cliragsoc ";
                }
                else
                {
                    sql = @"SELECT * FROM CLIENTI ";
                    sql += " ORDER BY cliragsoc ";
                }
            }

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cboCliente.Items.Clear();
            ListItem myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Selezionare un cliente --";
            cboCliente.Items.Add(myItem);

            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["clicodice"];
                myItem.Text = "" + DTResult.Rows[i]["clicodice"] + " - " + DTResult.Rows[i]["cliPRENOME"] + " , " + DTResult.Rows[i]["cliragsoc"];
                //myItem.Text  = "" + DTResult.Rows[i]["clicodice"] + " - " + DTResult.Rows[i]["cliragsoc"];
                cboCliente.Items.Add(myItem);
            }

            cboCliente.SelectedIndex = 0;

            lblPostaCAP.Text = "";
            lblPostaINDIRIZZO.Text = "";
            lblPostaLOCALITA.Text = "";
            lblPostaNAZIONE.Text = "";
            lblPostaPROVINCIA.Text = "";
            lblPostaRAGSOC.Text = "";

            txtCliBanca.Text = "";
            txtCliPiazza.Text = "";
            txtCliSwift.Text = "";
        }

        protected void lnkCercaCLIENTE_Click(object sender, EventArgs e)
        {
            //Carico clienti in Combo
            CaricaCampiCliente(true);
        }
        protected void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            CambioCliente();
        }

        private void CambioCliente()
        {
            //Carico dati del cliente
            string CLICODICE = cboCliente.SelectedValue;

            //Carico clienti in Combo
//            sql = @"select clipagamento
//                --,CASE WHEN (NOT CliNumDicIntenti IS NULL AND CliDataDicIntenti < GETDATE()) THEN 0 ELSE cliperciva END as cliperciva -- FC 20180312 Gestione IVA 0% se Dichiarazione intenti compilata
//                ,cliperciva
//                ,clipercdiritti
//                ,ISNULL(Cliragsoc,Clipostaragsoc) as Clipostaragsoc
//                ,ISNULL(Cliindirizzo,ClipostaIndirizzo) as ClipostaIndirizzo
//                ,ISNULL(Clilocalita,Clipostalocalita) as Clipostalocalita
//                ,ISNULL(Cliprovincia,Clipostaprov) as Clipostaprov
//                ,ISNULL(CliCAP,ClipostaCAP) as ClipostaCAP
//                ,ISNULL(CliNazione,ClipostaNazione) as ClipostaNazione
//                ,B.Banca as CliBanca -- ISNULL(CliBanca,'') as CliBanca
//                ,ISNULL(CliIban,'') as CliPiazza
//                , ISNULL(CliSwift,'') as CliSwift
//                , GACalcoloIva
//                FROM CLIENTI C
//                INNER JOIN [dbo].[Banche] B ON B.SWIFT = C.CliSWIFT
//                WHERE clicodice = " + CLICODICE + " ";

            //con la inner se non c'è la banca non si trova il cliente, cambiata a left
            sql = @"select clipagamento
                --,CASE WHEN (NOT CliNumDicIntenti IS NULL AND CliDataDicIntenti < GETDATE()) THEN 0 ELSE cliperciva END as cliperciva -- FC 20180312 Gestione IVA 0% se Dichiarazione intenti compilata
                ,cliperciva
                ,clipercdiritti
                ,ISNULL(Cliragsoc,Clipostaragsoc) as Clipostaragsoc
                ,ISNULL(Cliindirizzo,ClipostaIndirizzo) as ClipostaIndirizzo
                ,ISNULL(Clilocalita,Clipostalocalita) as Clipostalocalita
                ,ISNULL(Cliprovincia,Clipostaprov) as Clipostaprov
                ,ISNULL(CliCAP,ClipostaCAP) as ClipostaCAP
                ,ISNULL(CliNazione,ClipostaNazione) as ClipostaNazione
                ,B.Banca as CliBanca -- ISNULL(CliBanca,'') as CliBanca
                ,ISNULL(CliIban,'') as CliPiazza
                , ISNULL(CliSwift,'') as CliSwift
                , GACalcoloIva
                FROM CLIENTI C
                LEFT JOIN [dbo].[Banche] B ON B.SWIFT = C.CliSWIFT
                WHERE clicodice = " + CLICODICE + " ";

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            lblC_DCdiritti.Text = "0";
            lblC_DCimponibile.Text = "0";
            lblC_DCIVA.Text = "0";
            lblC_DCspese.Text = "0";
            lblC_DCtotale.Text = "0";
            lblC_DCtrattenute.Text = "0";
            lblC_DCanticipi.Text = "0";

            if (DTResult.Rows.Count > 0)
            {
                for (int i = 0; i <= cboPagamento.Items.Count - 1; i++)
                {
                    if (cboPagamento.Items[i].Value.ToString() == "" + DTResult.Rows[0]["clipagamento"])
                    {
                        cboPagamento.SelectedIndex = i;
                        break;
                    }
                }

                ViewState["CALCOLOIVA"] = Convert.ToBoolean(DTResult.Rows[0]["GACalcoloIva"]);
                lblPostaCAP.Text = "" + DTResult.Rows[0]["ClipostaCAP"];
                lblPostaINDIRIZZO.Text = "" + DTResult.Rows[0]["ClipostaIndirizzo"];
                lblPostaLOCALITA.Text = "" + DTResult.Rows[0]["Clipostalocalita"];
                lblPostaNAZIONE.Text = "" + DTResult.Rows[0]["ClipostaNazione"];
                lblPostaPROVINCIA.Text = "" + DTResult.Rows[0]["Clipostaprov"];
                lblPostaRAGSOC.Text = "" + DTResult.Rows[0]["Clipostaragsoc"];
                txtCliBanca.Text = "" + DTResult.Rows[0]["CliBanca"];
                txtCliPiazza.Text = "" + DTResult.Rows[0]["CliPiazza"];
                txtCliSwift.Text = "" + DTResult.Rows[0]["CliSwift"];

                lblC_DCpercdiritti.Text = "" + DTResult.Rows[0]["clipercdiritti"];
                if (lblC_DCpercdiritti.Text == "") lblC_DCpercdiritti.Text = "0";
                txtManualPercDiritti.Text = lblC_DCpercdiritti.Text;


                lblC_DCpercIVA.Text = "" + DTResult.Rows[0]["cliperciva"];
                if (lblC_DCpercIVA.Text == "") lblC_DCpercIVA.Text = "20";

            }
            else
            {
                ViewState["CALCOLOIVA"] = true;
                lblPostaCAP.Text = "";
                lblPostaINDIRIZZO.Text = "";
                lblPostaLOCALITA.Text = "";
                lblPostaNAZIONE.Text = "";
                lblPostaPROVINCIA.Text = "";
                lblPostaRAGSOC.Text = "";
                txtCliBanca.Text = "";
                txtCliPiazza.Text = "";
                txtCliSwift.Text = "";

                lblC_DCpercdiritti.Text = "";
                txtManualPercDiritti.Text = lblC_DCpercdiritti.Text;

                lblC_DCpercIVA.Text = "";
            }
        }

        protected void lnkInserisciRiga_Click(object sender, EventArgs e)
        {
            if (cboCliente.SelectedIndex > 0)
            {
                ViewState["IDMOD"] = "";

                CaricaPrestazioniLibere();

                divRigheFattura.Visible = true;

                CambioCliente();

                if (gridData.SelectedIndex > -1)
                {
                    lnkInserisciRiga.Enabled = false;
                    lnkAggiorna.Enabled = false;
                }
            }
        }

        private void CaricaPrestazioniLibere()
        {
            //

            //Carico le righe collegate
            sql = "SELECT (select count(*) from servizi where srnumeroprestazione = prestazioni.prnumero) AS NumServizi, ";
            sql += " ISNULL(ModalitaPagamento.Descrizione,'------') AS PAGAMENTO, ";
            sql += @"CASE 
                            WHEN 
                            PRfatturare = 0 and PRfatturato = 0 and PRpagata = 0
                            THEN 'Emessa'
                            WHEN 
                            PRfatturare = 1 and PRfatturato = 0 and PRpagata = 0
                            THEN 'Da Fatturare'
                            WHEN 
                            PRfatturato = 1  and PRpagata = 0
                            THEN 'Fatturata'
                            WHEN 
                            PRpagata = 1 and PRfatturato = 1  
                            THEN 'Pagata'
                            END AS COLORE, ";
            //sql += " ISNULL(CAST(documclienti.DCanno as nvarchar(255)),'') as ANNO,ISNULL(CAST(documclienti.DCnumero as nvarchar(255)),'') as NUMERO, ISNULL(PRvaluta,'EUR') as VALUTA, ";
            sql += " ISNULL(CAST(documclienti.DCanno as nvarchar(255)),'') as ANNO,documclienti.DCnumeroCompleto as NUMERO, ISNULL(PRvaluta,'EUR') as VALUTA, ";
            sql += clsDB.DatePartGG_MM_AAAA("PRdatainizio", "DATAINIZIO", false) + ",";
            sql += clsDB.DatePartGG_MM_AAAA("PRdatafine", "DATAFINE", false) + ",";
            sql += clsDB.DatePartGG_MM_AAAA("PRdatafattura", "DATAFATTURA", false) + ",";
            sql += " isnull(Lista_CategoriaServizi.CSdescrizione,'------') as CATDESCR, Clienti.cliragsoc AS GREEN_Cliente , prestazioni.* FROM prestazioni ";
            sql += " inner join clienti on prestazioni.PRcliente=clienti.clicodice ";
            sql += " left join Lista_CategoriaServizi on Lista_CategoriaServizi.CScodice = prestazioni.PRcategoria ";
            sql += " left join documclienti on documclienti.DCid = prestazioni.PRfattura ";
            sql += " left join ModalitaPagamento on ModalitaPagamento.Codice = prestazioni.PRcondpag ";
            sql += " WHERE PRfattura IS NULL and PRfatturare = 1 ";
            sql += " and prestazioni.PRcliente = " + cboCliente.SelectedValue;
            sql += " ORDER BY PRnumero DESC ";

            DataTable DTPrestazioniLibere = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTPrestazioniLibere, true);

            gridPrestazioniLibere.DataSource = DTPrestazioniLibere;
            gridPrestazioniLibere.DataBind();
        }

        protected void lnkCloseRiga_Click(object sender, EventArgs e)
        {
            divRigheFattura.Visible = false;
            CalcolaTotaliFattura();

            lnkInserisciRiga.Enabled = true;
            lnkAggiorna.Enabled = true;
        }

        protected void lnkAggiornaRiga_Click(object sender, EventArgs e)
        {
            string FATTURASEL = "";
            if (Request.QueryString["DCId"] != null)
            {
                FATTURASEL = Request.QueryString["DCId"].ToString();
            }
            else
            {
                FATTURASEL = gridData.DataKeys[gridData.SelectedIndex].ToString();
            }

            sql = "select * from documclienti WHERE DCid=" + FATTURASEL;
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);
            DataRow DTRow;
            DTRow = DTResult.Rows[0];

            //Aggiorno i selezionati
            for (int i = 0; i <= gridPrestazioniLibere.Items.Count - 1; i++)
            {
                if (((CheckBox)gridPrestazioniLibere.Items[i].FindControl("chkSelPrestazione")).Checked)
                {
                    string SEL = gridPrestazioniLibere.DataKeys[i].ToString();

                    //sql = " UPDATE Prestazioni SET PRannoDoc ='" + Convert.ToDateTime(txtDataRiferimento.Text).Year.ToString() + "', PRcondpag = '" + cboPagamento.SelectedValue + "', PRfatturato=1, PRfatturare=0, PRdatafattura = CONVERT(datetime,'" + txtDataRiferimento.Text + "',105), PRFattura = " + FATTURASEL + " WHERE PRNumero = " + SEL + " ";
                    //clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                    //sql = " UPDATE Servizi SET SRfatclianno ='" + Convert.ToDateTime(txtDataRiferimento.Text).Year.ToString() + "', SRfatturato=1, SRfatturare=0, SRdatafatturacli = CONVERT(datetime,'" + txtDataRiferimento.Text + "',105), SRfatturacli = " + FATTURASEL + " WHERE SRNumeroPrestazione = " + SEL + " ";
                    //clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                }
            }

            CaricaPrestazioniCollegate(txtDataRiferimento.Text, DTRow["DCid"].ToString(), DTRow["DCnumeroprestazione"].ToString());

            divRigheFattura.Visible = false;

            lnkInserisciRiga.Enabled = true;
            lnkAggiorna.Enabled = true;



        }

        private void CalcolaTotaliFattura()
        {
            //************** CARICAMENTO ALIQUOTA IVA CONDIVISA
            string sqlALIQUOTA = "SELECT top 1 AliquotaIVA FROM Admin_DatiAnagrifici";
            DataTable DTResultALIQUOTA = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sqlALIQUOTA, "RESULTALIQUOTA", ref DTResultALIQUOTA, true);
            double ALIQUOTAIVA = double.Parse(DTResultALIQUOTA.Rows[0]["AliquotaIVA"].ToString());
            //************** CARICAMENTO ALIQUOTA IVA CONDIVISA

            double IMPO = 0;
            double SPESE = 0;
            double DIRITTI = 0;
            double IVA = 0;
            double TRATTENUTE = 0;
            double TOTALE = 0;
            double ANTICIPI = 0;

            lblC_DCimponibile.Text = "0";
            lblC_DCspese.Text = "0";
            lblC_DCdiritti.Text = "0";
            lblC_DCIVA.Text = "0";
            lblC_DCtrattenute.Text = "0";
            lblC_DCtotale.Text = "0";
            lblC_DCanticipi.Text = "0";

            string FATTURASEL = "";

            if (Request.QueryString["DCId"] != null)
            {
                FATTURASEL = Request.QueryString["DCId"].ToString();
            }
            else
            {
                FATTURASEL = gridData.DataKeys[gridData.SelectedIndex].ToString();
            }

            sql = "select * from Prestazioni WHERE PRFattura=" + FATTURASEL;
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTRigheFattura, true);

            for (int i = 0; i < DTRigheFattura.Rows.Count; i++)
            {
                DataRow DTRow = DTRigheFattura.Rows[i];

                IMPO = IMPO + double.Parse(DTRow["PRtotale"].ToString());

                try
                { SPESE = SPESE + double.Parse(DTRow["PRspese"].ToString()); }
                catch { }

                try
                { SPESE = SPESE + double.Parse(DTRow["PRspese1"].ToString()); }
                catch { }

                try
                { SPESE = SPESE + double.Parse(DTRow["PRspese2"].ToString()); }
                catch { }


                DIRITTI = DIRITTI + double.Parse(DTRow["PRdiritti"].ToString());
                IVA = IVA + double.Parse(DTRow["PRimportoIVA"].ToString());

                TRATTENUTE = TRATTENUTE + double.Parse(DTRow["PRtrattenute"].ToString());
                ANTICIPI = ANTICIPI + double.Parse(DTRow["PRanticipi"].ToString());
            }

            double res = 0;

            double CALCOLOSCONTO = 0;
            if (double.TryParse(txtManualPercSconto.Text, out res))
            {
                CALCOLOSCONTO = IMPO * Convert.ToDouble(txtManualPercSconto.Text) / 100;
                //IMPO = IMPO - CALCOLOSCONTO;
            }

            DIRITTI = IMPO * double.Parse(txtManualPercDiritti.Text) / 100;

            //APPLICO SCONTO ANCHE SU DIRITTI
            double CALCOLOSCONTO_DIRITTI = 0;
            if (double.TryParse(txtManualPercSconto.Text, out res))
            {
                CALCOLOSCONTO_DIRITTI = DIRITTI * Convert.ToDouble(txtManualPercSconto.Text) / 100;
                //DIRITTI = DIRITTI - CALCOLOSCONTO_DIRITTI;
            }

            lblImportoSconto.Text = String.Format("{0:N2}", CALCOLOSCONTO + CALCOLOSCONTO_DIRITTI);

            if (Convert.ToBoolean(ViewState["CALCOLOIVA"]))
            {
                IVA = (IMPO + DIRITTI + SPESE - CALCOLOSCONTO - CALCOLOSCONTO_DIRITTI) * ALIQUOTAIVA / 100;
            }

            TOTALE = (IMPO) + DIRITTI + SPESE + IVA;

            TOTALE = TOTALE - CALCOLOSCONTO - CALCOLOSCONTO_DIRITTI;




            lblC_DCimponibile.Text = String.Format("{0:N2}", IMPO);
            lblC_DCspese.Text = String.Format("{0:N2}", SPESE);
            lblC_DCdiritti.Text = String.Format("{0:N2}", DIRITTI);

            if (Convert.ToBoolean(ViewState["CALCOLOIVA"]))
            {
                lblC_DCIVA.Text = String.Format("{0:N2}", IVA);
            }
            else
            {
                lblC_DCIVA.Text = String.Format("{0:N2}", "0");
            }

            lblC_DCtrattenute.Text = String.Format("{0:N2}", TRATTENUTE);
            lblC_DCanticipi.Text = String.Format("{0:N2}", ANTICIPI);

            lblC_DCtotale.Text = String.Format("{0:N2}", (TOTALE));




        }

        private void CalcolaTotaliFattura_SIMULATA(string DCnumeroprestazione)
        {
            //************** CARICAMENTO ALIQUOTA IVA CONDIVISA
            string sqlALIQUOTA = "SELECT top 1 AliquotaIVA FROM Admin_DatiAnagrifici";
            DataTable DTResultALIQUOTA = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sqlALIQUOTA, "RESULTALIQUOTA", ref DTResultALIQUOTA, true);
            double ALIQUOTAIVA = double.Parse(DTResultALIQUOTA.Rows[0]["AliquotaIVA"].ToString());
            //************** CARICAMENTO ALIQUOTA IVA CONDIVISA

            double IMPO = 0;
            double SPESE = 0;
            double DIRITTI = 0;
            double IVA = 0;
            double TRATTENUTE = 0;
            double TOTALE = 0;
            double ANTICIPI = 0;

            lblC_DCimponibile.Text = "0";
            lblC_DCspese.Text = "0";
            lblC_DCdiritti.Text = "0";
            lblC_DCIVA.Text = "0";
            lblC_DCtrattenute.Text = "0";
            lblC_DCtotale.Text = "0";
            lblC_DCanticipi.Text = "0";

            sql = "select * from Prestazioni WHERE PRNumero=" + DCnumeroprestazione;
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTRigheFattura, true);

            for (int i = 0; i < DTRigheFattura.Rows.Count; i++)
            {
                DataRow DTRow = DTRigheFattura.Rows[i];

                IMPO = IMPO + double.Parse(DTRow["PRtotale"].ToString());

                try
                { SPESE = SPESE + double.Parse(DTRow["PRspese"].ToString()); }
                catch { }

                try
                { SPESE = SPESE + double.Parse(DTRow["PRspese1"].ToString()); }
                catch { }

                try
                { SPESE = SPESE + double.Parse(DTRow["PRspese2"].ToString()); }
                catch { }

                DIRITTI = DIRITTI + double.Parse(DTRow["PRdiritti"].ToString());

                IVA = IVA + double.Parse(DTRow["PRimportoIVA"].ToString());

                TRATTENUTE = TRATTENUTE + double.Parse(DTRow["PRtrattenute"].ToString());
                ANTICIPI = ANTICIPI + double.Parse(DTRow["PRanticipi"].ToString());
            }

            double res = 0;

            double CALCOLOSCONTO = 0;
            if (double.TryParse(txtManualPercSconto.Text, out res))
            {
                CALCOLOSCONTO = IMPO * Convert.ToDouble(txtManualPercSconto.Text) / 100;
                //IMPO = IMPO - CALCOLOSCONTO;
            }

            if (txtManualPercDiritti.Text == "")
                txtManualPercDiritti.Text = "0";
            DIRITTI = IMPO * double.Parse(txtManualPercDiritti.Text) / 100;

            //APPLICO SCONTO ANCHE SU DIRITTI
            double CALCOLOSCONTO_DIRITTI = 0;
            if (double.TryParse(txtManualPercSconto.Text, out res))
            {
                CALCOLOSCONTO_DIRITTI = DIRITTI * Convert.ToDouble(txtManualPercSconto.Text) / 100;
                //DIRITTI = DIRITTI - CALCOLOSCONTO_DIRITTI;
            }

            lblImportoSconto.Text = String.Format("{0:N2}", CALCOLOSCONTO + CALCOLOSCONTO_DIRITTI);

            if (Convert.ToBoolean(ViewState["CALCOLOIVA"]))
            {
                IVA = (IMPO + DIRITTI + SPESE - CALCOLOSCONTO - CALCOLOSCONTO_DIRITTI) * ALIQUOTAIVA / 100;
            }

            TOTALE = (IMPO) + DIRITTI + SPESE + IVA;

            TOTALE = TOTALE - CALCOLOSCONTO - CALCOLOSCONTO_DIRITTI;



            lblC_DCimponibile.Text = String.Format("{0:N2}", IMPO);
            lblC_DCspese.Text = String.Format("{0:N2}", SPESE);
            lblC_DCdiritti.Text = String.Format("{0:N2}", DIRITTI);

            if (Convert.ToBoolean(ViewState["CALCOLOIVA"]))
            {
                lblC_DCIVA.Text = String.Format("{0:N2}", IVA);
            }
            else
            {
                lblC_DCIVA.Text = String.Format("{0:N2}", "0");
            }
            lblC_DCtrattenute.Text = String.Format("{0:N2}", TRATTENUTE);
            lblC_DCanticipi.Text = String.Format("{0:N2}", ANTICIPI);

            lblC_DCtotale.Text = String.Format("{0:N2}", (TOTALE));
        }

        protected void gridPrestazioni_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "MODIFICA":
                    {
                        break;
                    }
                case "ELIMINA":
                    {
                        ViewState["IDMOD"] = "";

                        string SEL = gridPrestazioni.DataKeys[e.Item.ItemIndex].ToString();

                        sql = " UPDATE Prestazioni SET PRFattura = NULL, PRDATAFATTURA = NULL, PRfatturato = 0,PRfatturare=1,PRpagata=0,PRcondpag=NULL,PRtotalefattura=0,PRannoDoc=NULL WHERE PRNumero = " + SEL + " ";
                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                       // sql = " UPDATE Servizi SET SRfatclianno =NULL, SRfatturato=0, SRfatturare=1, SRdatafatturacli = NULL, SRfatturacli = NULL WHERE SRNumeroPrestazione = " + SEL + " ";
                       // clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        break;
                    }
                case "VIEW":
                    {
                        ViewState["IDMOD"] = "";

                        string SEL = gridPrestazioni.DataKeys[e.Item.ItemIndex].ToString();
                        Response.Redirect("Prestazioni.aspx?PRNumero=" + SEL);
                        break;
                    }
            }

            string FATTURASEL = "";
            if (Request.QueryString["DCId"] != null)
            {
                FATTURASEL = Request.QueryString["DCId"].ToString();
            }
            else
            {
                FATTURASEL = gridData.DataKeys[gridData.SelectedIndex].ToString();
            }

            sql = "select * from documclienti WHERE DCid=" + FATTURASEL;
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);
            DataRow DTRow;
            DTRow = DTResult.Rows[0];
            CaricaPrestazioniCollegate(DTRow["DCdata"].ToString(), DTRow["DCid"].ToString(), DTRow["DCnumeroprestazione"].ToString());

            CalcolaTotaliFattura();

        }

        protected void lnkCambioDiritti_Click(object sender, EventArgs e)
        {
            if (Request.QueryString["IDPrestazione"] != null)
            {
                CalcolaTotaliFattura_SIMULATA(Request.QueryString["IDPrestazione"].ToString());
            }
            else
            {
                CalcolaTotaliFattura();
            }
        }

        private void CaricaComboAnnoMese()
        {
            cboAnno.Items.Clear();

            for (int i = 2000; i <= (DateTime.Now.Year + 1); i++)
            {
                ListItem myItemA = new ListItem();
                myItemA.Value = "" + i;
                myItemA.Text = "" + i;
                cboAnno.Items.Add(myItemA);
            }

            for (int i = 0; i <= cboAnno.Items.Count - 1; i++)
            {
                if (cboAnno.Items[i].Value == DateTime.Now.Year.ToString())
                {
                    cboAnno.SelectedIndex = i;
                    break;
                }
            }

            cboMese.Items.Clear();

            ListItem myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Tutti i mesi --";
            cboMese.Items.Add(myItem);

            for (int i = 1; i <= 12; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + i;
                switch (i)
                {
                    case 1: myItem.Text = "Gennaio"; break;
                    case 2: myItem.Text = "Febbraio"; break;
                    case 3: myItem.Text = "Marzo"; break;
                    case 4: myItem.Text = "Aprile"; break;
                    case 5: myItem.Text = "Maggio"; break;
                    case 6: myItem.Text = "Giugno"; break;
                    case 7: myItem.Text = "Luglio"; break;
                    case 8: myItem.Text = "Agosto"; break;
                    case 9: myItem.Text = "Settembre"; break;
                    case 10: myItem.Text = "Ottobre"; break;
                    case 11: myItem.Text = "Novembre"; break;
                    case 12: myItem.Text = "Dicembre"; break;
                }
                cboMese.Items.Add(myItem);
            }

            for (int i = 0; i <= cboMese.Items.Count - 1; i++)
            {
                if (cboMese.Items[i].Value == DateTime.Now.Month.ToString())
                {
                    cboMese.SelectedIndex = i;
                    break;
                }
            }

        }

        private void SaveEFatturaXmlOnDB(string dcid, string xml)
        {
            clsParameter pParameter = new clsParameter();
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Data", DateTime.Now, SqlDbType.DateTime2, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DocumClientiId", dcid, SqlDbType.Int, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Xml", xml, SqlDbType.NVarChar, ParameterDirection.Input));

            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Id", 0, SqlDbType.Int, ParameterDirection.Output));

            sql = @"INSERT INTO FattureElettroniche 
	                (
		                Data,
                        DocumClientiId,
                        Xml
	                ) 
	                VALUES(
		                @Data,
                        @DocumClientiId,
                        @Xml
		            ); 
		            SET @Id = @@IDENTITY";

            Dictionary<string, string> Ret = new Dictionary<string, string>();

            Ret = clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
        }

        protected void lnkFiltroMeseAnno_Click(object sender, EventArgs e)
        {
            gridData.CurrentPageIndex = 0;

            ViewState["TIPOCERCA"] = "MESEANNO";
            CaricaDati("MESEANNO");
        }
        protected void lnkFiltroDate_Click(object sender, EventArgs e)
        {
            gridData.CurrentPageIndex = 0;

            ViewState["TIPOCERCA"] = "DATE";
            CaricaDati("DATE");
        }
    }
}