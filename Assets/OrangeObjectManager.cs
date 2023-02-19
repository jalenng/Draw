using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeObjectManager : MonoBehaviour
{
       // This is a class to collectively start resapwns for all orange objects added to it. 
    // Have to add all orange objects in a stage to the list for them to respawn.
    [SerializeField] List<OrangeObject> orangeObjects;

    public void StartOrangeObjectsRespawn() {
        Debug.Log("InManagaer");
        foreach(OrangeObject o in orangeObjects) {
            Debug.Log("Looping");
            o.StartRespawn();
        }
    }
}
