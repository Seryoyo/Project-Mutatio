using UnityEngine;

public class Collectable : Collidable
{
    // Logic
    protected bool collected;

    protected override void OnCollide(Collider2D coll)
    {
        if (coll.name == "Player")
        {
            OnCollect();
        }
    }
    protected virtual void OnCollect()
    {
        if (!collected)
            collected = true;
    }
}
