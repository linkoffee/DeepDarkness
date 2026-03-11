using UnityEngine;

public class FinishLevelPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            LevelLoader.Instance.LoadNextLevel();
        }
    }
}
