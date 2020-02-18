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

namespace Green.Apple.Management
{
    public partial class Green_View_SchedaFornitore : Green_BaseUserControl
    {

        #region DECLARATIONS

        private string sql = "";
        protected int COL_ELIMINA = 0;
        protected int COL_MODIFICA = 0;

        #endregion

        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            lnkANNULLASCHEDA.Attributes.Add("onclick", "if(confirm('Si vuole eliminare lo stato di TUTTI i servizi visualizzati ?')){}else{return false}");

            clsFunctions.AssegnaEventoCalendar(imgDataRiferimento, txtDataRiferimento, false);

            clsFunctions.AssegnaEventoCalendar(imgDataInizio, txtDataInizio, false);
            clsFunctions.AssegnaEventoCalendar(imgtxtDataFine, txtDataFine, false);

            COL_ELIMINA = 15;
            COL_MODIFICA = 14;
            //clsFunctions.AssegnaEventoCalendar(imgtxtSerDataFatturaFor, txtSerDataFatturaFor, false);

            grdServizi.DataKeyField = "SRcodice";
            grdFornitori.DataKeyField = "Forcodice";

            //lblError.Text = "";
            //lblError.Visible = false;
            if (!IsPostBack)
            {
                txtDataInizio.Text = DateTime.Now.ToShortDateString();
                txtDataFine.Text = DateTime.Now.ToShortDateString();

                lblFiltroSel.Text = "";
                lnkFilterRemove.Visible = false;
                ViewState["SCHEDA"] = "SI";
                ViewState["FILTER_STATUS"] = "";
                RWTotaliServizi.Visible = false;
                ModificaServizioVisible = false;

                RWANNULLASCHEDA.Visible = false;
                grdServizi.Visible = false;
                grdFornitori.Visible = true;

                CaricaComboAnnoMese();
                CaricaServiziCollegati("");
            }
            else
            {
                if (chkDataRiferimento.Checked == true)
                {
                    txtDataRiferimento.Style.Add("display", "");
                    imgDataRiferimento.Style.Add("display", "");
                }
                else
                {
                    txtDataRiferimento.Style.Add("display", "none");
                    imgDataRiferimento.Style.Add("display", "none");
                }
                    
            }
        }

        protected new void Page_PreRender(object sender, EventArgs e)
        {
            if (chkDataRiferimento.Checked == true)
            {
                txtDataRiferimento.Style.Add("display", "");
                imgDataRiferimento.Style.Add("display", "");
            }
            else
            {
                txtDataRiferimento.Style.Add("display", "none");
                imgDataRiferimento.Style.Add("display", "none");
            }
        }

        private void CaricaComboAnnoMese()
        {
            cboAnno.Items.Clear();
            cboSerAnno.Items.Clear();

            //ListItem myItem = new ListItem();
            //myItem.Value = "0";
            //myItem.Text = "-- Tutti gli anni --";
            //cboAnno.Items.Add(myItem);

            //aggiunto un anno su richiesta del cliente (year + 2)
            for (int i = 2000; i <= (DateTime.Now.Year + 2); i++)
            {
                ListItem myItemA = new ListItem();
                myItemA.Value = "" + i;
                myItemA.Text = "" + i;
                cboAnno.Items.Add(myItemA);
                cboSerAnno.Items.Add(myItemA);
            }

            for (int i = 0; i <= cboAnno.Items.Count-1; i++)
            {
                if (cboAnno.Items[i].Value == DateTime.Now.Year.ToString())
                {
                    cboAnno.SelectedIndex = i;
                    break;
                }
            }

            cboMese.Items.Clear();
            cboSerMese.Items.Clear();

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
                cboSerMese.Items.Add(myItem);
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
         
        protected void lnkCerca_Click(object sender, EventArgs e)
        {
            CaricaServiziCollegati("");
        }
        #endregion

        

        #region PROPERTY
     
        private bool ModificaServizioVisible
        {
            get
            {
                return RWServizioMOD.Visible;
            }
            set
            {
                RWServizioMOD.Visible = value; grdServizi.Enabled = !value;
                for (int i = 0; i <= grdServizi.Items.Count - 1; i++)
                {
                    ((LinkButton)grdServizi.Items[i].Cells[COL_ELIMINA].Controls[0]).Visible = !value;
                }
                string pstrToolTip = "";
                if (value)
                {
                    pstrToolTip = "Modalità di Modifica/Inserimento attiva. Prima di procedere confermare o annullare l'operazione.";
                }
                else
                {

                }

                RWANNULLASCHEDA.Visible = !value;
                
                grdServizi.Visible = !value;
                grdServizi.ToolTip = pstrToolTip;
            }
        }
        #endregion

       

        protected void grdServizi_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "MODIFICA":
                    {   
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        grdServizi.SelectedIndex = e.Item.ItemIndex;
                        
                        string SERSEL = grdServizi.DataKeys[e.Item.ItemIndex].ToString();
                        ViewState["SERSEL"] = SERSEL;

                        sql = "select * from Servizi WHERE srcodice = " + SERSEL;
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        if (DTResult.Rows.Count > 0)
                        {
                            DataRow DTRow;
                            DTRow = DTResult.Rows[0];

                            for (int i = 0; i < cboSerAnno.Items.Count; i++)
                            {
                                if (cboSerAnno.Items[i].Value == "" + DTRow["SRAnnoFatt"])
                                {
                                    cboSerAnno.SelectedIndex = i;
                                    break;
                                }
                            }

                            for (int i = 0; i < cboSerMese.Items.Count; i++)
                            {
                                if (cboSerMese.Items[i].Value == "" + DTRow["SRMeseFatt"])
                                {
                                    cboSerMese.SelectedIndex = i;
                                    break;
                                }
                            }
                        }

                        ModificaServizioVisible = true;
                        RWANNULLASCHEDA.Visible = false;
                
                        grdServizi.Visible = false;

                        break;
                    }
                case "ELIMINA":
                    {
                        grdServizi.SelectedIndex = e.Item.ItemIndex;

                        string SERSEL = grdServizi.DataKeys[e.Item.ItemIndex].ToString();

                        sql = "DELETE FROM Servizi WHERE srcodice = " + SERSEL;
                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        CaricaServiziCollegati("");

                        break;
                    }
            }
        }

        #region FUNCTIONS

        protected void lnkSerAggiorna_Click(object sender, EventArgs e)
        {
            //AGGIORNAMENTO DEL SERVIZIO
            bool ShowError = false;

            
                clsParameter pParameter = new clsParameter();
                //pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRfatturafor", txtSerFatturaFor.Text, SqlDbType.NVarChar, ParameterDirection.Input));
                
                //if (txtSerDataFatturaFor.Text != "")
                //{
                //    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRdatafatturafor", txtSerDataFatturaFor.Text, SqlDbType.DateTime, ParameterDirection.Input));
                //}
                //else
                //{
                //    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRdatafatturafor", System.DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input));
                //}

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRAnnoFatt", cboSerAnno.SelectedValue, SqlDbType.SmallInt, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRMeseFatt", cboSerMese.SelectedValue, SqlDbType.SmallInt, ParameterDirection.Input));
                
                ShowError = false;

                sql = @"UPDATE Servizi
                    SET
                    SRAnnoFatt = @SRAnnoFatt, 
                    SRMeseFatt = @SRMeseFatt
                    WHERE SRcodice = " + ViewState["SERSEL"].ToString() + @"
                   ";

                clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                
                ModificaServizioVisible = false;
                CaricaServiziCollegati("");
                grdServizi.SelectedIndex = -1;
                RWANNULLASCHEDA.Visible = true;
                
                grdServizi.Visible = true;

                ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Dettaglio; 

        }

        protected void lnkSerAnnulla_Click(object sender, EventArgs e)
        {
            grdServizi.SelectedIndex = -1;
            ModificaServizioVisible = false;
            RWANNULLASCHEDA.Visible = true;
                
            grdServizi.Visible = true;
            CaricaServiziCollegati("");
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
        }

        private void CaricaSchede(string FILTRO)
        {
            sql = "select distinct fornitori.*,0.0 as conto,0.0 as SUMIMPLORDO,0.0 as SUMIMPFATTURATO,0.0 as SUMIMPDIRITTI from fornitori";
            sql += " inner join servizi on servizi.SROperatore=fornitori.forcodice";
            sql += " where  1=1 "; //SRfatturare = 1 and SRFatturato = 0

            if (FILTRO == "")
            {
                sql += " AND ";
                sql += " ( ";
                sql += "     (SRAnnoFatt = " + cboAnno.SelectedValue + "  AND SRMeseFatt = " + cboMese.SelectedValue + ")";
                sql += "     OR ";
                sql += "     (SRAnnoFatt is null and (DATEPART(yyyy,SRdatainizio) = " + cboAnno.SelectedValue + " AND DATEPART(mm,SRdatainizio) = " + cboMese.SelectedValue + ")) ";
                sql += " ) ";
            }
            else
            {
                sql += " AND SRdatainizio >= CONVERT(datetime,'" + txtDataInizio.Text + "',105) ";
                sql += " AND SRdatainizio <= CONVERT(datetime,'" + txtDataFine.Text + "',105) ";
            }

            if (ViewState["FILTER_STATUS"].ToString() != "" && ViewState["FILTER_STATUS"].ToString() != "ESCLUDIPAGATI")
            {
                sql += " AND CASE ";
                sql += " WHEN ";
                sql += " SRfatturare = 0 and SRfatturato = 0 and SRpagato = 0 ";
                sql += " THEN 'ESEGUITO' ";
                sql += " WHEN ";
                sql += " SRfatturare = 1 and SRfatturato = 0 and SRpagato = 0 ";
                sql += " THEN 'DA FATTURARE' ";
                sql += " WHEN ";
                sql += " SRfatturato = 1  and SRpagato = 0 ";
                sql += " THEN 'FATTURATA' ";
                sql += " WHEN ";
                sql += " SRpagato = 1 ";
                sql += " THEN 'PAGATA' ";
                sql += " END  = '" + ViewState["FILTER_STATUS"].ToString() + "' ";
            }
            else if (ViewState["FILTER_STATUS"].ToString() == "ESCLUDIPAGATI")
            {
                sql += " AND CASE ";
                sql += " WHEN ";
                sql += " SRpagato = 0 and fornitori.Forragsoc != 'CREDITI DIVERSI' ";
                sql += " THEN 'ESCLUDIPAGATI' ";
                sql += " END  = '" + ViewState["FILTER_STATUS"].ToString() + "' ";
            }
            

            if (cboSerFornitore.SelectedIndex > 0)
            {
                sql += " AND fornitori.forcodice = " + cboSerFornitore.SelectedValue + " ";
            }

            sql += " ORDER BY fornitori.forragsoc ";

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            int Conteggio = 0;
            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                //SRimportoLordo
                sql = "select isnull(count(SRCOdice),0) AS CONTO ,SUM(SRimportoLordo) as SUMIMPLORDO ,SUM(SRImporto) as SUMIMPFATTURATO ,SUM(SRDiritti) as SUMIMPDIRITTI from servizi ";
                sql += " where servizi.SROperatore = " + DTResult.Rows[i]["FORCODICE"].ToString();
                sql += " AND ";
                sql += " ( ";
                sql += "     (SRAnnoFatt = " + cboAnno.SelectedValue + "  AND SRMeseFatt = " + cboMese.SelectedValue + ")";
	            sql += "     OR ";
                sql += "     (SRAnnoFatt is null and (DATEPART(yyyy,SRdatainizio) = " + cboAnno.SelectedValue + " AND DATEPART(mm,SRdatainizio) = " + cboMese.SelectedValue + ")) ";
                sql += " ) ";
                DataTable DTConto = new DataTable();
                clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CONTO", ref DTConto, true);
                if (DTConto != null)
                {
                    if (DTConto.Rows.Count > 0)
                    {
                        if (DTConto.Rows[0]["CONTO"] != null && DTConto.Rows[0]["CONTO"].ToString() != "0")
                        {
                            Conteggio += int.Parse(DTConto.Rows[0]["CONTO"].ToString());
                            DTResult.Rows[i]["CONTO"] = DTConto.Rows[0]["CONTO"].ToString();

                            DTResult.Rows[i]["SUMIMPLORDO"] = Convert.ToDouble(DTConto.Rows[0]["SUMIMPLORDO"]);
                            DTResult.Rows[i]["SUMIMPFATTURATO"] = Convert.ToDouble(DTConto.Rows[0]["SUMIMPFATTURATO"]);
                            DTResult.Rows[i]["SUMIMPDIRITTI"] = Convert.ToDouble(DTConto.Rows[0]["SUMIMPDIRITTI"]);
                            
                 
                        }
                    }
                }
            }

            grdFornitori.DataSource = DTResult;
            grdFornitori.Columns[0].HeaderText = "&nbsp;&nbsp;Totale servizi : " + Conteggio;
            grdFornitori.DataBind();

            grdFornitori.Visible = true;
            RWANNULLASCHEDA.Visible = false;
                
            grdServizi.Visible = false;
        }

        private void CaricaServiziCollegati(string FILTRO)
        {
            CaricaServiziCollegati(FILTRO, "");
        }

        private void CaricaServiziCollegati(string FILTRO,string FORNITORE)
        {
            if (ViewState["SCHEDA"].ToString() == "SI")
            {
                CaricaSchede(FILTRO);
                return;
            }

            ModificaServizioVisible = false;

            sql = "select ";
            sql += " (select ";
            sql += " 'Prestazione N° : ' + CAST(PRNumero as nvarchar(255)) ";
            sql += " + '\r\n' +  ";
            sql += " 'Tipo Servizio : ' + Lista_Tiposervizio.CSDescrizione ";
            sql += " + '\r\n' + ";
            sql += " 'Categoria Associata : ' + Lista_CategoriaServizi.CSDescrizione ";
            sql += " + '\r\n' + ";
            sql += " 'Inizio Prest. : ' + CAST(PRDataInizio as nvarchar(12)) ";
            sql += " + '\r\n' + ";
            sql += " 'Fine Prest. : ' + CAST(PRDataFine as nvarchar(12)) ";
            sql += " from prestazioni where PRNumero = servizi.SRNumeroPrestazione) ";
            sql += " as INFOSCHEDA, ";
            sql += " srimponibile + SRimportoRedazionale as SRIMPO_REDA, srfatturare,srfatturato,ISNULL(CliRagSoc,'') as SERCliente,";
            sql += " srcodice, ";
            sql += " srcliente, ";
            sql += " srcategoria, ";
            sql += " Lista_CategoriaServizi.CSDescrizione as Categoria, ";
            sql += " srtiposervizio, ";
            sql += " Lista_Tiposervizio.CSDescrizione as TipoServizio, ";
            sql += " fornitori.FORragsoc as GREEN_Fornitore,  ";
            sql += " srimporto,srimportoGA, ";
            sql += " srimponibile, ";
            sql += " srdiritti, ";
            sql += " sranticipi, ";
            sql += " srtrattenute, ";
            sql += " srfatturafor, ";
            sql += " srfatturacli, ";
            sql += " srdatafatturafor, ";
            sql += " srnumgiorni, ";
            sql += " srnumeroprestazione, ";
            sql += " SRfatclianno, ";
            sql += " SRanticipige, ";
            sql += clsDB.DatePartGG_MM_AAAA("SRdatainizio", "SRdatainizio", false) + ",";
            sql += clsDB.DatePartGG_MM_AAAA("SRdatafine", "SRdatafine", false) + ",";
            sql += " SRimportoLordo, ";
            sql += " SRpctDiritti, ";
            sql += " SRdescSpese,";
            sql += @" ISNULL(SRspese,0) as SRspese,
            SRdescSpeseGA,
            SRspeseGA,
            SRdescSpese1,
            ISNULL(SRspese1,0) as SRspese1,
            SRdescSpeseGA1,
            SRspeseGA1,
            SRdescSpese2,
            ISNULL(SRspese2,0) as SRspese2,
            SRdescSpeseGA2,
            SRspeseGA2, ";
            sql += " (CAST(ISNULL(SRspese,0) AS FLOAT) + CAST(ISNULL(SRspese1,0) AS FLOAT) + CAST(ISNULL(SRspese2,0) AS FLOAT)) as SpeseTotali, ";
            sql += " CASE  ";
            sql += " WHEN  ";
            sql += " srfatturare = 1 ";
            sql += " THEN '<img src=../customLayout/images/icoConfirm.gif border=0>' ELSE '<img src=../customLayout/images/icoClose.gif border=0>' END AS DESCsrfatturare,";

            sql += " CASE  ";
            sql += " WHEN  ";
            sql += " srfatturato = 1 ";
            sql += " THEN '<img src=../customLayout/images/icoConfirm.gif border=0><a href=\"FatturaFornitore.aspx?IDRef=' + CAST(ISNULL(SRFatturaFor,'') as nvarchar(50)) + '\"><img src=../customLayout/images/SEARCH.GIF border=0 width=16 height=16 title=' + CAST(ISNULL(SRFatturaFor,'') as nvarchar(50)) + '></a>' ELSE '<img src=../customLayout/images/icoClose.gif border=0>' END AS DESCsrfatturato,";

            sql += " CASE  ";
            sql += " WHEN  ";
            sql += " SRAnnoFatt is not null ";
            sql += " THEN '' + CAST(SRAnnoFatt as nvarchar(50)) ELSE '' END AS SRAnnoFatt,";
            sql += " CASE  ";
            sql += " WHEN  ";
            sql += " SRMeseFatt is not null ";
            sql += " THEN '-' + CAST(SRMeseFatt as nvarchar(50)) ELSE '' END AS SRMeseFatt,";
            sql += " SResenzioneIvaArt15, ";
            sql += " SRpctRivPrev, ";
            sql += " SRpctIVA, ";
            sql += " SRpctRitAcc, ";
            sql += " SRimportoRedazionale, ";
            sql += " SRimportoOvertime, SRimportoOvertimeGA, ";
            sql += " CASE  ";
            sql += " WHEN  ";
            sql += " SRfatturare = 0 and SRfatturato = 0 and SRpagato = 0 ";
            sql += " THEN 'Eseguito' ";
            sql += " WHEN  ";
            sql += " SRfatturare = 1 and SRfatturato = 0 and SRpagato = 0 ";
            sql += " THEN 'Da Fatturare' ";
            sql += " WHEN  ";
            sql += " SRfatturato = 1  and SRpagato = 0 ";
            sql += " THEN 'Fatturata' ";
            sql += " WHEN  ";
            sql += " SRpagato = 1 ";
            sql += " THEN 'Pagata' ";
            sql += " END AS COLORE  ";

            sql += " from servizi ";
            sql += " left join clienti on servizi.srcliente=clienti.clicodice ";
            sql += " inner join fornitori on servizi.sroperatore=fornitori.forcodice ";
            //sql += " left join Lista_CategoriaServizi on Lista_CategoriaServizi.CScodice = servizi.srcategoria ";
            sql += " inner join prestazioni ON servizi.SRNumeroPrestazione = prestazioni.PRNumero ";
            sql += " left join Lista_CategoriaServizi on Lista_CategoriaServizi.CScodice = prestazioni.prcategoria ";

            sql += " left join Lista_Tiposervizio on servizi.srtiposervizio = Lista_Tiposervizio.cscodice ";

            sql += " left join Documfornitori FATTURE ON FATTURE.dcid = servizi.srfatturafor ";

            sql += " WHERE 1=1 ";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " AND (fornitori.FORragsoc like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR Lista_Tiposervizio.CSDescrizione like '%" + txtFiltroLibero.Text.Trim() + "%' )";
            }
            else
            {
               
            }

            if (ViewState["FILTER_STATUS"].ToString() != "" && ViewState["FILTER_STATUS"].ToString() != "ESCLUDIPAGATI")
            {
                sql += " AND CASE ";
                sql += " WHEN ";
                sql += " SRfatturare = 0 and SRfatturato = 0 and SRpagato = 0 ";
                sql += " THEN 'ESEGUITO' ";
                sql += " WHEN ";
                sql += " SRfatturare = 1 and SRfatturato = 0 and SRpagato = 0 ";
                sql += " THEN 'DA FATTURARE' ";
                sql += " WHEN ";
                sql += " SRfatturato = 1  and SRpagato = 0 ";
                sql += " THEN 'FATTURATA' ";
                sql += " WHEN ";
                sql += " SRpagato = 1 ";
                sql += " THEN 'PAGATA' ";
                sql += " END  = '" + ViewState["FILTER_STATUS"].ToString() + "' ";
            }
            else if (ViewState["FILTER_STATUS"].ToString() == "ESCLUDIPAGATI")
            {
                sql += " AND CASE ";
                sql += " WHEN ";
                sql += " SRpagato = 0 "; // tengo i non pagati 

                // tutta questa parte sembra decisamente errata: presuppone che si vogliano estrarre come NON PAGATI anche quelli PAGATI AD OGGI, ma che ALL'EPOCA non lo erano.
                // a che pro???
                //if (FILTRO == "")
                //{
                //    //sql += " OR (SRpagato = 1 AND FATTURE.dfdata > DATEADD(s,-1,DATEADD(mm, DATEDIFF(m,0,CAST('01/' + CAST(" + cboMese.SelectedValue + " AS VARCHAR) + '/' + CAST(" + cboAnno.SelectedValue + " AS VARCHAR) AS DATE))+2,0)))) "; // considero non pagati i pagati che sono stati fatturati a partire dal primo giorno di 2 mesi dopo (es: 2015-5 --> considero non pagati i fatturati dal 1/7/2015)
                //    if (txtDataRiferimento.Text == "")
                //        sql += " OR (SRpagato = 1 AND FATTURE.dfdata > DATEADD(MONTH, DATEDIFF(MONTH, -1, GETDATE())-1, -1))) ";
                //    else
                //        sql += " OR (SRpagato = 1 AND FATTURE.dfdata > DATEADD(MONTH, DATEDIFF(MONTH, -1, '" + txtDataRiferimento.Text + "')-1, -1))) ";
                //}
                //else
                //{
                //    if (txtDataRiferimento.Text == "")
                //        sql += " OR (SRpagato = 1 AND FATTURE.dfdata > CAST('" + txtDataFine.Text + "' AS DATETIME))) "; // considero non pagati i pagati che sono stati fatturati a partire dal primo giorno dopo la data fine 
                //    else
                //        sql += " OR (SRpagato = 1 AND FATTURE.dfdata > DATEADD(MONTH, DATEDIFF(MONTH, -1, '" + txtDataRiferimento.Text + "')-1, -1))) ";
                //}
                sql += " and fornitori.Forragsoc != 'CREDITI DIVERSI' ";
                sql += " THEN 'ESCLUDIPAGATI' ";
                sql += " END  = '" + ViewState["FILTER_STATUS"].ToString() + "' ";
            }
            
            if (cboSerFornitore.SelectedIndex > 0)
            {
                sql += " AND fornitori.forcodice = " + cboSerFornitore.SelectedValue + " ";
            }

            if (FORNITORE!="")
            {
                sql += " AND fornitori.forcodice = " + FORNITORE + " ";
            }
            if (chkSommaFiltri.Checked == false)
            { 
                if (FILTRO == "")
                {
                    if (cboMese.SelectedIndex > 0)
                    {
                        sql += " AND ";
                        sql += " ( ";
                        sql += "     (SRAnnoFatt = " + cboAnno.SelectedValue + "  AND SRMeseFatt = " + cboMese.SelectedValue + ")";
                        sql += "     OR ";
                        sql += "     (SRAnnoFatt is null and (DATEPART(yyyy,SRdatainizio) = " + cboAnno.SelectedValue + " AND DATEPART(mm,SRdatainizio) = " + cboMese.SelectedValue + ")) ";
                        sql += " ) ";
                    }
                    if (cboMese.SelectedIndex == 0)
                    {
                        sql += " AND ";
                        sql += " ( ";
                        sql += "     (SRAnnoFatt = " + cboAnno.SelectedValue + ")";
                        sql += "     OR ";
                        sql += "     (SRAnnoFatt is null and (DATEPART(yyyy,SRdatainizio) = " + cboAnno.SelectedValue + ")) ";
                        sql += " ) ";
                    }
                }
                else
                {
                    sql += " AND SRdatainizio >= CONVERT(datetime,'" + txtDataInizio.Text + "',105) ";
                    sql += " AND SRdatainizio <= CONVERT(datetime,'" + txtDataFine.Text + "',105)  ";
                }
            }
            else
            {
                // uso insieme il filtro ANNO/MESE di competenza + DataInizio/DataFine dei servizi
                sql += " AND ";
                sql += " ( ";
                sql += "     (SRAnnoFatt = " + cboAnno.SelectedValue + "  AND SRMeseFatt = " + cboMese.SelectedValue + ")";
                sql += "     OR ";
                sql += "     (SRAnnoFatt is null and (DATEPART(yyyy,SRdatainizio) = " + cboAnno.SelectedValue + " AND DATEPART(mm,SRdatainizio) = " + cboMese.SelectedValue + ")) ";
                sql += " ) ";
                sql += " AND SRdatainizio >= CONVERT(datetime,'" + txtDataInizio.Text + "',105) ";
                sql += " AND SRdatainizio <= CONVERT(datetime,'" + txtDataFine.Text + "',105)  ";
            }
            sql += " ORDER BY fornitori.FORragsoc ";

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            grdServizi.DataSource = DTResult;
            grdServizi.Columns[0].HeaderText = "Ser.: " + DTResult.Rows.Count;
            grdServizi.DataBind();

            grdFornitori.Visible = false;
            RWANNULLASCHEDA.Visible = true;
                
            grdServizi.Visible = true;

            lblTotDiritti.Text = "";
            lblTotImpo.Text = "";
            lblTotLordo.Text = "";
            lblTotReda.Text = "";
            lblTotSpese.Text = "";
            lblTotTOTALE.Text = "";

            double TOTDIRITTI = 0;
            double TOTIMPO = 0;
            double TOTLORDO = 0;
            double TOTREDA = 0;

            double TOTSPESE = 0;
        
            for (int i = 0; i <= grdServizi.Items.Count - 1; i++)
            {
                
                try{TOTDIRITTI += double.Parse(DTResult.Rows[i]["srdiritti"].ToString());}catch{}
                try{TOTIMPO += double.Parse(DTResult.Rows[i]["SRImponibile"].ToString());}catch{}
                try{TOTLORDO += double.Parse(DTResult.Rows[i]["SRimportoLordo"].ToString());}catch{}
                try { TOTREDA += double.Parse(DTResult.Rows[i]["SRimportoRedazionale"].ToString()); }
                catch { }
                try { TOTSPESE += double.Parse(DTResult.Rows[i]["SRspese"].ToString()) + double.Parse(DTResult.Rows[i]["SRspese1"].ToString()) + double.Parse(DTResult.Rows[i]["SRspese2"].ToString()); }
                catch { }

                ((LinkButton)grdServizi.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare il Servizio selezionato ?')){}else{return false}");
                ((LinkButton)grdServizi.Items[i].Cells[COL_ELIMINA].Controls[0]).Visible = false;

                switch (DTResult.Rows[i]["COLORE"].ToString())
                {
                    case "Pagata":
                    case "Fatturata":
                        {
                            ((LinkButton)grdServizi.Items[i].Cells[COL_MODIFICA].Controls[0]).Visible = false;
                            ((LinkButton)grdServizi.Items[i].Cells[COL_ELIMINA].Controls[0]).Visible = false;
                            break;
                        }
                }

            }

            lblTotDiritti.Text = String.Format("{0:N2}", TOTDIRITTI);
            lblTotImpo.Text = String.Format("{0:N2}", TOTIMPO + TOTREDA);
            lblTotLordo.Text = String.Format("{0:N2}", TOTLORDO);
            lblTotReda.Text = String.Format("{0:N2}", TOTREDA);

            lblTotSpese.Text = String.Format("{0:N2}", TOTSPESE);
            lblTotTOTALE.Text = String.Format("{0:N2}", TOTIMPO + TOTREDA + TOTSPESE);  
        }

        protected void cboSerFornitore_SelectedIndexChanged(object sender, EventArgs e)
        {
            CaricaServiziCollegati("");
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

        protected void lnkFilterESEGUITO_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "ESEGUITO";

            lblFiltroSel.Text = "Filtro selezionato : ESEGUITO";
            lnkFilterRemove.Visible = true;

            CaricaServiziCollegati("");
        }

        protected void lnkFilterDAFATTURARE_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "DA FATTURARE";

            lblFiltroSel.Text = "Filtro selezionato : DA FATTURARE";
            lnkFilterRemove.Visible = true;

            CaricaServiziCollegati("");
        }
        protected void lnkFilterFATTURATA_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "FATTURATA";
            lblFiltroSel.Text = "Filtro selezionato : FATTURA DA RICEVERE";
            lnkFilterRemove.Visible = true;
            CaricaServiziCollegati("");
        }
        protected void lnkFilterPAGATA_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "PAGATA";
            lblFiltroSel.Text = "Filtro selezionato : PAGATA";
            lnkFilterRemove.Visible = true;
            CaricaServiziCollegati("");

        }
        protected void lnkFilterESCLUDIPAGATI_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "ESCLUDIPAGATI";
            lblFiltroSel.Text = "Filtro selezionato : ESCLUDI PAGATI";
            lnkFilterRemove.Visible = true;
            CaricaServiziCollegati("");

        }
        protected void lnkFilterRemove_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "";
            lblFiltroSel.Text = "";
            lnkFilterRemove.Visible = false;
            CaricaServiziCollegati("");
        }

        #endregion

        protected void lnkViewServizi_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Scheda Fornitore - VISUALIZZAZIONE DEI SERVIZI";
            ViewState["SCHEDA"] = "NO";
            RWTotaliServizi.Visible = true;
            CaricaServiziCollegati("");
        }
        protected void lnkViewScheda_Click(object sender, EventArgs e)
        {
            lblTitle.Text = "Scheda Fornitore - VISUALIZZAZIONE DELLE SCHEDE DEI FORNITORI";
            ViewState["SCHEDA"] = "SI";
            RWTotaliServizi.Visible = false;
            CaricaSchede("");
        }
        
        protected void grdFornitori_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "SCHEDA":
                    {
                        clsParameter pParameter = new clsParameter();

                        string DCID = grdFornitori.DataKeys[e.Item.ItemIndex].ToString();
                        
                        string URL = "PrintScheda.aspx?FORCODICE=" + DCID + "&ANNO=" + cboAnno.SelectedValue + "&MESE=" + cboMese.SelectedValue+ "";
                        ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("PRINTReport")).Attributes.Add("src", URL);
                        

                        break;
                    }
                case "DETTAGLI":
                    {
                        string DCID = grdFornitori.DataKeys[e.Item.ItemIndex].ToString();
                        
                        lblTitle.Text = "Scheda Fornitore - VISUALIZZAZIONE DEI SERVIZI";
                        ViewState["SCHEDA"] = "NO";
                        ViewState["DCIDSELFILTRO"] = DCID;
                        RWTotaliServizi.Visible = true;
                        CaricaServiziCollegati("",DCID);

                        break;
                    }
            }
        }
        protected void lnkFiltroMeseAnno_Click(object sender, EventArgs e)
        {
            CaricaServiziCollegati("");
        }
        protected void lnkFiltroDate_Click(object sender, EventArgs e)
        {
            CaricaServiziCollegati("DATE");
        }

        protected void lnkANNULLASCHEDA_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grdServizi.Items.Count; i++)
            {
                string SERSEL = grdServizi.DataKeys[i].ToString();

                sql = " UPDATE Servizi SET SRfatturare=1,SRFatturato=0,SRPagato=0 WHERE SRCodice = " + SERSEL + " ";
                clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

            }

            CaricaServiziCollegati("", ViewState["DCIDSELFILTRO"].ToString());
        }
    }
}