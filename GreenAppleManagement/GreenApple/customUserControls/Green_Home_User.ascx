<%@ Control Language="C#" CodeFile="Green_Home_User.ascx.cs" Inherits="Green.Apple.Management.Green_Home_User" %>

<div style="float:right">
    Colore:<br />
    <asp:DropDownList ID="cboColore" runat="server" AutoPostBack="true" 
        onselectedindexchanged="cboColore_SelectedIndexChanged">
    </asp:DropDownList>&nbsp;&nbsp;&nbsp;&nbsp;
</div>

<asp:Label ID="lblColors" runat="server" Text=""></asp:Label>
<font style="font-size: 14px; font-weight: bold;">GREEN APPLE MANAGEMENT</font>
<br />
<br />
<asp:Label ID="lblData" runat="server" Text="Label"></asp:Label> - <b>
<asp:Label ID="lblUtente" runat="server" Text=""></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp; </b>

<br />
<asp:LinkButton ID="lnkLogOut" runat="server" onclick="lnkLogOut_Click">LogOut</asp:LinkButton>