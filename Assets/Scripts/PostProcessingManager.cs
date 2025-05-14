using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class PostProcessingManager : MonoBehaviour
{
    public static PostProcessingManager instance;
    public Volume postProcessingVolume;
    
    private readonly HashSet<string> excludedScenes = new HashSet<string> { "MainMenu", "Death" };

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        instance = this;
        DontDestroyOnLoad(gameObject);
        
        // find the post-processing volume if not assigned
        if (postProcessingVolume == null)
            postProcessingVolume = GetComponent<Volume>();
        
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (this == null || !gameObject)
            return;
            
        // enable/disable post-processing based on scene name
        bool shouldEnablePostProcessing = !excludedScenes.Contains(scene.name);
        
        if (postProcessingVolume != null)
        {
            postProcessingVolume.enabled = shouldEnablePostProcessing;
            Debug.Log($"post processing in {scene.name}: {(shouldEnablePostProcessing ? "enabled" : "disabled")}");
        }
    }
}