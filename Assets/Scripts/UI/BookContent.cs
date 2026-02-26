using TMPro;
using UnityEngine;

public class BookContent : MonoBehaviour
{
    [TextArea(10, 20)]
    [SerializeField] private string content;
    [SerializeField] private TMP_Text leftSide;
    [SerializeField] private TMP_Text rightSide;
    [SerializeField] private TMP_Text leftPagination;
    [SerializeField] private TMP_Text rightPagination;

    private void Awake()
    {
        SetupContent();
        UpdatePagination();
    }

    private void OnValidate()
    {
        UpdatePagination();

        if (leftSide.text == content)
            return;

        SetupContent();
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

    private void SetupContent()
    {
        leftSide.text = content;
        rightSide.text = content;
    }

    private void UpdatePagination()
    {
        leftPagination.text = Converter.ToRoman(leftSide.pageToDisplay);
        rightPagination.text = Converter.ToRoman(rightSide.pageToDisplay);
    }
}
