using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using UnityEngine;
public class SampleController : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private List<GameObject> Cards;
    [SerializeField]
    private GameObject[] selectcards;
    [SerializeField]
    private int selectnumber;
    public int player1count, player2count;
    private AvatarScript Player;
    public bool turnend;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("マッチング中");
        turnend = false;
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
        // すでに配置されてたら生成しない
        if (Player == null)
        {
            GameObject Obj = PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
            Player = Obj.GetComponent<AvatarScript>();
        }
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log(otherPlayer + "が退出しました。ゲーム終了いたします");
    }

    /// <summary>
    /// カードを配置する
    /// </summary>
    public void Set()
    {
        for(int i = 1; i <=13; i++)
        {
            GameObject Card = PhotonNetwork.Instantiate("c" + i.ToString("00"), new Vector3(-7f + i * 1f, 3f, 0f), Quaternion.identity);
            Cards.Add(Card);
        }
        for (int i = 1; i <= 13; i++)
        {
            GameObject Card = PhotonNetwork.Instantiate("d" + i.ToString("00"), new Vector3(-7f + i * 1f, 1f, 0f), Quaternion.identity);
            Cards.Add(Card);
        }
        for (int i = 1; i <= 13; i++)
        {
            GameObject Card = PhotonNetwork.Instantiate("h" + i.ToString("00"), new Vector3(-7f + i * 1f, -1f, 0f), Quaternion.identity);
            Cards.Add(Card);
        }
        for (int i = 1; i <= 13; i++)
        {
            GameObject Card = PhotonNetwork.Instantiate("s" + i.ToString("00"), new Vector3(-7f + i * 1f, -3f, 0f), Quaternion.identity);
            Cards.Add(Card);
        }
        photonView.RPC(nameof(SetCard), RpcTarget.Others);
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

    public void CardSerach(GameObject obj)
    {
        int number = Cards.IndexOf(obj);
        Debug.Log("CardNumber:" + number);
        //selectcards[selectnumber] = obj;
        //selectnumber++;
        // 全員に伝える
        photonView.RPC(nameof(CardOpen), RpcTarget.All, number);
    }

    public bool CardCheck(GameObject obj)
    {
        if (selectcards[0] == obj || selectcards[1] == obj)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    [PunRPC]
    public void CardOpen(int cardnumber)
    {
        Debug.Log(cardnumber);
        GameObject card = Cards[cardnumber];
        if (selectnumber > 0 && card == Cards[0])
        {
            return;
        }

        selectcards[selectnumber] = card;
        selectcards[selectnumber].GetComponent<Card>().Touch();
        if (selectnumber > 0)
        {
            Debug.Log("選択したカードの判定");

            if (!Judge())
            {
                turnend = false;
                // 相手のターンにする
                Player.OnConnected();
                CardReset();
            }
        }
        else
        {
            selectnumber++;
        }
    }
    [PunRPC]
    public void SetCard()
    {
        GameObject[] Cards = GameObject.FindGameObjectsWithTag("Card");
        foreach(GameObject Card in Cards)
        {
            this.Cards.Add(Card);
        }
    }

    private bool Judge()
    {
        int card1number = selectcards[0].GetComponent<Card>().number;
        int card2number = selectcards[1].GetComponent<Card>().number;

        if(card1number == card2number)
        {
            Debug.Log("同じカードが揃いました！");
            if(PhotonNetwork.IsMasterClient)
            {
                player1count += 2;
            }
            else
            {
                player2count += 2;
            }
            selectcards[0].GetComponent<Card>().MatchProcess();
            selectcards[1].GetComponent<Card>().MatchProcess();

            for (int i = 0;i < selectcards.Length;i++)
            {
                selectcards[i] = null;
            }
            selectnumber = 0;

            return true;
        }
        else
        {
            Debug.Log("カードが一致しませんでした");
            return false;
        }
    }

    public void CardReset()
    {
        for(int i = 0;  i< selectcards.Length; i++)
        {
            selectcards[i].GetComponent<Card>().Hide();
            selectcards[i] = null;
        }

        selectnumber = 0;
    }
}
