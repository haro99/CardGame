using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deck2 : MonoBehaviour
{
    public GolfManager GolfManager;
    public List<Data> DeckDatas = new List<Data>();

    public void SetData(List<Data> setdata)
    {
        DeckDatas = setdata;

        Data data = DeckDatas[0];
        DeckDatas.RemoveAt(0);
        GolfManager.AAA(data);
    }

    public void OnMouseDown()
    {
        Data data = DeckDatas[0];
        DeckDatas.RemoveAt(0);
        GolfManager.AAA(data);

        if (DeckDatas.Count < 1)
        {
            gameObject.SetActive(false);
        }
    }
}
