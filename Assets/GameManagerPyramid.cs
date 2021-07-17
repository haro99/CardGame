using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerPyramid : MonoBehaviour
{
    public List<GameObject> CardList;
    public List<GameObject> SetCards;
    public GameObject ChoiceObj;
    public int total13;
    public Deck Deck;
    public int[,] classsize = {
        {0,0},
        {1, 2},
        {4, 6},
        {7, 10},
        {11, 14},
        {15, 21},
    };
    [SerializeField]
    int[] classcount;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 100; i++)
        {
            int number1 = Random.Range(0, CardList.Count), number2 = Random.Range(0, CardList.Count);

            while (number1 == number2)
            {
                number2 = Random.Range(0, CardList.Count);
            }
            GameObject obj = CardList[number1];
            CardList[number1] = CardList[number2];
            CardList[number2] = obj;
        }
        int cardsetnumber = 0;
        for (int i = 0; i < 7; i++)
        {
            for (int j = 0; j < i + 1; j++)
            {
                GameObject obj = CardList[0];
                CardList.RemoveAt(0);
                //段々に配置
                GameObject Card = Instantiate(obj, new Vector2(-i, 4) + new Vector2(j * 2, -i), Quaternion.identity);
                Vector3 vector = Card.transform.position;
                //下のカードが手前に見えるように
                Card.transform.position = new Vector3(vector.x, vector.y, -i);
                Card.GetComponent<Cards>().SetNumber(i, cardsetnumber);
                SetCards.Add(Card);
                cardsetnumber++;
            }
        }
        Deck.DeckAdd(CardList);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// カードがタッチされた時に呼び出す
    /// </summary>
    /// <param name="number"></param>
    /// <param name="CardObj"></param>
    public void Touch(int number, GameObject CardObj,bool pyramidcard)
    {
        GameObject TouchCard = CardObj;
        if (!ChoiceObj)
        {
            ChoiceObj = TouchCard;
            total13 = number;
            TouchCard = null;
        }
        else
        {
            total13 += number;
        }

        int[] carddata;
        //合計が13だった場合
        if (total13 == 13)
        {
            if (ChoiceObj.tag == "Untagged")
            {
                carddata = ChoiceObj.GetComponent<Cards>().GetCardNumber();
                OpenCardAdd(carddata[0], carddata[1]);
                SetCards.Remove(ChoiceObj);
                Destroy(ChoiceObj);
            }
            else if (ChoiceObj.tag == "OpenCard")
            {
                ChoiceObj.GetComponent<OpenCard>().None();
            }
            else if (ChoiceObj.tag == "Discard")
            {
                ChoiceObj.GetComponent<Discard>().Use();
            }

            if (TouchCard != null && TouchCard.tag == "Untagged")
            {
                carddata = TouchCard.GetComponent<Cards>().GetCardNumber();
                OpenCardAdd(carddata[0], carddata[1]);
                SetCards.Remove(TouchCard);
                Destroy(TouchCard);
            }
            else if (TouchCard != null && TouchCard.tag == "OpenCard")
            {
                TouchCard.GetComponent<OpenCard>().None();
            }
            else if (TouchCard.tag == "Discard")
            {
                ChoiceObj.GetComponent<Discard>().Use();
            }

            total13 = 0;
        }
        //else
        //{
        //    ChoiceObj = TouchCard;
        //    total13 = number;
        //}
    }

    /// <summary>
    /// ピラミッドに置かれたカードの階層の上のカードをオープンする時の処理
    /// </summary>
    /// <param name="setnumber"></param>
    /// <param name="classnumber"></param>
    public void OpenCardAdd(int setnumber, int classnumber)
    {
        if (classnumber >= 0)
        {
            int number = setnumber - classcount[classnumber - 1];
            Debug.Log(setnumber + " " + classnumber + " " + number);
            if (classsize[classnumber-1, 0] <= number)
            {
                SetCards[number].GetComponent<Cards>().Add();
            }
            if (classsize[classnumber - 1, 1] >= number + 1)
            {
                SetCards[number+1].GetComponent<Cards>().Add();

            }
        }
    }
}
