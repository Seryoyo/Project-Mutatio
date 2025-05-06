using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerManager : MonoBehaviour
{
    private Animator animator;

    public void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAnimTrigger(string trigger)
    {
        animator.SetTrigger(trigger);
    }

    public void ResetAnimTrigger(string trigger)
    {
        animator.ResetTrigger(trigger);
    }


}
