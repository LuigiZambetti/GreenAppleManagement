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
    public partial class Green_View_Fornitori : Green_BaseUserControl
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

            COL_ELIMINA = 3;

            gridData.DataKeyField = "Forcodice";
            sqlControl = "select * from Servizi where SROperatore = @CODE";

            objPagining.CaricaPagining(phPagine, gridData);
            objPagining.PageChange += new clsPagining.CustomPaginingEventHandler(objPagining_PageChange);
            lblError.Text = "";
            lblError.Visible = false;
            if (!IsPostBack)
            {
                ViewState["IDMOD"] = "";            
                ViewState["MODFORN"] = "NO";
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
                        ViewState["MODFORN"] = "SI";
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        gridData.SelectedIndex = e.Item.ItemIndex;

                        string IDCliente = gridData.DataKeys[e.Item.ItemIndex].ToString();
                        sql = "SELECT * FROM FORNITORI ";
                        sql += " WHERE Forcodice = '" + IDCliente + "' ";
                        DataTable DTResult = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "RESULT", ref DTResult, true);

                        ViewState["IDMOD"] = IDCliente.ToString();
                        if (DTResult.Rows.Count > 0)
                        {
                            DataRow myRow = DTResult.Rows[0];

                            txtForprenome.Text = "" + myRow["Forprenome"].ToString();
                            txtForragsoc.Text = "" + myRow["Forragsoc"].ToString();
                            txtForindirizzo.Text = "" + myRow["Forindirizzo"].ToString();
                            txtForlocalita.Text = "" + myRow["Forlocalita"].ToString();
                            txtForprovincia.Text = "" + myRow["Forprovincia"].ToString();
                            txtForCAP.Text = "" + myRow["ForCAP"].ToString();
                            txtForNazione.Text = myRow["ForNazione"].ToString();
                            txtForpostaragsoc.Text = "" + myRow["Forpostaragsoc"].ToString();
                            txtForpostaind.Text = "" + myRow["Forpostaind"].ToString();
                            txtForpostaloc.Text = "" + myRow["Forpostaloc"].ToString();
                            txtForpostCAP.Text = "" + myRow["ForpostCAP"].ToString();
                            txtForPostaNazione.Text = "" + myRow["ForPostaNazione"].ToString();
                            txtForPostaProv.Text = "" + myRow["ForPostaProv"].ToString();
                            txtFortelef.Text = "" + myRow["Fortelef"].ToString();
                            txtForEmail.Text = "" + myRow["ForEmail"].ToString();
                            txtForNote.Text = "" + myRow["GANote"].ToString();
                            txtForfax.Text = "" + myRow["Forfax"].ToString();
                            txtForIVA.Text = "" + myRow["ForIVA"].ToString();
                            txtForCF.Text = "" + myRow["ForCF"].ToString();
                            txtFordatanascita.Text = "" + myRow["Fordatanascita"].ToString();
                            txtForluogonascita.Text = "" + myRow["Forluogonascita"].ToString();
                            txtForpagamento.Text = "" + myRow["Forpagamento"].ToString();
                            txtForbanca.Text = "" + myRow["Forbanca"].ToString();
                            txtForPiazza.Text = "" + myRow["ForPiazza"].ToString();
                            txtForpercIVA.Text = "" + myRow["ForpercIVA"].ToString();
                            txtForpercritenuta.Text = "" + myRow["Forpercritenuta"].ToString();
                            txtForpercacconto.Text = "" + myRow["Forpercacconto"].ToString();
                            txtForpercsuacconto.Text = "" + myRow["Forpercsuacconto"].ToString();
                            txtForperc1.Text = "" + myRow["Forperc1"].ToString();
                            txtForperc2.Text = "" + myRow["Forperc2"].ToString();
                            txtForperc3.Text = "" + myRow["Forperc3"].ToString();
                            txtForperc4.Text = "" + myRow["Forperc4"].ToString();
                            txtForperc5.Text = "" + myRow["Forperc5"].ToString();
                            txtForperc6.Text = "" + myRow["Forperc6"].ToString();

                            txtForcat1.Text = "" + myRow["Forcat1"].ToString();
                            txtForcat2.Text = "" + myRow["Forcat2"].ToString();
                            txtForcat3.Text = "" + myRow["Forcat3"].ToString();
                            txtForcat4.Text = "" + myRow["Forcat4"].ToString();
                            txtForcat5.Text = "" + myRow["Forcat5"].ToString();
                            txtForcat6.Text = "" + myRow["Forcat6"].ToString();

                            txtForIBAN.Text = "" + myRow["ForIBAN"].ToString();
                            txtForPctRivPrev.Text = "" + myRow["ForPctRivPrev"].ToString();
                            txtForPagCodice.Text = "" + myRow["ForPagCodice"].ToString();


                            if (myRow["ForInUso"] != System.DBNull.Value)
                            {
                                txtForInUso.Checked = Convert.ToBoolean(myRow["ForInUso"]);
                            }
                            if (myRow["ForEsenzioneIvaArt15"] != System.DBNull.Value)
                            {
                                txtForEsenzioneIvaArt15.Checked = Convert.ToBoolean(myRow["ForEsenzioneIvaArt15"]);
                            }
                            if (myRow["Foroperatore"] != System.DBNull.Value)
                            {
                                txtForoperatore.Checked = Convert.ToBoolean(myRow["Foroperatore"]);
                            }

                        }

                        ModificaVisible = true;
                        ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Modifica;
                        //clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtForprenome, this.Page);
                        break;
                    }
                case "ELIMINA":
                    {
                        clsParameter pParameter = new clsParameter();

                       
                        pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", gridData.DataKeys[e.Item.ItemIndex], SqlDbType.Int, ParameterDirection.Input));

                        sql = "DELETE FROM FORNITORI WHERE Forcodice=@ID";
						clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);
                        ModificaVisible = false;
                        CaricaDati();
                        
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

            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Inserimento;
            clsFunctions.WriteGOTO_and_FOCUS("SectionEdit", txtForprenome, this.Page);
            this.Page.Validate();
        }
        protected void lnkAnnulla_Click(object sender, EventArgs e)
        {
            gridData.SelectedIndex = -1;
            gridData.Visible = true;
            ModificaVisible = false;
            CaricaDati();
        }
        protected void lnkAggiorna_Click(object sender, EventArgs e)
        {
            bool ShowError = false;

            object ForPctRivPrev = txtForPctRivPrev.Text.Trim();
            object Forperc1 = txtForperc1.Text.Trim();
            object Forperc2 = txtForperc2.Text.Trim();
            object Forperc3 = txtForperc3.Text.Trim();
            object Forperc4 = txtForperc4.Text.Trim();
            object Forperc5 = txtForperc5.Text.Trim();
            object Forperc6 = txtForperc6.Text.Trim();
            object ForpercIVA = txtForpercIVA.Text.Trim();
            object Forpercritenuta = txtForpercritenuta.Text.Trim();
            object Forpercacconto = txtForpercacconto.Text.Trim();
            object Forpercsuacconto = txtForpercsuacconto.Text.Trim();
            object Fordatanascita = txtFordatanascita.Text.Trim();
            object ForpostCAP = txtForpostCAP.Text.Trim();
            object ForCAP = txtForCAP.Text.Trim();

            if (ForPctRivPrev.ToString() == "") ForPctRivPrev = System.DBNull.Value;
            if (Forperc1.ToString() == "") Forperc1 = System.DBNull.Value;
            if (Forperc2.ToString() == "") Forperc2 = System.DBNull.Value;
            if (Forperc3.ToString() == "") Forperc3 = System.DBNull.Value;
            if (Forperc4.ToString() == "") Forperc4 = System.DBNull.Value;
            if (Forperc5.ToString() == "") Forperc5 = System.DBNull.Value;
            if (Forperc6.ToString() == "") Forperc6 = System.DBNull.Value;
            if (ForpercIVA.ToString() == "") ForpercIVA = System.DBNull.Value;
            if (Forpercritenuta.ToString() == "") Forpercritenuta = System.DBNull.Value;
            if (Forpercacconto.ToString() == "") Forpercacconto = System.DBNull.Value;
            if (Forpercsuacconto.ToString() == "") Forpercsuacconto = System.DBNull.Value;
            if (Fordatanascita.ToString() == "") Fordatanascita = System.DBNull.Value;
            if (ForpostCAP.ToString() == "") ForpostCAP = System.DBNull.Value;
            if (ForCAP.ToString() == "") ForCAP = System.DBNull.Value;

            clsParameter pParameter = new clsParameter();

            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forprenome", txtForprenome.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forragsoc", txtForragsoc.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forindirizzo", txtForindirizzo.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forlocalita", txtForlocalita.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forprovincia", txtForprovincia.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForCAP", ForCAP, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForNazione", txtForNazione.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forpostaragsoc", txtForpostaragsoc.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forpostaind", txtForpostaind.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forpostaloc", txtForpostaloc.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForpostCAP", ForpostCAP, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForPostaNazione", txtForPostaNazione.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForPostaProv", txtForPostaProv.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Fortelef", txtFortelef.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForEmail", txtForEmail.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@GANote", txtForNote.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forfax", txtForfax.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForIVA", txtForIVA.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForCF", txtForCF.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Fordatanascita", Fordatanascita, SqlDbType.DateTime, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forluogonascita", txtForluogonascita.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forpagamento", txtForpagamento.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forbanca", txtForbanca.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForPiazza", txtForPiazza.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForpercIVA", txtForpercIVA.Text.Trim(), SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forpercritenuta", txtForpercritenuta.Text.Trim(), SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forpercacconto", Forpercacconto, SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forpercsuacconto", Forpercsuacconto, SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forperc1", Forperc1, SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forperc2", Forperc2, SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forperc3", Forperc3, SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forperc4", Forperc4, SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forperc5", Forperc5, SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forperc6", Forperc6, SqlDbType.SmallInt, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Foroperatore", txtForoperatore.Checked, SqlDbType.Bit, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forcat1", txtForcat1.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forcat2", txtForcat2.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forcat3", txtForcat3.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forcat4", txtForcat4.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forcat5", txtForcat5.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@Forcat6", txtForcat6.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForIBAN", txtForIBAN.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForEsenzioneIvaArt15", txtForEsenzioneIvaArt15.Checked, SqlDbType.Bit, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForPctRivPrev", ForPctRivPrev, SqlDbType.Real, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForPagCodice", txtForPagCodice.Text.Trim(), SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ForInUso", txtForInUso.Checked, SqlDbType.Bit, ParameterDirection.Input));


            if(ViewState["IDMOD"].ToString()=="")            
            {
                        sql = "SELECT * FROM FORNITORI WHERE Forragsoc = '" + txtForragsoc.Text + "' ";
                        DataTable DTCheck = new DataTable();
                        clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                        if (DTCheck.Rows.Count == 0)
                        {
                            //INSERIMENTO
                            
                            
                            
                            sql = @"INSERT INTO FORNITORI 
                            (
                                Forprenome
                                ,Forragsoc
                                ,Forindirizzo
                                ,Forlocalita
                                ,Forprovincia
                                ,ForCAP
                                ,ForNazione
                                ,Forpostaragsoc
                                ,Forpostaind
                                ,Forpostaloc
                                ,ForpostCAP
                                ,ForPostaNazione
                                ,ForPostaProv
                                ,Fortelef,ForEmail,GANote
                                ,Forfax
                                ,ForIVA
                                ,ForCF
                                ,Fordatanascita
                                ,Forluogonascita
                                ,Forpagamento
                                ,Forbanca
                                ,ForPiazza
                                ,ForpercIVA
                                ,Forpercritenuta
                                ,Forpercacconto
                                ,Forpercsuacconto
                                ,Forperc1
                                ,Forperc2
                                ,Forperc3
                                ,Forperc4
                                ,Forperc5
                                ,Forperc6
                                ,Foroperatore
                                ,Forcat1
                                ,Forcat2
                                ,Forcat3
                                ,Forcat4
                                ,Forcat5
                                ,Forcat6
                                ,ForIBAN
                                ,ForEsenzioneIvaArt15
                                ,ForPctRivPrev
                                ,ForPagCodice
                                ,ForInUso
                            ) 
                            VALUES(
                                @Forprenome
                                ,@Forragsoc
                                ,@Forindirizzo
                                ,@Forlocalita
                                ,@Forprovincia
                                ,@ForCAP
                                ,@ForNazione
                                ,@Forpostaragsoc
                                ,@Forpostaind
                                ,@Forpostaloc
                                ,@ForpostCAP
                                ,@ForPostaNazione
                                ,@ForPostaProv
                                ,@Fortelef,@ForEmail,@GANote
                                ,@Forfax
                                ,@ForIVA
                                ,@ForCF
                                ,@Fordatanascita
                                ,@Forluogonascita
                                ,@Forpagamento
                                ,@Forbanca
                                ,@ForPiazza
                                ,@ForpercIVA
                                ,@Forpercritenuta
                                ,@Forpercacconto
                                ,@Forpercsuacconto
                                ,@Forperc1
                                ,@Forperc2
                                ,@Forperc3
                                ,@Forperc4
                                ,@Forperc5
                                ,@Forperc6
                                ,@Foroperatore
                                ,@Forcat1
                                ,@Forcat2
                                ,@Forcat3
                                ,@Forcat4
                                ,@Forcat5
                                ,@Forcat6
                                ,@ForIBAN
                                ,@ForEsenzioneIvaArt15
                                ,@ForPctRivPrev
                                ,@ForPagCodice
                                ,@ForInUso
                                ) ";

                            clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                            if (((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divErroreBox")).Visible == false)
                            {
                                ModificaVisible = false;
                                CaricaDati();
                                
                                gridData.Visible = true;
                                gridData.SelectedIndex = -1;
                            }
                        }
                        else
                        {
                            ShowError = true;
                        }
                    }
        

            if(ViewState["IDMOD"].ToString()!="")            
            {
                ShowError = false;

                //sql = "SELECT * FROM FORNITORI WHERE Forragsoc = '" + txtForragsoc.Text + "' ";
                //sql += " AND Forcodice <> " + gridData.DataKeys[gridData.SelectedIndex];
                //DataTable DTCheck = new DataTable();
                //clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CHECKESISTENZA", ref DTCheck, true);
                //if (DTCheck.Rows.Count > 0)
                //{
                //    ShowError = true;
                //}
           
                if (ShowError == false)
                {
                    //AGGIORNAMENTO
                    
                    pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", gridData.DataKeys[gridData.SelectedIndex].ToString(), SqlDbType.NVarChar, ParameterDirection.Input));

                    sql = "UPDATE FORNITORI "
                    + @" SET 
                        Forprenome = @Forprenome
                        ,Forragsoc = @Forragsoc
                        ,Forindirizzo = @Forindirizzo
                        ,Forlocalita = @Forlocalita
                        ,Forprovincia = @Forprovincia
                        ,ForCAP = @ForCAP
                        ,ForNazione = @ForNazione
                        ,Forpostaragsoc = @Forpostaragsoc
                        ,Forpostaind = @Forpostaind
                        ,Forpostaloc = @Forpostaloc
                        ,ForpostCAP = @ForpostCAP
                        ,ForPostaNazione = @ForPostaNazione
                        ,ForPostaProv = @ForPostaProv
                        ,Fortelef = @Fortelef
                        ,ForEmail = @ForEmail
                        ,GANote = @GANote
                        ,Forfax = @Forfax
                        ,ForIVA = @ForIVA
                        ,ForCF = @ForCF
                        ,Fordatanascita = @Fordatanascita
                        ,Forluogonascita = @Forluogonascita
                        ,Forpagamento = @Forpagamento
                        ,Forbanca = @Forbanca
                        ,ForPiazza = @ForPiazza
                        ,ForpercIVA = @ForpercIVA
                        ,Forpercritenuta = @Forpercritenuta
                        ,Forpercacconto = @Forpercacconto
                        ,Forpercsuacconto = @Forpercsuacconto
                        ,Forperc1 = @Forperc1
                        ,Forperc2 = @Forperc2
                        ,Forperc3 = @Forperc3
                        ,Forperc4 = @Forperc4
                        ,Forperc5 = @Forperc5
                        ,Forperc6 = @Forperc6
                        ,Foroperatore = @Foroperatore
                        ,Forcat1 = @Forcat1
                        ,Forcat2 = @Forcat2
                        ,Forcat3 = @Forcat3
                        ,Forcat4 = @Forcat4
                        ,Forcat5 = @Forcat5
                        ,Forcat6 = @Forcat6
                        ,ForIBAN = @ForIBAN
                        ,ForEsenzioneIvaArt15 = @ForEsenzioneIvaArt15
                        ,ForPctRivPrev = @ForPctRivPrev
                        ,ForPagCodice = @ForPagCodice
                        ,ForInUso = @ForInUso
                    WHERE Forcodice = @ID";

                    clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

                    if (((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divErroreBox")).Visible == false)
                    {
                        ViewState["IDMOD"] = "";
                        ViewState["MODFORN"] = "NO";
                        ModificaVisible = false;
                        CaricaDati();
                        
                        gridData.Visible = true;
                        gridData.SelectedIndex = -1;
                    }
                }
            }
            
            if (ShowError)
            {
                lblError.Text = "Esite già un Fornitore con lo stesso nome.";
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
            sql = "select * from Lista_CategoriaServizi order by cscodice";
            DataTable DTCatego = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "CATEGORIE", ref DTCatego, true);
            for (int i = 0; i < DTCatego.Rows.Count; i++)
            {
                switch (DTCatego.Rows[i]["CScodice"].ToString())
                {
                    case "1": lblPerc1.Text = "Perc. " + DTCatego.Rows[i]["CSdescrizione"].ToString(); break;
                    case "2": lblPerc2.Text = "Perc. " + DTCatego.Rows[i]["CSdescrizione"].ToString(); break;
                    case "3": lblPerc3.Text = "Perc. " + DTCatego.Rows[i]["CSdescrizione"].ToString(); break;
                    case "4": lblPerc4.Text = "Perc. " + DTCatego.Rows[i]["CSdescrizione"].ToString(); break;
                    case "5": lblPerc5.Text = "Perc. " + DTCatego.Rows[i]["CSdescrizione"].ToString(); break;
                    case "6": lblPerc6.Text = "Perc. " + DTCatego.Rows[i]["CSdescrizione"].ToString(); break;
                }
            }

            sql = @"SELECT 
                 (select CSdescrizione from Lista_CategoriaServizi where CScodice = 1) AS CAT1
                ,(select CSdescrizione from Lista_CategoriaServizi where CScodice = 2) AS CAT2
                ,(select CSdescrizione from Lista_CategoriaServizi where CScodice = 3) AS CAT3
                ,(select CSdescrizione from Lista_CategoriaServizi where CScodice = 4) AS CAT4
                ,(select CSdescrizione from Lista_CategoriaServizi where CScodice = 5) AS CAT5
                ,(select CSdescrizione from Lista_CategoriaServizi where CScodice = 6) AS CAT6
                ,FORNITORI.* 
                FROM FORNITORI
            ";

            if (txtFiltroLibero.Text.Trim().Length > 0)
            {
                sql += " WHERE (Forcodice like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR Forprenome like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR ForIVA like '%" + txtFiltroLibero.Text.Trim() + "%' ";
                sql += " OR Forragsoc like '%" + txtFiltroLibero.Text.Trim() + "%' )";
            }

            sql += " ORDER BY Forcodice DESC, Forragsoc ";
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
                ((LinkButton)gridData.Items[i].Cells[COL_ELIMINA].Controls[0]).Attributes.Add("onclick", "if(confirm('Si vuole eliminare il Fornitore selezionato/a ?')){}else{return false}");
                if (sqlControl != "")
                {
                    string sqlControlCODE = sqlControl.Replace("@CODE", DTResult.Rows[i]["Forcodice"].ToString());
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