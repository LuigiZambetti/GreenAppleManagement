<%@ Control Language="C#" CodeFile="Green_View_Fattura.ascx.cs" Inherits="Green.Apple.Management.Green_View_Fattura" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr>
        <td class="form_Title" colspan="3">
            &nbsp;&nbsp;Amministrazione - Fatture / Note Credito
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
                        <asp:LinkButton ID="lnkInserisci" Font-Bold="true" runat="server" OnClick="lnkInserisci_Click"><img src='../customLayout/images/icoNew.gif' border='0'>&nbsp;Inserisci Fattura / Nota Credito</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="box_Menu_Item">
                        <table cellpadding="0" cellspacing="0" width="50%" border="0">
                            <tr>
                                <td class="form_text">
                                    Anno:
                                    <br />
                                    <br />
                                    <asp:DropDownList ID="cboAnno" runat="server" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <td class="form_text">
                                    Mese:
                                    <br />
                                    <br />
                                    <asp:DropDownList ID="cboMese" runat="server" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <td class="form_text">
                                    Mese/Anno:
                                    <br />
                                    <br />
                                    <asp:LinkButton ID="lnkFiltroMeseAnno" runat="server" OnClick="lnkFiltroMeseAnno_Click">Filtra</asp:LinkButton>
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text">
                                    Inizio:
                                    <br />
                                    <br />
                                    <asp:TextBox Width="80px" ID="txtFiltroInizio" runat="server"></asp:TextBox>
                                    <asp:Image ID="imgtxtFiltroInizio" runat="server" ImageUrl="../customLayout/images/calendar/calendar.gif" />
                                </td>
                                <td class="form_text">
                                    Fine:
                                    <br />
                                    <br />
                                    <asp:TextBox Width="80px" ID="txtFiltroFine" runat="server"></asp:TextBox>
                                    <asp:Image ID="imgtxtFiltroFine" runat="server" ImageUrl="../customLayout/images/calendar/calendar.gif" />
                                </td>
                                <td class="form_text">
                                    Date:
                                    <br />
                                    <br />
                                    <asp:LinkButton ID="lnkFiltroDate" runat="server" OnClick="lnkFiltroDate_Click">Filtra</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
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
                                <asp:BoundColumn DataField="DCid" HeaderText="DCid" Visible="false"></asp:BoundColumn>
                                
                                <asp:TemplateColumn HeaderText="Contenuto">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="0" border="0" style="width: 100%;">
                                            
                                            <tr valign="top">
                                                <td valign="top" width="120px" align="center">
                                                    <div style="align:center;">
                                                        <a href="#" onclick="VisibileDettaglio('TBLDetail<%# DataBinder.Eval(Container, "DataItem.DCid")%>');">
                                                            <img src='../customLayout/images/Plus.gif' border='0' style="WIDTH:14px;HEIGHT:14px;">&nbsp;&nbsp;&nbsp;
                                                        </a>
                                                    </div>
                                                </td>
                                                <td style="border: 0;" width="100%">
                                                    <br />
                                                    
                                                        [<%# DataBinder.Eval(Container, "DataItem.DCid")%>] 
                                                        <B><%# DataBinder.Eval(Container, "DataItem.GREEN_Cliente")%></B>
                                                        
                                                    <B><%# DataBinder.Eval(Container, "DataItem.DCtipo")%></B> - <I>Anno : </I><B><%# DataBinder.Eval(Container, "DataItem.DCanno")%></B> - <I>Numero : </I> <B><%# DataBinder.Eval(Container, "DataItem.DCnumeroCompleto")%></B>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<%# DataBinder.Eval(Container, "DataItem.DCdescfat")%>
                                                    <br /><br />
                                                    
                                                </td>
                                            </tr>
                                        </table>
                                        <table ID="TBLDetail<%# DataBinder.Eval(Container, "DataItem.DCid")%>" cellpadding="0" cellspacing="2" border="0" style="width: 100%;DISPLAY:none;" valign="top">
                 
                                            <tr valign="top">
                                                <td style="border: 0;" valign="top" width="50%">
                                                    
                                                    <br /><B>Numero : </B><%# DataBinder.Eval(Container, "DataItem.DCnumeroCompleto")%>
                                                    <br /><B>Anno : </B><%# DataBinder.Eval(Container, "DataItem.DCanno")%>
                                                    <br /><B>Tipo : </B><%# DataBinder.Eval(Container, "DataItem.DCtipo")%>
                                                    <br /><B>Data : </B><%# DataBinder.Eval(Container, "DataItem.DCdata")%>
                                                    <br /><B>Imponibile : </B><%# DataBinder.Eval(Container, "DataItem.DCimponibile")%>
                                                    <br /><B>Spese : </B><%# DataBinder.Eval(Container, "DataItem.DCspese")%>
                                                    <br /><B>Diritti : </B><%# DataBinder.Eval(Container, "DataItem.DCdiritti")%>
                                                    <br /><B>IVA : </B><%# DataBinder.Eval(Container, "DataItem.DCIVA")%>
                                                    <br /><B>Totale : </B><%# DataBinder.Eval(Container, "DataItem.DCtotale")%>
                                                    
                                                    <br /><B>Descrizione Spese : </B><%# DataBinder.Eval(Container, "DataItem.DCdescspese")%>
                                                    <br /><B>Rag. Soc. : </B><%# DataBinder.Eval(Container, "DataItem.DCpostaRagSoc")%>
                                                    <br /><B>Indirizzo : </B><%# DataBinder.Eval(Container, "DataItem.DCpostaIndirizzo")%>
                                                    <br /><B>Località : </B><%# DataBinder.Eval(Container, "DataItem.DCpostaLocalita")%>
                                                    <br /><B>Provincia : </B><%# DataBinder.Eval(Container, "DataItem.DCpostaProv")%>
                                                    <br /><B>CAP : </B><%# DataBinder.Eval(Container, "DataItem.DCpostaCAP")%>
                                                    <br /><B>Nazione : </B><%# DataBinder.Eval(Container, "DataItem.DCpostaNazione")%>
                                                    <br /><B>Banca : </B><%# DataBinder.Eval(Container, "DataItem.DCBanca")%>
                                                    <br /><B>IBAN : </B><%# DataBinder.Eval(Container, "DataItem.DCPiazza")%>
                                                    <br /><B>Swift : </B><%# DataBinder.Eval(Container, "DataItem.DCSwift")%>
                                                    
                                                    <br /><B>Lingua : </B><%# DataBinder.Eval(Container, "DataItem.DClingua")%>
                                                    <br />
                                                    <br /><B>Valuta : </B><%# DataBinder.Eval(Container, "DataItem.DCvaluta")%>
                                                    <br /><B>Cambio : </B><%# DataBinder.Eval(Container, "DataItem.Cambio")%>
                                                    
                                                </td>
                                                <td style="border: 0;" valign="top" width="50%">

                                                    <B>Anticipi : </B><%# DataBinder.Eval(Container, "DataItem.DCanticipi")%>
                                                    <br /><B>Diritti Op. : </B><%# DataBinder.Eval(Container, "DataItem.DCdirittiop")%>
                                                    <br /><B>Trattenute : </B><%# DataBinder.Eval(Container, "DataItem.DCtrattenute")%>
                                                    <br /><B>% IVA : </B><%# DataBinder.Eval(Container, "DataItem.DCpercIVA")%>
                                                    <br /><B>% Diritti : </B><%# DataBinder.Eval(Container, "DataItem.DCpercdiritti")%>
                                                    <br /><B>Pagamento : </B><%# DataBinder.Eval(Container, "DataItem.PAGAMENTO")%>
                                                    <br /><B>Banca Green Apple : </B><%# DataBinder.Eval(Container, "DataItem.DCBancagreenapple")%>
                                                    <br /><B>Buono Ordine : </B><%# DataBinder.Eval(Container, "DataItem.DCbuonoOrdine")%>
                                                    
                                                    <br /><B>Numero Prestazioni Collegate : </B><%# DataBinder.Eval(Container, "DataItem.PRESTAZIONICOLLEGATE")%>
                                                    <br /><B>Spese 1 : </B><%# DataBinder.Eval(Container, "DataItem.DCspese1")%>
                                                    <br /><B>Descrizione Spese 1 : </B><%# DataBinder.Eval(Container, "DataItem.DCdescspese1")%>
                                                    <br /><B>Spese 2 : </B><%# DataBinder.Eval(Container, "DataItem.DCspese2")%>
                                                    <br /><B>Descrizione Spese 2 : </B><%# DataBinder.Eval(Container, "DataItem.DCdescspese2")%>
                                                    
                                                    
                                                    <br /><B>SWIFT : </B><%# DataBinder.Eval(Container, "DataItem.DCSWIFT")%>
                                                </td>
                                            </tr>
                                            <tr valign="top"><td>&nbsp;</td></tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                
                                <asp:ButtonColumn CommandName="MODIFICA" Text="<img src='../customLayout/images/Edit.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn>
                                <asp:ButtonColumn Visible="true" CommandName="ELIMINA" Text="<img src='../customLayout/images/Delete.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn> 
                                <asp:ButtonColumn CommandName="PRINT" Text="<img src='../customLayout/images/PRINT.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn> 
                                
                                <asp:ButtonColumn Visible="false" CommandName="UP" Text="<img src='../customLayout/images/Up.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn>   
                                <asp:ButtonColumn CommandName="PRINT_VALUTA" Text="<img src='../customLayout/images/PRINT_VALUTA.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn> 
                                <asp:ButtonColumn CommandName="PRINT_EFATTURA" Text="<img src='../customLayout/images/XML_EFATTURA.png' border='0'>">
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
                        <a name="SectionEdit"></a>Inserimento/Modifica Fattura/Nota
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
                        FATTURA / NOTA
                    </td>
                    <td class="form_textarea" width="80%" >
                        <asp:RadioButton ID="chkFATTURA" GroupName="FATTNOTA" runat="server" Text="FATTURA"/>
                        <asp:RadioButton ID="chkNOTA" GroupName="FATTNOTA" runat="server" Text="NOTA DI CREDITO"/>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" width="20%">
                        Numero Fattura / Nota:
                    </td>
                    <td class="form_textarea" width="80%" >
                        <asp:TextBox ID="txtNumeroFatt" runat="server" Width="100px" MaxLength="8" style="display:none;"></asp:TextBox> 
                        <asp:TextBox ID="txtNumeroFattCompleto" runat="server" Width="100px" MaxLength="8" ></asp:TextBox> 
                    </td>
                </tr>
                
               
                
                <tr>
                    <td class="form_text" width="20%">
                        Fattura Pagata?
                                        <td class="form_textarea" width="80%" onkeydown="CatturaInvioCLIENTE();" >
                        <asp:CheckBox ID="chkPagata" runat="server" Text="La fattura risulta pagata"/> 
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" width="20%">
                        Valuta:
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:DropDownList ID="cboValuta" runat="server" Width="30%">
                        </asp:DropDownList>
                        <asp:Label ID="lblVALUTA_CAMBIO" runat="server" Text=""></asp:Label>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" width="20%">
                        Selezione Cliente:
                    </td>
                    <td class="form_textarea" width="80%" onkeydown="CatturaInvioCLIENTE();" >
                        <asp:TextBox ID="txtFiltro" runat="server" Width="150px"></asp:TextBox> 
                        
                        <asp:LinkButton ID="lnkCercaCLIENTE" runat="server" Font-Bold="true" 
                            onclick="lnkCercaCLIENTE_Click">Cerca</asp:LinkButton>
                            &nbsp;
                        <asp:DropDownList ID="cboCliente" runat="server" Width="460px" 
                            AutoPostBack="True" 
                            onselectedindexchanged="cboCliente_SelectedIndexChanged">
                        </asp:DropDownList>
                        <br />
                        <asp:LinkButton ID="lnkCaricaDATI" runat="server" Font-Bold="true" 
                            onclick="lnkCaricaDATI_Click">Ricarica Dati Cliente da Anagrafica</asp:LinkButton>
                            &nbsp;
                        
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" width="20%">
                        Rag. Soc :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="lblPostaRAGSOC" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Data Fattura :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox Width="80px" ID="txtDataRiferimento" runat="server"></asp:TextBox>
                        <asp:Image ID="imgtxtDataRiferimento" runat="server" ImageUrl="../customLayout//images/calendar/calendar.gif" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" 
                            ControlToValidate="txtDataRiferimento" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                
                 <tr>
                    <td class="form_text">
                        Data Pagamento :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox Width="80px" ID="txtDataPagamento" runat="server"></asp:TextBox>
                        <asp:Image ID="imgtxtDataPagamento" runat="server" ImageUrl="../customLayout//images/calendar/calendar.gif" />
                        <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" 
                            ControlToValidate="txtDataPagamento" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Pagamento :
                    </td>
                    <td class="form_textarea">
                        <asp:DropDownList ID="cboPagamento" runat="server">
                        </asp:DropDownList>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Indirizzo :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="lblPostaINDIRIZZO" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Località :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="lblPostaLOCALITA" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Provincia :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="lblPostaPROVINCIA" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        CAP :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="lblPostaCAP" runat="server" MaxLength="5" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Nazione :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="lblPostaNAZIONE" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Banca :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtCliBanca" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text">
                        IBAN :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtCliPiazza" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text">
                        Swift :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtCliSwift" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Descrizione Fattura :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtDESCR_FATT" runat="server" MaxLength="255" Width="98%" TextMode="MultiLine" Rows="4"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Descrizione Spese :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtDESCR_SPESE" runat="server" MaxLength="255" Width="98%" TextMode="MultiLine" Rows="4"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <table  cellpadding="0" cellspacing="0" width="96%" border="0" >
                            <tr>
                                <td class="form_textarea" colspan="8">
                                    <br />
                                    <i>
                                    Per applicare una percentuale di sconto oppure modificare la percentuale dei diritti modificare il valore nella casella e premere invio. Alla fine della procedura selezionare "Applica modifiche fattura" per salvare definitivamente.
                                    </i>
                                    <br /><br />
                                </td>
                            </tr>
                            <tr>
                                <td class="form_text">
                                    % Sconto :
                                </td>
                                <td class="form_textarea" onkeydown="CambioPercDiritti();" colspan="2">
                                    <asp:TextBox ID="txtManualPercSconto" runat="server" MaxLength="4" 
                                        Width="30px" ></asp:TextBox>
                                    <asp:Label ID="lblImportoSconto" runat="server" Text=""></asp:Label>    
                                </td>
                            <tr>
                                
                                <td class="form_text">
                                    Imponibile :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCimponibile" runat="server" Text=""></asp:Label> 
                                </td>
                                <td class="form_text">
                                    Importo Spese :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCspese" runat="server" Text=""></asp:Label> 
                                </td>
                                <td class="form_text">
                                    Importo Trattenute :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCtrattenute" runat="server" Text=""></asp:Label> 
                                </td>
                                <td class="form_text">
                                    Importo Anticipi :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCanticipi" runat="server" Text=""></asp:Label> 
                                </td>
                            </tr>
                            
                            <tr>
                                <td class="form_text">
                                    % Diritti :
                                </td>
                                <td class="form_textarea" onkeydown="CambioPercDiritti();" >
                                    <asp:Label ID="lblC_DCpercdiritti" runat="server" Text="" Visible="false"></asp:Label> 
                                    <asp:TextBox ID="txtManualPercDiritti" runat="server" MaxLength="3" 
                                        Width="30px" ></asp:TextBox>
                                    <asp:LinkButton ID="lnkCambioDiritti" runat="server" Visible="true" onclick="lnkCambioDiritti_Click">.</asp:LinkButton>
                                </td>
                                <td class="form_text">
                                    Diritti Agenzia:
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCdiritti" runat="server" Text=""></asp:Label> 
                                </td>
                                
                                <td class="form_text">
                                    % IVA :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCpercIVA" runat="server" Text=""></asp:Label> 
                                </td>
                                
                                
                                <td class="form_text">
                                    Importo IVA :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCIVA" runat="server" Text=""></asp:Label> 
                                </td>
                                
                            </tr>
                            
                            <tr>
                                <td class="form_text">
                                    Importo Totale :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCtotale" runat="server" Text=""></asp:Label> 
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" colspan="2" align="right">
                        <asp:LinkButton ID="lnkAnnulla" Font-Bold="true" runat="server" OnClick="lnkAnnulla_Click" CausesValidation="False"><img id="Img1" src="../customLayout/images/icoClose.gif" alt="" border="0" />&nbsp;Annulla Modifica Fattura&nbsp;</asp:LinkButton><asp:LinkButton ID="lnkAggiorna" Font-Bold="true" runat="server" OnClick="lnkAggiorna_Click"><img id="Img1" src="../customLayout/images/icoConfirm.gif" alt="" border="0" />&nbsp;Applica Modifiche Fattura&nbsp;</asp:LinkButton>&nbsp;&nbsp;
                    </td>
                    
                </tr>
 
                <tr><td>&nbsp;</td></tr>
                
                <tr>
                <td>
                    &nbsp;
                </td>
                <td class="form_textarea" width="80%">
                    <div class="DivMessageBox2" id="divRigheFattura" runat="server">
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr valign="top">
                                <td align="center" class="DivMessageBox_Titolo">
                                    <table width="100%" cellpadding="0" cellspacing="0" border="0">
                                        <tr>
                                            <td colspan="2" align="center">
                                                Selezione Prestazioni per Fattura
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td colspan="2" align="center" class="DivMessageBox_Testo">
                                                <asp:Label ID="Label1" runat="server" Text=""></asp:Label>       
                                            </td>
                                        </tr>
                                        <tr valign="top">
                                            <td align="center" class="" colspan="2">
                                                <table cellpadding="0" cellspacing="0" border="0">
                                                    <tr>
                                                        <td class="form_text2" colspan="2">
                                                            <asp:DataGrid ID="gridPrestazioniLibere" runat="server" 
                                                                Width="100%" AutoGenerateColumns="False" 
                                                                CssClass="webgridGeneral" AllowPaging="True"  CellPadding="5" PageSize="50"
                                                                GridLines="Horizontal" >
                                                                <SelectedItemStyle CssClass="webgridSelectedRowStyleDefault" />
                                                                <AlternatingItemStyle CssClass="webgridRowAlternateStyleDefault" />
                                                                <ItemStyle CssClass="webgridRowStyleDefault" />
                                                                <HeaderStyle CssClass="webgridHeaderStyleDefault" />
                                                                <Columns>
                                                                    <asp:TemplateColumn HeaderText="&nbsp;&nbsp;Prestazioni">
                                                                        <ItemTemplate>
                                                                            <table cellpadding="0" cellspacing="2" border="0" style="width: 100%;" valign="top">
                                                                                <tr>
                                                                                    <td rowspan="10" valign="top">
                                                                                        <img src="../customLayout/images/cruscotto/<%# DataBinder.Eval(Container, "DataItem.COLORE")%>.gif" alt="<%# DataBinder.Eval(Container, "DataItem.COLORE")%>"/>&nbsp;&nbsp;&nbsp;
                                                                                        <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                                                                                    </td>
                                                                                </tr>
                                                                                <tr valign="top">
                                                                                    <td style="border: 0;" colspan="2">
                                                                                        <h5>
                                                                                            <asp:CheckBox ID="chkSelPrestazione" runat="server"/>
                                                                                            <%# DataBinder.Eval(Container, "DataItem.GREEN_Cliente")%>
                                                                                        </h5>
                                                                                        [Numero <%# DataBinder.Eval(Container, "DataItem.PRnumero")%>] <I> Dal : </I><B><%# DataBinder.Eval(Container, "DataItem.DATAINIZIO")%></B><I> Al : </I><B><%# DataBinder.Eval(Container, "DataItem.DATAFINE")%></B>&nbsp;(Servizi Collegati : <%# DataBinder.Eval(Container, "DataItem.NumServizi")%>)
                                                                                    </td>
                                                                                </tr>
                                                                                <tr valign="top">
                                                                                    <td style="border: 0;" colspan="2" >
                                                                                        <br />
                                                                                        <br /><B><%# DataBinder.Eval(Container, "DataItem.PRnote")%></B>
                                                                                        <br /><br />
                                                                                    </td>
                                                                                </tr>
                                                                                
                                                                                <tr>
                                                                                    <td valign="top" width="50%">
                                                                                        <table cellpadding="0" cellspacing="0" border="0" style="WIDTH:100%;HEIGHT:100%;">
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>Categoria :</B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                    <%# DataBinder.Eval(Container, "DataItem.CATDESCR")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>Imponibile : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                    <%#String.Format("{0:N2}",DataBinder.Eval(Container, "DataItem.PRImponibile"))%>
                                                                                                    &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>Diritti : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                    <%# String.Format("{0:N2}",DataBinder.Eval(Container, "DataItem.PRdiritti"))%>
                                                                                                    &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>% Diritti : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                    <%# DataBinder.Eval(Container, "DataItem.PRtipodiritti")%> %
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>IVA : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                     <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRimportoIVA"))%>
                                                                                                    &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>% IVA : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                    <%# DataBinder.Eval(Container, "DataItem.PRpctIVA")%> %
                                                                                                </td>
                                                                                            </tr>
                                                                                            
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>Totale Prestazione: </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                    <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRtotale"))%>
                                                                                                    &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>Overtime : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                    <%# DataBinder.Eval(Container, "DataItem.PRovertime")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                    
                                                                                    <td valign="top" width="50%">
                                                                                        <table cellpadding="0" cellspacing="0" border="0" style="WIDTH:100%;HEIGHT:100%;">
                                                                                            <tr>
                                                                                                <!--<td class="TDcol">
                                                                                                    <B><%-- <%# DataBinder.Eval(Container, "DataItem.DCTipo")%> --%>: </B>
                                                                                                </td>-->
                                                                                                <td class="TDcol">
                                                                                                    Anno <%# DataBinder.Eval(Container, "DataItem.ANNO")%> 
                                                                                                    - Numero <%# DataBinder.Eval(Container, "DataItem.NUMERO")%>
                                                                                                    ( <%# DataBinder.Eval(Container, "DataItem.DATAFATTURA")%> )
                                                                                                    - <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRtotalefattura"))%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>Chiave : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                    <%# DataBinder.Eval(Container, "DataItem.PRchiave")%> 
                                                                                                </td>
                                                                                            </tr>
                                                                                            
                                                                                            
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>Pagamento : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                     <%# DataBinder.Eval(Container, "DataItem.PAGAMENTO")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            
                                                                                            
                                                                                            
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>Anticipi : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                    <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRanticipi"))%>
                                                                                                    &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>Trattenute : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                     <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRtrattenute"))%>
                                                                                                    &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>Rivalsa : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                     <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRrivalsa"))%>
                                                                                                    &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>Spese : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                     <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRspese"))%>
                                                                                                     <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRspese1"))%>
                                                                                                     <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRspese2"))%>
                                                                                                    &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                                                                </td>
                                                                                            </tr>
                                                                                            <tr>
                                                                                                <td class="TDcol">
                                                                                                    <B>IVA Spese : </B>
                                                                                                </td>
                                                                                                <td class="TDcol">
                                                                                                    <%# DataBinder.Eval(Container, "DataItem.PRivaspese")%> %
                                                                                                </td>
                                                                                            </tr>
                                                                                        </table>
                                                                                    </td>
                                                                                </tr>
                                                                                
                                                                            </table>
                                                                        </ItemTemplate>
                                                                    </asp:TemplateColumn>
                                                                </Columns>
                                                                <PagerStyle Visible="False" CssClass="webgridPagerStyleDefault" Mode="NumericPages" />
                                                            </asp:DataGrid>
                            
                                                        </td>
                                                    </tr>
                                                    
                                                    <tr>
                                                        <td class="form_text2">
                                                            &nbsp;
                                                        </td>
                                                        <td class="form_text2">
                                                            <img id="img2" runat="server" src="../customLayout/images/icoClose.gif" alt="" border="0" /><asp:LinkButton 
                                                                ID="lnkCloseRiga" runat="server" CssClass="Link_Action" 
                                                                CausesValidation="False" onclick="lnkCloseRiga_Click">&nbsp;&nbsp;Annulla Modifica Prestazioni</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                                <img id="img3" runat="server" src="../customLayout/images/icoConfirm.gif" alt="" border="0" /><asp:LinkButton 
                                                                ID="lnkAggiornaRiga" runat="server" CssClass="Link_Action" 
                                                                CausesValidation="False" onclick="lnkAggiornaRiga_Click">&nbsp;&nbsp;Aggiungi Prestazioni</asp:LinkButton>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                                                        </td>
                                                    </tr>
                                                    
                                                </table>   
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </div>
                </td>
                </tr>
                
                <tr>
                    <td class="form_text" width="20%">
                        Prestazioni :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:LinkButton ID="lnkInserisciRiga" Font-Bold="true" runat="server" 
                            onclick="lnkInserisciRiga_Click"><img src='../customLayout/images/icoNew.gif' border='0'>&nbsp;Aggiungi altre Prestazioni Cliente</asp:LinkButton>
                            <br /><br />
                         <asp:DataGrid ID="gridPrestazioni" runat="server" 
                            Width="100%" AutoGenerateColumns="False" 
                            CssClass="webgridGeneral" AllowPaging="True"  CellPadding="5" PageSize="50"
                            GridLines="Horizontal" onitemcommand="gridPrestazioni_ItemCommand" >
                            <SelectedItemStyle CssClass="webgridSelectedRowStyleDefault" />
                            <AlternatingItemStyle CssClass="webgridRowAlternateStyleDefault" />
                            <ItemStyle CssClass="webgridRowStyleDefault" />
                            <HeaderStyle CssClass="webgridHeaderStyleDefault" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="&nbsp;&nbsp;Prestazioni">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="2" border="0" style="width: 100%;" valign="top">
                                            <tr>
                                                <td rowspan="10" valign="top">
                                                    <img src="../customLayout/images/cruscotto/<%# DataBinder.Eval(Container, "DataItem.COLORE")%>.gif" alt="<%# DataBinder.Eval(Container, "DataItem.COLORE")%>"/>&nbsp;&nbsp;&nbsp;
                                                    <br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br /><br />
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td style="border: 0;" colspan="2">
                                                    <h5>
                                                        <%# DataBinder.Eval(Container, "DataItem.GREEN_Cliente")%>
                                                    </h5>
                                                    [Numero <%# DataBinder.Eval(Container, "DataItem.PRnumero")%>] <I> Dal : </I><B><%# DataBinder.Eval(Container, "DataItem.DATAINIZIO")%></B><I> Al : </I><B><%# DataBinder.Eval(Container, "DataItem.DATAFINE")%></B>&nbsp;(Servizi Collegati : <%# DataBinder.Eval(Container, "DataItem.NumServizi")%>)
                                                </td>
                                            </tr>
                                            <tr valign="top">
                                                <td style="border: 0;" colspan="2" >
                                                    <br />
                                                    <br /><B><%# DataBinder.Eval(Container, "DataItem.PRnote")%></B>
                                                    <br /><br />
                                                </td>
                                            </tr>
                                            
                                            <tr>
                                                <td valign="top" width="50%">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="WIDTH:100%;HEIGHT:100%;">
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Categoria :</B>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# DataBinder.Eval(Container, "DataItem.CATDESCR")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Imponibile : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%#String.Format("{0:N2}",DataBinder.Eval(Container, "DataItem.PRImponibile"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Diritti : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# String.Format("{0:N2}",DataBinder.Eval(Container, "DataItem.PRdiritti"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>% Diritti : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# DataBinder.Eval(Container, "DataItem.PRtipodiritti")%> %
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>IVA : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                 <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRimportoIVA"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>% IVA : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# DataBinder.Eval(Container, "DataItem.PRpctIVA")%> %
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Totale Prestazione: </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRtotale"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Overtime : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# DataBinder.Eval(Container, "DataItem.PRovertime")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                
                                                <td valign="top" width="50%">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="WIDTH:100%;HEIGHT:100%;">
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Fattura : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                Anno <%# DataBinder.Eval(Container, "DataItem.ANNO")%> 
                                                                - Numero <%# DataBinder.Eval(Container, "DataItem.NUMERO")%>
                                                                ( <%# DataBinder.Eval(Container, "DataItem.DATAFATTURA")%> )
                                                                - <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRtotalefattura"))%>
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Chiave : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# DataBinder.Eval(Container, "DataItem.PRchiave")%> 
                                                            </td>
                                                        </tr>
                                                        
                                                        
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Pagamento : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                 <%# DataBinder.Eval(Container, "DataItem.PAGAMENTO")%>
                                                            </td>
                                                        </tr>
                                                        
                                                        
                                                        
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Anticipi : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRanticipi"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Trattenute : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                 <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRtrattenute"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Rivalsa : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                 <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRrivalsa"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>Spese : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                 <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRspese"))%>
                                                                 <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRspese1"))%>
                                                                 <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRspese2"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <B>IVA Spese : </B>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# DataBinder.Eval(Container, "DataItem.PRivaspese")%> %
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                            
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                
                                <asp:ButtonColumn CommandName="ELIMINA" Text="<img src='../customLayout/images/Delete.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn> 
                                
                                <asp:ButtonColumn CommandName="VIEW" Text="<img src='../customLayout/images/Link.gif' border='0'>">
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
</table>


        
<script>
    function CatturaInvio() {
        if (window.event.keyCode == 13) {
            __doPostBack('<%=lnkCerca.UniqueID %>', '');
	}
}
function CatturaInvioCLIENTE() {
    if (window.event.keyCode == 13) {
        __doPostBack('<%=lnkCercaCLIENTE.UniqueID %>', '');
	}
}
function CambioPercDiritti() {
    if (window.event.keyCode == 13) {
        __doPostBack('<%=lnkCambioDiritti.UniqueID %>', '');
    }
}


function VisibileDettaglio(code) {
    var TBLDettaglio = document.getElementById(code);
    if (TBLDettaglio.style.display != 'none') {
        TBLDettaglio.style.display = 'none';
    }
    else {
        TBLDettaglio.style.display = 'block';
    }

}
</script>
<asp:ValidationSummary ID="vldSummary" runat="server" ShowMessageBox="True" ShowSummary="True" />
