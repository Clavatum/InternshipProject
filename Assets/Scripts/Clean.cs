using UnityEngine;
using System.Collections;

public class Clean : MonoBehaviour
{
    [Header("Brush Settings")]
    [SerializeField] private Texture2D brush;
    [SerializeField][Range(0.1f, 1f)] private float brushStrength = 0.5f;
    [SerializeField] private float cleaningCooldown = 0.05f;
    [SerializeField] private float rayLength = 0.3f;

    public string ToolName;
    public bool IsContinuous;
    [Range(0f, 1f)] public float minCleanliness;
    [Range(0f, 1f)] public float maxCleanliness;
    //public AudioClip UseSound;

    private Coroutine cleaningRoutine;
    private Color[] brushPixels;
    private int brushHalfWidth;
    private int brushHalfHeight;
    private bool isCleaningActive;

    private void Awake()
    {
        if (brush != null)
        {
            brushPixels = brush.GetPixels();
            brushHalfWidth = brush.width / 2;
            brushHalfHeight = brush.height / 2;
        }
    }

    public void StartCleaning()
    {
        if (IsContinuous)
        {
            if (!isCleaningActive)
            {
                isCleaningActive = true;
                cleaningRoutine = StartCoroutine(ContinuousCleaning());
            }
        }
        else
        {
            CleanGlass();
        }
    }

    public void StopCleaning()
    {
        if (isCleaningActive)
        {
            if (cleaningRoutine != null)
            {
                StopCoroutine(cleaningRoutine);
            }
            isCleaningActive = false;
        }
    }

    private IEnumerator ContinuousCleaning()
    {
        while (isCleaningActive)
        {
            CleanGlass();
            yield return new WaitForSeconds(cleaningCooldown);
        }
    }

    public bool CanUseOnWindow(Window window)
    {
        float cleanliness = window.CurrentCleanliness;
        return cleanliness >= minCleanliness && cleanliness <= maxCleanliness;
    }

    private void CleanGlass()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, rayLength))
        {
            Window window = hit.collider.GetComponent<Window>();
            if (window == null || window.TemplateDirtMask == null) return;

            if (!CanUseOnWindow(window) || !window.CanUseTool(this))
            {
                Debug.Log(CanUseOnWindow(window));
                return;
            }

            ProcessCleaning(hit, window, this);
        }
    }

    private void ProcessCleaning(RaycastHit hit, Window window, Clean tool)
    {
        Debug.Log("cleaning");
        Vector2 textureCoord = hit.textureCoord;
        Texture2D dirtMask = window.TemplateDirtMask;

        int pixelX = (int)(textureCoord.x * dirtMask.width);
        int pixelY = (int)(textureCoord.y * dirtMask.height);

        int startX = Mathf.Clamp(pixelX - brushHalfWidth, 0, dirtMask.width - 1);
        int startY = Mathf.Clamp(pixelY - brushHalfHeight, 0, dirtMask.height - 1);
        int endX = Mathf.Clamp(pixelX + brushHalfWidth, 0, dirtMask.width - 1);
        int endY = Mathf.Clamp(pixelY + brushHalfHeight, 0, dirtMask.height - 1);

        Color[] dirtPixels = dirtMask.GetPixels(startX, startY, endX - startX + 1, endY - startY + 1);
        bool pixelsChanged = false;

        for (int y = startY; y <= endY; y++)
        {
            for (int x = startX; x <= endX; x++)
            {
                int brushX = x - (pixelX - brushHalfWidth);
                int brushY = y - (pixelY - brushHalfHeight);

                if (brushX < 0 || brushX >= brush.width || brushY < 0 || brushY >= brush.height)
                    continue;

                int brushIndex = brushY * brush.width + brushX;
                int dirtIndex = (y - startY) * (endX - startX + 1) + (x - startX);

                if (IsBlackEnough(brushPixels[brushIndex]))
                {
                    dirtPixels[dirtIndex] = ProcessPixel(dirtPixels[dirtIndex], brushPixels[brushIndex]);
                    pixelsChanged = true;
                }
            }
        }

        if (pixelsChanged)
        {
            dirtMask.SetPixels(startX, startY, endX - startX + 1, endY - startY + 1, dirtPixels);
            dirtMask.Apply();
            window.UpdateCleaningState(tool);
            //PlayToolSound(tool);
        }
    }

    private Color ProcessPixel(Color dirtPixel, Color brushPixel)
    {
        dirtPixel.r = 0f;
        dirtPixel.g *= Mathf.Lerp(1f, brushPixel.g, brushStrength);
        return dirtPixel;
    }

    private bool IsBlackEnough(Color color) => (color.r + color.g + color.b) < 0.3f;

    /*private void PlayToolSound(Clean tool)
    {
        if (tool.useSound != null)
        {
            AudioSource.PlayClipAtPoint(tool.useSound, transform.position);
        }
    }*/

    private void OnDisable()
    {
        StopCleaning();
    }
}