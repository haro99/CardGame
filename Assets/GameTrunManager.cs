using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameTrunManager :MonoBehaviour, IPunTurnManagerCallbacks
{
    private PunTurnManager turnManager;

    public void OnPlayerFinished(Player player, int turn, object move)
    {
        throw new System.NotImplementedException();
    }

    public void OnPlayerMove(Player player, int turn, object move)
    {
        throw new System.NotImplementedException();
    }

    public void OnTurnBegins(int turn)
    {
        Debug.Log("ターンを開始します" + turn);
    }

    public void OnTurnCompleted(int turn)
    {
        throw new System.NotImplementedException();
    }

    public void OnTurnTimeEnds(int turn)
    {

    }

    public void OnClickButton()//ボタンをクリックで自分のターン終了
    {
        Debug.Log("ボタンクリック");
        this.turnManager.SendMove(1, true);
    }
}
