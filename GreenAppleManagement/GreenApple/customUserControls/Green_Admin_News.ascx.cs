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
    public partial class Green_Admin_News : Green_BaseUserControl
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

            COL_ELIMINA = 7;

            gridData.DataKeyField = "IDNews";

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
                        txtNome.Text = gridData.Items[e.Item.ItemIndex].Cells[3].Text.Replace("&nbsp;", "").Trim();
                        txtDescrizione.Text = gridData.Items[e.Item.ItemIndex].Cells[4].Text.Replace("&nbsp;", "").Trim();

                        if (gridData.Items[e.Item.ItemIndex].Cells[1].Text.Contains("icoConfirm"))
                        {
                            //NEWS APERTA
                            chkChiusa.Checked = false;
                        }
                        else
                        { 
                            //NEWS CHIUSA
                            chkChiusa.Checked = true;
                        }

                        ModificaVisible = true;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtNome, this.Page);
                        break;
                    }
                case "ELIMINA":
                    {
                        clsParameter pParameter = new clsParameter();

                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));

                        sql = "DELETE FROM ADMIN_NEWS WHERE IDNEWS=@ID";
						clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                        CaricaDati();
                        ModificaVisible = false;
                        break;
                    }
                case "DOWN":
                    {
                        clsParameter pParameter = new clsParameter();
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));

                        sql = "UPDATE ADMIN_NEWS SET POSIZIONE = POSIZIONE + 1 WHERE IDNEWS = @COLUMNID ";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "UPDATE ADMIN_NEWS SET POSIZIONE = POSIZIONE - 1 ";
                        sql += "WHERE POSIZIONE IN (SELECT POSIZIONE FROM ADMIN_NEWS WHERE IDNEWS = @COLUMNID) AND IDNEWS <> @COLUMNID ";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        CaricaDati();
                        ModificaVisible = false;
                        break;
                    }
                case "UP":
                    {
                        clsParameter pParameter = new clsParameter();
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));

                        sql = "UPDATE ADMIN_NEWS SET POSIZIONE = POSIZIONE - 1 WHERE IDNEWS = @COLUMNID ";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "UPDATE ADMIN_NEWS SET POSIZIONE = POSIZIONE + 1 ";
                        sql += "WHERE POSIZIONE IN (SELECT POSIZIONE FROM ADMIN_NEWS WHERE IDNEWS = @COLUMNID) AND IDNEWS <> @COLUMNID ";
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
            
            txtNome.Text = "";
            txtDescrizione.Text = "";
            chkChiusa.Checked = false;

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtNome, this.Page);
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

            string Nome = txtNome.Text.Trim();
            string Descrizione = txtDescrizione.Text.Trim();
            bool Chiusa = chkChiusa.Checked;

            switch (((clsSession)Session["GreenApple"]).AzioneCorrente)
            {
                case clsSession.AzioniPagina.Inserimento:
                    {
                        sql = "SELECT * FROM ADMIN_NEWS WHERE TITOLO = '" + Nome + "' ";
                        DataTable DTCheck = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                        if (DTCheck.Rows.Count == 0)
                        {
                            clsParameter pParameter = new clsParameter();
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NOME", Nome, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DESCRIZIONE", Descrizione, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DATA", DateTime.Now, SqlDbType.DateTime, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CHIUSA", Chiusa, SqlDbType.Bit, ParameterDirection.Input));

                            sql = "INSERT INTO ADMIN_NEWS (TITOLO,DESCRIZIONE,DATA,CHIUSA) "
                            + " VALUES(@NOME,@DESCRIZIONE,@DATA,@CHIUSA) ";
							clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                            sql = "UPDATE ADMIN_NEWS "
                            + " SET POSIZIONE = (SELECT ISNULL(MAX(POSIZIONE)+1,1) FROM ADMIN_NEWS) WHERE TITOLO = '" + Nome + "' ";
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

                        sql = "SELECT * FROM ADMIN_NEWS WHERE TITOLO = '" + Nome + "' ";
                        sql += " AND IDNEWS <> " + gridData.DataKeys[gridData.SelectedIndex];
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
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NOME", Nome, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DESCRIZIONE", Descrizione, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DATA", DateTime.Now, SqlDbType.DateTime, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CHIUSA", Chiusa, SqlDbType.Bit, ParameterDirection.Input));

                            sql = "UPDATE ADMIN_NEWS "
                            + " SET TITOLO = @NOME "
                            + ",DESCRIZIONE = @DESCRIZIONE "
                            + ",DATA = @DATA "
                            + ",CHIUSA = @CHIUSA "
                            + " WHERE IDNEWS = @ID";
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
                lblError.Text = "Esite già una News con lo stesso titolo.";
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
            sql = "SELECT *,CASE WHEN ISNULL(Chiusa,0)=0 THEN '<img src=''../customLayout/images/icoConfirm.gif''>' ELSE '<img src=''../customLayout/images/icoClose.gif''>' END AS NewsChiusa FROM ADMIN_NEWS ";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " WHERE (TITOLO like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR DESCRIZIONE like '%" + txtFiltroLibero.Text.Trim() + "%' )";
            }

            sql += " ORDER BY POSIZIONE ";
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
                ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare la News selezionato/a ?')){}else{return false}");
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