using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ZNet;

public class GameMove : MonoBehaviour
{
    public void MatgoMove()
    {
        NetworkManager.Instance.client.RequestLobbyKey(ZNet.RemoteID.Remote_Server, ZNet.CPackOption.Encrypt, NetworkManager.Instance.IdentifierID, NetworkManager.Instance.IdentifierKey);
    }
}