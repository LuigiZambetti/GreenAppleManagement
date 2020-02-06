<%@ Control Language="C#" CodeFile="Green_View_SchedaFornitore.ascx.cs" Inherits="Green.Apple.Management.Green_View_SchedaFornitore" %>

<style>
    .TDcol
    {
        border-bottom:1px solid #DDDDDD !important;
    }
</style>
<script type="text/javascript">

    function AttivaDisattivaDataRiferimento(cb) {
        //alert(cb.checked);
        if (cb.checked == true) {
            document.getElementById("ctl00_ContentMain_ctl00_txtDataRiferimento").style.display = "";
            document.getElementById("ctl00_ContentMain_ctl00_imgDataRiferimento").style.display = "";
        } else if (cb.checked == false) {
            
            document.getElementById("ctl00_ContentMain_ctl00_txtDataRiferimento").style.display = "none";
            document.getElementById("ctl00_ContentMain_ctl00_imgDataRiferimento").style.display = "none";
            document.getElementById("ctl00_ContentMain_ctl00_txtDataRiferimento").value = "";
        }
    }

</script>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr>
        <td class="form_Title" colspan="3">
            
            &nbsp;&nbsp;<asp:Label ID="lblTitle" runat="server" Text="Scheda Fornitore - VISUALIZZAZIONE DEI SERVIZI"></asp:Label>
        </td>
    </tr>
    <tr height="5">
        <td colspan="3">
        </td>
    </tr>
    <tr valign="top">
        <td class="box_Menu_Admin" style="WIDTH:200px !important;">
            <table width="100%" onkeydown="CatturaInvio();" cellpadding="0" cellspacing="0">
                
                <tr>
                    <td class="form_text">
                        Ricerca:
                    </td>
                </tr>
                <tr>
                    <td class="form_textarea">
                        <asp:TextBox Width="100%" CssClass="form_DropDown" ID="txtFiltroLibero" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" align="right">
                        <asp:LinkButton ID="lnkCerca" runat="server" Font-Bold="true" OnClick="lnkCerca_Click">Cerca</asp:LinkButton>
                    </td>
                </tr>
                <tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr><tr><td>&nbsp;</td></tr>
                
                <tr>
                    <td class="form_text" align="left">
                        <asp:LinkButton ID="lnkViewScheda" runat="server" Font-Bold="true" 
                            onclick="lnkViewScheda_Click">Elenco Schede Fornitore</asp:LinkButton>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" align="left">
                        <asp:LinkButton ID="lnkViewServizi" runat="server" Font-Bold="true" 
                            onclick="lnkViewServizi_Click">Elenco Servizi</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </td>
        <td width="5">
        </td>
        <td class="box_Data_Admin">
            <table cellpadding="0" cellspacing="0" width="100%" border="0">
                <tr>
                    <td class="box_Menu_Item">
                        <table border="0" width="100%">
                            <tr>
                                <td class="form_text" style="border-bottom:1px solid #BBBBBB;">
                                    <asp:LinkButton ID="lnkFilterESEGUITO" runat="server" 
                                        onclick="lnkFilterESEGUITO_Click" ><img border='0' src='../customLayout/images/cruscotto/ESEGUITO.gif' /></asp:LinkButton>
                                </td>
                                <td class="form_text" style="border-bottom:1px solid #BBBBBB;">
                                    Eseguito
                                </td>
                                <td class="form_text" style="border-bottom:1px solid #BBBBBB;">
                                    <asp:LinkButton ID="lnkFilterDAFATTURARE" runat="server" 
                                        onclick="lnkFilterDAFATTURARE_Click"><img border='0' src='../customLayout/images/cruscotto/DA FATTURARE.gif' /></asp:LinkButton>
                                </td>
                                <td class="form_text" style="border-bottom:1px solid #BBBBBB;">
                                    Da Fatturare
                                </td>
                                <td class="form_text" style="border-bottom:1px solid #BBBBBB;">
                                    <asp:LinkButton ID="lnkFilterFATTURATA" runat="server" 
                                        onclick="lnkFilterFATTURATA_Click"><img border='0' src='../customLayout/images/cruscotto/FATTURATA.gif' /></asp:LinkButton>
                                </td>
                                <td class="form_text" style="border-bottom:1px solid #BBBBBB;">
                                    Fattura da Ricevere
                                </td>
                                <td class="form_text" style="border-bottom:1px solid #BBBBBB;">
                                    <asp:LinkButton ID="lnkFilterPAGATA" runat="server" 
                                        onclick="lnkFilterPAGATA_Click"><img border='0' src='../customLayout/images/cruscotto/PAGATA.gif' /></asp:LinkButton>
                                </td>
                                <td class="form_text" style="border-bottom:1px solid #BBBBBB;">
                                    Pagata
                                </td>

                                <td class="form_text" style="border-bottom:1px solid #BBBBBB;">
                                    <asp:LinkButton ID="lnkFilterESCLUDIPAGATI" runat="server" 
                                        onclick="lnkFilterESCLUDIPAGATI_Click"><img border='0' src='../customLayout/images/cruscotto/NONPAGATI.gif' />
                                    </asp:LinkButton>
                                    
                                </td>
                                <td class="form_text" style="border-bottom:1px solid #BBBBBB;">
                                    Escludi Pagati
                                </td>
                                
                                <td class="form_text" colspan="3" width="300px" style="border-bottom:1px solid #BBBBBB;">
                                    <asp:Label ID="lblFiltroSel" runat="server" Text=""></asp:Label>
                                    <br />
                                    <asp:LinkButton ID="lnkFilterRemove" runat="server" 
                                        onclick="lnkFilterRemove_Click">Rimuovi Filtro</asp:LinkButton>&nbsp;
                                </td>
                                
                            </tr>
                            <tr>
                                <td class="form_text">
                                    Forni<BR />tore :
                                </td>
                                <td class="form_text"  colspan="5" onkeydown="CatturaInvioFORNITORE();">
                                    <asp:TextBox ID="txtSerFornitoreSearch" runat="server" style="WIDTH:50px;"></asp:TextBox>
                                    <asp:LinkButton ID="lnkCERCAFORNITORE" runat="server" Font-Bold="true" CausesValidation="false" Visible="true" onclick="lnkCERCAFORNITORE_Click">Cerca</asp:LinkButton>
                                    <br />
                                    <asp:DropDownList width="300px" ID="cboSerFornitore" runat="server" AutoPostBack="true" onselectedindexchanged="cboSerFornitore_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                                <td class="form_text" colspan="4">
                                    &nbsp;
                                    <br /><br />
                                    <asp:CheckBox ID="chkSommaFiltri" Text="Usa insieme anno/mese e inizio/fine" runat="server" />
                                </td>
                                <td class="form_text">
                                    Anno:
                                    <br /><br />
                                    <asp:DropDownList ID="cboAnno" runat="server" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <td class="form_text">
                                    Mese:
                                    <br /><br />
                                    <asp:DropDownList ID="cboMese" runat="server" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <td class="form_text">
                                    Mese/Anno:
                                    <br /><br />
                                    <asp:LinkButton ID="lnkFiltroMeseAnno" runat="server" onclick="lnkFiltroMeseAnno_Click">Filtra</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text"  colspan="10" style="text-align:right;">
                                    Data di riferimento:
                                    <br /><br />
                                    <asp:CheckBox ID="chkDataRiferimento" Text="Attiva scelta data riferimento" runat="server" onclick="AttivaDisattivaDataRiferimento(this);" />
                                    <asp:TextBox Width="80px" ID="txtDataRiferimento" runat="server" style="display:none;"></asp:TextBox>
                                    <asp:Image ID="imgDataRiferimento" runat="server" style="display:none;" ImageUrl="../customLayout/images/calendar/calendar.gif" />
                                </td>
                                <td class="form_text">
                                    Inizio:
                                    <br /><br />
                                    <asp:TextBox Width="80px" ID="txtDataInizio" runat="server"></asp:TextBox>
                                    <asp:Image ID="imgDataInizio" runat="server" ImageUrl="../customLayout/images/calendar/calendar.gif" />
                                </td>
                                <td class="form_text">
                                    Fine:
                                    <br /><br />
                                    <asp:TextBox Width="80px" ID="txtDataFine" runat="server"></asp:TextBox>
                                    <asp:Image ID="imgtxtDataFine" runat="server" ImageUrl="../customLayout/images/calendar/calendar.gif" />
                                </td>
                                <td class="form_text">
                                    Date:
                                    <br /><br />
                                    <asp:LinkButton ID="lnkFiltroDate" runat="server" onclick="lnkFiltroDate_Click" >Filtra</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                  
                <tr>
                    <td>
                        <table id="TBLModifica" runat="server"  cellpadding="0" cellspacing="0" width="100%" border="0" style="border: 1px solid #999999;">
                            <tr id="RWServizioMOD" runat="server" class="form_text">
                                <td colspan="4" class="form_text">
                                    <table class="RWServizioMOD" cellpadding="0" cellspacing="0" border="0" style="WIDTH:100%;HEIGHT:100%;background-color:#DDDDDD;" >
                                        <tr>
                                             <td class="TDcol form_text" colspan="2">
                                                Modifica Date Previste per Fatturazione Servizio a Fornitore:
                                             </td> 
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text" width="20%">
                                                <B>Anno Competenza :</B>
                                            </td>
                                            <td class="TDcol form_text"  width="80%">
                                                 <asp:DropDownList ID="cboSerAnno" runat="server" Width="40%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text">
                                                <B>Mese Competenza :</B>
                                            </td>
                                            <td class="TDcol form_text">
                                                 <asp:DropDownList ID="cboSerMese" runat="server" Width="40%">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="form_text" align="right" colspan="2">
                                                <asp:LinkButton ID="lnkSerAnnulla" OnClick="lnkSerAnnulla_Click"  Font-Bold="true" runat="server" CausesValidation="False"><img id="Img1" src="../customLayout/images/icoClose.gif" alt="" border="0" />&nbsp;Annulla Modifica Servizio&nbsp;</asp:LinkButton><asp:LinkButton ID="lnkSerAggiorna" OnClick="lnkSerAggiorna_Click"  Font-Bold="true" runat="server"><img id="Img1" src="../customLayout/images/icoConfirm.gif" alt="" border="0" />&nbsp;Applica Modifica Servizio&nbsp;</asp:LinkButton>&nbsp;&nbsp;
                                            </td>
                                            
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="RWTotaliServizi" runat="server">
                                <td  align="right">
                                    <table class="RWServizioMOD" cellpadding="0" cellspacing="0" border="0" style="WIDTH:30%;background-color:#DDDDDD;" >
                                        <tr>
                                             <td class="TDcol form_text" colspan="2">
                                                Totale Importo Lordo
                                             </td> 
                                             <td class="TDcol form_text" colspan="2" align="right">
                                                <asp:Label ID="lblTotLordo" runat="server" Text="Label"></asp:Label>
                                             </td>
                                        </tr>
                                        <tr>
                                             <td class="TDcol form_text" colspan="2">
                                                Totale Diritti
                                             </td> 
                                             <td class="TDcol form_text" colspan="2" align="right">
                                                 <asp:Label ID="lblTotDiritti" runat="server" Text="Label"></asp:Label>
                                             </td>
                                        </tr>
                                        <tr>
                                             <td class="TDcol form_text" colspan="2">
                                                Totale Redazionale
                                             </td> 
                                             <td class="TDcol form_text" colspan="2" align="right">
                                                <asp:Label ID="lblTotReda" runat="server" Text="Label"></asp:Label>
                                             </td>
                                        </tr>
                                        <tr>
                                             <td class="TDcol form_text" colspan="2">
                                                Totale Imponibile
                                             </td> 
                                             <td class="TDcol form_text" colspan="2" align="right">
                                                <asp:Label ID="lblTotImpo" runat="server" Text="Label"></asp:Label>
                                             </td>
                                        </tr>
                                        
                                        <tr>
                                             <td class="TDcol form_text" colspan="2" style="background-color:#FFFFFF;">
                                                Totale Spese
                                             </td> 
                                             <td class="TDcol form_text" colspan="2" align="right" style="background-color:#FFFFFF;">
                                                <asp:Label ID="lblTotSpese" runat="server" Text="Label"></asp:Label>
                                             </td>
                                        </tr>
                                        
                                        <tr>
                                             <td class="TDcol form_text" colspan="2"  style="background-color:#FFFFFF;">
                                                TOTALE
                                             </td> 
                                             <td class="TDcol form_text" colspan="2" align="right" style="background-color:#FFFFFF;">
                                                <asp:Label ID="lblTotTOTALE" runat="server" Text="Label"></asp:Label>
                                             </td>
                                        </tr>
                                        
                                    </table>
                                </td>
                            </tr>
                            
                            <tr id="RWANNULLASCHEDA" runat="server" style="display:none !important;">
                                <td colspan="2" class="form_text" align="right">                    
                                    <B><asp:LinkButton ID="lnkANNULLASCHEDA" runat="server" 
                                        onclick="lnkANNULLASCHEDA_Click">Annulla Stato dei SERVIZI VISUALIZZATI>></asp:LinkButton>
                                    </B>
                                </td>
                            </tr>
                            
                            <tr>
                                <td colspan="2" class="form_text">                    
                                   <asp:DataGrid ID="grdServizi" runat="server" 
                                    Width="1000px" AutoGenerateColumns="False" 
                                    CssClass="webgridGeneral" AllowPaging="False"  CellPadding="5" CellSpacing="1" PageSize="50"
                                    GridLines="Horizontal" onitemcommand="grdServizi_ItemCommand" >
                                    <SelectedItemStyle CssClass="webgridSelectedRowStyleDefault" />
                                    <AlternatingItemStyle CssClass="webgridRowAlternateStyleDefault" />
                                    <ItemStyle CssClass="webgridRowStyleDefault" />
                                    <HeaderStyle CssClass="webgridHeaderStyleDefault" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="&nbsp;">
                                            <ItemTemplate>
                                                <div style="float:left;">
                                                    <img src="../customLayout/images/cruscotto/<%# DataBinder.Eval(Container, "DataItem.COLORE")%>.gif"
                                                    alt="<%# DataBinder.Eval(Container, "DataItem.COLORE")%>" style="width:14px;height:14px;"/>
                                                </div> 
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Fine">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.SRdatafine")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        
                                        <asp:TemplateColumn HeaderText="Fornitore">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.GREEN_Fornitore")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        
                                        <asp:TemplateColumn HeaderText="Cliente">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.SERCliente")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        
                                        <asp:TemplateColumn HeaderText="Giorni">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.srnumgiorni")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Tar.Giorn.">
                                            <ItemTemplate>
                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.srimporto"))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Imp.Lordo">
                                            <ItemTemplate>
                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRimportoLordo"))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Diritti">
                                            <ItemTemplate>
                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.srdiritti"))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Redaz.">
                                            <ItemTemplate>
                                                 <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRimportoRedazionale"))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Impon.">
                                            <ItemTemplate>
                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRIMPO_REDA") )%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Spese">
                                            <ItemTemplate>
                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SpeseTotali") )%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Da Fatt.">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.DESCsrfatturare")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Fatt.">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.DESCsrfatturato")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Competenza">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.SRAnnoFatt")%> <%# DataBinder.Eval(Container, "DataItem.SRMeseFatt")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        
                                        <asp:ButtonColumn CommandName="MODIFICA" Text="<img src='../customLayout/images/Edit.gif' border='0'>">
                                            <HeaderStyle Width="25px" />
                                        </asp:ButtonColumn>
                                        <asp:ButtonColumn CommandName="ELIMINA" Text="<img src='../customLayout/images/Delete.gif' border='0'>">
                                            <HeaderStyle Width="25px" />
                                        </asp:ButtonColumn> 
                                        <asp:TemplateColumn HeaderText="&nbsp;">
                                            <ItemTemplate>
                                                <img src='../customLayout/images/SEARCH.GIF' style='width:16px;height:16px;' border='0' title='<%# DataBinder.Eval(Container, "DataItem.INFOSCHEDA")%>'>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle Visible="False" CssClass="webgridPagerStyleDefault" Mode="NumericPages" />
                                </asp:DataGrid>
                               
                                <asp:DataGrid ID="grdFornitori" runat="server" 
                                    Width="100%" AutoGenerateColumns="False" 
                                    CssClass="webgridGeneral" AllowPaging="False"  CellPadding="5" PageSize="50"
                                    GridLines="Horizontal" onitemcommand="grdFornitori_ItemCommand" >
                                    <SelectedItemStyle CssClass="webgridSelectedRowStyleDefault" />
                                    <AlternatingItemStyle CssClass="webgridRowAlternateStyleDefault" />
                                    <ItemStyle CssClass="webgridRowStyleDefault" />
                                    <HeaderStyle CssClass="webgridHeaderStyleDefault" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="Fornitore">
                                            <ItemTemplate>
                                                <%# DataBinder.Eval(Container, "DataItem.Forragsoc")%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Numero Servizi">
                                            <ItemTemplate>
                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.CONTO"))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Totale Servizi (Imp.Lordo)">
                                            <ItemTemplate>
                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SUMIMPLORDO"))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn HeaderText="Importo Fatturato">
                                            <ItemTemplate>
                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SUMIMPFATTURATO"))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        
                                        <asp:TemplateColumn HeaderText="Diritti">
                                            <ItemTemplate>
                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SUMIMPDIRITTI"))%>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:TemplateColumn  Visible="false" HeaderText="&nbsp;&nbsp;Schede Fornitore mese Selezionato">
                                            
                                            <ItemTemplate>
                                            
                                            


                                                <table cellpadding="0" cellspacing="10" border="0" style="width: 100%;" valign="top">
                                                                                                      
                                                    <tr valign="top">
                                                        <td style="border: 0;" colspan="2">
                                                            <div style="font-size:13px;font-weight:bold;">
                                                                Fornitore : <%# DataBinder.Eval(Container, "DataItem.Forragsoc")%>
                                                            </div>
                                                            <br />Elementi rilevati nel Periodo : <B><%# DataBinder.Eval(Container, "DataItem.CONTO")%>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        
                                        <asp:ButtonColumn CommandName="DETTAGLI" Text="<img src='../customLayout/images/Link.gif' border='0'>">
                                            <HeaderStyle Width="25px" />
                                        </asp:ButtonColumn>
                                        <asp:ButtonColumn CommandName="SCHEDA" Text="<img src='../customLayout/images/Print.gif' border='0'>">
                                            <HeaderStyle Width="25px" />
                                        </asp:ButtonColumn>
                                        
                                        
                                    </Columns>
                                    <PagerStyle Visible="False" CssClass="webgridPagerStyleDefault" Mode="NumericPages" />
                                </asp:DataGrid>
								</td>
                            </tr>
                            
                        </table>
                    </td>
                </tr>
                <tr height="30px">
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
</table>


        
<script>
function CatturaInvio()
{
	if (window.event.keyCode == 13) 
	{		
		__doPostBack('<%=lnkCerca.UniqueID %>','');					
	}
}

function CatturaInvioFORNITORE()
{
	if (window.event.keyCode == 13) 
	{		
		__doPostBack('<%=lnkCERCAFORNITORE.UniqueID %>','');					
	}
}

function VisibileDettaglio(code)
{
    var TBLDettaglio = document.getElementById(code);
    if(TBLDettaglio.style.display!='none')
    {
        TBLDettaglio.style.display = 'none';
    }
    else
    {
        TBLDettaglio.style.display = 'block';
    }
    
}



</script>
<asp:ValidationSummary ID="vldSummary" runat="server" ShowMessageBox="True" ShowSummary="True" />
