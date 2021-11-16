using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TargetGroupUpdater : MonoBehaviour
{
    // Configuration parameters
    [Header("Focus Area Detection")]
    [SerializeField] Transform playerTransform;
    
    [Header("Target Group Index")]
    [SerializeField] int dynamicTargetIndex = 0;

    [Header("Target Transition")]
    [Range(0, 2)] 
    [Tooltip("The higher the value, the smoother and longer the transition will be")]
    [SerializeField] float transitionSmoothTime = 0.7f;
    
    // Cached components and objects
    CinemachineTargetGroup ctg;
    FocusArea[] focusAreas;

    // State variables
    bool isTransitioning = false;
    bool isFocused = false;
    float currentWeight = 0f;
    float targetWeight = 0f;
    float weightVelocity = 0;

    void Start()
    {
        ctg = GetComponent<CinemachineTargetGroup>();

        // Find all the focus areas
        focusAreas = FindObjectsOfType<FocusArea>();
    }

    void FixedUpdate()
    {
        SearchClosestFocusArea();
        UpdateCurrentWeight();
    }

    void SearchClosestFocusArea()
    {
        Vector3 playerPosition = playerTransform.position;

        // Iterate through each drawing area to find the closest one
        foreach (FocusArea area in focusAreas)
        {
            Bounds bounds = area.GetComponent<SpriteRenderer>().bounds;

            // If the player is inside the bounds of the focus area,
            // focus on the focus area.
            if (bounds.Contains(playerPosition))
            {
                Transform target = area.transform;

                Vector2 size = bounds.size;
                float radius = Mathf.Max(size.x / 2, size.y / 2);

                StartCoroutine(Focus(target, radius));
                return;                   
            }
        }

        // If no focus area is found, unfocus.
        StartCoroutine(Unfocus());
    }

    // Update the current weight
    void UpdateCurrentWeight()
    {
        // Use smooth damp to get a transition value between the current and target weight
        currentWeight = Mathf.SmoothDamp(currentWeight, targetWeight, ref weightVelocity, transitionSmoothTime);

        // Add/subtract a very small amount to the weight to ensure it hits 0 or 1
        if (targetWeight == 1.0f)
            currentWeight += 0.0001f;
        else if (targetWeight == 0.0f)
            currentWeight -= 0.0001f;

        // Clamp the weight, then update it in the target group
        currentWeight = Mathf.Clamp01(currentWeight);
        ctg.m_Targets[dynamicTargetIndex].weight = currentWeight;
    }

    // Focus on a target
    IEnumerator Focus(Transform target, float radius)
    {
        // Ensure transitions do not happen concurrently 
        if (isTransitioning || isFocused) 
            yield break;
        isTransitioning = true;
        isFocused = true;

        // Add the target to the target group
        UpdateTarget(target, radius);

        // Update the target weight
        targetWeight = 1.0f;

        // Then wait for the current weight to reach the target weight.
        // When it does, the focusing animation is finished.
        yield return new WaitUntil(() => currentWeight >= targetWeight);

        isTransitioning = false;
    }

    // Unfocus from a target
    IEnumerator Unfocus()
    {
        // Ensure transitions do not happen concurrently 
        if (isTransitioning || !isFocused) 
            yield break;
        isTransitioning = true;
        isFocused = false;

        // Update the target weight
        targetWeight = 0.0f;

        // Wait for the current weight to reach the target weight.
        // When it does, the unfocusing animation is finished.
        yield return new WaitUntil(() => currentWeight <= targetWeight);

        // Then remove the target from the target group
        UpdateTarget(null, 0);

        isTransitioning = false;
    }

    // Update the target in the target group, as well as its radius    
    void UpdateTarget(Transform target, float radius) {
        ctg.m_Targets[dynamicTargetIndex].target = target;
        ctg.m_Targets[dynamicTargetIndex].radius = radius;
    }

}
