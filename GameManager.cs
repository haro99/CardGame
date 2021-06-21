using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    public GameObject Card, StartSetPos;    //カードのオブジェクト、配置するスタート位置のオブジェクト
    [SerializeField]
    public Sprite[] CardImage;              //カードの数の絵柄
    [SerializeField]
    public float offsetx, offsety;          //カードを配置綺麗にはいつする時のカード間の距離
    [SerializeField]
    public List<GameObject> SetCards;       //シャッフルする時に生成したカードを保存するリスト
    [SerializeField]
    public GameObject[] Choices;            //選んだ2枚のカード
    
    private int choiceCount, Total;          //選んだ枚数（2枚）、一致してないカードのトータル
    private bool isJudge, play;              //カードの判定中のフラグ、タイマースタートするフラグ
    public AudioSource audioSource;         //一致した時のSE
    [SerializeField]
    public GameTime GameTime;               //時間カウントクラス


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
    }

    // Update is called once per frame
    void Update()
    {
        if (!isJudge)
        {
            if (choiceCount < 2 && Input.GetMouseButtonDown(0))
            {
                //タイムスタート
                if (!play)
                {
                    GameTime.GameStart();
                    play = true;
                }

                //レイの生成
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, ray.direction, 12f);

                if (hit.collider)
                {
                    //Debug.Log(hit.collider.gameObject.name);
                    GameObject HitObject = hit.collider.gameObject;


                    if (Choices[0] && Choices[0] == HitObject)
                    {
                        Choices[0].GetComponent<Card>().Hide();
                        Choices[0] = null;
                        choiceCount--;
                    }
                    else
                    {
                        HitObject.gameObject.GetComponent<Card>().Touch();
                        Choices[choiceCount] = HitObject;
                        choiceCount++;
                    }
                }
            }

            if (choiceCount == 2)
            {
                StartCoroutine("Judge");
                isJudge = true;
            }
        }
    }

    //選択したカードが同じか処理
    IEnumerator Judge()
    {
        //数字をちょっと見せる
        yield return new WaitForSeconds(1);

        if (Choices[0].GetComponent<Card>().number == Choices[1].GetComponent<Card>().number)
        {
            //Debug.Log("揃いました");

            for (int i = 0; i < choiceCount; i++)
            {
                //Destroy(Choices[i]);
                Choices[i].GetComponent<Card>().MatchProcess();
                Choices[i] = null;
                audioSource.Play();
            }
            Total -= 2;
        }
        else
        {
            //Debug.Log("揃ってません");

            for (int i = 0; i < choiceCount; i++)
            {
                Choices[i].GetComponent<Card>().Hide();
                Choices[i] = null;
            }
        }
        choiceCount = 0;
        isJudge = false;

        if (Total == 0)
        {
            //Debug.Log("ゲームクリア！");
            GameTime.GameClear();
        }
    }

    //ゲームを再度読み込む
    public void Restart()
    {
        SceneManager.LoadScene(0);
    }
}
