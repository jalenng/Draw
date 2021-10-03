using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    public GameObject linePrefab;
    public LayerMask cantDrawOverLayer;
    int cantDrawOverLayerIndex;

    [Space ( 30f )]
    public Gradient lineColor;
    public float linePointsMinDistance;
    public float lineWidth;

    Line currentLine;

    Camera cam;


    void Start ( ) {
        cam = Camera.main;
    }

    void Update ( ) {
        if ( Input.GetMouseButtonDown ( 0 ) )
            BeginDraw ( );

        if ( currentLine != null )
            Draw ( );

        if ( Input.GetMouseButtonUp ( 0 ) )
            EndDraw ( );
    }

    // Begin Draw ----------------------------------------------
    void BeginDraw ( ) {
        currentLine = Instantiate ( linePrefab, this.transform ).GetComponent <Line> ( );
        
        currentLine.SetLineColor ( lineColor );
        currentLine.SetPointsMinDistance ( linePointsMinDistance );
        currentLine.SetLineWidth ( lineWidth );

    }
    void Draw ( ) {
        Vector2 mousePosition = cam.ScreenToWorldPoint ( Input.mousePosition );
        RaycastHit2D hit = Physics2D.CircleCast ( mousePosition, lineWidth / 3f, Vector2.zero, 1f, cantDrawOverLayer );

        if ( hit )
            EndDraw ( );
        else
            currentLine.AddPoint ( mousePosition );
    }
    void EndDraw ( ) {
        if ( currentLine != null ) {
            if ( currentLine.pointsCount < 2 ) {
                Destroy ( currentLine.gameObject );
            } else {
                currentLine = null;
            }
        }
    }
}
