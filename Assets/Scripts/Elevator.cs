using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 10f;

    [Header("Clamp Settings")]
    [SerializeField] private float minX;
    [SerializeField] private float maxX;
    [SerializeField] private float minY;
    [SerializeField] private float maxY;

    [Header("Sound Settings")]
    [SerializeField] AudioSource audioSource;
    [SerializeField] private AudioClip elevatorStartClip;
    [SerializeField] private AudioClip elevatorEndClip;

    void Awake()
    {
        audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void ClampPosition()
    {
        Vector3 position = transform.position;
        position.x = Mathf.Clamp(position.x, minX, maxX);
        position.y = Mathf.Clamp(position.y, minY, maxY);
        transform.position = position;
    }

    public void PlayStartClip()
    {
        audioSource.loop = true;
        audioSource.clip = elevatorStartClip;
        audioSource.Play();
    }

    public void PlayEndClip()
    {
        audioSource.loop = false;
        audioSource.clip = elevatorEndClip;
        audioSource.Play();
    }

    public void MoveElevatorUp()
    {
        transform.position = Vector2.MoveTowards(transform.position, Vector2.up, moveSpeed * Time.deltaTime);
        ClampPosition();

    }
    public void MoveElevatorDown()
    {
        transform.position = Vector2.MoveTowards(transform.position, Vector2.down, moveSpeed * Time.deltaTime);
        ClampPosition();
    }
    public void MoveElevatorLeft()
    {
        transform.position = Vector2.MoveTowards(transform.position, Vector2.left, moveSpeed * Time.deltaTime);
        ClampPosition();
    }
    public void MoveElevatorRight()
    {
        transform.position = Vector2.MoveTowards(transform.position, Vector2.right, moveSpeed * Time.deltaTime);
        ClampPosition();
    }
}
