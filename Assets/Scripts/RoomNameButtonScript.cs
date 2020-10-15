using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomNameButtonScript : MonoBehaviour
{
    [SerializeField]
    GameObject createRoomButton = null;
    CreateRoomButton createRoomButtonScript = null;
    // Start is called before the first frame update
    void Start()
    {
        createRoomButtonScript = createRoomButton.GetComponent<CreateRoomButton>();
    }

    
    public void ValueChange()
    {
        Debug.Log("入力されました");
    }
    public void EndEdit()
    {
        createRoomButtonScript.SetRoomName(GetComponent<InputField>().text);
        createRoomButton.SetActive(true);
    }
}
