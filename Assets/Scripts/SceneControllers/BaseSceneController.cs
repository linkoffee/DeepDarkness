using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public abstract class BaseSceneController : MonoBehaviour
{
    private void Start()
    {
        PlayMusicTrack();
        StartCoroutine(SceneSequence());
    }

    protected abstract void PlayMusicTrack();
    
    protected virtual void UpdateKnowledgeBookContent() { }

    protected virtual IEnumerator RunStorytelling()
    {
        yield break;
    }

    protected virtual IEnumerator ShowNotification()
    {
        yield break;
    }

    private IEnumerator SceneSequence()
    {
        yield return StartCoroutine(RunStorytelling());

        yield return StartCoroutine(ShowNotification());
        
        UpdateKnowledgeBookContent();
    }
}
