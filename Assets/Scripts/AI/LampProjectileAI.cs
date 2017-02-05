using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Game;

public class LampProjectileAI : MonoBehaviour
{
    public Vector3 dir;
    private EnemyController _controller;
    public float speed;
    public float lifespan;
    public float damage;
    private float aliveStart;
    public GameObject proj;

    private Rigidbody2D rb;

    private void Start()
    {
        _controller = GetComponent<EnemyController>();
        rb = GetComponent<Rigidbody2D>();
        if (dir != null)
            rb.velocity = new Vector2(dir.x * speed, dir.y*speed);
        aliveStart = Time.time;
    }

    private void Update()
    {
        if (Time.time - aliveStart > lifespan)
            Destroy(proj);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject actor = other.gameObject;
        if(actor.CompareTag("Player"))
        {
            _controller.Attack(actor.GetComponent<PlayerController>(), 10, damage);
            Destroy(proj);

        }
    }

}
