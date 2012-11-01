<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MockStock.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
	<input type="text" id="msg" />
	<input type="button" id="broadcast" value="broadcast" />
	
	<ul id="messages"></ul>

    <script src="Scripts/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.signalR-0.5.3.js" type="text/javascript"></script>
    <script src="/signalr/hubs" type="text/javascript"></script>
    <script src="js/main.js" type="text/javascript"></script>
</body>
</html>
