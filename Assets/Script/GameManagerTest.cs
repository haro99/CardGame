using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Status
{
    Wait,
    Play,
    End
}
public class GameManagerTest : MonoBehaviour
{
    public GameObject Card, StartSetPos;    //カードのオブジェクト、配置するスタート位置のオブジェクト
    public Sprite[] CardImage;              //カードの数の絵柄
    public List<GameObject> SetCards;       //シャッフルする時に生成したカードを保存するリスト
    public float offsetx, offsety;          //カードを配置綺麗にはいつする時のカード間の距離
    public List<GameObject> PlayerChoiceCards;
    public PlayerController PlayerController;
    public List<GameObject> CpuChoiceCards;
    public CpuController CpuController;
    public int playergetcard, cpugetcard;

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
        PlayerController.Statusset = Status.Play;
        CpuController.Statusset = Status.Play;
    }

    // Update is called once per frame
    void Update()
    {
        if (SetCards.Count < 4)
        {
            PlayerController.Statusset = Status.End;
            CpuController.Statusset = Status.End;
        }
    }

    public void PlayerCardTake(GameObject takeCard)
    {
        if (!PlayerChoiceCards.Contains(takeCard)&& !CpuChoiceCards.Contains(takeCard))
        {
            takeCard.GetComponent<Card>().Touch();
            PlayerChoiceCards.Add(takeCard);
        }

        if (PlayerChoiceCards.Count == 2)
        {
            PlayerController.Statusset = Status.Wait;
            StartCoroutine( PlayerMatchCheck(PlayerChoiceCards, PlayerController));
        }
    }
    public void CpuCardTake(int number)
    {
        GameObject takeCard = SetCards[number];
        if (!CpuChoiceCards.Contains(takeCard) && !PlayerChoiceCards.Contains(takeCard))
        {
            takeCard.GetComponent<Card>().Touch();
            CpuChoiceCards.Add(takeCard);
        }

        if (CpuChoiceCards.Count == 2)
        {
            CpuController.Statusset = Status.Wait;
            CpuMatchCheck(CpuChoiceCards, CpuController);
            CpuChoiceCards.Clear();
        }
    }

    IEnumerator PlayerMatchCheck(List<GameObject> Choicelist, PlayerController player)
    {
        int cardnumber1 = Choicelist[0].GetComponent<Card>().number;
        int cardnumber2 = Choicelist[1].GetComponent<Card>().number;

        yield return new WaitForSeconds(1);

        if (cardnumber1 == cardnumber2)
        {
            Debug.Log("揃ったよ");
            Choicelist[0].GetComponent<Card>().MatchProcess();
            Choicelist[1].GetComponent<Card>().MatchProcess();
            SetCards.Remove(Choicelist[0]);
            SetCards.Remove(Choicelist[1]);
            playergetcard += 2;
        }
        else
        {
            Choicelist[0].GetComponent<Card>().Hide();
            Choicelist[1].GetComponent<Card>().Hide();
        }


        PlayerChoiceCards.Clear();

        player.Statusset = Status.Play;
    }

    private void CpuMatchCheck(List<GameObject> Choicelist, CpuController Cpu)
    {
        int cardnumber1 = Choicelist[0].GetComponent<Card>().number;
        int cardnumber2 = Choicelist[1].GetComponent<Card>().number;

        //yield return new WaitForSeconds(1);

        if (cardnumber1 == cardnumber2)
        {
            Debug.Log("揃ったよ");
            Choicelist[0].GetComponent<Card>().MatchProcess();
            Choicelist[1].GetComponent<Card>().MatchProcess();
            SetCards.Remove(Choicelist[0]);
            SetCards.Remove(Choicelist[1]);
            cpugetcard += 2;
        }

        CpuChoiceCards.Clear();
        Cpu.Statusset = Status.Play;
    }
}
