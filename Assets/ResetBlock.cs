using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBlock : MonoBehaviour
{
    public OrangeObject orangeObject;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player") orangeObject.enableOrangeObject();
    }
}
