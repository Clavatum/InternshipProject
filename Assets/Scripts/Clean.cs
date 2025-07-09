using UnityEngine;

public class Clean : MonoBehaviour
{
    [Header("Brush Settings")]
    [SerializeField] private Texture2D brush;
    [SerializeField][Range(0.1f, 1f)] private float brushStrength = 0.5f;

    [Header("Raycast Settings")]
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float rayLength = 0.3f;

    [Header("Performance Settings")]
    [SerializeField] private int maxCleaningPerFrame = 3;
    [SerializeField] private float cleaningCooldown = 0.05f;

    [Header("State Control")]
    [SerializeField] private ToolType currentTool;
    [SerializeField] private float requiredCleanliness = 0.85f;

    [SerializeField] private bool isEquipped = false;

    private float lastCleaningTime;
    private int cleaningCountThisFrame;
    private Color[] brushPixels;
    private int brushHalfWidth;
    private int brushHalfHeight;

    public enum ToolType
    {
        WaterSpray,
        ChemicalSpray,
        Squeegee,
        Towel
    }

    private void Awake()
    {
        if (brush != null)
        {
            brushPixels = brush.GetPixels();
            brushHalfWidth = brush.width / 2;
            brushHalfHeight = brush.height / 2;
        }
    }

    private void Update()
    {
        cleaningCountThisFrame = 0;

        if (isEquipped && Time.time - lastCleaningTime >= cleaningCooldown)
        {
            CleanGlass();
        }
    }

    public void SetEquip(bool equipped)
    {
        isEquipped = equipped;
    }

    public void SetCurrentTool(ToolType tool)
    {
        currentTool = tool;
    }

    private void CleanGlass()
    {
        if (brush == null || cleaningCountThisFrame >= maxCleaningPerFrame) return;

        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, rayLength))
        {
            Window window = hit.collider.GetComponent<Window>();
            if (window == null || window.TemplateDirtMask == null) return;

            if (!CanUseTool(window))
            {
                Debug.Log($"Cannot use {currentTool} in current state");
                return;
            }

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
                        Color dirtPixel = dirtPixels[dirtIndex];
                        dirtPixel.r = 0f;
                        dirtPixel.g *= Mathf.Lerp(1f, brushPixels[brushIndex].g, brushStrength);
                        dirtPixels[dirtIndex] = dirtPixel;
                        pixelsChanged = true;
                    }
                }
            }

            if (pixelsChanged)
            {
                dirtMask.SetPixels(startX, startY, endX - startX + 1, endY - startY + 1, dirtPixels);
                dirtMask.Apply();
                cleaningCountThisFrame++;
                lastCleaningTime = Time.time;

                window.UpdateCleaningState(currentTool);
            }
        }
    }

    private bool CanUseTool(Window window)
    {
        float cleanliness = window.CalculateCleanliness();

        switch (currentTool)
        {
            case ToolType.WaterSpray:
                return cleanliness < 0.3f;

            case ToolType.ChemicalSpray:
                return cleanliness >= 0.3f && cleanliness < 0.6f;

            case ToolType.Squeegee:
                return cleanliness >= 0.6f && cleanliness < requiredCleanliness;

            case ToolType.Towel:
                return cleanliness >= requiredCleanliness;

            default:
                return false;
        }
    }

    private bool IsBlackEnough(Color color)
    {
        return (color.r * color.r + color.g * color.g + color.b * color.b) < 0.01f;
    }

    private void OnDrawGizmos()
    {
        if (rayOrigin != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(rayOrigin.position, rayOrigin.forward * rayLength);
        }
    }
}