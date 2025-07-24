using TMPro;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI TotalPlayedTimeText;
    [SerializeField] private TextMeshProUGUI totalGoldText;
    [SerializeField] private TextMeshProUGUI feedbackText;

    private float messageDuration = 1f;

    public void ShopButton()
    {
        feedbackText.transform.gameObject.SetActive(true);
        feedbackText.text = "VERY SOON!";
        Invoke(nameof(ClearMessage), messageDuration);
    }

    private void ClearMessage()
    {
        feedbackText.text = "";
        feedbackText.transform.gameObject.SetActive(false);
    }

    public void ExitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }
}
