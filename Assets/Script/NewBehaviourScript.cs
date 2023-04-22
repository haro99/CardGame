using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject ui, campus;

    // Start is called before the first frame update
    void Start()
    {
        ui = (GameObject)Resources.Load("Text");
        //修正後
        GameObject ui_clone = Instantiate(ui, new Vector3(0, 0, 0), Quaternion.identity);

        //canvasの子に指定
        ui_clone.transform.SetParent(campus.transform, false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
