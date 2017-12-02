using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardAI : MonoBehaviour {

	public bool canSeeTarget = false;
	public float turnSpeed = 20;
	public float timeBetweenShots = 2;
	public GameObject Bullet;
	public Transform muzzle;

	public Transform target;
	private float timeTillNextShot = 0;

	// Use this for initialization
	void Start () {
		target = FindObjectOfType<PlayerMovementMouse>().transform;
	}
	
	// Update is called once per frame
	void Update () {
		if (target != null)
		{
			Debug.DrawRay(transform.position, (target.position - transform.position) * 50f, Color.green);

			Vector3 direction = target.position - transform.position;
			float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
			//float angle = Vector3.Angle((target.position - transform.position), Vector3.up);

			if (canSeeTarget)
			{
				OnSee(angle);
			}
		}
	}

	private void OnSee(float angle) {
		transform.eulerAngles = Vector3.forward * (270 + angle);
		Shoot();
	}

	private void Shoot() {
		print("shot");
		if (Time.time > timeTillNextShot)
		{
			timeTillNextShot += timeBetweenShots;
			Destroy(Instantiate(Bullet, muzzle.position, transform.rotation), 5);
		}
	}
}
