using UnityEngine;

public class PlayerMovementMouse: MonoBehaviour
{
	public float moveSpeed;
	public float turnSpeed;

	Vector3 movement;
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
		float h = Input.GetAxis("Horizontal");

		Turning();
		Move(v, h);
	}
	private void Move(float v, float h)
	{
		movement.Set(h, v, 0f);

		movement = movement.normalized * moveSpeed * Time.deltaTime;

		rb.MovePosition(transform.position + movement);
	}
	private void Turning()
	{
		Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, turnSpeed * Time.deltaTime);
	}
}
