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
using System.Text;
using System.IO;
using System.Linq;

namespace Green.Apple.Management
{
    public partial class Green_Admin_ExportClienti : Green_BaseUserControl
    {

        #region DECLARATIONS

        protected string NOMEOGGETTO = "Nome Oggetto";
        protected string CODICEOGGETTO = "Codice Oggetto";
        protected string MEX_NOMEOGGETTO = "l'oggetto";
        protected string TABLENAME = "TABELLA";
        protected string COLUMNID = "ID";
        protected string COLUMNOME = "Nome";
        protected string COLUMRAGSOC = "Ragione Sociale";
        protected string COLUMNMODIFICATA = "Modificata";
        protected int COL_ESPORTA = 4;
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

            COL_ESPORTA = 4;

            TABLENAME = "Export_Clienti";
            CheckUnivocita = true;
            CheckOrder = true;
            NvarcharID = true;


            NOMEOGGETTO = "Anagrafica Cliente";
            CODICEOGGETTO = "Codice";
            MEX_NOMEOGGETTO = "la Anagrafica Cliente";
            COLUMNID = "ID";


            gridData.DataKeyField = COLUMNID;

            ((BoundColumn)gridData.Columns[0]).DataField = COLUMNID;
            ((BoundColumn)gridData.Columns[0]).HeaderText = "ID";

            ((BoundColumn)gridData.Columns[1]).DataField = COLUMNOME;
            ((BoundColumn)gridData.Columns[1]).HeaderText = COLUMNOME;

            ((BoundColumn)gridData.Columns[2]).DataField = COLUMRAGSOC;
            ((BoundColumn)gridData.Columns[2]).HeaderText = COLUMRAGSOC;

            ((BoundColumn)gridData.Columns[3]).DataField = COLUMNMODIFICATA;
            ((BoundColumn)gridData.Columns[3]).HeaderText = COLUMNMODIFICATA;


            objPagining.CaricaPagining(phPagine, gridData);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            //lblError.Text = "";
            //lblError.Visible = false;
            if (!IsPostBack)
            {
                //if (((clsSession)Session["GreenApple"]).AzioneCorrente != clsSession.AzioniPagina.Modifica)
                //    ModificaVisible = false;
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
            // esporta UN ELEMENTO

            switch (e.CommandName.ToUpper())
            {
                case "ESPORTA":
                    {
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        gridData.SelectedIndex = e.Item.ItemIndex;

                        string ID = gridData.Items[e.Item.ItemIndex].Cells[0].Text.Replace("&nbsp;", "").Trim();

                        sql = @"SELECT [Clicodice] IdCliente
                                      ,[Cliprenome] Nome
                                      ,[Cliragsoc] RagioneSociale
                                      ,[Cliindirizzo] Indirizzo
                                      ,[Clilocalita] Localita
                                      ,[Cliprovincia] Provincia
                                      ,[CliCAP] CAP
                                      ,[CliNazione] Nazione
                                      ,[Clitelefono] Telefono
                                      ,[CliEMAIL] Email
                                      ,[Clifax] Fax
                                      ,[CliIVA] PartitaIva
                                      ,[Clipagamento] TipoPagamento
                                      ,[CliBanca] banca
                                      ,[CliIBAN] IBAN
                                      ,[CliSWIFT] SWIFT
                                      ,[Modificata]
                                FROM [dbo].[EXPORT_Clienti] 
                                WHERE Clicodice = " + ID ;
                        sql += " ORDER BY Cliprenome";

                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        // esporto in CSV la DataTable
                        EsportaDataTableInCSV(DTResult);

                        // modifico il flag ESPORTATA
                        sql = @"UPDATE [dbo].[EXPORT_Clienti]
                                SET Esportata = 1
                                WHERE Clicodice = " + ID ;
                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        CaricaDati();

                        //txtID.Text = gridData.Items[e.Item.ItemIndex].Cells[0].Text.Replace("&nbsp;", "").Trim();
                        //txtNazione.Text = gridData.Items[e.Item.ItemIndex].Cells[1].Text.Replace("&nbsp;", "").Trim();
                        //txtIVA.Text = gridData.Items[e.Item.ItemIndex].Cells[2].Text.Replace("&nbsp;", "").Trim();
                        //txtMessaggio.Text = gridData.Items[e.Item.ItemIndex].Cells[3].Text.Replace("&nbsp;", "").Trim();

                        //ModificaVisible = true;
                        //((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        //clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtMessaggio, this.Page);
                        break;
                    }
            }
        }
        protected void lnkEsporta_Click(object sender, EventArgs e)
        {
            // esporta TUTTO
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            gridData.SelectedIndex = -1;

            sql = @"SELECT [Clicodice] AS IdCliente
                          ,[Cliprenome] AS Nome
                          ,[Cliragsoc] AS RagioneSociale
                          ,[Cliindirizzo] As Indirizzo
                          ,[Clilocalita] AS Localita
                          ,[Cliprovincia]	AS Provincia
                          ,[CliCAP] AS CAP
	                      ,[CliNazione] AS Nazione
	                      ,[Clitelefono] AS Telefono
	                      ,[CliEMAIL] AS Email
	                      ,[Clifax] AS Fax
	                      ,[CliIVA] AS PartitaIva
	                      ,[Clipagamento] AS TipoPagamento
	                      ,[CliBanca] AS Banca
                          ,[CliIBAN] AS IBAN
                          ,[CliSWIFT] AS SWIFT
                          ,[Modificata]
                      FROM [dbo].[EXPORT_Clienti]
                      WHERE Esportata = 0
                      ORDER BY Cliprenome";

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            if (DTResult.Rows.Count > 0)
            {
                // esporto in CSV
                EsportaDataTableInCSV(DTResult);

                // modifico il flag ESPORTATA
                string IDs = "";
                foreach (DataRow row in DTResult.Rows)
                {
                    IDs += ", " + row["IdCliente"].ToString() ;
                }
                IDs = IDs.Substring(2);

                sql = @"UPDATE [dbo].[EXPORT_Clienti]
                                    SET Esportata = 1
                                    WHERE Clicodice IN (" + IDs + ")";

                clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
            }
            CaricaDati();
        }

        protected void lnkVedi_Click(object sender, EventArgs e)
        {
            Response.Redirect("ExportVisualizzaCSV.aspx");
        }

        protected void lnkAnnulla_Click(object sender, EventArgs e)
        {
            gridData.SelectedIndex = -1;
            //ModificaVisible = false;
        }
        //protected void lnkAggiorna_Click(object sender, EventArgs e)
        //{
        //    bool ShowError = false;
        //    string ID = txtID.Text.Trim();
        //    string Iva = txtIVA.Text.Trim();
        //    string Nazione = txtNazione.Text.Trim();
        //    string Messaggio = txtMessaggio.Text.Trim();

        //    switch (((clsSession)Session["GreenApple"]).AzioneCorrente)
        //    {
        //        case clsSession.AzioniPagina.Inserimento:
        //            {
        //                sql = "SELECT * FROM " + TABLENAME + " WHERE " + COLUMNID + "='" + ID + "' ";
        //                DataTable DTCheck = new DataTable();
        //                clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
        //                if (DTCheck.Rows.Count == 0)
        //                {
        //                    clsParameter pParameter = new clsParameter();
        //                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@IVA", Iva, SqlDbType.NVarChar, ParameterDirection.Input));
        //                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NAZIONE", Nazione, SqlDbType.NVarChar, ParameterDirection.Input));
        //                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@MESSAGGIO", Messaggio, SqlDbType.NVarChar, ParameterDirection.Input));

        //                    sql = "INSERT INTO " + TABLENAME + "(" + COLUMNIVA + "," + COLUMNMESSAGE + "," + COLUMNNAZIONE + ") "
        //                    + " VALUES(@IVA,@MESSAGGIO,@NAZIONE) ";
        //                    clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
        //                    CaricaDati();
        //                    ModificaVisible = false;
        //                    gridData.SelectedIndex = -1;
        //                }
        //                else
        //                {
        //                    ShowError = true;
        //                }
        //                break;
        //            }
        //        case clsSession.AzioniPagina.Modifica:
        //            {
        //                ShowError = false;


        //                if (ShowError == false)
        //                {
        //                    clsParameter pParameter = new clsParameter();
        //                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@IVA", Iva, SqlDbType.NVarChar, ParameterDirection.Input));
        //                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@NAZIONE", Nazione, SqlDbType.NVarChar, ParameterDirection.Input));
        //                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@MESSAGGIO", Messaggio, SqlDbType.NVarChar, ParameterDirection.Input));

        //                    if (NvarcharID == false)
        //                    {
        //                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[gridData.SelectedIndex], SqlDbType.Int, ParameterDirection.Input));
        //                    }
        //                    else
        //                    {
        //                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLUMNID", gridData.DataKeys[gridData.SelectedIndex], SqlDbType.NVarChar, ParameterDirection.Input));
        //                    }

        //                    sql = "UPDATE " + TABLENAME + " "
        //                    + " SET " + COLUMNMESSAGE + " = @MESSAGGIO "
        //                    + "," + COLUMNNAZIONE + " = @NAZIONE "
        //                    + "," + COLUMNIVA + " = @IVA "
        //                    + " WHERE " + COLUMNID + "=@COLUMNID";
        //                    clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
        //                    CaricaDati();
        //                    ModificaVisible = false;
        //                    gridData.SelectedIndex = -1;
        //                }
        //                break;
        //            }
        //    }
        //    if (ShowError)
        //    {
        //        lblError.Text = "Esite già un " + NOMEOGGETTO + " con lo stesso nome.";
        //        lblError.Visible = true;
        //    }
        //}

        protected void lnkCerca_Click(object sender, EventArgs e)
        {
            CaricaDati();
        }
        #endregion

        #region FUNCTIONS
        private void CaricaDati()
        {

            sql = @"SELECT [Clicodice] ID
                            ,[Cliprenome] Nome
                            ,[Cliragsoc] [Ragione Sociale]
                          ,[Modificata]
                      FROM [dbo].[EXPORT_Clienti]
                      WHERE Esportata = 0";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " WHERE [Cliprenome] like '%" + txtFiltroLibero.Text.Trim() + "%'";
                sql += " OR [Cliragsoc] like '%" + txtFiltroLibero.Text.Trim() + "%'";
            }

            sql += " ORDER BY [Cliprenome]";
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
                ((LinkButton)gridData.Items[i].Cells[COL_ESPORTA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole esportare " + MEX_NOMEOGGETTO + " selezionata ?')){}else{return false}");
            }
            objPagining.CaricaPagining(phPagine, gridData);
        }

        private void EsportaDataTableInCSV(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();

            string[] columnNames = dt.Columns.Cast<DataColumn>().
                                              Select(column => column.ColumnName).
                                              ToArray();
            sb.AppendLine(string.Join(";", columnNames));

            foreach (DataRow row in dt.Rows)
            {
                string[] fields = row.ItemArray.Select(field => field.ToString()).
                                                ToArray();
                sb.AppendLine(string.Join(";", fields));
            }

            DateTime now = DateTime.Now;
            string filename = now.Year.ToString().Substring(2) + now.Month.ToString("00") + now.Day.ToString("00")
                                + now.Hour.ToString("00") + now.Minute.ToString("00") + now.Second.ToString("00")
                                + now.Millisecond.ToString("000")
                                + "_anagrafiche.csv";

            File.WriteAllText(HttpContext.Current.Server.MapPath("~\\export\\" + filename), sb.ToString());
        }
        #endregion

        #region PROPERTY
        //private bool ModificaVisible
        //{
        //    get
        //    {
        //        return TBLModifica.Visible;
        //    }
        //    set
        //    {
        //        TBLModifica.Visible = value; gridData.Enabled = !value; phPagine.Visible = !value; lnkInserisci.Enabled = !value;
        //        for (int i = 0; i <= gridData.Items.Count - 1; i++)
        //        {
        //            ((LinkButton)gridData.Items[i].Cells[COL_ESPORTA].Controls[0]).Visible = !value;
        //        }
        //        string pstrToolTip = "";
        //        if (value)
        //        {
        //            pstrToolTip = "Modalità di Esportazione attiva. Prima di procedere confermare o annullare l'operazione.";
        //        }
        //        else
        //        {
        //            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Dettaglio;
        //        }
        //        gridData.ToolTip = pstrToolTip;
        //        lnkInserisci.ToolTip = pstrToolTip;
        //    }
        //}
        #endregion
    }

}