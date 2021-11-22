using System;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum ELoadTransition
{
    Fade,
    FreezeFrame
}

[AddComponentMenu("SCScene/Scene Controller")]
public class SceneController : MonoBehaviour
{
    #region Public fields

    public event Action OnBeforeSceneUnload;
    public event Action OnAfterSceneLoad;

    [Header("ComponentSetup")] [Tooltip("Canvas group to play fade animation")]
    public CanvasGroup faderCanvasGroup;

    [Tooltip("Fade animation duration")] public float fadeDuration = 1f;

    public string persistentLevelName;

    #endregion


    #region Private fields

    private bool isFading;

    #endregion

    public static SceneController Instance;

    public SceneController()
    {
        Instance = this;
    }

    private void Start()
    {
        Application.targetFrameRate = 60;
        StartCoroutine(LoadSceneByFade("Scene_Game_01_S"));
    }

    #region Public methods

    public IEnumerator LoadSceneByFade(string sceneName)
    {
        if (!isFading)
        {
            if (SceneManager.GetActiveScene().name != persistentLevelName)
            {
                yield return StartCoroutine(FadeAndSwitchScenes(sceneName));
            }
            else
            {
                yield return StartCoroutine(FadeAndLoadScene(sceneName));
            }
        }
    }

    #endregion


    #region Private methods

    private IEnumerator FadeAndLoadScene(string sceneName)
    {
        faderCanvasGroup.alpha = 1f;
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        StartCoroutine(Fade(0f));
    }

    private IEnumerator FadeAndSwitchScenes(string sceneName)
    {
        yield return StartCoroutine(Fade(1f));
        OnBeforeSceneUnload?.Invoke();
        yield return SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().buildIndex);
        yield return StartCoroutine(LoadSceneAndSetActive(sceneName));
        OnAfterSceneLoad?.Invoke();
        yield return StartCoroutine(Fade(0f));
    }

    private IEnumerator LoadSceneAndSetActive(string sceneName)
    {
        yield return SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        var newlyLoadedScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);
        SceneManager.SetActiveScene(newlyLoadedScene);
    }


    private IEnumerator Fade(float finalAlpha)
    {
        isFading = true;
        var fadeSpeed = Mathf.Abs(faderCanvasGroup.alpha - finalAlpha) / fadeDuration;

        while (!Mathf.Approximately(faderCanvasGroup.alpha, finalAlpha))
        {
            faderCanvasGroup.alpha = Mathf.MoveTowards(faderCanvasGroup.alpha, finalAlpha,
                fadeSpeed * Time.deltaTime);

            yield return null;
        }

        isFading = false;
        faderCanvasGroup.blocksRaycasts = false;
    }

    #endregion
}