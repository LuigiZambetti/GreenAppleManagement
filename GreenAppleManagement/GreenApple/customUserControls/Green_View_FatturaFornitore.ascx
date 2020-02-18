<%@ Control Language="C#" CodeFile="Green_View_FatturaFornitore.ascx.cs" Inherits="Green.Apple.Management.Green_View_FatturaFornitore" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr>
        <td class="form_Title" colspan="3">
            &nbsp;&nbsp;Amministrazione - Scheda Fornitore
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
                        <asp:LinkButton ID="lnkInserisci" Font-Bold="true" runat="server" OnClick="lnkInserisci_Click"><img src='../customLayout/images/icoNew.gif' border='0'>&nbsp;Inserisci Fattura</asp:LinkButton>
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
                                    <asp:DropDownList ID="cboAnnoFilto" runat="server" Width="120px">
                                    </asp:DropDownList>
                                </td>
                                <td class="form_text">
                                    Mese:
                                    <br />
                                    <br />
                                    <asp:DropDownList ID="cboMeseFiltro" runat="server" Width="120px">
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
                                                        [<%# DataBinder.Eval(Container, "DataItem.DFNumero")%>] 
                                                        <B><%# DataBinder.Eval(Container, "DataItem.GREEN_Fornitore")%></B>
                                                        
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<B><I>Anno : </I><B><%# DataBinder.Eval(Container, "DataItem.DFAnnoFatt")%></B> - <I>Mese : </I> <B><%# DataBinder.Eval(Container, "DataItem.DFMeseFatt")%></B>
                                                    <br /><br />
                                                </td>
                                            </tr>
                                        </table>
                                        <table ID="TBLDetail<%# DataBinder.Eval(Container, "DataItem.DCid")%>" cellpadding="0" cellspacing="2" border="0" style="width: 100%;DISPLAY:none;" valign="top">
                 
                                            <tr valign="top">
                                                <td style="border: 0;" valign="top" width="50%">
                                                    
                                                    <br /><B>Numero : </B><%# DataBinder.Eval(Container, "DataItem.DFnumero")%>
                                                    <br /><B>Data : </B><%# DataBinder.Eval(Container, "DataItem.DFdata")%>
                                                    <br /><B>Descrizione Fattura : </B><%# DataBinder.Eval(Container, "DataItem.DFdescfattura")%>
                                                    
                                                    <br /><B>Note : </B><%# DataBinder.Eval(Container, "DataItem.DFnote")%>
                                                    
                                                    <br /><B>Pagamento : </B><%# DataBinder.Eval(Container, "DataItem.PAGAMENTO")%>
                                                    <br /><B>Data Pagamento : </B><%# DataBinder.Eval(Container, "DataItem.DFdatapagamento")%>
                                                    <br /><B>Imponibile : </B><%# DataBinder.Eval(Container, "DataItem.DFimponibile")%>
                                                    <br /><B>IVA : </B><%# DataBinder.Eval(Container, "DataItem.DFIVA")%>
                                                    <br /><B>Ritenuta : </B><%# DataBinder.Eval(Container, "DataItem.DFritenuta")%>
                                                    
                                                    <br /><B>Anticipo : </B><%# DataBinder.Eval(Container, "DataItem.DFanticipo")%>
                                                    <br /><B>Totale : </B><%# DataBinder.Eval(Container, "DataItem.DFtotale")%>
                                                    
                                                    <br /><B>Banca : </B><%# DataBinder.Eval(Container, "DataItem.DFbanca")%>
                                                    <br /><B>Piazza : </B><%# DataBinder.Eval(Container, "DataItem.DFPiazza")%>
                                                    <br /><B>Banca Green Apple : </B><%# DataBinder.Eval(Container, "DataItem.DCBancagreenapple")%>
                                                    <br /><B>Riv.Prev. : </B><%# DataBinder.Eval(Container, "DataItem.DFrivprev")%>
                                                    <br /><B>Riv.Descr. : </B><%# DataBinder.Eval(Container, "DataItem.DFrivdesc")%>
                                                    <br /><B>Esenzione Iva Art15 : </B><%# DataBinder.Eval(Container, "DataItem.DFesenzioneIvaArt15")%>
                                                    
                                                    
                                                    
                                                </td>
                                                <td style="border: 0;" valign="top" width="50%">
                                                    <br /><B>Spesa 1 : </B><%# DataBinder.Eval(Container, "DataItem.DFspese")%>
                                                    <br /><B>Descr. Spesa 1 : </B><%# DataBinder.Eval(Container, "DataItem.DFdescspese")%>
                                                    <br /><B>Spesa 2 : </B><%# DataBinder.Eval(Container, "DataItem.DFspese1")%>
                                                    <br /><B>Descr. Spesa 2 : </B><%# DataBinder.Eval(Container, "DataItem.DFdescspese1")%>
                                                    <br /><B>Spesa 3 : </B><%# DataBinder.Eval(Container, "DataItem.DFspese2")%>
                                                    <br /><B>Descr. Spesa 3 : </B><%# DataBinder.Eval(Container, "DataItem.DFdescspese2")%>
                                                    <br /><B>Rag.Soc. : </B><%# DataBinder.Eval(Container, "DataItem.DFpostaRagSoc")%>
                                                    <br /><B>Indirizzo : </B><%# DataBinder.Eval(Container, "DataItem.DFpostaInd")%>
                                                    <br /><B>CAP : </B><%# DataBinder.Eval(Container, "DataItem.DFpostaCAP")%>
                                                    <br /><B>Località : </B><%# DataBinder.Eval(Container, "DataItem.DFpostaLoc")%>
                                                    <br /><B>Provincia : </B><%# DataBinder.Eval(Container, "DataItem.DFpostaProv")%>
                                                    <br /><B>Importo OverTime : </B><%# DataBinder.Eval(Container, "DataItem.DFimportoOvertime")%>
                                                    <br /><B>Importo Sfilate : </B><%# DataBinder.Eval(Container, "DataItem.DFimportoSfilate")%>
                                                </td>
                                            </tr>
                                            <tr valign="top"><td>&nbsp;</td></tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                
                                <asp:ButtonColumn CommandName="MODIFICA" Text="<img src='../customLayout/images/Edit.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn>
                                <asp:ButtonColumn  CommandName="ELIMINA" Text="<img src='../customLayout/images/Delete.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn> 
                                <asp:ButtonColumn CommandName="PRINT" Text="<img src='../customLayout/images/PRINT.gif' border='0'  title='STAMPA FATTURA'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn> 
                                
                                <asp:ButtonColumn CommandName="PRINTNOTULA" Text="<img src='../customLayout/images/LINK.gif' border='0' title='STAMPA NOTULA'>">
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
                        <a name="SectionEdit"></a>Inserimento/Modifica Fattura Fornitore
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
                        Fornitore:
                    </td>
                    <td class="form_textarea" width="80%" onkeydown="CatturaInvioFORNITORE();" >
                        <asp:TextBox ID="txtSerFornitoreSearch" runat="server" style="WIDTH:50px;"></asp:TextBox>
                        <asp:LinkButton ID="lnkCERCAFORNITORE" runat="server" Font-Bold="true" CausesValidation="false" Visible="true" onclick="lnkCERCAFORNITORE_Click">Cerca</asp:LinkButton>
                        <br />
                        <asp:DropDownList width="300px" ID="cboSerFornitore" runat="server" AutoPostBack="true" onselectedindexchanged="cboSerFornitore_SelectedIndexChanged">
                        </asp:DropDownList> 
                        <br />
                        <asp:LinkButton ID="lnkCaricaDATI" runat="server" Font-Bold="true" CausesValidation="false"
                            onclick="lnkCaricaDATI_Click">Ricarica Dati Fornitore da Anagrafica</asp:LinkButton>
                            &nbsp;
                    </td>
                </tr>
                           
                <tr>
                    <td class="form_text" width="20%">
                        Fattura Pagata?
                    </td>
                    <td class="form_textarea" width="80%" onkeydown="CatturaInvioCLIENTE();" >
                        <asp:CheckBox ID="chkPagata" runat="server" Text="La fattura risulta pagata"/> 
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text" width="20%">
                        Numero Fattura :<font color="red">*</font>
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtNumeroFattura" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                         <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" 
                            ControlToValidate="txtNumeroFattura" ErrorMessage="*"></asp:RequiredFieldValidator>
                           
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Data :<font color="red">*</font>
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
                        Data Pagamento :<font color="red">*</font>
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
                        <asp:TextBox ID="txtPagamento" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Descrizione :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtDESCR_FATT" runat="server" MaxLength="255" Width="98%" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </td>
                </tr>
                
                 <tr>
                    <td class="form_text">
                        Descrizione Spese:
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtDESCR_SPESE" runat="server" MaxLength="255" Width="98%" TextMode="MultiLine" Rows="3"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Note :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtFattNote" runat="server" MaxLength="50" Width="98%"></asp:TextBox>
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
                        Banca :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtCliBanca" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td class="form_text">
                        Piazza :
                    </td>
                    <td class="form_textarea">
                        <asp:TextBox ID="txtCliPiazza" runat="server" MaxLength="255" Width="98%"></asp:TextBox>
                    </td>
                </tr>
                
                <tr>
                    <td class="form_text">
                        Valori per Calcolo :
                    </td> 
                    <td class="form_text">
                        Anno : 
                        <asp:DropDownList ID="cboAnno" runat="server" Width="100px" AutoPostBack="true" onselectedindexchanged="cboAnno_SelectedIndexChanged">
                        </asp:DropDownList>
                        &nbsp;&nbsp;Mese :
                        <asp:DropDownList ID="cboMese" runat="server" Width="100px" AutoPostBack="true" onselectedindexchanged="cboMese_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                </tr> 
               
                <tr>
                    <td class="form_text">
                        &nbsp;
                    </td> 
                    <td class="form_text">
                        Importo Sfilate : 
                        <asp:TextBox ID="txtImpSfilate" runat="server"></asp:TextBox>
                    </td>
                </tr> 
               
                <tr>
                    <td>&nbsp;</td>
                    <td>
                        <table  cellpadding="0" cellspacing="0" width="96%" border="0" >
                            <tr>
                                <td class="form_text">
                                    Esenzione IVA Art 15 :
                                </td>
                                <td class="form_text">
                                    <asp:Label ID="HDlblIMPOEsenteIVA" runat="server" Text=""></asp:Label>
                                </td>
                            
                            </tr>
                            <tr>
                                <td class="form_text">
                                    % IVA :
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCpercIVA" runat="server" Text=""></asp:Label> 
                                </td>
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
                                    <br /><asp:Label ID="lblC_DCspese1" runat="server" Text=""></asp:Label> 
                                    <br /><asp:Label ID="lblC_DCspese2" runat="server" Text=""></asp:Label> 
                                </td>
                            </tr>
                            
                            <tr>
                                
                                <td class="form_text">
                                    <asp:HiddenField ID="HDlblIMPOIva" runat="server" />
                                    Importo IVA <asp:Label ID="lblIMPOIva" runat="server" Text=""></asp:Label> % :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCIVA" runat="server" Text=""></asp:Label> 
                                </td>
                                <td class="form_text">
                                    <asp:HiddenField ID="HDlblIMPORiten" runat="server" />
                                    Importo Ritenuta <asp:Label ID="lblIMPORiten" runat="server" Text=""></asp:Label> % :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCritenuta" runat="server" Text=""></asp:Label> 
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
                                    Importo Totale :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCtotale" runat="server" Text=""></asp:Label> 
                                </td>
                                <td class="form_text">
                                    <asp:HiddenField ID="HDlblIMPOPrev" runat="server" />
                                    Importo Prev.le <asp:Label ID="lblIMPOPrev" runat="server" Text=""></asp:Label> % :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCRivPrev" runat="server" Text=""></asp:Label> 
                                </td>
                                <td class="form_text">
                                    Netto a pagare :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblC_DCNetto" runat="server" Text=""></asp:Label> 
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

function CatturaInvioFORNITORE() {
    if (window.event.keyCode == 13) {
        __doPostBack('<%=lnkCERCAFORNITORE.UniqueID %>', '');
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
