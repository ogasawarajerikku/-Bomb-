using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.UI;

public class NetWorkController : MonobitEngine.MonoBehaviour
{
    string serverName = "!Bomb!_v1.0";

    /** プレイヤーキャラクタ. */
    private GameObject playerObject = null;

    //ルーム内のプレイヤー人数
    int inRoomPlayerCount = 0;

    [SerializeField, Tooltip("プレイヤー出現位置")]
    Transform[] spawnPoints = null;

    //ゲーム開始時の準備確認用リスト
    List<bool> readyPlayers = new List<bool>();
    bool player1_ready = false;
    bool player2_ready = false;
    bool player3_ready = false;
    bool player4_ready = false;
    int readyPlayersCount = 0;

    //すでに存在しているルームのテキストのリスト
    [SerializeField]
    List<GameObject> roomButtons = new List<GameObject>();
    [SerializeField]
    List<Text> roomButtonTexts = new List<Text>();
    int roomCount = 0;

    //ロビー接続時に使用するUI類
    [SerializeField]
    GameObject createRoomNameField=null;
    [SerializeField]
    GameObject joinRandomButton = null;
    [SerializeField]
    GameObject roomsButton = null;
    public enum ConnectStep
    {
        Awake, ServerConnecting, LobbyEntering,RoomSelect, RoomCreating, RoomEntering, RoomEnterd
    }
    private ConnectStep connectStep = ConnectStep.Awake;

    // Start is called before the first frame update
    void Start()
    {
        //ゲーム開始管理用準備完了プレイヤーリスト
        readyPlayers.Add(player1_ready);
        readyPlayers.Add(player2_ready);
        readyPlayers.Add(player3_ready);
        readyPlayers.Add(player4_ready);
        StartCoroutine(ConnetSequence());
    }

    // Update is called once per frame
    void Update()
    {
    }
    IEnumerator ConnetSequence()
    {
        yield return new WaitForEndOfFrame();

        // デフォルトロビーへの自動入室を許可する
        MonobitNetwork.autoJoinLobby = true;

        // MUNサーバに接続する
        MonobitNetwork.ConnectServer(serverName);
        connectStep = ConnectStep.ServerConnecting;

        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (MonobitNetwork.isConnect)
            {
                connectStep = ConnectStep.LobbyEntering;
                break;
            }
        }

        while (true)
        {
            yield return new WaitForSeconds(1.0f);
            if (MonobitNetwork.inLobby)
            {
                connectStep = ConnectStep.RoomCreating;
                break;
            }
        }
       while(true)
        {
            yield return new WaitForSeconds(1.0f);
            if(!MonobitNetwork.inRoom)
            {
                checkObjectFrag(createRoomNameField);
                checkObjectFrag(joinRandomButton);
                checkObjectFrag(roomsButton);
                for (int i = 0; i < roomButtonTexts.Count; i++)
                {
                    roomButtonTexts[i].text = "No Room";
                }
                // ルーム一覧から選択式で入室する
                //取得したルーム情報をボタンに表示させる
                foreach (RoomData room in MonobitNetwork.GetRoomData())
                {
                    string strRoomInfo =
                        string.Format("{0}({1}/{2})",
                                      room.name,
                                      room.playerCount,
                                      (room.maxPlayers == 0) ? "-" : room.maxPlayers.ToString());
                    roomButtonTexts[roomCount].text = ("Enter Room :" + strRoomInfo);
                    roomButtons[roomCount].GetComponent<selectRoomButton>().SetRoomName(room.name);
                    if (roomCount < roomButtons.Count)
                        roomCount++;
                }
                roomCount = 0;
            }
            if (MonobitNetwork.inRoom)
            {
                Debug.Log("入室しました");
                createRoomNameField.SetActive(false);
                joinRandomButton.SetActive(false);
                roomsButton.SetActive(false);
                for (int i = 0; i < roomButtons.Count; i++)
                {
                    if (roomButtons[i].activeInHierarchy == true)
                        roomButtons[i].SetActive(false);
                }
                connectStep = ConnectStep.RoomEnterd;
                break;
            }
        }

        yield return null;

    }
    void checkObjectFrag(GameObject obj)
    {
        if (obj.activeInHierarchy == false)
            obj.SetActive(true);
    }
    private void OnDestroy()
    {
        StopAllCoroutines();
    }
}