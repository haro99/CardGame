using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolfCard : MonoBehaviour
{
    public GolfManager GolfManager;
    public GameObject TopCard;
    public Data data;

    // Start is called before the first frame update
    void Start()
    {
        GolfManager = GameObject.Find("GameManager").GetComponent<GolfManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnMouseDown()
    {

        if (GolfManager.NumberCheck(this.data.number))
        {
            GolfManager.AAA(data);
            if(TopCard)
                TopCard.GetComponent<BoxCollider2D>().enabled = true;
            Destroy(this.gameObject);
        }
    }

    public void CardActive()
    {
        if (TopCard != null)
        {
            TopCard.GetComponent<BoxCollider2D>().enabled = true;
        }
    }
}
