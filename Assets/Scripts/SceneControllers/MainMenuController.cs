using UnityEngine;

public class MainMenuController : BaseSceneController
{
    [SerializeField] private bool playMusic = true;

    protected override void PlayMusicTrack()
    {
        if (playMusic)
            MusicManager.Instance.PlayMusic("MainMenu");
    }
}
