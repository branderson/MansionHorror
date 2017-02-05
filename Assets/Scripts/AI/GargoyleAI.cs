using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Game;

public class GargoyleAI : MonoBehaviour
{
    private EnemyController _controller;
    private GameObject Player;
    private Vector2 dir;
    private Rigidbody2D rb;
    private float timeStart;
    public float speed;
    public float damage;
    public float cooldown;
    public float radius;
    public float lifespan;

	void Start ()
    {
        timeStart = Time.time;
        _controller = GetComponent<EnemyController>();
        Player = GameObject.FindGameObjectWithTag("Player");
        Vector2 playerPos = new Vector2(Player.transform.position.x, Player.transform.position.y);
        Vector2 gargPos = new Vector2(transform.position.x, transform.position.y);
        //spawn = (Random.insideUnitCircle.normalized) * radius;
        dir = (playerPos - gargPos).normalized;

        rb = GetComponent<Rigidbody2D>();
        rb.velocity = speed * dir;

	}
	
	// Update is called once per frame
	void Update ()
    {
		if(Time.time-timeStart>lifespan)
        {
            Destroy(this.gameObject);
        }
	}

    private void OnCollisionEnter2D(Collision2D other)
    {
        GameObject actor = other.gameObject;
        if (actor.CompareTag("Player"))
        {
            _controller.Attack(actor.GetComponent<PlayerController>(), 10, damage);
            Destroy(this.gameObject);

        }
    }
}
