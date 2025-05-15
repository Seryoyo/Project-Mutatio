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
    private string lastLoadedScene = "";
    MusicManager musicManager;

    // end victory
    public string endGameScene = "End";
    private bool bossDefeated = false;

    // room progression tracking
    public int currRoomCount = 0;
    public int totalRoomCount = 0;
    public string miniBRoomScene = "MiniBossRoom";
    public string bossRoomScene = "BossRoom";
    public float miniBossChance = 0.25f;  // 40% chance for mini boss room when eligible
    public bool hasShownDejaVuText = false;

    // room progression thresholds
    public int miniBossMinCount = 4; // start checking for mini boss room after this many rooms
    public int miniBossMaxCount = 8; // stops checking after this many rooms
    public int bossRoomCount = 10;  // forces boss room after this many total rooms
    public int dejaVuModulo = 3;    // show the text when totalRoomCount % dejaVuModulo == 0
    public int firstDejaVuText = 3; // first time to show text

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
        musicManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<MusicManager>();
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        if (enemyCount == 0)
            OpenPortals();
    }

    public void DecrementEnemyCount()
    {
        --enemyCount;

        if (SceneManager.GetActiveScene().name == bossRoomScene)
        {
            StartCoroutine(EndGameSequence());
        } else if (enemyCount == 0)
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

    public void ResetRoomCounts()
    {
        currRoomCount = 0;
        totalRoomCount = 0;
        hasShownDejaVuText = false;
        Debug.Log("room counts reset");
    }

    // checks if should force a specific scene (mini boss or boss)
    private void MaybeOverrideDestination(DoorPortal portal)
    {
        if (SceneManager.GetActiveScene().name == "Tutorial" ||
            SceneManager.GetActiveScene().name == miniBRoomScene ||
            SceneManager.GetActiveScene().name == bossRoomScene)
            return;

        if (totalRoomCount >= bossRoomCount)
        {
            portal.destinationScene = bossRoomScene;
            return;
        }

        // check miniboss room first (takes priority over boss room)
        if (currRoomCount >= miniBossMinCount && currRoomCount < miniBossMaxCount)
        {
            if (Random.value < miniBossChance)
            {
                portal.destinationScene = miniBRoomScene;
                return;
            }
        }

        // check for boss room after mini-boss check fails
        if (currRoomCount >= miniBossMinCount && currRoomCount < miniBossMaxCount)
        {
            float roll = Random.value;
            if (roll < miniBossChance)
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
        bool isBossRoom = currentScene.name == bossRoomScene;

        if (currentScene.name != "MainMenu" && currentScene.name != "Death" &&
            currentScene.name != "StartMenu" && !isTutorial)
        {
            currRoomCount++;
            totalRoomCount++;

            // reset currRoomCount if we're in the mini boss room
            if (currentScene.name == miniBRoomScene)
                currRoomCount = 0;

            if (isBossRoom)
            {
                StartCoroutine(ShowBossRoomText());
            }
            else if (totalRoomCount >= firstDejaVuText && totalRoomCount % dejaVuModulo == 0 && gameObject != null)
            {
                StartCoroutine(ShowDejaVuText());
                hasShownDejaVuText = true;
            }
        }

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (!isTutorial)
        {
            if (enemies != null)
            {
                enemyCount = enemies.Where(e => e != null && e.hitpoint > 0).Count();
            }
            else
            {
                enemyCount = 0;
            }
        }
        else
        {
            enemyCount = 0;
        }

        portals = FindObjectsOfType<DoorPortal>();

        if (isTutorial && portals != null && portals.Length > 0)
        {
            OpenPortals();
        }
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this == null || !gameObject)
        {
            return;
        }

        if (scene.name != bossRoomScene)
            bossDefeated = false;

        if (scene.name == bossRoomScene)
        {
            musicManager.BossChange(musicManager.bossMusic);
        }

        if (scene.name == "Tutorial" && lastLoadedScene == "Death")
        {
            ResetRoomCounts();
        }

        bool isNewScene = (lastLoadedScene != scene.name);
        lastLoadedScene = scene.name;

        if (isNewScene)
        {
            SetupCurrentScene();
        }
        else
        {
            portals = FindObjectsOfType<DoorPortal>();
            bool isTutorial = scene.name == "Tutorial";
            if (isTutorial || enemyCount == 0 &&  scene.name != bossRoomScene)
            {
                OpenPortals();
            }
        }
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

    private IEnumerator ShowBossRoomText()
    {
        yield return new WaitForSeconds(2f);

        if (this == null || !gameObject)
            yield break;

        if (GameManager.instance != null && Player.instance != null)
        {
            string bossMessage = "Wait... that monster feels familiar..";

            GameManager.instance.ShowText(bossMessage,
                30, Color.white,
                Player.instance.transform.position,
                new Vector3(0, 50f, 0),
                3f);
        }
    }

    private IEnumerator EndGameSequence()
    {
        if (bossDefeated)
        {
            yield break;
        }
        bossDefeated = true;

        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene(endGameScene);
    }
}
