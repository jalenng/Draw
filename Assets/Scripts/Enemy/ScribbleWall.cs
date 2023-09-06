using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random=UnityEngine.Random;

public class ScribbleWall : RespawnInterface
{
    [SerializeField] Rigidbody2D rb2d;
    [SerializeField] private float initWait;
    [SerializeField] private float speed;

    [Header("Straight Movement")]
    [SerializeField] private List<Transform> points = new List<Transform>();
    [SerializeField] private float distanceOffset;
    
    private int index = 0;
    private float wait;
    private Animator anim;
    Vector3 respawnPos;
    bool playerDead = false;
    void Awake()
    {
        respawnPos = transform.position;
        Animator anim = GetComponent<Animator>();
        AnimatorStateInfo state = anim.GetCurrentAnimatorStateInfo(0);//could replace 0 by any other animation layer index
        anim.Play(state.fullPathHash, -1, Random.Range(0f, 1f));
    }
    void Update() {
        if(!playerDead) MoveStraight();
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
    public void SetRespawnPos(Vector3 pos) 
    {
        respawnPos = pos;
    }
    private void OnCollisionEnter2D(Collision2D other) 
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<PlayerMovement>().Die();
            playerDead = true;
            StartCoroutine(Respawn(1f));
        }
    }
    public override void StartRespawn() {
        StartCoroutine(Respawn(0f));
    }
    IEnumerator Respawn(float wait)
    {
        yield return new WaitForSeconds(wait);
        transform.position = respawnPos;
        playerDead = false;
    }
}
