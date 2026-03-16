using UnityEngine;

public class SfxManager : MonoBehaviour
{
    public static SfxManager Instance { get; private set; }

    [SerializeField] private SfxLib sfxLib;
    [SerializeField] private AudioSource sfxSource;

    public void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void PlaySound3D(AudioClip audioClip, Vector3 position)
    {
        if (audioClip != null)
            AudioSource.PlayClipAtPoint(audioClip, position);
    }

    public void PlaySound3D(string sfxName, Vector3 position)
    {
        PlaySound3D(sfxLib.GetClipFromName(sfxName), position);
    }

    public void PlaySound2D(string sfxName)
    {
        sfxSource.PlayOneShot(sfxLib.GetClipFromName(sfxName));
    }
}
