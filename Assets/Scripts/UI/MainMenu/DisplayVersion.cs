using TMPro;
using UnityEngine;

public class DisplayVersion : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI versionText;

    private void Start()
    {
        if (versionText != null)
        {
            versionText.text = Application.version;
        }
    }
}