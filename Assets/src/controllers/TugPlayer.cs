namespace TugOWar
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using Unity.Netcode;

    public class TugPlayer : NetworkBehaviour 
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                var randomPosition = GetRandomPositionOnScene();
                transform.position = randomPosition;
                Position.Value = randomPosition;
            }
            else
            {
                SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(ServerRpcParams rpcParams = default)
        {
            Position.Value = GetRandomPositionOnScene();
        }

        static Vector3 GetRandomPositionOnScene()
        {
            return new Vector3(Random.Range(-2f, 2f), Random.Range(-3f, 3f), 0f);
        }

        void Update()
        {
            transform.position = Position.Value;
        }
    }
}
