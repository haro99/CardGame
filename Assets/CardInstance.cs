using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInstance : MonoBehaviour
{
    public GameObject Card, Position;
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 5; i++)
        {
            GameObject InstaCard = Instantiate(Card, transform);
            InstaCard.GetComponent<CardMovement>().SetPosition(Position.transform.position + new Vector3(i, 0));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
