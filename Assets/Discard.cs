using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Discard : MonoBehaviour
{
    public OpenCard openCard;
    public GameManagerPyramid GameManagerPyramid;
    Stack<CardData> Discards = new Stack<CardData>();
    public CardData NowData;
    public SpriteRenderer spriteRenderer;
    public int number;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Add(CardData data)
    {
        Discards.Push(data);
        spriteRenderer.sprite = data.cardimage;
        number = data.number;
        NowData = Discards.Pop();
        gameObject.SetActive(true);
    }
    private void OnMouseDown()
    {
        GameManagerPyramid.Touch(number, this.gameObject, false);
    }

    public void Use()
    {
        if (Discards.Count > 0)
        {
            NowData = Discards.Pop();
            spriteRenderer.sprite = NowData.cardimage;
            number = NowData.number;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
