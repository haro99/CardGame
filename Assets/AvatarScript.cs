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
        this.punTurnManager = this.gameObject.AddComponent<PunTurnManager>();//PunTurnManager���R���|�[�l���g�ɒǉ�
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
                    //���C�̐���
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
                            Debug.Log("�^�b�`�����J�[�h�ł�");
                        }
                    }
                }
            }

            if (!PhotonNetwork.IsMasterClient && (currentturn % 2) == 0)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Player2 Click!");
                    //���C�̐���
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
                            Debug.Log("�^�b�`�����J�[�h�ł�");
                        }
                    }
                }
            }
        }
    }

    public void OnClick()
    {
        // �摜�Ȃǂ̓������邽�߂ɑ��v���C���[�ɑΏۂ̊֐����Ăяo���悤�ɂ���
        //photonView.RPC(", RpcTarget.All);
    }

    void SetupTurnManager()
    {
        punTurnManager = GetComponent<PunTurnManager>();
        punTurnManager.enabled = true;
        punTurnManager.TurnManagerListener = this;
    }


    /// <summary>
    /// �K��l�����������Ƃ��ɌĂяo����鏈��
    /// </summary>
    /// <param name="newPlayer"></param>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);

        if (PhotonNetwork.CurrentRoom.PlayerCount >= 1)
        {
            Debug.Log("�v���C���[�������܂����I�Q�[�����J�n���܂�");
            //GameObject.Find("MatchingText").SetActive(false);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("�}�X�^�[�N���C�A���g�ł�");
                
                GameManager.Set();
                punTurnManager.BeginTurn();
            }
            else
            {
                Debug.Log("�}�X�^�[�N���C�A���g�ł͂���܂���");
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
        Debug.Log("�^�[�����J�n���܂�" + turn);
        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.IsMasterClient && (currentturn % 2) == 1)
            {
                Debug.Log("�N���C�A���g�}�X�^�[�ł�");
                CurrentText.text = "���Ȃ��̃^�[���ł�";
            }
            else
            {
                Debug.Log("�N���C�A���g�}�X�^�[�ł͂���܂���");
                CurrentText.text = "�����Ẵ^�[���ł�";
            }
        }
        else
        {
            if(currentturn % 2 == 0)
            {
                CurrentText.text = "���Ȃ��̃^�[���ł�";
            }
            else
            {
                CurrentText.text = "�����Ẵ^�[���ł�";
            }
        }

    }

    /// <summary>
    /// ��Ԃ��I������Ƃ��ɌĂ΂��
    /// </summary>
    /// <param name="player"></param>
    /// <param name="turn"></param>
    /// <param name="move"></param>
    void IPunTurnManagerCallbacks.OnPlayerFinished(Photon.Realtime.Player player, int turn, object move)
    {

    }

    public void OnClickButton()//�{�^�����N���b�N�Ŏ����̃^�[���I��
    {
        Debug.Log("�{�^���N���b�N");
        this.punTurnManager.SendMove(null, true);
    }

    /// <summary>
    /// �S�v���C���[�̎�Ԃ��I�������A�Ă΂��֐��ł��B
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
        // �������Ԃ��Ȃ��̂ł�����͎g��Ȃ�
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
        Debug.Log("����̃^�[���ł�");
        if (!PhotonNetwork.IsMasterClient)
        {
            isplay = true;
        }
    }

}
