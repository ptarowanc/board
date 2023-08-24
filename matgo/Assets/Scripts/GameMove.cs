using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using ZNet;

public class GameMove : MonoBehaviour
{
    public void BadugiMove()
    {
        NetworkManager.Instance.client.RequestLobbyKey(RemoteID.Remote_Server, CPackOption.Encrypt, NetworkManager.Instance.IdentifierID, NetworkManager.Instance.IdentifierKey);
    }
}