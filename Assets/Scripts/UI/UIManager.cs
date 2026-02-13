using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Terminal terminal;
    
    public void OnExecutePressed()
    {
        if (terminal != null)
        {
            terminal.ExecuteAll();
        }
    }

    public void OnRestartLevelPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
