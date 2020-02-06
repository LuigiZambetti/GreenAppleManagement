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
    public partial class Green_Admin_Report : Green_BaseUserControl
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

            COL_ELIMINA = 9;

            gridData.DataKeyField = "IDReport";

            objPagining.CaricaPagining(phPagine, gridData);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);

            lnkTest.Click += new EventHandler(lnkTest_Click);
            lblError.Text = "";
            lblError.Visible = false;
            if (!IsPostBack)
            {
                ModificaVisible = false;
                CaricaDati();
            }
            gridTest.DataSource = null;
            gridTest.DataBind();
        }

        void lnkTest_Click(object sender, EventArgs e)
        {
            DataTable DTResult = new DataTable();
            string Err = clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, txtQuery.Text.Replace("@@DATA@@",""), "RESULT", ref DTResult, true);

            if (Err == "")
            {
                if (DTResult != null)
                {
                    if (DTResult.Rows.Count >= 0)
                    {
                        gridTest.DataSource = DTResult;
                        gridTest.DataBind();
                        lblTest.Text = "";
                    }
                    else
                    {
                        lblTest.Text = "Query validata ma tabella vuota";
                        gridTest.DataSource = null;
                        gridTest.DataBind();
                    }
                }
                else
                {
                    lblTest.Text = "Presunta query non valida";
                    gridTest.DataSource = null;
                    gridTest.DataBind();
                }
                
            }
            else
            {
                gridTest.DataSource = null;
                gridTest.DataBind();
                lblTest.Text = Err;
            }
        }

        private void Save_Link_Load_Collegati(string ID)
        {
            sql = "DELETE FROM Admin_ReportGruppi WHERE IDREPORT=" + ID;
            clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

            for (int i = 0; i < chkCollegati.Items.Count; i++)
            {
                if (chkCollegati.Items[i].Selected == true)
                {
                    sql = "INSERT INTO Admin_ReportGruppi (IDGRUPPO,IDREPORT) VALUES(" + chkCollegati.Items[i].Value + "," + ID + ") ";
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

                sql = "select * from Admin_ReportGruppi where IdReport = " + ID + " and IdGruppo=" + DTVoci.Rows[i]["IdGruppo"].ToString() + " ";
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
                        txtNome.Text = gridData.Items[e.Item.ItemIndex].Cells[2].Text.Replace("&nbsp;", "").Trim();
                        txtDescrizione.Text = gridData.Items[e.Item.ItemIndex].Cells[3].Text.Replace("&nbsp;", "").Trim();
                        txtQuery.Text = gridData.Items[e.Item.ItemIndex].Cells[4].Text.Replace("&nbsp;", "").Trim();
                        txtColumnData.Text = gridData.Items[e.Item.ItemIndex].Cells[5].Text.Replace("&nbsp;", "").Trim();
                        txtRighe.Text = gridData.Items[e.Item.ItemIndex].Cells[6].Text.Replace("&nbsp;", "").Trim();

                        bool EXPORTF = false;
                        try
                        {
                            EXPORTF = Convert.ToBoolean(gridData.Items[e.Item.ItemIndex].Cells[7].Text.Replace("&nbsp;", "").Trim());
                        }
                        catch { }

                        chkExport.Checked = EXPORTF;
                        Link_Load_Collegati(gridData.DataKeys[e.Item.ItemIndex].ToString());

                        ModificaVisible = true;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtNome, this.Page);
                        break;
                    }
                case "ELIMINA":
                    {
                        clsParameter pParameter = new clsParameter();

                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));

                        sql = "DELETE FROM ADMIN_REPORT WHERE IDREPORT=@ID";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                        CaricaDati();
                        ModificaVisible = false;
                        break;
                    }
                case "DOWN":
                    {
                        clsParameter pParameter = new clsParameter();
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));
                        
                        sql = "UPDATE ADMIN_REPORT SET POSIZIONE = POSIZIONE+1 WHERE IDREPORT=@COLUMNID ";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "UPDATE ADMIN_REPORT SET POSIZIONE = POSIZIONE - 1 ";
                        sql += "WHERE  POSIZIONE IN (SELECT POSIZIONE FROM ADMIN_REPORT WHERE IDREPORT = @COLUMNID) AND IDREPORT <> @COLUMNID ";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        CaricaDati();
                        ModificaVisible = false;
                        break;
                    }
                case "UP":
                    {
                        clsParameter pParameter = new clsParameter();
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));

                        sql = "UPDATE ADMIN_REPORT SET POSIZIONE = POSIZIONE-1 WHERE IDREPORT=@COLUMNID ";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                        sql = "UPDATE ADMIN_REPORT SET POSIZIONE = POSIZIONE + 1 ";
                        sql += "WHERE  POSIZIONE IN (SELECT POSIZIONE FROM ADMIN_REPORT WHERE IDREPORT = @COLUMNID) AND IDREPORT <> @COLUMNID ";
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
            txtQuery.Text = "";
            txtColumnData.Text = "";
            txtRighe.Text = "";

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
            string Query = txtQuery.Text.Trim();
            string myDataColumn = txtColumnData.Text;
            string Righe = txtRighe.Text;

            if (Righe == "") Righe = "100";

            switch (((clsSession)Session["GreenApple"]).AzioneCorrente)
            {
                case clsSession.AzioniPagina.Inserimento:
                    {
                        sql = "SELECT * FROM ADMIN_REPORT WHERE NOME = '" + Nome + "' ";
                        DataTable DTCheck = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                        if (DTCheck.Rows.Count == 0)
                        {
                            clsParameter pParameter = new clsParameter();
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NOME", Nome, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DESCRIZIONE", Descrizione, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@QUERY", Query, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DATACOLUMN", myDataColumn, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@RowPerPage", Righe, SqlDbType.Int, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ExportFile", chkExport.Checked, SqlDbType.Bit, ParameterDirection.Input));

                            sql = "INSERT INTO ADMIN_REPORT (NOME,DESCRIZIONE,QUERY,DATACOLUMN,RowPerPage,ExportFile) "
                            + " VALUES(@NOME,@DESCRIZIONE,@QUERY,@DATACOLUMN,@RowPerPage,@ExportFile) ";
                            clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                            sql = "UPDATE ADMIN_REPORT "
                            + " SET POSIZIONE = (SELECT ISNULL(MAX(POSIZIONE)+1,1) FROM ADMIN_REPORT) WHERE NOME = '" + Nome + "' ";
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

                        sql = "SELECT * FROM ADMIN_REPORT WHERE NOME = '" + Nome + "' ";
                        sql += " AND IDREPORT <> " + gridData.DataKeys[gridData.SelectedIndex];
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
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@QUERY", Query, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@DATACOLUMN", myDataColumn, SqlDbType.NVarChar, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@RowPerPage", Righe, SqlDbType.Int, ParameterDirection.Input));
                            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ExportFile", chkExport.Checked, SqlDbType.Bit, ParameterDirection.Input));

                            sql = "UPDATE ADMIN_REPORT "
                            + " SET NOME = @NOME "
                            + ",DESCRIZIONE = @DESCRIZIONE "
                            + ",QUERY = @QUERY "
                            + ",DATACOLUMN = @DATACOLUMN "
                            + ",RowPerPage = @RowPerPage "
                            + ",ExportFile = @ExportFile "
                            + " WHERE IDREPORT = @ID";
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
                lblError.Text = "Esite già un Report con lo stesso nome.";
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
            sql = "SELECT * FROM ADMIN_REPORT ";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " WHERE (NOME like '%" + txtFiltroLibero.Text.Trim() + "%' ";
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
                ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare il Report selezionato/a ?')){}else{return false}");
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

        protected void lnkTest_Click1(object sender, EventArgs e)
        {

        }
}
}