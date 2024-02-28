using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;

public class AvatarScript : MonoBehaviourPunCallbacks, IPunTurnManagerCallbacks
{
    private PunTurnManager punTurnManager = default;
    private SampleController GameManager;
    [SerializeField]
    private bool isplay;
    public int currentturn;
    public Text CurrentText;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    void Start()
    {
        isplay = false;
        CurrentText = GameObject.Find("CurrentTurn").GetComponent<Text>();
        this.punTurnManager = this.gameObject.AddComponent<PunTurnManager>();//PunTurnManagerをコンポーネントに追加
        GameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SampleController>();
        Button button = GameObject.Find("NextTurn").GetComponent<Button>();
        button.onClick.AddListener(() => OnTurnCompleted(0));
        SetupTurnManager();

        Debug.Log(PhotonNetwork.LocalPlayer.UserId);
    }

    private void Update()
    {
        if (GameManager.turnend)
        {


            if (PhotonNetwork.IsMasterClient && (currentturn % 2) == 1)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Player1 Click!");
                    //レイの生成
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, ray.direction, 12f);

                    if (hit.collider)
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        GameObject HitObject = hit.collider.gameObject;
                        if (!GameManager.CardCheck(HitObject))
                        {
                            HitObject.GetComponent<Card>().Touch();
                            GameManager.CardSerach(HitObject);
                        }
                        else
                        {
                            Debug.Log("タッチしたカードです");
                        }
                    }
                }
            }

            if (!PhotonNetwork.IsMasterClient && (currentturn % 2) == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Player2 Click!");
                    //レイの生成
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                    RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, ray.direction, 12f);

                    if (hit.collider)
                    {
                        Debug.Log(hit.collider.gameObject.name);
                        GameObject HitObject = hit.collider.gameObject;
                        if (!GameManager.CardCheck(HitObject))
                        {
                            HitObject.GetComponent<Card>().Touch();
                            GameManager.CardSerach(HitObject);
                        }
                        else
                        {
                            Debug.Log("タッチしたカードです");
                        }
                    }
                }
            }
        }
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
            //GameObject.Find("MatchingText").SetActive(false);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("マスタークライアントです");
                
                GameManager.Set();
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
        GameObject obj = GameObject.Find("MatchingText");
        if(obj)
        {
            obj.SetActive(false);
        }

        currentturn = turn;
        GameManager.turnend = true;
        Debug.Log("ターンを開始します" + turn);
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.IsMasterClient && (currentturn % 2) == 1)
            {
                Debug.Log("クライアントマスターです");
                CurrentText.text = "あなたのターンです";
            }
            else
            {
                Debug.Log("クライアントマスターではありません");
                CurrentText.text = "あいてのターンです";
            }
        }
        else
        {
            if(currentturn % 2 == 0)
            {
                CurrentText.text = "あなたのターンです";
            }
            else
            {
                CurrentText.text = "あいてのターンです";
            }
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
        // 制限時間がないのでこちらは使わない
    }

    public void TurnChange()
    {
        isplay =false;
        photonView.RPC(nameof(OnMyTurnEnd), RpcTarget.Others);
    }

    public void TurnEnd()
    {
        isplay = false;
    }

    [PunRPC]
    public void OnMyTurnEnd()
    {
        //GameManager.CardReset();
        Debug.Log("相手のターンです");
        if (!PhotonNetwork.IsMasterClient)
        {
            isplay = true;
        }
    }

}
