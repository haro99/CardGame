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
        Debug.Log("�}�b�`���O��");
        turnend = false;
        PhotonNetwork.NickName = "Player";
        // PhotonServerSettings�̐ݒ���e���g���ă}�X�^�[�T�[�o�[�֐ڑ�����
        PhotonNetwork.ConnectUsingSettings();
    }

    // �}�X�^�[�T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnConnectedToMaster()
    {
        // "Room"�Ƃ������O�̃��[���ɎQ������i���[�������݂��Ȃ���΍쐬���ĎQ������j
        PhotonNetwork.JoinRandomRoom();
    }

    // �����_���ŎQ���ł��郋�[�������݂��Ȃ��Ȃ�A�V�K�Ń��[�����쐬����
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // ���[���̎Q���l����2�l�ɐݒ肷��
        var roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;

        PhotonNetwork.CreateRoom(null, roomOptions);
    }

    // �Q�[���T�[�o�[�ւ̐ڑ��������������ɌĂ΂��R�[���o�b�N
    public override void OnJoinedRoom()
    {
        Debug.Log("Player�����O�C�����܂���");
        Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        // �����_���ȍ��W�Ɏ��g�̃A�o�^�[�i�l�b�g���[�N�I�u�W�F�N�g�j�𐶐�����
        var position = new Vector3(Random.Range(-3f, 3f), Random.Range(-3f, 3f));
        // ���łɔz�u����Ă��琶�����Ȃ�
        if (Player == null)
        {
            GameObject Obj = PhotonNetwork.Instantiate("Avatar", position, Quaternion.identity);
            Player = Obj.GetComponent<AvatarScript>();
        }
    }
    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log(otherPlayer + "���ޏo���܂����B�Q�[���I���������܂�");
    }

    /// <summary>
    /// �J�[�h��z�u����
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
    /// �z�u���ꂽ�J�[�h���V���b�t������
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
        // �S���ɓ`����
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
            Debug.Log("�I�������J�[�h�̔���");

            if (!Judge())
            {
                turnend = false;
                // ����̃^�[���ɂ���
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
            Debug.Log("�����J�[�h�������܂����I");
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
            Debug.Log("�J�[�h����v���܂���ł���");
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
