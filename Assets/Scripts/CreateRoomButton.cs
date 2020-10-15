using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
public class CreateRoomButton : MonobitEngine.MonoBehaviour
{
    /** ルーム名. */
    private string roomName = "";
    public void SetRoomName(string newRoomName)
    {
        roomName = newRoomName;
    }
    public void inputButton()
    {
        MonobitEngine.RoomSettings settings = new MonobitEngine.RoomSettings();
        settings.maxPlayers = 4;
        settings.isVisible = true;
        settings.isOpen = true;
        MonobitNetwork.CreateRoom(roomName, settings, null);
    }
}
