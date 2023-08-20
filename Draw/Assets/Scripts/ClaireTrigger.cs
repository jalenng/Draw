using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClaireTrigger : MonoBehaviour
{
    [SerializeField] private Animator anim;

    public void SetBool(string parameter)
    {
        anim.SetBool(parameter,true);
    }

    public void Trigger(string parameter)
    {
        anim.SetTrigger(parameter);
    }
}
