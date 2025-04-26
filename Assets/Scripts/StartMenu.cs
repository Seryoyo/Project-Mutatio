using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{

    public void LoadGame()
    {
        Debug.Log("Loading Game");
        SceneManager.LoadScene("Game");
    }

    public void QuitGame(){
        Debug.Log("Quitting Game...");
        Application.Quit();
    }


/*#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        Application.Quit();

    }*/

}
