using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public static PortalManager instance;
    int enemyCount; // Alive enemy components
    DoorPortal[] portals; // Portals to enable upon room clear

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    

    public void DecrementEnemyCount()
    {
        Debug.Log("Decrementing enemy count");
        --enemyCount;
        if (enemyCount == 0)
            OpenPortals();
    }

    public void OpenPortals()
    {
        foreach (var port in portals)
            port.EnableDoor();
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>().Where(e => e.hitpoint > 0).ToArray();
        portals = FindObjectsOfType<DoorPortal>();
        Debug.Log("Enemies is null? - " + (enemies == null));
        Debug.Log("Portals is null? - " + (portals == null));
        if (enemies != null)
        {
            Debug.Log("Live enemies found: " + enemies.Length);
            Debug.Log("Door portals found: " + portals.Length);
            enemyCount = enemies.Length;

            if (enemies.Length == 0)
                OpenPortals();
        }
    }

}
