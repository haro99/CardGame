using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenCard : MonoBehaviour
{
    public Discard Discard;
    public GameManagerPyramid GameManagerPyramid;
    public SpriteRenderer sprite;
    CardData thisccard;
    public int number;

    public void Add(CardData data)
    {
        if(thisccard.cardimage != null)
            Discard.Add(thisccard);

        thisccard = data;
        sprite.sprite = thisccard.cardimage;
        number = thisccard.number;
        gameObject.SetActive(true);
    }

    private void OnMouseDown()
    {
        GameManagerPyramid.Touch(number, this.gameObject, false);
    }

    public void None()
    {
        thisccard.cardimage = null;
        number = 0;
        gameObject.SetActive(false);
    }
}
