using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Card : MonoBehaviour
{
    [SerializeField]
    public GameObject StarEffect;
    [SerializeField]
    public Sprite CloseImage, OpenImage;
    public AudioSource SE;
    public BoxCollider2D Colloder;
    public int number;

    public void SetCardnumber(int number, Sprite OpenImage)
    {
        this.number = number;
        this.OpenImage = OpenImage;

    }

    public void Touch()
    {
        //SE.Play();
        gameObject.GetComponent<SpriteRenderer>().sprite = OpenImage;
    }

    public void Hide()
    {
        SE.Play();
        gameObject.GetComponent<SpriteRenderer>().sprite = CloseImage;

    }

    public void MatchProcess()
    {
        //コライダーを消して数が合わない事故をさせないようにする
        Destroy(gameObject.GetComponent<Collider2D>());
        //アニメーションか何か

        //エフェクト演出を出す
        Instantiate(StarEffect, transform.position + new Vector3(0f, 0f, -1f), Quaternion.identity);
    }
}
