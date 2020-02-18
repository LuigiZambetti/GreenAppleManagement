<%@ Control Language="C#" CodeFile="Green_Admin_ModalitaPagamento.ascx.cs" Inherits="Green.Apple.Management.Green_Admin_ModalitaPagamento" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr>
        <td class="form_Title" colspan="3">
            &nbsp;&nbsp;Amministrazione - Modalità Pagamento
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
                        <asp:LinkButton ID="lnkInserisci" Font-Bold="true" runat="server" OnClick="lnkInserisci_Click"><img src='../customLayout/images/icoNew.gif' border='0'>&nbsp;Inserisci Modalità Pagamento</asp:LinkButton>
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
                                <asp:BoundColumn DataField="IDModalita" HeaderText="IDModalita" visible="false"></asp:BoundColumn>
                                <asp:BoundColumn DataField="Codice" HeaderText="Codice" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="Descrizione" HeaderText="Descrizione" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="NumRate" HeaderText="NumRate" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="Giorni" HeaderText="Giorni" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="FM" HeaderText="FM" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="RiBa" HeaderText="RiBa" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="BB" HeaderText="BB" ></asp:BoundColumn>
                                <asp:BoundColumn DataField="RD" HeaderText="RD" ></asp:BoundColumn>
                                
                                <asp:ButtonColumn CommandName="MODIFICA" Text="<img src='../customLayout/images/Edit.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn>
                                <asp:ButtonColumn CommandName="ELIMINA" Text="<img src='../customLayout/images/Delete.gif' border='0'>">
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
                        <a name="SectionEdit"></a>Inserimento/Modifica Modalità Pagamento
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
                        Codice:
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCodice" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqElemento" runat="server" ControlToValidate="txtCodice" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                    <td class="form_text">
                        Descrizione:
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtDescrizione" runat="server" MaxLength="80" Width="98%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtDescrizione" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                    <td class="form_text">
                        NumRate:
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtNumRate" runat="server" MaxLength="80" Width="98%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtNumRate" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="form_text">
                        Giorni:
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtGiorni" runat="server" MaxLength="80" Width="98%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtGiorni" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                 <tr>
                    <td class="form_text">
                        
                    </td>
                    <td class="form_textarea">
                        <asp:CheckBox ID="chkFM" runat="server" Text="FM"/>
                        <asp:CheckBox ID="chkRIBA" runat="server"  Text="RiBa"/>
                        <asp:CheckBox ID="chkBB" runat="server"  Text="BB"/>
                        <asp:CheckBox ID="chkRD" runat="server"  Text="RD"/>
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
