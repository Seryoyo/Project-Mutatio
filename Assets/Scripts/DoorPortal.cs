using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorPortal : Portal
{
    bool isOpen;
    public bool isSideDoor;

    private void Awake()
    {
        isOpen = false;
    }

    // Call from game manager when everyone is dead
    // TBA: Implement persistent chests/enemy death later

    public void EnableDoor()
    {
        isOpen = true;
        GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Sprites/OpenDoors{(isSideDoor ? "_Side" : "")}");
    }

    protected override void OnCollide(Collider2D coll)
    {
        if (isOpen)
            base.OnCollide(coll);
    }

}
