using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public Animator anim;

    public void LoadTransition(string name)
    {
        StartCoroutine(LoadScene(name));
    }

    IEnumerator LoadScene(string sceneName)
    {
        Time.timeScale = 1f;
        anim.SetTrigger("end");
        yield return new WaitForSeconds(1.5f);

        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
