using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Textbox : MonoBehaviour
{
    // Configuration parameters
    [Header("Components")]
    [SerializeField] TextMeshProUGUI contentTMP;
    [SerializeField] Image avatarImage;
    [SerializeField] Animator CTCAnimator;

    // State variables
    Sprite avatarSprite;
    string contentText;
    int CPS;

    int totalCharacters;
    int i;

    // Updates the avatar image to show
    public void setAvatar(Sprite avatar)
    {
        this.avatarSprite = avatar;
    }

    // Updates the content text to show
    public void setText(string newText)
    {
        this.contentText = newText;
    }

    // Updates the characters-per-second
    public void setCPS(int newCPS)
    {
        this.CPS = newCPS;
    }

    // Clears the avatar image and content text
    public void Clear()
    {
        this.avatarSprite = null;
        this.contentText = "";
        this.CPS = 0;
        StartCoroutine(Say());
    }

    // Stops the typewriting effect for the current dialogue entry, and shows all the text
    public void FastForward()
    {
        i = totalCharacters - 1;
    }

    // Updates the textbox to show the current avatar and content
    public IEnumerator Say()
    {
        // Hide the CTC indicator
        CTCAnimator.SetBool("Hidden", true);

        // Update avatar image
        avatarImage.sprite = avatarSprite;

        // If there is no avatar image, let the text use the space for the avatar
        RectTransform contentTMPRectTransform = contentTMP.gameObject.GetComponent<RectTransform>();
        if (avatarSprite == null)
        {
            avatarImage.color = new Color(1, 1, 1, 0);
            contentTMPRectTransform.offsetMin = new Vector2(42, contentTMPRectTransform.offsetMin.y);
        }
        else
        {
            avatarImage.color = new Color(1, 1, 1, 1);
            contentTMPRectTransform.offsetMin = new Vector2(320, contentTMPRectTransform.offsetMin.y);
        }

        // Update dialogue content text.
        // If typewriting effect is enabled...
        if (CPS > 0)
        {
            // Calculate the duration between characters
            float SPC = 1.0f / CPS;

            // Get the number of characters
            totalCharacters = contentText.Length;

            // At every interval, update the textbox text to achieve a typewriter effect
            for (i = 0; i <= totalCharacters; i++)
            {
                contentTMP.text = contentText.Substring(0, i);
                yield return new WaitForSeconds(SPC);
            }
        }
        // Else, don't typewrite the text.
        else
            contentTMP.text = contentText;

        // Show the CTC indicator
        CTCAnimator.SetBool("Hidden", false);

    }

}