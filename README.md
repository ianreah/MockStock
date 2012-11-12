MockStock
=========

A mock stock price subscription service using SignalR and Rx

## Requirements ##

A (semi-)realistic stock price subscription service would have these key requirements:

* It won't produce price updates for a stock that no clients are watching
* The price updates can be randomly generated for the simulation, but...
* Multiple clients watching the same stock should see the same prices.

Can I use SignalR and Rx to do this effectively?

[See it in action](http://mockstock.apphb.com/)