using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomInfo : MonoBehaviour
{
    [System.NonSerialized]
    public int channel;
    [System.NonSerialized]
    public int roomNum;
    [System.NonSerialized]
    public int bet;
    [System.NonSerialized]
    public bool isClose;
    [System.NonSerialized]
    public bool isPW;

    public Text RoomNumber;
    public Text RoomBet;

    public void onClick()
    {
        LobbyPopup.Instance.StartJoinInfo(isClose, isPW, channel, roomNum);
        RoomNumber.text = roomNum + "번방";

        RoomBet.text = GetRoomBet(ChannerManager.Instance.NowChannel, bet);
    }

    public string GetRoomBet(int channel, int betType)
    {
        string roomBet = "";
        switch (channel)
        {
            case 1:
                switch (betType)
                {
                    case 1: roomBet = "100"; break;
                }
                break;
            case 2:
                switch (betType)
                {
                    case 1: roomBet = "10만"; break;
                    case 2: roomBet = "100만"; break;
                    case 3: roomBet = "1천만"; break;
                    case 4: roomBet = "3천만"; break;
                    case 5: roomBet = "1억"; break;
                }
                break;
            case 3:
                switch (betType)
                {
                    case 1: roomBet = "3천만"; break;
                    case 2: roomBet = "1억"; break;
                    case 3: roomBet = "3억"; break;
                    case 4: roomBet = "5억"; break;
                    case 5: roomBet = "10억"; break;
                }
                break;
            case 4:
                switch (betType)
                {
                    case 1: roomBet = "100"; break;
                    case 2: roomBet = "10만"; break;
                    case 3: roomBet = "100만"; break;
                    case 4: roomBet = "1천만"; break;
                    case 5: roomBet = "3천만"; break;
                    case 6: roomBet = "1억"; break;
                    case 7: roomBet = "3억"; break;
                    case 8: roomBet = "5억"; break;
                    case 9: roomBet = "10억"; break;
                }
                break;
            case 5:
                switch (betType)
                {
                    case 1: roomBet = "200"; break;
                    case 2: roomBet = "500"; break;
                    case 3: roomBet = "1000"; break;
                    case 4: roomBet = "2000"; break;
                    case 5: roomBet = "5000"; break;
                }
                break;
            case 6:
                switch (betType)
                {
                    case 1: roomBet = "1000"; break;
                    case 2: roomBet = "2000"; break;
                    case 3: roomBet = "5000"; break;
                    case 4: roomBet = "5000"; break;
                    case 5: roomBet = "10000"; break;
                }
                break;
            case 7:
                switch (betType)
                {
                    case 1: roomBet = "1"; break;
                    case 2: roomBet = "100"; break;
                    case 3: roomBet = "300"; break;
                    case 4: roomBet = "500"; break;
                    case 5: roomBet = "1000"; break;
                }
                break;
            case 8:
                switch (betType)
                {
                    case 1: roomBet = "1"; break;
                    case 2: roomBet = "100"; break;
                    case 3: roomBet = "300"; break;
                    case 4: roomBet = "500"; break;
                    case 5: roomBet = "1000"; break;
                }
                break;
        }
        roomBet = roomBet += "냥";
        return roomBet;
    }
}