using UnityEngine;

public class WebGLAspectRatio : MonoBehaviour
{
    //I chatgpt this shit ngl
    private float targetAspect = 16f / 9f;

    void Start()
    {
        AdjustAspectRatio();
    }

    void Update()
    {
        AdjustAspectRatio();
    }

    void AdjustAspectRatio()
    {
        // Current screen aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Calculate scaling factor
        float scaleHeight = windowAspect / targetAspect;

        Camera mainCamera = Camera.main;

        if (scaleHeight < 1.0f) // Letterbox
        {
            Rect rect = mainCamera.rect;

            rect.width = 1.0f;
            rect.height = scaleHeight;
            rect.x = 0;
            rect.y = (1.0f - scaleHeight) / 2.0f;

            mainCamera.rect = rect;
        }
        else // Pillarbox
        {
            float scaleWidth = 1.0f / scaleHeight;

            Rect rect = mainCamera.rect;

            rect.width = scaleWidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scaleWidth) / 2.0f;
            rect.y = 0;

            mainCamera.rect = rect;
        }
    }
}
