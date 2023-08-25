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
        Debug.Log("Trigger entered");
        if (other.gameObject.CompareTag("OrangeObject"))
        {
            Debug.Log("OrangeObject collided");
            scene1.SetActive(true);
        }
    }
    
}
