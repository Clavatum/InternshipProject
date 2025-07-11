using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CleaningTool : MonoBehaviour
{
    public WindowStateMachine windowStateMachine;

    private List<(Color color, Vector2Int position)> usableBrushPixels = new List<(Color color, Vector2Int position)>();

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
        CacheUsableBrushPixels();
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
                StartCoroutine(ContinuousCleaning());
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
        if (isCleaningActive)
        {
            Debug.Log("continuous tool usage stopped");
            StopCoroutine(ContinuousCleaning());
            return;
        }
        Debug.Log("continuous tool usage stopped");
    }


    private void CacheUsableBrushPixels()
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
                    usableBrushPixels.Add((pixel, new Vector2Int(x, y)));
                }
            }
        }
    }

    private void Convert()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength))
        {
            Window window = hit.transform.GetComponent<Window>();
            if (window == null)
            {
                Debug.Log("no window");
                return;
            }

            if (!window.CanUseTool(this) || window.CalculateConvertedPercentage() == 100f)
            {
                Debug.Log("zort");
                Debug.Log(window.CalculateConvertedPercentage());
                return;
            }

            Debug.Log("Cleaning");
            isCleaningActive = true;

            Vector2 textureCoord = hit.textureCoord;
            Texture2D mask = window.MaskOfMaterialToWorkOn;

            int pixelX = (int)(textureCoord.x * mask.width);
            int pixelY = (int)(textureCoord.y * mask.height);

            int startX = Mathf.Clamp(pixelX - brushHalfWidth, 0, mask.width - 1);
            int startY = Mathf.Clamp(pixelY - brushHalfHeight, 0, mask.height - 1);
            int endX = Mathf.Clamp(pixelX + brushHalfWidth, 0, mask.width - 1);
            int endY = Mathf.Clamp(pixelY + brushHalfHeight, 0, mask.height - 1);

            Color[] startPixels = window.StartOfMaterialToWorkOn.GetPixels(startX, startY, endX - startX + 1, endY - startY + 1);

            bool pixelsChanged = false;

            for (int y = startY; y <= endY; y++)
            {
                for (int x = startX; x <= endX; x++)
                {
                    int brushX = x - (pixelX - brushHalfWidth);
                    int brushY = y - (pixelY - brushHalfHeight);

                    if (brushX < 0 || brushX >= brush.width || brushY < 0 || brushY >= brush.height)
                        continue;

                    int dirtIndex = (y - startY) * (endX - startX + 1) + (x - startX);

                    startPixels[dirtIndex] = Color.clear;
                    pixelsChanged = true;
                }
            }
            if (pixelsChanged)
            {
                mask.SetPixels(startX, startY, endX - startX + 1, endY - startY + 1, startPixels);
                mask.Apply();
            }
            if (window.CalculateConvertedPercentage() == 100f)
            {
                isCompleted = true;
                window.ChangeMaterial();
                windowStateMachine.ChangeState(window.NextState);
                Debug.Log("current state completed, changing state...");
            }
            isCleaningActive = false;
        }
        else
        {
            Debug.Log(hit.transform.gameObject.name);
        }
    }

    public static bool IsBlackEnough(Color color, float threshold = 0.1f) => color.r <= threshold && color.g <= threshold && color.b <= threshold;
}
