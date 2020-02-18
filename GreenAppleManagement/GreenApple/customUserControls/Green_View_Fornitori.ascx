<%@ Control Language="C#" CodeFile="Green_View_Fornitori.ascx.cs" Inherits="Green.Apple.Management.Green_View_Fornitori" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr>
        <td class="form_Title" colspan="3">
            &nbsp;&nbsp;Amministrazione - Elenco Fornitori
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
                        <asp:LinkButton ID="lnkInserisci" Font-Bold="true" runat="server" OnClick="lnkInserisci_Click"><img src='../customLayout/images/icoNew.gif' border='0'>&nbsp;Inserisci Fornitore</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        <asp:DataGrid ID="gridData" runat="server" 
                            Width="100%" AutoGenerateColumns="False" 
                            CssClass="webgridGeneral" AllowPaging="True"  CellPadding="5"
                            GridLines="Horizontal" OnItemCommand="gridData_ItemCommand" PageSize="5">
                            <SelectedItemStyle CssClass="webgridSelectedRowStyleDefault" />
                            <AlternatingItemStyle CssClass="webgridRowAlternateStyleDefault" />
                            <ItemStyle CssClass="webgridRowStyleDefault" />
                            <HeaderStyle CssClass="webgridHeaderStyleDefault" />
                            <Columns>
                                <asp:BoundColumn DataField="Forcodice" HeaderText="Forcodice" Visible="false"></asp:BoundColumn>
                                
                                <asp:TemplateColumn HeaderText="Contenuto">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                            
                                            <tr valign="top">
                                                <td style="border: 0;" colspan="2">
                                                    <br />
                                                    <h5>
                                                        <%# DataBinder.Eval(Container, "DataItem.Forcodice")%> - <I>Prenome : </I><%# DataBinder.Eval(Container, "DataItem.Forprenome")%> - <I>Ragione Sociale : </I> <%# DataBinder.Eval(Container, "DataItem.Forragsoc")%>
                                                    </h5>
                                                </td>
                                            </tr>
                                            
                                            <tr valign="top">
                                                <td style="border: 0;" valign="top" width="60%">
                                                    <B>Prenome : </B><%# DataBinder.Eval(Container, "DataItem.Forprenome")%>
                                                    <br /><B>Indirizzo : </B><%# DataBinder.Eval(Container, "DataItem.Forindirizzo")%> - <%# DataBinder.Eval(Container, "DataItem.ForCAP")%> <%# DataBinder.Eval(Container, "DataItem.Forlocalita")%> <%# DataBinder.Eval(Container, "DataItem.Forprovincia")%> <%# DataBinder.Eval(Container, "DataItem.ForNazione")%>
                                                    <br /><B>Posta : </B><%# DataBinder.Eval(Container, "DataItem.Forpostaragsoc")%> , <%# DataBinder.Eval(Container, "DataItem.Forpostaind")%> <%# DataBinder.Eval(Container, "DataItem.ForpostCAP")%> <%# DataBinder.Eval(Container, "DataItem.Forpostaloc")%> <%# DataBinder.Eval(Container, "DataItem.ForPostaProv")%> <%# DataBinder.Eval(Container, "DataItem.ForPostaNazione")%>
                                                    <br /><B>Telefono : </B><%# DataBinder.Eval(Container, "DataItem.Fortelef")%> , <B>Fax : </B><%# DataBinder.Eval(Container, "DataItem.Forfax")%>
                                                    <br /><B>Email : </B><%# DataBinder.Eval(Container, "DataItem.ForEmail")%> 
                                                    <br /><B>Partita IVA : </B><%# DataBinder.Eval(Container, "DataItem.ForIVA")%>
                                                    <br /><B>Codice Fiscale : </B><%# DataBinder.Eval(Container, "DataItem.ForCF")%>
                                                    <br /><B>Data Nascita : </B><%# DataBinder.Eval(Container, "DataItem.Fordatanascita")%>
                                                    <br /><B>Luogo Nascita : </B><%# DataBinder.Eval(Container, "DataItem.Forluogonascita")%>
                                                    <br /><B>Pagamento : </B><%# DataBinder.Eval(Container, "DataItem.Forpagamento")%>
                                                    <br /><B>Banca : </B><%# DataBinder.Eval(Container, "DataItem.Forbanca")%>
                                                    <br /><B>Piazza : </B><%# DataBinder.Eval(Container, "DataItem.ForPiazza")%>
                                                    <br /><B>I.B.A.N. : </B><%# DataBinder.Eval(Container, "DataItem.ForIBAN")%>
                                                    <br /><B>Operatore : </B><%# DataBinder.Eval(Container, "DataItem.Foroperatore")%>
                                                    
                                                    <br /><B>Pag Codice : </B><%# DataBinder.Eval(Container, "DataItem.ForPagCodice")%>
                                                    <br /><B>Operatore : </B><%# DataBinder.Eval(Container, "DataItem.Foroperatore")%>
                                                    <br /><B>In Uso : </B><%# DataBinder.Eval(Container, "DataItem.ForInUso")%>
                                                </td>
                                                <td style="border: 0;" valign="top" width="40%">
                                                    
                                                    <br /><B>Esenzione Iva Art15 : </B><%# DataBinder.Eval(Container, "DataItem.ForEsenzioneIvaArt15")%>
                                                    <br /><B>% IVA : </B><%# DataBinder.Eval(Container, "DataItem.ForpercIVA")%>
                                                    <br /><B>% Ritenuta : </B><%# DataBinder.Eval(Container, "DataItem.Forpercritenuta")%>
                                                    
                                                    <br /><B>% Riv. Prev. : </B><%# DataBinder.Eval(Container, "DataItem.ForPctRivPrev")%>
                                                    <br /><B>% Acconto : </B><%# DataBinder.Eval(Container, "DataItem.Forpercacconto")%>
                                                    <br /><B>% su Acconto : </B><%# DataBinder.Eval(Container, "DataItem.Forpercsuacconto")%>
                                                    <br />
                                                    <br /><B>% <%# DataBinder.Eval(Container, "DataItem.CAT1")%> : </B><%# DataBinder.Eval(Container, "DataItem.Forperc1")%>
                                                    <br /><B>% <%# DataBinder.Eval(Container, "DataItem.CAT2")%> : </B><%# DataBinder.Eval(Container, "DataItem.Forperc2")%>
                                                    <br /><B>% <%# DataBinder.Eval(Container, "DataItem.CAT3")%> : </B><%# DataBinder.Eval(Container, "DataItem.Forperc3")%>
                                                    <br /><B>% <%# DataBinder.Eval(Container, "DataItem.CAT4")%> : </B><%# DataBinder.Eval(Container, "DataItem.Forperc4")%>
                                                    <br /><B>% <%# DataBinder.Eval(Container, "DataItem.CAT5")%> : </B><%# DataBinder.Eval(Container, "DataItem.Forperc5")%>
                                                    <br /><B>% <%# DataBinder.Eval(Container, "DataItem.CAT6")%> : </B><%# DataBinder.Eval(Container, "DataItem.Forperc6")%>
                                                    
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
                        <a name="SectionEdit"></a>Inserimento/Modifica Fornitore
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
                        Nome :<font color="Red">*</font>
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForprenome" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtForprenome" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>

                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" width="20%">
                        Rag. Soc. :<font color="Red">*</font>

                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForragsoc" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtForragsoc" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Indirizzo :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForindirizzo" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Località :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForlocalita" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Provincia :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForprovincia" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        CAP :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForCAP" runat="server" MaxLength="10" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Nazione :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForNazione" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta Rag. Soc. :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForpostaragsoc" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta Indirizzo :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForpostaind" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta Località :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForpostaloc" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta CAP :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForpostCAP" runat="server" MaxLength="10" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta Nazione :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForPostaNazione" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Posta Provincia:
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForPostaProv" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Telefono :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtFortelef" runat="server" MaxLength="30" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Fax :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForfax" runat="server" MaxLength="15" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Email :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForEmail" runat="server" MaxLength="30" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Partita IVA :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForIVA" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Codice Fiscale :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForCF" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Data Nascita :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtFordatanascita" runat="server" MaxLength="10" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Luogo Nascita :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForluogonascita" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Pagamento :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForpagamento" runat="server" MaxLength="60" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Banca :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForbanca" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                       
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Piazza :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForPiazza" runat="server" MaxLength="255" Width="98%"></asp:TextBox>

                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" width="20%">
                        % IVA :<font color="Red">*</font>
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForpercIVA" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator11" runat="server" ControlToValidate="txtForpercIVA" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        % Riv.Prev. :<font color="Red">*</font>
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForPctRivPrev" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator12" runat="server" ControlToValidate="txtForPctRivPrev" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        % Ritenuta :<font color="Red">*</font>
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForpercritenuta" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator13" runat="server" ControlToValidate="txtForpercritenuta" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>

                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        % Acconto :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForpercacconto" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        % su Acconto :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForpercsuacconto" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" width="20%">
                        Percentuali :
                    </td>
                    <td class="form_textarea" width="80%">
                        <table>
                            <tr>
                                <td class="form_text" >
                                    <asp:Label ID="lblPerc1" runat="server" Text=""></asp:Label><font color="Red">*</font>
                                </td>
                                <td class="form_text" >
                                    <asp:TextBox ID="txtForperc1" runat="server" MaxLength="3" Width="50"></asp:TextBox> 
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator8" runat="server" ControlToValidate="txtForperc1" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>
       
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text" >
                                    <asp:Label ID="lblPerc2" runat="server" Text=""></asp:Label><font color="Red">*</font>
                                </td>
                                <td class="form_text" >
                                    <asp:TextBox ID="txtForperc2" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtForperc2" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td class="form_text" >
                                    <asp:Label ID="lblPerc3" runat="server" Text=""></asp:Label><font color="Red">*</font>
                                </td>
                                <td class="form_text" >
                                    <asp:TextBox ID="txtForperc3" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtForperc3" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td class="form_text" >
                                    <asp:Label ID="lblPerc4" runat="server" Text=""></asp:Label><font color="Red">*</font>
                                </td>
                                <td class="form_text" >
                                    <asp:TextBox ID="txtForperc4" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator7" runat="server" ControlToValidate="txtForperc4" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td class="form_text" >
                                    <asp:Label ID="lblPerc5" runat="server" Text=""></asp:Label><font color="Red">*</font>
                                </td>
                                <td class="form_text" >
                                    <asp:TextBox ID="txtForperc5" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator9" runat="server" ControlToValidate="txtForperc5" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>

                                </td>
                            </tr>
                            <tr>
                                <td class="form_text" >
                                    <asp:Label ID="lblPerc6" runat="server" Text=""></asp:Label><font color="Red">*</font>

                                </td>
                                <td class="form_text" >
                                    <asp:TextBox ID="txtForperc6" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                                    <asp:RequiredFieldValidator ID="RequiredFieldValidator10" runat="server" ControlToValidate="txtForperc6" ErrorMessage="" Font-Bold="True">*</asp:RequiredFieldValidator>

                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                
                <tr style="DISPLAY:none;">
                    <td class="form_text" width="20%">
                        Categorie :
                    </td>
                    <td class="form_textarea" width="80%">
                        Cate. 1: <asp:TextBox ID="txtForcat1" runat="server" MaxLength="3" Width="50" Text="1"></asp:TextBox>
                        Cate. 2: <asp:TextBox ID="txtForcat2" runat="server" MaxLength="3" Width="50" Text="2"></asp:TextBox>
                        Cate. 3: <asp:TextBox ID="txtForcat3" runat="server" MaxLength="3" Width="50" Text="3"></asp:TextBox>
                        Cate. 4: <asp:TextBox ID="txtForcat4" runat="server" MaxLength="3" Width="50" Text="4"></asp:TextBox>
                        Cate. 5: <asp:TextBox ID="txtForcat5" runat="server" MaxLength="3" Width="50" Text="5"></asp:TextBox>
                        Cate. 6: <asp:TextBox ID="txtForcat6" runat="server" MaxLength="3" Width="50" Text="6"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        IBAN :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForIBAN" runat="server" MaxLength="40" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                
                 <tr>
                    <td class="form_text" width="20%">
                        Pag. Codice :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForPagCodice" runat="server" MaxLength="3" Width="50"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" width="20%">
                        Varie per Fornitore :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:CheckBox ID="txtForoperatore" runat="server" text="Operatore"/>
                        <asp:CheckBox ID="txtForEsenzioneIvaArt15" runat="server" text="Esenzione Iva Art.15"/>
                        <asp:CheckBox ID="txtForInUso" runat="server" text ="In Uso"/>
                    </td>
                </tr>
               <tr>
                    <td class="form_text" width="20%">
                        Note :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtForNote" runat="server" TextMode="MultiLine" Rows="6" Width="98%"></asp:TextBox>
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
