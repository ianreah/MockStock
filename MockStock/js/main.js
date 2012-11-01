$(function () {
	var stockHub = $.connection.stockHub;
	
	// Declare a function on the hub so the server can invoke it
	stockHub.addMessage = function (message) {
		$('#messages').append('<li>' + message + '</li>');
	};

	$("#broadcast").click(function () {
		// Call the method on the server
		stockHub.send($('#msg').val());
	});

	// Start the connection
	$.connection.hub.start();
});