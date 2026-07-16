using System;
using Mirror;
using UnityEngine;

namespace InventorySystem.Items.Pickups
{
    public abstract class PickupPhysicsModule
    {
        private static readonly byte[] ReaderBuffer = new byte[1024];

        private bool _wasSpawned;

        private SyncList<byte> SyncData => Pickup.PhysicsModuleSyncData;

        protected bool IsSpawned
        {
            get
            {
                if (_wasSpawned) return true;

                if (NetworkServer.spawned.ContainsKey(Pickup.netId))
                {
                    _wasSpawned = true;
                    return true;
                }

                return false;
            }
        }

        protected abstract ItemPickupBase Pickup { get; }

        public virtual void DestroyModule() { }

        [Client]
        internal virtual void ClientProcessRpc(NetworkReader rpcData)
        {
            if (!NetworkClient.active)
            {
                Debug.LogWarning("[Client] function 'ClientProcessRpc' called when client was not active");
                return;
            }
        }

        [Server]
        protected void ServerSetSyncData(Action<NetworkWriter> writerMethod)
        {
            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'ServerSetSyncData' called when server was not active");
                return;
            }

            using (NetworkWriterPooled writer = NetworkWriterPool.Get())
            {
                writerMethod?.Invoke(writer);

                SyncData.Clear();

                ArraySegment<byte> buffer = writer.ToArraySegment();
                foreach (byte b in buffer)
                    SyncData.Add(b);
            }
        }

        [Client]
        protected void ClientReadSyncData(Action<NetworkReader> readerMethod)
        {
            if (!NetworkClient.active)
            {
                Debug.LogWarning("[Client] function 'ClientReadSyncData' called when client was not active");
                return;
            }

            SyncList<byte> syncData = SyncData;
            int count = syncData.Count;

            if (count > 0)
            {
                for (int i = 0; i < count && i < ReaderBuffer.Length; i++)
                    ReaderBuffer[i] = syncData[i];

                ArraySegment<byte> segment = new ArraySegment<byte>(ReaderBuffer, 0, count);
                using (NetworkReaderPooled reader = NetworkReaderPool.Get(segment))
                    readerMethod?.Invoke(reader);
            }
        }

        [Server]
        protected void ServerSendRpc(Action<NetworkWriter> writerMethod)
        {
            if (!IsSpawned) return;

            if (!NetworkServer.active)
            {
                Debug.LogWarning("[Server] function 'ServerSendRpc' called when server was not active");
                return;
            }

            using NetworkWriterPooled writer = NetworkWriterPool.Get();
            writerMethod?.Invoke(writer);
            Pickup.SendPhysicsModuleRpc(writer.ToArraySegment());
        }
    }
}