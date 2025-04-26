using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : Collidable
{
    public string destinationScene;
    public GameManager instance;
    public Vector3 spawnPoint;
    public bool flipSprite;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.LoadScene(destinationScene);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        GameObject player = GameObject.Find("Player");
        if (player != null && spawnPoint != null)
            player.transform.position = spawnPoint;
        if (flipSprite)
            player.transform.localScale = new Vector3(-1, 1, 1);

        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
