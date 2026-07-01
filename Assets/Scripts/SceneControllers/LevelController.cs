using System.Collections;
using DD.Utils;
using UnityEngine;

public class LevelController : BaseSceneController
{
    [Header("Music")]
    [SerializeField] public bool playMusic = true;
    [SerializeField] public string musicToPlay;

    [Header("Storytelling")]
    [SerializeField] public bool enableStorytelling = true;
    [TextArea(10, 20)]
    [SerializeField] public string storytellingText;
    [SerializeField] public string storytellingTranslationName;

    [Header("Notifications")]
    [SerializeField] public bool enableNotifications = false;
    [TextArea(3, 6)]
    [SerializeField] public string notificationText;
    [SerializeField] public string notificationTranslationName;
    [SerializeField] public float notificationDuration = 3f;
    [SerializeField] public string notificationSound = "DefaultNotification";

    [Header("Knowledge Book")]
    [SerializeField] public bool enableKnowledgeUpdate = false;
    [TextArea(10, 20)]
    [SerializeField] public string bookContentToAdd;
    [SerializeField] public string bookContentTranslationName;


    protected override void PlayMusicTrack()
    {
        if (playMusic && !string.IsNullOrEmpty(musicToPlay))
            MusicManager.Instance.PlayMusic(musicToPlay);
    }

    protected override void UpdateKnowledgeBookContent()
    {
        if (!enableKnowledgeUpdate)
            return;

        string id = $"{gameObject.scene.name}_{name}";

        if (!string.IsNullOrEmpty(bookContentTranslationName))
            BookData.Instance.AddLocalizedContent(bookContentTranslationName, id);
        else if (!string.IsNullOrEmpty(bookContentToAdd))
            BookData.Instance.AddContent(bookContentToAdd, id);
        else
            Debug.LogWarning("No content to add: both translation name and fallback text are empty");
    }

    protected override IEnumerator RunStorytelling()
    {
        if (!enableStorytelling)
            yield break;

        string localizedText = Localizator.GetLocalizedTextWithFallback(storytellingTranslationName, storytellingText);

        if (string.IsNullOrEmpty(localizedText))
            yield break;

        bool isClosed = false;

        void OnClosed()
        {
            isClosed = true;
            StorytellerManager.Instance.OnClosed -= OnClosed;
        }

        StorytellerManager.Instance.OnClosed += OnClosed;
        StorytellerManager.Instance.Show(localizedText);

        yield return new WaitUntil(() => isClosed);
    }

    protected override IEnumerator ShowNotification()
    {
        if (!enableNotifications)
            yield break;

        string localizedText = Localizator.GetLocalizedTextWithFallback(notificationTranslationName, notificationText);

        if (string.IsNullOrEmpty(localizedText))
            yield break;

        NotificationManager.Instance.Show(localizedText, notificationDuration, notificationSound);

        yield return new WaitForSeconds(notificationDuration);
    }
}
