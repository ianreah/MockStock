<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="MockStock.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta name="description" content="MockStock : A mock stock price subscription service using SignalR and Rx" />

    <title>MockStock</title>

    <link rel="stylesheet" type="text/css" media="screen" href="styles/stylesheet.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="styles/theme.css" />
</head>
<body>
    <div id="header_wrap" class="outer">
	    <header class="inner">
            <a id="forkme_banner" href="https://github.com/ianreah/MockStock">View on GitHub</a>

            <h1 id="project_title">MockStock</h1>
            <h2 id="project_tagline">A mock stock price subscription service using SignalR and Rx</h2>
	    </header>
    </div>

    <div id="main_content_wrap" class="outer">
      <section id="main_content" class="inner">
        <ul id="stocks"></ul>

        <form>
            <input type="text" id="symbol" maxlength="5" placeholder="Enter a stock symbol"/>
        </form>
        <p><strong>Important:</strong> This is completely fake, randomly generated data purely for the purposes
            of investigating the capabilities of the libraries. If you enter a non-existing stock symbol you'll
            still get price data. If you enter a real stock symbol the values you see will have no resemblance to 
            the actual value of the stock. Don't make any real investments based on this fake data!</p>

        <p>To find out more check out the <a href ="https://github.com/ianreah/MockStock">code on GitHub</a>, or read the blog post (coming soon!)</p>
      </section>
    </div>
    
    <div id="footer_wrap" class="outer">
      <footer class="inner">
        <p class="copyright">MockStock maintained by <a href="http://ianreah.com">ianreah</a><br />
        Hosted with <a href="https://appharbor.com/">AppHarbor</a></p>
      </footer>
    </div>

    <script type="text/x-jquery-tmpl" id="stockTemplate">
        <li>
            <span class="symbol">${symbol}</span>
            <div class="dynamicSection">{{tmpl($data) "#stockDynamicSection"}}</div>
            <div class="unsubscribe"></div>
        </li>
    </script>
	
    <script type="text/x-jquery-tmpl" id="stockDynamicSection">
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
    </script>

    <script src="Scripts/jquery-1.7.2.min.js" type="text/javascript"></script>
    <script src="Scripts/jquery.signalR-0.5.3.js" type="text/javascript"></script>
    <script src="Scripts/jQuery.tmpl.min.js" type="text/javascript"></script>
    <script src="/signalr/hubs" type="text/javascript"></script>
    <script src="Scripts/spine/spine.js" type="text/javascript"></script>
    <script src="js/main.js" type="text/javascript"></script>
	
	<script type="text/javascript">
		var _gaq = _gaq || [];
		_gaq.push(['_setAccount', 'UA-36275300-1']);
		_gaq.push(['_trackPageview']);

		(function () {
			var ga = document.createElement('script'); ga.type = 'text/javascript'; ga.async = true;
			ga.src = ('https:' == document.location.protocol ? 'https://ssl' : 'http://www') + '.google-analytics.com/ga.js';
			var s = document.getElementsByTagName('script')[0]; s.parentNode.insertBefore(ga, s);
		})();
	</script>
</body>
</html>
