using System;
using MockStock.Core;
using NSubstitute;
using NUnit.Framework;

namespace MockStock.Tests
{
	[TestFixture]
	public class GroupSubscriptionManagerTests
	{
		private IPriceFeedGenerator priceFeedGenerator;
		private IPriceFeedSubscriber priceFeedSubscriber;
		private ISubscriptionStore subscriptionStore;

		private GroupSubscriptionManager subscriptionManagerUnderTest;

		[SetUp]
		public void TestInitialize()
		{
			priceFeedGenerator = Substitute.For<IPriceFeedGenerator>();
			priceFeedSubscriber = Substitute.For<IPriceFeedSubscriber>();
			subscriptionStore = Substitute.For<ISubscriptionStore>();

			subscriptionManagerUnderTest = new GroupSubscriptionManager(priceFeedGenerator, priceFeedSubscriber, subscriptionStore);
		}

		[Test]
		public void Constructor_GeneratesNoFeeds()
		{
			// Assert
			priceFeedGenerator.DidNotReceiveWithAnyArgs().Generate(null);
		}

		[Test]
		public void Constructor_AddsNoSubscriptions()
		{
			// Assert
			subscriptionStore.DidNotReceiveWithAnyArgs().AddSubscription(null, null);
		}

		[Test]
		public void Constructor_AddNoSubscribers()
		{
			// Assert
			subscriptionStore.DidNotReceiveWithAnyArgs().AddSubscriber(null, null);
		}

		[Test]
		public void Subscribe_WhenSubscriptionDoesntExist_GeneratesTheFeed()
		{
			// Act
			subscriptionManagerUnderTest.Subscribe("XYZ", "client_1");

			// Assert
			priceFeedGenerator.Received(1).Generate("XYZ");
		}

		[Test]
		public void Subscribe_WhenSubscriptionDoesntExist_AddsTheSubscription()
		{
			// Arrange
			var priceFeed = SetupFeedForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Subscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.Received(1).AddSubscription("XYZ", priceFeed);
		}

		[Test]
		public void Subscribe_WhenSubscriptionDoesntExist_AddsTheSubscriber()
		{
			// Act
			subscriptionManagerUnderTest.Subscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.Received(1).AddSubscriber("XYZ", "client_1");
		}

		[Test]
		public void Subscribe_WhenSubscriptionExists_DoesntGenerateTheFeed()
		{
			// Arrange
			SetupSubscriptionForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Subscribe("XYZ", "client_1");

			// Assert
			priceFeedGenerator.DidNotReceive().Generate("XYZ");
		}

		[Test]
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

		[Test]
		public void Subscribe_WhenSubscriptionExists_AddsTheSubscriber()
		{
			// Arrange
			SetupSubscriptionForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Subscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.Received(1).AddSubscriber("XYZ", "client_1");
		}


		[Test]
		public void Unsubscribe_LastSubscriber_DisposesTheFeed()
		{
			// Arrange
			var priceFeed = SetupFeedForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Unsubscribe("XYZ", "client_1");

			// Assert
			priceFeed.Received(1).Dispose();
		}

		[Test]
		public void Unsubscribe_LastSubscriber_RemovesTheSubscription()
		{
			// Act
			subscriptionManagerUnderTest.Unsubscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.Received(1).RemoveSubscription("XYZ");
		}

		[Test]
		public void Unsubscribe_LastSubscriber_RemovesTheSubscriber()
		{
			// Act
			subscriptionManagerUnderTest.Unsubscribe("XYZ", "client_1");
			
			// Assert
			subscriptionStore.Received(1).RemoveSubscriber("XYZ", "client_1");
		}

		[Test]
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

		[Test]
		public void Unsubscribe_NotLastSubscriber_DoesntRemoveTheSubscription()
		{
			// Arrange
			SetupSubscriptionForSymbol("XYZ");

			// Act
			subscriptionManagerUnderTest.Unsubscribe("XYZ", "client_1");

			// Assert
			subscriptionStore.DidNotReceive().RemoveSubscription("XYZ");
		}

		[Test]
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
			var observable = Substitute.For<IObservable<StockPrice>>();

			priceFeedSubscriber.Subscribe(observable).Returns(disposable);
			priceFeedGenerator.Generate(symbol).Returns(observable);
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
