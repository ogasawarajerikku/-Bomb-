using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
public class JoinRandomButton : MonobitEngine.MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void inputButton()
    {
        MonobitNetwork.JoinRandomRoom();
    }
}
