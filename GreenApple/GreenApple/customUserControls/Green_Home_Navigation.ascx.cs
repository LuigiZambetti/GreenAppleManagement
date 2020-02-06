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
using System.Drawing;

namespace Green.Apple.Management
{
    public partial class Green_Home_Navigation : Green_BaseUserControl
    {
        #region EVENTS
        protected new void Page_Load(object sender, EventArgs e)
        {
            base.Page_Load(sender, e);

            LoadMenu();
        }

        private void LoadMenu()
        {

            TABLEMenu.BorderWidth = Unit.Pixel(0);
            
            TableRow myRow = new TableRow();

            //ELEMENTO HOME SEMPRE PRESENTE
            TableCell myCell = new TableCell();
            myCell.CssClass = "menuItem";

            Panel myDiv = new Panel();
            myDiv.Attributes.Add("class", "menuHome");
            myDiv.Attributes.Add("onmouseover", "hideMenu();");

            LinkButton myLink = new LinkButton();
            myLink.ID = "Home_aspx";
            myLink.CausesValidation = false;
            myLink.Style.Add("color", "#FFFFFF");
            myLink.Style.Add("text-decoration", "none");
            myLink.Click += new EventHandler(Home_aspx_Click);
            myLink.Text = "Home";

            myDiv.Controls.Add(myLink);
            myCell.Controls.Add(myDiv);
            myRow.Controls.Add(myCell);

            //CARICAMENTO DELLE CATEGORIE (Per Ogni Categoria Caricamento dei Link)

            string sql = @"SELECT DISTINCT(GRUPPO) AS GRUPPO,COUNT(Admin_Menu.IDMENU) AS CONTO
            FROM Admin_Utenti 
            INNER JOIN Admin_UtentiGruppi ON Admin_Utenti.IdUtente = Admin_UtentiGruppi.IdUtente 
            INNER JOIN Admin_GruppiMenu ON Admin_UtentiGruppi.IdGruppo = Admin_GruppiMenu.IdGruppo 
            INNER JOIN Admin_Menu ON Admin_GruppiMenu.IdMenu = Admin_Menu.IdMenu
            WHERE Admin_Utenti.IdUtente = " + ((clsSession)Session["GreenApple"]).IDUtente + @" GROUP BY GRUPPO ";

            DataTable DTGruppi = new DataTable();
            clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "GRUPPIMENU", ref DTGruppi, true);

            PrintJavascriptMenu(DTGruppi.Rows.Count);

            for (int g = 0; g < DTGruppi.Rows.Count; g++)
            {
                string myGruppo="" + DTGruppi.Rows[g]["GRUPPO"];
                int Conto = int.Parse("" + DTGruppi.Rows[g]["CONTO"].ToString());

                myCell = new TableCell();
                myCell.CssClass = "menuItem";

                myDiv = new Panel();
                myDiv.Attributes.Add("onmousemove", "displayMenu(" + g + ");");
                myDiv.Attributes.Add("onmouseover", "displayMenu(" + g + ");");
                myDiv.Attributes.Add("onmouseover", "hideMenu();");
                myDiv.CssClass="menu";
                
                
                
                Literal myTitle=new Literal();
                myTitle.Text=myGruppo;

                myDiv.Controls.Add(myTitle);
                myCell.Controls.Add(myDiv);

                //CICLO ELEMENTI SU DB PER MENU
                if (Conto > 0)
                {
                    sql = @"SELECT DISTINCT Admin_Menu.*
                    FROM Admin_Utenti 
                    INNER JOIN Admin_UtentiGruppi ON Admin_Utenti.IdUtente = Admin_UtentiGruppi.IdUtente 
                    INNER JOIN Admin_GruppiMenu ON Admin_UtentiGruppi.IdGruppo = Admin_GruppiMenu.IdGruppo 
                    INNER JOIN Admin_Menu ON Admin_GruppiMenu.IdMenu = Admin_Menu.IdMenu
                    WHERE GRUPPO = '" + myGruppo + @"'
                    AND Admin_Utenti.IdUtente = " + ((clsSession)Session["GreenApple"]).IDUtente + @"
                    ORDER BY Admin_Menu.GRUPPO,Admin_Menu.POSIZIONE,Admin_Menu.Voce " ;

                    DataTable DTElementi = new DataTable();
                    clsDB.Load_DataTable(this.Page, ((clsSession)Session["GreenApple"]).CnnStr, sql, "ELEMENTI", ref DTElementi, true);
                    //DISEGNO GLI ELEMENTI
                    if(DTElementi.Rows.Count>0)
                    {
                        myDiv = new Panel();
                        myDiv.Attributes.Add("onmousemove", "displayMenu(" + g + ");");
                        //myDiv.Attributes.Add("onmouseover", "hideMenu();");
                        myDiv.CssClass="divItem";
                        myDiv.Style.Add("position", "absolute");
                        myDiv.Style.Add("border-top", "1px solid #666666");
                        myDiv.Style.Add("z-index", "1000");
                        myDiv.ID = "menu" + g;

                        Table myTableInt=new Table();
                        myTableInt.Width=Unit.Percentage(100);
                        myTableInt.CellSpacing=0;
                        myTableInt.CellPadding=0;

                        TableRow myRowInt = new TableRow();
                        TableCell myCellInt = new TableCell();

                        //alla cella associo gli elementi linkbutton
                        for(int e=0;e<DTElementi.Rows.Count;e++)
                        {
                            myLink = new LinkButton();
                            myLink.ID = myGruppo + "_EL_" + DTElementi.Rows[e]["IdMenu"].ToString();

                            if (bool.Parse(DTElementi.Rows[e]["SimpleList"].ToString()))
                            {
                                myLink.Attributes.Add("GreenTable", DTElementi.Rows[e]["Tabella"].ToString());
                                myLink.Click += new EventHandler(Admin_SimpleList);
                            }
                            else
                            {
                                myLink.Attributes.Add("GreenTable", DTElementi.Rows[e]["Pagina"].ToString());
                                myLink.Click += new EventHandler(Admin_Page);
                            }

                            myLink.CausesValidation = false;
                            myLink.Style.Add("color", "#FFFFFF");
                            myLink.Style.Add("text-decoration", "none");
                            myLink.Text = DTElementi.Rows[e]["Voce"].ToString();
                            myCellInt.Controls.Add(myLink);
                        }
                        myRowInt.Cells.Add(myCellInt);
                        myTableInt.Rows.Add(myRowInt);
                        myDiv.Controls.Add(myTableInt);
                        myCell.Controls.Add(myDiv);
                    }

                }

                myRow.Controls.Add(myCell);
            }

            //Aggiungo cella per spazio in coda
            myCell = new TableCell();
            myCell.CssClass = "menuItemCoda";
            //myCell.Style.Add("background-color", "#89C56B");
            myCell.Width = Unit.Percentage(100);
            myRow.Controls.Add(myCell);

            TABLEMenu.Controls.Add(myRow);


        }

        protected void Admin_SimpleList(object sender, EventArgs e)
        {
            ViewState["URLDESTINAZIONE"] = "Admin.aspx";
            ((clsSession)Session["GreenApple"]).SimpleList = ((LinkButton)sender).Attributes["GreenTable"].ToString();
          
            DoAzione();
        }

        protected void Admin_Page(object sender, EventArgs e)
        {
            ViewState["URLDESTINAZIONE"] = ((LinkButton)sender).Attributes["GreenTable"].ToString() + ".aspx";
            DoAzione();
        }

        protected void Home_aspx_Click(object sender, EventArgs e)
        { 
            ViewState["URLDESTINAZIONE"] ="Home.aspx";
            DoAzione();
        }

        private void PrintJavascriptMenu(int MenuNumber)
        { 
            string scriptDISPLAY = @"
            var objTimeOut;    
            function displayMenu(Num)
            {
                document.getElementById('ctl00_ContentNavigation_Green_Home_Navigation1_menu' + Num).style.display='block';     
            }";

            string scriptHIDE = @"
            function hideMenu()
            {";

            for(int m=0;m<MenuNumber;m++)
            {
                scriptHIDE += "document.getElementById('ctl00_ContentNavigation_Green_Home_Navigation1_menu" + m + "').style.display='none';";
            }

            scriptHIDE += @"
                if (typeof document.body.style.maxHeight != 'undefined') 
                {
                  // IE 7, mozilla, safari, opera 9
                } 
                else 
                {
                  // IE6, older browsers
                    var elements = document.getElementsByTagName('select');
                    for (var i=0; i<elements.length; i++) 
                    {            
                        elements[i].style.visibility = 'visible';
                    }                                                                            
                } 
            }";

            Literal LitscriptDISPLAY=new Literal();
            LitscriptDISPLAY.Text="<script>" + scriptDISPLAY + "</script>";

            Literal LitscriptHIDE=new Literal();
            LitscriptHIDE.Text="<script>" + scriptHIDE + "</script>";

            ((Literal)this.Page.Master.FindControl("PLACE_SCRIPT")).Text = "<script>";
            ((Literal)this.Page.Master.FindControl("PLACE_SCRIPT")).Text += scriptDISPLAY;
            ((Literal)this.Page.Master.FindControl("PLACE_SCRIPT")).Text += scriptHIDE;
            ((Literal)this.Page.Master.FindControl("PLACE_SCRIPT")).Text += "</script>";
          
  
        }
        #endregion


		
}
}