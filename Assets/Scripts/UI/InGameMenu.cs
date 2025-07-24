using UnityEngine;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private GameObject menuPanel;

    private bool isMenuPanelActive = false;

    public void IsMenuPanelActive()
    {
        isMenuPanelActive = !isMenuPanelActive;
    }

    public void SetMenuPanelActiveness()
    {
        menuPanel.SetActive(isMenuPanelActive);
    }
}