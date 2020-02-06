<%@ Control Language="C#" CodeFile="Green_View_Report.ascx.cs" Inherits="Green.Apple.Management.Green_View_Report" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr>
        <td class="form_Title" colspan="3">
            &nbsp;&nbsp;<asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
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
                        Report:
                    </td>
                </tr>
                <tr>
                    <td>
                        <asp:DataGrid ID="gridReport" runat="server" AutoGenerateColumns="False" AllowPaging="false" CssClass="webgridGeneral_Home" GridLines="None" Width="100%"  CellPadding="2" ShowHeader="False">
                            <Columns>
                                <asp:BoundColumn DataField="IdReport" HeaderText="IdReport" Visible="False"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Contenuto">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                            <tr valign="top">
                                                <td style="border: 0;">
                                                    <%# DataBinder.Eval(Container, "DataItem.Link")%>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td style="border: 0;">
                                                    <%# DataBinder.Eval(Container, "DataItem.Descrizione")%>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid>
                    </td>
                </tr>
            </table>
        </td>
        <td width="5">
        </td>
        <td class="box_Data_Admin">
            <table cellpadding="0" cellspacing="0" width="100%" border="0">
                <tr>
                    <td colspan="4" class="form_text">
                        <asp:Label ID="lblDescrizione" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                <tr id="RWDATE" runat="server">
                    <td class="form_text" >
                        <asp:Label ID="Label1" runat="server" Text="Data Inizio Periodo:"></asp:Label><br />
                            <asp:TextBox Width="80px" ID="txtDataInizio" runat="server"></asp:TextBox>
                            <asp:Image ID="imgDataInizio" runat="server" ImageUrl="../customLayout//images/calendar/calendar.gif" />
                    </td>

                    <td class="form_text" >
                        <asp:Label ID="Label2" runat="server" Text="Data Fine Periodo:"></asp:Label><br />
                        <asp:TextBox Width="80px" ID="txtDataFine" runat="server"></asp:TextBox>
                        <asp:Image ID="imgDataFine" runat="server" ImageUrl="../customLayout//images/calendar/calendar.gif" />
                    </td>
                    <td colspan="2" class="form_text" width="440px">
                        <asp:LinkButton ID="lnkFiltra" runat="server" onclick="lnkFiltra_Click">Applica Filtro Date</asp:LinkButton> 
                    </td>
                </tr>
                <tr><td colspan="4" >&nbsp;</td></tr>
                
                <tr id="ROWEsporta" runat="server">
                    <td colspan="4" style="font-weight:bold;">
                        <asp:Image ID="Image1" runat="server" ImageUrl="../customLayout//images/icoNew.gif" />&nbsp;&nbsp;&nbsp;<asp:LinkButton 
                            ID="lnkEsporta" runat="server" onclick="lnkEsporta_Click">ESPORTA TRACCIATO</asp:LinkButton>
                    </td>
                </tr>
                
                <tr><td colspan="4" >&nbsp;</td></tr>
                
                <tr>
                    <td colspan="4" valign="top">
                        <div style="overflow:scroll;width: 870px! important;">
                            <asp:DataGrid ID="gridData" runat="server" 
                                Width="100%" AutoGenerateColumns="True" 
                                CssClass="webgridGeneral" AllowPaging="True"  CellPadding="10" CellSpacing="2"
                                GridLines="Horizontal" PageSize="10">
                                <SelectedItemStyle CssClass="webgridSelectedRowStyleDefault" />
                                <AlternatingItemStyle CssClass="webgridRowAlternateStyleDefault" />
                                <ItemStyle CssClass="webgridRowStyleDefault" />
                                <HeaderStyle CssClass="webgridHeaderStyleDefault" />
                                <PagerStyle Visible="False" CssClass="webgridPagerStyleDefault" Mode="NumericPages" />
                            </asp:DataGrid>
                        </div> 
                    </td>
                </tr>
                <tr>
                    <td colspan="4" align="center" style="padding-top: 10px;">
                        <asp:PlaceHolder ID="phPagine" runat="server"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr height="30px">
                    <td colspan="4" >
                        &nbsp;</td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<asp:ValidationSummary ID="vldSummary" runat="server" ShowMessageBox="True" ShowSummary="True" />
