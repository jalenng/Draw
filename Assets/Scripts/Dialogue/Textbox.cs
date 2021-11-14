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
    [SerializeField] Image CTCImage;

    // State variables
    Sprite avatarSprite;
    string contentText;
    int CPS;

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

    // Updates the textbox to show the current avatar and content
    public IEnumerator Say()
    {
        // Hide the CTM indicator
        CTCImage.enabled = false;

        // Update avatar image
        avatarImage.sprite = avatarSprite;

        // Update dialogue content text.
        // If typewriting effect is enabled...
        if (CPS > 0)
        {
            // Get the number of characters
            int totalCharacters = contentText.Length;

            // At every interval, update the textbox text to achieve a typewriter effect
            for (int i = 0; i <= totalCharacters; i++)
            {
                contentTMP.text = contentText.Substring(0, i);
                yield return new WaitForSeconds(1.0f / CPS);
            }
        }
        // Else, don't typewrite the text.
        else
        {
            contentTMP.text = contentText;
        }

        // Show the CTM indicator
        CTCImage.enabled = true;
    }

}