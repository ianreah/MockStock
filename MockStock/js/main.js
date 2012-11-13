$(function () {
	// The model...	
	var StockModel = Spine.Model.sub();
	StockModel.configure("StockModel", "symbol", "price", "change");

	// Model static functions...
	StockModel.extend({
		findBySymbol: function (symbol) {
			return this.findByAttribute("symbol", symbol);
		}
	});
	
	// Model instance functions...
	StockModel.include({
		// Called when a model is saved or updated...
		validate: function() {
			if(!this.symbol) {
				return "Can't subscribe to an empty stock symbol";
			}
		}
	});
	
	// Controller for a single item...
	// (...stockModel attribute set to an instance of StockModel at construction,
	// e.g., "new StockController({ stockModel: stockModel })")
	var StockController = Spine.Controller.sub({
	    events: {
	        "click .unsubscribe": "unsubscribe"
	    },

		init: function() {
			this.stockModel.bind("update", this.proxy(this.render));
		},

		render: function () {
			// Replace the controller's el with the template output...
		    this.replace($("#stockTemplate").tmpl(this.stockModel));
			return this;
		},
		
		unsubscribe: function () {
		    this.stockModel.destroy();

			// Fires the release event, which removes el from the page
			// and unbinds controller events.
		    this.release();
		}
	});
	
	// Controller for the collection of items...
	// ...manages the creation of models and their controllers,
	// and the subscription/unsubscription with the signalR hub.
	var StocksController = Spine.Controller.sub({
		elements: {
		    "#stocks": "stocks", // the stocks container
            "#symbol": "symbol"  // the symbol text box
		},

		events: {
		    "submit form": "subscribe",
		},

		init: function () {
			// Declare a function on the hub so the server can invoke it
		    this.stockHub.updatePrice = function (stockPrice) {
		        var stock = StockModel.findBySymbol(stockPrice.Symbol);
		        stock.price = stockPrice.Price;
		        stock.change = stockPrice.Change;
		        stock.save();
		    };

		    StockModel.bind("create", this.proxy(this.add));
		},

		add: function (stockModel) {
		    var controller = new StockController({ stockModel: stockModel });
			this.stocks.append(controller.render().el);
		},

		subscribe: function (e) {
		    e.preventDefault();

		    var newSymbol = this.symbol.val().toUpperCase().trim();

		    if (StockModel.findBySymbol(newSymbol) == null) {
			    // Create the model
			    var newStock = new StockModel({ symbol: newSymbol });
			    if (newStock.save()) {
				    newStock.bind("destroy", this.proxy(this.unsubscribe));

				    // Call the method on the server...
				    this.stockHub.subscribe(newSymbol);
			    }
		    }
			
			this.symbol.val("");
		},

		unsubscribe: function (stockModel) {
			// Tell the server to unsubscribe us...
		    this.stockHub.unsubscribe(stockModel.symbol);
		}
	});

	// Kick off the main controller...
	new StocksController({
		el: $("body"),
		stockHub: $.connection.stockHub
	});
	
	// ...and start the signalR connection.
	$.connection.hub.start();
});