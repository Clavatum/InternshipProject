using TMPro;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class InGameStatsUI : MonoBehaviour
{
    private GameManager gameManager;
    private Heist heist;
    private OpenedWindowTrigger currentOpenedWindow;
    private Teleport teleport;
    private XRSimpleInteractable apartmentButtonInteractable;

    [Header("Object References")]
    [SerializeField] private TextMeshProUGUI feedbackText;
    [SerializeField] private TextMeshProUGUI scoreText;
    public TextMeshProUGUI totalTimeLeftText;
    public TextMeshProUGUI heistTimeLeftText;
    [SerializeField] private AudioSource SFXAudioSource;
    [SerializeField] private AudioClip errorSFX;
    [SerializeField] private AudioClip successSFX;
    [SerializeField] private AudioClip endGameSFX;
    [SerializeField] private AudioClip gameWonSFX;
    [SerializeField] private GameObject apartmentButton;
    public GameObject gameOverPanel;

    [SerializeField] private float messageDuration = 2f;

    public bool isEnteredApartment;

    void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        teleport = FindAnyObjectByType<Teleport>();
        heist = FindAnyObjectByType<Heist>();
    }

    public void SuccessFeedback(string message) => SetFeedbackText(message, Color.green, successSFX);
    public void ErrorFeedback(string message) => SetFeedbackText(message, Color.red, errorSFX);
    public void GameOverFeedback(string message) => SetFeedbackText(message, Color.red, endGameSFX);
    public void GameWonFeedback(string message) => SetFeedbackText(message, Color.green, gameWonSFX);

    private void SetFeedbackText(string message, Color color, AudioClip audioClip)
    {
        feedbackText.transform.gameObject.SetActive(true);
        feedbackText.text = message;
        feedbackText.color = color;
        SFXAudioSource.clip = audioClip;
        SFXAudioSource.PlayOneShot(audioClip);
        Invoke(nameof(ClearMessage), messageDuration);
    }

    public void UpdateScoreText(int score)
    {
        scoreText.text = $"Score: {score}";
    }

    public void ShowTimeLeft(TextMeshProUGUI timeLeftText, float timeLeft)
    {
        timeLeftText.text = $"Time Left: {(int)(timeLeft / 60):D2}:{(int)(timeLeft % 60):D2}";
    }

    private void ClearMessage()
    {
        feedbackText.text = "";
        feedbackText.transform.gameObject.SetActive(false);
    }

    private void SetOpenedWindowPanel(OpenedWindowTrigger openedWindowTrigger)
    {
        currentOpenedWindow = openedWindowTrigger;
        apartmentButtonInteractable = apartmentButton.GetComponent<XRSimpleInteractable>();
        float totalTime = gameManager.TotalTimeLeft < heist.totalHeistTime ? gameManager.TotalTimeLeft : heist.totalHeistTime;
        feedbackText.text = openedWindowTrigger.isInside ? $"Get out of the apartment" : $"Do you want to enter apartment and steal stuff?\n You have {totalTime} seconds";

        apartmentButtonInteractable.selectEntered.RemoveAllListeners();
        apartmentButtonInteractable.selectEntered.AddListener(zort => teleport.TeleportTo(currentOpenedWindow));
        apartmentButtonInteractable.selectEntered.AddListener(zort => heist.SetHeist());
        apartmentButtonInteractable.selectEntered.AddListener(zort => ResetOpenedWindowPanel());

        feedbackText.gameObject.SetActive(true);
        apartmentButton.SetActive(true);
    }

    private void ResetOpenedWindowPanel()
    {
        apartmentButton.SetActive(false);
        ClearMessage();
    }

    void OnEnable()
    {
        OpenedWindowTrigger.OnWindowEnter += SetOpenedWindowPanel;
        OpenedWindowTrigger.OnWindowExit += ResetOpenedWindowPanel;
    }

    void OnDisable()
    {
        OpenedWindowTrigger.OnWindowEnter -= SetOpenedWindowPanel;
        OpenedWindowTrigger.OnWindowExit -= ResetOpenedWindowPanel;
    }
}