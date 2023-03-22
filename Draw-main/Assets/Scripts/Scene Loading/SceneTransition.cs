using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] Image loadingAnimation;
    
    // Cached components
    Animator anim;
    Image image;

    void Start()
    {
        anim = GetComponent<Animator>();
        image = GetComponent<Image>();

        StartCoroutine(FadeIn());
    }

    public void SetTransitionColor(Color color)
    {
        image.color = color;

        loadingAnimation.color = new Color(
            1f - color.r, 
            1f - color.g, 
            1f - color.b);
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
