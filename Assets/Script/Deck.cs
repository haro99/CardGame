using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct CardData {
    public Sprite cardimage;
    public int number;
}
public class Deck : MonoBehaviour
{
    public OpenCard OpenCard;
    public Discard Discard;
    public Stack<CardData> CardDatas = new Stack<CardData>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DeckAdd(List<GameObject> Cards)
    {
        foreach(var Card in Cards)
        {
            Sprite image = Card.GetComponent<Cards>().ImageNumber;
            int number = Card.GetComponent<Cards>().cardnumber;
            Debug.Log(number);
            CardData data = new CardData { cardimage = image, number = number };
            CardDatas.Push(data);
        }

        Debug.Log(CardDatas.Count);
    }

    public CardData Add()
    {
        return CardDatas.Pop();
        if (CardDatas.Count == 0)
            gameObject.SetActive(false);
    }
    private void OnMouseDown()
    {
        OpenCard.Add(CardDatas.Pop());
        if (CardDatas.Count == 0)
            gameObject.SetActive(false);
    }
}
