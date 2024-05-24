using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject LoadingScreen;
    [SerializeField]
    private Image LoadingBarFill;
    public void LoadScene(int sceneId)
    {
        StartCoroutine(LoadSceneAsync(sceneId));
        SoundManager.PlaySound("MenuSound");
    }

    IEnumerator LoadSceneAsync(int sceneId)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneId);
        LoadingScreen.SetActive(true);
        while(!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress/0.9f);
            LoadingBarFill.fillAmount = progressValue;
            yield return null;
        }
    }

}
