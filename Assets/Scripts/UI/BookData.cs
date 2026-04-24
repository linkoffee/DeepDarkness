using System.Collections.Generic;
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

    private string _content;

    private HashSet<string> addedContentIds = new HashSet<string>();

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

    public string GetContent() => _content;

    public void SetContent(string newContent)
    {
        _content = newContent;
        addedContentIds.Clear();
    }

    public void AddContent(string additionalContent)
    {
        AddContent(additionalContent, null);
    }

    public void AddContent(string additionalContent, string contentId)
    {
        if (!string.IsNullOrEmpty(contentId) && addedContentIds.Contains(contentId))
            return;

        if (string.IsNullOrEmpty(_content))
            SetContent(additionalContent);
        else
            _content += "\n<page>" + additionalContent;

        if (!string.IsNullOrEmpty(contentId))
            addedContentIds.Add(contentId);
    }

    public void ClearContent()
    {
        SetContent("");
    }
}
