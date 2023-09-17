using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Events;

public class DrawingCanvas : MonoBehaviour
{
    // Cofiguration parameters
    [Header("Line")]
    [SerializeField] Line linePrefab;

    [Tooltip("The parent object that will hold all the drawn lines")]
    [SerializeField] GameObject linesParent;

    [Header("Line Drawing Settings")]
    [SerializeField] LayerMask cantDrawOverLayer;
    [SerializeField] float minLineLength = 0.5f;
    [Range(0, 50)]
    [SerializeField] float maxTotalLineLength = 10f;
    
    [Header("SFX Settings")]
    [SerializeField] private float sfxIntensityLogBase = 5f;
    [SerializeField] private float sfxIntensityToPitchScale = 5f;
    [SerializeField] private float sfxIntensityToVolumeScale = 5f;

    // State variables
    Vector3 lastPointPos;
    float currentLineLength = 0f;
    float totalDrawnLineLength = 0f;

    // Actions
    [SerializeField] private UnityEvent onReset;
    [SerializeField] private UnityEvent onEndDraw;

    // Cached components
    Line currentLine;
    Camera cam;
    AudioSource audioSource;
    AchievementUnlocker achievementUnlocker;

    void Start()
    {
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();
        achievementUnlocker = GetComponent<AchievementUnlocker>();
    }

    void FixedUpdate()
    {
        // If current line exists, draw
        if (currentLine != null)
        {
            Draw();
        }
    }

    void Update()
    {
        // If the mouse button is released, end drawing
        if (Input.GetMouseButtonUp(0))
            EndDraw();
    }

    // Begins adding points to a new line. 
    // This function is invoked when the player clicks inside the drawing area's collider.
    public void BeginDraw()
    {
        if (CanDraw())
        {
            audioSource.Play();
            // Instantiate a new line
            currentLine = Instantiate(linePrefab, this.transform).GetComponent<Line>();

            // Make it a child of the drawer's "Lines" (for organization)
            currentLine.transform.SetParent(linesParent.transform);

            currentLineLength = 0;
        }
    }

    // Adds a point to the line
    void Draw()
    {
        if (CanDraw())
        {
            // Account for canvas position
            Vector2 pointPosition = GetMousePosInWorldSpace();
            currentLine.AddPoint(pointPosition);

            // Calculate distance between the points and update ink counters
            bool isFirstPoint = currentLine.pointsCount == 1;
            float distance = isFirstPoint ? 0 : Vector2.Distance(lastPointPos, pointPosition);  // If it's the line's first point, distance is 0.
            totalDrawnLineLength += distance;
            currentLineLength += distance;

            // Update last point position
            lastPointPos = pointPosition;

            // Play and update SFX
            if (distance > 0)
            {
                if (!audioSource.isPlaying) {
                    audioSource.Play();
                }
                float sfxIntensity = Mathf.Log(1 + distance, sfxIntensityLogBase);
                audioSource.pitch = 1 + (sfxIntensity * sfxIntensityToPitchScale);
                audioSource.volume = sfxIntensity * sfxIntensityToVolumeScale;
            }
            else {
                audioSource.pitch = 0;
                audioSource.volume = 0;
            }
        }
        else
        {
            EndDraw();
        }
    }

    // Stops adding points to the line to terminate the line
    void EndDraw()
    {
        if (currentLine != null)
        {
            // If the currently drawn line is too short, ignore it.
            if (currentLineLength < minLineLength)
                Destroy(currentLine.gameObject);

            // Otherwise...
            else
            {
                onEndDraw.Invoke();

                // Apply the line's body type property.
                // This makes the line either static or dynamic, depending on whether it's affected by gravity.
                currentLine.ApplyBodyTypeProperty();
                // Then give the player the related achievement.
                achievementUnlocker.SetAchievement();
            }

            // Make currentLine null. We're done with this line.
            currentLine = null;
        }

        // Stop SFX
        audioSource.Stop();
    }

    // Returns the prefab of the line that will be drawn
    public Line GetLinePrefab()
    {
        return linePrefab;
    }

    // Deletes all the drawn lines and resets the ink counter
    public void Reset()
    {
        // Reset the ink counter
        totalDrawnLineLength = 0;

        // Delete all Lines pertaining to the LineParent
        foreach (Transform child in linesParent.transform.GetComponentInChildren<Transform>())
            GameObject.Destroy(child.gameObject);

        onReset.Invoke();
    }

    // Helper function to retrieve the mouse position in world space
    private Vector2 GetMousePosInWorldSpace()
    {
        Vector2 canvasPosition = transform.position;
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);

        return mousePosition - canvasPosition;
    }

    // Helper function that checks conditions for drawing
    public bool CanDraw()
    {
        Vector2 mousePosition = cam.ScreenToWorldPoint(Input.mousePosition);
        float lineRadius = linePrefab.GetWidth() / 2f;

        // Check for collision with cantDrawOverLayer
        RaycastHit2D hitCantDraw = Physics2D.CircleCast(mousePosition, lineRadius, Vector2.zero, 1f, cantDrawOverLayer);

        // Check for collision with the drawing area
        RaycastHit2D hitDrawingArea = Physics2D.CircleCast(mousePosition, lineRadius, Vector2.zero, 1f, LayerMask.GetMask("Drawing Area"));

        // Check if there is enough ink
        bool inkRemaining = totalDrawnLineLength < maxTotalLineLength;

        return !hitCantDraw && hitDrawingArea && inkRemaining;
    }

    // Returns the ratio of ink used to the maximum ink
    public float GetInkRatio()
    {
        return totalDrawnLineLength / maxTotalLineLength;
    }

}
