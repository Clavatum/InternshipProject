using TMPro;
using UnityEngine;

public class CleaningHUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _messageText;
    [SerializeField] private float _messageDuration = 2f;
    [SerializeField] private AudioSource _errorSound;

    public void ShowError(string message)
    {
        _messageText.text = message;
        _messageText.color = Color.red;
        _errorSound.Play();
        Invoke(nameof(ClearMessage), _messageDuration);
    }

    public void ShowSuccess(string message)
    {
        _messageText.text = message;
        _messageText.color = Color.green;
        Invoke(nameof(ClearMessage), _messageDuration);
    }

    private void ClearMessage() => _messageText.text = "";
}