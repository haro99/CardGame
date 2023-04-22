using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct Data {
    public Sprite cardimage;
    public int number;
}
public class GolfManager : MonoBehaviour
{
    public GameObject BaseCard, TargetCard, ClearText;
    public Sprite[] Sprites;
    public List<Data> Datas;
    public Vector3 position;
    public Deck2 Deck2;
    public int targetnumber, setcardnumber;
    // Start is called before the first frame update
    void Start()
    {
        Datas = new List<Data>();

        Data data = new Data();
        for (int i = 0; i < Sprites.Length; i++)
        {
            data.number = i % 13 + 1;

            data.cardimage = Sprites[i];

            Datas.Add(data);
        }

        for (int i = 0; i < 50; i++)
        {
            int number = Random.Range(0, Datas.Count), number2 = Random.Range(0, Datas.Count);
            Data data1 = Datas[number];
            Datas[number] = Datas[number2];
            Datas[number2] = data1;
        }

        GameObject[] TopCards = new GameObject[7];
        for (int j = 0; j < 5; j++)
        {
            for (int i = 0; i < 7; i++)
            {
                Data data2 = Datas[0];
                Datas.RemoveAt(0);

                GameObject Card = Instantiate(BaseCard, position + new Vector3(i * 2, -j, -j), Quaternion.identity);
                Card.GetComponent<SpriteRenderer>().sprite = data2.cardimage;
                Card.GetComponent<GolfCard>().data = data2;
                if (j > 0)
                {
                    Card.GetComponent<GolfCard>().TopCard = TopCards[i];
                }

                TopCards[i] = Card;

                if (4 == j)
                {
                    Card.GetComponent<BoxCollider2D>().enabled = true;
                }
            }
        }
        //for (int i = 0; i < Datas.Count; i++)
        //{
        //    Debug.Log(Datas[i].number);
        //}

        Deck2.SetData(Datas);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AAA(Data data)
    {
        TargetCard.GetComponent<SpriteRenderer>().sprite = data.cardimage;
        targetnumber = data.number;

        if(setcardnumber == 0)
        {
            Debug.Log("ゲームクリア");
            ClearText.SetActive(true);
        }
    }

    public bool NumberCheck(int number)
    {
        int min = number < 2 ? number + 12 : number - 1, max = number % 13 + 1;

        if (targetnumber == min || targetnumber == max)
        {
            setcardnumber--;
            return true;
        }
        return false;
    }

    public void SceneChange(int number)
    {
        SceneManager.LoadScene(number);
    }
}
