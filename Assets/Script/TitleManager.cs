using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AppBtn()
    {
        Application.OpenURL("https://play.google.com/store/apps/dev?id=7769508386756843350&hl=ja");
    }

    public void SceneChange(int scenenumber)
    {
        SceneManager.LoadScene(scenenumber);
    }
}
