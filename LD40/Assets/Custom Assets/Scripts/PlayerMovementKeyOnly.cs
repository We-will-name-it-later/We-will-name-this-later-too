using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class PlayerMovementKeyOnly : MonoBehaviour {

	public float moveSpeed = 7f;

	private Rigidbody2D rb;
	private BoxCollider2D box;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

		this.rb.MovePosition((Vector2)this.transform.position + input.normalized * moveSpeed * Time.deltaTime);
	}
}
