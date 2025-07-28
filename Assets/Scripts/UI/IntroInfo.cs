using UnityEngine;
using UnityEngine.UI;

public class IntroInfo : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;

    public void OpenInfoPanel()
    {
        infoPanel.SetActive(true);
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}
