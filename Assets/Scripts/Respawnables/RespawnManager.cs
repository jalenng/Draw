using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnManager : MonoBehaviour
{
    [SerializeReference] List<RespawnInterface> respawnOnDeath;

    public void StartObjectRespawn() {
        foreach(RespawnInterface o in respawnOnDeath) {
            o.StartRespawn();
        }
    }
}
