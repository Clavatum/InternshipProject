using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI TotalPlayedTimeText;
    [SerializeField] private TextMeshProUGUI totalGoldText;
    [SerializeField] private TextMeshProUGUI feedbackText;

    private float messageDuration = 1f;

    #region - UI Events - 

    public void ShopButton()
    {
        feedbackText.transform.gameObject.SetActive(true);
        feedbackText.text = "VERY SOON!";
        Invoke(nameof(ClearMessage), messageDuration);
    }

    public void ExitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    #endregion

    private void ClearMessage()
    {
        feedbackText.text = "";
        feedbackText.transform.gameObject.SetActive(false);
    }
}
