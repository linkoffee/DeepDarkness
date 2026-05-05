using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct BookContentData
{
    public string content;
    public string contentIds;
}

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

    private const string SaveKey = "BookContentData";

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
        LoadData();
    }

    public string GetContent() => _content;

    public void SetContent(string newContent)
    {
        _content = newContent;
        addedContentIds.Clear();
        SaveData();
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

        SaveData();
    }

    public void ClearContent()
    {
        SetContent("");
    }

    private void SaveData()
    {
        BookContentData data = new BookContentData
        {
            content = _content,
            contentIds = string.Join(",", addedContentIds)
        };
        SaveManager.Save(SaveKey, data);
    }

    private void LoadData()
    {
        BookContentData data = SaveManager.Load<BookContentData>(SaveKey);
        _content = data.content ?? "";

        if (!string.IsNullOrEmpty(data.contentIds))
        {
            addedContentIds.Clear();
            string[] ids = data.contentIds.Split(",");
            foreach (string id in ids)
            {
                if (!string.IsNullOrEmpty(id))
                    addedContentIds.Add(id);
            }
        }
    }
}
