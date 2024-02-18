using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class AvatarScript : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks
{
    PunTurnManager punTurnManager = default;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    void Start()
    {
        this.punTurnManager = this.gameObject.AddComponent<PunTurnManager>();//PunTurnManagerをコンポーネントに追加

        Button button = GameObject.Find("NextTurn").GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
        SetupTurnManager();
    }

    public void OnClick()
    {
        // 画像などの同期するために他プレイヤーに対象の関数を呼び出すようにする
        //photonView.RPC(", RpcTarget.All);
    }

    void SetupTurnManager()
    {
        punTurnManager = GetComponent<PunTurnManager>();
        punTurnManager.enabled = true;
        punTurnManager.TurnManagerListener = this;
    }


    /// <summary>
    /// 規定人数が揃ったときに呼び出される処理
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            Debug.Log("プレイヤーが揃いました！ゲームを開始します");

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("マスタークライアントです");
                SampleController controller = GameObject.Find("GameObject").GetComponent<SampleController>();

                controller.Set();
                punTurnManager.BeginTurn();
            }
            else
            {
                Debug.Log("マスタークライアントではありません");
            }
        }
    }

    public void OnTurnBegins(int turn)
    {
        Debug.Log("ターンを開始します" + turn);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log(punTurnManager.Turn);
        }
    }

    /// <summary>
    /// 手番が終わったときに呼ばれる
    /// </summary>
    /// <param name="player"></param>
    /// <param name="turn"></param>
    /// <param name="move"></param>
    void IPunTurnManagerCallbacks.OnPlayerFinished(Photon.Realtime.Player player, int turn, object move)
    {

    }

    public void OnClickButton()//ボタンをクリックで自分のターン終了
    {
        Debug.Log("ボタンクリック");
        this.punTurnManager.SendMove(null, true);
    }

    /// <summary>
    /// 全プレイヤーの手番が終わったら、呼ばれる関数です。
    /// </summary>
    /// <param name="turn"></param>
    public void OnTurnCompleted(int turn)
    {
        punTurnManager.BeginTurn();
    }

    public void OnPlayerMove(Player player, int turn, object move)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerFinished(Player player, int turn, object move)
    {
        throw new System.NotImplementedException();
    }

    public void OnTurnTimeEnds(int turn)
    {
        throw new System.NotImplementedException();
    }

    [PunRPC]
    public void OnMyTurnEnd(int turn)
    {
        Debug.Log("相手のターンです");
    }
}
