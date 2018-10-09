using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Level_Loader : MonoBehaviour {

    public GameObject loadingScreen;
    public Slider slider;
    public Text progressText;

	public void Load_Next_Level(string scene_name)
    {
        StartCoroutine(LoadAsynchronously(scene_name));
    }

    IEnumerator LoadAsynchronously (string scene_name)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene_name);
        loadingScreen.SetActive(true);

        while(!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / .9f);
            slider.value = progress;
            progressText.text = progress * 100f + "%";

            yield return null;
        }
    }

}
