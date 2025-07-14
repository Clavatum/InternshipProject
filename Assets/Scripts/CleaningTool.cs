using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CleaningTool : MonoBehaviour
{
    public WindowStateMachine windowStateMachine;

    private List<Vector2Int> usableBrushPixelPositions = new List<Vector2Int>();
    private Coroutine currentCoroutine;

    [SerializeField] private Texture2D brush;
    [SerializeField] private float rayLength;
    [SerializeField] private float cleaningCooldown = 0.05f;

    private int brushHalfWidth;
    private int brushHalfHeight;

    private bool isCleaningActive = false;
    public bool IsContinuous;
    private bool isCompleted = false;

    void Awake()
    {
        CacheEffectiveBrushPixels();
    }

    void Start()
    {
        brushHalfWidth = brush.width / 2;
        brushHalfHeight = brush.height / 2;
    }

    public void StartConvert()
    {
        if (!isCleaningActive)
        {
            if (IsContinuous)
            {
                Debug.Log("continuous tool usage started");
                isCleaningActive = true;
                currentCoroutine = StartCoroutine(ContinuousCleaning());
                return;
            }
            Debug.Log("not continuous tool usage started");
            Convert();
        }
    }

    private IEnumerator ContinuousCleaning()
    {
        while (isCleaningActive)
        {
            Convert();
            yield return new WaitForSeconds(cleaningCooldown);
        }
    }

    public void StopConvert()
    {
        if (isCleaningActive && currentCoroutine != null)
        {
            Debug.Log("continuous tool usage stopped");
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
            isCleaningActive = false;
        }
    }

    private void CacheEffectiveBrushPixels()
    {
        Color[] brushPixels = brush.GetPixels();
        int width = brush.width;

        for (int y = 0; y < brush.height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int index = y * width + x;
                Color pixel = brushPixels[index];
                if (IsBlackEnough(pixel))
                {
                    usableBrushPixelPositions.Add(new Vector2Int(x, y));
                }
            }
        }
    }

    private void Convert()
    {
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength))
        {
            Debug.Log("No hit detected.");
            return;
        }

        Window window = hit.transform.GetComponentInChildren<Window>();
        if (window == null)
        {
            Debug.Log("Hit object is not a window.");
            return;
        }

        float convertedPercentage = window.CalculateConvertedPercentage();

        if (!window.CanUseTool(this) || convertedPercentage == 100f)
        {
            Debug.Log("Cleaning not allowed or already 100% clean.");
            return;
        }

        isCleaningActive = true;
        Debug.Log("Cleaning started");

        Vector2 textureCoord = hit.textureCoord;
        Texture2D mask = (Texture2D)window.CopyOfMaterialToWorkOn.GetTexture("_Mask");
        Vector2Int hitPoint = new Vector2Int(
            (int)(textureCoord.x * mask.width),
            (int)(textureCoord.y * mask.height)
        );

        int maskWidth = mask.width;
        int maskHeight = mask.height;
        Vector2 brushCenter = new Vector2(brushHalfWidth, brushHalfHeight);
        Vector2 pivot = brushCenter;

        float angleDegrees = transform.parent.eulerAngles.z;
        float angleRad = angleDegrees * Mathf.Deg2Rad;

        bool pixelsChanged = false;
        Color clear = Color.clear;

        foreach (var brushPos in usableBrushPixelPositions)
        {
            Vector2 rotated = RotatePoint(brushPos, angleRad, pivot);
            Vector2Int rotatedPos = new Vector2Int(Mathf.RoundToInt(rotated.x), Mathf.RoundToInt(rotated.y));

            int targetX = hitPoint.x - brushHalfWidth + rotatedPos.x;
            int targetY = hitPoint.y - brushHalfHeight + rotatedPos.y;

            if (targetX < 0 || targetX >= maskWidth || targetY < 0 || targetY >= maskHeight)
                continue;

            mask.SetPixel(targetX, targetY, clear);
            pixelsChanged = true;
        }

        if (pixelsChanged)
        {
            mask.Apply();
            Debug.Log(convertedPercentage = window.CalculateConvertedPercentage());
        }

        if (convertedPercentage == 100f)
        {
            isCompleted = true;
            window.ChangeMaterial();

            if (window.NextState != null)
            {
                windowStateMachine.ChangeState(window.NextState);
                Debug.Log("State advanced to: " + window.NextState.StateName);
            }
            else
            {
                Debug.Log("Final state reached: " + window.StateName);
            }
            Debug.Log("Window fully cleaned. State changed.");
        }

        isCleaningActive = false;
    }

    private Vector2 RotatePoint(Vector2 point, float angleRad, Vector2 pivot)
    {
        Vector2 dir = point - pivot;
        float cos = Mathf.Cos(angleRad);
        float sin = Mathf.Sin(angleRad);
        Vector2 rotated = new Vector2(
            dir.x * cos - dir.y * sin,
            dir.x * sin + dir.y * cos
        );
        return rotated + pivot;
    }


    public static bool IsBlackEnough(Color color, float threshold = 0.1f) => color.r <= threshold && color.g <= threshold && color.b <= threshold;
}
