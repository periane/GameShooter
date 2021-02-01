using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ntp_Ennemis : MonoBehaviour
{
	public int points = 10;
	int life = 3;
	float speed;
	bool arret = true;
	readonly float Speedmax = 5f;
	readonly float Speedmin = 1f;
	Bounds Attack;
	Vector3 target;

	public GameObject projectile;
	readonly float projectileSpeed = 4f;
	readonly float fireRate = .25f;
	float nextFire;

	Rigidbody2D rb;

	ntp_GameManager ntp_gameManager;

	Camera cam;
	float height, width;

	void Start()
	{
		ntp_gameManager = GameObject.Find("GameManager").GetComponent<ntp_GameManager>();

		cam = Camera.main;
		height = cam.orthographicSize;
		width = height * cam.aspect;

		speed = Random.Range(Speedmin, Speedmax);

		rb = GetComponent<Rigidbody2D>();

		Attack = ntp_gameManager.attackArea.GetComponent<BoxCollider2D>().bounds;
	}

	void Update()
	{
		if (ntp_GameManager.state == ntp_GameManager.States.play)
		{
			StartCoroutine(ntp_Move());
		}
	}

	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			ntp_gameManager.ntp_KillPlayer();
		}
		else if (collision.tag == "Bullet")
		{
			// détruire la bullet
			Destroy(collision.gameObject);
			// destruction
			if (life > 0)
			{
				life -= 1;
			}
			else
			{
				Destroy(gameObject);
			}
			// score
			ntp_gameManager.AddScore(points);
		}
	}

	IEnumerator ntp_Move()
	{
		if (arret)
		{
			ntp_Fire();
			yield return new WaitForSecondsRealtime(3f);
			arret = false;
			//pont d'attaque random
			target = new Vector3(
			   Random.Range(Attack.min.x, Attack.max.x),
			   Random.Range(Attack.min.y, Attack.max.y),
			   0
			);
			target = ntp_gameManager.attackArea.GetComponent<BoxCollider2D>().ClosestPoint(target);
		}
		else
		{
			//se diriger vers le point donné aléatoirement dans l'aire d'attaque du jeu et s'arrêter si atteint
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, target, step);
			if (transform.position == target)
			{
				arret = true;
			}
		}
	}

	void ntp_Fire()
	{
		nextFire += Time.deltaTime;
		if (nextFire > fireRate)
		{
			ntp_Shoot();
			nextFire = 0;
		}
	}

	void ntp_Shoot()
	{
		GameObject bullet = Instantiate(projectile, transform.position, transform.rotation);
		bullet.GetComponent<Rigidbody2D>().velocity = transform.TransformDirection(0, projectileSpeed, 0);
	}

}
