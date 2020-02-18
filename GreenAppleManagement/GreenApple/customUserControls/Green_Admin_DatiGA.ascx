<%@ Control Language="C#" CodeFile="Green_Admin_DatiGA.ascx.cs" Inherits="Green.Apple.Management.Green_Admin_DatiGA" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr>
        <td class="form_Title" colspan="3">
            &nbsp;&nbsp;Amministrazione - Dati Anagrafici
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
                        &nbsp;
                    </td>
                </tr>
            </table>
        </td>
        <td width="5">
        </td>
        <td class="box_Data_Admin">
            <table runat="server" cellpadding="0" cellspacing="0" width="100%" border="0" style="border: 1px solid #999999;">
                <tr>
                    <td colspan="2" class="form_AnagraficaInsert">
                        <a name="SectionEdit"></a>Dati Anagrafici Green Apple
                    </td>
                </tr>
                <tr>
                    <td class="form_Error_Message" colspan="2" align="left">
                        <asp:Label ID="lblError" runat="server" Text="" Width="100%"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Ragione Sociale
                        :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtRAGSOC" runat="server" MaxLength="255" Width="98%"></asp:TextBox><asp:RequiredFieldValidator ID="reqElemento" runat="server" ControlToValidate="txtRAGSOC" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Indirizzo
                        :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtINDIRIZZO" runat="server" MaxLength="255" Width="98%"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtINDIRIZZO" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        CAP
                        :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtCAP" runat="server" MaxLength="255" Width="98%"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="txtCAP" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Località
                        :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtLOCALITA" runat="server" MaxLength="255" Width="98%"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="txtLOCALITA" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Nazione
                        :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtNAZIONE" runat="server" MaxLength="255" Width="98%"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator4" runat="server" ControlToValidate="txtNAZIONE" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        Partita IVA
                        :
                    </td>
                    <td class="form_textarea" width="80%">
                        <asp:TextBox ID="txtPIVA" runat="server" MaxLength="255" Width="98%"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator5" runat="server" ControlToValidate="txtPIVA" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                    <td class="form_text" width="20%">
                        % IVA APPLICATIVA:
                        
                    </td>
                    <td class="form_textarea" width="80%">
                        <br />Aliquota IVA applicata a tutta l'applicazione (verranno aggiornate anche TUTTE le aliquote IVA dei fornitori)
                        <asp:TextBox ID="txtIVAAPPLICATA" runat="server" MaxLength="6" Width="30%"></asp:TextBox><asp:RequiredFieldValidator ID="RequiredFieldValidator6" runat="server" ControlToValidate="txtPIVA" ErrorMessage="*" Font-Bold="True">*</asp:RequiredFieldValidator>
                    </td>
                    
                </tr>
                <tr>
                    <td class="form_text" colspan="2" align="right">
                        <asp:LinkButton ID="lnkAggiorna" Font-Bold="true" runat="server" OnClick="lnkAggiorna_Click"><img id="Img1" src="../customLayout/images/icoConfirm.gif" alt="" border="0" />&nbsp;Applica&nbsp;</asp:LinkButton>&nbsp;&nbsp;
                    </td>
                    
                </tr>
            </table>
        </td>
    </tr>
</table>

<asp:ValidationSummary ID="vldSummary" runat="server" ShowMessageBox="True" ShowSummary="True" />
