<%@ Page Language="C#" CodeFile="ReloadSession.aspx.cs" Inherits="Green.Apple.Management.ReloadSession" %>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <%--<link href="/customLayout/styles/RCSCore.css" rel="stylesheet" type="text/css" />--%>
    <%--<link href="/customLayout/styles/RCSMenu.css" rel="stylesheet" type="text/css" />--%>
    <link href="/customLayout/styles/forms.css" rel="stylesheet" type="text/css" />
    <title>Reload Session</title>
</head>
<body style="margin: 0px;" bgcolor="#FFFFFF">
    <form id="form1" runat="server">
            <asp:LinkButton ID="lnkRefresh" runat="server" OnClick="lnkRefresh_Click">Refresh Session</asp:LinkButton>
            <br />
            <asp:Label ID="lblInfo" runat="server"></asp:Label>
        </form>
    </body>
    
    <script>
        window.setTimeout("Ricarica()",60000);
        
        function Ricarica()
        {
           __doPostBack('<%=lnkRefresh.UniqueID %>','');		 
        }
    </script>
</html>
    


