using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIButton : MonoBehaviour
{
    AudioSystem audioSys;
    // Start is called before the first frame update
    void Start()
    {
        audioSys = FindObjectOfType<AudioSystem>();
    }

    // Update is called once per frame
    public void buttonSFX() {
        audioSys.PlaySFX("pageflip");
    }
}
