using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cards : MonoBehaviour
{
    public GameManagerPyramid GameManagerPyramid;
    public Sprite ImageNumber, closeImage;
    public int cardnumber, classnumber, setnumber, number;
    public BoxCollider2D collider2D;
    public bool open;
    // Start is called before the first frame update
    void Start()
    {
        GameManagerPyramid = GameObject.Find("GameManager").GetComponent<GameManagerPyramid>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CardOpne()
    {
        GetComponent<SpriteRenderer>().sprite = ImageNumber;
    }
    public void SetNumber(int classnumber, int setnumber)
    {
        this.classnumber = classnumber;
        this.setnumber = setnumber;

        if (classnumber >5)
        {
            GetComponent<SpriteRenderer>().sprite = ImageNumber;
            open = true;
            collider2D.enabled = true;
        }
    }

    private void OnMouseDown()
    {
        GameManagerPyramid.Touch(cardnumber, this.gameObject, true);
    }

    public int[] GetCardNumber()
    {
        int[] array = { setnumber, classnumber };
        return array;
    }

    public void Add()
    {
        number++;
        if (number > 1)
        {
            collider2D.enabled = true;
            GetComponent<SpriteRenderer>().sprite = ImageNumber;
        }       
    }
}

