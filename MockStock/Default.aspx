<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MockStock.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <link rel="stylesheet" type="text/css" media="screen" href="styles/stylesheet.css" />
</head>
<body>
    <ul id="stocks"></ul>

    <form>
        <input type="text" id="symbol" maxlength="5" placeholder="symbol"/>
    </form>
    
    <script type="text/x-jquery-tmpl" id="stockTemplate">
        <li>
            <span class="symbol">${symbol}</span>
            <span>{{if price}}${price.toFixed(2)}{{else}}-{{/if}}</span>
            {{if change}}
                {{if change < 0}}
                    <span class="changeDown">${change.toFixed(2)}</span>
                {{else}}
                    <span class="changeUp">+${change.toFixed(2)}</span>
                {{/if}}
            {{else}}
                <span>-</span>
            {{/if}}
            <input type="button" class="unsubscribe" value="x" />
        </li>
    </script>

    <script src="Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.signalR-0.5.3.js" type="text/javascript"></script>
    <script src="Scripts/jQuery.tmpl.min.js" type="text/javascript"></script>
    <script src="/signalr/hubs" type="text/javascript"></script>
    <script src="Scripts/spine/spine.js" type="text/javascript"></script>
    <script src="js/main.js" type="text/javascript"></script>
</body>
</html>
