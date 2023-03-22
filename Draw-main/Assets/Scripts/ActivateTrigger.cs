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
        if (other.gameObject.CompareTag("OrangeObject"))
        {
            scene1.SetActive(true);
        }
    }
    
}
