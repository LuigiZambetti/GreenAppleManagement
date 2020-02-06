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
using System.Collections.Generic;
/*
 * EDIT:
     Aggiunta CliNumDicIntenti e CliDataDicIntenti per gestire la dichiarazione d'intenti
     Al click su modifica carico i dati presenti in DB nei TextBox
 */
namespace Green.Apple.Management
{
    public partial class Green_View_Clienti : Green_BaseUserControl
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

            /* Added */
            clsFunctions.AssegnaEventoCalendar(imgtxtCliDataDicIntenti, txtCliDataDicIntenti, false);

            COL_ELIMINA = 3;

            gridData.DataKeyField = "Clicodice";
            sqlControl = "select * from servizi where SRCliente = @CODE";

            objPagining.CaricaPagining(phPagine, gridData);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            lblError.Text = "";
            lblError.Visible = false;
            if (!IsPostBack)
            {
                ViewState["IDMOD"] = "";
                ModificaVisible = false;
                gridData.Visible = true;
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

                        string IDCliente = gridData.DataKeys[e.Item.ItemIndex].ToString();
                        sql = "SELECT * FROM CLIENTI ";
                        sql += " WHERE Clicodice = '" + IDCliente + "' ";
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        ViewState["IDMOD"] = IDCliente.ToString();
                        if (DTResult.Rows.Count > 0)
                        {
                            DataRow myRow = DTResult.Rows[0];

                            //txtCliBanca.Text = "" + myRow["CliBanca"].ToString();
                            txtCliCAP.Text = "" + myRow["CliCAP"].ToString();
                            txtCliCF.Text = "" + myRow["CliCF"].ToString();
                            txtClicodfor.Text = "" + myRow["Clicodfor"].ToString();
                            txtClifax.Text = "" + myRow["Clifax"].ToString();
                            txtCliIBAN.Text = "" + myRow["CliIBAN"].ToString();
                            txtCliindirizzo.Text = "" + myRow["Cliindirizzo"].ToString();
                            txtCliInUso.Checked = Convert.ToBoolean(myRow["CliInUso"]);
                            txtCliIVA.Text = "" + myRow["CliIVA"].ToString();

                            for (int i = 0; i < cbotxtCliLingua.Items.Count; i++)
                            {
                                if (cbotxtCliLingua.Items[i].Value == myRow["CliLingua"].ToString())
                                {
                                    cbotxtCliLingua.SelectedIndex = i;
                                    break;
                                }
                            }
                            //txtCliLingua.Text = "" + myRow["CliLingua"].ToString();

                            txtClilocalita.Text = "" + myRow["Clilocalita"].ToString();
                            txtCliNazione.Text = "" + myRow["CliNazione"].ToString();

                            for (int i = 0; i < cbotxtClipagamento.Items.Count; i++)
                            {
                                if (cbotxtClipagamento.Items[i].Value == myRow["Clipagamento"].ToString())
                                {
                                    cbotxtClipagamento.SelectedIndex = i;
                                    break;
                                }
                            }

                            for (int i = 0; i < cbotxtCliBanca.Items.Count; i++)
                            {
                                if (cbotxtCliBanca.Items[i].Value == myRow["CliBanca"].ToString())
                                {
                                    cbotxtCliBanca.SelectedIndex = i;
                                    break;
                                }
                            }

                            txtClipercdiritti.Text = "" + myRow["Clipercdiritti"].ToString();

                            txtTarParrucchiere.Text = "" + myRow["TarParrucchiere"];
                            txtTarTruccatore.Text = "" + myRow["TarTruccatore"];
                            txtTarManicurista.Text = "" + myRow["TarManicurista"];
                            txtTarGroomer.Text = "" + myRow["TarGroomer"];

                            if (txtTarParrucchiere.Text == "") txtTarParrucchiere.Text = "0";
                            if (txtTarTruccatore.Text == "") txtTarTruccatore.Text = "0";
                            if (txtTarManicurista.Text == "") txtTarManicurista.Text = "0";
                            if (txtTarGroomer.Text == "") txtTarGroomer.Text = "0";

                            txtClipercIVA.Text = "" + myRow["ClipercIVA"].ToString();
                            txtCliPiazza.Text = "" + myRow["CliPiazza"].ToString();
                            txtClipostaCAP.Text = "" + myRow["ClipostaCAP"].ToString();
                            txtClipostaIndirizzo.Text = "" + myRow["ClipostaIndirizzo"].ToString();
                            txtClipostalocalita.Text = "" + myRow["Clipostalocalita"].ToString();
                            txtClipostaNazione.Text = "" + myRow["ClipostaNazione"].ToString();
                            txtClipostaprov.Text = "" + myRow["Clipostaprov"].ToString();
                            txtClipostaragsoc.Text = "" + myRow["Clipostaragsoc"].ToString();
                            txtCliprenome.Text = "" + myRow["Cliprenome"].ToString();
                            txtCliprovincia.Text = "" + myRow["Cliprovincia"].ToString();
                            txtCliragsoc.Text = "" + myRow["Cliragsoc"].ToString();
                            txtCliRedazione.Text = "" + myRow["CliRedazione"].ToString();
                            txtCliSWIFT.Text = "" + myRow["CliSWIFT"].ToString();
                            txtClitelefono.Text = "" + myRow["Clitelefono"].ToString();
                            txtCliEmail.Text = "" + myRow["CliEmail"].ToString();
                            txtCliNote.Text = "" + myRow["GANote"].ToString();

                            txtCodDestinatario.Text = "" + myRow["CliCodDestinatario"].ToString();

                            chkGACalcoloIva.Checked = Convert.ToBoolean(myRow["GACalcoloIva"]);

                            txtGAAttenzione.Text = "" + myRow["GAAttenzione"].ToString();

                            for (int i = 0; i < cbotxtCliLingua.Items.Count; i++)
                            {
                                if (cbotxtCliLingua.Items[i].Value == myRow["CliLingua"].ToString())
                                {
                                    cbotxtCliLingua.SelectedIndex = i;
                                    break;
                                }
                            }

                            for (int i = 0; i < cbotxtCliValuta.Items.Count; i++)
                            {
                                if (cbotxtCliValuta.Items[i].Value == myRow["CliValuta"].ToString())
                                {
                                    cbotxtCliValuta.SelectedIndex = i;
                                    break;
                                }
                            }
                            //txtCliValuta.Text = "" + myRow["CliValuta"].ToString();
                            txtNomclatura.Text = "" + myRow["Nomenclatura"].ToString();

                            /* carica CliNumDicIntenti e CliDataDicIntenti */
                            //txtCliNumDicIntenti.Text = "" + myRow["CliNumDicIntenti"].ToString();
                            txtCliDataDicIntenti.Text = "" + string.Format("{0:dd/MM/yyyy}", myRow["CliDataDicIntenti"]);
                        }


                        ModificaVisible = true;
                        gridData.Visible = false;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtCliprenome, this.Page);
                        break;
                    }
                case "ELIMINA":
                    {
                        clsParameter pParameter = new clsParameter();

                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));

                        sql = "DELETE FROM CLIENTI WHERE Clicodice=@ID";
                        clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                        ModificaVisible = false;
                        CaricaDati();

                        gridData.Visible = true;
                        break;
                    }
            }
        }
        protected void lnkInserisci_Click(object sender, EventArgs e)
        {
            ViewState["IDMOD"] = "";
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            gridData.SelectedIndex = -1;
            ModificaVisible = true;
            gridData.Visible = false;

            //txtCliBanca.Text = "";
            txtCliCAP.Text = "";
            txtCliCF.Text = "";
            txtClicodfor.Text = "";
            txtClifax.Text = "";
            txtCliIBAN.Text = "";
            txtCliindirizzo.Text = "";
            txtCliInUso.Checked = true;
            txtCliIVA.Text = "";
            //txtCliLingua.Text = "";
            cbotxtCliLingua.SelectedIndex = 1;
            txtClilocalita.Text = "";
            txtCliNazione.Text = "";
            cbotxtClipagamento.SelectedIndex = 0;
            //txtClipercdiritti.Text = "20";
            txtClipercdiritti.Text = "";

            //************** CARICAMENTO ALIQUOTA IVA CONDIVISA
            string sqlALIQUOTA = "SELECT top 1 AliquotaIVA FROM Admin_DatiAnagrifici";
            DataTable DTResultALIQUOTA = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sqlALIQUOTA, "RESULTALIQUOTA", ref DTResultALIQUOTA, true);
            double ALIQUOTAIVA = double.Parse(DTResultALIQUOTA.Rows[0]["AliquotaIVA"].ToString());
            //************** CARICAMENTO ALIQUOTA IVA CONDIVISA
            txtClipercIVA.Text = ALIQUOTAIVA.ToString();

            cbotxtCliBanca.SelectedIndex = 0;
            txtCliPiazza.Text = "";
            txtClipostaCAP.Text = "";
            txtClipostaIndirizzo.Text = "";
            txtClipostalocalita.Text = "";
            txtClipostaNazione.Text = "";
            txtClipostaprov.Text = "";
            txtClipostaragsoc.Text = "";
            txtCliprenome.Text = "";
            txtCliprovincia.Text = "";
            txtCliragsoc.Text = "";
            txtCliRedazione.Text = "";
            txtCliSWIFT.Text = "";
            txtClitelefono.Text = "";
            txtCliEmail.Text = "";
            txtCliNote.Text = "";
            txtGAAttenzione.Text = "";
            //txtCliValuta.Text = "";
            cbotxtCliValuta.SelectedIndex = 1;

            if (txtTarParrucchiere.Text == "") txtTarParrucchiere.Text = "0";
            if (txtTarTruccatore.Text == "") txtTarTruccatore.Text = "0";
            if (txtTarManicurista.Text == "") txtTarManicurista.Text = "0";
            if (txtTarGroomer.Text == "") txtTarGroomer.Text = "0";

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtCliprenome, this.Page);
            this.Page.Validate();
        }
        protected void lnkAnnulla_Click(object sender, EventArgs e)
        {
            gridData.SelectedIndex = -1;
            ModificaVisible = false;
            gridData.Visible = true;
            CaricaDati();
        }
        protected void lnkAggiorna_Click(object sender, EventArgs e)
        {
            bool ShowError = false;

            if (ViewState["IDMOD"].ToString() == "")
            {

                object CliCAP = txtCliCAP.Text.Trim();
                object ClipostaCAP = txtClipostaCAP.Text.Trim();
                object CliPercIVA = txtClipercIVA.Text.Trim();
                object CliPercDiritti = txtClipercdiritti.Text.Trim();

                if (CliCAP.ToString() == "") CliCAP = System.DBNull.Value;
                if (ClipostaCAP.ToString() == "") ClipostaCAP = System.DBNull.Value;
                if (CliPercIVA.ToString() == "") CliPercIVA = System.DBNull.Value;
                if (CliPercDiritti.ToString() == "") CliPercDiritti = System.DBNull.Value;

                clsParameter pParameter = new clsParameter();
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Cliprenome", txtCliprenome.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Cliragsoc", txtCliragsoc.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Cliindirizzo", txtCliindirizzo.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clilocalita", txtClilocalita.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Cliprovincia", txtCliprovincia.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliCAP", CliCAP, SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliRedazione", txtCliRedazione.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clipostaragsoc", txtClipostaragsoc.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ClipostaIndirizzo", txtClipostaIndirizzo.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clipostalocalita", txtClipostalocalita.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clipostaprov", txtClipostaprov.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ClipostaCAP", ClipostaCAP, SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clitelefono", txtClitelefono.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliEmail", txtCliEmail.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GANote", txtCliNote.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GACalcoloIva", chkGACalcoloIva.Checked, SqlDbType.Bit, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GAAttenzione", txtGAAttenzione.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clifax", txtClifax.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliIVA", txtCliIVA.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliCF", txtCliCF.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clipagamento", cbotxtClipagamento.SelectedValue.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliBanca", cbotxtCliBanca.SelectedValue.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliPiazza", txtCliPiazza.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ClipercIVA", CliPercIVA, SqlDbType.Real, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clipercdiritti", CliPercDiritti, SqlDbType.Real, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clicodfor", txtClicodfor.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliIBAN", txtCliIBAN.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliLingua", cbotxtCliLingua.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliValuta", cbotxtCliValuta.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliNazione", txtCliNazione.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ClipostaNazione", txtClipostaNazione.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliSWIFT", txtCliSWIFT.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliInUso", txtCliInUso.Checked, SqlDbType.Bit, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@TarTruccatore", txtTarTruccatore.Text, SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@TarParrucchiere", txtTarParrucchiere.Text, SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@TarManicurista", txtTarManicurista.Text, SqlDbType.Float, ParameterDirection.Input));
                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@TarGroomer", txtTarGroomer.Text, SqlDbType.Float, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliCodDestinatario", txtCodDestinatario.Text, SqlDbType.NVarChar, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Nomenclatura", txtNomclatura.Text, SqlDbType.NVarChar, ParameterDirection.Input));

                pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clicodice", 0, SqlDbType.Int, ParameterDirection.Output));


                /* nuovi parametri - START */
                //if (txtCliNumDicIntenti.Text != "") {
                //    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliNumDicIntenti", txtCliNumDicIntenti.Text, SqlDbType.NVarChar, ParameterDirection.Input));
                //}
                //else
                //{
                //    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliNumDicIntenti", DBNull.Value, SqlDbType.NVarChar, ParameterDirection.Input));
                //}

                if (txtCliDataDicIntenti.Text != "")
                {
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliDataDicIntenti", txtCliDataDicIntenti.Text, SqlDbType.Date, ParameterDirection.Input));
                }
                else
                {
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliDataDicIntenti", DBNull.Value, SqlDbType.Date, ParameterDirection.Input));
                }
                /* nuovi parametri - END */

                sql = @"INSERT INTO CLIENTI 
                            (
                                Nomenclatura
                                ,Cliprenome
                                ,Cliragsoc
                                ,Cliindirizzo
                                ,Clilocalita
                                ,Cliprovincia
                                ,CliCAP
                                ,CliRedazione
                                ,Clipostaragsoc
                                ,ClipostaIndirizzo
                                ,Clipostalocalita
                                ,Clipostaprov
                                ,ClipostaCAP
                                ,Clitelefono,CliEmail,GANote,GAAttenzione,GACalcoloIva
                                ,Clifax
                                ,CliIVA
                                ,CliCF
                                ,Clipagamento
                                ,CliBanca
                                ,CliPiazza
                                ,ClipercIVA
                                ,Clipercdiritti
                                ,Clicodfor
                                ,CliIBAN
                                ,CliLingua
                                ,CliValuta
                                ,CliNazione
                                ,ClipostaNazione
                                ,CliSWIFT
                                ,CliCodDestinatario
                                ,CliInUso,TarTruccatore,TarParrucchiere,TarManicurista,TarGroomer
                                --,CliNumDicIntenti -- FC 20180312 nuovi parametri
                                ,CliDataDicIntenti -- FC 20180312 nuovi parametri
                            ) 
                            VALUES(
                                @Nomenclatura
                                ,@Cliprenome
                                ,@Cliragsoc
                                ,@Cliindirizzo
                                ,@Clilocalita
                                ,@Cliprovincia
                                ,@CliCAP
                                ,@CliRedazione
                                ,@Clipostaragsoc
                                ,@ClipostaIndirizzo
                                ,@Clipostalocalita
                                ,@Clipostaprov
                                ,@ClipostaCAP
                                ,@Clitelefono,@CliEmail,@GANote,@GAAttenzione,@GACalcoloIva
                                ,@Clifax
                                ,@CliIVA
                                ,@CliCF
                                ,@Clipagamento
                                ,@CliBanca
                                ,@CliPiazza
                                ,@ClipercIVA
                                ,@Clipercdiritti
                                ,@Clicodfor
                                ,@CliIBAN
                                ,@CliLingua
                                ,@CliValuta
                                ,@CliNazione
                                ,@ClipostaNazione
                                ,@CliSWIFT
                                ,@CliCodDestinatario
                                ,@CliInUso,@TarTruccatore,@TarParrucchiere,@TarManicurista,@TarGroomer
                                --,@CliNumDicIntenti -- FC 20180312 nuovi parametri
                                ,@CliDataDicIntenti -- FC 20180312 nuovi parametri
                                ) 
                                SET @Clicodice = SCOPE_IDENTITY()";

                Dictionary<string, string> Ret = new Dictionary<string, string>();

                Ret = clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                string IDCLIENTE = Ret["@Clicodice"].ToString();


                // inserisco in tabella EXPORT_Clienti

                sql = @"INSERT INTO [dbo].[EXPORT_Clienti]

                                    SELECT [Clicodice] AS IdCliente
                                          ,[Cliprenome] AS Nome
                                          ,[Cliragsoc] AS RagioneSociale
                                          ,[Cliindirizzo] As Indirizzo
                                          ,[Clilocalita] AS Localita
                                          ,CASE
	                                      WHEN [CliNazione] = 'ITALIA' THEN [Cliprovincia] ELSE 'EE'
	                                      END AS Provincia
                                          ,[CliCAP] AS CAP
	                                      ,[CliNazione] AS Nazione
	                                      ,[Clitelefono] AS Telefono
	                                      ,[CliEMAIL] AS Email
	                                      ,[Clifax] AS Fax
	                                      ,[CliIVA] AS PartitaIva
	                                      ,[Clipagamento] AS TipoPagamento
	                                      ,[CliBanca] AS Banca
	                                      ,REPLACE([CliIBAN], 'IBAN ', '') AS IBAN
	                                      ,REPLACE([CliSWIFT], 'BIC/SWIFT ', '') AS SWIFT
                                          ,0
                                          ,0
                                      FROM [dbo].[Clienti] C
                                      WHERE Clicodice = " + IDCLIENTE;
                clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                // -----------------------------------

                ModificaVisible = false;
                CaricaDati();

                gridData.Visible = true;
                gridData.SelectedIndex = -1;
            }
            if (ViewState["IDMOD"].ToString() != "")
            {
                ShowError = false;

                if (ShowError == false)
                {
                    object CliCAP = txtCliCAP.Text.Trim();
                    object ClipostaCAP = txtClipostaCAP.Text.Trim();
                    object CliPercIVA = txtClipercIVA.Text.Trim();
                    object CliPercDiritti = txtClipercdiritti.Text.Trim();

                    if (CliCAP.ToString() == "") CliCAP = System.DBNull.Value;
                    if (ClipostaCAP.ToString() == "") ClipostaCAP = System.DBNull.Value;
                    if (CliPercIVA.ToString() == "") CliPercIVA = System.DBNull.Value;
                    if (CliPercDiritti.ToString() == "") CliPercDiritti = System.DBNull.Value;

                    clsParameter pParameter = new clsParameter();
                    string IDCLIENTE = gridData.DataKeys[gridData.SelectedIndex].ToString();

                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", IDCLIENTE, SqlDbType.NVarChar, ParameterDirection.Input));

                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Cliprenome", txtCliprenome.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Cliragsoc", txtCliragsoc.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Cliindirizzo", txtCliindirizzo.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clilocalita", txtClilocalita.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Cliprovincia", txtCliprovincia.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliCAP", CliCAP, SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliRedazione", txtCliRedazione.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clipostaragsoc", txtClipostaragsoc.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ClipostaIndirizzo", txtClipostaIndirizzo.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clipostalocalita", txtClipostalocalita.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clipostaprov", txtClipostaprov.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ClipostaCAP", ClipostaCAP, SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clitelefono", txtClitelefono.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliEmail", txtCliEmail.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GANote", txtCliNote.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GAAttenzione", txtGAAttenzione.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GACalcoloIva", chkGACalcoloIva.Checked, SqlDbType.Bit, ParameterDirection.Input));

                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clifax", txtClifax.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliIVA", txtCliIVA.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliCF", txtCliCF.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clipagamento", cbotxtClipagamento.SelectedValue.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliBanca", cbotxtCliBanca.SelectedValue.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliPiazza", txtCliPiazza.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ClipercIVA", CliPercIVA, SqlDbType.Real, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clipercdiritti", CliPercDiritti, SqlDbType.Real, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Clicodfor", txtClicodfor.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));

                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliIBAN", txtCliIBAN.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliLingua", cbotxtCliLingua.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliValuta", cbotxtCliValuta.SelectedValue, SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliNazione", txtCliNazione.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ClipostaNazione", txtClipostaNazione.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliSWIFT", txtCliSWIFT.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliInUso", txtCliInUso.Checked, SqlDbType.Bit, ParameterDirection.Input));

                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliCodDestinatario", txtCodDestinatario.Text, SqlDbType.NVarChar, ParameterDirection.Input));

                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@TarTruccatore", txtTarTruccatore.Text, SqlDbType.Float, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@TarParrucchiere", txtTarParrucchiere.Text, SqlDbType.Float, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@TarManicurista", txtTarManicurista.Text, SqlDbType.Float, ParameterDirection.Input));
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@TarGroomer", txtTarGroomer.Text, SqlDbType.Float, ParameterDirection.Input));

                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Nomenclatura", txtNomclatura.Text, SqlDbType.NVarChar, ParameterDirection.Input));

                    /* nuovi parametri - START */
                    //if (txtCliNumDicIntenti.Text != "")
                    //{
                    //    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliNumDicIntenti", txtCliNumDicIntenti.Text, SqlDbType.NVarChar, ParameterDirection.Input));
                    //}
                    //else
                    //{
                    //    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliNumDicIntenti", DBNull.Value, SqlDbType.NVarChar, ParameterDirection.Input));
                    //}

                    if (txtCliDataDicIntenti.Text != "")
                    {
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliDataDicIntenti", txtCliDataDicIntenti.Text, SqlDbType.Date, ParameterDirection.Input));
                    }
                    else
                    {
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@CliDataDicIntenti", DBNull.Value, SqlDbType.Date, ParameterDirection.Input));
                    }
                    /*nuovi parametri - END */

                    sql = "UPDATE CLIENTI "
                    + @" SET 
                                Nomenclatura = @Nomenclatura
                                ,Cliprenome = @Cliprenome
                                ,Cliragsoc = @Cliragsoc
                                ,Cliindirizzo = @Cliindirizzo
                                ,Clilocalita = @Clilocalita
                                ,Cliprovincia = @Cliprovincia
                                ,CliCAP = @CliCAP
                                ,CliRedazione = @CliRedazione
                                ,Clipostaragsoc = @Clipostaragsoc
                                ,ClipostaIndirizzo = @ClipostaIndirizzo
                                ,Clipostalocalita = @Clipostalocalita
                                ,Clipostaprov = @Clipostaprov
                                ,ClipostaCAP = @ClipostaCAP
                                ,Clitelefono = @Clitelefono
                                ,CliEmail = @CliEmail
                                ,GANote = @GANote
                                ,GACalcoloIva = @GACalcoloIva
                                ,GAAttenzione = @GAAttenzione
                                ,Clifax = @Clifax
                                ,CliIVA = @CliIVA
                                ,CliCF = @CliCF
                                ,Clipagamento = @Clipagamento
                                ,CliBanca = @CliBanca
                                ,CliPiazza = @CliPiazza
                                ,ClipercIVA = @ClipercIVA
                                ,Clipercdiritti = @Clipercdiritti
                                ,Clicodfor = @Clicodfor
                                ,CliIBAN = @CliIBAN
                                ,CliLingua = @CliLingua
                                ,CliValuta = @CliValuta
                                ,CliNazione = @CliNazione
                                ,ClipostaNazione = @ClipostaNazione
                                ,CliSWIFT = @CliSWIFT
                                ,CliInUso = @CliInUso
                                ,CliCodDestinatario = @CliCodDestinatario
                                ,TarTruccatore = @TarTruccatore
                                ,TarParrucchiere = @TarParrucchiere
                                ,TarManicurista = @TarManicurista
                                ,TarGroomer = @TarGroomer
                                --,CliNumDicIntenti = @CliNumDicIntenti -- FC 20180312 nuovi parametri
                                ,CliDataDicIntenti = @CliDataDicIntenti -- FC 20180312 nuovi parametri
                            WHERE CLICODICE = @ID";
                    clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                    /* aggiornamento fatture a IVA 0, se postume a CliDataDicIntenti */
                    //if (txtCliNumDicIntenti.Text != "" && txtCliDataDicIntenti.Text != "")
                    if (txtCliDataDicIntenti.Text != "" && !chkGACalcoloIva.Checked)
                    {
                        sql = @"UPDATE Documclienti
                                            SET  DCperciva = 0
                                                ,DCIVA = 0
                                                ,DCtotale = (DCimponibile+DCspese+DCdiritti)
                                                ,DCnoteCliente = '" + txtCliNote.Text.Trim() + @"'
                                                ,CliDataDicIntenti = '" + txtCliDataDicIntenti.Text + @"'
                                            WHERE DCtipo = 'FATTURA' AND DCcliente = " + IDCLIENTE + " AND DCdata >= '" + txtCliDataDicIntenti.Text + "'";
                        clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);
                    }


                    // Aggiorno la tabella EXPORT_Clienti (in realtà cancello e poi reinserisco da Clienti, per avere i dati aggiornati) e metto Modificata = true

                    sql = @"DELETE FROM [dbo].[EXPORT_Clienti]
                                      WHERE Clicodice = " + IDCLIENTE;
                    clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                    sql = @"INSERT INTO [dbo].[EXPORT_Clienti]

                                    SELECT [Clicodice] AS IdCliente
                                          ,[Cliprenome] AS Nome
                                          ,[Cliragsoc] AS RagioneSociale
                                          ,[Cliindirizzo] As Indirizzo
                                          ,[Clilocalita] AS Localita
                                          ,CASE
	                                      WHEN [CliNazione] = 'ITALIA' THEN [Cliprovincia] ELSE 'EE'
	                                      END AS Provincia
                                          ,[CliCAP] AS CAP
	                                      ,[CliNazione] AS Nazione
	                                      ,[Clitelefono] AS Telefono
	                                      ,[CliEMAIL] AS Email
	                                      ,[Clifax] AS Fax
	                                      ,[CliIVA] AS PartitaIva
	                                      ,[Clipagamento] AS TipoPagamento
	                                      ,[CliBanca] AS Banca
	                                      ,REPLACE([CliIBAN], 'IBAN ', '') AS IBAN
	                                      ,REPLACE([CliSWIFT], 'BIC/SWIFT ', '') AS SWIFT
                                          ,0
                                          ,1
                                      FROM [dbo].[Clienti] C
                                      WHERE Clicodice = " + IDCLIENTE;
                    clsDB.Execute_Command(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql);

                    // -------------------------------------------------------------------------

                    ModificaVisible = false;
                    CaricaDati();

                    gridData.Visible = true;
                    gridData.SelectedIndex = -1;
                }
            }

            if (ShowError)
            {
                lblError.Text = "Esite già Cliente con la stessa ragione sociale.";
                lblError.Visible = true;
            }
        }

        protected void lnkCerca_Click(object sender, EventArgs e)
        {
            CaricaDati();
        }
        #endregion

        #region FUNCTIONS
        private void CaricaComboLinguaValuta()
        {
            ListItem myItem = new ListItem();
            DataTable DTResult = new DataTable();

            //Carico Lingue
            sql = @"select * from Lista_Lingua ORDER BY POSIZIONE ";
            DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cbotxtCliLingua.Items.Clear();
            myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Selezionare una lingua --";
            cbotxtCliLingua.Items.Add(myItem);

            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["CodLingua"];
                myItem.Text = "" + DTResult.Rows[i]["Codice"] + " - " + DTResult.Rows[i]["Descrizione"];
                cbotxtCliLingua.Items.Add(myItem);
            }

            cbotxtCliLingua.SelectedIndex = 0;

            //Carico Valuta
            sql = @"select * from Lista_Valuta ORDER BY POSIZIONE ";
            DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cbotxtCliValuta.Items.Clear();
            myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Selezionare una lingua --";
            cbotxtCliValuta.Items.Add(myItem);

            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["Codice"];
                myItem.Text = "" + DTResult.Rows[i]["Codice"] + " - " + DTResult.Rows[i]["Descrizione"];
                cbotxtCliValuta.Items.Add(myItem);
            }

            cbotxtCliValuta.SelectedIndex = 0;


            //Carico PAGAMENTO
            sql = @"select * from ModalitaPagamento ORDER BY DESCRIZIONE";
            DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cbotxtClipagamento.Items.Clear();
            myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Selezionare modalità pagamento --";
            cbotxtClipagamento.Items.Add(myItem);

            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["Codice"];
                myItem.Text = "" + DTResult.Rows[i]["Codice"] + " - " + DTResult.Rows[i]["Descrizione"];
                cbotxtClipagamento.Items.Add(myItem);
            }

            cbotxtClipagamento.SelectedIndex = 0;

            //Carico BANCHE
            sql = @"select * from Banche ORDER BY OrdineLista";
            DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

            cbotxtCliBanca.Items.Clear();
            myItem = new ListItem();
            myItem.Value = "0";
            myItem.Text = "-- Selezionare banca --";
            cbotxtCliBanca.Items.Add(myItem);

            for (int i = 0; i < DTResult.Rows.Count; i++)
            {
                myItem = new ListItem();
                myItem.Value = "" + DTResult.Rows[i]["IBAN"] + "#" + DTResult.Rows[i]["SWIFT"];
                myItem.Text = "" + DTResult.Rows[i]["Banca"];
                cbotxtCliBanca.Items.Add(myItem);
            }

            cbotxtCliBanca.SelectedIndex = 0;

        }

        private void CaricaDati()
        {
            CaricaComboLinguaValuta();

            sql = "SELECT ModalitaPagamento.Descrizione as ClipagamentoDESC,Clienti.* FROM CLIENTI LEFT JOIN ModalitaPagamento on Clienti.CliPagamento = ModalitaPagamento.Codice ";
            //sql += " WHERE CliInUso = 1 ";
            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                //sql += " AND (Clicodice like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " WHERE (Clicodice like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR Cliprenome like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR CliIVA like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR Cliragsoc like '%" + txtFiltroLibero.Text.Trim() + "%' )";
            }

            sql += " ORDER BY Clicodice DESC, Cliragsoc ";
            DataTable DTResult = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);
            try
            {
                int maxPage = (int)Math.Floor((double) (DTResult.Rows.Count / gridData.PageSize));
                if (gridData.CurrentPageIndex > maxPage)
                    gridData.CurrentPageIndex = maxPage;
                //if (gridData.CurrentPageIndex >= ((double)DTResult.Rows.Count / (double)gridData.PageSize))
                //{
                //    gridData.CurrentPageIndex -= 1;
                //}
            }
            catch { }
            gridData.DataSource = DTResult;
            gridData.DataBind();

            for (int i = 0; i <= gridData.Items.Count - 1; i++)
            {
                ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare il Cliente selezionato/a ?')){}else{return false}");
                if (sqlControl != "")
                {
                    string sqlControlCODE = sqlControl.Replace("@CODE", DTResult.Rows[i]["Clicodice"].ToString());
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

        protected void cbotxtCliBanca_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCliIBAN.Text = cbotxtCliBanca.SelectedItem.Value.Split('#')[0];
            txtCliSWIFT.Text = cbotxtCliBanca.SelectedItem.Value.Split('#')[1];
        }
    }
}