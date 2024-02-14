using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class AvatarScript : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks
{
    PunTurnManager punTurnManager = default;

    public MeshRenderer Sprite;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    void Start()
    {
        this.punTurnManager = this.gameObject.AddComponent<PunTurnManager>();//PunTurnManagerをコンポーネントに追加

        SetupTurnManager();
    }

    public void OnClick()
    {
        // 画像などの同期するために他プレイヤーに対象の関数を呼び出すようにする
        photonView.RPC("ChangeSprite", RpcTarget.All);
    }

    [PunRPC]
    public void ChangeSprite()
    {
        Sprite.enabled = false;
    }

    void SetupTurnManager()
    {
        punTurnManager = GetComponent<PunTurnManager>();
        punTurnManager.enabled = true;
        punTurnManager.TurnManagerListener = this;
    }



    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 2)
        {
            Debug.Log("プレイヤーが揃いました！ゲームを開始します");

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("マスタークライアントです");
                SampleController controller = GameObject.Find("GameObject").GetComponent<SampleController>();

                controller.Set();
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
        throw new System.NotImplementedException();
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

    /// <summary>
    /// 次のターンにする
    /// </summary>
    public void NextTurn()
    {
        punTurnManager.BeginTurn();
    }

    public void OnTurnCompleted(int turn)
    {        
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
}
