using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelTransition : MonoBehaviour
{
    // Configuration parameters
    [Header("Fade In")]
    [SerializeField] Color fadeInColor = Color.black;

    [Header("Fade Out")]
    [SerializeField] Color fadeOutColor = Color.black;

    // Cached components
    Animator anim;
    Image image;

    void Start()
    {
        anim = GetComponent<Animator>();
        image = GetComponent<Image>();

        image.color = fadeInColor;
    }

    public void SetFadeOutColor(Color color)
    {
        fadeOutColor = color;
    }

    public IEnumerator FadeOut()
    {
        image.color = fadeOutColor;
        anim.SetTrigger("fadeOut");
        yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
    }

}
