using UnityEngine;

public class Elevator : MonoBehaviour
{
    private float moveSpeed = 0.5f;
    private bool isMovingUpward;
    private bool isMovingLeft;
    private bool isMovingHorizontally = false;
    private bool isMovingVertically = false;

    [Header("Boundries")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header("Sound Settings")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip elevatorStartClip;
    [SerializeField] private AudioClip elevatorEndClip;

    void Update()
    {
        if (isMovingHorizontally)
        {
            MoveElevatorHorizontally();
        }

        if (isMovingVertically)
        {
            MoveElevatorVertically();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<CleaningTool>() != null)
        {
            CleaningTool cleaningTool = other.GetComponent<CleaningTool>();
            cleaningTool.ResetTransformSmoothly();
        }
    }

    #region - Audio -

    public void PlayStartClip()
    {
        audioSource.Stop();
        audioSource.loop = true;
        audioSource.clip = elevatorStartClip;
        audioSource.Play();
    }

    public void PlayEndClip()
    {
        audioSource.Stop();
        audioSource.loop = false;
        audioSource.clip = elevatorEndClip;
        audioSource.Play();
    }

    #endregion

    #region - Movement -

    private void ClampPosition()
    {
        Vector3 position = transform.localPosition;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        transform.localPosition = position;
    }

    public void SetHorizontalMovement(bool isMovingLeft)
    {
        this.isMovingLeft = isMovingLeft;
        isMovingHorizontally = true;
        PlayStartClip();
    }

    public void StopHorizontalMovement()
    {
        isMovingHorizontally = false;
        PlayEndClip();
    }

    public void SetVerticalMovement(bool isMovingUpward)
    {
        this.isMovingUpward = isMovingUpward;
        isMovingVertically = true;
        PlayStartClip();
    }

    public void StopVerticalMovement()
    {
        isMovingVertically = false;
        PlayEndClip();
    }

    private void MoveElevatorVertically()
    {
        transform.Translate(moveSpeed * Time.deltaTime * (isMovingUpward ? Vector3.up : Vector3.down));
        ClampPosition();
    }

    private void MoveElevatorHorizontally()
    {
        transform.Translate(moveSpeed * Time.deltaTime * (isMovingLeft ? Vector3.left : Vector3.right));
        ClampPosition();
    }
    #endregion
}