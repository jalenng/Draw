using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    // Cached components
    Animator anim;
    Image image;

    void Start()
    {
        anim = GetComponent<Animator>();
        image = GetComponent<Image>();
    }

    public void SetTransitionColor(Color color)
    {
        image.color = color;
    }

    public IEnumerator FadeIn()
    {
        anim.SetTrigger("fadeIn");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
    }

    public IEnumerator FadeOut()
    {
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
    }

}
