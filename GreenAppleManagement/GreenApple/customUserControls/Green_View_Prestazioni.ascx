<%@ Control Language="C#" CodeFile="Green_View_Prestazioni.ascx.cs" Inherits="Green.Apple.Management.Green_View_Prestazioni" %>
<style>
    .TDcol
    {
        border-bottom: 1px solid #DDDDDD !important;
    }
</style>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr>
        <td class="form_Title" colspan="3">
            &nbsp;&nbsp;Amministrazione - Prestazioni e Servizi
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
                        <asp:LinkButton ID="lnkCerca" runat="server" Font-Bold="true" OnClick="lnkCerca_Click"
                            CausesValidation="false">Cerca</asp:LinkButton>
                    </td>
                </tr>
            </table>
        </td>
        <td width="5">
        </td>
        <td class="box_Data_Admin">
            <table cellpadding="0" cellspacing="0" width="100%" border="0">
                <tr>
                    <td class="box_Menu_Item" colspan=2>
                        <asp:LinkButton ID="lnkInserisci" Font-Bold="true" runat="server" OnClick="lnkInserisci_Click"><img src='../customLayout/images/icoNew.gif' border='0'>&nbsp;Inserisci Prestazione</asp:LinkButton>
                    </td>
                </tr>
                <tr>
                    <td class="box_Menu_Item">
                        <table>
                            <tr>
                                <td class="form_text">
                                    <asp:LinkButton ID="lnkFilterESEGUITO" runat="server" 
                                        onclick="lnkFilterESEGUITO_Click" ><img border='0' src='../customLayout/images/cruscotto/ESEGUITO.gif' /></asp:LinkButton>
                                </td>
                                <td class="form_text">
                                    Emessa
                                </td>
                                <td class="form_text">
                                    <asp:LinkButton ID="lnkFilterDAFATTURARE" runat="server" OnClick="lnkFilterDAFATTURARE_Click"><img border='0' src='../customLayout/images/cruscotto/DA FATTURARE.gif' /></asp:LinkButton>
                                </td>
                                <td class="form_text">
                                    Da Fatturare
                                </td>
                                <td class="form_text">
                                    <asp:LinkButton ID="lnkFilterFATTURATA" runat="server" OnClick="lnkFilterFATTURATA_Click"><img border='0' src='../customLayout/images/cruscotto/FATTURATA.gif' /></asp:LinkButton>
                                </td>
                                <td class="form_text">
                                    Fatturata
                                </td>
                                <td class="form_text">
                                    <asp:LinkButton ID="lnkFilterPAGATA" runat="server" OnClick="lnkFilterPAGATA_Click"><img border='0' src='../customLayout/images/cruscotto/PAGATA.gif' /></asp:LinkButton>
                                </td>
                                <td class="form_text">
                                    Pagata
                                </td>
                                <%--<td class="form_text">
                                    <asp:LinkButton ID="lnkFilterESCLUDIPAGATI" runat="server" 
                                        onclick="lnkFilterESCLUDIPAGATI_Click"><img border='0' src='../customLayout/images/cruscotto/NONPAGATI.gif' />
                                    </asp:LinkButton>
                                </td>
                                <td class="form_text">
                                    Escludi Pagati
                                </td>--%>
                                <td class="form_text" width="200px">
                                </td>
                                <td class="form_text">
                                    <asp:Label ID="lblFiltroSel" runat="server" Text=""></asp:Label>
                                    <br />
                                    <asp:LinkButton ID="lnkFilterRemove" runat="server" OnClick="lnkFilterRemove_Click">Rimuovi Filtro</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr id="RWPrest12" runat="server" >
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
                    <td class="box_Menu_Item">
                        <h3>Totale Periodo : <asp:Label ID="ltlTOTALEPERIODO" runat="server" Text="" Width="100%"></asp:Label></h3>
                        <h3>Totale Redazionale : <asp:Label ID="ltlTOTALEPERIODOREDAZIONALE" runat="server" Text="" Width="100%"></asp:Label></h3>
                    </td>
                </tr>
                <tr>
                    <td colspan="3" valign="top">
                        <asp:DataGrid ID="gridPrestazioni" runat="server" Width="100%" AutoGenerateColumns="False"
                            CssClass="webgridGeneral" AllowPaging="False" CellPadding="5" PageSize="1000" GridLines="Horizontal"
                            OnItemCommand="gridPrestazioni_ItemCommand">
                            <SelectedItemStyle CssClass="webgridSelectedRowStyleDefault" />
                            <AlternatingItemStyle CssClass="webgridRowAlternateStyleDefault" />
                            <ItemStyle CssClass="webgridRowStyleDefault" />
                            <HeaderStyle CssClass="webgridHeaderStyleDefault" />
                            <Columns>
                                <asp:TemplateColumn HeaderText="&nbsp;&nbsp;Prestazioni">
                                    <ItemTemplate>
                                        <table cellpadding="0" cellspacing="1" border="0" style="width: 100%;" valign="top">
                                            <tr valign="top">
                                                <td valign="top" width="120px">
                                                    <div style="float:left;vertical-align:middle;">
                                                        <br /><a href="#" onclick="VisibileDettaglio('TBLDetail<%# DataBinder.Eval(Container, "DataItem.PRNumero")%>');">
                                                            <img src='../customLayout/images/Plus.gif' border='0' style="width: 14px; height: 14px;">
                                                        </a>
                                                    </div>
                                                    
                                                    <div style="float:left;">
                                                    &nbsp;&nbsp;
                                                        <img src="../customLayout/images/cruscotto/<%# DataBinder.Eval(Container, "DataItem.COLORE")%>.gif"
                                                        alt="<%# DataBinder.Eval(Container, "DataItem.COLORE")%>" />
                                                    </div>                                                    
                                                </td>
                                                <td style="border: 0;" width="92%">
                                                    <b>
                                                        <%# DataBinder.Eval(Container, "DataItem.GREEN_Cliente")%>
                                                    </b>&nbsp;&nbsp;&nbsp;[Numero
                                                    <%# DataBinder.Eval(Container, "DataItem.PRnumero")%>] <i>Dal : </i><b>
                                                        <%# DataBinder.Eval(Container, "DataItem.DATAINIZIO")%></b><i> Al : </i><b>
                                                            <%# DataBinder.Eval(Container, "DataItem.DATAFINE")%></b>&nbsp;(Servizi
                                                    Collegati :
                                                    <%# DataBinder.Eval(Container, "DataItem.NumServizi")%>)
                                                    <br />Chiave:&nbsp;&nbsp;
                                                    <font style="font-size:9px;font-weight:normal;"><%# DataBinder.Eval(Container, "DataItem.PRChiave")%></font>
                                                </td>
                                            </tr>
                                            <tr style="display:none;">
                                                <td class="TDcol">
                                                    <b>Categoria :</b>
                                                    <%# DataBinder.Eval(Container, "DataItem.CATDESCR")%>
                                                    &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; <b>Chiave : </b>
                                                    <%# DataBinder.Eval(Container, "DataItem.PRchiave")%>
                                                </td>
                                            </tr>
                                        </table>
                                        <table id="TBLDetail<%# DataBinder.Eval(Container, "DataItem.PRNumero")%>" cellpadding="0"
                                            cellspacing="2" border="0" style="width: 100%; display: none;" valign="top">
                                            <tr valign="top">
                                                <td style="border: 0;">
                                                    <br />
                                                    <br />
                                                    <b>
                                                        <%# DataBinder.Eval(Container, "DataItem.PRnote")%></b>
                                                    <br />
                                                    <br />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td valign="top" width="50%">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 100%;">
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>Imponibile : </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%#String.Format("{0:N2}",DataBinder.Eval(Container, "DataItem.PRImponibile"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>Dir. Agenzia: </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# String.Format("{0:N2}",DataBinder.Eval(Container, "DataItem.PRdiritti"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>% Dir. Agenzia : </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# DataBinder.Eval(Container, "DataItem.PRtipodiritti")%>
                                                                %
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>IVA : </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRimportoIVA"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>% IVA : </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# DataBinder.Eval(Container, "DataItem.PRpctIVA")%>
                                                                %
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>Totale Prestazione: </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRtotale"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>Overtime : </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# DataBinder.Eval(Container, "DataItem.PRovertime")%>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                                <td valign="top" width="50%">
                                                    <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 100%;">
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>Fattura : </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                Anno
                                                                <%# DataBinder.Eval(Container, "DataItem.ANNO")%>
                                                                - Numero
                                                                <%# DataBinder.Eval(Container, "DataItem.NUMERO")%>
                                                                (
                                                                <%# DataBinder.Eval(Container, "DataItem.DATAFATTURA")%>
                                                                ) -
                                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRtotalefattura"))%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>Pagamento : </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# DataBinder.Eval(Container, "DataItem.PAGAMENTO")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>Anticipi : </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRanticipi"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>Trattenute : </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRtrattenute"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>Rivalsa : </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.PRrivalsa"))%>
                                                                &nbsp;<%# DataBinder.Eval(Container, "DataItem.VALUTA")%>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td class="TDcol">
                                                                <b>Spese : </b>
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
                                                                <b>IVA Spese : </b>
                                                            </td>
                                                            <td class="TDcol">
                                                                <%# DataBinder.Eval(Container, "DataItem.PRivaspese")%>
                                                                %
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </ItemTemplate>
                                </asp:TemplateColumn>
                                <asp:ButtonColumn CommandName="MODIFICA" Text="<img src='../customLayout/images/Edit.gif' border='0'>">
                                    <HeaderStyle Width="25px" />
                                </asp:ButtonColumn>
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
                <tr>
                    <td colspan = 2>
                        <table id="TBLModifica" runat="server" cellpadding="0" cellspacing="0" width="100%"
                            border="0" style="border: 1px solid #999999;">
                            <tr >
                                <td colspan="4" class="form_AnagraficaInsert">
                                    <a name="SectionEdit"></a>
                                Inserimento/Modifica Prestazione :
                            </tr>
                            <tr id="RWPrest11" runat="server" >
                                <td class="form_Error_Message" colspan="4" align="left">
                                    <asp:Label ID="lblError" runat="server" Text="" Width="100%"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td width="20%">
                                </td>
                                <td width="30%">
                                </td>
                                <td width="20%">
                                </td>
                                <td width="30%">
                                </td>
                            </tr>
                            <tr  id="RWPrest10" runat="server" >
                                <td class="form_text">
                                    Fatturazione:
                                </td> 
                                <td class="form_textarea" colspan="3"> 
                                    <asp:CheckBox ID="chkDAFATTURARE" runat="server" 
                                        Text="Prestazione da fatturare?" AutoPostBack="True" 
                                        oncheckedchanged="chkDAFATTURARE_CheckedChanged"/>
                                </td>
                            </tr>
                            <tr id="RWPrest9" runat="server" >
                                <td class="form_text">
                                    Selezione Cliente:<font color="red"><b>*</b></font>
                                    <asp:RangeValidator ID="RangeValidator4" runat="server" ErrorMessage="" ControlToValidate="cboCliente"
                                        MinimumValue="1" MaximumValue="99999999"></asp:RangeValidator>
                                </td>
                                <td class="form_textarea" colspan="3" onkeydown="CatturaInvioCLIENTE();">
                                    <asp:TextBox ID="txtFiltro" runat="server" Width="100px"></asp:TextBox>
                                    <asp:LinkButton ID="lnkCercaCLIENTE" runat="server" Font-Bold="true" OnClick="lnkCercaCLIENTE_Click"
                                        CausesValidation="false">Cerca</asp:LinkButton>
                                    &nbsp;
                                    <asp:DropDownList ID="cboCliente" runat="server" Width="520px" AutoPostBack="True"
                                        OnSelectedIndexChanged="cboCliente_SelectedIndexChanged">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr id="RWPrest8" runat="server" >
                                <td class="form_text">
                                    Rag. Soc :
                                </td>
                                <td class="form_textarea">
                                    <asp:Label ID="lblPostaRAGSOC" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="form_textarea" colspan="2">
                                    <asp:Label ID="lblPostaINDIRIZZO" runat="server" Text=""></asp:Label>
                                    &nbsp;<asp:Label ID="lblPostaCAP" runat="server" Text=""></asp:Label>
                                    &nbsp;<asp:Label ID="lblPostaLOCALITA" runat="server" Text=""></asp:Label>
                                    &nbsp;(<asp:Label ID="lblPostaPROVINCIA" runat="server" Text=""></asp:Label>) &nbsp;<asp:Label
                                        ID="lblPostaNAZIONE" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr  id="RWPrest7" runat="server" >
                                <td class="form_text">
                                    Banca :
                                </td>
                                <td class="form_textarea" colspan="3">
                                    <asp:Label ID="txtCliBanca" runat="server" Text=""></asp:Label>
                                    &nbsp;(<asp:Label ID="txtCliPiazza" runat="server" Text=""></asp:Label>)
                                </td>
                            </tr>
                            <tr id="RWPrest6" runat="server" >
                                <td class="form_text">
                                    Categoria :<font color="red"><b>*</b></font>
                                    <asp:RangeValidator ID="RangeValidator5" runat="server" ErrorMessage="" ControlToValidate="cboCategoria"
                                        MinimumValue="1" MaximumValue="99999999"></asp:RangeValidator>
                                </td>
                                <td class="form_textarea">
                                    <asp:DropDownList ID="cboCategoria" runat="server" Width="90%">
                                    </asp:DropDownList>
                                </td>
                                <td class="form_text">
                                    Lingua :
                                </td>
                                <td class="form_textarea">
                                    <asp:DropDownList ID="cboLingua" runat="server" Width="90%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr  id="RWPrest5" runat="server" >
                                <td class="form_text">
                                    Valuta :
                                </td>
                                <td class="form_textarea">
                                    <asp:DropDownList ID="cboValuta" runat="server" Width="90%">
                                    </asp:DropDownList>
                                </td>
                                <td class="form_text">
                                    Parola Chiave :
                                </td>
                                <td class="form_textarea">
                                    <asp:TextBox ID="txtParolaChiave" runat="server" Width="100%" MaxLength="30"></asp:TextBox>
                                </td>
                            </tr>
                            <tr  id="RWPrest4" runat="server" >
                                <td class="form_text">
                                    Data Inizio :
                                </td>
                                <td class="form_textarea">
                                    <asp:TextBox Width="80px" ID="txtDataInizio" runat="server"></asp:TextBox>
                                    <asp:Image ID="imgDataInizio" runat="server" ImageUrl="../customLayout//images/calendar/calendar.gif" />
                                </td>
                                <td class="form_text">
                                    Data Fine :
                                </td>
                                <td class="form_textarea">
                                    <asp:TextBox Width="80px" ID="txtDataFine" runat="server"></asp:TextBox>
                                    <asp:Image ID="imgDataFine" runat="server" ImageUrl="../customLayout//images/calendar/calendar.gif" />
                                </td>
                            </tr>
                            <tr  id="RWPrest3" runat="server" >
                                <td class="form_text">
                                    Descrizione :
                                </td>
                                <td class="form_textarea" colspan="3">
                                    <asp:TextBox Width="100%" ID="txtDescrizione" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="RWPrest2" runat="server" >
                                <td>
                                    &nbsp;
                                </td>
                                <td colspan="3">
                                    <table cellpadding="0" cellspacing="0" width="100%" border="0">
                                        <tr>
                                            <td class="TDcol form_text">
                                                Giorni:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_GIORNI" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="TDcol form_text">
                                                Imponibile:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_IMPONIBILE" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="TDcol form_text">
                                                Imp. OverTime:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_OVERTIME" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="TDcol form_text">
                                                Importo Lordo:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_TOTALE" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text">
                                                % Dir. Agenzia:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_PERCDIRITTI" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="TDcol form_text">
                                                Dir. Agenzia:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_DIRITTI" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="TDcol form_text">
                                                % IVA:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_PERCIVA" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="TDcol form_text">
                                                IVA:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_IMPORTOIVA" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text">
                                                Spese:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_SPESE" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr style="display: none;">
                                            <td class="TDcol form_text">
                                                IVA Spese:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_IVASPESE" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="TDcol form_text">
                                                Anticipi:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_ANTICIPI" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="TDcol form_text">
                                                Rivalsa:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_RIVALSA" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="TDcol form_text">
                                                TratteTratteTrattenute:
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:Label ID="lblCALCOLO_TRATTENUTE" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr id="RWPrest1" runat="server" >
                                <td class="form_text" colspan="4" align="right">
                                    <asp:LinkButton ID="lnkPrestAnnulla" OnClick="lnkPrestAnnulla_Click" Font-Bold="true"
                                        runat="server" CausesValidation="False"><img id="Img2" src="../customLayout/images/icoClose.gif" alt="" border="0" />&nbsp;Annulla Modifica Prestazione&nbsp;</asp:LinkButton>
                                    <asp:LinkButton ID="lnkPrestAggiorna" OnClick="lnkPrestAggiorna_Click" Font-Bold="true"
                                        runat="server"><img id="Img3" src="../customLayout/images/icoConfirm.gif" alt="" border="0" />&nbsp;Applica Modifica Prestazione&nbsp;</asp:LinkButton>&nbsp;&nbsp;
                                    <asp:LinkButton ID="lnkPrestFATTURACLIENTE" OnClick="lnkPrestFATTURACLIENTE_Click"
                                        Font-Bold="true" runat="server"><img id="Img4" src="../customLayout/images/Calcola.gif" alt="" border="0" />&nbsp;FATTURA/NOTA CLIENTE&nbsp;</asp:LinkButton>&nbsp;&nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" class="form_text">
                                    &nbsp;
                                </td>
                            </tr>
                            <tr>
                                <td class="box_Menu_Item">
                                    <asp:LinkButton ID="lnkSerInserisci" Font-Bold="true" runat="server" OnClick="lnkSerInserisci_Click"><img src='../customLayout/images/icoNew.gif' border='0'>&nbsp;Inserisci Servizio</asp:LinkButton>
                                </td>
                            </tr>
                            <tr id="RWServizioMOD" runat="server" class="form_text">
                                <td colspan="4" class="form_text">
                                    <table class="RWServizioMOD" cellpadding="0" cellspacing="0" border="0" style="width: 100%;
                                        height: 100%; background-color: #DDDDDD;">
                                        <tr>
                                            <td class="TDcol form_text" colspan="8">
                                                Inserimento/Modifica Servizio:
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text">
                                                <b>Competenza: :</b>
                                            </td>
                                            <td class="TDcol form_text" colspan="5">
                                                <asp:DropDownList ID="cboSerAnno" runat="server" Width="80px">
                                                </asp:DropDownList>
                                                <asp:DropDownList ID="cboSerMese" runat="server" Width="200px">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        
                                        <tr>
                                            <td class="TDcol form_text">
                                                <b>Fornitore :</b><font color="red"><b>*</b></font><asp:RangeValidator ID="RangeValidator3"
                                                    runat="server" ErrorMessage="" ControlToValidate="cboSerFornitore" MinimumValue="1"
                                                    MaximumValue="99999999"></asp:RangeValidator>
                                            </td>
                                            <td class="TDcol form_text" colspan="5" onkeydown="CatturaInvioFORNITORE();">
                                                <asp:TextBox ID="txtSerFornitoreSearch" runat="server"></asp:TextBox>
                                                <asp:LinkButton ID="lnkCERCAFORNITORE" runat="server" Font-Bold="true" CausesValidation="false"
                                                    Visible="true" Style="display: none;" OnClick="lnkCERCAFORNITORE_Click">Cerca</asp:LinkButton>
                                                &nbsp;
                                                <asp:DropDownList Width="400px" ID="cboSerFornitore" runat="server" AutoPostBack="true"
                                                    OnSelectedIndexChanged="cboSerFornitore_SelectedIndexChanged">
                                                </asp:DropDownList>
                                            </td>
                                            <td rowspan="3" colspan="2" style="padding: 10px; border: 1px solid #CCCCCC;" class="form_textFlat">
                                                <b>Informazioni :
                                                    <br />
                                                    <b>% Comp. Red:</b>
                                                    <asp:Label ID="lblSerPercCompenso" runat="server"></asp:Label>&nbsp;%
                                                    <asp:HiddenField ID="HDlblSerPercCompenso" runat="server" />
                                                    <br />
                                                    <b>% Dir. Agenzia :</b>
                                                    <asp:Label ID="lblSerPercDiritti" runat="server"></asp:Label>&nbsp;%
                                                    <asp:HiddenField ID="HDlblSerPercDiritti" runat="server" />
                                                    <br />
                                                    <b>% IVA :</b>
                                                    <asp:Label ID="lblSerPercIVA" runat="server"></asp:Label>&nbsp;%
                                                    <br />
                                                    <b>% Riv. Prev. :</b>
                                                    <asp:Label ID="lblSerPercRicPrev" runat="server"></asp:Label>&nbsp;%
                                                    <br />
                                                    <b>% Rit. Acc. :</b>
                                                    <asp:Label ID="lblSerPercRitAcc" runat="server"></asp:Label>&nbsp;%
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text">
                                                <b>Tipo Servizio :</b><font color="red"><b>*</b></font><asp:RangeValidator ID="RangeValidator1"
                                                    runat="server" ErrorMessage="" ControlToValidate="cboSerTipoServizio" MinimumValue="1"
                                                    MaximumValue="99999999"></asp:RangeValidator>
                                            </td>
                                            <td class="TDcol form_text" colspan="5">
                                                <asp:DropDownList ID="cboSerTipoServizio" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text">
                                                <b>Inizio :</b><font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator1"
                                                    runat="server" ErrorMessage="" ControlToValidate="txtSerInizio"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerInizio" runat="server"></asp:TextBox>
                                                <asp:Image ID="imgSerInizio" runat="server" ImageUrl="../customLayout//images/calendar/calendar.gif" />
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Fine :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator2"
                                                    runat="server" ErrorMessage="" ControlToValidate="txtSerFine"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerFine" runat="server"></asp:TextBox>
                                                <asp:Image ID="imgSerFine" runat="server" ImageUrl="../customLayout//images/calendar/calendar.gif" />
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Giorni :</b><font color="red"><b>*</b></font>
                                                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ErrorMessage=""
                                                    ControlToValidate="txtSerNumGiorni"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerNumGiorni" runat="server" onblur="BlurImporto();"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text">
                                                <b>Tar. Giorn. :</b><font color="red"><b>*</b></font><asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator5" runat="server" ErrorMessage="" ControlToValidate="txtSerImporto"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerImporto" runat="server" onblur="BlurImporto();"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Imp. Over. :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator12" runat="server" ErrorMessage="" ControlToValidate="txtSerImpOver"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerImpOver" runat="server" onblur="BlurImporto();"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Imp. GA :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator14"
                                                    runat="server" ErrorMessage="" ControlToValidate="txtSerImportoGA"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerImportoGA" runat="server" onblur="BlurImporto();"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Imp. Over. GA :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator4" runat="server" ErrorMessage="" ControlToValidate="txtSerOverGA"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerOverGA" runat="server" onblur="BlurImporto();"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text">
                                                <b>Imp. Lordo :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator6" runat="server" ErrorMessage="" ControlToValidate="txtSerImportoLordo"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerImportoLordo" runat="server" ReadOnly="false" Enabled="true"
                                                    BackColor="#DDDDDD"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Diritti Agenzia :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator8" runat="server" ErrorMessage="" ControlToValidate="txtSerDiritti"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerDiritti" runat="server" ReadOnly="false" Enabled="true" BackColor="#DDDDDD"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Imponibile :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator7" runat="server" ErrorMessage="" ControlToValidate="txtSerImponibile"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerImponibile" runat="server" ReadOnly="false" Enabled="true"
                                                    BackColor="#DDDDDD"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Comp. Red :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator15"
                                                    runat="server" ErrorMessage="" ControlToValidate="txtCompAggiunto"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtCompAggiunto" runat="server" ReadOnly="false" Enabled="true"
                                                    BackColor="#DDDDDD"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="2">
                                                &nbsp;
                                            </td>
                                            <td class="TDcol form_text" style="display: none;">
                                                <b>Comp. Red. :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator11" runat="server" ErrorMessage="" ControlToValidate="txtSerImportoRed"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text" style="display: none;">
                                                <asp:TextBox ID="txtSerImportoRed" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Tratt. :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator10"
                                                    runat="server" ErrorMessage="" ControlToValidate="txtSerTrattenute"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerTrattenute" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Anticipi :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator ID="RequiredFieldValidator9"
                                                    runat="server" ErrorMessage="" ControlToValidate="txtSerAnticipi"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerAnticipi" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Anticipi GA :</b> <font color="red"><b>*</b></font><asp:RequiredFieldValidator
                                                    ID="RequiredFieldValidator13" runat="server" ErrorMessage="" ControlToValidate="txtSerAnticipiGA"></asp:RequiredFieldValidator>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerAnticipiGA" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text">
                                                <b>Spese 1 :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa1" runat="server" onblur="BlurImporto();"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Descr. Spesa 1 :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa1Descr" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Extensions :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa1GA" onblur="BlurImporto();" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Descr. Extensions :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa1GADescr" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text">
                                                <b>Spese 2 :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa2" runat="server" onblur="BlurImporto();"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Descr. Spesa 2 :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa2Descr" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Spese Viaggio :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa2GA" onblur="BlurImporto();" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Descr. Viaggio :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa2GADescr" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text">
                                                <b>Spese 3 :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa3" runat="server" onblur="BlurImporto();"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Descr. Spesa 3 :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa3Descr" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Spese Varie :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa3GA" onblur="BlurImporto();" runat="server"></asp:TextBox>
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>Descr. Varie :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtSerSpesa3GADescr" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="TDcol form_text" style="display: none;">
                                                <b>Esente IVA :</b>
                                            </td>
                                            <td class="TDcol form_text" style="display: none;">
                                                <asp:CheckBox ID="chkEsenzioneIVA" runat="server" />
                                            </td>
                                            <td colspan="6">
                                                &nbsp;
                                            </td>
                                            <td class="TDcol form_text">
                                                <b>TOTALE SERVIZIO :</b>
                                            </td>
                                            <td class="TDcol form_text">
                                                <asp:TextBox ID="txtTotaleFinale" runat="server" ReadOnly="false" Enabled="true"
                                                    BackColor="#DDDDDD"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="form_text" colspan="8" align="right">
                                                <asp:LinkButton ID="lnkSerAnnulla" OnClick="lnkSerAnnulla_Click" Font-Bold="true"
                                                    runat="server" CausesValidation="False"><img id="Img1" src="../customLayout/images/icoClose.gif" alt="" border="0" />&nbsp;Annulla Modifica Servizio&nbsp;</asp:LinkButton><asp:LinkButton
                                                        ID="lnkSerAggiorna" OnClick="lnkSerAggiorna_Click" Font-Bold="true" runat="server"><img id="Img1" src="../customLayout/images/icoConfirm.gif" alt="" border="0" />&nbsp;Applica Modifica Servizio&nbsp;</asp:LinkButton>&nbsp;&nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4">
                                    <table width="100%" cellpadding="10" cellspacing="0">
                                        <tr>
                                            <td class="form_text">
                                                <b>TOT.IMPONIBILE : </b>
                                            </td>
                                            <td class="form_text">
                                                <asp:Label ID="lblSer_TOTIMPO" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="form_text">
                                                <b>TOT.IMPORTO LORDO : </b>
                                            </td>
                                            <td class="form_text">
                                                <asp:Label ID="lblSer_TOTLORDO" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="form_text">
                                                <b>TOT.SPESE : </b>
                                            </td>
                                            <td class="form_text">
                                                <asp:Label ID="lblSer_TOTSPESE" runat="server" Text=""></asp:Label>
                                            </td>
                                            <td class="form_text">
                                                <b>TOT.DIRITTI : </b>
                                            </td>
                                            <td class="form_text">
                                                <asp:Label ID="lblSer_TOTDIRITTI" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="4" class="form_text">
                                    <asp:DataGrid ID="grdServizi" runat="server" Width="100%" AutoGenerateColumns="False"
                                        CssClass="webgridGeneral" AllowPaging="True" CellPadding="5" PageSize="50" GridLines="Horizontal"
                                        OnItemCommand="grdServizi_ItemCommand">
                                        <SelectedItemStyle CssClass="webgridSelectedRowStyleDefault" />
                                        <AlternatingItemStyle CssClass="webgridRowAlternateStyleDefault" />
                                        <ItemStyle CssClass="webgridRowStyleDefault" />
                                        <HeaderStyle CssClass="webgridHeaderStyleDefault" />
                                        <Columns>
                                            <asp:TemplateColumn HeaderText="&nbsp;&nbsp;Servizi">
                                                <ItemTemplate>
                                                    <table cellpadding="0" cellspacing="2" border="0" style="width: 100%;" valign="top">
                                                        <tr>
                                                            <td rowspan="10" valign="top">
                                                                <img src="../customLayout/images/cruscotto/<%# DataBinder.Eval(Container, "DataItem.COLORE")%>.gif"
                                                                    alt="<%# DataBinder.Eval(Container, "DataItem.COLORE")%>" />&nbsp;&nbsp;&nbsp;
                                                            </td>
                                                        </tr>
                                                        <tr valign="top">
                                                            <td style="border: 0;" colspan="2">
                                                                <h5>
                                                                    [<%# DataBinder.Eval(Container, "DataItem.SRAnnoFatt")%>
                                                                    <%# DataBinder.Eval(Container, "DataItem.SRMeseFatt")%>] - [<%# DataBinder.Eval(Container, "DataItem.srcodice")%>]
                                                                    Fornitore :
                                                                    <%# DataBinder.Eval(Container, "DataItem.GREEN_Fornitore")%>
                                                                </h5>
                                                            </td>
                                                        </tr>
                                                        <tr>
                                                            <td valign="top" width="100%">
                                                                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; height: 100%;">
                                                                    <tr>
                                                                        <td class="TDcol">
                                                                            <b>Servizio :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# DataBinder.Eval(Container, "DataItem.TipoServizio")%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Inizio :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.SRdatainizio")).ToShortDateString()%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Fine :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# Convert.ToDateTime(DataBinder.Eval(Container, "DataItem.SRdatafine")).ToShortDateString()%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Num Giorni :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# DataBinder.Eval(Container, "DataItem.srnumgiorni")%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TDcol">
                                                                            <b>Tar. Gionaliera :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.srimporto"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Importo Over. :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRimportoOvertime"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Importo GA :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.srimportoGA"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Importo Over. GA :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRimportoOvertimeGA"))%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TDcol">
                                                                            <b>Importo Lordo :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRimportoLordo"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Diritti Agenzia :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.srdiritti"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Imponibile :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.srimponibile"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Comp. Redaz. :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.srcompensoaggiunto"))%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td colspan="2">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td class="TDcol" style="display: none;">
                                                                            <b>Comp. Redaz. :</b>
                                                                        </td>
                                                                        <td class="TDcol" style="display: none;">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRimportoRedazionale"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Trattenute :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.srtrattenute"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Anticipi :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.sranticipi"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Anticipi GA :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.sranticipige"))%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TDcol">
                                                                            <b>Spese 1 :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRspese"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Descr. Spesa 1 :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# DataBinder.Eval(Container, "DataItem.SRdescSpese")%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Extensions :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRspeseGA"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Descr. Extensions :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# DataBinder.Eval(Container, "DataItem.SRdescSpeseGA")%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TDcol">
                                                                            <b>Spese 2 :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRspese1"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Descr. Spesa 2 :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# DataBinder.Eval(Container, "DataItem.SRdescSpese1")%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Spese Viaggio :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRspeseGA1"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Descr. Viaggio :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# DataBinder.Eval(Container, "DataItem.SRdescSpeseGA1")%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TDcol">
                                                                            <b>Spese 3 :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRspese2"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Descr. Spesa 3 :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# DataBinder.Eval(Container, "DataItem.SRdescSpese2")%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Spese Varie :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SRspeseGA2"))%>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>Descr. Varie :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# DataBinder.Eval(Container, "DataItem.SRdescSpeseGA2")%>
                                                                        </td>
                                                                    </tr>
                                                                    <tr>
                                                                        <td class="TDcol" style="display: none;">
                                                                            <b>Esente IVA :</b>
                                                                        </td>
                                                                        <td class="TDcol" style="display: none;">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.SResenzioneIvaArt15"))%>
                                                                        </td>
                                                                        <td colspan="6">
                                                                            &nbsp;
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <b>TOTALE SERVIZIO :</b>
                                                                        </td>
                                                                        <td class="TDcol">
                                                                            <%# String.Format("{0:N2}", DataBinder.Eval(Container, "DataItem.TOTFINALE"))%>
                                                                        </td>
                                                            </td>
                                                        </tr>
                                                    </table>
                                                    </td> </tr> </table>
                                                </ItemTemplate>
                                            </asp:TemplateColumn>
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
                        </table>
                    </td>
                </tr>
                <tr>
                    <td align="center" style="padding-top: 10px;">
                        <asp:PlaceHolder ID="phPagine" runat="server"></asp:PlaceHolder>
                    </td>
                </tr>
                <tr height="30px">
                    <td>
                        &nbsp;
                    </td>
                </tr>
            </table>
        </td>
    </tr>
</table>

<script type="text/javascript">
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

function BlurImporto() {
    try {
        var txtSerImporto = document.getElementById('ctl00_ContentMain_ctl00_txtSerImporto');
        var txtSerNumGiorni = document.getElementById('ctl00_ContentMain_ctl00_txtSerNumGiorni');

        var txtSerImpOver = document.getElementById('ctl00_ContentMain_ctl00_txtSerImpOver');

        var REAL_txtSerImpOver;
        if (txtSerImpOver != null) REAL_txtSerImpOver = txtSerImpOver.value;

        var txtSerImportoGA = document.getElementById('ctl00_ContentMain_ctl00_txtSerImportoGA');
        var txtSerOverGA = document.getElementById('ctl00_ContentMain_ctl00_txtSerOverGA');
        var txtCompAggiunto = document.getElementById('ctl00_ContentMain_ctl00_txtCompAggiunto');
        var txtTotaleFinale = document.getElementById('ctl00_ContentMain_ctl00_txtTotaleFinale');


        var HDlblSerPercDiritti = document.getElementById('ctl00_ContentMain_ctl00_HDlblSerPercDiritti');
        var HDlblSerPercCompenso = document.getElementById('ctl00_ContentMain_ctl00_HDlblSerPercCompenso');
        var lblSerPercCompenso = document.getElementById('ctl00_ContentMain_ctl00_lblSerPercCompenso');

        var txtSerImportoLordo = document.getElementById('ctl00_ContentMain_ctl00_txtSerImportoLordo');
        var txtSerDiritti = document.getElementById('ctl00_ContentMain_ctl00_txtSerDiritti');
        var txtSerImponibile = document.getElementById('ctl00_ContentMain_ctl00_txtSerImponibile');
        var txtSerImportoRed = document.getElementById('ctl00_ContentMain_ctl00_txtSerImportoRed');

        var txtSerSpesa1 = document.getElementById('ctl00_ContentMain_ctl00_txtSerSpesa1');
        var txtSerSpesa2 = document.getElementById('ctl00_ContentMain_ctl00_txtSerSpesa2');
        var txtSerSpesa3 = document.getElementById('ctl00_ContentMain_ctl00_txtSerSpesa3');
        var txtSerSpesa1GA = document.getElementById('ctl00_ContentMain_ctl00_txtSerSpesa1GA');
        var txtSerSpesa2GA = document.getElementById('ctl00_ContentMain_ctl00_txtSerSpesa2GA');
        var txtSerSpesa3GA = document.getElementById('ctl00_ContentMain_ctl00_txtSerSpesa3GA');

        //alert(txtSerImporto.value);
        //REAL_txtSerImpOver = REAL_txtSerImpOver.replace(",", ".");


        if (txtSerImporto != null) txtSerImporto.value = txtSerImporto.value.replace(',', '.');
        if (txtSerImpOver != null) txtSerImpOver.value = txtSerImpOver.value.replace(',', '.');
        if (txtSerDiritti != null) txtSerDiritti.value = txtSerDiritti.value.replace(',', '.');
        if (txtSerImponibile != null) txtSerImponibile.value = txtSerImponibile.value.replace(',', '.');
        if (txtSerImportoRed != null) txtSerImportoRed.value = txtSerImportoRed.value.replace(',', '.');
        if (txtCompAggiunto != null) txtCompAggiunto.value = txtCompAggiunto.value.replace(',', '.');
        if (txtSerImportoRed != null) txtSerImportoRed.value = txtSerImportoRed.value.replace(',', '.');

        if (txtSerImportoLordo != null) txtSerImportoLordo.value = (parseFloat(txtSerImporto.value) * parseFloat(txtSerNumGiorni.value)) + parseFloat(txtSerImpOver.value);
        if (txtSerDiritti != null) txtSerDiritti.value = (parseFloat(txtSerImportoLordo.value) * parseFloat(HDlblSerPercDiritti.value) / 100);
        if (txtSerImponibile != null) txtSerImponibile.value = parseFloat(txtSerImportoLordo.value) - parseFloat(txtSerDiritti.value);


        if (txtCompAggiunto != null) txtCompAggiunto.value = (parseFloat(txtSerImponibile.value) * parseFloat(HDlblSerPercCompenso.value)) / 100;
        if (txtSerImportoRed != null) txtSerImportoRed.value = txtCompAggiunto.value;
        //txtCompAggiunto.value='1003';

        //RICALCOLO IMPONIBILE AGGIUNGENDO IMPORTO AGGIUNTIVO

        if (txtTotaleFinale != null) {
            txtTotaleFinale.value = parseFloat(txtSerImponibile.value) + parseFloat(txtCompAggiunto.value) + parseFloat(txtSerSpesa1.value) + parseFloat(txtSerSpesa2.value) + parseFloat(txtSerSpesa3.value);
        }
    }
    catch (err) {
        var lblError = document.getElementById('ctl00_ContentMain_ctl00_lblError');
        lblError.value = err;
    }

}




</script>

<asp:ValidationSummary ID="vldSummary" runat="server" ShowMessageBox="True" ShowSummary="True" />
