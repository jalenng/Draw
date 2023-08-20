using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneSwitch : MonoBehaviour
{
    public GameObject scene1;
    public GameObject scene2;
    // Start is called before the first frame update
    void Start()
    {
        
    }

private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("OrangeObject"))
        {
            scene2.SetActive(true);
            scene1.SetActive(false);
        }
    }
    
}
