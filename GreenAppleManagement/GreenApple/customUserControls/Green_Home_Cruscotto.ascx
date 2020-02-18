<%@ Control Language="C#" CodeFile="Green_Home_Cruscotto.ascx.cs" Inherits="Green.Apple.Management.Green_Home_Cruscotto" %>
<table cellpadding="5" cellspacing="0" border="0" style="width: 100%; height: 100%; border: 1px solid #666666;">
    <tr valign="top" height="100%">
        <td width="60%">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 100%; border: 1px solid #666666;">
                <tr valign="top">
                    <td class="form_Title_SottoTitolo" style="border-bottom: 1px solid #666666;">
                        <asp:Label ID="lblTitolo" runat="server" 
                            Text="Benvenuti in Green Apple Management"></asp:Label>
                    </td>
                </tr>
                <tr valign="top">
                    <td style="padding: 20px;text-align:justify;" align="center">
                        <asp:Panel ID="pnlLogin" runat="server">
                            <table cellpadding="10" cellspacing="0" border="0" style="width: 80%;border: 1px solid #666666;">
                                <tr>
                                    <td class="form_Title_SottoTitolo" width="30%">
                                        Login :
                                    </td>
                                    <td class="form_Title_SottoTitolo" width="70%">
                                        <asp:TextBox ID="txtLogin" runat="server" Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="form_Title_SottoTitolo"  >
                                        Password :
                                    </td>
                                    <td class="form_Title_SottoTitolo">
                                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password"  Width="100%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="form_Title_SottoTitolo"  >
                                        &nbsp;
                                    </td>
                                    <td class="form_Title_SottoTitolo" >
                                        <asp:Label ID="lblErrorLogin" runat="server" Text=""></asp:Label>
                                    </td>
                                </tr>
                                
                                <tr>
                                    <td class="form_Title_SottoTitolo"  >
                                        &nbsp;
                                    </td>
                                    <td class="form_Title_SottoTitolo">
                                        <asp:LinkButton ID="lnkACCEDI" runat="server" onclick="lnkACCEDI_Click">ACCEDI >></asp:LinkButton>
                                    </td>
                                </tr>
                            </table> 
                        </asp:Panel>
                        
                        <asp:DataGrid ID="gridNews" runat="server" AutoGenerateColumns="False" AllowPaging="false" CssClass="webgridGeneral_Home" GridLines="None" Width="100%"  CellPadding="2" ShowHeader="False" PageSize="15">
                            <Columns>
                                <asp:BoundColumn DataField="IdNews" HeaderText="IdNews" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="NewsChiusa" HeaderText="Chiusa"></asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="Contenuto">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                            <tr valign="top">
                                                <td style="border: 0;">
                                                    <B><%# DataBinder.Eval(Container, "DataItem.Data")%> - 
                                                    <%# DataBinder.Eval(Container, "DataItem.Titolo")%></B>
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td style="font-size: 9px; border: 0;">
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
        <td width="40%">
            <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 100%; border: 1px solid #666666;">
                <tr valign="top">
                    <td class="form_Title_SottoTitolo" style="border-bottom: 1px solid #666666;">
                        <asp:Label ID="lblTitolo2" runat="server" 
                            Text="GREEN APPLE MANAGEMENT - Reportistica"></asp:Label>
                    </td>
                </tr>
                <tr valign="top">
                    <td style="padding: 20px;">
                        <table cellpadding="0" cellspacing="0" border="0" width="100%">
                            <tr>
                                <td valign=top>
                                    <asp:DataGrid ID="gridReport" runat="server" AutoGenerateColumns="False" AllowPaging="true" CssClass="webgridGeneral_Home" GridLines="None" Width="100%"  CellPadding="2" ShowHeader="False" PageSize="15">
                                        <Columns>
                                            <asp:BoundColumn DataField="IdReport" HeaderText="IdReport" Visible="False"></asp:BoundColumn>
                                            <asp:BoundColumn DataField="ReportHome" HeaderText="Report"></asp:BoundColumn>
                                            <asp:TemplateColumn HeaderText="Contenuto">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                                        <tr valign="top">
                                                            <td style="border: 0;">
                                                                <%# DataBinder.Eval(Container, "DataItem.Link")%>
                                                            </td>
                                                        </tr>
                                                        <tr valign="top">
                                                            <td style="font-size: 9px; border: 0;">
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
                </tr>
                
            </table>
        </td>
    </tr>
    <tr height="100%">
        <td>
        </td>
    </tr>
</table>
