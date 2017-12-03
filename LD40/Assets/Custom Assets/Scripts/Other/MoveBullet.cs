using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour {

	public float moveSpeed;
	public int damage;
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.up * moveSpeed * Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D c)
	{
		if (c.CompareTag("Player"))
		{
			c.GetComponent<Health>().TakeDamage(damage);
			Destroy(this.gameObject);
		}
	}
}
