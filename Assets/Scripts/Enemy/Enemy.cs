using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    Straight,
    Circle,
}

public class Enemy : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] MovementType movementType = MovementType.Straight;
    [SerializeField] protected float speed;

    [Header("Straight Movement")]
    [SerializeField] protected List<Transform> points = new List<Transform>();
    [SerializeField] private float distanceOffset;

    [Header("Circle Movement")]
    [SerializeField] private float radius = 2.0f;
    [SerializeField] private bool clockwise = false;


    // State variables
    protected Vector3 originalPosition;
    protected int index = 0;

    // Cached components
    private Animator anim;
    private Rigidbody2D rb2d;

    void Start()
    {
        originalPosition = transform.position;

        anim = GetComponent<Animator>();
        rb2d = GetComponent<Rigidbody2D>();

        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
        // anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
        // Choose a random frame (there are 8 frames)
        anim.playbackTime = Random.Range(0, 7) * (state.length / 8f);
    }

    void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (movementType == MovementType.Straight)
        {
            MoveStraight();
        }
        else if (movementType == MovementType.Circle)
        {
            MoveCircle();
        }
    }

    protected void MoveStraight()
    {
        // Ensure there are points to move to
        if (points.Count == 0) return;

        // Update velocity to move towards the target position from the current position
        Vector3 targetPosition = points[index].transform.position;
        Vector3 currentPosition = transform.position;
        Vector3 normDisplacement = (targetPosition - currentPosition).normalized;
        rb2d.velocity = normDisplacement * speed;

        if (Vector2.Distance(transform.position, points[index].transform.position) < distanceOffset)
        {
            index = (index + 1) % points.Count;
        }
    }

    private void MoveCircle()
    {
        // Get the tangent velocity vector at the current position. Use vector-cross product to achieve this.
        Vector3 crossVector = clockwise ? Vector3.forward : Vector3.back;
        Vector3 offset = transform.position - originalPosition;
        Vector3 normTangent = Vector3.Cross(offset, crossVector).normalized;
        Vector3 tangentVelocity = normTangent * speed;

        // Get the correction vector. This is to maintain the set radius (distance from the origin)
        bool noOffset = offset.magnitude == 0;
        Vector3 radiusCorrectionOffset = noOffset ? Vector3.right : offset;
        Vector3 targetPosition = originalPosition + (radiusCorrectionOffset.normalized * radius);
        Vector3 correctionVelocity = targetPosition - transform.position;

        // Update velocity
        rb2d.velocity = tangentVelocity + correctionVelocity;
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerMovement>().Die();
    }

}
