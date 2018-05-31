using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuController : MonoBehaviour {

    public GameObject ButtonStart;
    public GameObject ButtonEnd;

    public void On() {
        Debug.Log("Privet");
    }
    public void ExitGame() {
        Application.Quit();
    }
    public void GameLoadScreen() {
        Application.LoadLevel("Maine_Scene");
        
    }
}
