using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    private bool isMenuPanelActive = false;

    public void ToggleMenuPanel()
    {
        isMenuPanelActive = !isMenuPanelActive;
        menuPanel.SetActive(isMenuPanelActive);
    }
}