using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MockStock.Core;
using NSubstitute;

namespace MockStock.Tests
{
	[TestClass]
	public class GroupSubscriptionManagerTests
	{
		private IPriceFeedGenerator priceFeedGenerator;
		private ISubscriptionStore subscriptionStore;

		private GroupSubscriptionManager subscriptionManagerUnderTest;

		[TestInitialize]
		public void TestInitialize()
		{
			priceFeedGenerator = Substitute.For<IPriceFeedGenerator>();
			subscriptionStore = Substitute.For<ISubscriptionStore>();

			subscriptionManagerUnderTest = new GroupSubscriptionManager(priceFeedGenerator, subscriptionStore);
		}

		[TestMethod]
		public void Constructor_GeneratesNoFeeds()
		{
			// Assert
			priceFeedGenerator.DidNotReceiveWithAnyArgs().Generate(null);
		}

		[TestMethod]
		public void Constructor_AddsNoSubscriptions()
		{
			// Assert
			subscriptionStore.DidNotReceiveWithAnyArgs().AddSubscription(null, null);
		}

		[TestMethod]
		public void Constructor_AddNoSubscribers()
		{
			// Assert
			subscriptionStore.DidNotReceiveWithAnyArgs().AddSubscriber(null, null);
		}

		[TestMethod]
		public void Subscribe_WhenSubscriptionDoesntExist_GeneratesTheFeed()
		{
			// Act
			subscriptionManagerUnderTest.Subscribe("XYZ", "client_1");

			// Assert
			priceFeedGenerator.Received(1).Generate("XYZ");
		}

		[TestMethod]
		public void Subscribe_WhenSubscriptionDoesntExist_AddsTheSubscription()
		{
			// Arrange
			var priceFeed = SetupFeedForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Subscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.Received(1).AddSubscription("XYZ", priceFeed);
		}

		[TestMethod]
		public void Subscribe_WhenSubscriptionDoesntExist_AddsTheSubscriber()
		{
			// Act
			subscriptionManagerUnderTest.Subscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.Received(1).AddSubscriber("XYZ", "client_1");
		}

		[TestMethod]
		public void Subscribe_WhenSubscriptionExists_DoesntGenerateTheFeed()
		{
			// Arrange
			SetupSubscriptionForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Subscribe("XYZ", "client_1");

			// Assert
			priceFeedGenerator.DidNotReceive().Generate("XYZ");
		}

		[TestMethod]
		public void Subscribe_WhenSubscriptionExists_DoesntAddsTheSubscription()
		{
			// Arrange
			var priceFeed = SetupFeedForSymbol("XYZ");
			SetupSubscriptionForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Subscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.DidNotReceive().AddSubscription("XYZ", priceFeed);
		}

		[TestMethod]
		public void Subscribe_WhenSubscriptionExists_AddsTheSubscriber()
		{
			// Arrange
			SetupSubscriptionForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Subscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.Received(1).AddSubscriber("XYZ", "client_1");
		}


		[TestMethod]
		public void Unsubscribe_LastSubscriber_DisposesTheFeed()
		{
			// Arrange
			var priceFeed = SetupFeedForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Unsubscribe("XYZ", "client_1");

			// Assert
			priceFeed.Received(1).Dispose();
		}

		[TestMethod]
		public void Unsubscribe_LastSubscriber_RemovesTheSubscription()
		{
			// Act
			subscriptionManagerUnderTest.Unsubscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.Received(1).RemoveSubscription("XYZ");
		}

		[TestMethod]
		public void Unsubscribe_LastSubscriber_RemovesTheSubscriber()
		{
			// Act
			subscriptionManagerUnderTest.Unsubscribe("XYZ", "client_1");
			
			// Assert
			subscriptionStore.Received(1).RemoveSubscriber("XYZ", "client_1");
		}

		[TestMethod]
		public void Unsubscribe_NotLastSubscriber_DoesntDisposeTheFeed()
		{
			// Arrange
			var priceFeed = SetupFeedForSymbol("XYZ");
			SetupSubscriptionForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Unsubscribe("XYZ", "client_1");

			// Assert
			priceFeed.DidNotReceive().Dispose();
		}

		[TestMethod]
		public void Unsubscribe_NotLastSubscriber_DoesntRemoveTheSubscription()
		{
			// Arrange
			SetupSubscriptionForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Unsubscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.DidNotReceive().RemoveSubscription("XYZ");
		}

		[TestMethod]
		public void Unsubscribe_NotLastSubscriber_RemovesTheSubscriber()
		{
			// Arrange
			SetupSubscriptionForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Unsubscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.Received(1).RemoveSubscriber("XYZ", "client_1");
		}

		private IDisposable SetupFeedForSymbol(string symbol)
		{
			var disposable = Substitute.For<IDisposable>();
			priceFeedGenerator.Generate(symbol).Returns(disposable);
			subscriptionStore.RemoveSubscription(symbol).Returns(disposable);
			return disposable;
		}

		private void SetupSubscriptionForSymbol(string symbol)
		{
			subscriptionStore.Exists(symbol).Returns(true);
			subscriptionStore.AnySubscribers(symbol).Returns(true);
		}
	}
}
