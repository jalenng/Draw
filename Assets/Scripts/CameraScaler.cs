using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScaler : MonoBehaviour
{
    public float aspectRatio = 1.5f; // 3:2

    private Camera camera;

    private int lastScreenWidth = -1;
    private int lastScreenHeight = -1;

    void Start()
    {
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        int screenWidth = Screen.width; //px
        int screenHeight = Screen.height; //px
        
        bool screenSizeChanged = screenWidth != lastScreenWidth || screenHeight != lastScreenHeight;
        if (screenSizeChanged) {
            // Center viewport and set aspect ratio to 3:2
            int aspectRatioWidth = (int)(screenHeight * aspectRatio);

            float newWidth = (float)aspectRatioWidth / screenWidth;
            float newX = (1f - newWidth) / 2;

            camera.rect = new Rect(newX, 0, newWidth, 1);

            // Remember width and height to track changes
            lastScreenWidth = screenWidth;
            lastScreenHeight = screenHeight;
        }
    }

    // Because we scale the camera viewport to be smaller than the screen,
    // this creates black bars on the screen which don't get refreshed.
    // So we need to manually clear them.
    void OnPreCull()
    {
        // Remember viewport rect
        Rect oldViewportRect = camera.rect;
        
        // New viewport rect to clear the whole screen
        Rect newViewportRect = new Rect(0, 0, 1, 1);
 
        // Clear the pixels with black
        camera.rect = newViewportRect;
        GL.Clear(true, true, Color.black);
       
        // Restore the previous viewport rect
        camera.rect = oldViewportRect;
    }
}
