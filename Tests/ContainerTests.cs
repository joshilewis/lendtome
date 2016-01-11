using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;
using EventStore.ClientAPI.SystemData;
using Lending.Cqrs;
using Lending.Domain;
using Lending.Execution.DI;
using Lending.Execution.EventStore;
using NUnit.Framework;
using StructureMap;

namespace Tests
{
    [TestFixture]
    public class ContainerTests
    {
        [Test]
        public void Test()
        {
            var container = new Container(x =>
            {
                x.For<IEventStoreConnection>()
                    .Use<DummyConnection>();

                x.Scan(scan =>
                {
                    scan.LookForRegistries();
                    scan.AssemblyContainingType<DomainRegistry>();
                    scan.WithDefaultConventions();
                });

            });

            container.AssertConfigurationIsValid();

        }
    }

    public class DummyConnection : IEventStoreConnection
    {
        event System.EventHandler<ClientConnectionEventArgs> IEventStoreConnection.Connected
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event System.EventHandler<ClientConnectionEventArgs> IEventStoreConnection.Disconnected
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event System.EventHandler<ClientReconnectingEventArgs> IEventStoreConnection.Reconnecting
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event System.EventHandler<ClientClosedEventArgs> IEventStoreConnection.Closed
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event System.EventHandler<ClientErrorEventArgs> IEventStoreConnection.ErrorOccurred
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        event System.EventHandler<ClientAuthenticationFailedEventArgs> IEventStoreConnection.AuthenticationFailed
        {
            add
            {
                throw new NotImplementedException();
            }

            remove
            {
                throw new NotImplementedException();
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task ConnectAsync()
        {
            throw new NotImplementedException();
        }

        public void Close()
        {
            throw new NotImplementedException();
        }

        public Task<DeleteResult> DeleteStreamAsync(string stream, int expectedVersion, UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<DeleteResult> DeleteStreamAsync(string stream, int expectedVersion, bool hardDelete, UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<WriteResult> AppendToStreamAsync(string stream, int expectedVersion, params EventData[] events)
        {
            throw new NotImplementedException();
        }

        public Task<WriteResult> AppendToStreamAsync(string stream, int expectedVersion, UserCredentials userCredentials, params EventData[] events)
        {
            throw new NotImplementedException();
        }

        public Task<WriteResult> AppendToStreamAsync(string stream, int expectedVersion, IEnumerable<EventData> events, UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<EventStoreTransaction> StartTransactionAsync(string stream, int expectedVersion, UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public EventStoreTransaction ContinueTransaction(long transactionId, UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<EventReadResult> ReadEventAsync(string stream, int eventNumber, bool resolveLinkTos, UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<StreamEventsSlice> ReadStreamEventsForwardAsync(string stream, int start, int count, bool resolveLinkTos,
            UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<StreamEventsSlice> ReadStreamEventsBackwardAsync(string stream, int start, int count, bool resolveLinkTos,
            UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<AllEventsSlice> ReadAllEventsForwardAsync(Position position, int maxCount, bool resolveLinkTos,
            UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<AllEventsSlice> ReadAllEventsBackwardAsync(Position position, int maxCount, bool resolveLinkTos,
            UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<EventStoreSubscription> SubscribeToStreamAsync(string stream, bool resolveLinkTos, Action<EventStoreSubscription, ResolvedEvent> eventAppeared, Action<EventStoreSubscription, SubscriptionDropReason, Exception> subscriptionDropped = null,
            UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public EventStoreStreamCatchUpSubscription SubscribeToStreamFrom(string stream, int? lastCheckpoint, bool resolveLinkTos,
            Action<EventStoreCatchUpSubscription, ResolvedEvent> eventAppeared, Action<EventStoreCatchUpSubscription> liveProcessingStarted = null, Action<EventStoreCatchUpSubscription, SubscriptionDropReason, Exception> subscriptionDropped = null,
            UserCredentials userCredentials = null, int readBatchSize = 500)
        {
            throw new NotImplementedException();
        }

        public Task<EventStoreSubscription> SubscribeToAllAsync(bool resolveLinkTos, Action<EventStoreSubscription, ResolvedEvent> eventAppeared, Action<EventStoreSubscription, SubscriptionDropReason, Exception> subscriptionDropped = null,
            UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public EventStorePersistentSubscriptionBase ConnectToPersistentSubscription(string stream, string groupName,
            Action<EventStorePersistentSubscriptionBase, ResolvedEvent> eventAppeared, Action<EventStorePersistentSubscriptionBase, SubscriptionDropReason, Exception> subscriptionDropped = null, UserCredentials userCredentials = null, int bufferSize = 10,
            bool autoAck = true)
        {
            throw new NotImplementedException();
        }

        public EventStoreAllCatchUpSubscription SubscribeToAllFrom(Position? lastCheckpoint, bool resolveLinkTos, Action<EventStoreCatchUpSubscription, ResolvedEvent> eventAppeared,
            Action<EventStoreCatchUpSubscription> liveProcessingStarted = null, Action<EventStoreCatchUpSubscription, SubscriptionDropReason, Exception> subscriptionDropped = null, UserCredentials userCredentials = null,
            int readBatchSize = 500)
        {
            throw new NotImplementedException();
        }

        public Task UpdatePersistentSubscriptionAsync(string stream, string groupName, PersistentSubscriptionSettings settings,
            UserCredentials credentials)
        {
            throw new NotImplementedException();
        }

        public Task CreatePersistentSubscriptionAsync(string stream, string groupName, PersistentSubscriptionSettings settings,
            UserCredentials credentials)
        {
            throw new NotImplementedException();
        }

        public Task DeletePersistentSubscriptionAsync(string stream, string groupName, UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<WriteResult> SetStreamMetadataAsync(string stream, int expectedMetastreamVersion, StreamMetadata metadata,
            UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<WriteResult> SetStreamMetadataAsync(string stream, int expectedMetastreamVersion, byte[] metadata,
            UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<StreamMetadataResult> GetStreamMetadataAsync(string stream, UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task<RawStreamMetadataResult> GetStreamMetadataAsRawBytesAsync(string stream, UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public Task SetSystemSettingsAsync(SystemSettings settings, UserCredentials userCredentials = null)
        {
            throw new NotImplementedException();
        }

        public string ConnectionName { get; }
        public event EventHandler Connected;
        public event EventHandler Disconnected;
        public event EventHandler Reconnecting;
        public event EventHandler Closed;
        public event EventHandler ErrorOccurred;
        public event EventHandler AuthenticationFailed;
    }
}
