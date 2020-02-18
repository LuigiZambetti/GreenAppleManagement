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

/*
 * EDIT
 * cambio regole flag SRfatturare all'eliminazione della fattura fornitore
 * Differenziamento del comportamento dell'eliminazione della fattura nel caso di servizi con il flag
 *             FatturaRifferita della tabella Lista_CategoriaServizi pari a 1 (i.e. sfilate)
 */

namespace Green.Apple.Management
{
    public partial class Green_View_FatturaFornitore : Green_BaseUserControl
    {

        #region DECLARATIONS

        private string sql = "";
        clsPagining objPagining = new clsPagining();
        protected int COL_ELIMINA = 0;
        DataTable DTRigheFattura = new DataTable();
        string IDSelezionati = "";

        #endregion

        #region EVENTS


        protected void lnkCaricaDATI_Click(object sender, EventArgs e)
        {
            CambioFornitore();
        }

        protected void lnkCERCAFORNITORE_Click(object sender, EventArgs e)
        {
            //lnkCERCAFORNITORE
            //Carico Fornitori
            string sql = @"select * from Fornitori where Forragsoc LIKE '%" + txtSerFornitoreSearch.Text + "%' order by Forragsoc ";
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cboSerFornitore.Items.Clear();
            ListItem myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Fornitore --";
            cboSerFornitore.Items.Add(myItem);

            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["ForCodice"];
                myItem.Text = "" + DTResult.Rows[i]["Forragsoc"];
                cboSerFornitore.Items.Add(myItem);
            }

            cboSerFornitore.SelectedIndex = 0;
        }

        protected void cboSerFornitore_SelectedIndexChanged(object sender, EventArgs e)
        {
            CambioFornitore();
        }

        private void CambioFornitore()
        {
            //Carico dati del cliente
            string CLICODICE = cboSerFornitore.SelectedValue;

            //Carico clienti in Combo
            sql = @"select ForEsenzioneIvaArt15,Forpagamento,ISNULL(Forragsoc,'') as Forpostaragsoc
                ,ISNULL(Forindirizzo,'') as Forpostaind
                ,ISNULL(Forlocalita,'') as Forpostaloc
                ,ISNULL(Forprovincia,'') as ForPostaProv
                ,ISNULL(ForCAP,'') as ForpostCAP
                ,ISNULL(ForNazione,'') as ForPostaNazione
                ,ISNULL(Forbanca,'') as Forbanca
                ,ISNULL(ForPiazza,'') as ForPiazza
                FROM FORNITORI WHERE forcodice = " + CLICODICE + " ";

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            lblC_DCimponibile.Text = "0";
            lblC_DCIVA.Text = "0";
            lblC_DCspese.Text = "0";
            lblC_DCtotale.Text = "0";
            lblC_DCanticipi.Text = "0";

            if (DTResult.Rows.Count > 0)
            {
                txtPagamento.Text = "" + DTResult.Rows[0]["Forpagamento"];

                lblPostaCAP.Text = "" + DTResult.Rows[0]["ForpostCAP"];
                lblPostaINDIRIZZO.Text = "" + DTResult.Rows[0]["Forpostaind"];
                lblPostaLOCALITA.Text = "" + DTResult.Rows[0]["Forpostaloc"];
                lblPostaPROVINCIA.Text = "" + DTResult.Rows[0]["ForPostaProv"];
                lblPostaRAGSOC.Text = "" + DTResult.Rows[0]["Forpostaragsoc"];
                txtCliBanca.Text = "" + DTResult.Rows[0]["Forbanca"];
                txtCliPiazza.Text = "" + DTResult.Rows[0]["ForPiazza"];

                HDlblIMPOEsenteIVA.Text = "" + DTResult.Rows[0]["ForEsenzioneIvaArt15"];
            }
            else
            {
                txtPagamento.Text = "";
                lblPostaCAP.Text = "";
                lblPostaINDIRIZZO.Text = "";
                lblPostaLOCALITA.Text = "";
                lblPostaPROVINCIA.Text = "";
                lblPostaRAGSOC.Text = "";
                txtCliBanca.Text = "";
                txtCliPiazza.Text = "";

                lblC_DCpercIVA.Text = "";
            }

            //RIPORTO ULTIMA DESCRIZIONE FATTURA
            sql = @"select top 1 DFdescfattura,dcid from documfornitori where dffornitore = " + CLICODICE + " order by dcid desc ";
            DataTable DTLastFattura = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "LASTFATTURA", ref DTLastFattura, true);
            if (DTLastFattura != null)
            {
                if (DTLastFattura.Rows.Count > 0)
                {
                    txtDESCR_FATT.Text = "" + DTLastFattura.Rows[0]["DFdescfattura"];
                }
            }

            sql = "select * from Fornitori WHERE ForCodice = " + CLICODICE;
            DataTable DTFornitore = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "FORNITORE", ref DTFornitore, true);

            if (DTFornitore != null)
            {
                if (DTFornitore.Rows.Count > 0)
                {
                    lblIMPOPrev.Text = "" + DTFornitore.Rows[0]["ForPctRivPrev"];
                    HDlblIMPOPrev.Value = "" + DTFornitore.Rows[0]["ForPctRivPrev"];

                    lblIMPOIva.Text = "" + DTFornitore.Rows[0]["ForpercIVA"];
                    HDlblIMPOIva.Value = "" + DTFornitore.Rows[0]["ForpercIVA"];

                    lblIMPORiten.Text = "" + DTFornitore.Rows[0]["Forpercritenuta"];
                    HDlblIMPORiten.Value = "" + DTFornitore.Rows[0]["Forpercritenuta"];

                    HDlblIMPOEsenteIVA.Text = "" + DTFornitore.Rows[0]["ForEsenzioneIvaArt15"];

                }
            }
            else
            {
                lblIMPOPrev.Text = "0";
                HDlblIMPOPrev.Value = "0";

                lblIMPOIva.Text = "0";
                HDlblIMPOIva.Value = "0";

                lblIMPORiten.Text = "0";
                HDlblIMPORiten.Value = "0";
            }

            //RIPORTO DESCRIZIONE SPESE - Contenuto in CalcolaTotaliFattura

            CalcolaTotaliFattura();
        }

        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            clsFunctions.AssegnaEventoCalendar(imgtxtDataRiferimento, txtDataRiferimento, false);
            clsFunctions.AssegnaEventoCalendar(imgtxtDataPagamento, txtDataPagamento, false);

            clsFunctions.AssegnaEventoCalendar(imgtxtFiltroFine, txtFiltroFine, false);
            clsFunctions.AssegnaEventoCalendar(imgtxtFiltroInizio, txtFiltroInizio, false);


            COL_ELIMINA = 3;

            gridData.DataKeyField = "DCId";

            objPagining.CaricaPagining(phPagine, gridData);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            lblError.Text = "";
            lblError.Visible = false;
            if (!IsPostBack)
            {
                ViewState["TIPOCERCA"] = "";
                ViewState["MODIFICAFATTURA"] = "NO";
                CaricaComboAnnoMeseFiltri();
                txtFiltroInizio.Text = DateTime.Now.AddMonths(-3).ToShortDateString(); // ho impostato di base gli ultimi 3 mesi con .AddMonths(-3)
                txtFiltroFine.Text = DateTime.Now.ToShortDateString();

                if (!string.IsNullOrEmpty(Request.QueryString["IDRef"]))
                {
                    txtFiltroLibero.Text = Request.QueryString["IDRef"].ToString();
                }

                IDSelezionati = "";
                ModificaVisible = false;
                gridData.Visible = true;
                CaricaDati(ViewState["TIPOCERCA"].ToString());
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
                        ViewState["MODIFICAFATTURA"] = "SI";
                        lnkAggiorna.Enabled = true;
                        ModificaVisible = true;
                        gridData.SelectedIndex = e.Item.ItemIndex;

                        string FATTURASEL = gridData.DataKeys[e.Item.ItemIndex].ToString();

                        sql = "select * from documfornitori WHERE DCid=" + FATTURASEL;
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        if (DTResult.Rows.Count > 0)
                        {

                            DataRow DTRow;
                            DTRow = DTResult.Rows[0];

                            lnkCERCAFORNITORE_Click(new object(), new EventArgs());
                            cboSerFornitore.SelectedValue = DTRow["DFfornitore"].ToString();

                            sql = "select * from Fornitori WHERE ForCodice = " + DTRow["DFfornitore"].ToString();
                            DataTable DTFornitore = new DataTable();
                            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "FORNITORE", ref DTFornitore, true);

                            if (DTFornitore != null)
                            {
                                if (DTFornitore.Rows.Count > 0)
                                {
                                    lblIMPOPrev.Text = "" + DTFornitore.Rows[0]["ForPctRivPrev"];
                                    HDlblIMPOPrev.Value = "" + DTFornitore.Rows[0]["ForPctRivPrev"];

                                    lblIMPOIva.Text = "" + DTFornitore.Rows[0]["ForpercIVA"];
                                    HDlblIMPOIva.Value = "" + DTFornitore.Rows[0]["ForpercIVA"];

                                    lblIMPORiten.Text = "" + DTFornitore.Rows[0]["Forpercritenuta"];
                                    HDlblIMPORiten.Value = "" + DTFornitore.Rows[0]["Forpercritenuta"];

                                    HDlblIMPOEsenteIVA.Text = "" + DTFornitore.Rows[0]["ForEsenzioneIvaArt15"];

                                }
                            }
                            else
                            {
                                lblIMPOPrev.Text = "0";
                                HDlblIMPOPrev.Value = "0";

                                lblIMPOIva.Text = "0";
                                HDlblIMPOIva.Value = "0";

                                lblIMPORiten.Text = "0";
                                HDlblIMPORiten.Value = "0";
                            }

                            txtNumeroFattura.Text = "" + DTRow["DFNumero"];
                            lblPostaRAGSOC.Text = DTRow["DFpostaRagSoc"].ToString();
                            txtDataRiferimento.Text = Convert.ToDateTime(DTRow["DFdata"].ToString()).ToShortDateString();
                            txtDataPagamento.Text = Convert.ToDateTime(DTRow["DFdatapagamento"].ToString()).ToShortDateString();

                            txtPagamento.Text = DTRow["DFpagamento"].ToString();

                            lblPostaINDIRIZZO.Text = DTRow["DFpostaInd"].ToString();
                            lblPostaLOCALITA.Text = DTRow["DFpostaLoc"].ToString();
                            lblPostaPROVINCIA.Text = DTRow["DFpostaProv"].ToString();
                            lblPostaCAP.Text = DTRow["DFpostaCAP"].ToString();
                            txtCliBanca.Text = DTRow["DFbanca"].ToString();
                            txtCliPiazza.Text = DTRow["DFPiazza"].ToString();
                            txtImpSfilate.Text = DTRow["DFimportoSfilate"].ToString();
                            txtDESCR_FATT.Text = DTRow["DFdescfattura"].ToString();
                            txtDESCR_SPESE.Text = DTRow["DFDescspese"].ToString();

                            if (DTRow["DFpagata"] != null)
                            {
                                chkPagata.Checked = Convert.ToBoolean(DTRow["DFpagata"]);
                            }

                            for (int i = 0; i < cboAnno.Items.Count; i++)
                            {
                                if (cboAnno.Items[i].Value.ToString() == "" + DTRow["DFAnnoFatt"].ToString())
                                {
                                    cboAnno.SelectedIndex = i;
                                    break; ;
                                }
                            }
                            for (int i = 0; i < cboMese.Items.Count; i++)
                            {
                                if (cboMese.Items[i].Value.ToString() == "" + DTRow["DFMeseFatt"].ToString())
                                {
                                    cboMese.SelectedIndex = i;
                                    break;
                                }
                            }
                        }

                        double ImpSfilate = 0;
                        try { ImpSfilate = Convert.ToDouble(txtImpSfilate.Text); }
                        catch { }
                        CalcolaTotaliFattura(ImpSfilate);

                        ModificaVisible = true;
                        gridData.Visible = false;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;

                        break;
                    }
                case "ELIMINA":
                    {
                        //ELIMINO FATTURA
                        string DCID = gridData.DataKeys[e.Item.ItemIndex].ToString();
                        sql = "SELECT * FROM DOCUMFORNITORI ";
                        sql += " WHERE DCid = '" + DCID + "' ";
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        if (DTResult.Rows.Count > 0)
                        {
                            //TROVATA FATTURA

                            //sql originale commentato per introduzione regole su fatture clienti per determinazione Servizi.SRfatturare
                            //sql = " UPDATE Servizi SET SRFatturaFor=NULL, SRfatturare = 1, SRfatturato = 0, SRpagato=0 WHERE SRFatturaFor = '" + DCID + "' ";

                            /* Determinazione SRfatturare - START */
                            /* Promemoria link tra documenti cliente, prestazioni e servizi
                             * SELECT 
                                         SER.*
                                         , PRE.*
                                         , DOC.*

                              FROM [GREEN_PRODUZIONE].[dbo].[Documclienti] DOC
                              INNER JOIN [GREEN_PRODUZIONE].[dbo].[Prestazioni] PRE ON PRE.PRfattura = DOC.DCid
                              INNER JOIN [GREEN_PRODUZIONE].[dbo].[Servizi] SER ON SER.SRnumeroprestazione = PRE.PRnumero
                              WHERE DCID = 20274
                            */
                            clsParameter pParameters = new clsParameter();
                            Dictionary<string, string> datiFattura = new Dictionary<string, string>();
                            string curFornitore, curMese, curAnno;

                            // salvo dati fattura
                            sql = "SELECT DFfornitore, DFAnnoFatt, DFMeseFatt FROM Documfornitori WHERE DCid = " + DCID;
                            datiFattura = clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, null);
                            if (datiFattura.Count > 0)
                            {
                                curFornitore = datiFattura["DFfornitore"].ToString();
                                curMese = datiFattura["DFMeseFatt"].ToString();
                                curAnno = datiFattura["DFAnnoFatt"].ToString();

                                /* 
                                 * Differenziamento del comportamento dell'eliminazione della fattura nel caso di servizi con il flag
                                 * FatturaRifferita della tabella Lista_CategoriaServizi pari a 1 (i.e. sfilate) - START
                                 */
                                // estraggo lista dei servizi da join
                                //Aggiorno i flag nel caso in cui la prestazione NON sia Rifferita
//                                sql = @"UPDATE Servizi
//                                            SET SRFatturaFor=NULL
//                                                ,SRfatturare = CASE DC.DCPagata WHEN 1 THEN 1 ELSE 0 END
//                                                ,SRfatturato = 0
//                                                ,SRpagato = 0
//                                            FROM Documclienti DC LEFT JOIN
//                                                 Prestazioni PR ON PR.PRfattura = DC.DCid LEFT JOIN
//                                                 Servizi SR ON SR.SRnumeroprestazione = PR.PRnumero
//                                            WHERE SR.SRAnnoFatt = {0} AND SR.SRMeseFatt = {1} AND SR.SRoperatore = {2}
//                                                  AND SRcategoria in (select CSCodice from Lista_CategoriaServizi where FatturaRifferita = 0); ";

                                //i servizi (non sfilate) devono essere fatturabili se esiste una fattura cliente, anche se non è stata pagata
                                sql = @"UPDATE Servizi
                                            SET SRFatturaFor=NULL
                                                ,SRfatturare = CASE PR.PRfattura WHEN NULL THEN 0 ELSE 1 END
                                                ,SRfatturato = 0
                                                ,SRpagato = 0
                                            FROM Documclienti DC LEFT JOIN
                                                 Prestazioni PR ON PR.PRfattura = DC.DCid LEFT JOIN
                                                 Servizi SR ON SR.SRnumeroprestazione = PR.PRnumero
                                            WHERE SR.SRAnnoFatt = {0} AND SR.SRMeseFatt = {1} AND SR.SRoperatore = {2}
                                                  AND SRcategoria in (select CSCodice from Lista_CategoriaServizi where FatturaRifferita = 0); ";

                                //Aggiorno i flag nel caso in cui la prestazione sia Rifferita;
                                // NOTA: Su richiesta di GreenApple (Barbara) il flag SRfatturare deve essere sempre 1, anche se la fattura cliente non è pagata
                                sql += @"UPDATE Servizi
                                            SET SRFatturaFor=NULL
                                                ,SRfatturare = 1
                                                ,SRfatturato = 0
                                                ,SRpagato = 0
                                            FROM Documclienti DC LEFT JOIN
                                                 Prestazioni PR ON PR.PRfattura = DC.DCid LEFT JOIN
                                                 Servizi SR ON SR.SRnumeroprestazione = PR.PRnumero
                                            WHERE SR.SRAnnoFatt = {0} AND SR.SRMeseFatt = {1} AND SR.SRoperatore = {2}
                                                  AND SRcategoria in (select CSCodice from Lista_CategoriaServizi where FatturaRifferita = 1); ";
                                /* 
                                 * Differenziamento del comportamento dell'eliminazione della fattura nel caso di servizi con il flag
                                 * FatturaRifferita della tabella Lista_CategoriaServizi pari a 1 (i.e. sfilate) - END
                                 */

                                sql = String.Format(sql, /*SRAnnoFatt 0*/ curAnno, /*SRMeseFatt 1*/ curMese, /*DFfornitore 2*/ curFornitore);

                                clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                                
                            }
                            /* Determinazione SRfatturare - END */

                            sql = "DELETE FROM DOCUMFORNITORI ";
                            sql += " WHERE DCid = '" + DCID + "' ";
                            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        }

                        CaricaDati(ViewState["TIPOCERCA"].ToString());


                        break;
                    }
                case "PRINT":
                    {
                        clsParameter pParameter = new clsParameter();

                        string DCID = gridData.DataKeys[e.Item.ItemIndex].ToString();
                        sql = "SELECT * FROM DOCUMFORNITORI ";
                        sql += " WHERE DCid = '" + DCID + "' ";
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        if (DTResult.Rows.Count > 0)
                        {
                            DataRow myRow = DTResult.Rows[0];

                            string URL = "Print.aspx?CAMBIO=NO&DCID=" + DCID + "&NUMERO=&ANNO=&TIPO=FORNITORE";
                            ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("PRINTReport")).Attributes.Add("src", URL);
                        }

                        break;
                    }
                case "PRINTNOTULA":
                    {
                        clsParameter pParameter = new clsParameter();

                        string DCID = gridData.DataKeys[e.Item.ItemIndex].ToString();
                        sql = "SELECT * FROM DOCUMFORNITORI ";
                        sql += " WHERE DCid = '" + DCID + "' ";
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        if (DTResult.Rows.Count > 0)
                        {
                            DataRow myRow = DTResult.Rows[0];

                            string URL = "Print.aspx?CAMBIO=NO&DCID=" + DCID + "&NUMERO=&ANNO=&TIPO=NOTULA";
                            ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("PRINTReport")).Attributes.Add("src", URL);
                        }

                        break;
                    }
            }
        }

        protected void lnkInserisci_Click(object sender, EventArgs e)
        {
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            ModificaVisible = true;
            gridData.Visible = false;

            txtDataRiferimento.Text = DateTime.Today.ToShortDateString();
            txtDataPagamento.Text = DateTime.Today.ToShortDateString();

            txtSerFornitoreSearch.Text = "";
            cboSerFornitore.Items.Clear();
            txtNumeroFattura.Text = "";
            txtPagamento.Text = "";
            txtDESCR_FATT.Text = "";
            txtDESCR_SPESE.Text = "";
            txtFattNote.Text = "";
            txtImpSfilate.Text = "";
            lblPostaCAP.Text = "";
            lblPostaINDIRIZZO.Text = "";
            lblPostaLOCALITA.Text = "";
            lblPostaPROVINCIA.Text = "";
            lblPostaRAGSOC.Text = "";
            txtCliBanca.Text = "";
            txtCliPiazza.Text = "";

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            this.Page.Validate();
        }

        protected void lnkAnnulla_Click(object sender, EventArgs e)
        {
            ViewState["MODIFICAFATTURA"] = "NO";
            gridData.SelectedIndex = -1;
            ModificaVisible = false;
            gridData.Visible = true;
        }

        protected void lnkAggiorna_Click(object sender, EventArgs e)
        {
            bool ShowError = false;
            string Nomenclatura = "";
            string FATTURASEL = "";

            //RICALCOLO AL VOLO PER CAPIRE SE IMPORTO SFILATE INSERITO
            CalcolaTotaliFattura();

            if (ViewState["MODIFICAFATTURA"].ToString() == "NO")
            {
                string ANNO = Convert.ToDateTime(txtDataRiferimento.Text).Year.ToString();
                string NUMERO = txtNumeroFattura.Text;

                clsParameter pParameter = new clsParameter();
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFfornitore", cboSerFornitore.SelectedValue, SqlDbType.Int, ParameterDirection.Input));

                if (txtPagamento.Text != "")
                {
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpagamento", txtPagamento.Text, SqlDbType.NVarChar, ParameterDirection.Input));
                }
                else
                {
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpagamento", " ", SqlDbType.NVarChar, ParameterDirection.Input));
                }

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpostaRagSoc", lblPostaRAGSOC.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpostaInd", lblPostaINDIRIZZO.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpostaLoc", lblPostaLOCALITA.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpostaProv", lblPostaPROVINCIA.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));

                string POSTACAP = "";
                int res;
                POSTACAP = lblPostaCAP.Text;

                int Res = 0;
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpostaCAP", POSTACAP, SqlDbType.NVarChar, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFbanca", txtCliBanca.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFPiazza", txtCliPiazza.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFnumero", NUMERO, SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFAnnoFatt", cboAnno.SelectedValue, SqlDbType.SmallInt, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFMeseFatt", cboMese.SelectedValue, SqlDbType.SmallInt, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFtipo", "FATTURA", SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFdata", txtDataRiferimento.Text.Trim(), SqlDbType.DateTime, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFdatapagamento", txtDataPagamento.Text.Trim(), SqlDbType.DateTime, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFimponibile", lblC_DCimponibile.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFIVA", lblC_DCIVA.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFtotale", lblC_DCtotale.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFanticipo", lblC_DCanticipi.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFdescfattura", txtDESCR_FATT.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                double ImpSfilate = 0;

                try { ImpSfilate = Convert.ToDouble(txtImpSfilate.Text.Trim()); }
                catch { }

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFimportoSfilate", ImpSfilate, SqlDbType.Float, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFspese", lblC_DCspese.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFspese1", lblC_DCspese1.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFspese2", lblC_DCspese2.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFrivprev", lblC_DCRivPrev.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFritenuta", lblC_DCritenuta.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFesenzioneIvaArt15", false, SqlDbType.Bit, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpagata", chkPagata.Checked, SqlDbType.Bit, ParameterDirection.Input));

                //pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCId", 0, SqlDbType.Bit, ParameterDirection.Output));

                string DESCR_SPESE = "";
                if (lblC_DCspese.Text != "0,00")
                {
                    DESCR_SPESE += "Extensions ,";
                }
                if (lblC_DCspese1.Text != "0,00")
                {
                    DESCR_SPESE += "Spese Viaggio ,";
                }
                if (lblC_DCspese2.Text != "0,00")
                {
                    DESCR_SPESE += "Spese Varie ,";
                }
                if (DESCR_SPESE.Length > 0)
                {
                    DESCR_SPESE = DESCR_SPESE.Substring(0, DESCR_SPESE.Length - 1);
                }

                DESCR_SPESE = txtDESCR_SPESE.Text;

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFdescspese", DESCR_SPESE, SqlDbType.NVarChar, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCid", 0, SqlDbType.Int, ParameterDirection.Output));

                string sqlINSERT = "";
                sqlINSERT = @"INSERT INTO DOCUMFORNITORI
                        (
                            DFpagamento,DFimportoSfilate
                            ,DFesenzioneIvaArt15
                            ,DFpagata
                            ,DFdescspese
                            ,DFfornitore
                            ,DFpostaRagSoc
                            ,DFpostaInd
                            ,DFpostaLoc
                            ,DFpostaProv
                            ,DFpostaCAP
                            ,DFbanca
                            ,DFPiazza
                            ,DFnumero
                            ,DFAnnoFatt
                            ,DFMeseFatt
                            ,DFtipo
                            ,DFdata
                            ,DFdatapagamento
                            ,DFimponibile
                            ,DFIVA
                            ,DFtotale
                            ,DFanticipo
                            ,DFdescfattura
                            ,DFspese
                            ,DFspese1
                            ,DFspese2
                            ,DFrivprev
                            ,DFritenuta
                        ) 
                        VALUES(
                            @DFpagamento,@DFimportoSfilate
                            ,@DFesenzioneIvaArt15
                            ,@DFpagata
                            ,@DFdescspese
                            ,@DFfornitore
                            ,@DFpostaRagSoc
                            ,@DFpostaInd
                            ,@DFpostaLoc
                            ,@DFpostaProv
                            ,@DFpostaCAP
                            ,@DFbanca
                            ,@DFPiazza
                            ,@DFnumero
                            ,@DFAnnoFatt
                            ,@DFMeseFatt
                            ,@DFtipo
                            ,@DFdata
                            ,@DFdatapagamento
                            ,@DFimponibile
                            ,@DFIVA
                            ,@DFtotale
                            ,@DFanticipo
                            ,@DFdescfattura
                            ,@DFspese
                            ,@DFspese1
                            ,@DFspese2
                            ,@DFrivprev
                            ,@DFritenuta 
                            ); 
                            SET @DCid = SCOPE_IDENTITY()";

                Dictionary<string, string> Ret = new Dictionary<string, string>();

                Ret = clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sqlINSERT, pParameter.Parameters);

                FATTURASEL = Ret["@DCid"].ToString();

            }

            if (ViewState["MODIFICAFATTURA"].ToString() == "SI")
            {
                ShowError = false;
                FATTURASEL = gridData.DataKeys[gridData.SelectedIndex].ToString();

                clsParameter pParameter = new clsParameter();
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DCid", FATTURASEL, SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFfornitore", cboSerFornitore.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));

                string POSTACAP = "";
                int res;
                POSTACAP = lblPostaCAP.Text;

                int Res = 0;
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpostaCAP", POSTACAP, SqlDbType.NVarChar, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpagamento", txtPagamento.Text, SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpostaRagSoc", lblPostaRAGSOC.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpostaInd", lblPostaINDIRIZZO.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpostaLoc", lblPostaLOCALITA.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpostaProv", lblPostaPROVINCIA.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFbanca", txtCliBanca.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFPiazza", txtCliPiazza.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFnumero", txtNumeroFattura.Text, SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFAnnoFatt", cboAnno.SelectedValue, SqlDbType.SmallInt, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFMeseFatt", cboMese.SelectedValue, SqlDbType.SmallInt, ParameterDirection.Input));

                double ImpSfilate = 0;
                try { ImpSfilate = Convert.ToDouble(txtImpSfilate.Text.Trim()); }
                catch { }

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFimportoSfilate", ImpSfilate, SqlDbType.Float, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFtipo", "FATTURA", SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFdata", txtDataRiferimento.Text.Trim(), SqlDbType.DateTime, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFdatapagamento", txtDataPagamento.Text.Trim(), SqlDbType.DateTime, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFimponibile", lblC_DCimponibile.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFIVA", lblC_DCIVA.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFtotale", lblC_DCtotale.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFanticipo", lblC_DCanticipi.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFdescfattura", txtDESCR_FATT.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFspese", lblC_DCspese.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFspese1", lblC_DCspese1.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFspese2", lblC_DCspese2.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFrivprev", lblC_DCRivPrev.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFritenuta", lblC_DCritenuta.Text.Trim(), SqlDbType.Float, ParameterDirection.Input));

                if (chkPagata.Checked)
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpagata", 1, SqlDbType.Bit, ParameterDirection.Input));
                else
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpagata", 0, SqlDbType.Bit, ParameterDirection.Input));
                //pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFpagata", chkPagata.Checked, SqlDbType.Bit, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFesenzioneIvaArt15", false, SqlDbType.Bit, ParameterDirection.Input));

                string DESCR_SPESE = "";
                if (lblC_DCspese.Text != "0,00")
                {
                    DESCR_SPESE += "Extensions ,";
                }
                if (lblC_DCspese1.Text != "0,00")
                {
                    DESCR_SPESE += "Spese Viaggio ,";
                }
                if (lblC_DCspese2.Text != "0,00")
                {
                    DESCR_SPESE += "Spese Varie ,";
                }
                if (DESCR_SPESE.Length > 0)
                {
                    DESCR_SPESE = DESCR_SPESE.Substring(0, DESCR_SPESE.Length - 1);
                }
                DESCR_SPESE = txtDESCR_SPESE.Text;

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DFdescspese", DESCR_SPESE, SqlDbType.NVarChar, ParameterDirection.Input));


                sql = @"UPDATE DOCUMFORNITORI 
                    SET DFimportoSfilate=@DFimportoSfilate
                    ,DFpagamento = @DFpagamento
                    ,DFdescspese = @DFdescspese
                    ,DFpagata = @DFpagata
                    ,DFesenzioneIvaArt15 = @DFesenzioneIvaArt15
                    ,DFfornitore = @DFfornitore
                    ,DFpostaRagSoc = @DFpostaRagSoc
                    ,DFpostaInd = @DFpostaInd
                    ,DFpostaLoc = @DFpostaLoc
                    ,DFpostaProv = @DFpostaProv
                    ,DFpostaCAP = @DFpostaCAP
                    ,DFbanca = @DFbanca
                    ,DFPiazza = @DFPiazza
                    ,DFnumero = @DFnumero
                    ,DFAnnoFatt = @DFAnnoFatt
                    ,DFMeseFatt = @DFMeseFatt
                    ,DFtipo = @DFtipo
                    ,DFdata = @DFdata
                    ,DFdatapagamento = @DFdatapagamento
                    ,DFimponibile = @DFimponibile
                    ,DFIVA = @DFIVA
                    ,DFtotale = @DFtotale
                    ,DFanticipo = @DFanticipo
                    ,DFdescfattura = @DFdescfattura
                    ,DFspese = @DFspese
                    ,DFspese1 = @DFspese1
                    ,DFspese2 = @DFspese2
                    ,DFrivprev = @DFrivprev
                    ,DFritenuta = @DFritenuta
                    WHERE DCid = @DCid ";

                clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                //ViewState["MODIFICAFATTURA"] = "NO";
            }

            if (ShowError)
            {
                lblError.Text = "Esite già una Fattura.";
                lblError.Visible = true;
            }
            else
            {
                DataTable DTSerColl = new DataTable();

                //SE PAGATA AGGIORNO CAMPI FIGLI

                sql = @"select Lista_PrestazioniTipi.descrizione AS TIPO,'' AS FATTURA, '' AS DATAFATTURA, ";
                sql += clsDB.DatePartGG_MM_AAAA("srdatainizio", "DATAINIZIO", false) + ",";
                sql += clsDB.DatePartGG_MM_AAAA("srdatafine", "DATAFINE", false) + ",";
                sql += @" clienti.cliragsoc, fornitori.*,servizi.* from fornitori
                        inner join servizi on servizi.SROperatore=fornitori.forcodice
                        left join Lista_PrestazioniTipi on servizi.SRtiposervizio = Lista_PrestazioniTipi.idtipo
                        inner join clienti on servizi.srcliente=clienti.clicodice
                        where 1=1 ";

                sql += " AND ";
                sql += " ( ";
                sql += "     (SRAnnoFatt = " + cboAnno.SelectedValue + "  AND SRMeseFatt = " + cboMese.SelectedValue + ")";
                sql += "     OR ";
                sql += "     (SRAnnoFatt is null and (DATEPART(yyyy,SRdatainizio) = " + cboAnno.SelectedValue + " AND DATEPART(mm,SRdatainizio) = " + cboMese.SelectedValue + ")) ";
                sql += " ) ";

                sql += " and forcodice = " + cboSerFornitore.SelectedValue;
                sql += " order by SRAnnoFatt,SRMeseFatt,SRDataInizio,SRDataFine";


                clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTSerColl, true);

                for (int i = 0; i <= DTSerColl.Rows.Count - 1; i++)
                {
                    sql = " UPDATE Servizi SET SRFatturaFor = '" + FATTURASEL + "', SRfatturato = 1 WHERE SRcodice = " + DTSerColl.Rows[i]["SRcodice"].ToString();
                    clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                }

                if (chkPagata.Checked == true)
                {
                    string SEL = FATTURASEL;

                    for (int i = 0; i <= DTSerColl.Rows.Count - 1; i++)
                    {
                        sql = " UPDATE Servizi SET SRpagato = 1 WHERE SRcodice = " + DTSerColl.Rows[i]["SRcodice"].ToString();
                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                    }
                }
                else
                {
                    string SEL = FATTURASEL;

                    for (int i = 0; i <= DTSerColl.Rows.Count - 1; i++)
                    {
                        sql = " UPDATE Servizi SET SRpagato = 0 WHERE SRcodice = " + DTSerColl.Rows[i]["SRcodice"].ToString();
                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                    }
                }

                CaricaDati(ViewState["TIPOCERCA"].ToString());
                ModificaVisible = false;
                gridData.SelectedIndex = -1;
                gridData.Visible = true;
            }

            ViewState["MODIFICAFATTURA"] = "NO";
        }

        protected void lnkCerca_Click(object sender, EventArgs e)
        {
            gridData.CurrentPageIndex = 0;

            CaricaDati(ViewState["TIPOCERCA"].ToString());
        }
        #endregion

        #region FUNCTIONS
        private void CaricaComboAnnoMese()
        {
            cboAnno.Items.Clear();
            for (int i = 2000; i <= (DateTime.Now.Year + 1); i++)
            {
                ListItem myItem = new ListItem();
                myItem.Value = "" + i;
                myItem.Text = "" + i;
                cboAnno.Items.Add(myItem);
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
            for (int i = 1; i <= 12; i++)
            {
                ListItem myItem = new ListItem();
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

        private void CaricaDati()
        {
            CaricaDati("");
        }
        private void CaricaDati(string TIPO)
        {
            CaricaComboAnnoMese();

            string sql = @" 
                SELECT ModalitaPagamento.Descrizione AS PAGAMENTO, FORNITORI.forragsoc AS GREEN_Fornitore , DocumFornitori.* FROM DocumFornitori
                inner join FORNITORI on DocumFornitori.DFFornitore=FORNITORI.forcodice
                left join ModalitaPagamento on ModalitaPagamento.Codice=DocumFornitori.DFPagamento
            ";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " WHERE (DFpostaRagSoc like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR DFpostaInd like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR DFpostaLoc like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR DFNumero = '" + txtFiltroLibero.Text.Trim() + "' ";
                sql += " OR CAST(DCId as nvarchar(250)) = '" + txtFiltroLibero.Text.Trim() + "' ";
                sql += " OR FORNITORI.forragsoc like '%" + txtFiltroLibero.Text.Trim() + "%')";
            }

            if (TIPO == "MESEANNO")
            {
                if (txtFiltroLibero.Text.Trim().Length > 0)
                {
                    if (cboMeseFiltro.SelectedIndex > 0)
                    {
                        sql += " AND DFMeseFatt = " + cboMeseFiltro.SelectedValue + " AND DFAnnoFatt = " + cboAnnoFilto.SelectedValue + " ";
                        sql += " OR (MONTH(DFdata) = " + cboMeseFiltro.SelectedValue + " ";
                        sql += " AND YEAR(DFdata)=" + cboAnnoFilto.SelectedValue + ")  ";
                    }
                    else
                    {
                        sql += " AND DFAnnoFatt = " + cboAnnoFilto.SelectedValue + " ";
                        sql += " OR (MONTH(DFdata) = " + cboMeseFiltro.SelectedValue + ")  ";
                    }
                }
                else
                {
                    if (cboMeseFiltro.SelectedIndex > 0)
                    {
                        sql += " WHERE 1=1 AND DFMeseFatt = " + cboMeseFiltro.SelectedValue + " AND DFAnnoFatt = " + cboAnnoFilto.SelectedValue + " ";
                        sql += " OR (MONTH(DFdata) = " + cboMeseFiltro.SelectedValue + " ";
                        sql += " AND YEAR(DFdata)=" + cboAnnoFilto.SelectedValue + ")  ";
                    }
                    else
                    {
                        sql += " WHERE 1=1 AND DFAnnoFatt = " + cboAnnoFilto.SelectedValue + " ";
                        sql += " OR (MONTH(DFdata) = " + cboMeseFiltro.SelectedValue + ")  ";
                    }
                }
            }
            else if (TIPO == "DATE")
            {
                if (txtFiltroLibero.Text.Trim().Length > 0)
                {
                    sql += " AND DFdata >= CONVERT(datetime,'" + txtFiltroInizio.Text + "',105) ";
                    sql += " AND DFdata <= CONVERT(datetime,'" + txtFiltroFine.Text + "',105)  ";
                }
                else
                {
                    sql += " WHERE 1=1 AND DFdata >= CONVERT(datetime,'" + txtFiltroInizio.Text + "',105) ";
                    sql += " AND DFdata <= CONVERT(datetime,'" + txtFiltroFine.Text + "',105)  ";
                }
            }
            else //aggiungo questo ELSE, così all'apertura iniziale della pagina cerca sempre per range di date (di default ho impostato gli ultimi 3 mesi, vedi riga 56), a meno che non si stia filtrando per campo libero
            {
                if (txtFiltroLibero.Text.Trim().Length == 0)
                {
                    sql += " WHERE 1=1 AND DFdata >= CONVERT(datetime,'" + txtFiltroInizio.Text + "',105) ";
                    // elimino il limite superiore di data: alcune fatture possono avere date posteriori a quella odierna.
                    //sql += " AND DFdata <= CONVERT(datetime,'" + txtFiltroFine.Text + "',105)  ";
                }
            }

            //sql += " ORDER BY DFdata DESC, DFNumero DESC  ";
            sql += " ORDER BY DCid DESC  ";

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);
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
                ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare la Fattura selezionata/a ?')){}else{return false}");
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



        protected void lnkInserisciRiga_Click(object sender, EventArgs e)
        {
            ViewState["IDMOD"] = "";

            if (gridData.SelectedIndex > -1)
            {
                lnkAggiorna.Enabled = false;
            }
        }

        protected void lnkCloseRiga_Click(object sender, EventArgs e)
        {
            CalcolaTotaliFattura();

            lnkAggiorna.Enabled = true;
        }

        protected void lnkAggiornaRiga_Click(object sender, EventArgs e)
        {
            string FATTURASEL = gridData.DataKeys[gridData.SelectedIndex].ToString();
            sql = "select * from documclienti WHERE DCid=" + FATTURASEL;
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);
            DataRow DTRow;
            DTRow = DTResult.Rows[0];

            //Aggiorno i selezionati
            //CALCOLIIIIIIIIIIIIIIII


            lnkAggiorna.Enabled = true;



        }

        private void CalcolaTotaliFattura()
        {
            double ImpSfilate = 0;
            try { ImpSfilate = Convert.ToDouble(txtImpSfilate.Text); }
            catch { }

            CalcolaTotaliFattura(ImpSfilate);
        }

        private void CalcolaTotaliFattura(double ImpoSfilate)
        {
            double IMPO = 0;
            double COMPAGG = 0;

            double SPESE = 0;
            double SPESE1 = 0;
            double SPESE2 = 0;

            double DIRITTI = 0;
            double IVA = 0;
            double TRATTENUTE = 0;
            double TOTALE = 0;
            double ANTICIPI = 0;
            double IMPSFILATE = 0;

            lblC_DCimponibile.Text = "0";
            lblC_DCspese.Text = "0";

            lblC_DCspese1.Text = "0";
            lblC_DCspese2.Text = "0";

            lblC_DCIVA.Text = "0";
            lblC_DCtotale.Text = "0";
            lblC_DCRivPrev.Text = "0";

            if (cboSerFornitore.SelectedIndex <= 0) return;

            string FORNSEL = cboSerFornitore.SelectedValue;

            string sql = @"select Lista_PrestazioniTipi.descrizione AS TIPO,'' AS FATTURA, '' AS DATAFATTURA, ";
            sql += clsDB.DatePartGG_MM_AAAA("srdatainizio", "DATAINIZIO", false) + ",";
            sql += clsDB.DatePartGG_MM_AAAA("srdatafine", "DATAFINE", false) + ",";
            sql += @" clienti.cliragsoc, fornitori.*,servizi.* from fornitori
                        inner join servizi on servizi.SROperatore=fornitori.forcodice
                        left join Lista_PrestazioniTipi on servizi.SRtiposervizio = Lista_PrestazioniTipi.idtipo
                        inner join clienti on servizi.srcliente=clienti.clicodice
                        where 1=1 "; //SRfatturare = 1 and SRFatturato = 0

            sql += " AND ";
            sql += " ( ";
            sql += "     (SRAnnoFatt = " + cboAnno.SelectedValue + "  AND SRMeseFatt = " + cboMese.SelectedValue + ")";
            sql += "     OR ";
            sql += "     (SRAnnoFatt is null and (DATEPART(yyyy,SRdatainizio) = " + cboAnno.SelectedValue + " AND DATEPART(mm,SRdatainizio) = " + cboMese.SelectedValue + ")) ";
            sql += " ) ";

            sql += " and forcodice = " + FORNSEL;
            sql += " order by SRAnnoFatt,SRMeseFatt,SRDataInizio,SRDataFine";

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTRigheFattura, true);

            //RIPORTO DESCRIZIONE SPESE - Contenuto in CalcolaTotaliFattura
            string DESCR_SPESE = "";

            string ForIVA = "";
            for (int i = 0; i < DTRigheFattura.Rows.Count; i++)
            {
                DataRow DTRow = DTRigheFattura.Rows[i];

                if (txtDESCR_SPESE.Text == "")
                {
                    if (!DESCR_SPESE.ToUpper().Contains(DTRow["SRdescspese"].ToString().ToUpper()))
                    {
                        if ("" + DTRow["SRdescspese"] != "") DESCR_SPESE += "" + DTRow["SRdescspese"] + "\r\n";
                    }
                    if (!DESCR_SPESE.ToUpper().Contains(DTRow["SRdescspese1"].ToString().ToUpper()))
                    {
                        if ("" + DTRow["SRdescspese1"] != "") DESCR_SPESE += "" + DTRow["SRdescspese1"] + "\r\n";
                    }
                    if (!DESCR_SPESE.ToUpper().Contains(DTRow["SRdescspese2"].ToString().ToUpper()))
                    {
                        if ("" + DTRow["SRdescspese2"] != "") DESCR_SPESE += "" + DTRow["SRdescspese2"] + "\r\n";
                    }
                }

                ForIVA = "" + DTRow["ForIVA"];

                IMPO = IMPO + double.Parse(DTRow["SRimponibile"].ToString()) + double.Parse(DTRow["SRImportoRedazionale"].ToString());
                COMPAGG = COMPAGG + double.Parse(DTRow["SRCompensoaggiunto"].ToString());

                SPESE = SPESE + double.Parse(DTRow["SRspese"].ToString());
                SPESE1 = SPESE1 + double.Parse(DTRow["SRspese1"].ToString());
                SPESE2 = SPESE2 + double.Parse(DTRow["SRspese2"].ToString());
            }


            IMPO += ImpoSfilate;

            //HDlblIMPOIva.Value;
            //HDlblIMPOPrev.Value;
            //HDlblIMPORiten.Value;

            txtDESCR_SPESE.Text = DESCR_SPESE;

            if (Convert.ToBoolean(HDlblIMPOEsenteIVA.Text) == false)
            {
                IMPO = IMPO + SPESE + SPESE1 + SPESE2;
            }

            //CALCOLO IMPORTO PREVIDENZIALE
            double RIT_PREV = 0;
            double IVA_CAL = 0;
            double RITEN_CAL = 0;
            if (HDlblIMPOPrev.Value.ToString() != "0")
            {
                double ScorporoPerc = Convert.ToDouble(HDlblIMPOPrev.Value);
                ScorporoPerc = 1 + (ScorporoPerc / 100);

                //RIT_PREV = (IMPO * Convert.ToDouble(HDlblIMPOPrev.Value) / 100); //CALCOLO I 4%
                RIT_PREV = IMPO - (IMPO / ScorporoPerc);
                IMPO = IMPO / ScorporoPerc;

                //RIT_PREV = (IMPO - RIT_PREV) * Convert.ToDouble(HDlblIMPOPrev.Value) / 100); //CALCOLO I 4%

                lblC_DCRivPrev.Text = String.Format("{0:N2}", RIT_PREV);


            }
            else
            {
                lblC_DCRivPrev.Text = String.Format("{0:N2}", 0);
            }

            IMPO = Convert.ToDouble(String.Format("{0:N2}", IMPO));
            RIT_PREV = Convert.ToDouble(String.Format("{0:N2}", RIT_PREV));


            TOTALE = IMPO + RIT_PREV;

            //if (HDlblIMPOPrev.Value.ToString() != "0")
            //{
            //    TOTALE = IMPO;
            //    IMPO = IMPO - (IMPO * Convert.ToDouble(HDlblIMPOPrev.Value) / 100);
            //    TOTALE = IMPO;
            //}
            //else
            //{
            //    TOTALE = IMPO;
            //}

            if (HDlblIMPOEsenteIVA.Text == "") HDlblIMPOEsenteIVA.Text = "False";
            if (HDlblIMPOIva.Value == "") HDlblIMPOIva.Value = "0";
            if (HDlblIMPOPrev.Value == "") HDlblIMPOPrev.Value = "0";
            if (HDlblIMPORiten.Value == "") HDlblIMPORiten.Value = "0";

            lblC_DCpercIVA.Text = HDlblIMPOIva.Value + "%";

            //CALCOLO IVA - SOMMO SE IVA
            if (Convert.ToBoolean(HDlblIMPOEsenteIVA.Text) == true)
            {
                IVA_CAL = TOTALE * Convert.ToDouble(HDlblIMPOIva.Value) / 100;
                lblC_DCIVA.Text = String.Format("{0:N2}", IVA_CAL);
                //TOTALE = TOTALE + (TOTALE * Convert.ToDouble(HDlblIMPOIva.Value) / 100);
                //lblC_DCNetto.Text = String.Format("{0:N2}", TOTALE + SPESE + SPESE1 + SPESE2);

            }
            else
            {
                IVA_CAL = (TOTALE) * Convert.ToDouble(HDlblIMPOIva.Value) / 100;
                lblC_DCIVA.Text = String.Format("{0:N2}", IVA_CAL);
                //IMPO = IMPO + SPESE + SPESE1 + SPESE2;
                //TOTALE = TOTALE + ((TOTALE + SPESE + SPESE1 + SPESE2) * Convert.ToDouble(HDlblIMPOIva.Value) / 100);
                //lblC_DCNetto.Text = String.Format("{0:N2}", TOTALE);


            }

            //if (Convert.ToBoolean(HDlblIMPOEsenteIVA.Text) == false)
            //{

            //}
            //else
            //{ 
            //    //ESENTE IVA
            //    lblC_DCIVA.Text = String.Format("{0:N2}", 0);
            //    lblC_DCNetto.Text = String.Format("{0:N2}", TOTALE + SPESE + SPESE1 + SPESE2);
            //}

            IVA_CAL = Convert.ToDouble(String.Format("{0:N2}", IVA_CAL));
            TOTALE = TOTALE + IVA_CAL;

            //CALCOLO RITENUTA
            if (HDlblIMPORiten.Value.ToString() != "0")
            {
                //DETRAGGO SE RITENUTA
                if (RIT_PREV > 0)
                {
                    RITEN_CAL = (IMPO + RIT_PREV) * Convert.ToDouble(HDlblIMPORiten.Value) / 100;
                }
                else
                {
                    RITEN_CAL = IMPO * Convert.ToDouble(HDlblIMPORiten.Value) / 100;
                }

                lblC_DCritenuta.Text = String.Format("{0:N2}", RITEN_CAL);
            }
            else
            {
                RITEN_CAL = 0;
                lblC_DCritenuta.Text = String.Format("{0:N2}", 0);
            }

            //TOTALE = TOTALE - RITEN_CAL;

            if (Convert.ToBoolean(HDlblIMPOEsenteIVA.Text) == true)
            {
                lblC_DCNetto.Text = String.Format("{0:N2}", TOTALE - RITEN_CAL + SPESE + SPESE1 + SPESE2);
            }
            else
            {
                // su richiesta di GreenApple (Barbara) esponiamo il Netto come nella stampa fattura: punto 5 alta priorità
                //lblC_DCNetto.Text = String.Format("{0:N2}", TOTALE - RITEN_CAL - SPESE - SPESE1 - SPESE2);
                lblC_DCNetto.Text = String.Format("{0:N2}", TOTALE - RITEN_CAL);
            }


            lblC_DCimponibile.Text = String.Format("{0:N2}", IMPO);
            lblC_DCspese.Text = String.Format("{0:N2}", SPESE);
            lblC_DCspese1.Text = String.Format("{0:N2}", SPESE1);
            lblC_DCspese2.Text = String.Format("{0:N2}", SPESE2);

            lblC_DCanticipi.Text = String.Format("{0:N2}", ANTICIPI);

            lblC_DCtotale.Text = String.Format("{0:N2}", TOTALE);
        }

        protected void cboAnno_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcolaTotaliFattura();
        }
        protected void cboMese_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcolaTotaliFattura();
        }

        private void CaricaComboAnnoMeseFiltri()
        {
            cboAnnoFilto.Items.Clear();

            for (int i = 2000; i <= (DateTime.Now.Year + 2); i++)
            {
                ListItem myItemA = new ListItem();
                myItemA.Value = "" + i;
                myItemA.Text = "" + i;
                cboAnnoFilto.Items.Add(myItemA);
            }

            for (int i = 0; i <= cboAnnoFilto.Items.Count - 1; i++)
            {
                if (cboAnnoFilto.Items[i].Value == DateTime.Now.Year.ToString())
                {
                    cboAnnoFilto.SelectedIndex = i;
                    break;
                }
            }

            cboMeseFiltro.Items.Clear();

            ListItem myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Tutti i mesi --";
            cboMeseFiltro.Items.Add(myItem);

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
                cboMeseFiltro.Items.Add(myItem);
            }

            for (int i = 0; i <= cboMeseFiltro.Items.Count - 1; i++)
            {
                if (cboMeseFiltro.Items[i].Value == DateTime.Now.Month.ToString())
                {
                    cboMeseFiltro.SelectedIndex = i;
                    break;
                }
            }

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