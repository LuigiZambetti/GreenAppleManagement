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
    public partial class Green_Admin_Utenti : Green_BaseUserControl
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

            gridData.DataKeyField = "IDUtente";

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

        private void Save_Link_Load_Collegati(string ID)
        {
            sql = "DELETE FROM ADMIN_UTENTIGRUPPI WHERE IDUTENTE=" + ID;
            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

            for (int i = 0; i < chkCollegati.Items.Count; i++)
            {
                if (chkCollegati.Items[i].Selected == true)
                {
                    sql = "INSERT INTO ADMIN_UTENTIGRUPPI (IDGRUPPO,IDUTENTE) VALUES(" + chkCollegati.Items[i].Value + "," + ID + ") ";
                    clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                }
            }
        }

        private void Link_Load_Collegati(string ID)
        {
            chkCollegati.Enabled = true;

            chkCollegati.Items.Clear();

            sql = "select IdGruppo, Gruppo from admin_Gruppi order by Gruppo ";
            DataTable DTVoci = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "VOCI", ref DTVoci, true);
            for (int i = 0; i < DTVoci.Rows.Count; i++)
            {
                ListItem myitem = new ListItem();
                myitem.Value = DTVoci.Rows[i]["IdGruppo"].ToString();
                myitem.Text = DTVoci.Rows[i]["Gruppo"].ToString();

                sql = "select * from Admin_UtentiGruppi where IdUtente = " + ID + " and IdGruppo=" + DTVoci.Rows[i]["IdGruppo"].ToString() + " ";
                DataTable DTCheck = new DataTable();
                clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECK", ref DTCheck, true);

                if (DTCheck.Rows.Count > 0)
                {
                    myitem.Selected = true;
                }

                chkCollegati.Items.Add(myitem);
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
                        txtAccount.Text = gridData.Items[e.Item.ItemIndex].Cells[1].Text.Replace("&nbsp;", "").Trim();
                        txtNome.Text = gridData.Items[e.Item.ItemIndex].Cells[2].Text.Replace("&nbsp;", "").Trim();
                        txtCognome.Text = gridData.Items[e.Item.ItemIndex].Cells[3].Text.Replace("&nbsp;", "").Trim();

                        txtLogin.Text = gridData.Items[e.Item.ItemIndex].Cells[4].Text.Replace("&nbsp;", "").Trim();
                        txtPassword.Text = gridData.Items[e.Item.ItemIndex].Cells[5].Text.Replace("&nbsp;", "").Trim();

                        for (int i = 0; i <= cboColore.Items.Count - 1; i++)
                        {
                            if (cboColore.Items[i].Value == gridData.Items[e.Item.ItemIndex].Cells[6].Text)
                            {
                                cboColore.SelectedIndex = i;
                                break;
                            }
                        }

                        ModificaVisible = true;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtAccount, this.Page);

                        Link_Load_Collegati(gridData.DataKeys[e.Item.ItemIndex].ToString());

                        break;
                    }
                case "ELIMINA":
                    {
                        clsParameter pParameter = new clsParameter();

                       
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));

                        sql = "DELETE FROM ADMIN_UTENTI WHERE IDUTENTE=@ID";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "DELETE FROM Admin_UtentiGruppi WHERE IDUTENTE=@ID";
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
            txtAccount.Text = "";
            txtNome.Text = "";
            txtCognome.Text = "";
            txtLogin.Text = "";
            txtPassword.Text = "";

            chkCollegati.Enabled = false;

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtAccount, this.Page);
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
            string Account = txtAccount.Text.Trim();
            string Nome = txtNome.Text.Trim();
            string Cognome = txtCognome.Text.Trim();
            string Login = txtLogin.Text.Trim();
            string Password = txtPassword.Text.Trim();

            switch (((clsSession)Session["GreenApple"]).AzioneCorrente)
            {
                case clsSession.AzioniPagina.Inserimento:
                    {
                        if (Account != "")
                        {
                            sql = "SELECT * FROM ADMIN_UTENTI WHERE ACCOUNT = '" + Account + "' ";
                        }
                        else
                        {
                            sql = "SELECT * FROM ADMIN_UTENTI WHERE LOGIN = '" + Login + "' ";
                        }
                        DataTable DTCheck = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                        if (DTCheck.Rows.Count == 0)
                        {
                            clsParameter pParameter = new clsParameter();
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ACCOUNT", Account, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NOME", Nome, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COGNOME", Cognome, SqlDbType.NVarChar, ParameterDirection.Input));

                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@LOGIN", Login, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PASSWORD", Password, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLORE", cboColore.SelectedValue, SqlDbType.Int, ParameterDirection.Input));
                            
                            sql = "INSERT INTO ADMIN_UTENTI (ACCOUNT,NOME,COGNOME,LOGIN,PASSWORD,COLORE) "
                            + " VALUES(@ACCOUNT,@NOME,@COGNOME,@LOGIN,@PASSWORD,@COLORE) ";
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

                        if (Account != "")
                        {
                            sql = "SELECT * FROM ADMIN_UTENTI WHERE ACCOUNT = '" + Account + "' ";
                            sql += " AND IDUTENTE <> " + gridData.DataKeys[gridData.SelectedIndex];
                        }
                        else
                        {
                            sql = "SELECT * FROM ADMIN_UTENTI WHERE LOGIN = '" + Login + "' ";
                            sql += " AND IDUTENTE <> " + gridData.DataKeys[gridData.SelectedIndex];
                        }
                        
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
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ACCOUNT", Account, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NOME", Nome, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COGNOME", Cognome, SqlDbType.NVarChar, ParameterDirection.Input));

                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@LOGIN", Login, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@PASSWORD", Password, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLORE", cboColore.SelectedValue, SqlDbType.Int, ParameterDirection.Input));
                            

                            sql = "UPDATE ADMIN_UTENTI "
                            + " SET ACCOUNT = @ACCOUNT "
                            + ",NOME = @NOME "
                            + ",COGNOME = @COGNOME "
                            + ",LOGIN = @LOGIN "
                            + ",PASSWORD = @PASSWORD "
                            + ",COLORE = @COLORE "
                            + " WHERE IDUTENTE = @ID";
                            clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                            Save_Link_Load_Collegati(gridData.DataKeys[gridData.SelectedIndex].ToString());

                            CaricaDati();
                            ModificaVisible = false;
                            gridData.SelectedIndex = -1;
                        }
                        break;
                    }
            }
            if (ShowError)
            {
                lblError.Text = "Esite già un Utente con lo stesso account.";
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
            ListItem myitem = new ListItem();
            myitem.Value = "0";
            myitem.Text = "-- Selezionare un colore --";
            cboColore.Items.Add(myitem);

            sql = "select * from Admin_Colori order by colore ";
            DataTable DTColori = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "COLORI", ref DTColori, true);
            for (int i = 0; i < DTColori.Rows.Count; i++)
            {
                myitem = new ListItem();
                myitem.Value = DTColori.Rows[i]["ID"].ToString();
                myitem.Text = DTColori.Rows[i]["Colore"].ToString();
                cboColore.Items.Add(myitem);
            }

            sql = "SELECT * FROM ADMIN_UTENTI ";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " WHERE (ACCOUNT like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR NOME like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR COGNOME like '%" + txtFiltroLibero.Text.Trim() + "%' )";
            }

            sql += " ORDER BY NOME,COGNOME ";
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
                ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare la persona selezionato/a ?')){}else{return false}");
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