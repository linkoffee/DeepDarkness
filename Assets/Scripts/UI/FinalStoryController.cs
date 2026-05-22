using UnityEngine;
using System.Collections;

public class LastLevelStory : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 2f;

    [SerializeField] private CanvasGroup storyTextPanel;
    [SerializeField] private CanvasGroup theEndTextPanel;

    [SerializeField] private CanvasGroup storyText1;
    [SerializeField] private CanvasGroup storyText2;
    [SerializeField] private CanvasGroup storyText3;


    private enum StoryState
    {
        Idle,           // awaiting for user action
        FadingOut,      // disappear animation of current text
        FadingIn,       // appear animation of current text
        Completed       // story is completed
    }

    private StoryState currentState = StoryState.Idle;
    private int currentStep = 0;

    private void Start()
    {
        InitUIObjects();
        StartCoroutine(RunStory());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && currentState == StoryState.Idle && currentStep < 4)
            GoToNextStep();
    }

    private void OnGUI()
    {
        if (currentState == StoryState.Idle && currentStep < 3)
        {
            GUI.skin.label.fontSize = 20;
            GUI.skin.label.alignment = TextAnchor.MiddleCenter;

            GUI.Label(new Rect(Screen.width / 2 - 250, Screen.height - 100, 500, 40),
                      "Press left mouse button to continue...");
        }
    }

    private void InitUIObjects()
    {
        storyTextPanel.alpha = 0f;
        theEndTextPanel.alpha = 0f;
        storyText1.alpha = 0f;
        storyText2.alpha = 0f;
        storyText3.alpha = 0f;

        storyTextPanel.interactable = false;
        storyTextPanel.blocksRaycasts = false;
        theEndTextPanel.interactable = false;
        theEndTextPanel.blocksRaycasts = false;
    }

    private void GoToNextStep()
    {
        currentState = StoryState.FadingOut;
        StartCoroutine(ProcessStep());
    }

    private IEnumerator RunStory()
    {
        currentState = StoryState.FadingIn;

        yield return StartCoroutine(FadeTo(storyTextPanel, 0f, 1f));
        yield return StartCoroutine(FadeTo(storyText1, 0f, 1f));

        currentStep = 0;
        currentState = StoryState.Idle;
    }

    private IEnumerator ProcessStep()
    {
        switch (currentStep)
        {
            case 0:
                yield return StartCoroutine(FadeTo(storyText1, 1f, 0f));
                currentStep++;

                currentState = StoryState.FadingIn;
                yield return StartCoroutine(FadeTo(storyText2, 0f, 1f));
                currentState = StoryState.Idle;
                break;

            case 1:
                yield return StartCoroutine(FadeTo(storyText2, 1f, 0f));
                currentStep++;

                currentState = StoryState.FadingIn;
                yield return StartCoroutine(FadeTo(storyText3, 0f, 1f));
                currentState = StoryState.Idle;
                break;

            case 2:
                yield return StartCoroutine(FadeTo(storyText3, 1f, 0f));
                yield return StartCoroutine(FadeTo(storyTextPanel, 1f, 0f));
                currentStep++;

                currentState = StoryState.FadingIn;
                yield return StartCoroutine(FadeTo(theEndTextPanel, 0f, 1f));
                currentState = StoryState.Completed;
                break;

            case 3:
                LevelLoader.Instance.LoadLevelByName("MainMenu");
                break;
        }
    }

    private IEnumerator FadeTo(CanvasGroup cg, float startAlpha, float endAlpha)
    {
        if (cg == null) yield break;

        float elapsedTime = 0f;
        cg.alpha = startAlpha;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / fadeDuration;
            cg.alpha = Mathf.Lerp(startAlpha, endAlpha, t);
            yield return null;
        }

        cg.alpha = endAlpha;
    }
}