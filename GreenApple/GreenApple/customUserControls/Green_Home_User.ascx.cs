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
    public partial class Green_Home_User : Green_BaseUserControl
    {
        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            if (!IsPostBack)
            {
                CaricaColori();
            }

            string UtenteCollegato = HttpContext.Current.User.Identity.Name;

            string sql = "";
            if (((clsSession)Session["GreenApple"]).Login != "" && ((clsSession)Session["GreenApple"]).Password != "")
            {
                sql = "SELECT * FROM ADMIN_UTENTI WHERE LOGIN ='" + ((clsSession)Session["GreenApple"]).Login + "' ";
                sql += " AND PASSWORD ='" + ((clsSession)Session["GreenApple"]).Password + "'";
            }
            else
            {
                sql = "SELECT * FROM ADMIN_UTENTI WHERE ACCOUNT ='" + UtenteCollegato + "' AND ACCOUNT IS NOT NULL AND ACCOUNT <> '' ";   
            }
            
            DataTable DTUtente = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "UTENTE", ref DTUtente, true);
            if (DTUtente.Rows.Count > 0)
            {
                if("" + DTUtente.Rows[0]["Colore"]!="")
                {
                    ((clsSession)Session["GreenApple"]).SITE_Colore = "" + DTUtente.Rows[0]["Colore"];

                    if (!IsPostBack)
                    {
                        for (int i = 0; i <= cboColore.Items.Count - 1; i++)
                        {
                            if (cboColore.Items[i].Value == ((clsSession)Session["GreenApple"]).SITE_Colore)
                            {
                                cboColore.SelectedIndex = i;
                                break;
                            }
                        }
                    }
                }
                
                lblUtente.Text = DTUtente.Rows[0]["Nome"].ToString() + " " + DTUtente.Rows[0]["Cognome"].ToString();
                ((clsSession)Session["GreenApple"]).IDUtente = DTUtente.Rows[0]["IDUtente"].ToString();
            }
            else
            {
                ((clsSession)Session["GreenApple"]).IDUtente = "0";
                lblUtente.Text = "Utente NON AUTORIZZATO (" + UtenteCollegato + ") ";
            }


            lblData.Text = DateTime.Now.ToLongDateString().Substring(0, 1).ToUpper() + DateTime.Now.ToLongDateString().Substring(1);

            //SEZIONE DEDICATA AL COLORE DI SFONDO
            if (((clsSession)Session["GreenApple"]).SITE_Colore != "")
            {
                if (((clsSession)Session["GreenApple"]).SITE_Colore != "1")
                {
                    lblColors.Text = CalcoloGammaColori();
                }
            }

        }

        private void CaricaColori()
        {
            cboColore.Items.Clear();
            string sql = "select * from Admin_Colori order by colore ";
            DataTable DTColori = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "COLORI", ref DTColori, true);
            for (int i = 0; i < DTColori.Rows.Count; i++)
            {
                ListItem myitem = new ListItem();
                myitem.Value = DTColori.Rows[i]["ID"].ToString();
                myitem.Text = DTColori.Rows[i]["Colore"].ToString();
                cboColore.Items.Add(myitem);
            }
        }

        public string CalcoloGammaColori()
        {
            string sql = "select * from Admin_Colori where id = " + cboColore.SelectedValue;
            DataTable DTColori = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "COLORI", ref DTColori, true);
            

            string ColorePersonalizzato = @"
                <style>
                BODY
                {
                    background-color: #" + DTColori.Rows[0]["BODY"] + @";
                    padding: 1px;
                    margin: 0px;
                  
                }
                .BGHeader
                {
                    background-color: #" + DTColori.Rows[0]["BGHeader"] + @";
                }
                .menuItem
                {
                    border-top:1px solid #" + DTColori.Rows[0]["menuItemBorderTop"] + @";
                    border-bottom:2px solid #" + DTColori.Rows[0]["menuItemBorderBottom"] + @";;     
                }
                .menuItemCoda
                {
                    border-top:1px solid #" + DTColori.Rows[0]["menuItemBorderTop"] + @";   
                    border-bottom:2px solid #" + DTColori.Rows[0]["menuItemBorderBottom"] + @";     
                    background-color: #" + DTColori.Rows[0]["menuItemCodaColor"] + @";    
                }
                .form_Title_SottoTitolo{background-color: #" + DTColori.Rows[0]["form_Title_SottoTitoloColor"] + @";}
                .form_Title{background-color: #" + DTColori.Rows[0]["form_TitleColor"] + @";}
                .box_Menu_Admin{background-color: #" + DTColori.Rows[0]["box_Menu_AdminColor"] + @";}
                .menu
                {
                    background-color: #" + DTColori.Rows[0]["menuColor"] + @";
                    border-right:2px solid #" + DTColori.Rows[0]["divItemBorderPopIn2PX"] + @";      
                }
                .menuHome a
                {
                    background-color: #" + DTColori.Rows[0]["menuHomeAColor"] + @";
                    border-right:2px solid #" + DTColori.Rows[0]["divItemBorderPopIn2PX"] + @";      
                }
                
                .divItem{background-color: #" + DTColori.Rows[0]["divItemColor"] + @";}
                
                .divItem a
                {   
                    background-color: #" + DTColori.Rows[0]["divItemAColor"] + @";    
                    border:1px solid #" + DTColori.Rows[0]["divItemABorder"] + @";     
                    border-bottom:1px solid #" + DTColori.Rows[0]["divItemBorderPopIn"] + @";                   
                }
                .divItem a:hover{background-color: #" + DTColori.Rows[0]["divItemAHoverColor"] + @";}
                .divItem
                {
	                border-bottom:1px solid #" + DTColori.Rows[0]["divItemBorderPopIn"] + @";     
	                border-left:1px solid #" + DTColori.Rows[0]["divItemBorderPopIn"] + @";   
	                border-right:1px solid #" + DTColori.Rows[0]["divItemBorderPopIn"] + @";       
                }

                </style>
                ";

            return ColorePersonalizzato;
        }

        #endregion
        protected void lnkLogOut_Click(object sender, EventArgs e)
        {
            Session["GreenApple"] = new clsSession();

            ((clsSession)Session["GreenApple"]).Login = "";
            ((clsSession)Session["GreenApple"]).Password = "";
            ((clsSession)Session["GreenApple"]).IDUtente = "0";

            Response.Redirect("Home.aspx");
        }
        protected void cboColore_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CAMBIO COLORE UTENTE
            clsParameter pParameter = new clsParameter();
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@ID", ((clsSession)Session["GreenApple"]).IDUtente, SqlDbType.NVarChar, ParameterDirection.Input));
            pParameter.Parameters.Add(new clsParameter.MemberOfclsParameter("@COLORE", cboColore.SelectedValue, SqlDbType.Int, ParameterDirection.Input));

            string sql = "UPDATE ADMIN_UTENTI "
            + " SET COLORE = @COLORE "
            + " WHERE IDUTENTE = @ID";
            clsDB.Execute_Command_ClsParameter(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, pParameter.Parameters);

            Response.Redirect("Home.aspx");
        }
}
}