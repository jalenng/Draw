using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton : MonoBehaviour
{
    [SerializeField] bool destroyImmediately = true;

    private void Awake()
    {
        // Check if another game object with the same tag already exists
        string tag = gameObject.tag;
        GameObject[] instance = GameObject.FindGameObjectsWithTag(tag);

        bool instanceAlreadyExists = false;
        for (int i = 0; i < instance.Length; i++)
        {
            if (instance[i] != gameObject)
            {
                instanceAlreadyExists = true;
                break;
            }
        }

        if (instanceAlreadyExists)
        {
            // The instance already exists, so destroy this one
            if (destroyImmediately)
                DestroyImmediate(this.gameObject);
            else
                Destroy(this.gameObject);
        }
        else
        {
            // This is the singleton instance
            DontDestroyOnLoad(this.gameObject);
        }
    }
}
