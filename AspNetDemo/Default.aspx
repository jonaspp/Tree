<%@ Page Language="C#" AutoEventWireup="true"  CodeFile="Default.aspx.cs" Inherits="_Default" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Tree ASP.net Demo</title>
</head>
<body style="margin:0 0 0 0; font-size: 14px; font-family: Helvetica;">
    <div style="background-color: #424242; border-bottom: solid 3px #ff8c00; padding: 4px;">
        <h1 style="color: #FFFFFF; margin: 0 0 0 0; padding: 0 0 0 120px;">[Tree] In memory Chat Demo</h1>
    </div>
    <form id="form1" runat="server">
    <div style="display:table; background-color: #C1C1C1; padding: 0 0 0 120px; width:100%;">
        <div style="padding: 6px 6px 6px 0; margin: 6px 6px 6px 0; float: left;">
            Who: <br /> 
            <asp:TextBox ID="TextBoxWho" runat="server"></asp:TextBox>
        </div>
        <div style="padding: 6px 6px 6px 0; margin: 6px 6px 6px 0; float: left;">
            Message: <br />
            <asp:TextBox ID="TextBoxMessage" runat="server"></asp:TextBox>    
            <asp:Button ID="ButtonChat" runat="server" Text="Chat" onclick="ButtonChat_Click" />    
        </div>        
    </div>
    <div style="line-height: 20px; clear: both; padding: 0 0 0 120px; margin-top: 20px;">
            <asp:Label ID="LabelMessages" runat="server" Text=""></asp:Label>    
    </div>
    </form>
</body>
</html>
