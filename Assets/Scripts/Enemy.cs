using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private int index = 0;
    private float wait;
    [SerializeField] private List<Transform> points = new List<Transform>();
    [SerializeField] private float initWait;
    [SerializeField] private float distanceOffset;
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        Move();
    }
    
    private void Move()
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
            other.gameObject.GetComponent<PlayerMovement>().Die();
    }

}
