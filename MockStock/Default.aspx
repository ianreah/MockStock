<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MockStock.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" media="screen" href="styles/stylesheet.css" />
</head>
<body>
	<input type="text" id="symbol" maxlength="5"/>
	<input type="button" id="subscribe" value="subscribe" />
	<input type="button" id="unsubscribe" value="unsubscribe" />
	
	<ul id="stocks"></ul>

    <script src="Scripts/jquery-1.6.4.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.signalR-0.5.3.js" type="text/javascript"></script>
    <script src="/signalr/hubs" type="text/javascript"></script>
    <script src="js/main.js" type="text/javascript"></script>
</body>
</html>
