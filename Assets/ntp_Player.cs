using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ntp_Player : MonoBehaviour
{
	readonly float speed = 10f;
	float moveHorizontal;

	public GameObject projectile;
	readonly float projectileSpeed = 4f;
	readonly float fireRate = .25f;
	float nextFire;

	Rigidbody2D rb;

	ntp_GameManager ntp_gameManager;

	void Start()
	{
		ntp_gameManager = GameObject.Find("GameManager").GetComponent<ntp_GameManager>();

		rb = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		if (ntp_GameManager.state == ntp_GameManager.States.play)
		{
			ntp_Move();
			ntp_Fire();
		}
	}

	void ntp_Fire()
	{
		nextFire += Time.deltaTime;
		if (Input.GetButton("Fire1") && nextFire > fireRate)
		{
			ntp_Shoot();
			nextFire = 0;
		}
	}

	void ntp_Shoot()
	{
		GameObject ntp_bullet = Instantiate(projectile, transform.position, transform.rotation);
		ntp_bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0, projectileSpeed, 0);
	}

	void ntp_Move()
	{
		moveHorizontal = Input.GetAxisRaw("Horizontal");
	}

	private void ntp_FixedUpdate()
	{
		Vector3 force = transform.TransformDirection(-moveHorizontal * speed, 0, 0);
		rb.AddForce(force);
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "BulletEnemy")
		{
			// détruire la bullet
			Destroy(collision.gameObject);
			ntp_gameManager.ntp_KillPlayer();
		}
	}

}
