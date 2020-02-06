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
    public partial class Green_Admin_SimpleList : Green_BaseUserControl
    {

        #region DECLARATIONS

        protected string NOMEOGGETTO = "Nome Oggetto";
        protected string MEX_NOMEOGGETTO = "l'oggetto";
        protected string TABLENAME = "TABELLA";
        protected string COLUMNID = "ID";
        protected string COLUMNDESCR = "DESCRIZIONE";
        protected string COLUMNORDER = "POSIZIONE";
        protected string sqlControl = "";
        protected int COL_MODIFICA = 2;
        protected int COL_ELIMINA = 3;
        protected bool CheckUnivocita = true;
        protected bool CheckOrder = true;
        protected bool NvarcharID = false;
        protected bool NvarcharPOS=false;

        private string sql = "";
        clsPagining objPagining = new clsPagining();
        #endregion
        
        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            COL_MODIFICA = 3;
            COL_ELIMINA = 4;

            TABLENAME = ((clsSession)Session["GreenApple"]).SimpleList;
            CheckUnivocita = true;
            CheckOrder = true;
            NvarcharID = false;

            switch (TABLENAME)
            {
                case "Lista_CategoriaServizi":
                    {
                        NOMEOGGETTO = "Categoria Servizi";
                        MEX_NOMEOGGETTO = "la Categoria Servizi";
                        COLUMNID = "CSCodice";
                        COLUMNDESCR = "CSDescrizione";
                        COLUMNORDER = "Posizione";
                        sqlControl = "select * from prestazioni where PRCategoria = @CODE";
                        break;
                    }
                case "Lista_CategoriaSpese":
                    {
                        NOMEOGGETTO = "Categoria Spese";
                        MEX_NOMEOGGETTO = "la Categoria Spese";
                        COLUMNID = "ID";
                        COLUMNDESCR = "Descrizione";
                        COLUMNORDER = "Posizione";
                        break;
                    }
                case "Lista_Gruppi":
                    {
                        NOMEOGGETTO = "Gruppi";
                        MEX_NOMEOGGETTO = "il Gruppo";
                        COLUMNID = "IDGruppo";
                        COLUMNDESCR = "DescGruppo";
                        COLUMNORDER = "Posizione";
                        break;
                    }
                case "Lista_PrestazioniTipi":
                    {
                        NOMEOGGETTO = "Tipo Prestazione";
                        MEX_NOMEOGGETTO = "il Tipo Prestazione";
                        COLUMNID = "IDTipo";
                        COLUMNDESCR = "Descrizione";
                        COLUMNORDER = "Posizione";
                        sqlControl = "select * from servizi where SRTipoServizio = @CODE";
                        break;
                    }
                case "Lista_TipoDocumentiClienti":
                    {
                        NOMEOGGETTO = "Tipo Documenti";
                        MEX_NOMEOGGETTO = "il Tipo Documento";
                        COLUMNID = "IDTipo";
                        COLUMNDESCR = "Descrizione";
                        COLUMNORDER = "Posizione";
                        break;
                    }
                case "Lista_TipoPagamento":
                    {
                        NOMEOGGETTO = "Tipo Pagamento";
                        MEX_NOMEOGGETTO = "il Tipo Pagamento";
                        COLUMNID = "TPCodice";
                        COLUMNDESCR = "TPDescrizione";
                        COLUMNORDER = "Posizione";
                        break;
                    }
                case "Lista_TipoServizio":
                    {
                        NOMEOGGETTO = "Tipo Servizio";
                        MEX_NOMEOGGETTO = "il Tipo Servizio";
                        COLUMNID = "CSCodice";
                        COLUMNDESCR = "CSDescrizione";
                        COLUMNORDER = "Posizione";
                        sqlControl = "select * from Servizi where SRTipoServizio = @CODE";
                        
                        break;
                    }
                case "Lista_Professioni":
                    {
                        NOMEOGGETTO = "Professioni";
                        MEX_NOMEOGGETTO = "la Professione";
                        COLUMNID = "Profcodice";
                        COLUMNDESCR = "Profdescrizione";
                        COLUMNORDER = "Posizione";
                        break;
                    }
                case "Lista_Valuta":
                    {
                        CheckOrder = false;
                        NvarcharID = true;
                        NvarcharPOS=true;

                        NOMEOGGETTO = "Valuta";
                        MEX_NOMEOGGETTO = "la Valuta";
                        COLUMNID = "Codice";
                        COLUMNDESCR = "Descrizione";
                        COLUMNORDER = "Codice";

                        sqlControl = "select * from Prestazioni where PRValuta = '@CODE'";
                        
                        break;
                    }
                case "Lista_Lingua":
                    {
                        NOMEOGGETTO = "Lingua";
                        MEX_NOMEOGGETTO = "la Lingua";
                        COLUMNID = "Codice";
                        COLUMNDESCR = "Descrizione";
                        COLUMNORDER = "Posizione";
                        sqlControl = "select * from Prestazioni where PRLingua = '@CODE'";
                        
                        break;
                    }
                case "Lista_GiorniMesi":
                    {
                        CheckUnivocita = false;
                        NOMEOGGETTO = "Giorni";
                        MEX_NOMEOGGETTO = "il Mese / Giorno";
                        COLUMNID = "Mese";
                        COLUMNDESCR = "Giorni";
                        COLUMNORDER = "Mese";
                        break;
                    }
                default:
                    {
                        ViewState["URLDESTINAZIONE"] = "Home.aspx";
                        DoAzione();
                        break;
                    }
            }

            gridData.DataKeyField = COLUMNID;

            ((BoundColumn)gridData.Columns[0]).DataField = COLUMNID;
            ((BoundColumn)gridData.Columns[0]).HeaderText = "ID";

            ((BoundColumn)gridData.Columns[1]).DataField = COLUMNORDER;
            ((BoundColumn)gridData.Columns[1]).HeaderText = COLUMNORDER;

            ((BoundColumn)gridData.Columns[2]).DataField = COLUMNDESCR;
            ((BoundColumn)gridData.Columns[2]).HeaderText = COLUMNDESCR;

            if (CheckOrder == false)
            {
                gridData.Columns[5].Visible = false;
                gridData.Columns[6].Visible = false;
            }
            objPagining.CaricaPagining(phPagine, gridData);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            lblError.Text = "";
            lblError.Visible = false;
            if (!IsPostBack)
            {
                ModificaVisible = false;
                CaricaDati();
            }
        }
        protected void objPagining_PageChange(object sender, clsPagining.CustomPaginingArgs e)
        {
            gridData.SelectedIndex = -1;
            gridData.EditItemIndex = -1;
            gridData.CurrentPageIndex = e.NewPage;
            CaricaDati();
        }
        protected void gridData_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "MODIFICA":
                    {
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        gridData.SelectedIndex = e.Item.ItemIndex;
                        txtElemento.Text = gridData.Items[e.Item.ItemIndex].Cells[2].Text.Replace("&nbsp;", "").Trim();
                        txtPosizione.Text = gridData.Items[e.Item.ItemIndex].Cells[1].Text.Replace("&nbsp;", "").Trim();

                        ModificaVisible = true;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtElemento, this.Page);
                        break;
                    }
                case "ELIMINA":
                    {
                        clsParameter pParameter = new clsParameter();

                        if (NvarcharID==false)
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));
                        }
                        else
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.NVarChar, ParameterDirection.Input));
                        }

                        sql = "DELETE FROM " + TABLENAME + " WHERE " + COLUMNID + "=@COLUMNID";
						clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                        ModificaVisible = false;
                        CaricaDati();
                        
                        break;
                    }
                case "DOWN":
                    {
                        clsParameter pParameter = new clsParameter();
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));
                        sql = "UPDATE " + TABLENAME + " SET " + COLUMNORDER + " = " + COLUMNORDER + "+1 WHERE " + COLUMNID + "=@COLUMNID";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "UPDATE " + TABLENAME + " SET " + COLUMNORDER + " = " + COLUMNORDER + " - 1 ";
                        sql += "WHERE " + COLUMNORDER + " IN (SELECT " + COLUMNORDER + " FROM " + TABLENAME + " WHERE " + COLUMNID + " = @COLUMNID) AND " + COLUMNID + " <> @COLUMNID ";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                        
                        ModificaVisible = false;
                        CaricaDati();
                        
                        break;
                    }
                case "UP":
                    {
                        clsParameter pParameter = new clsParameter();
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));
                        sql = "UPDATE " + TABLENAME + " SET " + COLUMNORDER + " = " + COLUMNORDER + "-1 WHERE " + COLUMNID + "=@COLUMNID";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "UPDATE " + TABLENAME + " SET " + COLUMNORDER + " = " + COLUMNORDER + " + 1 ";
                        sql += "WHERE " + COLUMNORDER + " IN (SELECT " + COLUMNORDER + " FROM " + TABLENAME + " WHERE " + COLUMNID + " = @COLUMNID) AND " + COLUMNID + " <> @COLUMNID ";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                        
                        ModificaVisible = false;
                        CaricaDati();
                        
                        break;
                    }
            }
        }
        protected void lnkInserisci_Click(object sender, EventArgs e)
        {
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            gridData.SelectedIndex = -1;
            ModificaVisible = true;
            txtElemento.Text = "";
            txtPosizione.Text = "";

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtElemento, this.Page);
            this.Page.Validate();
        }
        protected void lnkAnnulla_Click(object sender, EventArgs e)
        {
            gridData.SelectedIndex = -1;
            ModificaVisible = false;
            CaricaDati();
        }
        protected void lnkAggiorna_Click(object sender, EventArgs e)
        {
            bool ShowError = false;
            string Elemento = txtElemento.Text.Trim();
            string Posizione = txtPosizione.Text.Trim();

            switch (((clsSession)Session["GreenApple"]).AzioneCorrente)
            {
                case clsSession.AzioniPagina.Inserimento:
                    {
                        sql = "SELECT * FROM " + TABLENAME + " WHERE " + COLUMNDESCR + "='" + Elemento + "' ";
                        DataTable DTCheck = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                        if (DTCheck.Rows.Count == 0)
                        {
                            clsParameter pParameter = new clsParameter();
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ELEMENTO", Elemento, SqlDbType.NVarChar, ParameterDirection.Input));

                            if (NvarcharID == false)
                            {
                                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@POSIZIONE", Posizione, SqlDbType.Int, ParameterDirection.Input));
                            }
                            else
                            {
                                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@POSIZIONE", Posizione, SqlDbType.NVarChar, ParameterDirection.Input));
                            }
                            
                            sql = "INSERT INTO " + TABLENAME + "(" + COLUMNDESCR + "," + COLUMNORDER + ") "
                            + " VALUES(@ELEMENTO,@POSIZIONE) ";
							clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                            ModificaVisible = false;
                            CaricaDati();
                            
                            gridData.SelectedIndex = -1;
                        }
                        else
                        {
                            ShowError = true;
                        }
                        break;
                    }
                case clsSession.AzioniPagina.Modifica:
                    {
                        ShowError = false;

                        if (CheckUnivocita)
                        {
                            sql = "SELECT * FROM " + TABLENAME + " WHERE " + COLUMNDESCR + "='" + Elemento + "' ";
                            sql += " AND " + COLUMNID + " <> " + gridData.DataKeys[gridData.SelectedIndex];
                            DataTable DTCheck = new DataTable();
                            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                            if (DTCheck.Rows.Count > 0)
                            {
                                ShowError = true;
                            }
                        }

                        if (ShowError == false)
                        {
                            clsParameter pParameter = new clsParameter();
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ELEMENTO", Elemento, SqlDbType.NVarChar, ParameterDirection.Input));
                            
                            if (NvarcharID == false)
                            {
                                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[gridData.SelectedIndex], SqlDbType.Int, ParameterDirection.Input));
                            }
                            else
                            {
                                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[gridData.SelectedIndex], SqlDbType.NVarChar, ParameterDirection.Input));
                            }

                            if (NvarcharPOS == false)
                            {
                                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@POSIZIONE", Posizione, SqlDbType.Int, ParameterDirection.Input));
                            }
                            else
                            {
                                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@POSIZIONE", Posizione, SqlDbType.NVarChar, ParameterDirection.Input));
                            }

                            sql = "UPDATE " + TABLENAME + " "
                            + " SET " + COLUMNDESCR + " = @ELEMENTO "
                            + "," + COLUMNORDER + " = @POSIZIONE "
                            + " WHERE " + COLUMNID + "=@COLUMNID";
                            clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                            ModificaVisible = false;

                            CaricaDati();
                            gridData.SelectedIndex = -1;
                        }
                        break;
                    }
            }
            if (ShowError)
            {
                lblError.Text = "Esite già un " + NOMEOGGETTO + " con lo stesso nome.";
                lblError.Visible = true;
            }
        }
        
        protected void lnkCerca_Click(object sender, EventArgs e)
        {
            CaricaDati();
        }
        #endregion

        #region FUNCTIONS
        private void CaricaDati()
        {

            sql = "SELECT * FROM " + TABLENAME;

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " WHERE " + COLUMNDESCR + " like '%" + txtFiltroLibero.Text.Trim() + "%'";
            }

            sql += " ORDER BY " + COLUMNORDER;
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
                ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare " + MEX_NOMEOGGETTO + " selezionato/a ?')){}else{return false}");

                if (sqlControl != "")
                {
                    string sqlControlCODE = sqlControl.Replace("@CODE", DTResult.Rows[i][COLUMNID].ToString());
                    DataTable DTControl = new DataTable();
                    clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sqlControlCODE, "CONTROL", ref DTControl, true);
                    if (DTControl.Rows.Count > 0)
                    {
                        ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Visible = false;
                    }
                }
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
        
    }
}