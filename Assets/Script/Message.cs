using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    public Text text;
    const int messagecount = 0;
    // Start is called before the first frame update
    string[] message = { 
        "初めまして、私は百合です\nあなたは誰ですか？是非お名前を教えていただけるとうれしいです。",
        "いいお名前ですね\nなんて素敵な名前なんでしょうか、好きになってしまいそうです！"
    };
    void Start()
    {
        StartCoroutine("MessageDisplay");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            StopCoroutine("MessageDisplay");
            text.text = message[messagecount];
        }
    }
    IEnumerator MessageDisplay()
    {
        int maxnumber = message[messagecount].Length;

        for (int i = 1; i < maxnumber; i++)
        {
            text.text = message[messagecount].Substring(0, i);
            yield return new WaitForSeconds(0.1f);
        }
    }
}
