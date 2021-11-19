using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusArea : MonoBehaviour
{
    void Start()
    {        
        // Disable line renderer component, as we only need it to show the area bounds in the scene view
        Destroy(GetComponent<LineRenderer>());   
    }
}
