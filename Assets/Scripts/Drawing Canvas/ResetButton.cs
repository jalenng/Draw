using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetButton : MonoBehaviour
{

    public GameObject linesParent;

    private void OnMouseDown() {
        // Delete all Lines pertaining to the LineParent
        foreach (Transform child in linesParent.transform) {
            GameObject.Destroy(child.gameObject);
        }
    }

}
