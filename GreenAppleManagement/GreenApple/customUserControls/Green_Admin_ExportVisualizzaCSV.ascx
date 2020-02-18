<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Green_Admin_ExportVisualizzaCSV.ascx.cs" Inherits="Green.Apple.Management.Green_Admin_ExportVisualizzaCSV" %>
<table cellpadding="0" cellspacing="0" width="100%" height="100%">
    <tr>
        <td class="form_Title" colspan="3">
            &nbsp;&nbsp;Amministrazione -
            <%=NOMEOGGETTO%>
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
                        
                    </td>
                </tr>
                <tr>
                    <td class="form_textarea">
                        
                    </td>
                </tr>
                <tr>
                    <td class="form_text" align="right">
                        
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
                        FILE CSV ESPORTATI
                    </td>
                </tr>
                <tr>
                    <td colspan="2" valign="top">
                        
                        <asp:Literal ID="litTABLE" runat="server"></asp:Literal>

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
            
        </td>
    </tr>
</table>

<asp:ValidationSummary ID="vldSummary" runat="server" ShowMessageBox="True" ShowSummary="True" />


