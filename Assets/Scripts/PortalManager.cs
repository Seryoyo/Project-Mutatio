using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalManager : MonoBehaviour
{
    public static PortalManager instance;
    int enemyCount;
    DoorPortal[] portals;
    
    // room progression tracking
    public int currRoomCount = 0;
    public int totalRoomCount = 0;
    public string miniBRoomScene = "MiniBossRoom";
    public string bossRoomScene = "BossRoom";
    public float miniBossChance = 0.25f;  // 40% chance for mini boss room when eligible
    public bool hasShownDejaVuText = false;
    
    // room progression thresholds
    public int miniBossMinCount = 6; // start checking for mini boss room after this many rooms
    public int miniBossMaxCount = 9; // stops checking after this many rooms
    public int bossRoomCount = 20;  // forces boss room after this many total rooms
    public int dejaVuModulo = 3;    // show the text when totalRoomCount % dejaVuModulo == 0
    public int firstDejaVuText = 6; // first time to show text

    // random deja vu messages for different stages of the game
    [Header("Déjà Vu Messages")]
    public string[] earlyDejaVuMessages = new string[] 
    {
        "Haven't I been here before?...",
        "This place looks familiar...",
        "I feel like I've seen this before...",
        "Something about this room feels off..."
    };
    
    public string[] midDejaVuMessages = new string[] 
    {
        "I'm definitely getting deja vu...",
        "I swear I've been through this room already...",
        "Am I going in circles?",
        "This can't be a coincidence anymore..."
    };
    
    public string[] lateDejaVuMessages = new string[] 
    {
        "I'm trapped in some kind of loop...",
        "This is the same place. I know it. I'M NOT CRAZY!",
        "Am I going insane?",
        "Am I schizophrenic?",
    };

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        SetupCurrentScene();
        
        if (enemyCount == 0)
            OpenPortals();
    }

    public void DecrementEnemyCount()
    {
        --enemyCount;
        if (enemyCount == 0)
            OpenPortals();
    }

    public void OpenPortals()
    {
        if (portals == null)
            return;
            
        foreach (var port in portals)
        {
            if (port == null)
                continue;
                
            MaybeOverrideDestination(port);
            port.EnableDoor();
        }
    }
    
    // checks if should force a specific scene (mini boss or boss)
    private void MaybeOverrideDestination(DoorPortal portal)
    {
        if (SceneManager.GetActiveScene().name == "Tutorial" || 
            SceneManager.GetActiveScene().name == miniBRoomScene || 
            SceneManager.GetActiveScene().name == bossRoomScene)
            return;
            
        // force boss room after bossRoomCount rooms
        if (totalRoomCount >= bossRoomCount)
        {
            portal.destinationScene = bossRoomScene;
            return;
        }
        
        // randomly trigger mini boss room between thresholds
        if (currRoomCount >= miniBossMinCount && currRoomCount < miniBossMaxCount)
        {
            if (Random.value < miniBossChance)
            {
                portal.destinationScene = miniBRoomScene;
                return;
            }
        }
    }

    private void SetupCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        bool isTutorial = currentScene.name == "Tutorial";

        if (currentScene.name != "MainMenu" && currentScene.name != "Death" && 
            currentScene.name != "StartMenu" && !isTutorial)
        {
            currRoomCount++;
            totalRoomCount++;
            
            // reset currRoomCount if we're in the mini boss room
            if (currentScene.name == miniBRoomScene)
                currRoomCount = 0;
                
            // show deja vu text based on modulo and after the first threshold
            if (totalRoomCount >= firstDejaVuText && totalRoomCount % dejaVuModulo == 0 && gameObject != null)
            {
                StartCoroutine(ShowDejaVuText());
                hasShownDejaVuText = true;
            }
        }
        
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (!isTutorial) {
            if (enemies != null) {
                enemyCount = enemies.Where(e => e != null && e.hitpoint > 0).Count();
            } else {
                enemyCount = 0;
            }
        } else {
            enemyCount = 0;
        }
            
        portals = FindObjectsOfType<DoorPortal>();

        if (isTutorial && portals != null && portals.Length > 0) {
            OpenPortals();
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this == null || !gameObject)
            return;
            
        SetupCurrentScene();
    }
    
    private IEnumerator ShowDejaVuText()
    {
        yield return new WaitForSeconds(2f);
        
        if (this == null || !gameObject)
            yield break;
            
        if (GameManager.instance != null && Player.instance != null)
        {
            string[] messageArray;
            
            if (totalRoomCount >= firstDejaVuText + dejaVuModulo * 4)
            {
                messageArray = lateDejaVuMessages;
            }
            else if (totalRoomCount >= firstDejaVuText + dejaVuModulo * 2)
            {
                messageArray = midDejaVuMessages;
            }
            else
            {
                messageArray = earlyDejaVuMessages;
            }
            
            string message = messageArray[Random.Range(0, messageArray.Length)];
            
            GameManager.instance.ShowText(message, 
                30, Color.white, 
                Player.instance.transform.position, 
                new Vector3(0, 50f, 0), 
                3f);
        }
    }
}
