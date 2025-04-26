using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public void LoadGame()
    {
        Debug.Log("Loading Game");
        SceneManager.LoadScene("TestScene");
    }

    public void QuitGame(){
        Debug.Log("Quitting Game...");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
