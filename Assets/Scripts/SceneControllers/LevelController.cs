using System.Collections;
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

    [Header("Notifications")]
    [SerializeField] public bool enableNotifications = false;
    [TextArea(3, 6)]
    [SerializeField] public string notificationText;
    [SerializeField] public float notificationDuration = 3f;
    [SerializeField] public string notificationSound = "DefaultNotification";

    [Header("Knowledge Book")]
    [SerializeField] public bool enableKnowledgeUpdate = false;
    [TextArea(10, 20)]
    [SerializeField] public string contentToAdd;

    protected override void PlayMusicTrack()
    {
        if (playMusic && !string.IsNullOrEmpty(musicToPlay))
            MusicManager.Instance.PlayMusic(musicToPlay);
    }

    protected override void UpdateKnowledgeBookContent()
    {
        if (enableKnowledgeUpdate && !string.IsNullOrEmpty(contentToAdd))
        {
            string id = $"{gameObject.scene.name}_{name}";
            BookData.Instance.AddContent(contentToAdd, id);
        }
    }

    protected override IEnumerator RunStorytelling()
    {
        if (!enableStorytelling || string.IsNullOrEmpty(storytellingText))
            yield break;

        bool isClosed = false;

        void OnClosed()
        {
            isClosed = true;
            StorytellerManager.Instance.OnClosed -= OnClosed;
        }

        StorytellerManager.Instance.OnClosed += OnClosed;
        StorytellerManager.Instance.Show(storytellingText);

        yield return new WaitUntil(() => isClosed);
    }

    protected override IEnumerator ShowNotification()
    {
        if (!enableNotifications || string.IsNullOrEmpty(notificationText))
            yield break;

        NotificationManager.Instance.Show(notificationText, notificationDuration, notificationSound);

        yield return new WaitForSeconds(notificationDuration);
    }
}
