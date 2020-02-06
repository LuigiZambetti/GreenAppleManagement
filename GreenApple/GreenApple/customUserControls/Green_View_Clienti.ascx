<%@ Control Language="C#" CodeFile="Green_View_Clienti.ascx.cs" Inherits="Green.Apple.Management.Green_View_Clienti" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr>
        <td class="form_Title" colspan="3">
            &nbsp;&nbsp;Amministrazione - Elenco Clienti
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
                        <asp:LinkButton ID="lnkInserisci" Font-Bold="true" runat="server" OnClick="lnkInserisci_Click"><img src='../customLayout/images/icoNew.gif' border='0'>&nbsp;Inserisci Cliente</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <asp:DataGrid ID="gridData" runat="server" 
                            Width="100%" AutoGenerateColumns="False" 
                            CssClass="webgridGeneral" AllowPaging="True"  CellPadding="5"
                            GridLines="Horizontal" OnItemCommand="gridData_ItemCommand" PageSize="4">
                            <SelectedItemStyle CssClass="webgridSelectedRowStyleDefault" />
                            <AlternatingItemStyle CssClass="webgridRowAlternateStyleDefault" />
                            <ItemStyle CssClass="webgridRowStyleDefault" />
                            <HeaderStyle CssClass="webgridHeaderStyleDefault" />
                            <Columns>
                                <asp:BoundColumn DataField="Clicodice" HeaderText="Clicodice" Visible="false"></asp:BoundColumn>
                                
                                <asp:TemplateColumn HeaderText="Contenuto">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                            
                                            <tr valign="top">
                                                <td style="border: 0;" colspan="2">
                                                    <br />
                                                    <h5>
                                                        <%# DataBinder.Eval(Container, "DataItem.Clicodice")%> - <I>Prenome : </I><%# DataBinder.Eval(Container, "DataItem.Cliprenome")%>
                                                        <br /><%# DataBinder.Eval(Container, "DataItem.Nomenclatura")%> &nbsp;<%# DataBinder.Eval(Container, "DataItem.Cliragsoc")%>
                                                    </h5>
                                                </td>
                                            </tr>
                                            
                                            <tr valign="top">
                                                <td style="border: 0;" valign="top" width="60%">
                                                    <B>Prenome : </B><%# DataBinder.Eval(Container, "DataItem.Cliprenome")%>
                                                    <br /><B>Indirizzo : </B><%# DataBinder.Eval(Container, "DataItem.Cliindirizzo")%> - <%# DataBinder.Eval(Container, "DataItem.CliCAP")%> <%# DataBinder.Eval(Container, "DataItem.Clilocalita")%> <%# DataBinder.Eval(Container, "DataItem.Cliprovincia")%> <%# DataBinder.Eval(Container, "DataItem.CliNazione")%>
                                                    <br /><B>Posta : </B><%# DataBinder.Eval(Container, "DataItem.Clipostaragsoc")%> , <%# DataBinder.Eval(Container, "DataItem.ClipostaIndirizzo")%> <%# DataBinder.Eval(Container, "DataItem.ClipostaCAP")%> <%# DataBinder.Eval(Container, "DataItem.Clipostalocalita")%> <%# DataBinder.Eval(Container, "DataItem.Clipostaprov")%> <%# DataBinder.Eval(Container, "DataItem.ClipostaNazione")%>
                                                    <br /><B>Telefono : </B><%# DataBinder.Eval(Container, "DataItem.Clitelefono")%> , <B>Fax : </B><%# DataBinder.Eval(Container, "DataItem.Clifax")%>
                                                    <br /><B>Email : </B><%# DataBinder.Eval(Container, "DataItem.CliEmail")%>                                                    
                                                    <br /><B>Partita IVA : </B><%# DataBinder.Eval(Container, "DataItem.CliIVA")%>
                                                    <br /><B>Codice Fiscale : </B><%# DataBinder.Eval(Container, "DataItem.CliCF")%>
                                                    <br /><B>Pagamento : </B><%# DataBinder.Eval(Container, "DataItem.ClipagamentoDESC")%>
                                                    <br /><B>Banca : </B><%# DataBinder.Eval(Container, "DataItem.CliBanca")%>
                                                    <%--<br /><B>Piazza : </B><%# DataBinder.Eval(Container, "DataItem.CliPiazza")%>--%>
                                                    
                                                    <br /><br />
                                                </td>
                                                <td style="border: 0;" valign="top" width="40%">
                                                    <B>CALCOLA IVA: </B><%# DataBinder.Eval(Container, "DataItem.GACalcoloIva")%>
                                                    
                                                    <br /><B>Attenzione: </B><%# DataBinder.Eval(Container, "DataItem.GAAttenzione")%>
                                                    
                                                    <br /><B>I.B.A.N. : </B><%# DataBinder.Eval(Container, "DataItem.CliIBAN")%>
                                                    <br /><B>SWIFT : </B><%# DataBinder.Eval(Container, "DataItem.CliSWIFT")%>
                                                    <br /><B>% Diritti : </B><%# DataBinder.Eval(Container, "DataItem.Clipercdiritti")%>
                                                    <br /><B>Valuta : </B><%# DataBinder.Eval(Container, "DataItem.CliValuta")%>
                                                    <br /><B>Cod. Destinatario : </B><%# DataBinder.Eval(Container, "DataItem.CliCodDestinatario")%>
                                                    
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td colspan="2">
                                                    <b>Note : </b><%# DataBinder.Eval(Container, "DataItem.GANote")%> 
                                                </td>
                                            </tr>
                                            <tr valign="top"><td>&nbsp;</td></tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                
                                <asp:ButtonColumn CommandName="MODIFICA" Text="<img src='../customLayout/images/Edit.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn>
                                <asp:ButtonColumn CommandName="ELIMINA" Text="<img src='../customLayout/images/Delete.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn> 
                                <asp:ButtonColumn Visible="false" CommandName="DOWN" Text="<img src='../customLayout/images/Down.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn> 
                                <asp:ButtonColumn Visible="false" CommandName="UP" Text="<img src='../customLayout/images/Up.gif' border='0'>">
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
                        <a name="SectionEdit"></a>Inserimento/Modifica Cliente
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
                        CALCOLA IVA :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:CheckBox ID="chkGACalcoloIva" runat="server" />
                    </td>
                </tr>
               <tr>
                    <td class="form_text" width="20%">
                        Nomenclatura :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtNomclatura" runat="server" MaxLength="50" Width="55%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Attenzione di :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtGAAttenzione" runat="server" MaxLength="50" Width="80%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Nome :<font color="Red">*</font>
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliprenome" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="reqElemento" runat="server" ControlToValidate="txtCliprenome" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Rag. Soc.:<font color="Red">*</font>
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliragsoc" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtCliragsoc" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Indirizzo :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliindirizzo" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Località :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClilocalita" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Provincia:
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliprovincia" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        CAP :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliCAP" runat="server" MaxLength="50" Width="55%"></asp:TextBox>
                    &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Nazione :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliNazione" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Redazione :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliRedazione" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta Rag. Soc.:
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClipostaragsoc" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta Indirizzo :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClipostaIndirizzo" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta Località :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClipostalocalita" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta Provincia:
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClipostaprov" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta CAP :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClipostaCAP" runat="server" MaxLength="50" Width="55%"></asp:TextBox>
                    &nbsp;
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta Nazione :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClipostaNazione" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Telefono:
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClitelefono" runat="server" MaxLength="30" Width="120px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Email:
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliEmail" runat="server" MaxLength="255" Width="300px"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Fax :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClifax" runat="server" MaxLength="50" Width="55%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Partita IVA :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliIVA" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Codice Fiscale :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliCF" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                   </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Codice Destinatario :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCodDestinatario" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                   </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Pagamento :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:DropDownList ID="cbotxtClipagamento" runat="server" Width="300px">
                                    </asp:DropDownList>
                        
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Banca :
                    </td>
                    <td class="form_textarea" width="80%">
                        <%--<asp:TextBox ID="txtCliBanca" runat="server" MaxLength="255" Width="98%"></asp:TextBox>--%>
                        <asp:DropDownList ID="cbotxtCliBanca" runat="server" Width="300px" OnSelectedIndexChanged="cbotxtCliBanca_SelectedIndexChanged" AutoPostBack="true">
                                    </asp:DropDownList>
                    </td>
                </tr>
                <tr style="display:none;">
                    <td class="form_text" width="20%">
                        Piazza :
                                            Piazza :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliPiazza" runat="server" MaxLength="255 " Width="98%"></asp:TextBox>
                       
                    </td>
                </tr>
                <tr style="DISPLAY:none;">
                    <td class="form_text" width="20%">
                        % IVA :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClipercIVA" runat="server" MaxLength="4" Width="70px"></asp:TextBox>
                    &nbsp;<asp:CompareValidator ID="CompareValidator1" runat="server" 
                            ControlToValidate="txtClipercIVA" CultureInvariantValues="True" 
                            ErrorMessage="Inserire un valore numerico (1 - 100)" Operator="NotEqual" 
                            Type="Double"></asp:CompareValidator>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        % Diritti :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClipercdiritti" runat="server" MaxLength="4" Width="70px" 
                           ></asp:TextBox>
                    &nbsp;<asp:CompareValidator ID="CompareValidator2" runat="server" 
                            ControlToValidate="txtClipercdiritti" CultureInvariantValues="True" 
                            ErrorMessage="Inserire un valore numerico (1 - 100)" Operator="NotEqual" 
                            Type="Double"></asp:CompareValidator>
                    </td>
                </tr>
                
                <tr style="">
                    <td class="form_text" width="20%">
                        Tar. Truccatore :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtTarTruccatore" runat="server" MaxLength="4" Width="70px" ></asp:TextBox>
                    &nbsp;
                    </td>
                </tr>
                
                <tr style="">
                    <td class="form_text" width="20%">
                        Tar. Parruchiere :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtTarParrucchiere" runat="server" MaxLength="4" Width="70px" ></asp:TextBox>
                    &nbsp;
                    </td>
                </tr>
                
                <tr style="">
                    <td class="form_text" width="20%">
                        Tar. Manicurista :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtTarManicurista" runat="server" MaxLength="4" Width="70px" ></asp:TextBox>
                    &nbsp;
                    </td>
                </tr>
                
                <tr style="">
                    <td class="form_text" width="20%">
                        Tar. Groomer :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtTarGroomer" runat="server" MaxLength="4" Width="70px" ></asp:TextBox>
                    &nbsp;
                    </td>
                </tr>
                
                
                <tr>
                    <td class="form_text" width="20%">
                        Cod For :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtClicodfor" runat="server" MaxLength="10" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" width="20%">
                        IBAN:
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliIBAN" runat="server" MaxLength="40" Width="300px"></asp:TextBox>
                      </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Lingua :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:DropDownList ID="cbotxtCliLingua" runat="server" Width="300px">
                                    </asp:DropDownList>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Valuta :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:DropDownList ID="cbotxtCliValuta" runat="server" Width="300px">
                                    </asp:DropDownList>
                        
                   </td>
                </tr>
               
                <tr>
                    <td class="form_text" width="20%">
                        SWIFT :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliSWIFT" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        In Uso :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:CheckBox ID="txtCliInUso" runat="server" Text="In Uso"/>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Note :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliNote" runat="server" TextMode="MultiLine" Rows="6" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <!-- Nuovi campi - START -->
                <%--<tr>
                    <td class="form_text" width="20%">
                        Numero dichiarazione di intenti :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliNumDicIntenti" runat="server" MaxLength="20" Width="300px"></asp:TextBox>
                    </td>
                </tr>--%>
                <tr>
                    <td class="form_text" width="20%">
                        Data dichiarazione di intenti :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCliDataDicIntenti" runat="server" Width="120px"></asp:TextBox>
                        <asp:Image ID="imgtxtCliDataDicIntenti" runat="server" ImageUrl="../customLayout/images/calendar/calendar.gif" />
                        <asp:Label ID="lblCliDataDicIntenti" runat="server" Text="!! Le fatture successive alla data indicata verranno irreversibilmente aggiornate" Font-Bold="true"></asp:Label>
                    </td>
                </tr>
                <!-- Nuovi campi - END -->
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
    function CatturaInvio() {
        if (window.event.keyCode == 13) {
            __doPostBack('<%=lnkCerca.UniqueID %>', '');
	}
}
</script>
<asp:ValidationSummary ID="vldSummary" runat="server" ShowMessageBox="True" ShowSummary="True" />
