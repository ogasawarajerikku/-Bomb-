using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MonobitEngine;
using UnityEngine.SceneManagement;
public class NetworkControll : MonobitEngine.MonoBehaviour
{
    /** ルーム名. */
    private string roomName = "";

    /** プレイヤーキャラクタ. */
    private GameObject playerObject = null;

    //ルーム内のプレイヤー人数
    int inRoomPlayerCount = 0;

    [SerializeField, Tooltip("プレイヤー出現位置")]
    Transform[] spawnPoints = null;

    public GUIStyle buttonStyle = new GUIStyle();
    public GUIStyle textFieldStyle = new GUIStyle();

    public float uiButtonHeight = 0;
    public float uiButtonWidth = 0;

    //ゲーム開始時の準備確認用リスト
    List<bool> readyPlayers = new List<bool>();
    bool player1_ready = false;
    bool player2_ready = false;
    bool player3_ready = false;
    bool player4_ready = false;
    int readyPlayersCount = 0;

    // Start is called before the first frame update
    void Start()
    {

        // デフォルトロビーへの自動入室を許可する
        MonobitNetwork.autoJoinLobby = true;

        // MUNサーバに接続する
        MonobitNetwork.ConnectServer("!Bomb!_v1.0");

        //ゲーム開始管理用準備完了プレイヤーリスト
        readyPlayers.Add(player1_ready);
        readyPlayers.Add(player2_ready);
        readyPlayers.Add(player3_ready);
        readyPlayers.Add(player4_ready);

    }

    // Update is called once per frame
    void Update()
    {
        //入室しているプレイヤー全員が準備完了ならゲームスタート
        if (!MonobitEngine.MonobitNetwork.isHost)
        {
            return;
        }

        // MUNサーバに接続しており、かつルームに入室している場合
        if (MonobitNetwork.isConnect && MonobitNetwork.inRoom)
        {
            inRoomPlayerCount = MonobitEngine.MonobitNetwork.room.playerCount;
            //準備完了していたらカウント
            for (int i = 0; i < readyPlayers.Count; i++)
            {
                if (readyPlayers[i] == true)
                {
                    readyPlayersCount++;
                }
            }
            if (inRoomPlayerCount == readyPlayersCount)
                monobitView.RPC("PlayerInstantiate", MonobitTargets.All);
            readyPlayersCount = 0;
        }
    }

    [MunRPC]
    public void ReceiveGetReady(int id)
    {
        readyPlayers[id - 1] = true;
    }

    [MunRPC]
    public void PlayerInstantiate()
    {
        if (playerObject == null)
        {
            playerObject = MonobitNetwork.Instantiate(
                            "Player",
                            spawnPoints[MonobitEngine.MonobitNetwork.player.ID].position,
                            Quaternion.identity,
                            0);
            playerObject.name = "Player" + MonobitEngine.MonobitNetwork.player.ID;
        }
    }

    private void OnGUI()
    {
        // デフォルトのボタンと被らないように、段下げを行なう。
        GUILayout.Space(24);

        // MUNサーバに接続している場合
        if (MonobitNetwork.isConnect)
        {
            // ルームに入室している場合
            if (MonobitNetwork.inRoom)
            {
                // ボタン入力でルームから退室
                if (GUILayout.Button("Leave Room", buttonStyle, GUILayout.Width(uiButtonWidth), GUILayout.Height(uiButtonHeight)))
                {
                    if (MonobitEngine.MonobitNetwork.isHost)
                        for (int i = 0; i < readyPlayers.Count; i++)
                        {
                            readyPlayers[i] = false;
                        }
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                    MonobitNetwork.LeaveRoom();
                }
                //ボタン入力で準備完了
                if (GUILayout.Button("Ready", buttonStyle, GUILayout.Width(uiButtonWidth), GUILayout.Height(uiButtonHeight)))
                {
                    monobitView.RPC("ReceiveGetReady", MonobitTargets.Host, MonobitEngine.MonobitNetwork.player.ID);
                }
            }
        }
        // ルームに入室していない場合
        if (!MonobitNetwork.inRoom)
        {
            GUILayout.BeginHorizontal();

            // ルーム名の入力
            GUILayout.Label("RoomName : ", buttonStyle, GUILayout.Width(uiButtonWidth), GUILayout.Height(uiButtonHeight));
            roomName = GUILayout.TextField(roomName, textFieldStyle, GUILayout.Width(uiButtonWidth), GUILayout.Height(uiButtonHeight));
            MonobitEngine.RoomSettings settings = new MonobitEngine.RoomSettings();
            settings.maxPlayers = 4;
            settings.isVisible = true;
            settings.isOpen = true;
            // ボタン入力でルーム作成
            if (GUILayout.Button("Create Room", buttonStyle, GUILayout.Width(uiButtonWidth), GUILayout.Height(uiButtonHeight)))
            {
                MonobitNetwork.CreateRoom(roomName, settings, null);
            }

            GUILayout.EndHorizontal();

            // 現在存在するルームからランダムに入室する
            if (GUILayout.Button("Join Random Room", buttonStyle, GUILayout.Width(uiButtonWidth), GUILayout.Height(uiButtonHeight)))
            {
                MonobitNetwork.JoinRandomRoom();
            }

            // ルーム一覧から選択式で入室する
            foreach (RoomData room in MonobitNetwork.GetRoomData())
            {
                string strRoomInfo =
                    string.Format("{0}({1}/{2})",
                                  room.name,
                                  room.playerCount,
                                  (room.maxPlayers == 0) ? "-" : room.maxPlayers.ToString());

                if (GUILayout.Button("Enter Room : " + strRoomInfo, textFieldStyle))
                {
                    MonobitNetwork.JoinRoom(room.name);
                }
            }
        }
    }
}

