using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementMouseX : MonoBehaviour {

	public float moveSpeed;
	public float turnSpeed;

	Vector3 movement;
	Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
	}
	private void FixedUpdate()
	{
		float v = Input.GetAxis("Vertical");

		Turning();
		Move(v);
	}
	private void Move(float v)
	{
		rb.MovePosition(transform.position + transform.right * moveSpeed * v * Time.deltaTime);
	}
	private void Turning()
	{
		Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
	}
}
