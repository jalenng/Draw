using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetCanvas : MonoBehaviour
{
    public List<RespawnInterface> objs;

    public void RespawnObjects()
    {
        foreach (RespawnInterface o in objs)
        {
            o.StartRespawn();
        }
    }
}
