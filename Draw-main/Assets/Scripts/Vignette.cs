using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Vignette : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] Transform player;
    [SerializeField] float startX = 0.0f;
    [Range(0, 1)] [SerializeField] float startOpacity = 0.0f;
    [SerializeField] float endX = 100.0f;
    [Range(0, 1)] [SerializeField] float endOpacity = 1.0f;

    // Cached components
    Image image;

    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerPos = player.position;
        float opacity = Mathf.Lerp(startOpacity, endOpacity, (playerPos.x - startX) / (endX - startX));
        image.color = new Color(1, 1, 1, opacity);
    }
}
