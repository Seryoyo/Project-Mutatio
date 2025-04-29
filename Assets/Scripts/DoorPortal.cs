using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPortal : Portal
{
    bool isOpen;

    protected override void Start()
    {
        base.Start();
        isOpen = false;
    }

    // Call from game manager when everyone is dead
    // TBA: Implement persistent chests/enemy death later
    public void EnableDoor()
    {
        Debug.Log("Opening door");
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("Sprites/OpenDoors");
        isOpen = true;
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (isOpen)
            base.OnCollide(coll);
    }

}
