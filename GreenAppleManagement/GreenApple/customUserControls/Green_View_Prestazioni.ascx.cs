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
 *  Inclusione delle spese nel totale della prestazione
 */
namespace Green.Apple.Management
{
    public partial class Green_View_Prestazioni : Green_BaseUserControl
    {

        #region DECLARATIONS

        private string sql = "";
        clsPagining objPagining = new clsPagining();
        protected int COL_ELIMINA = 0;
        protected int COL_MODIFICA = 0;

        private double SER_TOTIMPO = 0;
        private double SER_TOTAGG = 0;
        private double SER_TOTLORDO = 0;
        private double SER_TOTDIRITTI = 0;
        private double SER_TOTSPESE = 0;

        private double CALCOLO_IMPONIBILE = 0;
        private double CALCOLO_PERCDIRITTI = 0;
        private double CALCOLO_DIRITTI = 0;
        private double CALCOLO_PERCIVA = 0;
        private double CALCOLO_IMPORTOIVA = 0;
        private double CALCOLO_TOTALE = 0;
        private double CALCOLO_ANTICIPI = 0;
        private double CALCOLO_TRATTENUTE = 0;
        private double CALCOLO_SPESE = 0;

        private double CALCOLO_SPESE_0 = 0;
        private double CALCOLO_SPESE_1 = 0;
        private double CALCOLO_SPESE_2 = 0;

        private double CALCOLO_IVASPESE = 0;
        private double CALCOLO_RIVALSA = 0;
        private double CALCOLO_GIORNI = 0;
        private double CALCOLO_OVERTIME = 0;

        #endregion

        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            clsFunctions.AssegnaEventoCalendar(imgtxtFiltroFine, txtFiltroFine, false);
            clsFunctions.AssegnaEventoCalendar(imgtxtFiltroInizio, txtFiltroInizio, false);


            string pScript = "<script>"
                + "BlurImporto();"
                + "</script>";
            this.Page.ClientScript.RegisterStartupScript(typeof(string), "CalcoloImporto", pScript);

            COL_ELIMINA = 2;
            COL_MODIFICA = 1;

            gridPrestazioni.DataKeyField = "PRNumero";
            grdServizi.DataKeyField = "SRcodice";

            objPagining.CaricaPagining(phPagine, gridPrestazioni);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            //lblError.Text = "";
            //lblError.Visible = false;
            if (!IsPostBack)
            {
                ViewState["MODIFICASTATO"] = "1";
                ViewState["PRfatturato"] = "";
                ViewState["PRfatturare"] = "";
                ViewState["MOD_SERVIZIO"] = "";
                ViewState["MOD_PREST"] = "";
                CaricaComboAnnoMese();
                txtFiltroInizio.Text = DateTime.Now.AddMonths(-3).ToShortDateString(); // ho impostato di base gli ultimi 3 mesi con .AddMonths(-3)
                txtFiltroFine.Text = DateTime.Now.ToShortDateString();

                lblFiltroSel.Text = "";
                lnkFilterRemove.Visible = false;
                ViewState["FILTER_STATUS"] = "";
                ViewState["FATTURAESISTE"] = "";
                ModificaServizioVisible = false;
                ModificaVisible = false;
                gridPrestazioni.Visible = true;
                phPagine.Visible = true;
                grdServizi.Visible = false;

                if (Request.QueryString["FilterNumero"] != null)
                {
                    txtFiltroLibero.Text = Request.QueryString["FilterNumero"].ToString();
                }

                CaricaAnagrafiche();
                CaricaDati();



                if (Request.QueryString["PRNumero"] != null)
                {
                    //VADO IN MODO VIEW, ARRIVO DA FATTURA QUINDI SICURO
                    ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Dettaglio;

                    string PRESTSEL = Request.QueryString["PRNumero"].ToString();
                    ViewState["PRESTSEL"] = PRESTSEL;

                    ModificaPrestazione();

                    ModificaVisible = true;
                    grdServizi.Visible = true;
                    gridPrestazioni.Visible = false;
                    phPagine.Visible = false;

                    lnkSerInserisci.Visible = false;
                    lnkPrestAggiorna.Visible = false;
                }
            }
        }

        protected void objPagining_PageChange(object sender, clsPagining.CustomPaginingArgs e)
        {
            gridPrestazioni.SelectedIndex = -1;
            gridPrestazioni.EditItemIndex = -1;
            gridPrestazioni.CurrentPageIndex = e.NewPage;
            CaricaDati();
        }

        protected void lnkInserisci_Click(object sender, EventArgs e)
        {
            ViewState["MOD_PREST"] = "INS";
            ViewState["PRESTSEL"] = "";
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            gridPrestazioni.SelectedIndex = -1;
            ModificaVisible = true;
            gridPrestazioni.Visible = false;
            phPagine.Visible = false;

            lnkSerInserisci.Visible = false;

            ViewState["INSERIMENTO_PRESTAZIONE"] = "SI";

            chkDAFATTURARE.Checked = false;
            lnkPrestFATTURACLIENTE.Visible = false;
            txtFiltro.Text = "";
            //lnkCercaCLIENTE_Click(new object(), new EventArgs());
            CaricaCampiCliente(true);
            cboCliente.SelectedIndex = 0;
            CambioCliente();

            txtDataInizio.Text = DateTime.Today.ToShortDateString();
            txtDataFine.Text = DateTime.Today.ToShortDateString();
            cboCategoria.SelectedIndex = 0;
            cboValuta.SelectedIndex = 0;
            cboLingua.SelectedIndex = 0;

            txtDescrizione.Text = "";
            txtParolaChiave.Text = "";

            grdServizi.DataSource = null;
            grdServizi.DataBind();


            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            this.Page.Validate();

            CaricaPercentuali();
        }

        protected void lnkAnnulla_Click(object sender, EventArgs e)
        {
            gridPrestazioni.SelectedIndex = -1;
            ModificaVisible = false;
            gridPrestazioni.Visible = true;
            phPagine.Visible = true;
        }

        protected void lnkCerca_Click(object sender, EventArgs e)
        {
            CaricaDati();
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
                TBLModifica.Visible = value; gridPrestazioni.Enabled = !value; phPagine.Visible = !value; lnkInserisci.Enabled = !value;
                for (int i = 0; i <= gridPrestazioni.Items.Count - 1; i++)
                {
                    ((LinkButton)gridPrestazioni.Items[i].Cells[COL_ELIMINA].Controls[0]).Visible = !value;
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
                gridPrestazioni.Visible = !value;
                phPagine.Visible = !value;
                gridPrestazioni.ToolTip = pstrToolTip;
                lnkInserisci.ToolTip = pstrToolTip;
            }
        }

        private void VisibilePRESTAZIONE(bool Vis)
        {
            RWPrest1.Visible = Vis;
            RWPrest2.Visible = Vis;
            RWPrest3.Visible = Vis;
            RWPrest4.Visible = Vis;
            RWPrest5.Visible = Vis;
            RWPrest6.Visible = Vis;
            RWPrest7.Visible = Vis;
            RWPrest8.Visible = Vis;
            RWPrest9.Visible = Vis;
            RWPrest10.Visible = Vis;
            RWPrest11.Visible = Vis;
            RWPrest12.Visible = Vis;
        }

        private bool ModificaServizioVisible
        {
            get
            {
                //VisibilePRESTAZIONE(!RWServizioMOD.Visible);
                return RWServizioMOD.Visible;
            }
            set
            {

                RWServizioMOD.Visible = value; grdServizi.Enabled = !value; lnkInserisci.Enabled = !value;
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
                    //((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Dettaglio;
                }
                grdServizi.Visible = !value;
                grdServizi.ToolTip = pstrToolTip;
                lnkInserisci.ToolTip = pstrToolTip;
                VisibilePRESTAZIONE(!RWServizioMOD.Visible);
            }
        }
        #endregion

        private void CaricaCampiCliente(bool onlyInUso)
        {
            //Carico clienti in Combo
            if (onlyInUso)
                sql = @"SELECT * FROM CLIENTI WHERE (cliragsoc like '%" + txtFiltro.Text + "%' or CLIPRENOME like '%" + txtFiltro.Text + "%') AND CliInUso = 1 ";
            else
                sql = @"SELECT * FROM CLIENTI WHERE (cliragsoc like '%" + txtFiltro.Text + "%' or CLIPRENOME like '%" + txtFiltro.Text + "%') ";

            sql += " ORDER BY cliragsoc ";
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
                cboCliente.Items.Add(myItem);
            }

            cboCliente.SelectedIndex = 0;

            //Pulizia dei campi per riselezione
            lblPostaCAP.Text = "";
            lblPostaINDIRIZZO.Text = "";
            lblPostaLOCALITA.Text = "";
            lblPostaNAZIONE.Text = "";
            lblPostaPROVINCIA.Text = "";
            lblPostaRAGSOC.Text = "";

            txtCliBanca.Text = "";
            txtCliPiazza.Text = "";
        }

        protected void lnkCercaCLIENTE_Click(object sender, EventArgs e)
        {
            //Carico clienti in Combo
            CaricaCampiCliente(true);
        }

        private void CaricaAnagrafiche()
        {
            clsFunctions.AssegnaEventoCalendar(imgDataInizio, txtDataInizio, false);
            clsFunctions.AssegnaEventoCalendar(imgDataFine, txtDataFine, false);

            clsFunctions.AssegnaEventoCalendar(imgSerInizio, txtSerInizio, false);
            clsFunctions.AssegnaEventoCalendar(imgSerFine, txtSerFine, false);

            txtParolaChiave.Text = "";

            //Carico Categorie
            sql = @"select * from Lista_CategoriaServizi ORDER BY POSIZIONE ";
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cboCategoria.Items.Clear();
            ListItem myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Selezionare una categoria --";
            cboCategoria.Items.Add(myItem);

            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["CSCodice"];
                myItem.Text = "" + DTResult.Rows[i]["CSCodice"] + " - " + DTResult.Rows[i]["CSDescrizione"];
                cboCategoria.Items.Add(myItem);
            }

            cboCategoria.SelectedIndex = 0;

            //Carico Lingue
            sql = @"select * from Lista_Lingua ORDER BY POSIZIONE ";
            DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cboLingua.Items.Clear();
            myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Selezionare una lingua --";
            cboLingua.Items.Add(myItem);

            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["CodLingua"];
                myItem.Text = "" + DTResult.Rows[i]["Codice"] + " - " + DTResult.Rows[i]["Descrizione"];
                cboLingua.Items.Add(myItem);
            }

            cboLingua.SelectedIndex = 0;

            //Carico Valuta
            sql = @"select * from Lista_Valuta ORDER BY POSIZIONE ";
            DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cboValuta.Items.Clear();
            myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Selezionare una lingua --";
            cboValuta.Items.Add(myItem);

            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["Codice"];
                myItem.Text = "" + DTResult.Rows[i]["Codice"] + " - " + DTResult.Rows[i]["Descrizione"];
                cboValuta.Items.Add(myItem);
            }

            cboValuta.SelectedIndex = 0;


            //Carico Categorie Servizi
            //sql = @"select * from Lista_CategoriaServizi ORDER BY POSIZIONE ";
            //DTResult = new DataTable();
            //clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            //cboSerCategoria.Items.Clear();
            //myItem = new ListItem();
            //myItem.Value = "0";
            //myItem.Text = "-- Categoria --";
            //cboSerCategoria.Items.Add(myItem);

            //for (int i = 0; i < DTResult.Rows.Count; i++)
            //{
            //    myItem = new ListItem();
            //    myItem.Value = "" + DTResult.Rows[i]["CSCodice"];
            //    myItem.Text = "" + DTResult.Rows[i]["CSCodice"] + " - " + DTResult.Rows[i]["CSdescrizione"];
            //    cboSerCategoria.Items.Add(myItem);
            //}

            //cboSerCategoria.SelectedIndex = 0;

            //Carico Tipo Servizi
            sql = @"select * from Lista_Tiposervizio ORDER BY CSdescrizione ";
            DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cboSerTipoServizio.Items.Clear();
            myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Tipo del Servizio --";
            cboSerTipoServizio.Items.Add(myItem);

            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["CSCodice"];
                //myItem.Text = "" + DTResult.Rows[i]["CSCodice"] + " - " + DTResult.Rows[i]["CSdescrizione"];
                myItem.Text = "" + DTResult.Rows[i]["CSdescrizione"];

                cboSerTipoServizio.Items.Add(myItem);
            }

            cboSerTipoServizio.SelectedIndex = 0;

            //Carico Fornitori
            sql = @"select * from Fornitori order by Forragsoc ";
            DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cboSerFornitore.Items.Clear();
            myItem = new ListItem();
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
        protected void cboCliente_SelectedIndexChanged(object sender, EventArgs e)
        {
            CambioCliente();
            CaricaPercentuali();
        }

        protected void cboSerFornitore_SelectedIndexChanged(object sender, EventArgs e)
        {
            CaricaPercentuali();
        }

        protected void cboSerCategoria_SelectedIndexChanged(object sender, EventArgs e)
        {
            CaricaPercentuali();
        }

        private void CambioCliente()
        {
            //Carico dati del cliente
            string CLICODICE = cboCliente.SelectedValue;

            //Carico clienti in Combo
            sql = @"select cliperciva, clipercdiritti
                ,ISNULL(Cliragsoc,Clipostaragsoc) as Clipostaragsoc
                ,ISNULL(Cliindirizzo,ClipostaIndirizzo) as ClipostaIndirizzo
                ,ISNULL(Clilocalita,Clipostalocalita) as Clipostalocalita
                ,ISNULL(Cliprovincia,Clipostaprov) as Clipostaprov
                ,ISNULL(CliCAP,ClipostaCAP) as ClipostaCAP
                ,ISNULL(CliNazione,ClipostaNazione) as ClipostaNazione
                ,ISNULL(CliBanca,'') as CliBanca
                ,ISNULL(CliPiazza,'') as CliPiazza
                ,ISNULL(CliLingua,'') as CliLingua
                ,ISNULL(CliValuta,'') as CliValuta
                ,GACalcoloIVA  
                FROM CLIENTI WHERE clicodice = " + CLICODICE + " ";

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);


            if (DTResult.Rows.Count > 0)
            {
                lblPostaCAP.Text = "" + DTResult.Rows[0]["ClipostaCAP"];
                lblPostaINDIRIZZO.Text = "" + DTResult.Rows[0]["ClipostaIndirizzo"];
                lblPostaLOCALITA.Text = "" + DTResult.Rows[0]["Clipostalocalita"];
                lblPostaNAZIONE.Text = "" + DTResult.Rows[0]["ClipostaNazione"];
                lblPostaPROVINCIA.Text = "" + DTResult.Rows[0]["Clipostaprov"];
                lblPostaRAGSOC.Text = "" + DTResult.Rows[0]["Clipostaragsoc"];
                txtCliBanca.Text = "" + DTResult.Rows[0]["CliBanca"];
                txtCliPiazza.Text = "" + DTResult.Rows[0]["CliPiazza"];

                ViewState["CALCOLOIVA"] = Convert.ToBoolean(DTResult.Rows[0]["GACalcoloIVA"]);

                if ("" + DTResult.Rows[0]["CliLingua"] != "")
                {
                    //Pre carico la Lingua
                    cboLingua.SelectedValue = "" + DTResult.Rows[0]["CliLingua"];
                }
                else
                {
                    cboLingua.SelectedIndex = 0;
                }
                if ("" + DTResult.Rows[0]["CliValuta"] != "")
                {
                    //Pre carico la Valuta
                    cboValuta.SelectedValue = "" + DTResult.Rows[0]["CliValuta"];
                }
                else
                {
                    cboValuta.SelectedIndex = 0;
                }

            }
            else
            {
                ViewState["CALCOLOIVA"] = true;
                cboLingua.SelectedIndex = 0;
                cboValuta.SelectedIndex = 0;
                lblPostaCAP.Text = "";
                lblPostaINDIRIZZO.Text = "";
                lblPostaLOCALITA.Text = "";
                lblPostaNAZIONE.Text = "";
                lblPostaPROVINCIA.Text = "";
                lblPostaRAGSOC.Text = "";
                txtCliBanca.Text = "";
                txtCliPiazza.Text = "";
            }
        }

        protected void gridPrestazioni_ItemCommand(object source, DataGridCommandEventArgs e)
        {

            switch (e.CommandName.ToUpper())
            {
                case "VIEW":
                    {
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Dettaglio;
                        gridPrestazioni.SelectedIndex = e.Item.ItemIndex;

                        string PRESTSEL = gridPrestazioni.DataKeys[e.Item.ItemIndex].ToString();
                        ViewState["PRESTSEL"] = PRESTSEL;

                        ModificaPrestazione();

                        ModificaVisible = true;
                        grdServizi.Visible = true;
                        gridPrestazioni.Visible = false;
                        phPagine.Visible = false;

                        lnkSerInserisci.Visible = false;
                        lnkPrestAggiorna.Visible = false;
                        break;
                    }
                case "MODIFICA":
                    {
                        ViewState["MOD_PREST"] = "UPD";
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        gridPrestazioni.SelectedIndex = e.Item.ItemIndex;

                        string PRESTSEL = gridPrestazioni.DataKeys[e.Item.ItemIndex].ToString();
                        ViewState["PRESTSEL"] = PRESTSEL;

                        ModificaPrestazione();
                        break;
                    }
                case "ELIMINA":
                    {
                        ViewState["MOD_PREST"] = "";
                        gridPrestazioni.SelectedIndex = e.Item.ItemIndex;

                        string PRESTSEL = gridPrestazioni.DataKeys[e.Item.ItemIndex].ToString();

                        sql = "DELETE FROM Servizi WHERE SRnumeroprestazione = " + PRESTSEL;
                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        sql = "DELETE FROM Prestazioni WHERE PRNumero = " + PRESTSEL;
                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        CaricaDati();

                        break;
                    }
            }
        }

        private void ModificaPrestazione()
        {
            string PRESTSEL = ViewState["PRESTSEL"].ToString();

            sql = "select * from Prestazioni inner join clienti on Prestazioni.PRcliente = clienti.clicodice WHERE PRNumero = " + PRESTSEL;
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            if (DTResult.Rows.Count > 0)
            {
                //lnkCercaCLIENTE_Click(new object(), new EventArgs());
                CaricaCampiCliente(false);

                DataRow DTRow;
                DTRow = DTResult.Rows[0];

                if (bool.Parse(DTRow["PRfatturato"].ToString()) || bool.Parse(DTRow["PRpagata"].ToString()))
                {
                    ViewState["PRfatturato"] = DTRow["PRfatturato"].ToString();
                    ViewState["PRfatturare"] = DTRow["PRfatturare"].ToString();
                    ViewState["MODIFICASTATO"] = "0";
                    chkDAFATTURARE.Checked = true;
                    lnkPrestFATTURACLIENTE.Visible = chkDAFATTURARE.Checked;
                }
                else
                {
                    ViewState["MODIFICASTATO"] = "1";
                    if (DTRow["PRfatturare"] != null)
                    {
                        chkDAFATTURARE.Checked = bool.Parse(DTRow["PRfatturare"].ToString());
                        lnkPrestFATTURACLIENTE.Visible = chkDAFATTURARE.Checked;
                    }
                    else
                    {
                        chkDAFATTURARE.Checked = false;
                        lnkPrestFATTURACLIENTE.Visible = chkDAFATTURARE.Checked;
                    }
                }

                cboCliente.SelectedValue = DTRow["clicodice"].ToString();
                lblPostaRAGSOC.Text = DTRow["cliRagSoc"].ToString();
                lblPostaINDIRIZZO.Text = DTRow["cliIndirizzo"].ToString();
                lblPostaLOCALITA.Text = DTRow["cliLocalita"].ToString();
                lblPostaPROVINCIA.Text = DTRow["cliProvincia"].ToString();
                lblPostaCAP.Text = DTRow["cliCAP"].ToString();
                lblPostaNAZIONE.Text = DTRow["cliNazione"].ToString();
                txtCliBanca.Text = DTRow["cliBanca"].ToString();
                txtCliPiazza.Text = DTRow["cliPiazza"].ToString();

                ViewState["FATTURAESISTE"] = "" + DTRow["PRFattura"];
                txtParolaChiave.Text = DTRow["PRchiave"].ToString();

                txtDataInizio.Text = Convert.ToDateTime(DTRow["prdatainizio"].ToString()).ToShortDateString();
                txtDataFine.Text = Convert.ToDateTime(DTRow["prdatafine"].ToString()).ToShortDateString();

                txtDescrizione.Text = DTRow["prnote"].ToString();

                for (int i = 0; i < cboCategoria.Items.Count; i++)
                {
                    if (cboCategoria.Items[i].Value == DTRow["PRcategoria"].ToString())
                    {
                        cboCategoria.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < cboLingua.Items.Count; i++)
                {
                    if (cboLingua.Items[i].Value == DTRow["PRlingua"].ToString())
                    {
                        cboLingua.SelectedIndex = i;
                        break;
                    }
                }

                for (int i = 0; i < cboValuta.Items.Count; i++)
                {
                    if (cboValuta.Items[i].Value == DTRow["PRvaluta"].ToString())
                    {
                        cboValuta.SelectedIndex = i;
                        break;
                    }
                }
            }

            CaricaServiziCollegati(PRESTSEL);

            ModificaVisible = true;
            grdServizi.Visible = true;
            gridPrestazioni.Visible = false;
            phPagine.Visible = false;

        }

        protected void grdServizi_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "MODIFICA":
                    {
                        ViewState["MOD_SERVIZIO"] = "UPD";
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

                            txtSerAnticipi.Text = "" + DTRow["SRanticipi"];
                            txtSerAnticipiGA.Text = "" + DTRow["SRanticipige"];
                            txtSerDiritti.Text = "" + DTRow["SRdiritti"];
                            txtSerFine.Text = "" + DTRow["SRdatafine"];
                            txtSerImponibile.Text = "" + DTRow["SRimponibile"];
                            txtSerImporto.Text = "" + DTRow["SRimporto"];
                            txtSerImportoGA.Text = "" + DTRow["SRimportoGA"];
                            txtSerImportoLordo.Text = "" + DTRow["SRimportoLordo"];
                            txtSerImportoRed.Text = "" + DTRow["SRimportoRedazionale"];
                            txtSerImpOver.Text = "" + DTRow["SRimportoOvertime"];
                            txtSerInizio.Text = "" + DTRow["SRdatainizio"];
                            txtSerNumGiorni.Text = "" + DTRow["SRnumgiorni"];
                            txtSerOverGA.Text = "" + DTRow["SRimportoOvertimeGA"];
                            txtSerSpesa1.Text = "" + DTRow["SRspese"];
                            txtSerSpesa1Descr.Text = "" + DTRow["SRdescSpese"];
                            txtSerSpesa1GA.Text = "" + DTRow["SRspeseGA"];
                            txtSerSpesa1GADescr.Text = "" + DTRow["SRdescSpeseGA"];
                            txtSerSpesa2.Text = "" + DTRow["SRspese1"];
                            txtSerSpesa2Descr.Text = "" + DTRow["SRdescSpese1"];
                            txtSerSpesa2GA.Text = "" + DTRow["SRspeseGA1"];
                            txtSerSpesa2GADescr.Text = "" + DTRow["SRdescSpeseGA1"];
                            txtSerSpesa3.Text = "" + DTRow["SRspese2"];
                            txtSerSpesa3Descr.Text = "" + DTRow["SRdescSpese2"];
                            txtSerSpesa3GA.Text = "" + DTRow["SRspeseGA2"];
                            txtSerSpesa3GADescr.Text = "" + DTRow["SRdescSpeseGA2"];
                            txtSerTrattenute.Text = "" + DTRow["SRtrattenute"];

                            chkEsenzioneIVA.Checked = Convert.ToBoolean(DTRow["SResenzioneIvaArt15"]);

                            //for (int i = 0; i < cboSerCategoria.Items.Count; i++)
                            //{
                            //    if (cboSerCategoria.Items[i].Value == DTRow["SRcategoria"].ToString())
                            //    {
                            //        cboSerCategoria.SelectedIndex = i;
                            //        break;
                            //    }
                            //}

                            for (int i = 0; i < cboSerTipoServizio.Items.Count; i++)
                            {
                                if (cboSerTipoServizio.Items[i].Value == DTRow["SRtiposervizio"].ToString())
                                {
                                    cboSerTipoServizio.SelectedIndex = i;
                                    break;
                                }
                            }

                            //COMPETENZA
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

                            txtSerFornitoreSearch.Text = "";
                            CaricaFornitoreCBO();
                            for (int i = 0; i < cboSerFornitore.Items.Count; i++)
                            {
                                if (cboSerFornitore.Items[i].Value == DTRow["SRoperatore"].ToString())
                                {
                                    cboSerFornitore.SelectedIndex = i;
                                    break;
                                }
                            }

                        }

                        CaricaPercentuali();

                        ModificaServizioVisible = true;
                        grdServizi.Visible = false;

                        break;
                    }
                case "ELIMINA":
                    {
                        ViewState["MOD_SERVIZIO"] = "";
                        grdServizi.SelectedIndex = e.Item.ItemIndex;

                        string SERSEL = grdServizi.DataKeys[e.Item.ItemIndex].ToString();

                        sql = "DELETE FROM Servizi WHERE srcodice = " + SERSEL;
                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        CaricaServiziCollegati(ViewState["PRESTSEL"].ToString());

                        break;
                    }
            }
        }

        private void CaricaPercentuali()
        {
            //CARICO ELEMENTI PERCENTUALI PER CALCOLI
            sql = "select * from Clienti WHERE Clicodice = " + cboCliente.SelectedValue;
            DataTable DTCliente = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTCliente, true);
            if (DTCliente.Rows.Count > 0)
            {
                lblSerPercIVA.Text = "" + DTCliente.Rows[0]["ClipercIVA"];
            }
            else
            {
                lblSerPercIVA.Text = "0";
            }

            sql = "select * from Fornitori WHERE Forcodice = " + cboSerFornitore.SelectedValue;
            DataTable DTFornitore = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTFornitore, true);
            if (DTFornitore.Rows.Count > 0)
            {
                //NEL CAMPO PERC COMPENSO
                //Forperc1  Pubblicità
                //Forperc2  Redazionale
                //Forperc3  Catalogo
                //Forperc4  Sfilata
                //Forperc5  Pubbliredazionale
                //Forperc6  Crediti

                try
                {
                    lblSerPercDiritti.Text = "" + DTFornitore.Rows[0]["Forperc" + cboCategoria.SelectedValue];
                    HDlblSerPercDiritti.Value = "" + DTFornitore.Rows[0]["Forperc" + cboCategoria.SelectedValue];
                }
                catch { }
                lblSerPercRitAcc.Text = "" + DTFornitore.Rows[0]["Forpercritenuta"];
                lblSerPercRicPrev.Text = "" + DTFornitore.Rows[0]["ForPctRivPrev"];
            }
            else
            {
                lblSerPercDiritti.Text = "0";
                HDlblSerPercDiritti.Value = "0";
                lblSerPercRicPrev.Text = "0";
                lblSerPercRitAcc.Text = "0";
            }

            sql = "select * from Lista_CategoriaServizi WHERE CSCodice = " + cboCategoria.SelectedValue;
            DataTable DTCategoria = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTCategoria, true);
            if (DTCategoria.Rows.Count > 0)
            {
                lblSerPercCompenso.Text = "" + DTCategoria.Rows[0]["PercCompenso"];
                HDlblSerPercCompenso.Value = "" + DTCategoria.Rows[0]["PercCompenso"];
            }
            else
            {
                lblSerPercCompenso.Text = "0";
                HDlblSerPercCompenso.Value = "0";
            }
        }

        #region FUNCTIONS
        private void CaricaDati()
        {
            CaricaDati("");
        }

        private void CaricaDati(string TIPO)
        {
            //***************************************************************************************

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
            sql += " WHERE 1=1 ";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " AND (Clienti.cliragsoc like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR PRnumero like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR PRchiave like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR PRnote like '%" + txtFiltroLibero.Text.Trim() + "%' )";
            }
            else
            {
                //TEMPORANEO
                //sql += " WHERE PRnumero = 11807 ";
            }

            //if (ViewState["FILTER_STATUS"].ToString() != "")
            //{ 
            //    sql += " AND CASE ";
            //    sql += " WHEN ";
            //    sql += " PRfatturare = 0 and PRfatturato = 0 and PRpagata = 0 ";
            //    sql += " THEN 'EMESSA' ";
            //    sql += " WHEN ";
            //    sql += " PRfatturare = 1 and PRfatturato = 0 and PRpagata = 0 ";
            //    sql += " THEN 'DA FATTURARE' ";
            //    sql += " WHEN ";
            //    sql += " PRfatturato = 1  and PRpagata = 0 ";
            //    sql += " THEN 'FATTURATA' ";
            //    sql += " WHEN ";
            //    sql += " PRpagata = 1 and PRfatturato = 1 ";
            //    sql += " THEN 'PAGATA' ";
            //    sql += " END  = '" + ViewState["FILTER_STATUS"].ToString() + "' ";
            //}
            if (ViewState["FILTER_STATUS"].ToString() != "" && ViewState["FILTER_STATUS"].ToString() != "ESCLUDIPAGATI")
            {
                sql += " AND CASE ";
                sql += " WHEN ";
                sql += " PRfatturare = 0 and PRfatturato = 0 and PRpagata = 0 ";
                sql += " THEN 'EMESSA' ";
                sql += " WHEN ";
                sql += " PRfatturare = 1 and PRfatturato = 0 and PRpagata = 0 ";
                sql += " THEN 'DA FATTURARE' ";
                sql += " WHEN ";
                sql += " PRfatturato = 1  and PRpagata = 0 ";
                sql += " THEN 'FATTURATA' ";
                sql += " WHEN ";
                sql += " PRpagata = 1 and PRfatturato = 1 ";
                sql += " THEN 'PAGATA' ";
                sql += " END  = '" + ViewState["FILTER_STATUS"].ToString() + "' ";
            }
            else if (ViewState["FILTER_STATUS"].ToString() == "ESCLUDIPAGATI")
            {
                sql += " AND CASE ";
                sql += " WHEN ";
                sql += " PRpagata = 0 ";
                sql += " THEN 'ESCLUDIPAGATI' ";
                sql += " END  = '" + ViewState["FILTER_STATUS"].ToString() + "' ";
            }

            if (TIPO == "MESEANNO" && cboMese.SelectedIndex > 0)
            {
                sql += " AND DATEPART(mm,PRdatainizio) = " + cboMese.SelectedValue + " AND DATEPART(yyyy,PRdatainizio) = " + cboAnno.SelectedValue + " ";
            }
            if (TIPO == "MESEANNO" && cboMese.SelectedIndex == 0)
            {
                sql += " AND DATEPART(yyyy,PRdatainizio) = " + cboAnno.SelectedValue + " ";
            }

            if (TIPO == "DATE" || (TIPO == "" && txtFiltroLibero.Text.Trim() == ""))
            {
                sql += " AND PRdatainizio >= CONVERT(datetime,'" + txtFiltroInizio.Text + "',105) ";
                sql += " AND PRdatainizio <= CONVERT(datetime,'" + txtFiltroFine.Text + "',105)  ";
            }

            sql += " ORDER BY PRnumero DESC ";

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);
            try
            {
                if (gridPrestazioni.CurrentPageIndex >= ((double)DTResult.Rows.Count / (double)gridPrestazioni.PageSize))
                {
                    gridPrestazioni.CurrentPageIndex -= 1;
                }
            }
            catch { }

            /*PH(1) */

            double TOTALEPRESTAZIONI = 0;
            foreach (DataRow row in DTResult.Rows)
            {
                /* Aggiunta spese al totale prestazione - START */
                double tmpTotale = 0;
                // modificato per gestire valori null di DB - START
                tmpTotale = double.Parse((row["PRImponibile"] is DBNull) ? "0" : row["PRImponibile"].ToString());
                tmpTotale += double.Parse((row["PRspese"] is DBNull) ? "0" : row["PRspese"].ToString());
                tmpTotale += double.Parse((row["PRspese1"] is DBNull) ? "0" : row["PRspese1"].ToString());
                tmpTotale += double.Parse((row["PRspese2"] is DBNull) ? "0" : row["PRspese2"].ToString());
                // modificato per gestire valori null di DB - END
                row["PRtotale"] = (object)tmpTotale;
                /* Aggiunta spese al totale prestazione - END */

                //brutto il try-catch solo per evitare errore sul double.Parse in caso di stringa vuota!
                // fa perdere un sacco di tempo macchina!
                /* utilizzo PRtotale anzichè PRImponibile */
                //if (row["PRImponibile"].ToString() != "")
                if (row["PRtotale"].ToString() != "")
                {
                    //try
                    //{
                    /* utilizzo PRtotale anzichè PRImponibile */
                    //TOTALEPRESTAZIONI += double.Parse(row["PRImponibile"].ToString());
                    TOTALEPRESTAZIONI += double.Parse(row["PRtotale"].ToString());
                    //}
                    //catch { }
                }
            }
            ltlTOTALEPERIODO.Text = String.Format("{0:N2}", TOTALEPRESTAZIONI) + " € ";

            /*spostato da riga PH(1) - START */
            gridPrestazioni.DataSource = DTResult;
            gridPrestazioni.DataBind();
            /*spostato da riga PH(1) - END */

            gridPrestazioni.SelectedIndex = -1;

            for (int i = 0; i <= gridPrestazioni.Items.Count - 1; i++)
            {
                ((LinkButton)gridPrestazioni.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare la Prestazione selezionata e tutti i servizi collegati ?')){}else{return false}");

                switch (DTResult.Rows[i]["COLORE"].ToString())
                {
                    case "Pagata":
                    case "Fatturata":
                        {
                            //RENDERE MODIFICABILE SEMPRE
                            //((LinkButton)gridPrestazioni.Items[i].Cells[COL_MODIFICA].Controls[0]).Visible = false;
                            //((LinkButton)gridPrestazioni.Items[i].Cells[COL_ELIMINA].Controls[0]).Visible = false;
                            break;
                        }
                }
            }

            objPagining.CaricaPagining(phPagine, gridPrestazioni);

            //CALCOLO REDAZIONALI
            sql = "select SUM(SRimportoRedazionale) as SOMMA from servizi inner join prestazioni on servizi.SRnumeroprestazione = prestazioni.PRnumero ";
            sql += " inner join clienti on prestazioni.PRcliente=clienti.clicodice ";
            sql += " left join Lista_CategoriaServizi on Lista_CategoriaServizi.CScodice = prestazioni.PRcategoria ";
            sql += " left join documclienti on documclienti.DCid = prestazioni.PRfattura ";
            sql += " left join ModalitaPagamento on ModalitaPagamento.Codice = prestazioni.PRcondpag ";
            sql += " WHERE 1=1 ";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " AND (Clienti.cliragsoc like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR PRnumero like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR PRchiave like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR PRnote like '%" + txtFiltroLibero.Text.Trim() + "%' )";
            }
            else
            {
                //TEMPORANEO
                //sql += " WHERE PRnumero = 11807 ";
            }

            if (ViewState["FILTER_STATUS"].ToString() != "")
            {
                sql += " AND CASE ";
                sql += " WHEN ";
                sql += " PRfatturare = 0 and PRfatturato = 0 and PRpagata = 0 ";
                sql += " THEN 'EMESSA' ";
                sql += " WHEN ";
                sql += " PRfatturare = 1 and PRfatturato = 0 and PRpagata = 0 ";
                sql += " THEN 'DA FATTURARE' ";
                sql += " WHEN ";
                sql += " PRfatturato = 1  and PRpagata = 0 ";
                sql += " THEN 'FATTURATA' ";
                sql += " WHEN ";
                sql += " PRpagata = 1 and PRfatturato = 1 ";
                sql += " THEN 'PAGATA' ";
                sql += " END  = '" + ViewState["FILTER_STATUS"].ToString() + "' ";
            }

            if (TIPO == "MESEANNO" && cboMese.SelectedIndex > 0)
            {
                sql += " AND DATEPART(mm,PRdatainizio) = " + cboMese.SelectedValue + " AND DATEPART(yyyy,PRdatainizio) = " + cboAnno.SelectedValue + " ";
            }
            if (TIPO == "MESEANNO" && cboMese.SelectedIndex == 0)
            {
                sql += " AND DATEPART(yyyy,PRdatainizio) = " + cboAnno.SelectedValue + " ";
            }

            if (TIPO == "DATE")
            {
                sql += " AND PRdatainizio >= CONVERT(datetime,'" + txtFiltroInizio.Text + "',105) ";
                sql += " AND PRdatainizio <= CONVERT(datetime,'" + txtFiltroFine.Text + "',105)  ";
            }

            DataTable DTRedaz = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTRedaz, true);

            if (DTRedaz != null)
            {
                if (DTRedaz.Rows.Count > 0)
                {
                    ltlTOTALEPERIODOREDAZIONALE.Text = String.Format("{0:N2}", DTRedaz.Rows[0]["SOMMA"]) + " € ";
                }
            }
        }


        protected void lnkSerInserisci_Click(object sender, EventArgs e)
        {
            ViewState["MOD_SERVIZIO"] = "INS";
            CaricaComboAnnoMeseSERVIZI();

            cboCategoria.Enabled = false;
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            txtSerAnticipi.Text = "0";
            txtSerAnticipiGA.Text = "0";

            txtSerDiritti.Text = "0";
            txtSerFine.Text = DateTime.Today.ToShortDateString();
            txtSerImponibile.Text = "0";
            txtSerImporto.Text = "0";
            txtSerImportoGA.Text = "0";
            txtSerImportoLordo.Text = "0";
            txtSerImportoRed.Text = "0";
            txtSerImpOver.Text = "0";
            txtSerInizio.Text = DateTime.Today.ToShortDateString();
            txtSerNumGiorni.Text = "0";
            txtSerOverGA.Text = "0";

            txtSerSpesa1.Text = "0";
            txtSerSpesa2.Text = "0";
            txtSerSpesa3.Text = "0";

            txtSerSpesa1GA.Text = "0";
            txtSerSpesa2GA.Text = "0";
            txtSerSpesa3GA.Text = "0";

            txtSerSpesa1Descr.Text = "";
            txtSerSpesa2Descr.Text = "";
            txtSerSpesa3Descr.Text = "";

            txtSerSpesa1GADescr.Text = "";
            txtSerSpesa2GADescr.Text = "";
            txtSerSpesa3GADescr.Text = "";

            txtSerTrattenute.Text = "0";
            //cboSerCategoria.SelectedIndex = 0;
            cboSerTipoServizio.SelectedIndex = 0;
            cboSerFornitore.SelectedIndex = 0;
            chkEsenzioneIVA.Checked = false;

            ModificaServizioVisible = true;
            lnkSerInserisci.Enabled = false;

            txtSerInizio.Text = txtDataInizio.Text;
            txtSerFine.Text = txtDataFine.Text;

            CaricaPercentuali();
        }

        protected void lnkSerAggiorna_Click(object sender, EventArgs e)
        {
            //AGGIORNAMENTO DELLA PRESTAZIONE
            if (Convert.ToDateTime(txtSerInizio.Text) > Convert.ToDateTime(txtSerFine.Text))
            {
                lblError.Text = "Controllare le date di inserimento del servizio.";
                return;
            }
            else
            {
                lblError.Text = "";
            }

            //AGGIORNAMENTO DEL SERVIZIO
            bool ShowError = false;

            clsParameter pParameter = new clsParameter();
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRoperatore", cboSerFornitore.SelectedValue, SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRcliente", cboCliente.SelectedValue, SqlDbType.Int, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRcategoria", cboCategoria.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRtiposervizio", cboSerTipoServizio.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));

            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRimporto", txtSerImporto.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));
            string IMP_GA = txtSerImportoGA.Text.ToString().Replace(".", ",");
            //if (Convert.ToDouble(IMP_GA) == 0)
            //{
            //    IMP_GA = txtSerImporto.Text.ToString().Replace(".", ",");
            //}
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRimportoGA", IMP_GA, SqlDbType.Float, ParameterDirection.Input));

            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRimportoOvertime", txtSerImpOver.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));
            string IMP_OVGA = txtSerOverGA.Text.ToString().Replace(".", ",");
            //if (Convert.ToDouble(IMP_OVGA) == 0)
            //{
            //    IMP_OVGA = txtSerImpOver.Text.ToString().Replace(".", ",");
            //}

            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRimportoOvertimeGA", IMP_OVGA, SqlDbType.Float, ParameterDirection.Input));


            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRimponibile", txtSerImponibile.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRdiritti", txtSerDiritti.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRanticipi", txtSerAnticipi.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRanticipige", txtSerAnticipiGA.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRtrattenute", txtSerTrattenute.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRnumgiorni", txtSerNumGiorni.Text, SqlDbType.Int, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRnumeroprestazione", ViewState["PRESTSEL"].ToString(), SqlDbType.Int, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRdatainizio", txtSerInizio.Text, SqlDbType.DateTime, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRdatafine", txtSerFine.Text, SqlDbType.DateTime, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRimportoLordo", txtSerImportoLordo.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRspese", txtSerSpesa1.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRspese1", txtSerSpesa2.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRspese2", txtSerSpesa3.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));

            string SP_GA = txtSerSpesa1GA.Text.ToString().Replace(".", ",");
            //if (Convert.ToDouble(SP_GA) == 0)
            //{
            //    SP_GA = txtSerSpesa1.Text.ToString().Replace(".", ",");
            //}
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRspeseGA", SP_GA, SqlDbType.Float, ParameterDirection.Input));

            string SP1_GA = txtSerSpesa2GA.Text.ToString().Replace(".", ",");
            //if (Convert.ToDouble(SP1_GA) == 0)
            //{
            //    SP1_GA = txtSerSpesa2.Text.ToString().Replace(".", ",");
            //}
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRspeseGA1", SP1_GA, SqlDbType.Float, ParameterDirection.Input));

            string SP2_GA = txtSerSpesa3GA.Text.ToString().Replace(".", ",");
            //if (Convert.ToDouble(SP2_GA) == 0)
            //{
            //    SP2_GA = txtSerSpesa3.Text.ToString().Replace(".", ",");
            //}
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRspeseGA2", SP2_GA, SqlDbType.Float, ParameterDirection.Input));

            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRdescSpese", txtSerSpesa1Descr.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRdescSpese1", txtSerSpesa2Descr.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRdescSpese2", txtSerSpesa3Descr.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRdescSpeseGA", txtSerSpesa1GADescr.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRdescSpeseGA1", txtSerSpesa2GADescr.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRdescSpeseGA2", txtSerSpesa3GADescr.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRimportoRedazionale", txtSerImportoRed.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SResenzioneIvaArt15", chkEsenzioneIVA.Checked, SqlDbType.Bit, ParameterDirection.Input));

            //************** CARICAMENTO ALIQUOTA IVA CONDIVISA
            string sqlALIQUOTA = "SELECT top 1 AliquotaIVA FROM Admin_DatiAnagrifici";
            DataTable DTResultALIQUOTA = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sqlALIQUOTA, "RESULTALIQUOTA", ref DTResultALIQUOTA, true);
            double ALIQUOTAIVA = double.Parse(DTResultALIQUOTA.Rows[0]["AliquotaIVA"].ToString());
            //************** CARICAMENTO ALIQUOTA IVA CONDIVISA

            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRpctIVA", ALIQUOTAIVA, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRpctRitAcc", lblSerPercRitAcc.Text, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRpctRivPrev", 4, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRpctDiritti", lblSerPercDiritti.Text, SqlDbType.Float, ParameterDirection.Input));

            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRCompensoAggiunto", txtCompAggiunto.Text.ToString().Replace(".", ","), SqlDbType.Float, ParameterDirection.Input));

            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRAnnoFatt", cboSerAnno.SelectedValue, SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRMeseFatt", cboSerMese.SelectedValue, SqlDbType.SmallInt, ParameterDirection.Input));


            switch (ViewState["MOD_SERVIZIO"].ToString())
            {
                case "INS":
                    {

                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRfatturare", false, SqlDbType.Bit, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRpagato", false, SqlDbType.Bit, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SRfatturato", false, SqlDbType.Bit, ParameterDirection.Input));



                        sql = @"INSERT INTO Servizi
                                (SRCompensoAggiunto,SRAnnoFatt,SRMeseFatt,
                                SRoperatore, 
                                SRcliente, 
                                SRcategoria, 
                                SRtiposervizio, 
                                SRimporto, 
                                SRimportoGA,
                                SRimponibile, 
                                SRdiritti, 
                                SRanticipi, 
                                SRanticipige, 
                                SRtrattenute, 
                                SRnumgiorni, 
                                SRnumeroprestazione, 
                                SRdatainizio, 
                                SRdatafine, 
                                SRimportoLordo, 
                                SRdescSpese, 
                                SRspese, 
                                SRspeseGA, 
                                SRdescSpese1, 
                                SRspese1, 
                                SRspeseGA1, 
                                SRdescSpese2, 
                                SRspese2, 
                                SRspeseGA2, 
                                SRimportoRedazionale, 
                                SRimportoOvertime, 
                                SRdescSpeseGA, 
                                SRdescSpeseGA1, 
                                SRdescSpeseGA2, 
                                SRimportoOvertimeGA,
                                SRfatturare,SRpagato,SRfatturato,
                                SResenzioneIvaArt15,
                                SRpctIVA,
                                SRpctRitAcc,
                                SRpctRivPrev,
                                SRpctDiritti
                                )     
                            VALUES
                                (@SRCompensoAggiunto,@SRAnnoFatt,@SRMeseFatt,
                                @SRoperatore, 
                                @SRcliente, 
                                @SRcategoria, 
                                @SRtiposervizio, 
                                @SRimporto, 
                                @SRimportoGA,
                                @SRimponibile, 
                                @SRdiritti, 
                                @SRanticipi, 
                                @SRanticipige,
                                @SRtrattenute, 
                                @SRnumgiorni, 
                                @SRnumeroprestazione, 
                                @SRdatainizio, 
                                @SRdatafine, 
                                @SRimportoLordo, 
                                @SRdescSpese, 
                                @SRspese, 
                                @SRspeseGA, 
                                @SRdescSpese1, 
                                @SRspese1, 
                                @SRspeseGA1, 
                                @SRdescSpese2, 
                                @SRspese2, 
                                @SRspeseGA2, 
                                @SRimportoRedazionale, 
                                @SRimportoOvertime, 
                                @SRdescSpeseGA, 
                                @SRdescSpeseGA1, 
                                @SRdescSpeseGA2, 
                                @SRimportoOvertimeGA,
                                @SRfatturare,@SRpagato,@SRfatturato,
                                @SResenzioneIvaArt15,
                                @SRpctIVA,
                                @SRpctRitAcc,
                                @SRpctRivPrev,
                                @SRpctDiritti
                                ) ";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        CaricaServiziCollegati(ViewState["PRESTSEL"].ToString());
                        ModificaServizioVisible = false;
                        grdServizi.SelectedIndex = -1;
                        grdServizi.Visible = true;
                        lnkSerInserisci.Enabled = true;

                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        break;
                    }
                case "UPD":
                    {
                        ShowError = false;

                        sql = @"UPDATE Servizi
                                SET
                                SRAnnoFatt=@SRAnnoFatt,
                                SRMeseFatt=@SRMeseFatt,
                                SRCompensoAggiunto = @SRCompensoAggiunto,
                                SRoperatore = @SRoperatore, 
                                SRcliente = @SRcliente,
                                SRcategoria = @SRcategoria,
                                SRtiposervizio = @SRtiposervizio,
                                SRimporto = @SRimporto,
                                SRimportoGA = @SRimportoGA,
                                SRimponibile = @SRimponibile,
                                SRdiritti = @SRdiritti,
                                SRanticipi = @SRanticipi,
                                SRanticipige = @SRanticipige,
                                SRtrattenute = @SRtrattenute,
                                SRnumgiorni = @SRnumgiorni,
                                SRnumeroprestazione = @SRnumeroprestazione,
                                SRdatainizio = @SRdatainizio,
                                SRdatafine = @SRdatafine,
                                SRimportoLordo = @SRimportoLordo,
                                SRdescSpese = @SRdescSpese,
                                SRspese = @SRspese,
                                SRspeseGA = @SRspeseGA,
                                SRdescSpese1 = @SRdescSpese1,
                                SRspese1 = @SRspese1,
                                SRspeseGA1 = @SRspeseGA1,
                                SRdescSpese2 = @SRdescSpese2,
                                SRspese2 = @SRspese2,
                                SRspeseGA2 = @SRspeseGA2,
                                SRimportoRedazionale = @SRimportoRedazionale,
                                SRimportoOvertime = @SRimportoOvertime,
                                SRdescSpeseGA = @SRdescSpeseGA,
                                SRdescSpeseGA1 = @SRdescSpeseGA1,
                                SRdescSpeseGA2 = @SRdescSpeseGA2,
                                SRimportoOvertimeGA = @SRimportoOvertimeGA,
                                SRpctIVA = @SRpctIVA,
                                SRpctRitAcc = @SRpctRitAcc,
                                SRpctRivPrev = @SRpctRivPrev,
                                SRpctDiritti = @SRpctDiritti
                                WHERE SRcodice = " + ViewState["SERSEL"].ToString() + @"
                               ";

                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        CaricaServiziCollegati(ViewState["PRESTSEL"].ToString());
                        ModificaServizioVisible = false;
                        grdServizi.SelectedIndex = -1;
                        grdServizi.Visible = true;
                        lnkSerInserisci.Enabled = true;

                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        break;
                    }
            }
            if (ShowError)
            {
                lblError.Text = "Esite già un Servizio con queste caratteristiche.";
                lblError.Visible = true;
            }
            cboCategoria.Enabled = true;
            ViewState["MOD_SERVIZIO"] = "";
        }

        protected void lnkSerAnnulla_Click(object sender, EventArgs e)
        {
            ViewState["MOD_SERVIZIO"] = "";
            grdServizi.SelectedIndex = -1;
            ModificaServizioVisible = false;
            grdServizi.Visible = true;
            lnkSerInserisci.Enabled = true;
            cboCategoria.Enabled = true;
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
        }

        protected void lnkPrestAggiorna_Click(object sender, EventArgs e)
        {
            //AGGIORNAMENTO DELLA PRESTAZIONE
            if (Convert.ToDateTime(txtDataInizio.Text) > Convert.ToDateTime(txtDataFine.Text))
            {
                lblError.Text = "Controllare le date di inserimento della prestazione.";
                return;
            }
            else
            {
                lblError.Text = "";
            }

            CaricaServiziCollegati(ViewState["PRESTSEL"].ToString());

            bool ShowError = false;

            clsParameter pParameter = new clsParameter();
            pParameter = new clsParameter();
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRdatainizio", txtDataInizio.Text, SqlDbType.DateTime, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRcategoria", cboCategoria.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRdatafine", txtDataFine.Text, SqlDbType.DateTime, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRlingua", cboLingua.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRvaluta", cboValuta.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRImponibile", CALCOLO_IMPONIBILE, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRtipodiritti", CALCOLO_PERCDIRITTI, SqlDbType.Real, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRdiritti", CALCOLO_DIRITTI, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRpctIVA", CALCOLO_PERCIVA, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRimportoIVA", CALCOLO_IMPORTOIVA, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRtotale", CALCOLO_TOTALE, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRanticipi", CALCOLO_ANTICIPI, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRtrattenute", CALCOLO_TRATTENUTE, SqlDbType.Float, ParameterDirection.Input));
            //TOLTO PER MODIFICHE CONSECUTIVE
            //pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRfattura", System.DBNull.Value, SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRdatafattura", System.DBNull.Value, SqlDbType.DateTime, ParameterDirection.Input));

            //HERE
            if (ViewState["MODIFICASTATO"].ToString() == "1")
            {
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRfatturato", false, SqlDbType.Bit, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRfatturare", chkDAFATTURARE.Checked, SqlDbType.Bit, ParameterDirection.Input));
            }
            else
            {
                if (ViewState["MOD_PREST"].ToString() == "INS")
                {
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRfatturare", chkDAFATTURARE.Checked, SqlDbType.Bit, ParameterDirection.Input));
                }
                else
                {
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRfatturare", ViewState["PRfatturare"].ToString(), SqlDbType.Bit, ParameterDirection.Input));
                }
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRfatturato", ViewState["PRfatturato"].ToString(), SqlDbType.Bit, ParameterDirection.Input));

            }

            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRspese", CALCOLO_SPESE_0, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRspese1", CALCOLO_SPESE_1, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRspese2", CALCOLO_SPESE_2, SqlDbType.Float, ParameterDirection.Input));

            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRivaspese", CALCOLO_IVASPESE, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRrivalsa", CALCOLO_RIVALSA, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRgiorni", CALCOLO_GIORNI, SqlDbType.Float, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRnote", txtDescrizione.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRcliente", cboCliente.SelectedValue, SqlDbType.Int, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRchiave", txtParolaChiave.Text, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRovertime", CALCOLO_OVERTIME, SqlDbType.Float, ParameterDirection.Input));

            if (CALCOLO_RIVALSA != 0)
            {
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRrivalsaSN", true, SqlDbType.Bit, ParameterDirection.Input));
            }
            else
            {
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRrivalsaSN", false, SqlDbType.Bit, ParameterDirection.Input));
            }
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRIVASN", true, SqlDbType.Bit, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PRpagata", false, SqlDbType.Bit, ParameterDirection.Input));

            switch (ViewState["MOD_PREST"].ToString())
            {
                case "INS":
                    {

                        sql = @"INSERT INTO PRESTAZIONI 
                        (
                            PRdatainizio
                            ,PRovertime
                            ,PRdatafine
                            ,PRlingua
                            ,PRvaluta
                            ,PRImponibile
                            ,PRtipodiritti
                            ,PRdiritti
                            ,PRpctIVA
                            ,PRimportoIVA
                            ,PRtotale
                            ,PRanticipi
                            ,PRtrattenute
                            ,PRdatafattura
                            ,PRfatturato
                            ,PRfatturare
                            ,PRspese
                            ,PRspese1
                            ,PRspese2
                            ,PRivaspese
                            ,PRrivalsa
                            ,PRgiorni
                            ,PRnote
                            ,PRcliente
                            ,PRrivalsaSN
                            ,PRIVASN,PRpagata,PRcategoria,PRchiave
                        ) 
                        VALUES(
                            @PRdatainizio
                            ,@PRovertime
                            ,@PRdatafine
                            ,@PRlingua
                            ,@PRvaluta
                            ,@PRImponibile
                            ,@PRtipodiritti
                            ,@PRdiritti
                            ,@PRpctIVA
                            ,@PRimportoIVA
                            ,@PRtotale
                            ,@PRanticipi
                            ,@PRtrattenute
                            ,@PRdatafattura
                            ,@PRfatturato
                            ,@PRfatturare
                            ,@PRspese
                            ,@PRspese1
                            ,@PRspese2
                            ,@PRivaspese
                            ,@PRrivalsa
                            ,@PRgiorni
                            ,@PRnote
                            ,@PRcliente
                            ,@PRrivalsaSN
                            ,@PRIVASN,@PRpagata,@PRcategoria,@PRchiave
                            ); ";

                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "select TOP 1 PRNumero FROM Prestazioni ORDER BY PRNumero DESC";
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);


                        //ModificaVisible = false;
                        //CaricaDati();
                        //gridPrestazioni.SelectedIndex = -1;
                        //gridPrestazioni.Visible = true;
                        //lnkInserisci.Enabled = true;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;

                        ViewState["PRESTSEL"] = DTResult.Rows[0]["PRNUMERO"].ToString();
                        ModificaPrestazione();

                        lnkSerInserisci.Visible = true;

                        break;
                    }
                case "UPD":
                    {
                        ShowError = false;

                        sql = @"UPDATE Prestazioni
                                SET
                                PRdatainizio = @PRdatainizio
                                ,PRovertime=@PRovertime
                                ,PRdatafine = @PRdatafine
                                ,PRlingua = @PRlingua
                                ,PRvaluta = @PRvaluta
                                ,PRImponibile = @PRImponibile
                                ,PRtipodiritti = @PRtipodiritti
                                ,PRdiritti = @PRdiritti
                                ,PRpctIVA = @PRpctIVA
                                ,PRimportoIVA = @PRimportoIVA
                                ,PRtotale = @PRtotale
                                ,PRanticipi = @PRanticipi
                                ,PRtrattenute = @PRtrattenute
                                ,PRdatafattura = @PRdatafattura
                                ,PRfatturato = @PRfatturato
                                ,PRfatturare = @PRfatturare
                                ,PRspese = @PRspese
                                ,PRspese1 = @PRspese1
                                ,PRspese2 = @PRspese2
                                ,PRivaspese = @PRivaspese
                                ,PRrivalsa = @PRrivalsa
                                ,PRgiorni = @PRgiorni
                                ,PRnote = @PRnote
                                ,PRcliente = @PRcliente
                                ,PRrivalsaSN = @PRrivalsaSN
                                ,PRIVASN = @PRIVASN
                                ,PRpagata = @PRpagata
                                ,PRcategoria = @PRcategoria
                                ,PRchiave = @PRchiave
                                WHERE prnumero = " + ViewState["PRESTSEL"].ToString() + @"
                               ";

                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                        
                        //Modificata update per tener conto delle categorie di servizi Rifferite. Una volta aggiornata la prestazione, 
                        //se la categoria è cambiata, vanno aggiornati gli stati dei servizi d'accordo con la presenza o meno della fattura cliente
                        sql = @"UPDATE Servizi
                                SET
                                SRimportoLordo = SRimporto * SRnumgiorni + SRimportoOvertime
			                    , SRdiritti = SRimportoLordo * (select Forperc" + cboCategoria.SelectedValue + @" from Fornitori WHERE Forcodice = SRoperatore) / 100
			                    , SRimponibile = SRimportoLordo - SRdiritti
			                    , SRCompensoAggiunto = SRimponibile * (select PercCompenso from Lista_CategoriaServizi WHERE CSCodice = " + cboCategoria.SelectedValue + @") / 100
			                    , SRimportoRedazionale = SRCompensoAggiunto 
                                , SRfatturare = CASE (select FatturaRifferita from Lista_CategoriaServizi WHERE CSCodice = " + cboCategoria.SelectedValue + @") 
                                    WHEN 1 THEN (
                                        CASE
                                        WHEN PR.PRPagata = 0 THEN 0 ELSE 1
                                        END
                                    ) 
                                    WHEN 0 THEN (
                                        CASE
		                                WHEN PR.PRfattura IS NULL THEN 0 ELSE 1 
		                                END
                                    ) 
                                END
                                , SRdatainizio = @PRdatainizio
                                , SRdatafine = @PRdatafine
                                FROM Prestazioni PR
                                LEFT JOIN Servizi SR ON  PR.PRNumero = SR.SRNumeroprestazione  
                                WHERE srnumeroprestazione = " + ViewState["PRESTSEL"].ToString() + @"
                               ";

                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        ModificaVisible = false;
                        CaricaDati();
                        gridPrestazioni.SelectedIndex = -1;
                        gridPrestazioni.Visible = true;
                        lnkInserisci.Enabled = true;
                        //CaricaServiziCollegati(ViewState["PRESTSEL"].ToString());
                        break;
                    }
            }
            if (ShowError)
            {
                lblError.Text = "Esite già un Servizio con queste caratteristiche.";
                lblError.Visible = true;
            }
            ViewState["MOD_PREST"] = "";
        }

        protected void lnkPrestAnnulla_Click(object sender, EventArgs e)
        {
            ViewState["MOD_PREST"] = "";
            gridPrestazioni.SelectedIndex = -1;
            ModificaVisible = false;
            gridPrestazioni.Visible = true;
            phPagine.Visible = true;
            lnkInserisci.Enabled = true;
            cboCategoria.Enabled = true;

            lnkSerInserisci.Visible = true;
            lnkPrestAggiorna.Visible = true;
            CaricaDati();
        }


        private void CaricaServiziCollegati(string NumPrest)
        {
            if (NumPrest == "") return;

            ModificaServizioVisible = false;

            sql = "select srimponibile + srcompensoaggiunto + srspese + srspese1 + srspese2 AS TOTFINALE ";
            sql += " ,srcompensoaggiunto, ";
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
            sql += " SRdatainizio, ";
            sql += " SRdatafine, ";
            sql += " SRimportoLordo, ";
            sql += " SRpctDiritti, ";
            sql += " SRdescSpese,";
            sql += @" SRspese,
            SRdescSpeseGA,
            SRspeseGA,
            SRdescSpese1,
            SRspese1,
            SRdescSpeseGA1,
            SRspeseGA1,
            SRdescSpese2,
            SRspese2,
            SRdescSpeseGA2,
            SRspeseGA2, ";
            sql += " CASE  ";
            sql += " WHEN  ";
            sql += " SRAnnoFatt is not null ";
            sql += " THEN 'Anno: ' + CAST(SRAnnoFatt as nvarchar(50)) ELSE '' END AS SRAnnoFatt,";
            sql += " CASE  ";
            sql += " WHEN  ";
            sql += " SRMeseFatt is not null ";
            sql += " THEN 'Mese: ' + CAST(SRMeseFatt as nvarchar(50)) ELSE '' END AS SRMeseFatt,";
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
            sql += " END AS COLORE ";
            sql += " from servizi ";
            sql += " inner join fornitori on servizi.sroperatore=fornitori.forcodice ";
            sql += " left join Lista_CategoriaServizi on Lista_CategoriaServizi.CScodice = servizi.srcategoria ";
            sql += " left join Lista_Tiposervizio on servizi.srtiposervizio = Lista_Tiposervizio.cscodice ";
            sql += " where srnumeroprestazione = " + NumPrest + " ";

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            grdServizi.DataSource = DTResult;
            grdServizi.DataBind();

            SER_TOTIMPO = 0;
            SER_TOTLORDO = 0;
            SER_TOTDIRITTI = 0;
            SER_TOTSPESE = 0;

            CALCOLO_IMPONIBILE = 0;
            CALCOLO_PERCDIRITTI = 0;
            CALCOLO_DIRITTI = 0;
            CALCOLO_PERCIVA = 0;
            CALCOLO_IMPORTOIVA = 0;
            CALCOLO_TOTALE = 0;
            CALCOLO_ANTICIPI = 0;
            CALCOLO_TRATTENUTE = 0;
            CALCOLO_SPESE = 0;

            CALCOLO_SPESE_0 = 0;
            CALCOLO_SPESE_1 = 0;
            CALCOLO_SPESE_2 = 0;

            CALCOLO_IVASPESE = 0;
            CALCOLO_RIVALSA = 0;
            CALCOLO_GIORNI = 0;
            CALCOLO_OVERTIME = 0;

            for (int i = 0; i <= grdServizi.Items.Count - 1; i++)
            {
                ((LinkButton)grdServizi.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare il Servizio selezionato ?')){}else{return false}");

                switch (DTResult.Rows[i]["COLORE"].ToString())
                {
                    case "Pagata":
                    case "Fatturata":
                        {
                            //RENDERE MODIFICABILE SEMPRE
                            //((LinkButton)grdServizi.Items[i].Cells[COL_MODIFICA].Controls[0]).Visible = false;
                            //((LinkButton)grdServizi.Items[i].Cells[COL_ELIMINA].Controls[0]).Visible = false;
                            break;
                        }
                }

                //CALCOLO I TOTALI PER PRESTAZIONE
                CALCOLO_RIVALSA = 0;

                double COMPAGG = 0;
                try
                {
                    COMPAGG = Convert.ToDouble(DTResult.Rows[i]["srcompensoaggiunto"].ToString().Replace(".", ","));
                }
                catch { }

                //CALCOLO_PERCIVA = Convert.ToDouble(DTResult.Rows[i]["SRpctIVA"].ToString().Replace(".",","));
                //CALCOLO_PERCDIRITTI = Convert.ToDouble(DTResult.Rows[i]["SRpctDiritti"].ToString().Replace(".", ","));
                //CALCOLO_IVASPESE = Convert.ToDouble(DTResult.Rows[i]["SRpctIVA"].ToString().Replace(".", ","));
                //CALCOLO_IMPONIBILE += Convert.ToDouble(DTResult.Rows[i]["SRimponibile"].ToString().Replace(".", ",")) + COMPAGG;
                //CALCOLO_DIRITTI += Convert.ToDouble(DTResult.Rows[i]["SRdiritti"].ToString().Replace(".", ","));
                //CALCOLO_IMPORTOIVA = (Convert.ToDouble(DTResult.Rows[i]["SRimponibile"].ToString().Replace(".", ",")) * Convert.ToDouble(DTResult.Rows[i]["SRpctIVA"].ToString().Replace(".", ",")) / 100);
                //CALCOLO_TOTALE += Convert.ToDouble(DTResult.Rows[i]["SRimportoLordo"].ToString().Replace(".", ","));
                //CALCOLO_ANTICIPI += Convert.ToDouble(DTResult.Rows[i]["SRanticipi"].ToString().Replace(".", ","));
                //CALCOLO_TRATTENUTE += Convert.ToDouble(DTResult.Rows[i]["SRtrattenute"].ToString().Replace(".", ","));
                //CALCOLO_SPESE += Convert.ToDouble(DTResult.Rows[i]["SRspese"].ToString().Replace(".", ",")) + Convert.ToDouble(DTResult.Rows[i]["SRspese1"].ToString().Replace(".", ",")) + Convert.ToDouble(DTResult.Rows[i]["SRspese2"].ToString().Replace(".", ",")) + Convert.ToDouble(DTResult.Rows[i]["SRspeseGA"].ToString().Replace(".", ",")) + Convert.ToDouble(DTResult.Rows[i]["SRspeseGA1"].ToString().Replace(".", ",")) + Convert.ToDouble(DTResult.Rows[i]["SRspeseGA2"].ToString().Replace(".", ","));
                //CALCOLO_GIORNI += Convert.ToDouble(DTResult.Rows[i]["SRnumgiorni"]);

                //CALCOLO_OVERTIME += Convert.ToDouble(DTResult.Rows[i]["SRImportoOvertime"].ToString().Replace(".", ",")) + Convert.ToDouble(DTResult.Rows[i]["SRImportoOvertimeGA"].ToString().Replace(".", ","));

                double SRpctIVA = Convert.ToDouble(DTResult.Rows[i]["SRpctIVA"].ToString().Replace(".", ","));

                double SRpctDiritti = 0;
                sql = " select clipercdiritti,GACalcoloIVA from clienti where clicodice = " + cboCliente.SelectedValue;
                DataTable DTCliente = new DataTable();
                clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTCliente, true);
                if (DTCliente != null)
                {
                    if (DTCliente.Rows.Count >= 0)
                    {
                        ViewState["CALCOLOIVA"] = Convert.ToBoolean(DTCliente.Rows[0]["GACalcoloIVA"]);
                        SRpctDiritti = Convert.ToDouble(DTCliente.Rows[0]["clipercdiritti"].ToString());
                    }
                    else
                    {
                        ViewState["CALCOLOIVA"] = true;
                    }
                }

                double SRnumgiorni = Convert.ToDouble(DTResult.Rows[i]["SRnumgiorni"].ToString().Replace(".", ","));
                double SRdiritti = Convert.ToDouble(DTResult.Rows[i]["SRdiritti"].ToString().Replace(".", ","));
                double SRanticipi = Convert.ToDouble(DTResult.Rows[i]["SRanticipi"].ToString().Replace(".", ","));
                double SRtrattenute = Convert.ToDouble(DTResult.Rows[i]["SRtrattenute"].ToString().Replace(".", ","));

                double SRimportoGA = Convert.ToDouble(DTResult.Rows[i]["SRimportoGA"].ToString().Replace(".", ","));
                double SRImportoOvertimeGA = Convert.ToDouble(DTResult.Rows[i]["SRImportoOvertimeGA"].ToString().Replace(".", ","));

                double SRimporto = Convert.ToDouble(DTResult.Rows[i]["SRimporto"].ToString().Replace(".", ","));
                double SRimponibile = Convert.ToDouble(DTResult.Rows[i]["SRimponibile"].ToString().Replace(".", ","));
                double SRImportoOvertime = Convert.ToDouble(DTResult.Rows[i]["SRImportoOvertime"].ToString().Replace(".", ","));
                double SRcompensoaggiunto = Convert.ToDouble(DTResult.Rows[i]["SRcompensoaggiunto"].ToString().Replace(".", ","));

                double SRspese = Convert.ToDouble(DTResult.Rows[i]["SRspese"].ToString().Replace(".", ","));
                double SRspese1 = Convert.ToDouble(DTResult.Rows[i]["SRspese1"].ToString().Replace(".", ","));
                double SRspese2 = Convert.ToDouble(DTResult.Rows[i]["SRspese2"].ToString().Replace(".", ","));

                double SRspeseGA = Convert.ToDouble(DTResult.Rows[i]["SRspeseGA"].ToString().Replace(".", ","));
                double SRspeseGA1 = Convert.ToDouble(DTResult.Rows[i]["SRspeseGA1"].ToString().Replace(".", ","));
                double SRspeseGA2 = Convert.ToDouble(DTResult.Rows[i]["SRspeseGA2"].ToString().Replace(".", ","));

                SER_TOTIMPO += SRimponibile;
                SER_TOTAGG += SRcompensoaggiunto;

                SER_TOTLORDO += (SRimporto * SRnumgiorni) + SRImportoOvertime;
                SER_TOTDIRITTI += SRdiritti;////((SRimporto * SRnumgiorni) + SRImportoOvertime) * SRpctDiritti / 100;
                SER_TOTSPESE += SRspese + SRspese1 + SRspese2;

                CALCOLO_PERCIVA = SRpctIVA;

                //Calcolo diritti da anagrafica cliente

                CALCOLO_PERCDIRITTI = SRpctDiritti;
                CALCOLO_IVASPESE = SRpctIVA;
                //////CALCOLO_IMPONIBILE += (SRimportoGA * SRnumgiorni);

                CALCOLO_IMPONIBILE += (SRimportoGA * SRnumgiorni);

                //LORDO - DIRITTI
                double DIRITTI = (((SRimportoGA) * SRnumgiorni) + SRImportoOvertimeGA) * SRpctDiritti / 100;

                //******************
                //****SE CLIENTE NON CALCOLA IVA **************
                //******************
                double IVA_CALC = 0;
                if (Convert.ToBoolean(ViewState["CALCOLOIVA"]))
                {
                    IVA_CALC = (((SRimportoGA) * SRnumgiorni) + SRImportoOvertimeGA);
                    IVA_CALC += SRspeseGA + SRspeseGA1 + SRspeseGA2 + DIRITTI;
                    IVA_CALC = IVA_CALC * SRpctIVA / 100;
                    CALCOLO_IMPORTOIVA += IVA_CALC;
                }
                //******************
                //******************
                //******************
                //******************
                //******************
                //******************


                ////////CALCOLO_TOTALE += (SRimportoGA + SRImportoOvertimeGA) * SRnumgiorni;

                CALCOLO_TOTALE += (SRimportoGA * SRnumgiorni) + SRImportoOvertimeGA;
                CALCOLO_ANTICIPI += SRanticipi;
                CALCOLO_TRATTENUTE += SRtrattenute;
                CALCOLO_SPESE += SRspeseGA + SRspeseGA1 + SRspeseGA2;

                CALCOLO_SPESE_0 += SRspeseGA;
                CALCOLO_SPESE_1 += SRspeseGA1;
                CALCOLO_SPESE_2 += SRspeseGA2;

                CALCOLO_GIORNI += SRnumgiorni;
                //////CALCOLO_OVERTIME += (SRImportoOvertimeGA * SRnumgiorni);
                //////CALCOLO_DIRITTI += (((SRimportoGA + SRImportoOvertimeGA) * SRnumgiorni) * SRpctDiritti) / 100;

                CALCOLO_OVERTIME += SRImportoOvertimeGA;
                CALCOLO_DIRITTI += ((SRimportoGA * SRnumgiorni) + SRImportoOvertimeGA) * SRpctDiritti / 100;

            }

            lblSer_TOTIMPO.Text = String.Format("{0:N2}", SER_TOTIMPO + SER_TOTAGG);
            lblSer_TOTLORDO.Text = String.Format("{0:N2}", SER_TOTLORDO);
            lblSer_TOTDIRITTI.Text = String.Format("{0:N2}", SER_TOTDIRITTI);
            lblSer_TOTSPESE.Text = String.Format("{0:N2}", SER_TOTSPESE);

            lblCALCOLO_PERCIVA.Text = CALCOLO_PERCIVA.ToString();
            lblCALCOLO_PERCDIRITTI.Text = CALCOLO_PERCDIRITTI.ToString();
            lblCALCOLO_IVASPESE.Text = CALCOLO_IVASPESE.ToString();
            lblCALCOLO_IMPONIBILE.Text = String.Format("{0:N2}", CALCOLO_IMPONIBILE);
            lblCALCOLO_DIRITTI.Text = String.Format("{0:N2}", CALCOLO_DIRITTI);
            lblCALCOLO_IMPORTOIVA.Text = String.Format("{0:N2}", CALCOLO_IMPORTOIVA);
            lblCALCOLO_TOTALE.Text = String.Format("{0:N2}", CALCOLO_TOTALE);
            lblCALCOLO_ANTICIPI.Text = String.Format("{0:N2}", CALCOLO_ANTICIPI);
            lblCALCOLO_TRATTENUTE.Text = String.Format("{0:N2}", CALCOLO_TRATTENUTE);
            lblCALCOLO_SPESE.Text = String.Format("{0:N2}", CALCOLO_SPESE);

            lblCALCOLO_RIVALSA.Text = String.Format("{0:N2}", CALCOLO_RIVALSA);
            lblCALCOLO_GIORNI.Text = String.Format("{0:N2}", CALCOLO_GIORNI);
            lblCALCOLO_OVERTIME.Text = String.Format("{0:N2}", CALCOLO_OVERTIME);


        }

        #endregion

        protected void lnkFilterESEGUITO_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "EMESSA";

            lblFiltroSel.Text = "Filtro selezionato : EMESSA";
            lnkFilterRemove.Visible = true;

            CaricaDati();
        }
        protected void lnkFilterDAFATTURARE_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "DA FATTURARE";

            lblFiltroSel.Text = "Filtro selezionato : DA FATTURARE";
            lnkFilterRemove.Visible = true;

            CaricaDati();
        }
        protected void lnkFilterFATTURATA_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "FATTURATA";
            lblFiltroSel.Text = "Filtro selezionato : FATTURATA";
            lnkFilterRemove.Visible = true;
            CaricaDati();
        }
        protected void lnkFilterPAGATA_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "PAGATA";
            lblFiltroSel.Text = "Filtro selezionato : PAGATA";
            lnkFilterRemove.Visible = true;
            CaricaDati();

        }
        protected void lnkFilterESCLUDIPAGATI_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "ESCLUDIPAGATI";
            lblFiltroSel.Text = "Filtro selezionato : ESCLUDI PAGATI";
            lnkFilterRemove.Visible = true;
            CaricaDati("");

        }
        protected void lnkFilterRemove_Click(object sender, EventArgs e)
        {
            ViewState["FILTER_STATUS"] = "";
            lblFiltroSel.Text = "";
            lnkFilterRemove.Visible = false;
            CaricaDati();
        }
        protected void lnkCERCAFORNITORE_Click(object sender, EventArgs e)
        {
            CaricaFornitoreCBO();
        }

        private void CaricaFornitoreCBO()
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

        protected void lnkPrestFATTURACLIENTE_Click(object sender, EventArgs e)
        {
            ViewState["MOD_PREST"] = "UPD";
            lnkPrestAggiorna_Click(new object(), new EventArgs());

            //GOTO FATTURA CLIENTE
            if (ViewState["FATTURAESISTE"].ToString() != "")
            {
                Response.Redirect("Fattura.aspx?DCId=" + ViewState["FATTURAESISTE"].ToString());
            }
            else
            {
                Response.Redirect("Fattura.aspx?IDPrestazione=" + ViewState["PRESTSEL"].ToString());
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

            CaricaComboAnnoMeseSERVIZI();
        }


        private void CaricaComboAnnoMeseSERVIZI()
        {
            cboSerAnno.Items.Clear();
            ListItem myItemA = new ListItem();
            myItemA.Value = "0";
            myItemA.Text = "--Anno--";
            cboSerAnno.Items.Add(myItemA);

            for (int i = 2000; i <= (DateTime.Now.Year + 2); i++)
            {
                myItemA = new ListItem();
                myItemA.Value = "" + i;
                myItemA.Text = "" + i;
                cboSerAnno.Items.Add(myItemA);
            }

            for (int i = 0; i <= cboSerAnno.Items.Count - 1; i++)
            {
                if (cboSerAnno.Items[i].Value == DateTime.Now.Year.ToString())
                {
                    cboSerAnno.SelectedIndex = i;
                    break;
                }
            }

            cboSerMese.Items.Clear();

            ListItem myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "--Mese--";
            cboSerMese.Items.Add(myItem);

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
                cboSerMese.Items.Add(myItem);
            }

            for (int i = 0; i <= cboSerMese.Items.Count - 1; i++)
            {
                if (cboSerMese.Items[i].Value == DateTime.Now.Month.ToString())
                {
                    cboSerMese.SelectedIndex = i;
                    break;
                }
            }

        }

        protected void lnkFiltroMeseAnno_Click(object sender, EventArgs e)
        {
            CaricaDati("MESEANNO");
        }
        protected void lnkFiltroDate_Click(object sender, EventArgs e)
        {
            CaricaDati("DATE");
        }
        protected void chkDAFATTURARE_CheckedChanged(object sender, EventArgs e)
        {
            lnkPrestFATTURACLIENTE.Visible = chkDAFATTURARE.Checked;
        }
    }
}