using Unity.XR.CoreUtils;
using UnityEngine;

public class Apartment : MonoBehaviour
{
    private InGameStatsUI inGameStatsUI;
    private OpenedWindow openedWindow;

    [SerializeField] private Transform character;
    [SerializeField] private Transform characterTransformInElevator;
    [SerializeField] private Camera mainCamera;

    void Awake()
    {
        inGameStatsUI = FindAnyObjectByType<InGameStatsUI>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<XROrigin>() != null)
        {
        }
    }

    private void GetIntoApartment()
    {
        if (!Physics.Raycast(mainCamera.transform.position, transform.forward, out RaycastHit hit, 1f)) { return; }

        openedWindow = hit.transform.GetComponentInChildren<OpenedWindow>();
        if (openedWindow == null) { return; }

        inGameStatsUI.SetOpenedWindowPanel();
    }

    public void SetPosition()
    {
        character.transform.localPosition = inGameStatsUI.isEnteredApartment ? characterTransformInElevator.localPosition : openedWindow.characterTransformInApartment.localPosition;
        character.transform.localRotation = inGameStatsUI.isEnteredApartment ? characterTransformInElevator.localRotation : openedWindow.characterTransformInApartment.localRotation;
        inGameStatsUI.ResetOpenedWindowPanel();
        inGameStatsUI.isEnteredApartment = !inGameStatsUI.isEnteredApartment;
    }
}
