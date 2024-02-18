using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;

public class SampleController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private List<GameObject> Cards;
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("マッチング中");
        PhotonNetwork.NickName = "Player";
        // PhotonServerSettingsの設定内容を使ってマスターサーバーへ接続する
        PhotonNetwork.ConnectUsingSettings();
    }

    // マスターサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnConnectedToMaster()
    {
        // "Room"という名前のルームに参加する（ルームが存在しなければ作成して参加する）
        PhotonNetwork.JoinRandomRoom();
    }

    // ランダムで参加できるルームが存在しないなら、新規でルームを作成する
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ルームの参加人数を2人に設定する
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // ゲームサーバーへの接続が成功した時に呼ばれるコールバック
    public override void OnJoinedRoom()
    {
        Debug.Log("Playerがログインしました");
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        // ランダムな座標に自身のアバター（ネットワークオブジェクト）を生成する
        var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        GameObject Obj = PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
    }

    /// <summary>
    /// カードを配置する
    /// </summary>
    public void Set()
    {
        for(int i = 1; i <=13; i++)
        {
            GameObject Card = PhotonNetwork.Instantiate("c" + i.ToString("00"), new Vector3(-9f + i * 1.5f, 3f, 0f), Quaternion.identity);
            Cards.Add(Card);
        }
        for (int i = 1; i <= 13; i++)
        {
            GameObject Card = PhotonNetwork.Instantiate("d" + i.ToString("00"), new Vector3(-9f + i * 1.5f, 1f, 0f), Quaternion.identity);
            Cards.Add(Card);
        }
        for (int i = 1; i <= 13; i++)
        {
            GameObject Card = PhotonNetwork.Instantiate("h" + i.ToString("00"), new Vector3(-9f + i * 1.5f, -1f, 0f), Quaternion.identity);
            Cards.Add(Card);
        }
        for (int i = 1; i <= 13; i++)
        {
            GameObject Card = PhotonNetwork.Instantiate("s" + i.ToString("00"), new Vector3(-9f + i * 1.5f, -3f, 0f), Quaternion.identity);
            Cards.Add(Card);
        }

        CardShuffle();
    }

    /// <summary>
    /// 配置されたカードをシャッフルする
    /// </summary>
    public void CardShuffle()
    {
        for(int i = 0;i < 10; i++)
        {
            int number = Random.Range(0, Cards.Count);
            int number2 = Random.Range(0, Cards.Count);

            if(number != number2)
            {
                Vector3 fistpos = Cards[number].transform.position;
                Cards[number].transform.position = Cards[number2].transform.position;
                Cards[number2].transform.position = fistpos;
            }
        }
    }
}
