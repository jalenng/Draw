using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType {
    Straight,
    Circle,
}

public class Enemy : MonoBehaviour
{
    // Configuration parameters
    [SerializeField] MovementType movementType = MovementType.Straight;
    [SerializeField] private float initWait;
    [SerializeField] private float speed;

    [Header("Straight Movement")]
    [SerializeField] private List<Transform> points = new List<Transform>();
    [SerializeField] private float distanceOffset;

    [Header("Circle Movement")]
    [SerializeField] private float radius = 2.0f;

    // State variables
    private Vector3 originalPosition;
    private int index = 0;
    private float wait;

    void Start()
    {
        originalPosition = transform.position;
    }

    void Update()
    {
        Move();
    }
    
    private void Move()
    {
        if (movementType == MovementType.Straight) {
            MoveStraight();
        }
        else if (movementType == MovementType.Circle) {
            MoveCircle();
        }
    }

    private void MoveStraight() 
    {
        if (points.Count > 0)
        {
            transform.position = Vector2.MoveTowards(
                transform.position,
                points[index].transform.position,
                speed * Time.deltaTime);
            if (Vector2.Distance(transform.position, points[index].transform.position) < distanceOffset)
            {
                if (wait <= 0)
                {
                    index = (index + 1) % points.Count;
                    wait = initWait;
                }
                else
                    wait -= Time.deltaTime;
            }
        }
    }

    private void MoveCircle()
    {
        float scaledTime = Time.time * speed;
        Vector3 offsetVector = new Vector3(
            Mathf.Cos(scaledTime + initWait), 
            Mathf.Sin(scaledTime + initWait),
            0
        ) * radius;
        
        transform.position = originalPosition + offsetVector;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerMovement>().Die();
    }

}
