using UnityEngine;

public class BookData : MonoBehaviour
{
    private static BookData _instance;
    public static BookData Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject gameObject = new GameObject("BookData");
                _instance = gameObject.AddComponent<BookData>();
                DontDestroyOnLoad(gameObject);
            }
            return _instance;
        }
    }

    private string content;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public string GetContent() => content;

    public void SetContent(string newContent)
    {
        content = newContent;
    }

    public void AddContent(string additionalContent)
    {
        if (string.IsNullOrEmpty(content))
            SetContent(additionalContent);
        else
            content += "\n\n<page>" + additionalContent;
    }

    public void ClearContent()
    {
        SetContent("");
    }
}
