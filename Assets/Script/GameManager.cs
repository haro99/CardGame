using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// CPUのジャッジが解除されてない
/// </summary>
public enum Turn {
    Ready,
    play,
    GameEnd
}
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Card, StartSetPos;    //カードのオブジェクト、配置するスタート位置のオブジェクト
    [SerializeField]
    private Sprite[] CardImage;              //カードの数の絵柄
    [SerializeField]
    private float offsetx, offsety;          //カードを配置綺麗にはいつする時のカード間の距離
    [SerializeField]
    private List<GameObject> SetCards;       //シャッフルする時に生成したカードを保存するリスト
    public List<GameObject> CpuCardRecord;
    [SerializeField]
    private GameObject[] Select, CpuSelect;            //選んだ2枚のカード
    
    public int choiceCount, Total, recrdnumber1, recrdnumber2, player, cpu;          //選んだ枚数（2枚）、一致してないカードのトータル, CPUレコードから取り出す位置のナンバー
    [SerializeField]
    private bool isplyaerjudge, iscpujudge;              //カードの判定中のフラグ、タイマースタートするフラグ
    public AudioSource audioSource;         //一致した時のSE
    public AudioClip[] audios;
    [SerializeField]
    public Turn nowturn;
    public GameObject ResultMenu, Sign;
    public Text PlyaerCardnumber, CpuCardnumber, winorlose;

    // Start is called before the first frame update
    void Start()
    {
        //配置
        for (int i = 0; i < CardImage.Length; i++)
        {
            int y = i / 13;
            int x = i % 13;

            GameObject card = Instantiate(Card, StartSetPos.transform.position + new Vector3(offsetx * x, offsety * y, 0f), Quaternion.identity);
            SetCards.Add(card);
            card.GetComponent<Card>().SetCardnumber(x + 1, CardImage[i]);
        }

        //カードののシャッフル
        for (int i = 0; i < 100; i++)
        {
            int number1 = Random.Range(0, SetCards.Count), number2 = Random.Range(0, SetCards.Count);

            while (number1 == number2)
            {
                number2 = Random.Range(0, SetCards.Count);
            }
            Vector2 Pos = SetCards[number1].transform.position;
            SetCards[number1].transform.position = SetCards[number2].transform.position;
            SetCards[number2].transform.position = Pos;
        }
        Total = SetCards.Count;

        StartCoroutine("StartSign");
    }

    // Update is called once per frame
    void Update()
    {
        switch (nowturn)
        {
                case Turn.play:
                if (!isplyaerjudge)
                    PlayerTurn();
                if (!iscpujudge)
                    CpuTurn();
                if (Total < 3)
                {
                    Ending();
                }
                break;
        }
    }

    //選択したカードが同じか処理
    IEnumerator PlayerJudge(GameObject[] Choicses)
    {
        //数字をちょっと見せる
        yield return new WaitForSeconds(1);

        if (Choicses[0].GetComponent<Card>().number == Choicses[1].GetComponent<Card>().number)
        {
            //Debug.Log("揃いました");

            for (int i = 0; i < Choicses.Length; i++)
            {
                //Destroy(Choices[i]);
                Choicses[i].GetComponent<Card>().MatchProcess();
                SetCards.Remove(Choicses[i]);
                audioSource.PlayOneShot(audios[0]);
                CpuCardRecord.Remove(Choicses[i]);
            }
            Total -= 2;
            player+=2;
            PlyaerCardnumber.text = player.ToString();
        }
        else
        {
            Debug.Log("揃ってません");

            for (int i = 0; i < Choicses.Length; i++)
            {
                Choicses[i].GetComponent<Card>().Hide();
                Choicses[i] = null;
            }
        }
        choiceCount = 0;
        Select[0] = null;
        Select[1] = null;
        isplyaerjudge = false;
    }

    IEnumerator CpuJudge(GameObject[] Choicses)
    {
        //数字をちょっと見せる
        yield return new WaitForSeconds(1);

        if (Choicses[0].GetComponent<Card>().number == Choicses[1].GetComponent<Card>().number)
        {
            //Debug.Log("揃いました");

            for (int i = 0; i < Choicses.Length; i++)
            {
                //Destroy(Choices[i]);
                Choicses[i].GetComponent<Card>().MatchProcess();
                SetCards.Remove(Choicses[i]);
                audioSource.PlayOneShot(audios[0]);
                CpuCardRecord.Remove(Choicses[i]);
            }
            Total -= 2;
            cpu+=2;
            CpuCardnumber.text = cpu.ToString();
        }
        else
        {
            Debug.Log("揃ってません");

            for (int i = 0; i < Choicses.Length; i++)
            {
                Choicses[i].GetComponent<Card>().Hide();
                Choicses[i] = null;
            }
        }

        iscpujudge = false;
    }

    //ゲームを再度読み込む
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }

    /// <summary>
    /// プレイヤー
    /// </summary>
    public void PlayerTurn()
    {
        if (SetCards.Count == 0)
            nowturn = Turn.GameEnd;


            if (choiceCount < 2 && Input.GetMouseButtonDown(0))
            {
                //レイの生成
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, ray.direction, 12f);

                if (hit.collider)
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    GameObject HitObject = hit.collider.gameObject;

                    if (Select[0] != HitObject && !Check(CpuSelect, HitObject))
                    {
                        HitObject.gameObject.GetComponent<Card>().Touch();
                        Select[choiceCount] = HitObject;
                        choiceCount++;
                    }
                 }
            }

            if (choiceCount == 2)
            {
                StartCoroutine(PlayerJudge(Select));
                isplyaerjudge = true;
            }
    }
    /// <summary>
    /// CPU
    /// </summary>
    public void CpuTurn()
    {
        if (SetCards.Count == 0)
            nowturn = Turn.GameEnd;

        if (RecordCheck())
        {
            StartCoroutine(CpuCardOpen());
        }
        else
        {
            
            int number1 = Random.Range(0, SetCards.Count);
            int number2 = Random.Range(0, SetCards.Count);

            while (number1 == number2)
            {
                number2 = Random.Range(0, SetCards.Count);
            }
            
            StartCoroutine( CpuRandomOpen(number1, number2));

            if (!CpuCardRecord.Contains(SetCards[number1]))
                CpuCardRecord.Add(SetCards[number1]);
            if (!CpuCardRecord.Contains(SetCards[number2]))
                CpuCardRecord.Add(SetCards[number2]);

        }
        iscpujudge = true;

    }

    public bool RecordCheck()
    {
        for (int i = 0; i < CpuCardRecord.Count - 1; i++)
        {
            int number = CpuCardRecord[i].GetComponent<Card>().number;
            for (int j = i + 1; j < CpuCardRecord.Count; j++)
            {
                int number2 = CpuCardRecord[j].GetComponent<Card>().number;
                if (number == number2)
                {
                    recrdnumber1 = i;
                    recrdnumber2 = j;
                    Debug.Log("一致するのがありました");
                    return true;
                }
            }
        }
        Debug.Log("一致するのがありませんでした");
        return false;
    }

    IEnumerator CpuCardOpen()
    {
        CpuSelect[0] = CpuCardRecord[recrdnumber1];
        CpuSelect[1] = CpuCardRecord[recrdnumber2];
        if (CpuCheck())
        {
            CpuSelect[0].GetComponent<Card>().Touch();
            yield return new WaitForSeconds(1);
            CpuSelect[1].GetComponent<Card>().Touch();
            yield return new WaitForSeconds(1);
            StartCoroutine(CpuJudge(CpuSelect));
            iscpujudge = true;
        }
        else 
        {
            CpuCardRecord.Remove(CpuSelect[0]);
            CpuSelect[0] = null;
            CpuCardRecord.Remove(CpuSelect[1]);
            CpuSelect[1] = null;
            yield return new WaitForSeconds(1);
            iscpujudge = false;
        }
    }
    IEnumerator CpuRandomOpen(int number1, int number2)
    {
        CpuSelect[0] = SetCards[number1];
        CpuSelect[1] = SetCards[number2];

        if (CpuSelect[0] != Select[0] && CpuSelect[0] != Select[1] && CpuSelect[1] != Select[0] && CpuSelect[1] != Select[1])
        {
            CpuSelect[0].gameObject.GetComponent<Card>().Touch();
            yield return new WaitForSeconds(1);
            CpuSelect[1].gameObject.GetComponent<Card>().Touch();
            yield return new WaitForSeconds(1);
            StartCoroutine(CpuJudge(CpuSelect));
        }
        else
        {
            CpuCardRecord.Remove(CpuSelect[0]);
            CpuSelect[0] = null;
            CpuCardRecord.Remove(CpuSelect[1]);
            CpuSelect[1] = null;
            yield return new WaitForSeconds(1);
            iscpujudge = false;
        }
    }

    bool Check(GameObject[] Takes, GameObject choice)
    {
        for (int i = 0; i < Takes.Length; i++)
        {
            if (Takes[i] == choice)
                return true;
        }
        return false;
    }

    bool CpuCheck()
    {
        for (int i = 0; i < CpuSelect.Length; i++)
        {
            for (int j = 0; j < Select.Length; j++)
            {
                if (CpuSelect[i] == Select[j])
                    return false;
            }
        }
        return true;
    }

    IEnumerator StartSign()
    {
        Text text = Sign.GetComponent<Text>();



        text.text = "よーい！";
        yield return new WaitForSeconds(2);

        text.text = "スタート！";
        audioSource.PlayOneShot(audios[1]);
        yield return new WaitForSeconds(1);
        Sign.SetActive(false);
        nowturn = Turn.play;
    }

    private void Ending()
    {
        if (player > cpu)
        {
            winorlose.text = "WIN!";
        }
        else if (player < cpu)
        {
            winorlose.text = "LOSE...";
        }
        else 
        {
            winorlose.text = "DRAW";
        }

        nowturn = Turn.GameEnd;
        StopAllCoroutines();
        ResultMenu.SetActive(true);
    }
}
