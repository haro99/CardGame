using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CpuController : MonoBehaviour
{
    public GameManagerTest GameManagerTest;
    public Status status;
    public float count;
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
        if (Status.Play == status)
        {
            count += Time.deltaTime;

            if(count>3f)
            {
                int max = GameManagerTest.SetCards.Count;
                int takenumber = Random.Range(0, max);
                GameManagerTest.CpuCardTake(takenumber);
                count = 0;
            }
        }
    }
}
