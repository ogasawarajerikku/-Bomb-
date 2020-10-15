using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MonobitEngine;
public class selectRoomButton : MonobitEngine.MonoBehaviour
{
    string roomNameText = null;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void inputButton()
    {
        MonobitNetwork.JoinRoom(roomNameText);
    }

    public void SetRoomName(string text)
    {
        roomNameText = text;
    }
}
