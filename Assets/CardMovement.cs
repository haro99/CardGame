using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardMovement : MonoBehaviour
{
    public GameObject Position;
    public Vector3 offset;
    // Start is called before the first frame update
    void Start()
    {
        offset = Position.transform.position - this.transform.position;

    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(Position.transform.position, this.transform.position);

        if (distance > 0.3f)
        {
            transform.position += offset * Time.deltaTime;
        }
        else
        {
            transform.position = Position.transform.position;
        }
    }

    public void SetPosition(Vector3 movepoint)
    {
        offset = movepoint;
    }
}
