using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningTool : MonoBehaviour
{
    [Header("Class References")]
    private WindowStateMachine windowStateMachine;
    private InGameStatsUI inGameStatsUI;

    private List<Vector2Int> usableBrushPixelPositions = new List<Vector2Int>();
    private Coroutine currentCoroutine;
    private Coroutine resetCoroutine;

    [Space, Header("Brush Settings")]
    [SerializeField] private Texture2D brush;
    private int brushHalfWidth;
    private int brushHalfHeight;

    [Header("Convertion Settings")]
    [SerializeField] private float rayLength;
    [SerializeField] private float convertedThreshhold = 98f;
    public bool IsContinuous;

    [Header("Tool Positioning Settings")]
    [SerializeField] private Transform originalToolTransform;
    [SerializeField] private float resetDuration;
    [SerializeField] private AnimationCurve easingCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private bool isCleaningActive = false;

    #region - Awake/Start -

    void Awake()
    {
        inGameStatsUI = FindAnyObjectByType<InGameStatsUI>();
        CacheEffectiveBrushPixels();
    }

    void Start()
    {
        initialPosition = originalToolTransform.localPosition;
        initialRotation = originalToolTransform.localRotation;
        brushHalfWidth = brush.width / 2;
        brushHalfHeight = brush.height / 2;
    }

    #endregion

    #region - Cleaning -

    public void StartCleaning()
    {
        if (!isCleaningActive)
        {
            if (IsContinuous)
            {
                isCleaningActive = true;
                currentCoroutine = StartCoroutine(ContinuousCleaning());
                return;
            }
            Clean();
            isCleaningActive = false;
        }
    }

    private IEnumerator ContinuousCleaning()
    {
        while (isCleaningActive)
        {
            Clean();
            yield return null;
        }
    }

    public void StopCleaning()
    {
        if (isCleaningActive && currentCoroutine != null)
        {
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

    private void Clean()
    {
        if (!Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength))
        {
            return;
        }

        WindowState windowState = hit.transform.GetComponentInChildren<WindowState>();
        if (windowState == null)
        {
            return;
        }

        windowStateMachine = hit.transform.GetComponentInChildren<WindowStateMachine>();

        float convertedPercentage = windowState.CalculateConvertedPercentage();
        Debug.Log("converted percentage: " + convertedPercentage);

        if (!windowState.CanUseTool(this))
        {
            inGameStatsUI.ErrorFeedback("Can't use this tool right now!");
            return;
        }

        isCleaningActive = true;

        Vector2 textureCoord = hit.textureCoord;
        Texture2D mask = (Texture2D)windowState.CopyOfMaterialToWorkOn.GetTexture("_Mask");
        Vector2Int hitPoint = new Vector2Int(
            (int)(textureCoord.x * mask.width),
            (int)(textureCoord.y * mask.height)
        );

        int maskWidth = mask.width;
        int maskHeight = mask.height;

        float toolRotation = transform.eulerAngles.z;

        float angleRad = -toolRotation * Mathf.Deg2Rad;
        Vector2 pivot = new Vector2(brushHalfWidth, brushHalfHeight);

        bool pixelsChanged = false;
        Color clear = Color.clear;

        foreach (var brushPos in usableBrushPixelPositions)
        {
            Vector2 rotated = RotatePoint(brushPos, angleRad, pivot);
            Vector2Int rotatedPos = new Vector2Int(
                Mathf.RoundToInt(rotated.x),
                Mathf.RoundToInt(rotated.y)
            );

            int targetX = hitPoint.x - brushHalfWidth + rotatedPos.y;
            int targetY = hitPoint.y - brushHalfHeight + rotatedPos.x;

            if (targetX < 0 || targetX >= maskWidth || targetY < 0 || targetY >= maskHeight)
                continue;

            mask.SetPixel(targetX, targetY, clear);
            int pixelIndex = targetY * maskWidth + targetX;
            windowState.pixels[pixelIndex] = clear;
            pixelsChanged = true;
        }

        if (pixelsChanged)
        {
            mask.Apply();
        }

        if (convertedPercentage >= convertedThreshhold)
        {
            windowStateMachine.ChangeState(windowState.NextState);
            windowState.ChangeMaterial();
            if (windowState.NextState == null)
            {
                inGameStatsUI.SuccessFeedback("Window fully cleared!");
                return;
            }
            inGameStatsUI.SuccessFeedback("Fully applied!");
        }
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

    #endregion

    #region - Tool Positioning -

    public void ResetTransformSmoothly()
    {
        if (resetCoroutine != null)
        {
            StopCoroutine(resetCoroutine);
        }
        resetCoroutine = StartCoroutine(SmoothReturn());
    }

    private IEnumerator SmoothReturn()
    {
        Vector3 startPosition = transform.parent.localPosition;
        Quaternion startRotation = transform.parent.localRotation;

        float elapsed = 0f;

        while (elapsed < resetDuration)
        {
            float t = elapsed / resetDuration;
            float curveT = easingCurve.Evaluate(t);

            transform.parent.localPosition = Vector3.Lerp(startPosition, initialPosition, curveT);
            transform.parent.localRotation = Quaternion.Slerp(startRotation, initialRotation, curveT);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.parent.localPosition = initialPosition;
        transform.parent.localRotation = initialRotation;
        resetCoroutine = null;
    }

    #endregion
}
