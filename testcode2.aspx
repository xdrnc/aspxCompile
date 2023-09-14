<%@ Page Language="C#" AutoEventWireup="true" Debug="true" CodeFile="testcode2.aspx.cs" inherits="TestCode"%>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
                <asp:Button ID="Button2" OnClick="preset1_Click" runat="server" Text="Cache" />
                <asp:Button ID="Button3" OnClick="preset2_Click" runat="server" Text="Index" />
                <asp:Button ID="Button4" OnClick="preset3_Click" runat="server" Text="GetItem" />
                <asp:Button ID="Button5" OnClick="preset4_Click" runat="server" Text="EventProcessing" />
        <br />
        <textarea id="TextArea1" name="TextArea1" runat="server" cols="150" rows="20"></textarea>
        <br />
        <asp:Button ID="Button1" OnClick="execute_Click" runat="server" Text="Execute" />
        <br />
        <textarea id="TextArea2" runat="server" cols="150" rows="20" ></textarea>
    </div>
    </form>
</body>
</html>
