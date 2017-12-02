using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBullet : MonoBehaviour {

	public float moveSpeed;
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.up * moveSpeed * Time.deltaTime;
	}

	private void OnTriggerEnter2D(Collider2D c)
	{
		if (c.CompareTag("Player"))
		{
			c.GetComponent<Health>().TakeDamage(50);
			Destroy(this.gameObject);
		}
	}
}
