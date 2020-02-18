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
using System.Linq;
using System.IO;

namespace Green.Apple.Management
{
    public partial class Green_Admin_ExportFattureClienti : Green_BaseUserControl
    {

        #region DECLARATIONS

        protected string NOMEOGGETTO = "Nome Oggetto";
        protected string CODICEOGGETTO = "Codice Oggetto";
        protected string MEX_NOMEOGGETTO = "l'oggetto";
        protected string TABLENAME = "TABELLA";
        protected string COLUMNID = "ID";
        protected string COLUMNNUMERO = "Numero";
        protected string COLUMNTIPO = "Tipo";
        protected string COLUMNDATA = "Data";
        protected string COLUMNCLIENTE = "Cliente";
        protected string COLUMNMODIFICATA = "Modificata";
        //protected string COLUMNMESSAGE = "Messaggio";
        //protected string COLUMNIVA = "IVA";
        //protected string COLUMNNAZIONE = "NAZIONE";
        //protected string COLUMNORDER = "POSIZIONE";
        //protected int COL_MODIFICA = 3;
        //protected int COL_ELIMINA = 4;
        protected int COL_ESPORTA = 6;
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

            COL_ESPORTA = 6;

            TABLENAME = "Export_Documclienti";
            CheckUnivocita = true;
            CheckOrder = true;
            NvarcharID = true;


            NOMEOGGETTO = "Fatture";
            CODICEOGGETTO = "Codice";
            MEX_NOMEOGGETTO = "la Fattura";
            COLUMNID = "Id";


            gridData.DataKeyField = COLUMNID;

            ((BoundColumn)gridData.Columns[0]).DataField = COLUMNID;
            ((BoundColumn)gridData.Columns[0]).HeaderText = "ID";

            ((BoundColumn)gridData.Columns[1]).DataField = COLUMNNUMERO;
            ((BoundColumn)gridData.Columns[1]).HeaderText = COLUMNNUMERO;

            ((BoundColumn)gridData.Columns[2]).DataField = COLUMNTIPO;
            ((BoundColumn)gridData.Columns[2]).HeaderText = COLUMNTIPO;

            ((BoundColumn)gridData.Columns[3]).DataField = COLUMNDATA;
            ((BoundColumn)gridData.Columns[3]).HeaderText = COLUMNDATA;

            ((BoundColumn)gridData.Columns[4]).DataField = COLUMNCLIENTE;
            ((BoundColumn)gridData.Columns[4]).HeaderText = COLUMNCLIENTE;

            ((BoundColumn)gridData.Columns[5]).DataField = COLUMNMODIFICATA;
            ((BoundColumn)gridData.Columns[5]).HeaderText = COLUMNMODIFICATA;


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

                        string ID = gridData.Items[e.Item.ItemIndex].Cells[1].Text.Replace("&nbsp;", "").Trim();

                        // formatta DCdata a dd-MM-yyyy H:mm:ss
                        sql = @"SELECT [DCnumeroCompleto] AS NumeroFattura
                                      ,[DCtipo] AS FatturaNotaCredito
                                      ,FORMAT(CAST([DCdata] AS DATE), 'dd-MM-yyyy H:mm:ss') AS DataFattura
                                      ,[DCcliente] AS CodiceCliente
                                      ,CASE 
										WHEN [DCimponibile]+[DCspese]+[DCdiritti] < 0 THEN -([DCimponibile]+[DCspese]+[DCdiritti]) ELSE [DCimponibile]+[DCspese]+[DCdiritti]
										END AS ImponibileLordo
                                      ,CASE
										WHEN [DCIVA] < 0 THEN -([DCIVA]) ELSE [DCIVA]
										END AS ImportoIVA
	                                  ,0 AS Spese
	                                  ,0 AS Diritti
                                      ,CASE
										WHEN [DCtotale] < 0 THEN -([DCtotale]) ELSE [DCtotale]
										END AS ImportoTotale
                                      ,[PRcategoria] AS CategoriaServizio
                                      ,[DCpagam] AS TipoPagamento
                                      ,[Modificata], [DCPiazza] as IBAN
                                  FROM [dbo].[EXPORT_Documclienti]
                                  WHERE DCnumeroCompleto = '" + ID + "'";
                        sql += " ORDER BY DCdata";

                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        //esporto in CSV la DataTable
                        EsportaDataTableInCSV(DTResult);

                        //modifico il flag ESPORTATA
                        sql = @"UPDATE [dbo].[EXPORT_DocumClienti]
                                SET Esportata = 1
                                WHERE DCnumeroCompleto = '" + ID + "'";
                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                        CaricaDati();
                        break;
                    }
            }
        }
        protected void lnkEsporta_Click(object sender, EventArgs e)
        {
            // esporta TUTTO
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            gridData.SelectedIndex = -1;

            
            // Se il giorno odierno è < 17, considero le fatture a partire da quelle del mese scorso
            // se il giorno odierno è >= 17, considero le fatture a partire da quelle del mese corrente
            // formatta DCdata a dd-MM-yyyy H:mm:ss
            //cambiato da 17 a 18, se il 16 capita di venerdì bisogna inviare l'IVA il lunedì 18
            //NB: la funzione FORMAT non funziona correttamente in SQL 2008 perche non esiste, ma in produzione va (SQL 2016)v
            sql = @"DECLARE @now AS DATETIME
                    SET @now = getdate()

                    DECLARE @dateFrom AS DATETIME
                    DECLARE @anno AS int
                    DECLARE @mese AS int

                    IF (DAY(@now) <= 18)
	                    BEGIN
		                    SET @dateFrom = DATEADD(MONTH, -1, @now)
	                    END 
                    ELSE
	                    BEGIN
		                    SET @dateFrom = @now
	                    END

                    SET @anno = YEAR(@dateFrom)
                    SET @mese = MONTH(@dateFrom)

                    SELECT [DCnumeroCompleto] AS NumeroFattura
                          ,[DCtipo] AS FatturaNotaCredito
                          ,FORMAT(CAST([DCdata] AS DATE), 'dd-MM-yyyy H:mm:ss') AS DataFattura
                          ,[DCcliente] AS CodiceCliente
                          ,CASE 
							WHEN [DCimponibile]+[DCspese]+[DCdiritti] < 0 THEN -([DCimponibile]+[DCspese]+[DCdiritti]) ELSE [DCimponibile]+[DCspese]+[DCdiritti]
							END AS ImponibileLordo
                          ,CASE
							WHEN [DCIVA] < 0 THEN -([DCIVA]) ELSE [DCIVA]
							END AS ImportoIVA
	                      ,0 AS Spese
	                      ,0 AS Diritti
                          ,CASE
							WHEN [DCtotale] < 0 THEN -([DCtotale]) ELSE [DCtotale]
							END AS ImportoTotale
                          ,[PRcategoria] AS CategoriaServizio
                          ,[DCpagam] AS TipoPagamento
                          ,[Modificata], [DCPiazza] as IBAN
                      FROM [dbo].[EXPORT_Documclienti]
                      WHERE Esportata = 0
                      AND MONTH(CAST([DCdata] as DATE)) >= @mese
                      AND YEAR(CAST([DCdata] as DATE)) >= @anno
                      ORDER BY DCdata";

            /*
             ,[DCimponibile] AS ImponibileLordo
                          ,[DCIVA] AS ImportoIVA
	                      ,[DCspese] AS Spese
	                      ,[DCdiritti] AS Diritti
                          ,[DCtotale] AS ImportoTotale
             */

            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            if (DTResult.Rows.Count > 0) 
            {
                //esporto in CSV
                EsportaDataTableInCSV(DTResult);

                //modifico il flag ESPORTATA
                string IDs = "";
                foreach (DataRow row in DTResult.Rows)
                {
                    IDs += ", '" + row["NumeroFattura"].ToString() + "'";
                }
                IDs = IDs.Substring(2);

                sql = @"UPDATE [dbo].[EXPORT_DocumClienti]
                                    SET Esportata = 1
                                    WHERE DCnumeroCompleto IN (" + IDs + ")";

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
            // Se il giorno odierno è < 17, considero le fatture a partire da quelle del mese scorso
            // se il giorno odierno è >= 17, considero le fatture a partire da quelle del mese corrente
            //formatta DCdata a dd-MM-yyyy H:mm:ss
            //cambiato da 17 a 18, se il 16 capita di sabato bisogna inviare l'IVA il lunedì 18
            //NB: la funzione FORMAT non funziona correttamente in SQL 2008 perche non esiste, ma in produzione va (SQL 2016)
            sql = @"DECLARE @now AS DATETIME
                    SET @now = getdate()

                    DECLARE @dateFrom AS DATETIME
                    DECLARE @anno AS int
                    DECLARE @mese AS int

                    IF (DAY(@now) <= 18)
	                    BEGIN
		                    SET @dateFrom = DATEADD(MONTH, -1, @now)
	                    END 
                    ELSE
	                    BEGIN
		                    SET @dateFrom = @now
	                    END

                    SET @anno = YEAR(@dateFrom)
                    SET @mese = MONTH(@dateFrom)
			
                    SELECT [DCid] ID
                      ,[DCnumeroCompleto] Numero
                      ,[DCtipo] Tipo
                      ,FORMAT(CAST([DCdata] AS DATE), 'dd-MM-yyyy H:mm:ss') Data
                      ,C.[Cliragsoc] Cliente
                      ,[Modificata] 
                    FROM [dbo].[EXPORT_Documclienti] F
                    INNER JOIN [dbo].[Clienti] C ON F.DCcliente = C.Clicodice
                    WHERE Esportata = 0
                    AND MONTH(CAST([DCdata] as DATE)) >= @mese
                    AND YEAR(CAST([DCdata] as DATE)) >= @anno";
            
            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " AND [DCnumeroCompleto] like '%" + txtFiltroLibero.Text.Trim() + "%'";
                sql += " OR [DCtipo] like '%" + txtFiltroLibero.Text.Trim() + "%'";
                sql += " OR C.[Cliragsoc] like '%" + txtFiltroLibero.Text.Trim() + "%'";
            }

            //sql += " ORDER BY ltrim(rtrim(C.[Cliragsoc]))";

            // ordino per anno e numero fattura
            sql += @" ORDER BY CAST(
							CASE
							WHEN CHARINDEX('/',DCnumeroCompleto) > 0 THEN SUBSTRING(DCnumeroCompleto, CHARINDEX('/',DCnumeroCompleto)+1, LEN(DCnumeroCompleto))
							ELSE '0'
							END
							AS INT
						)
						,
						CAST(
							CASE
							WHEN CHARINDEX('/',DCnumeroCompleto) > 0 THEN REPLACE(LEFT(DCnumeroCompleto, CHARINDEX('/',DCnumeroCompleto)), '/', '') 
							ELSE DCnumeroCompleto 
							END 
							AS INT
						)";
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
                                + "_fatture.csv";

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