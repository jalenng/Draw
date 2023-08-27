using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateTrigger : MonoBehaviour
{
    public GameObject scene1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[ActivateTrigger] Trigger entered");
        if (other.gameObject.CompareTag("OrangeObject"))
        {
            Debug.Log("[ActivateTrigger] OrangeObject collided");
            scene1.SetActive(true);
        }
    }
    
}
