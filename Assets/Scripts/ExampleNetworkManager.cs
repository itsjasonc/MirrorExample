using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

namespace io.choy.MirrorExample
{
    public class ExampleNetworkManager : NetworkManager
    {
        public override void OnServerAddPlayer(NetworkConnection conn)
        {
            base.OnServerAddPlayer(conn);
        }

        public override void OnClientConnect(NetworkConnection conn)
        {
            base.OnClientConnect(conn);
        }

        void OnError(NetworkMessage networkMessage)
        {
            
        }

        public override void OnClientError(NetworkConnection conn, int errorCode)
        {
            base.OnClientError(conn, errorCode);
            Debug.Log("Client error code: " + errorCode);
        }

        public override void OnServerError(NetworkConnection conn, int errorCode)
        {
            base.OnServerError(conn, errorCode);
            Debug.Log("Server error code: " + errorCode);
        }
    }
}