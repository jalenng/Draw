using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SendFeedbackButton : MonoBehaviour
{
    private string formURL = "https://forms.gle/PfYvvPr92fqZ8246A";
    public void OpenFeedbackPage()
    {
        Debug.Log($"[SendFeedbackButton] Opening the feedback form at {formURL}");
        Application.OpenURL(formURL);
    }
}
