using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour
{
    public GameManagerTest GameManagerTest;
    public Status status;

    public Status Statusset 
    {
        set { status = value; }
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Status.Play == status &&Input.GetMouseButtonDown(0))
        {
            //レイの生成
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast((Vector2)ray.origin, ray.direction, 12f);

            if (hit.collider)
            {
                Debug.Log(hit.collider.gameObject.name);
                GameObject HitObject = hit.collider.gameObject;
                GameManagerTest.PlayerCardTake(HitObject);
            }
        }
    }
    
}
