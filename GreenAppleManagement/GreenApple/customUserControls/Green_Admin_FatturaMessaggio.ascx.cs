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
    public partial class Green_Admin_FatturaMessaggio : Green_BaseUserControl
    {

        #region DECLARATIONS

        protected string NOMEOGGETTO = "Nome Oggetto";
        protected string CODICEOGGETTO = "Codice Oggetto";
        protected string MEX_NOMEOGGETTO = "l'oggetto";
        protected string TABLENAME = "TABELLA";
        protected string COLUMNID = "ID";
        protected string COLUMNMESSAGE = "Messaggio";
        protected string COLUMNIVA = "IVA";
        protected string COLUMNNAZIONE = "NAZIONE";
        //protected string COLUMNORDER = "POSIZIONE";
        protected int COL_MODIFICA = 3;
        protected int COL_ELIMINA = 4;
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

            COL_MODIFICA = 4;
            COL_ELIMINA = 5;

            TABLENAME = "FattureAlert";
            CheckUnivocita = true;
            CheckOrder = true;
            NvarcharID = true;


            NOMEOGGETTO = "Messaggi";
            CODICEOGGETTO = "Codice";
            MEX_NOMEOGGETTO = "il Messaggio";
            COLUMNID = "Id";
            COLUMNMESSAGE = "Messaggio";


            gridData.DataKeyField = COLUMNID;

            ((BoundColumn)gridData.Columns[0]).DataField = COLUMNID;
            ((BoundColumn)gridData.Columns[0]).HeaderText = "ID";

            ((BoundColumn)gridData.Columns[1]).DataField = COLUMNNAZIONE;
            ((BoundColumn)gridData.Columns[1]).HeaderText = COLUMNNAZIONE;

            ((BoundColumn)gridData.Columns[2]).DataField = COLUMNIVA;
            ((BoundColumn)gridData.Columns[2]).HeaderText = COLUMNIVA;

            ((BoundColumn)gridData.Columns[3]).DataField = COLUMNMESSAGE;
            ((BoundColumn)gridData.Columns[3]).HeaderText = COLUMNMESSAGE;

            

            


            objPagining.CaricaPagining(phPagine, gridData);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            lblError.Text = "";
            lblError.Visible = false;
            if (!IsPostBack)
            {
                if (((clsSession)Session["GreenApple"]).AzioneCorrente != clsSession.AzioniPagina.Modifica)
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

                        txtID.Text = gridData.Items[e.Item.ItemIndex].Cells[0].Text.Replace("&nbsp;", "").Trim();
                        txtNazione.Text = gridData.Items[e.Item.ItemIndex].Cells[1].Text.Replace("&nbsp;", "").Trim();
                        txtIVA.Text = gridData.Items[e.Item.ItemIndex].Cells[2].Text.Replace("&nbsp;", "").Trim();
                        txtMessaggio.Text = gridData.Items[e.Item.ItemIndex].Cells[3].Text.Replace("&nbsp;", "").Trim();

                        ModificaVisible = true;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtMessaggio, this.Page);
                        break;
                    }
                case "ELIMINA":
                    {
                        clsParameter pParameter = new clsParameter();

                        if (NvarcharID == false)
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));
                        }
                        else
                        {
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.NVarChar, ParameterDirection.Input));
                        }

                        sql = "DELETE FROM " + TABLENAME + " WHERE " + COLUMNID + "=@COLUMNID";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                        CaricaDati();
                        ModificaVisible = false;
                        break;
                    }
            }
        }
        protected void lnkInserisci_Click(object sender, EventArgs e)
        {
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            gridData.SelectedIndex = -1;
            ModificaVisible = true;
            txtNazione.Text = "";
            txtIVA.Text = "";
            txtMessaggio.Text = "";

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtMessaggio, this.Page);
            this.Page.Validate();
        }
        protected void lnkAnnulla_Click(object sender, EventArgs e)
        {
            gridData.SelectedIndex = -1;
            ModificaVisible = false;
        }
        protected void lnkAggiorna_Click(object sender, EventArgs e)
        {
            bool ShowError = false;
            string ID = txtID.Text.Trim();
            string Iva = txtIVA.Text.Trim();
            string Nazione = txtNazione.Text.Trim();
            string Messaggio = txtMessaggio.Text.Trim();

            switch (((clsSession)Session["GreenApple"]).AzioneCorrente)
            {
                case clsSession.AzioniPagina.Inserimento:
                    {
                        sql = "SELECT * FROM " + TABLENAME + " WHERE " + COLUMNID + "='" + ID + "' ";
                        DataTable DTCheck = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                        if (DTCheck.Rows.Count == 0)
                        {
                            clsParameter pParameter = new clsParameter();
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@IVA", Iva, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NAZIONE", Nazione, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@MESSAGGIO", Messaggio, SqlDbType.NVarChar, ParameterDirection.Input));

                            sql = "INSERT INTO " + TABLENAME + "(" + COLUMNIVA + "," + COLUMNMESSAGE + "," + COLUMNNAZIONE + ") "
                            + " VALUES(@IVA,@MESSAGGIO,@NAZIONE) ";
                            clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                            CaricaDati();
                            ModificaVisible = false;
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


                        if (ShowError == false)
                        {
                            clsParameter pParameter = new clsParameter();
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@IVA", Iva, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NAZIONE", Nazione, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@MESSAGGIO", Messaggio, SqlDbType.NVarChar, ParameterDirection.Input));

                            if (NvarcharID == false)
                            {
                                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[gridData.SelectedIndex], SqlDbType.Int, ParameterDirection.Input));
                            }
                            else
                            {
                                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[gridData.SelectedIndex], SqlDbType.NVarChar, ParameterDirection.Input));
                            }

                            sql = "UPDATE " + TABLENAME + " "
                            + " SET " + COLUMNMESSAGE + " = @MESSAGGIO "
                            + "," + COLUMNNAZIONE + " = @NAZIONE "
                            + "," + COLUMNIVA + " = @IVA "
                            + " WHERE " + COLUMNID + "=@COLUMNID";
                            clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                            CaricaDati();
                            ModificaVisible = false;
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
                sql += " WHERE " + COLUMNMESSAGE + " like '%" + txtFiltroLibero.Text.Trim() + "%'";
            }

            sql += " ORDER BY " + COLUMNID;
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