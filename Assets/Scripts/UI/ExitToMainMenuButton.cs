using UnityEngine;

public class ExitToMainMenuButton : MonoBehaviour
{
    public void Exit() => LevelLoader.Instance.LoadLevelByName("MainMenu");
}
