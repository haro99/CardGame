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
        this.punTurnManager = this.gameObject.AddComponent<PunTurnManager>();//PunTurnManager���R���|�[�l���g�ɒǉ�

        SetupTurnManager();
    }

    public void OnClick()
    {
        // �摜�Ȃǂ̓������邽�߂ɑ��v���C���[�ɑΏۂ̊֐����Ăяo���悤�ɂ���
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
            Debug.Log("�v���C���[�������܂����I�Q�[�����J�n���܂�");

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.Log("�}�X�^�[�N���C�A���g�ł�");
                SampleController controller = GameObject.Find("GameObject").GetComponent<SampleController>();

                controller.Set();
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
        throw new System.NotImplementedException();
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

    /// <summary>
    /// ���̃^�[���ɂ���
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
