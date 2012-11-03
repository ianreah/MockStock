$(function () {
	var stockHub = $.connection.stockHub;
	
	// Declare a function on the hub so the server can invoke it
	stockHub.updatePrice = function (stockPrice) {
	    $('#stocks').append('<li>' + stockPrice + '</li>');
	};

	$("#subscribe").click(function () {
		// Call the method on the server
	    stockHub.subscribe($('#symbol').val().toUpperCase());
	});

	// Start the connection
	$.connection.hub.start();
});