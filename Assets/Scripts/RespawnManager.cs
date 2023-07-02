using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeReference] List<RespawnInterface> objects;

    public void StartObjectRespawn() {
        foreach(RespawnInterface o in objects) {
            o.StartRespawn();
        }
    }
}
