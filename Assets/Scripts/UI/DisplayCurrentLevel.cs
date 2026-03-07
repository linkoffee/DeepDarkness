using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DisplayCurrentLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI currentLevel;

    private void Start()
    {
        currentLevel.text = "Level " + SceneManager.GetActiveScene().buildIndex.ToString();
    }
}
