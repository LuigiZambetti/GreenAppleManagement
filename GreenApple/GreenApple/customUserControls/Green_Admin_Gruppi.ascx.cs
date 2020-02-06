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
    public partial class Green_Admin_Gruppi : Green_BaseUserControl
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

            COL_ELIMINA = 3;

            gridData.DataKeyField = "IDGruppo";

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

        private void Save_Link_Load_Collegati(string ID)
        {
            sql = "DELETE FROM ADMIN_GRUPPIMENU WHERE IDGRUPPO=" + ID;
            clsDB.Execute_Command(this.Page,((clsSession)Session["GreenApple"]).CnnStr, sql);

            for (int i = 0; i < chkCollegati.Items.Count; i++)
            {
                if (chkCollegati.Items[i].Selected == true)
                {
                    sql = "INSERT INTO ADMIN_GRUPPIMENU (IDMENU,IDGRUPPO) VALUES(" + chkCollegati.Items[i].Value + "," + ID + ") " ;
                    clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                }
            }
        }

        private void Link_Load_Collegati(string ID)
        {
            chkCollegati.Enabled = true;

            chkCollegati.Items.Clear();

            sql = "select IdMenu, Gruppo + '  -  ' + Voce as Voce from admin_menu order by voce ";
            DataTable DTVoci = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "VOCI", ref DTVoci, true);
            for (int i = 0; i < DTVoci.Rows.Count; i++)
            {
                ListItem myitem = new ListItem();
                myitem.Value = DTVoci.Rows[i]["IdMenu"].ToString();
                myitem.Text = DTVoci.Rows[i]["Voce"].ToString();

                sql = "select * from Admin_GruppiMenu where idgruppo = " + ID + " and idmenu=" + DTVoci.Rows[i]["IdMenu"].ToString() + " ";
                DataTable DTCheck = new DataTable();
                clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECK", ref DTCheck, true);

                if (DTCheck.Rows.Count > 0)
                {
                    myitem.Selected = true;
                }

                chkCollegati.Items.Add(myitem);
            }

        }

        protected void gridData_ItemCommand(object source, DataGridCommandEventArgs e)
        {
            switch (e.CommandName.ToUpper())
            {
                case "MODIFICA":
                    {
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        gridData.SelectedIndex = e.Item.ItemIndex;
                        txtGruppo.Text = gridData.Items[e.Item.ItemIndex].Cells[1].Text.Replace("&nbsp;", "").Trim();
                        ModificaVisible = true;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtGruppo, this.Page);

                        Link_Load_Collegati(gridData.DataKeys[e.Item.ItemIndex].ToString());
                        
                        break;
                    }
                case "ELIMINA":
                    {
                        clsParameter pParameter = new clsParameter();

                       
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));

                        sql = "DELETE FROM ADMIN_GRUPPI WHERE IDGRUPPO=@ID";
						clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "DELETE FROM Admin_UtentiGruppi WHERE IDGRUPPO=@ID";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "DELETE FROM Admin_GruppiMenu WHERE IDGRUPPO=@ID";
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
            txtGruppo.Text = "";
            chkCollegati.Enabled = false;

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtGruppo, this.Page);
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
            string Gruppo = txtGruppo.Text.Trim();
            
            switch (((clsSession)Session["GreenApple"]).AzioneCorrente)
            {
                case clsSession.AzioniPagina.Inserimento:
                    {
                        sql = "SELECT * FROM ADMIN_GRUPPI WHERE GRUPPO = '" + Gruppo + "' ";
                        DataTable DTCheck = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                        if (DTCheck.Rows.Count == 0)
                        {
                            clsParameter pParameter = new clsParameter();
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GRUPPO", Gruppo, SqlDbType.NVarChar, ParameterDirection.Input));

                            sql = "INSERT INTO ADMIN_GRUPPI (GRUPPO) "
                            + " VALUES(@GRUPPO) ";
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

                        sql = "SELECT * FROM ADMIN_GRUPPI WHERE GRUPPO ='" + Gruppo + "' ";
                        sql += " AND IDGRUPPO <> " + gridData.DataKeys[gridData.SelectedIndex];
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
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GRUPPO", Gruppo, SqlDbType.NVarChar, ParameterDirection.Input));

                            sql = "UPDATE ADMIN_GRUPPI "
                            + " SET GRUPPO = @GRUPPO "
                            + " WHERE IDGRUPPO = @ID";
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
                lblError.Text = "Esite già un Gruppo con lo stesso account.";
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
            sql = "SELECT * FROM ADMIN_GRUPPI ";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " WHERE (GRUPPO like '%" + txtFiltroLibero.Text.Trim() + "%')";
            }

            sql += " ORDER BY GRUPPO ";
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
                ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare il gruppo selezionato/a ?')){}else{return false}");
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