using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueWindow : MonoBehaviour
{
    // References
    public static DialogueWindow instance;
    TextMeshProUGUI nameTxt;
    TextMeshProUGUI dialogueTxt;
    public bool isActive;

    // Start is called before the first frame update
    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        nameTxt = transform.Find("NameTag/DialogueSpeakerName").GetComponent<TextMeshProUGUI>();
        dialogueTxt = transform.Find("DialogueWindow/DialogueText").GetComponent<TextMeshProUGUI>();
        if (GameManager.instance != null)
            GameManager.instance.dialogueWindow = this;
    }

    public void Toggle(string name = "", string dialogue = "")
    {
        nameTxt.text = name;
        dialogueTxt.text = dialogue;

        // If name is blank, don't show nametag
        if (name.Equals(""))
            transform.Find("NameTag").GetComponent<UnityEngine.UI.Image>().enabled = false;
        else
            transform.Find("NameTag").GetComponent<UnityEngine.UI.Image>().enabled = true;

        isActive = !isActive;
        GetComponent<Canvas>().enabled = isActive;
    }

    public void Hide()
    {
        isActive = false;
        GetComponent<Canvas>().enabled = isActive;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Hide();
    }

}
