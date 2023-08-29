<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="testcode.aspx.cs" inherits="TestCode"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <textarea id="TextArea1" name="TextArea1" runat="server" cols="150" rows="20"></textarea>
        <asp:Button ID="Button1" OnClick="execute_Click" runat="server" Text="Execute" />
        <textarea id="TextArea2" runat="server" cols="150" rows="20" ></textarea>
    </div>
    </form>
</body>
</html>
