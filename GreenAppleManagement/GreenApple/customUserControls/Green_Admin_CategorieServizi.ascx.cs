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
    public partial class Green_Admin_CategorieServizi : Green_BaseUserControl
    {

        #region DECLARATIONS

        protected string NOMEOGGETTO = "Nome Oggetto";
        protected string NOMEPERCCOMPENSO = "Nome Percentuale Compenso";
        protected string MEX_NOMEOGGETTO = "l'oggetto";
        protected string TABLENAME = "TABELLA";
        protected string COLUMNID = "ID";
        protected string COLUMNDESCR = "DESCRIZIONE";
        protected string COLUMNORDER = "POSIZIONE";
        protected string COLPERCCOMPENSO = "AAA";
        protected string COLDIFFERITA = "AAA";
        protected string sqlControl = "";
        protected int COL_MODIFICA = 3;
        protected int COL_ELIMINA = 4;
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

            COL_MODIFICA = 5;
            COL_ELIMINA = 6;

            TABLENAME = "Lista_CategoriaServizi";
            CheckUnivocita = true;
            CheckOrder = true;
            NvarcharID = false;

            
            NOMEOGGETTO = "Categoria Servizi";
            NOMEPERCCOMPENSO = "Percentuale Compenso";
            MEX_NOMEOGGETTO = "la Categoria Servizi";
            COLUMNID = "CSCodice";
            COLUMNDESCR = "CSDescrizione";
            COLUMNORDER = "Posizione";
            COLPERCCOMPENSO = "PercCompenso";
            COLDIFFERITA = "FatturaRifferita";
            sqlControl = "select * from prestazioni where PRCategoria = @CODE";

            gridData.DataKeyField = COLUMNID;

            ((BoundColumn)gridData.Columns[0]).DataField = COLUMNID;
            ((BoundColumn)gridData.Columns[0]).HeaderText = "ID";

            ((BoundColumn)gridData.Columns[1]).DataField = COLUMNORDER;
            ((BoundColumn)gridData.Columns[1]).HeaderText = COLUMNORDER;

            ((BoundColumn)gridData.Columns[2]).DataField = COLUMNDESCR;
            ((BoundColumn)gridData.Columns[2]).HeaderText = COLUMNDESCR;

            ((BoundColumn)gridData.Columns[3]).DataField = COLPERCCOMPENSO;
            ((BoundColumn)gridData.Columns[3]).HeaderText = COLPERCCOMPENSO;

            ((BoundColumn)gridData.Columns[4]).DataField = COLDIFFERITA;
            ((BoundColumn)gridData.Columns[4]).HeaderText = "Fatt.Diff.";


            objPagining.CaricaPagining(phPagine, gridData);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            lblError.Text = "";
            lblError.Visible = false;
            if (!IsPostBack)
            {
                ViewState["INMODIFICA"] = "NO";
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
                        ViewState["INMODIFICA"] = "SI";
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        gridData.SelectedIndex = e.Item.ItemIndex;
                        txtElemento.Text = gridData.Items[e.Item.ItemIndex].Cells[2].Text.Replace("&nbsp;", "").Trim();
                        txtPosizione.Text = gridData.Items[e.Item.ItemIndex].Cells[1].Text.Replace("&nbsp;", "").Trim();
                        txtPercCompenso.Text = gridData.Items[e.Item.ItemIndex].Cells[3].Text.Replace("&nbsp;", "").Trim();
                        chkDifferita.Checked = bool.Parse(gridData.Items[e.Item.ItemIndex].Cells[4].Text);

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
            txtPercCompenso.Text = "";
            chkDifferita.Checked = false;

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
            string Compenso = txtPercCompenso.Text.Trim();

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
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COMPENSO", Compenso, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@FatturaRifferita", chkDifferita.Checked, SqlDbType.Bit, ParameterDirection.Input));


                            sql = "INSERT INTO " + TABLENAME + "(" + COLUMNDESCR + "," + COLUMNORDER + "," + COLPERCCOMPENSO + ",FatturaRifferita) "
                            + " VALUES(@ELEMENTO,@POSIZIONE,@COMPENSO,@FatturaRifferita) ";
							clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                            
                            ModificaVisible = false;
                            gridData.SelectedIndex = -1;
                            CaricaDati();
                        }
                        else
                        {
                            ShowError = true;
                        }
                        break;
                    }
            }

            if(ViewState["INMODIFICA"].ToString() == "SI")
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

                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COMPENSO", Compenso, SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@FatturaRifferita", chkDifferita.Checked, SqlDbType.Bit, ParameterDirection.Input));


                    sql = "UPDATE " + TABLENAME + " "
                    + " SET " + COLUMNDESCR + " = @ELEMENTO "
                    + "," + COLUMNORDER + " = @POSIZIONE "
                    + "," + COLPERCCOMPENSO + " = @COMPENSO "
                    + ",FatturaRifferita=@FatturaRifferita "
                    + " WHERE " + COLUMNID + "=@COLUMNID";
                    clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                    ViewState["INMODIFICA"] = "NO";
                    ModificaVisible = false;
                    gridData.SelectedIndex = -1;
                    CaricaDati();
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