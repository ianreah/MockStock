using System;
using MockStock.Core;
using NSubstitute;
using NUnit.Framework;

namespace MockStock.Tests
{
	[TestFixture]
	public class InMemorySubscriptionStoreTests
	{
		private InMemorySubscriptionStore storeUnderTest;

		[SetUp]
		public void TestInitialize()
		{
			storeUnderTest = new InMemorySubscriptionStore();
		}

		[Test]
		public void Exists_WhenSubscriptionNotAdded_ReturnsFalse()
		{
			Assert.That(storeUnderTest.Exists("XYZ"), Is.False);
		}

		[Test]
		public void Exists_AfterSubscriptionAdded_ReturnsTrue()
		{
			storeUnderTest.AddSubscription("XYZ", Substitute.For<IDisposable>());

			Assert.That(storeUnderTest.Exists("XYZ"), Is.True);
		}

		[Test]
		public void Exists_AfterSubscriptionAddedAndRemoved_ReturnsFalse()
		{
			storeUnderTest.AddSubscription("XYZ", Substitute.For<IDisposable>());
			storeUnderTest.RemoveSubscription("XYZ");

			Assert.That(storeUnderTest.Exists("XYZ"), Is.False);
		}

		[Test]
		public void AnySubscribers_WhenSubscriptionNotAdded_ReturnsFalse()
		{
			Assert.That(storeUnderTest.AnySubscribers("XYZ"), Is.False);
		}

		[Test]
		public void AnySubscribers_AfterSubscriptionAdded_ReturnsFalse()
		{
			storeUnderTest.AddSubscription("XYZ", Substitute.For<IDisposable>());

			Assert.That(storeUnderTest.AnySubscribers("XYZ"), Is.False);
		}

		[Test]
		public void AnySubscribers_AfterSubscriptionAndSubscriberAdded_ReturnsTrue()
		{
			storeUnderTest.AddSubscription("XYZ", Substitute.For<IDisposable>());
			storeUnderTest.AddSubscriber("XYZ", "client_1");

			Assert.That(storeUnderTest.AnySubscribers("XYZ"), Is.True);
		}

		[Test]
		public void AnySubscribers_WithTwoSubscribersAndOneRemoved_ReturnsTrue()
		{
			storeUnderTest.AddSubscription("XYZ", Substitute.For<IDisposable>());
			storeUnderTest.AddSubscriber("XYZ", "client_1");
			storeUnderTest.AddSubscriber("XYZ", "client_2");
			
			storeUnderTest.RemoveSubscriber("XYZ", "client_2");

			Assert.That(storeUnderTest.AnySubscribers("XYZ"), Is.True);
		}

		[Test]
		public void AnySubscribers_WithTwoSubscribersAndBothRemoved_ReturnsFalse()
		{
			storeUnderTest.AddSubscription("XYZ", Substitute.For<IDisposable>());
			storeUnderTest.AddSubscriber("XYZ", "client_1");
			storeUnderTest.AddSubscriber("XYZ", "client_2");

			storeUnderTest.RemoveSubscriber("XYZ", "client_1");
			storeUnderTest.RemoveSubscriber("XYZ", "client_2");

			Assert.That(storeUnderTest.AnySubscribers("XYZ"), Is.False);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void AddSubscription_WhenAlreadyExists_Throws()
		{
			storeUnderTest.AddSubscription("XYZ", Substitute.For<IDisposable>());
			storeUnderTest.AddSubscription("XYZ", Substitute.For<IDisposable>());
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void RemoveSubscription_WhenDoesntExist_Throws()
		{
			storeUnderTest.RemoveSubscription("XYZ");
		}

		[Test]
		public void RemoveSubscription_ThatDoesExist_ReturnsTheSubscription()
		{
			var subscription = Substitute.For<IDisposable>();

			storeUnderTest.AddSubscription("XYZ", subscription);
			Assert.That(storeUnderTest.RemoveSubscription("XYZ"), Is.EqualTo(subscription));
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void AddSubscriber_WhenSubscriptionDoesntExist_Throws()
		{
			storeUnderTest.AddSubscriber("XYZ", "client_1");
		}

		[Test]
		public void AddSubscriber_WhenClientAlreadySubscribed_HasNoEffect()
		{
			storeUnderTest.AddSubscription("XYZ", Substitute.For<IDisposable>());
			storeUnderTest.AddSubscriber("XYZ", "client_1");

			storeUnderTest.AddSubscriber("XYZ", "client_1");

			// Only need to remove the subscriber ONCE 
			storeUnderTest.RemoveSubscriber("XYZ", "client_1");
			Assert.That(storeUnderTest.AnySubscribers("XYZ"), Is.False);
		}

		[Test, ExpectedException(typeof(InvalidOperationException))]
		public void RemoveSubscriber_WhenSubscriptionDoesntExist_Throws()
		{
			storeUnderTest.RemoveSubscriber("XYZ", "client_1");
		}

		[Test]
		public void RemoveSubscriber_FromExistingSubscriptionButClientNotSubscribed_HasNoEffect()
		{
			storeUnderTest.AddSubscription("XYZ", Substitute.For<IDisposable>());
			storeUnderTest.AddSubscriber("XYZ", "client_1");

			storeUnderTest.RemoveSubscriber("XYZ", "client_2");

			// Still has the subscriber
			Assert.That(storeUnderTest.AnySubscribers("XYZ"), Is.True);
		}
	}
}
