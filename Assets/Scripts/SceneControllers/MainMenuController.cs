using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuController : BaseSceneController
{
    protected override void PlayMusicTrack()
    {
        MusicManager.Instance.PlayMusic("MainMenu");
    }
}
