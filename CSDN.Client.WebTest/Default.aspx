<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="CSDN.Client.WebTest.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title>open.csdn.net</title>
</head>
<body>
    <form id="form1" runat="server">
    <p>
        <asp:Button ID="Button1" runat="server" OnClick="Button1_Click" Text="登录" />
    </p>
        <p>Access Token: <asp:Literal ID="Literal1" runat="server"></asp:Literal></p>
        <p><asp:Button ID="Button2" runat="server" OnClick="Button2_Click" Text="获取用户资料" /></p>
        <p>资料：<asp:Literal ID="Literal2" runat="server"></asp:Literal></p>
        <p><asp:Button ID="Button3" runat="server" OnClick="Button3_Click" Text="获取博客专栏" /></p>
        <p><asp:Literal ID="Literal3" runat="server"></asp:Literal></p>
    </form>
</body>
</html>
