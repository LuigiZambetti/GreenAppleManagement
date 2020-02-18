using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Collections;
using System.Web.UI.HtmlControls;

/*modificato metodo Execute_Command_ClsParameter */
namespace Green.Apple.Management
{
    #region clsDB

    public class clsDB
    {
        static public string Load_DataTable(Page page, string pConnectionstring, string sqlQuery, string NomeTabella, ref DataTable DTReturn, bool PulisciTabella)
        {
            string Ret = "";
            SqlDataAdapter DtAdp;
            SqlConnection Cn = new SqlConnection(pConnectionstring);
            try
            {
                //pulisco la tabella
                if (PulisciTabella == true)
                {
                    if (DTReturn != null) DTReturn.Clear();
                }
                Cn.Open();
                //
                DtAdp = new SqlDataAdapter(sqlQuery, Cn);
                //DtAdp.MissingSchemaAction = MissingSchemaAction.AddWithKey;
                DtAdp.Fill(DTReturn);
                //
                DtAdp.Dispose();
            }
            catch (System.Exception myError)
            {
                //if (System.Diagnostics.Debugger.IsAttached == true)
                //{ 
                //    //Disabilitato
                //    //System.Diagnostics.Debugger.Break(); 
                //}

                Ret = sqlQuery + "<BR>" + myError.Message.ToString();

                ((Label)((System.Web.UI.HtmlControls.HtmlGenericControl)page.Master.FindControl("divErroreBox")).FindControl("lblError")).Text = Ret;
                ((System.Web.UI.HtmlControls.HtmlGenericControl)page.Master.FindControl("divErroreBox")).Visible = true;
            }
            finally
            {
                Cn.Close();
                Cn.Dispose();
            }
            return Ret;
        }

        static public string Load_DataTable_ClsParameter(Page page, string pConnectionstring, string sqlQuery, ref DataTable DTReturn, List<clsParameter.MemberOfclsParameter> pParameters)
        {
            string Ret = "";
            SqlDataAdapter dtAdap;
            SqlConnection Cnn = new SqlConnection(pConnectionstring);
            SqlCommand Cmd = new SqlCommand();
            try
            {
                Cnn.Open();
                Cmd.Connection = Cnn;
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = sqlQuery;
                for (Int32 I = 0; I <= (pParameters.Count - 1); I++)
                {
                    if (pParameters[I].ArrayName != null && pParameters[I].ArrayName.Trim().Length > 0)
                    {
                        Cmd.Parameters.Add(pParameters[I].ArrayName, pParameters[I].ArrayType);
                        Cmd.Parameters[Cmd.Parameters.Count - 1].Direction = pParameters[I].ArrayDir;
                        Cmd.Parameters[Cmd.Parameters.Count - 1].Value = pParameters[I].ArrayValue;
                    }
                }
                try
                {
                    dtAdap = new SqlDataAdapter();
                    dtAdap.SelectCommand = Cmd;
                    dtAdap.Fill(DTReturn);
                    dtAdap.Dispose();
                }
                catch (System.Exception myError)
                {
                    //if (System.Diagnostics.Debugger.IsAttached == true)
                    //{ System.Diagnostics.Debugger.Break(); }
                    Ret = sqlQuery + "<BR>" + myError.Message.ToString();
                    ((Label)((System.Web.UI.HtmlControls.HtmlGenericControl)page.Master.FindControl("divErroreBox")).FindControl("lblError")).Text = Ret;
                    ((System.Web.UI.HtmlControls.HtmlGenericControl)page.Master.FindControl("divErroreBox")).Visible = true;

                }
                finally
                {
                    Cmd.Dispose();
                    Cnn.Close(); Cnn.Dispose();
                }
            }
            catch
            {
            }
            return Ret;
        }

        static public string Execute_Command(Page page, string pConnectionstring, string sqlQuery)
        {
            string Ret = "";
            SqlConnection Cn = new SqlConnection(pConnectionstring);
            SqlCommand Cmd = new SqlCommand();
            try
            {
                Cn.Open();
                Cmd.Connection = Cn;
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = sqlQuery;
                Cmd.ExecuteReader();
            }
            catch (System.Exception myError)
            {
                //if (System.Diagnostics.Debugger.IsAttached == true)
                //{ System.Diagnostics.Debugger.Break(); }
                Ret = sqlQuery + "<BR>" + myError.Message.ToString();
                ((Label)((System.Web.UI.HtmlControls.HtmlGenericControl)page.Master.FindControl("divErroreBox")).FindControl("lblError")).Text = Ret;
                ((System.Web.UI.HtmlControls.HtmlGenericControl)page.Master.FindControl("divErroreBox")).Visible = true;

            }
            finally
            {
                Cmd.Dispose();
                Cn.Close();
                Cn.Dispose();
            }
            return Ret;
        }

        static public Dictionary<string, string> Execute_Command_ClsParameter(Page page, string pConnectionstring, string sqlQuery, List<clsParameter.MemberOfclsParameter> pParameters)
        {
            Dictionary<string, string> Ret = new Dictionary<string, string>();
            SqlConnection Cnn = new SqlConnection(pConnectionstring);
            SqlCommand Cmd = new SqlCommand();
            bool isNull = false; // aggiunto

            try
            {
                /* differenzia query con o senza parametri */
                isNull = pParameters == null;
                /* query con parametri - START */
                if (!isNull && pParameters.Count > 0)
                {
                    Cnn.Open();
                    Cmd.Connection = Cnn;
                    Cmd.CommandType = CommandType.Text;
                    Cmd.CommandText = sqlQuery;
                    for (Int32 I = 0; I <= (pParameters.Count - 1); I++)
                    {
                        if (pParameters[I].ArrayName != null && pParameters[I].ArrayName.Trim().Length > 0)
                        {
                            Cmd.Parameters.Add(pParameters[I].ArrayName, pParameters[I].ArrayType);
                            Cmd.Parameters[Cmd.Parameters.Count - 1].Direction = pParameters[I].ArrayDir;
                            Cmd.Parameters[Cmd.Parameters.Count - 1].Value = pParameters[I].ArrayValue;
                        }
                    }
                    Cmd.ExecuteNonQuery();

                    for (int i = 0; i <= Cmd.Parameters.Count - 1; i++)
                    {
                        if (Cmd.Parameters[i].Direction == ParameterDirection.InputOutput || Cmd.Parameters[i].Direction == ParameterDirection.Output)
                        {
                            Ret.Add(Cmd.Parameters[i].ParameterName.ToString(), Cmd.Parameters[i].Value.ToString());
                        }
                    }
                }
                /*query con parametri - END */
                /*query senza parametri - START */
                else
                {
                    SqlDataAdapter da = new SqlDataAdapter(sqlQuery, Cnn);
                    DataSet retDS = new DataSet();
                    da.Fill(retDS, "ret");
                    for (int i = 0; i < retDS.Tables["ret"].Rows[0].ItemArray.Length; i++)
                    {
                        Ret.Add(retDS.Tables["ret"].Columns[i].ToString(), retDS.Tables["ret"].Rows[0].ItemArray[i].ToString());
                    }
                }
                /*query senza parametri - END */
            }
            catch (System.Exception myError)
            {
                //MessageBox.Show(sqlQuery + myError.Message);
                //if (System.Diagnostics.Debugger.IsAttached == true && !myError.Message.ToUpper().Contains("IDENTITY_INSERT"))
                //{
                //    System.Diagnostics.Debugger.Break();
                //}
                Cmd.Dispose();
                Ret.Clear();
                Ret.Add("ERRORE", myError.Message.ToString());
                Ret.Add("STACK", myError.StackTrace.ToString()); // Added

                ((Label)((System.Web.UI.HtmlControls.HtmlGenericControl)page.Master.FindControl("divErroreBox")).FindControl("lblError")).Text = myError.Message.ToString();
                ((System.Web.UI.HtmlControls.HtmlGenericControl)page.Master.FindControl("divErroreBox")).Visible = true;

            }
            finally
            {
                Cmd.Dispose();
                Cnn.Close();
                Cnn.Dispose();
            }
            return Ret;
        }

        static public string DatePartGG_MM_AAAA(string DateTimeSel, string NameDate, bool NoAs)
        {
            string strDatePartGG_MM_AAAA = "";
            strDatePartGG_MM_AAAA = " CASE LEN(CAST(DATEPART(d,CONVERT(datetime," + DateTimeSel + ",105)) as nvarchar(2))) ";
            strDatePartGG_MM_AAAA += " WHEN 1 ";
            strDatePartGG_MM_AAAA += " THEN '0' + CAST(DATEPART(d,CONVERT(datetime," + DateTimeSel + ",105)) as nvarchar(2)) ";
            strDatePartGG_MM_AAAA += " ELSE CAST(DATEPART(d,CONVERT(datetime," + DateTimeSel + ",105)) as nvarchar(2))  END ";
            strDatePartGG_MM_AAAA += "  + '/' + ";
            strDatePartGG_MM_AAAA += " CASE LEN(CAST(DATEPART(mm,CONVERT(datetime," + DateTimeSel + ",105)) as nvarchar(2))) ";
            strDatePartGG_MM_AAAA += " WHEN 1 ";
            strDatePartGG_MM_AAAA += " THEN '0' + CAST(DATEPART(mm,CONVERT(datetime," + DateTimeSel + ",105)) as nvarchar(2)) ";
            strDatePartGG_MM_AAAA += " ELSE CAST(DATEPART(mm,CONVERT(datetime," + DateTimeSel + ",105)) as nvarchar(2))  END ";
            strDatePartGG_MM_AAAA += " + '/' + ";
            strDatePartGG_MM_AAAA += " CAST(DATEPART(yyyy,CONVERT(datetime," + DateTimeSel + ",105)) as nvarchar(4)) ";

            if (NoAs == false) strDatePartGG_MM_AAAA += " as " + NameDate + " ";
            return (strDatePartGG_MM_AAAA);
        }

        static public string ExecuteQueryScalar(Page page, string pConnectionstring, string sqlQuery)
        {
            string Ret = "";
            SqlConnection Cn = new SqlConnection(pConnectionstring);
            SqlCommand Cmd = new SqlCommand();
            try
            {
                Cn.Open();
                Cmd.Connection = Cn;
                Cmd.CommandType = CommandType.Text;
                Cmd.CommandText = sqlQuery;
                object obj = Cmd.ExecuteScalar();
                Ret = obj.ToString();
            }
            catch (System.Exception myError)
            {
                //if (System.Diagnostics.Debugger.IsAttached == true)
                //{ System.Diagnostics.Debugger.Break(); }
                Ret = myError.Message.ToString();
                ((Label)((System.Web.UI.HtmlControls.HtmlGenericControl)page.Master.FindControl("divErroreBox")).FindControl("lblError")).Text = Ret;
                ((System.Web.UI.HtmlControls.HtmlGenericControl)page.Master.FindControl("divErroreBox")).Visible = true;


            }
            finally
            {
                Cmd.Dispose();
                Cn.Close();
            }
            return Ret;
        }
    }
    #endregion

    #region clsPARAMETER
    /// <summary>
    /// Classe ponte di parameter SQL (usata da clsDB)
    /// </summary>
    public class clsParameter
    {
        public class MemberOfclsParameter
        {
            public string ArrayName;
            public object ArrayValue;
            public SqlDbType ArrayType;
            public ParameterDirection ArrayDir;
            public bool Includi = true;
            public MemberOfclsParameter()
            { }
            public MemberOfclsParameter(string _ArrayName, object _ArrayValue, SqlDbType _ArrayType, ParameterDirection _ArrayDir)
            {
                ArrayName = _ArrayName;
                if (_ArrayValue == null)
                {
                    ArrayValue = DBNull.Value;
                }
                else
                {
                    ArrayValue = _ArrayValue;
                }
                ArrayType = _ArrayType;
                ArrayDir = _ArrayDir;
            }
            public MemberOfclsParameter(string _ArrayName, object _ArrayValue, SqlDbType _ArrayType, ParameterDirection _ArrayDir, bool _Includi)
            {
                ArrayName = _ArrayName;
                if (_ArrayValue == null)
                {
                    ArrayValue = DBNull.Value;
                }
                else
                {
                    ArrayValue = _ArrayValue;
                }
                ArrayType = _ArrayType;
                ArrayDir = _ArrayDir;
                Includi = _Includi;
            }
        }
        public List<MemberOfclsParameter> Parameters = new List<MemberOfclsParameter>();
    }
    #endregion

    #region clsSESSION
    public class clsSession
    {
        public enum AzioniPagina
        {
            Vuoto,
            Inserimento,
            Modifica,
            Dettaglio,
            Elimina,
            ModificaNoCheck
        }
        public clsSession()
        {
            CnnStr = ConfigurationManager.AppSettings["CnnStr"].ToString();
        }

        public string Error = string.Empty;

        private AzioniPagina _AzioneCorrente = AzioniPagina.Vuoto;
        public string CnnStr = string.Empty;
        public string IDUtente = string.Empty;
        public string SimpleList = string.Empty;
        public string Login = string.Empty;
        public string Password = string.Empty;


        public string SITE_Colore = string.Empty;
        public string SITE_Colore_1 = string.Empty;
        public string SITE_Colore_2 = string.Empty;
        public string SITE_Colore_3 = string.Empty;
        public string SITE_Colore_4 = string.Empty;
        public string SITE_Colore_5 = string.Empty;

        public clsFiltriRicerca FiltriRicerca = new clsFiltriRicerca();
        public AzioniPagina AzioneCorrente
        {
            get
            {
                return _AzioneCorrente;
            }
            set
            {
                _AzioneCorrente = value;
            }
        }
        public class clsFiltriRicerca
        {
            public string RicercaLibera = string.Empty;
            public string RicercaDocumenti = string.Empty;
            public string StatoPratica = "0";
            public DateTime DataDal = DateTime.MinValue;
            public DateTime DataAl = DateTime.MinValue;
            public int IdSocieta = 0;
            public int idMateria = 0;
            public int idTestata = 0;
            public int IdLegaleEsterno = 0;
            public int IdTipoGrado = 0;
            public int IdEsito = 0;
            public int TopElement = 20;
            public bool VisControParte = true;
            public bool VisOggetto = false;
            public bool VisMateria = false;
            public bool VisSocieta = true;
            public bool VisGrado = true;
            public bool VisDocumenti = true;
        }

        public clsFiltriCollegate FiltriCollegate = new clsFiltriCollegate();
        public class clsFiltriCollegate
        {
            public string ANNO = "";
            public string TIPO = "";
            public string TESTO = "";
        }

        public clsReports ReportCollegato = new clsReports();
        public class clsReports
        {
            public string URI = "";
            public string NOME = "";
            public string DESCRIZIONE = "";
            public string PATH = "";

            public string UTENTE = "";
            public string PASSWORD = "";
            public string DOMINIO = "";
        }
    }
    #endregion

    #region clsQUERYSTRING
    /// <summary>
    /// Classe contenente metodi x interrogare il Querystring
    /// </summary>
    public class clsQueryString
    {
        static public int IdCorrente
        {
            get
            {
                int Ret = 0;
                if (HttpContext.Current.Request.QueryString["ID"] != null)
                {
                    try
                    {
                        Ret = Convert.ToInt32(HttpContext.Current.Request.QueryString["ID"]);
                    }
                    catch { }
                }
                return Ret;
            }
        }
        static public int IdProcedimento
        {
            get
            {
                int Ret = 0;
                if (HttpContext.Current.Request.QueryString["IDP"] != null)
                {
                    try
                    {
                        Ret = Convert.ToInt32(HttpContext.Current.Request.QueryString["IDP"]);
                    }
                    catch { }
                }
                return Ret;
            }
        }
        static public int IdMandato
        {
            get
            {
                int Ret = 0;
                if (HttpContext.Current.Request.QueryString["IDM"] != null)
                {
                    try
                    {
                        Ret = Convert.ToInt32(HttpContext.Current.Request.QueryString["IDM"]);
                    }
                    catch { }
                }
                return Ret;
            }
        }
        static public int IdMovimento
        {
            get
            {
                int Ret = 0;
                if (HttpContext.Current.Request.QueryString["IDMOV"] != null)
                {
                    try
                    {
                        Ret = Convert.ToInt32(HttpContext.Current.Request.QueryString["IDMOV"]);
                    }
                    catch { }
                }
                return Ret;
            }
        }
        static public int GradoCorrente
        {
            get
            {
                int Ret = 0;
                if (HttpContext.Current.Request.QueryString["GG"] != null)
                {
                    try
                    {
                        Ret = Convert.ToInt32(HttpContext.Current.Request.QueryString["GG"]);
                    }
                    catch { }
                }
                return Ret;
            }
        }
        static public string GradoDescrizione
        {
            get
            {
                string Ret = "";
                if (HttpContext.Current.Request.QueryString["GD"] != null)
                {
                    try
                    {
                        Ret = HttpContext.Current.Request.QueryString["GD"];
                    }
                    catch { }
                }
                return Ret;
            }
        }

        static public string TipoPratica
        {
            get
            {
                string Ret = "";
                //if (IdCorrente > 0)
                //{
                //    string sql = " SELECT TIPOPRATICA FROM PRATICACONTENZIOSO WHERE IDPRATICA=" + IdCorrente.ToString() + "";
                //    Ret = clsDB.ExecuteQueryScalar(((clsSession)HttpContext.Current.Session["GreenApple"]).CnnStr, sql);
                //}
                //else
                //{
                if (HttpContext.Current.Request.QueryString["TP"] != null)
                {
                    try
                    {
                        Ret = HttpContext.Current.Request.QueryString["TP"].ToString();
                    }
                    catch { }
                }
                //}
                return Ret;
            }
        }
        static public string Sezione
        {
            get
            {
                string Ret = "";
                if (HttpContext.Current.Request.QueryString["SEZ"] != null)
                {
                    try
                    {
                        Ret = HttpContext.Current.Request.QueryString["SEZ"].ToString();
                    }
                    catch { }
                }
                return Ret;
            }
        }



    }
    #endregion

    #region clsCOSTANTI
    /// <summary>
    /// Classe contenente Costanti Applicative
    /// </summary>
    public class clsCostanti
    {
        static public string ComboNessunaSelezione = "-- Nessuna Selezione --";
        static public string PraticaAttiva = "Attiva";
        static public string PraticaPassiva = "Passiva";
        static public string FormatNumber = "{0:#,0.00}";
        static public string FormatNumberNoDecimal = "{0:#,0}";
        static public string FormatNumberNoMigliaia = "{0:0.00}";
        static public string NonAncoraSpecificato = "Non ancora specificato";
        static public string NonAncoraSpecificata = "Non ancora specificata";
        static public string NonSpecificato = "Non specificato";
        static public string NonSpecificata = "Non specificata";
        static public string RootUserControls = ConfigurationManager.AppSettings["RootUserControls"].ToString();
        public const string SI = "Sì";
        public const string NO = "No";
        static public string TipoRubrica_LegaleEsterno = "LEGALE ESTERNO";
        static public string TipoRubrica_LegaleInterno = "LEGALE INTERNO";


    }
    #endregion

    #region clsFUNCTIONS
    /// <summary>
    /// Classe contenente metodi statici di utilizzo globale
    /// </summary>
    public class clsFunctions
    {
        static public void InitThread()
        {
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("IT-it");
        }
        static public int RetIndexValueCombo(string pValue, System.Web.UI.WebControls.DropDownList pCombo)
        {
            int Ret = 0;
            for (int i = 0; i <= pCombo.Items.Count - 1; i++)
            {
                if (pCombo.Items[i].Value.ToString() == pValue)
                {
                    Ret = i;
                    break;
                }
            }
            return Ret;
        }
        static public int RetIndexTextCombo(string pText, System.Web.UI.WebControls.DropDownList pCombo)
        {
            int Ret = 0;
            for (int i = 0; i <= pCombo.Items.Count - 1; i++)
            {
                if (pCombo.Items[i].Text.ToString() == pText)
                {
                    Ret = i;
                    break;
                }
            }
            return Ret;
        }

        static public void AssegnaEventoCalendar(System.Web.UI.WebControls.Image imgData, System.Web.UI.WebControls.TextBox txtData, bool SetNowOnTxt)
        {
            string formatDateStr = ConfigurationManager.AppSettings["DateFormat"].ToString();
            imgData.Attributes.Add("onclick", "showCalendar(this, document.getElementsByName('" + txtData.UniqueID.ToString() + "')[0], '" + formatDateStr + "','it',1,1)");
            imgData.Style.Add("cursor", "pointer");
            //txtData.Attributes["onkeypress"] = "return false;";
            //txtData.Attributes["onkeydown"] = "return false;";
            if (SetNowOnTxt)
                txtData.Text = DateTime.Now.ToShortDateString();
        }
        static public string RetScriptWindowOpen(string pUrl, int pWidth, int pHeight, bool pScrollbars, bool pStatusBar, bool pToolbar, bool pLocationBar, bool pMenuBar, bool pResizable, bool pBorder)
        {
            string ret = "";
            string parameters = "";
            ret += "X=" + pWidth.ToString() + ";";
            ret += "Y=" + pHeight.ToString() + ";";
            ret += "L = (screen.availWidth/2) - (X/2);";
            ret += "T = (screen.availHeight/2) - (Y/2);";
            ret += " window.open('" + pUrl + "',null,'";
            parameters += "width=' + X + ' ";
            parameters += ",height=' + Y + ' ";
            parameters += ",left=' + L + ' ";
            parameters += ",top=' + T + ' ";
            if (pScrollbars)
            {
                parameters += ",scrollbars=yes";
            }
            else
            {
                parameters += ",scrollbars=no";
            }
            if (pStatusBar)
            {
                parameters += ",status=yes";
            }
            else
            {
                parameters += ",status=no";
            }
            if (pToolbar)
            {
                parameters += ",toolbar=yes";
            }
            else
            {
                parameters += ",toolbar=no";
            }
            if (pLocationBar)
            {
                parameters += ",location=yes";
            }
            else
            {
                parameters += ",location=no";
            }
            if (pMenuBar)
            {
                parameters += ",menubar=yes";
            }
            else
            {
                parameters += ",menubar=no";
            }
            if (pResizable)
            {
                parameters += ",resizable=yes";
            }
            else
            {
                parameters += ",resizable=no";
            }
            if (pBorder)
            {
                parameters += ",border=yes";
            }
            else
            {
                parameters += ",border=no";
            }
            ret += parameters + "').focus();";
            return ret;
        }
        static public string RetScriptWindowOpenFullScreen(string pUrl, int pWidth, int pHeight, bool pScrollbars, bool pStatusBar, bool pToolbar, bool pLocationBar, bool pMenuBar, bool pResizable, bool pBorder)
        {
            string ret = "";
            string parameters = "";
            ret += "X=" + pWidth.ToString() + ";";
            ret += "Y=" + pHeight.ToString() + ";";
            ret += "L = (screen.availWidth/2) - (X/2);";
            ret += "T = (screen.availHeight/2) - (Y/2);";
            ret += " window.open('" + pUrl + "',null,'";

            parameters += "width='+screen.width +'";
            parameters += ", height='+screen.height +'";
            parameters += ", top=0, left=0";
            parameters += ", fullscreen=yes";


            if (pScrollbars)
            {
                parameters += ",scrollbars=yes";
            }
            else
            {
                parameters += ",scrollbars=no";
            }
            if (pStatusBar)
            {
                parameters += ",status=yes";
            }
            else
            {
                parameters += ",status=no";
            }
            if (pToolbar)
            {
                parameters += ",toolbar=yes";
            }
            else
            {
                parameters += ",toolbar=no";
            }
            if (pLocationBar)
            {
                parameters += ",location=yes";
            }
            else
            {
                parameters += ",location=no";
            }
            if (pMenuBar)
            {
                parameters += ",menubar=yes";
            }
            else
            {
                parameters += ",menubar=no";
            }
            if (pResizable)
            {
                parameters += ",resizable=yes";
            }
            else
            {
                parameters += ",resizable=no";
            }
            if (pBorder)
            {
                parameters += ",border=yes";
            }
            else
            {
                parameters += ",border=no";
            }
            ret += parameters + "').focus();";
            return ret;
        }
        public static void WriteGOTO_and_FOCUS(string p, Control txtElemento, System.Web.UI.Page PageCall)
        {
            if (txtElemento != null)
            {
                PageCall.ClientScript.RegisterStartupScript(typeof(string), "scriptGOTOandFOCUS", "<script>window.setTimeout(\"document.location='#" + p + "';document.getElementById('" + txtElemento.ClientID + "').select();\",300);<" + "/" + "script>");
            }
            else
            {
                PageCall.ClientScript.RegisterStartupScript(typeof(string), "scriptGOTOandFOCUS", "<script>window.setTimeout(\"document.location='#" + p + "';\",300);<" + "/" + "script>");
            }
        }
        public static void CaricaLettere(PlaceHolder phLettere, EventHandler lnkNew_Click, string StartUpFilter)
        {
            phLettere.Controls.Clear();

            ArrayList pArray = new ArrayList();

            for (int I = 65; I <= 90; I++)
            {
                pArray.Add(I);//CARICO TUTTE LE LETTERE DELL'ALFABETO
            }

            pArray.Add(48);//0
            pArray.Add(49);//1
            pArray.Add(50);//2
            pArray.Add(51);//3
            pArray.Add(52);//4
            pArray.Add(53);//5
            pArray.Add(54);//6
            pArray.Add(55);//7
            pArray.Add(56);//8
            pArray.Add(57);//9

            //pArray.Add(38);//  & 

            //pArray.Add(42);//  * 
            //pArray.Add(43);//  +
            //pArray.Add(44);//  ,
            //pArray.Add(45);//  -
            //pArray.Add(46);//  .
            //pArray.Add(47);//  /


            //pArray.Add(32);//SPAZIO


            for (int I = 0; I <= pArray.Count - 1; I++)
            {

                string strA = new string((Char)int.Parse(pArray[I].ToString()), 1);

                LinkButton lnkNew = new LinkButton();
                lnkNew.Attributes["VALUE"] = strA;

                //switch (strA)
                //{
                //    case " ":
                //        lnkNew.Text = "SPAZIO";
                //        break;
                //    case "*":
                //        lnkNew.Text = "ASTERISCO";
                //        break;
                //    case "+":
                //        lnkNew.Text = "PIU";
                //        break;
                //    case ",":
                //        lnkNew.Text = "VIRGOLA";
                //        break;
                //    case "-":
                //        lnkNew.Text = "MENO";
                //        break;
                //    case ".":
                //        lnkNew.Text = "PUNTO";
                //        break;
                //    case "/":
                //        lnkNew.Text = "SLASH";
                //        break;
                //    default:
                //        lnkNew.Text = strA;
                //        break;
                //}

                lnkNew.Text = strA;
                lnkNew.ID = "lnk" + lnkNew.Text;
                lnkNew.Font.Bold = false;
                lnkNew.Font.Underline = false;
                lnkNew.Font.Size = 8;
                lnkNew.Click += new EventHandler(lnkNew_Click);

                phLettere.Controls.Add(lnkNew);

                Label lblSpace = new Label();
                lblSpace.Text = "&nbsp;&nbsp;";
                //if (I == 25) { lblSpace.Text += "<br>"; }
                phLettere.Controls.Add(lnkNew);
                phLettere.Controls.Add(lblSpace);

                if (StartUpFilter == strA)
                {
                    lnkNew.Font.Bold = true;
                    lnkNew.BorderStyle = BorderStyle.Solid;
                    lnkNew.BorderWidth = 1;
                    lnkNew.Font.Size = 10;
                }
                lnkNew.Height = 20;
            }
        }
        public static void SelezionaLetteraCorrente(PlaceHolder phLettere, object sender)
        {
            for (int i = 0; i <= phLettere.Controls.Count - 1; i++)
            {
                if (phLettere.Controls[i].GetType() == typeof(LinkButton))
                {
                    ((LinkButton)phLettere.Controls[i]).Font.Size = 8;
                    ((LinkButton)phLettere.Controls[i]).Font.Bold = false;
                    ((LinkButton)phLettere.Controls[i]).BorderWidth = 0;
                    ((LinkButton)phLettere.Controls[i]).BorderStyle = BorderStyle.None;
                }
            }
            ((LinkButton)sender).Font.Size = 10;
            ((LinkButton)sender).Font.Bold = true;
            ((LinkButton)sender).BorderStyle = BorderStyle.Solid;
            ((LinkButton)sender).BorderWidth = 1;
        }



        private static void DisabilitaControlsRecursive(ControlCollection Controls)
        {
            foreach (Control pControl in Controls)
            {
                try
                {
                    if (((WebControl)pControl).Attributes["NODISABLE"] != null && ((WebControl)pControl).Attributes["NODISABLE"] == "1")
                    {
                        return;
                    }
                }
                catch { }
                if (pControl.GetType() == typeof(LinkButton))
                {
                    ((LinkButton)pControl).Enabled = false;
                    ((LinkButton)pControl).Attributes["onclick"] = "";
                }

                if (pControl.GetType().Name == "DataGridLinkButton")
                {
                    ((LinkButton)pControl).Enabled = false;
                    ((LinkButton)pControl).Attributes["onclick"] = "";
                }

                if (pControl.GetType() == typeof(FileUpload))
                {
                    ((FileUpload)pControl).Enabled = false;
                }

                DisabilitaControlsRecursive(pControl.Controls);
            }
        }



        #region TRYCONVERT
        static public object TryConvertToDate(object pEntry, object DefaultValue)
        {
            try
            {
                return Convert.ToDateTime(pEntry).ToShortDateString();
            }
            catch
            {
                return DefaultValue;
            }
        }
        static public DateTime TryConvertToDateTime(object pEntry, DateTime DefaultValue)
        {
            try
            {
                return Convert.ToDateTime(pEntry);
            }
            catch
            {
                return DefaultValue;
            }
        }
        static public Nullable<DateTime> TryConvertToDateTime(object pEntry, Nullable<DateTime> DefaultValue)
        {
            try
            {
                return Convert.ToDateTime(pEntry);
            }
            catch
            {
                return DefaultValue;
            }
        }
        static public string TryConvertToShortDate(object pEntry, string DefaultValue)
        {
            try
            {
                return Convert.ToDateTime(pEntry).ToShortDateString();
            }
            catch
            {
                return DefaultValue;
            }
        }
        static public string TryConvertToLongDate(object pEntry, string DefaultValue)
        {
            try
            {
                return Convert.ToDateTime(pEntry).ToLongDateString();
            }
            catch
            {
                return DefaultValue;
            }
        }
        static public string TryConvertToLongTime(object pEntry, string DefaultValue)
        {
            try
            {
                return Convert.ToDateTime(pEntry).ToLongTimeString();
            }
            catch
            {
                return DefaultValue;
            }
        }
        static public string TryConvertToSIorNO(object pEntry, string DefaultValue)
        {
            bool Ret = false;
            try
            {
                Ret = Convert.ToBoolean(pEntry);
                if (Ret)
                {
                    return clsCostanti.SI;
                }
                else
                {
                    return clsCostanti.NO;
                }
            }
            catch
            {
                return DefaultValue;
            }
        }
        static public double TryConvertToDouble(object pEntry, double DefaultValue)
        {
            double Ret = DefaultValue;
            try
            {
                Ret = Convert.ToDouble(pEntry);
            }
            catch
            {
            }
            return Ret;
        }
        static public Int32 TryConvertToInt32(object pEntry, Int32 DefaultValue)
        {
            Int32 Ret = DefaultValue;
            try
            {
                Ret = Convert.ToInt32(pEntry);
            }
            catch
            {
            }
            return Ret;
        }
        static public bool TryConvertToBoolean(object pEntry, bool DefaultValue)
        {
            bool Ret = DefaultValue;
            try
            {
                Ret = Convert.ToBoolean(pEntry);
            }
            catch
            {
            }
            return Ret;
        }

        #endregion



    }
    #endregion

    #region clsPAGINING
    /// <summary>
    /// Classe contenente metodi per Customizzare il Pagining dell'oggetto DataGrid
    /// </summary>
    public class clsPagining
    {
        public delegate void CustomPaginingEventHandler(object sender, CustomPaginingArgs e);
        public event CustomPaginingEventHandler PageChange;
        protected virtual void OnPageChange(CustomPaginingArgs e)
        {
            // solo se l'evento non è null
            if (PageChange != null)
            {
                //Scateno l'evento nel form genitore
                PageChange(this, e);
            }
        }
        public void internal_PageIndexChanged(object source, EventArgs e)
        {
            CustomPaginingArgs pArgs = new CustomPaginingArgs();
            try
            {
                pArgs.NewPage = int.Parse(((LinkButton)source).Attributes["NUMPAGINA"].ToString()) - 1;
            }
            catch
            {
                pArgs.NewPage = int.Parse(((DropDownList)source).SelectedValue.ToString()) - 1;
            }
            OnPageChange(pArgs);
        }
        public void CaricaPagining(PlaceHolder phPagine, DataGrid DGR)
        {
            //FAI UNA CHIAMATA IN PAGELOAD SE IS POSTBACK = TRUE E UNA DOPO IL DATABIND DELL GRIGLIA
            int startNum = 0;
            int endNum = 0;
            startNum = DGR.CurrentPageIndex - 3;
            endNum = DGR.CurrentPageIndex + 3;
            if (startNum < 0) { startNum = 0; }
            if (endNum > DGR.PageCount) { endNum = DGR.PageCount; }
            Label plbl;
            phPagine.Controls.Clear();
            LinkButton pLinkPaginaPrec = new LinkButton();
            LinkButton pLinkPaginaSucc = new LinkButton();
            LinkButton pLinkPaginaInizio = new LinkButton();
            LinkButton pLinkPaginaFine = new LinkButton();
            LinkButton pLinkPaginaCorrente = new LinkButton();
            LinkButton pPagina;
            if (DGR.Items.Count > 0)
            {
                ////////////////
                //PRIMA PAGINA//
                pPagina = new LinkButton();
                pPagina.ID = DGR.ClientID + "_" + "lnkInizioPagina";
                pPagina.Text = "&lt;&lt;&nbsp;" + "Inizio" + "&nbsp;&nbsp;&nbsp;";
                pPagina.ToolTip = "Vai alla prima pagina";
                int Valore = 1;
                pPagina.Attributes.Add("NUMPAGINA", Valore.ToString());
                pPagina.CssClass = "webgridCustomPagerStyleDefault";
                pPagina.Attributes.Add("NOTRAD", "YES");
                pPagina.Click += (internal_PageIndexChanged);
                phPagine.Controls.Add(pPagina);
                if (DGR.CurrentPageIndex == 0)
                {
                    pPagina.Enabled = false;
                    pLinkPaginaInizio = pPagina;
                }
                //--------------------------------------------
                /////////////////////
                //PAGINA PRECEDENTE//
                pPagina = new LinkButton();
                pPagina.ID = DGR.ClientID + "_" + "lnkPrecedentePagina";
                pPagina.Text = "&lt;&nbsp;" + "Precedente" + "&nbsp;";
                pPagina.ToolTip = "Vai alla pagina precedente";
                Valore = DGR.CurrentPageIndex;
                pPagina.Attributes.Add("NUMPAGINA", Valore.ToString());
                pPagina.CssClass = "webgridCustomPagerStyleDefault";
                pPagina.Attributes.Add("NOTRAD", "YES");
                pPagina.Click += (internal_PageIndexChanged);
                phPagine.Controls.Add(pPagina);
                if (DGR.CurrentPageIndex == 0)
                {
                    pPagina.Enabled = false;
                    pLinkPaginaPrec = pPagina;
                }
                plbl = new Label();
                plbl.ID = DGR.ClientID + "_" + "lblPrecedenteSpazio";
                plbl.Text = "&nbsp;&nbsp;";
                plbl.Attributes.Add("NOTRAD", "YES");
                plbl.CssClass = "webgridCustomPagerStyleDefault";
                phPagine.Controls.Add(plbl);
                //--------------------------------------------
                ///////////////////
                //PAGINA DIRETTA//
                for (int i = startNum + 1; i <= endNum; i++)
                {
                    pPagina = new LinkButton();
                    pPagina.ID = DGR.ClientID + "_" + "lnkPagina" + i.ToString();
                    pPagina.Text = i.ToString();
                    pPagina.ToolTip = "Vai alla pagina " + i.ToString();
                    pPagina.Attributes.Add("NUMPAGINA", i.ToString());
                    pPagina.CssClass = "webgridCustomPagerStyleDefault";
                    pPagina.Attributes.Add("NOTRAD", "YES");
                    pPagina.Click += (internal_PageIndexChanged);
                    if (DGR.CurrentPageIndex == i - 1)
                    {
                        pPagina.Enabled = false;
                        pLinkPaginaCorrente = pPagina;
                    }
                    phPagine.Controls.Add(pPagina);
                    plbl = new Label();
                    plbl.ID = DGR.ClientID + "_" + "lblSpazio" + i.ToString();
                    if (i < endNum)
                    {
                        plbl.Text = "&nbsp;-&nbsp;";
                    }
                    plbl.Attributes.Add("NOTRAD", "YES");
                    plbl.CssClass = "webgridCustomPagerStyleDefault";
                    phPagine.Controls.Add(plbl);
                }
                //--------------------------------------------
                //////////////////////
                //PAGINA SUCCESSIVA//
                plbl = new Label();
                plbl.ID = DGR.ClientID + "_" + "lblSuccessivaSpazio";
                plbl.Text = "&nbsp;&nbsp;";
                plbl.Attributes.Add("NOTRAD", "YES");
                plbl.CssClass = "webgridCustomPagerStyleDefault";
                phPagine.Controls.Add(plbl);
                pPagina = new LinkButton();
                pPagina.ID = DGR.ClientID + "_" + "lnkSuccessuvaPagina";
                pPagina.Text = "" + "Successiva" + "&nbsp;&gt;";
                pPagina.ToolTip = "Vai alla pagina successiva";
                Valore = DGR.CurrentPageIndex + 2;
                pPagina.Attributes.Add("NUMPAGINA", Valore.ToString());
                pPagina.CssClass = "webgridCustomPagerStyleDefault";
                pPagina.Attributes.Add("NOTRAD", "YES");
                pPagina.Click += (internal_PageIndexChanged);
                phPagine.Controls.Add(pPagina);
                if (DGR.CurrentPageIndex == DGR.PageCount - 1)
                {
                    pPagina.Enabled = false;
                    pLinkPaginaSucc = pPagina;
                }
                //--------------------------------------------
                //////////////////
                //ULTIMA PAGINA//
                pPagina = new LinkButton();
                pPagina.ID = DGR.ClientID + "_" + "lnkFinePagina";
                pPagina.Text = "&nbsp;&nbsp;&nbsp;" + "Fine" + "&nbsp;&gt;&gt;";
                pPagina.ToolTip = "Vai all'ultima pagina";
                Valore = DGR.PageCount;
                pPagina.Attributes.Add("NUMPAGINA", Valore.ToString());
                pPagina.CssClass = "webgridCustomPagerStyleDefault";
                pPagina.Attributes.Add("NOTRAD", "YES");
                pPagina.Click += (internal_PageIndexChanged);
                phPagine.Controls.Add(pPagina);
                //--------------------------------------------
                if (DGR.CurrentPageIndex == DGR.PageCount - 1)
                {
                    pPagina.Enabled = false;
                    pLinkPaginaFine = pPagina;
                }
                if (DGR.PageCount > 1)
                {
                    ///////////////////////////
                    //COMBO SELETTORE PAGINA//
                    plbl = new Label();
                    plbl.ID = DGR.ClientID + "_" + "lblComboSpazio";
                    plbl.Text = "&nbsp;&nbsp;Vai a:&nbsp;";
                    plbl.Attributes.Add("NOTRAD", "YES");
                    plbl.CssClass = "webgridCustomPagerStyleDefault";
                    phPagine.Controls.Add(plbl);
                    DropDownList pCboPages = new DropDownList();
                    pCboPages.ID = DGR.ClientID + "_" + "cboPages";
                    pCboPages.AutoPostBack = true;
                    for (int i = 1; i <= DGR.PageCount; i++)
                    {
                        pCboPages.Items.Add(new ListItem(i.ToString(), i.ToString()));
                    }
                    pCboPages.SelectedIndexChanged += (internal_PageIndexChanged);
                    pCboPages.SelectedIndex = DGR.CurrentPageIndex;
                    phPagine.Controls.Add(pCboPages);
                    //--------------------------------------------
                }
                ///////////////////////////////////
                //PIE DI GRIGLIA ELEMENTI TROVATI//
                plbl = new Label();
                plbl.ID = DGR.ClientID + "_" + "lblNumElementiPagine";
                string pElementoElementi = "";
                if (DGR.Items.Count > 1)
                {
                    pElementoElementi = "" + "Elementi in totale" + "";
                }
                else
                {
                    pElementoElementi = "" + "Elemento in totale" + "";
                }
                //AGGIUNTO DA STEFANO per Home e DataSource corretti
                if (DGR.DataSource != null)
                {
                    string DataSourceType = DGR.DataSource.GetType().ToString().ToUpper();
                    switch (DataSourceType)
                    {
                        case "SYSTEM.DATA.DATAVIEW":
                            {
                                try
                                {
                                    DGR.Attributes["NUMELEMENTI"] = ((DataView)DGR.DataSource).Count.ToString();
                                }
                                catch { }
                                break;
                            }
                        case "SYSTEM.DATA.DATATABLE":
                            {
                                try
                                {
                                    DGR.Attributes["NUMELEMENTI"] = ((DataTable)DGR.DataSource).Rows.Count.ToString();
                                }
                                catch { }
                                break;
                            }
                    }
                }
                try
                {
                    if (DGR.AllowPaging == false)
                    {
                        plbl.Text = "<br>&nbsp;&nbsp;" + pElementoElementi + "&nbsp;" + DGR.Attributes["NUMELEMENTI"].ToString();
                    }
                    else
                    {
                        plbl.Text = "<br>&nbsp;&nbsp;(" + "Pagina" + "&nbsp;" + pLinkPaginaCorrente.Attributes["NUMPAGINA"].ToString() + "&nbsp;" + "di" + "&nbsp;" + DGR.PageCount.ToString() + ",&nbsp;" + DGR.Attributes["NUMELEMENTI"].ToString() + "&nbsp;" + pElementoElementi + ")&nbsp;&nbsp;";
                    }


                }
                catch
                {
                }
                plbl.Attributes.Add("NOTRAD", "YES");
                plbl.CssClass = "webgridCustomPagerMessageStyleDefault";
                phPagine.Controls.Add(plbl);
                //--------------------------------------------
                //SE NO PAGINE NASCONDO TUTTO
                if (pLinkPaginaSucc.Enabled == false && pLinkPaginaPrec.Enabled == false)
                {
                    pLinkPaginaSucc.Visible = false;
                    pLinkPaginaPrec.Visible = false;
                    pLinkPaginaInizio.Visible = false;
                    pLinkPaginaFine.Visible = false;
                    pLinkPaginaCorrente.Visible = false;
                    plbl.Text = plbl.Text.Replace("<br>", "");
                }
            }
            else
            {
                ///////////////////////////////////
                //PIE DI GRIGLIA NESSUN ELEMENTO//
                plbl = new Label();
                plbl.ID = DGR.ClientID + "_" + "lblNessunElementiPagine";
                plbl.Text = "" + "Nessun elemento presente" + "";
                plbl.Attributes.Add("NOTRAD", "YES");
                plbl.CssClass = "webgridCustomPagerMessageStyleDefault";
                phPagine.Controls.Add(plbl);
                //--------------------------------------------
            }
        }
        public class CustomPaginingArgs : EventArgs
        {
            public int NewPage = 0;
        }
    }
    #endregion

    #region CLASSI BASE USER CONTROL
    public class Green_BaseUserControl : System.Web.UI.UserControl
    {
        protected internal void Page_Load(object sender, EventArgs e)
        {

            if (Session["GreenApple"] == null) Session["GreenApple"] = new clsSession();

            ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divMessageBox")).Visible = false;
            ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divErroreBox")).Visible = false;

            if (!this.Page.ClientScript.IsStartupScriptRegistered(typeof(string), "divScript"))
            {
                string pScript = "<script>"
                + "SetPositionCenterDivPopUp(document.getElementById('" + ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divMessageBox")).ClientID.ToString() + "'),512,160);GetStatusModalDivAbsPos();"
                + "SetHandleDrag(document.getElementById('" + ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divMessageBox")).ClientID.ToString() + "'),document.getElementById('" + ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divMessageBox")).ClientID.ToString() + "'))"
                + "</script>";
                this.Page.ClientScript.RegisterStartupScript(typeof(string), "divScript", pScript);
            }

            if (!this.Page.ClientScript.IsStartupScriptRegistered(typeof(string), "divScriptError"))
            {
                string pScript = "<script>"
                + "SetPositionCenterDivPopUp(document.getElementById('" + ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divErroreBox")).ClientID.ToString() + "'),512,160);GetStatusModalDivAbsPos();"
                + "SetHandleDrag(document.getElementById('" + ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divErroreBox")).ClientID.ToString() + "'),document.getElementById('" + ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divErroreBox")).ClientID.ToString() + "'))"
                + "</script>";
                this.Page.ClientScript.RegisterStartupScript(typeof(string), "divScriptError", pScript);
            }

            ((LinkButton)this.Page.Master.FindControl("lnkConfermaAzione")).Click += new EventHandler(lnkConfermaAzione_Click);

            string URL = "#";
            ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("PRINTReport")).Attributes.Add("src", URL);
        }

        protected void lnkConfermaAzione_Click(object sender, EventArgs e)
        {
            ((clsSession)Session["GreenApple"]).AzioneCorrente = clsSession.AzioniPagina.Dettaglio;
            if (ViewState["URLDESTINAZIONE"] != null)
            {
                Response.Redirect(ViewState["URLDESTINAZIONE"].ToString());
            }
        }
        protected void DoAzione()
        {
            //((clsSession)Session["GreenApple"]).FiltriRicerca = new clsSession.clsFiltriRicerca();
            //if (((clsSession)Session["GreenApple"]).AzioneCorrente == clsSession.AzioniPagina.Modifica
            //       || ((clsSession)Session["GreenApple"]).AzioneCorrente == clsSession.AzioniPagina.Inserimento
            //       )
            //{
            //    ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divMessageBox")).Visible = true;
            //}
            //else
            //{
            Response.Redirect(ViewState["URLDESTINAZIONE"].ToString());
            //}
        }
        protected void DoAzione(object sender, EventArgs e, string UrlDestinazione)
        {
            ViewState["URLDESTINAZIONE"] = UrlDestinazione;
            if (((clsSession)Session["GreenApple"]).AzioneCorrente == clsSession.AzioniPagina.Modifica
                   || ((clsSession)Session["GreenApple"]).AzioneCorrente == clsSession.AzioniPagina.Inserimento
                   )
            {
                ((System.Web.UI.HtmlControls.HtmlGenericControl)this.Page.Master.FindControl("divMessageBox")).Visible = true;
            }
            else
            {
                lnkConfermaAzione_Click(sender, e);
            }
        }

        protected string ConvertItemGridToShortDate(object pEntry)
        {
            try
            {
                return Convert.ToDateTime(pEntry).ToShortDateString();
            }
            catch
            {
                return clsCostanti.NonAncoraSpecificata;
            }
        }
        protected string ConvertItemGridToSIorNO(object pEntry)
        {
            bool Ret = false;
            try
            {
                Ret = Convert.ToBoolean(pEntry);
            }
            catch
            {
            }
            if (Ret)
            {
                return "Sì";
            }
            else
            {
                return "No";
            }
        }
    }

    #endregion

    #region CLASSI BASE PAGE

    public class GREEN_BasePage : System.Web.UI.Page
    {
        protected internal void Page_Load(object sender, EventArgs e)
        {
            if (Session["GreenApple"] == null)
            {
                Session["GreenApple"] = new clsSession();
            }

            clsFunctions.InitThread();
            this.Page.MaintainScrollPositionOnPostBack = true;
        }



    }


    #endregion


}