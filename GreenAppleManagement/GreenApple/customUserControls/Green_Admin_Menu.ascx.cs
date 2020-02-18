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
    public partial class Green_Admin_Menu : Green_BaseUserControl
    {

        #region DECLARATIONS

        private string sql = "";
        clsPagining objPagining = new clsPagining();
        protected int COL_ELIMINA = 0;

        #endregion

        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            COL_ELIMINA = 8;

            gridData.DataKeyField = "IDMenu";

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
                        txtVoce.Text = gridData.Items[e.Item.ItemIndex].Cells[2].Text.Replace("&nbsp;", "").Trim();
                        txtGruppo.Text = gridData.Items[e.Item.ItemIndex].Cells[3].Text.Replace("&nbsp;", "").Trim();
                        txtSimpleList.Checked = bool.Parse(gridData.Items[e.Item.ItemIndex].Cells[4].Text);
                        txtTabella.Text = gridData.Items[e.Item.ItemIndex].Cells[5].Text.Replace("&nbsp;", "").Trim();
                        txtPagina.Text = gridData.Items[e.Item.ItemIndex].Cells[6].Text.Replace("&nbsp;", "").Trim();
                       
                        ModificaVisible = true;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtVoce, this.Page);
                        break;
                    }
                case "ELIMINA":
                    {
                        clsParameter pParameter = new clsParameter();

                       
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));

                        sql = "DELETE FROM ADMIN_MENU WHERE IDMENU=@ID";
						clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                        CaricaDati();
                        ModificaVisible = false;
                        break;
                    }
                case "DOWN":
                    {
                        clsParameter pParameter = new clsParameter();
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GRUPPO", e.Item.Cells[3].Text, SqlDbType.NVarChar, ParameterDirection.Input));

                        sql = "UPDATE ADMIN_MENU SET POSIZIONE = POSIZIONE+1 WHERE IDMENU=@COLUMNID AND GRUPPO=@GRUPPO";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "UPDATE ADMIN_MENU SET POSIZIONE = POSIZIONE - 1 ";
                        sql += "WHERE  GRUPPO=@GRUPPO AND POSIZIONE IN (SELECT POSIZIONE FROM ADMIN_MENU WHERE IDMENU = @COLUMNID) AND IDMENU <> @COLUMNID ";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        CaricaDati();
                        ModificaVisible = false;
                        break;
                    }
                case "UP":
                    {
                        clsParameter pParameter = new clsParameter();
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GRUPPO", e.Item.Cells[3].Text, SqlDbType.NVarChar, ParameterDirection.Input));
                            
                        sql = "UPDATE ADMIN_MENU SET POSIZIONE = POSIZIONE-1 WHERE IDMENU=@COLUMNID AND GRUPPO=@GRUPPO";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "UPDATE ADMIN_MENU SET POSIZIONE = POSIZIONE + 1 ";
                        sql += "WHERE  GRUPPO=@GRUPPO AND POSIZIONE IN (SELECT POSIZIONE FROM ADMIN_MENU WHERE IDMENU = @COLUMNID) AND IDMENU <> @COLUMNID ";
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
            txtVoce.Text = "";
            txtGruppo.Text = "";
            txtSimpleList.Checked = false;
            txtTabella.Text = "";
            txtPagina.Text = "";
                       

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtVoce, this.Page);
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

            string Voce = txtVoce.Text.Trim();
            string Gruppo = txtGruppo.Text.Trim();
            bool SimpleList = txtSimpleList.Checked;
            string Tabella = txtTabella.Text.Trim();
            string Pagina = txtPagina.Text.Trim();

            switch (((clsSession)Session["GreenApple"]).AzioneCorrente)
            {
                case clsSession.AzioniPagina.Inserimento:
                    {
                        sql = "SELECT * FROM ADMIN_MENU WHERE VOCE = '" + Voce + "' ";
                        DataTable DTCheck = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                        if (DTCheck.Rows.Count == 0)
                        {
                            clsParameter pParameter = new clsParameter();
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@VOCE", Voce, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GRUPPO", Gruppo, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SIMPLELIST", SimpleList, SqlDbType.Bit, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@TABELLA", Tabella, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PAGINA", Pagina, SqlDbType.NVarChar, ParameterDirection.Input));

                            sql = "INSERT INTO ADMIN_MENU (VOCE,GRUPPO,SIMPLELIST,TABELLA,PAGINA) "
                            + " VALUES(@VOCE,@GRUPPO,@SIMPLELIST,@TABELLA,@PAGINA) ";
							clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                            sql = "UPDATE ADMIN_MENU "
                            + " SET POSIZIONE = (SELECT ISNULL(MAX(POSIZIONE)+1,1) FROM ADMIN_MENU WHERE GRUPPO=@GRUPPO) WHERE VOCE = '" + Voce + "' ";
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

                        sql = "SELECT * FROM ADMIN_MENU WHERE VOCE = '" + Voce + "' ";
                        sql += " AND IDMENU <> " + gridData.DataKeys[gridData.SelectedIndex];
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
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@VOCE", Voce, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GRUPPO", Gruppo, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@SIMPLELIST", SimpleList, SqlDbType.Bit, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@TABELLA", Tabella, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PAGINA", Pagina, SqlDbType.NVarChar, ParameterDirection.Input));

                            sql = "UPDATE ADMIN_MENU "
                            + " SET VOCE = @VOCE "
                            + ",GRUPPO = @GRUPPO "
                            + ",SIMPLELIST = @SIMPLELIST "
                            + ",TABELLA = @TABELLA "
                            + ",PAGINA = @PAGINA "
                            + " WHERE IDMENU = @ID";
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
                lblError.Text = "Esite già una voce di menu con lo stesso nome.";
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
            sql = "SELECT * FROM ADMIN_MENU ";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " WHERE (VOCE like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR GRUPPO like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR PAGINA like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR TABELLA like '%" + txtFiltroLibero.Text.Trim() + "%' )";
            }

            sql += " ORDER BY GRUPPO,POSIZIONE,VOCE ";
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
                ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare la voce selezionato/a ?')){}else{return false}");
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