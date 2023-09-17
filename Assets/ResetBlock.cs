using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetBlock : MonoBehaviour
{
    public List<RespawnInterface> objs;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Player") {
            foreach(RespawnInterface o in objs) {
                o.StartRespawn();
            }
        }
    }
}
