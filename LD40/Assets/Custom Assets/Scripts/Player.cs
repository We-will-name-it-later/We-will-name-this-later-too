﻿using UnityEngine;

public class Player: MonoBehaviour
{
	public float moveSpeed;
	public float turnSpeed;

	Vector2 movement;
	Rigidbody2D rb;
	int floor;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		floor = LayerMask.GetMask("Floor");
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
