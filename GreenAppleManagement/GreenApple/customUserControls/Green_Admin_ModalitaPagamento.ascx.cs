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
    public partial class Green_Admin_ModalitaPagamento : Green_BaseUserControl
    {

        #region DECLARATIONS

        private string sql = "";
        clsPagining objPagining = new clsPagining();
        protected int COL_ELIMINA = 0;
        protected string sqlControl = "";

        #endregion

        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            COL_ELIMINA = 10;

            gridData.DataKeyField = "IDModalita";
            sqlControl = "select * from documclienti where DCPagam = '@CODE'";

            objPagining.CaricaPagining(phPagine, gridData);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            lblError.Text = "";
            lblError.Visible = false;
            if (!IsPostBack)
            {
                if (((clsSession)Session["GreenApple"]).AzioneCorrente == clsSession.AzioniPagina.Dettaglio && ModificaVisible == true)
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
                        
                        txtCodice.Text = gridData.Items[e.Item.ItemIndex].Cells[1].Text.Replace("&nbsp;", "").Trim();
                        txtDescrizione.Text = gridData.Items[e.Item.ItemIndex].Cells[2].Text.Replace("&nbsp;", "").Trim();
                        txtNumRate.Text = gridData.Items[e.Item.ItemIndex].Cells[3].Text.Replace("&nbsp;", "").Trim();
                        txtGiorni.Text = gridData.Items[e.Item.ItemIndex].Cells[4].Text.Replace("&nbsp;", "").Trim();

                        chkFM.Checked=Convert.ToBoolean(gridData.Items[e.Item.ItemIndex].Cells[5].Text);
                        chkRIBA.Checked=Convert.ToBoolean(gridData.Items[e.Item.ItemIndex].Cells[6].Text);
                        chkBB.Checked=Convert.ToBoolean(gridData.Items[e.Item.ItemIndex].Cells[7].Text);
                        chkRD.Checked = Convert.ToBoolean(gridData.Items[e.Item.ItemIndex].Cells[8].Text);

                        ModificaVisible = true;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtDescrizione, this.Page);

                   
                        break;
                    }
                case "ELIMINA":
                    {
                        clsParameter pParameter = new clsParameter();
                       
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));

                        sql = "DELETE FROM ModalitaPagamento WHERE IDModalita=@ID";
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
            txtCodice.Text = "";
            txtDescrizione.Text = "";
            txtNumRate.Text = "";
            txtGiorni.Text = "";

            chkFM.Checked = false;
            chkRIBA.Checked = false;
            chkBB.Checked = false;
            chkRD.Checked = false;

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtDescrizione, this.Page);
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
         
            string Codice = txtCodice.Text.Trim();
            string Descrizione = txtDescrizione.Text.Trim();
            string NumRate = txtNumRate.Text.Trim();
            string Giorni = txtGiorni.Text.Trim();
            bool FM = chkFM.Checked ;
            bool RIBA = chkRIBA.Checked; 
            bool BB = chkBB.Checked ;
            bool RD = chkRD.Checked;

            switch (((clsSession)Session["GreenApple"]).AzioneCorrente)
            {
                case clsSession.AzioniPagina.Inserimento:
                    {
                        sql = "SELECT * FROM ModalitaPagamento WHERE CODICE = '" + Codice + "' ";
                        DataTable DTCheck = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                        if (DTCheck.Rows.Count == 0)
                        {
                            clsParameter pParameter = new clsParameter();
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CODICE", Codice, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DESCRIZIONE", Descrizione, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NUMRATE", NumRate, SqlDbType.SmallInt, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GIORNI", Giorni, SqlDbType.SmallInt, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@FM", FM, SqlDbType.Bit, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@RIBA", RIBA, SqlDbType.Bit, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@BB", BB, SqlDbType.Bit, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@RD", RD, SqlDbType.Bit, ParameterDirection.Input));

                            sql = "INSERT INTO ModalitaPagamento (CODICE,DESCRIZIONE,NUMRATE,GIORNI,FM,RIBA,BB,RD) "
                            + " VALUES(@CODICE,@DESCRIZIONE,@NUMRATE,@GIORNI,@FM,@RIBA,@BB,@RD) ";
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

                        sql = "SELECT * FROM ModalitaPagamento WHERE CODICE = '" + Codice + "' ";
                        sql += " AND IDModalita <> " + gridData.DataKeys[gridData.SelectedIndex];
                        DataTable DTCheck = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                        if (DTCheck.Rows.Count > 0)
                        {
                            ShowError = true;
                        }
                   
                        if (ShowError == false)
                        {
                            clsParameter pParameter = new clsParameter();
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", gridData.DataKeys[gridData.SelectedIndex], SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CODICE", Codice, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DESCRIZIONE", Descrizione, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NUMRATE", NumRate, SqlDbType.SmallInt, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GIORNI", Giorni, SqlDbType.SmallInt, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@FM", FM, SqlDbType.Bit, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@RIBA", RIBA, SqlDbType.Bit, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@BB", BB, SqlDbType.Bit, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@RD", RD, SqlDbType.Bit, ParameterDirection.Input));


                            sql = "UPDATE ModalitaPagamento "
                            + " SET CODICE = @CODICE "
                            + ",DESCRIZIONE = @DESCRIZIONE "
                            + ",NUMRATE = @NUMRATE "
                            + ",GIORNI = @GIORNI "
                            + ",FM = @FM "
                            + ",RIBA = @RIBA "
                            + ",BB = @BB "
                            + ",RD = @RD "
                            + " WHERE IDMODALITA = @ID";
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
                lblError.Text = "Esite già una Modalità di Pagamento con lo stesso Codice.";
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
            sql = "SELECT * FROM ModalitaPagamento ";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " WHERE (CODICE like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR DESCRIZIONE like '%" + txtFiltroLibero.Text.Trim() + "%' )";
            }

            sql += " ORDER BY CODICE ";
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
                ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare la modalità selezionato/a ?')){}else{return false}");

                if (sqlControl != "")
                {
                    string sqlControlCODE = sqlControl.Replace("@CODE", DTResult.Rows[i]["CODICE"].ToString());
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