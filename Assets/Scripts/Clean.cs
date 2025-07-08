using System;
using UnityEngine;

public class Clean : MonoBehaviour
{
    [SerializeField] private Texture2D brush;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private float rayLength = 0.3f;
    [SerializeField] private bool isEquipped = false;

    private void Update()
    {
        if (isEquipped) { CleanGlass(); }
    }

    public void SetEquip()
    {
        isEquipped = !isEquipped;
    }

    public void CleanGlass()
    {
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, rayLength))
        {
            if (hit.collider.GetComponent<Window>() is not Window window)
            {
                return;
            }

            Vector2 textureCoord = hit.textureCoord;

            int pixelX = (int)(textureCoord.x * window.TemplateDirtMask.width);
            int pixelY = (int)(textureCoord.y * window.TemplateDirtMask.height);

            for (int x = 0; x < brush.width; x++)
            {
                for (int y = 0; y < brush.height; y++)
                {
                    int targetX = pixelX - (brush.width / 2) + x;
                    int targetY = pixelY - (brush.height / 2) + y;

                    if (targetX < 0 || targetX >= window.TemplateDirtMask.width || targetY < 0 || targetY >= window.TemplateDirtMask.height)
                        continue;

                    Color brushPixel = brush.GetPixel(y, x);

                    bool isBlack = brushPixel.r < 0.01f && brushPixel.g < 0.01f && brushPixel.b < 0.01f;

                    if (isBlack)
                    {
                        Color dirtMaskPixel = window.TemplateDirtMask.GetPixel(targetX, targetY);
                        window.TemplateDirtMask.SetPixel(targetX, targetY, new Color(0, dirtMaskPixel.g * brushPixel.g, 0));
                    }
                }
            }
            window.TemplateDirtMask.Apply();
        }
        Debug.DrawRay(rayOrigin.position, rayOrigin.forward * rayLength, Color.red);
    }


}
