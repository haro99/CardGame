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
    private bool isplay;

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = false;
    }

    void Start()
    {
        isplay = false;
        this.punTurnManager = this.gameObject.AddComponent<PunTurnManager>();//PunTurnManager���R���|�[�l���g�ɒǉ�
        GameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<SampleController>();
        Button button = GameObject.Find("NextTurn").GetComponent<Button>();
        button.onClick.AddListener(OnClickButton);
        SetupTurnManager();
    }

    private void Update()
    {
        if(isplay)
        {
            if(Input.GetMouseButtonDown(0))
            {
                Debug.Log("Click!");
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
            GameObject.Find("MatchingText").SetActive(false);

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
        Debug.Log("�^�[�����J�n���܂�" + turn);
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("�N���C�A���g�}�X�^�[�̃^�[���ł�");
            isplay = true;
        }
        else
        {
            isplay = false;
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
        photonView.RPC(nameof(OnMyTurnEnd), RpcTarget.All);
    }

    [PunRPC]
    public void OnMyTurnEnd()
    {
        GameManager.CardReset();
        Debug.Log("����̃^�[���ł�");
        if (!PhotonNetwork.IsMasterClient)
        {
            isplay = true;
        }
    }

}
