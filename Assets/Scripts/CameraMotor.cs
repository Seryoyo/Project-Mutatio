using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt;
    // How far can player go before camera follows
    public float boundX = 0.15f;
    public float boundY = 0.15f;

    private void Start()
    {
        lookAt = GameManager.instance.player.transform;
    }

    // Late update so we can move camera after player moves w/ FixedUpdate
    // AKA: avoid small (but annoying) desync
    private void LateUpdate()
    {
        Vector3 delta = Vector3.zero;

        // Check if we're inside the bounds on the X axis
        float deltaX = lookAt.position.x - transform.position.x;
        if (deltaX > boundX || deltaX < -boundX)
        {
            if (transform.position.x < lookAt.position.x)
            {
                delta.x = deltaX - boundX;
            } else
            {
                delta.x = deltaX + boundX;
            }
        }
        // Check if we're inside the bounds on the Y axis
        float deltaY = lookAt.position.y - transform.position.y;
        if (deltaY > boundY || deltaY < -boundY)
        {
            if (transform.position.y < lookAt.position.y)
            {
                delta.y = deltaY - boundY;
            }
            else
            {
                delta.y = deltaY + boundY;
            }
        }

        transform.position += new Vector3(delta.x, delta.y, 0);
    }

}
