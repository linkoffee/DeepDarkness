using TMPro;
using UnityEngine;

public class BookContent : MonoBehaviour
{
    public static BookContent Instance { get; private set; }

    [SerializeField] private TMP_Text leftSide;
    [SerializeField] private TMP_Text rightSide;
    [SerializeField] private TMP_Text leftPagination;
    [SerializeField] private TMP_Text rightPagination;

    public static BookContent FindOrCreateInstance()
    {
        if (Instance == null)
        {
            Instance = FindObjectOfType<BookContent>(true);

            if (Instance != null)
                Instance.Initialize();
        }
        return Instance;
    }

    private void Awake()
    {
        Instance = this;
        Initialize();
    }

    private void OnValidate()
    {
        UpdatePagination();
    }

    public void SetContent(string newContent)
    {
        BookData.Instance.SetContent(newContent);
        LoadContentFromData();
        ResetToFirstPage();
    }

    public void AddContent(string additionalContent)
    {
        BookData.Instance.AddContent(additionalContent);
        LoadContentFromData();
    }

    public void ClearContent()
    {
        BookData.Instance.ClearContent();
        LoadContentFromData();
        ResetToFirstPage();
    }

    public void LoadContentFromData()
    {
        string currentContent = BookData.Instance.GetContent();

        leftSide.text = currentContent;
        rightSide.text = currentContent;
    }

    public void GoToPreviousPage()
    {
        if (leftSide.pageToDisplay < 1)
        {
            leftSide.pageToDisplay = 1;
            return;
        }

        if (leftSide.pageToDisplay - 2 > 1)
        {
            leftSide.pageToDisplay -= 2;
        }
        else
        {
            leftSide.pageToDisplay = 1;
        }

        rightSide.pageToDisplay = leftSide.pageToDisplay + 1;

        UpdatePagination();
    }

    public void GoToNextPage()
    {
        if (rightSide.pageToDisplay >= rightSide.textInfo.pageCount)
            return;

        if (leftSide.pageToDisplay >= leftSide.textInfo.pageCount - 1)
        {
            leftSide.pageToDisplay = leftSide.textInfo.pageCount - 1;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        }
        else
        {
            leftSide.pageToDisplay += 2;
            rightSide.pageToDisplay = leftSide.pageToDisplay + 1;
        }

        UpdatePagination();
    }

    private void Initialize()
    {
        LoadContentFromData();
        UpdatePagination();
    }

    private void ResetToFirstPage()
    {
        leftSide.pageToDisplay = 1;
        rightSide.pageToDisplay = 2;

        UpdatePagination();
    }

    private void UpdatePagination()
    {
        leftPagination.text = Converter.ToRoman(leftSide.pageToDisplay);
        rightPagination.text = Converter.ToRoman(rightSide.pageToDisplay);
    }
}
