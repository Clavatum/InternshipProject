using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class IntroInfo : MonoBehaviour
{
    [SerializeField] private GameObject infoPanel;
    [SerializeField] private XRSimpleInteractable openInfoPanelButton;
    [SerializeField] private XRSimpleInteractable closeInfoPanelButton;
    private Settings settings;

    void Awake()
    {
        settings = GetComponent<Settings>();
        openInfoPanelButton.selectEntered.AddListener(zort => OpenInfoPanel());
        closeInfoPanelButton.selectEntered.AddListener(zort => CloseInfoPanel());
    }

    public void OpenInfoPanel()
    {
        infoPanel.SetActive(true);
        closeInfoPanelButton.gameObject.SetActive(true);
        openInfoPanelButton.gameObject.SetActive(false);
        settings.settingsButton.SetActive(false);
        settings.menuPanel.SetActive(false);
    }

    public void CloseInfoPanel()
    {
        infoPanel.SetActive(false);
        closeInfoPanelButton.gameObject.SetActive(false);
        openInfoPanelButton.gameObject.SetActive(true);
        settings.settingsButton.SetActive(true);
        settings.menuPanel.SetActive(true);
        settings.menuButton.SetActive(true);
    }

    void OnDisable()
    {
        openInfoPanelButton.selectEntered.RemoveAllListeners();
        closeInfoPanelButton.selectEntered.RemoveAllListeners();
    }
}
