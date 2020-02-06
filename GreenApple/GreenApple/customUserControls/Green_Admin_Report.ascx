<%@ Control Language="C#" CodeFile="Green_Admin_Report.ascx.cs" Inherits="Green.Apple.Management.Green_Admin_Report" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr>
        <td class="form_Title" colspan="3">
            &nbsp;&nbsp;Amministrazione - Elenco Reportistica
        </td>
    </tr>
    <tr height="5">
        <td colspan="3">
        </td>
    </tr>
    <tr valign="top">
        <td class="box_Menu_Admin">
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
            </table>
        </td>
        <td width="5">
        </td>
        <td class="box_Data_Admin">
            <table cellpadding="0" cellspacing="0" width="100%" border="0">
                <tr>
                    <td class="box_Menu_Item">
                        <asp:LinkButton ID="lnkInserisci" Font-Bold="true" runat="server" OnClick="lnkInserisci_Click"><img src='../customLayout/images/icoNew.gif' border='0'>&nbsp;Inserisci Voce Menu</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <asp:DataGrid ID="gridData" runat="server" 
                            Width="100%" AutoGenerateColumns="False" 
                            CssClass="webgridGeneral" AllowPaging="True"  CellPadding="5"
                            GridLines="Horizontal" OnItemCommand="gridData_ItemCommand" PageSize="20">
                            <SelectedItemStyle CssClass="webgridSelectedRowStyleDefault" />
                            <AlternatingItemStyle CssClass="webgridRowAlternateStyleDefault" />
                            <ItemStyle CssClass="webgridRowStyleDefault" />
                            <HeaderStyle CssClass="webgridHeaderStyleDefault" />
                            <Columns>
                                <asp:BoundColumn DataField="IdReport" HeaderText="ID" Visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Posizione" HeaderText="Posizione" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="Nome" HeaderText="Nome" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="Query" HeaderText="Query" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="DataColumn" HeaderText="Column Data Filter" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="RowPerPage" HeaderText="Righe per Pagina" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="ExportFile" HeaderText="Export File" ></asp:BoundColumn>
                                
                                 
                                <asp:ButtonColumn CommandName="MODIFICA" Text="<img src='../customLayout/images/Edit.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn>
                                <asp:ButtonColumn CommandName="ELIMINA" Text="<img src='../customLayout/images/Delete.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn> 
                                <asp:ButtonColumn CommandName="DOWN" Text="<img src='../customLayout/images/Down.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn> 
                                <asp:ButtonColumn CommandName="UP" Text="<img src='../customLayout/images/Up.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn>   
                                
                            </Columns>
                            <PagerStyle Visible="False" CssClass="webgridPagerStyleDefault" Mode="NumericPages" />
                        </asp:DataGrid>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="padding-top: 10px;">
                        <asp:PlaceHolder ID="phPagine" runat="server"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr height="30px">
                    <td>
                        &nbsp;</td>
                </tr>
            </table>
            <table id="TBLModifica" runat="server" cellpadding="0" cellspacing="0" width="100%" border="0" style="border: 1px solid #999999;">
                <tr>
                    <td colspan="2" class="form_AnagraficaInsert">
                        <a name="SectionEdit"></a>Inserimento/Modifica Report
                        :
                    </td>
                </tr>
                <tr>
                    <td class="form_Error_Message" colspan="2" align="left">
                        <asp:Label ID="lblError" runat="server" Text="" Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Nome:
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtNome" runat="server" MaxLength="80" Width="98%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqElemento" runat="server" ControlToValidate="txtNome" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                    <td class="form_text">
                        Descrizione:
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtDescrizione" runat="server" MaxLength="250" TextMode="MultiLine" Rows="4" Width="98%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDescrizione" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                
                <tr>
                    <td class="form_text">
                        Query:
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtQuery" runat="server" MaxLength="4000" TextMode="MultiLine" Rows="10" Width="98%"></asp:TextBox>
                        <br />
                        Inserire <b>@@DATA@@</b> all&#39;interno della Query per definire dove eseguire la 
                        query su data Filter.</td>
                </tr> 
                
                <tr>
                    <td class="form_text">
                        Column Data Filter:
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtColumnData" runat="server" MaxLength="255"  Width="98%"></asp:TextBox>
                       </td>
                </tr> 
                
                <tr>
                    <td class="form_text">
                        Righe per Pagina:
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtRighe" runat="server" MaxLength="3"  Width="98%"></asp:TextBox>
                       </td>
                </tr> 
                
                <tr>
                    <td class="form_text">
                        Abilita Export File:
                    </td>
                    <td class="form_textarea">
                        <asp:CheckBox ID="chkExport" runat="server" />
                    </td>
                </tr> 
                
                <tr id="RWTesting" runat="server">
                    <td class="form_text">
                        &nbsp;
                    </td>
                    <td class="form_textarea" align="left" >
                        <div style="BORDER:1px dotted #000000;width: 500px! important;margin-right:20px;padding:10px;">
                            <asp:LinkButton ID="lnkTest" runat="server" onclick="lnkTest_Click1">Test Query</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;<asp:Label
                                ID="lblTest" runat="server" Text=""></asp:Label>
                            <br />
                            <div style="overflow:scroll;width: 500px! important;height:100px;">
                                <asp:DataGrid ID="gridTest" runat="server" 
                                    Width="100%" AutoGenerateColumns="true" 
                                    CssClass="webgridGeneral" AllowPaging="True"  CellPadding="5"
                                    GridLines="Horizontal" PageSize="20">
                                    <SelectedItemStyle CssClass="webgridSelectedRowStyleDefault" />
                                    <AlternatingItemStyle CssClass="webgridRowAlternateStyleDefault" />
                                    <ItemStyle CssClass="webgridRowStyleDefault" />
                                    <HeaderStyle CssClass="webgridHeaderStyleDefault" />
                                    <PagerStyle Visible="False" CssClass="webgridPagerStyleDefault" Mode="NumericPages" />
                                </asp:DataGrid>
                            </div>
                        </div>
                    </td>
                    
                </tr>
                <tr><td><br /></td></tr>
                <tr>
                    <td class="form_text" width="20%">
                        Gruppi:
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:CheckBoxList ID="chkCollegati" runat="server" CssClass="form_textarea">
                        </asp:CheckBoxList>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" colspan="2" align="right">
                        <asp:LinkButton ID="lnkAnnulla" Font-Bold="true" runat="server" OnClick="lnkAnnulla_Click" CausesValidation="False"><img id="Img1" src="../customLayout/images/icoClose.gif" alt="" border="0" />&nbsp;Annulla&nbsp;</asp:LinkButton><asp:LinkButton ID="lnkAggiorna" Font-Bold="true" runat="server" OnClick="lnkAggiorna_Click"><img id="Img1" src="../customLayout/images/icoConfirm.gif" alt="" border="0" />&nbsp;Applica&nbsp;</asp:LinkButton>&nbsp;&nbsp;
                    </td>
                    
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
</script>
<asp:ValidationSummary ID="vldSummary" runat="server" ShowMessageBox="True" ShowSummary="True" />
