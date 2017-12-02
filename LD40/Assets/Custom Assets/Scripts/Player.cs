using UnityEngine;

public class Player: MonoBehaviour
{
	public float moveSpeed;

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
		rb.MovePosition(transform.position + transform.up * moveSpeed * v * Time.deltaTime);
	}
	private void Turning()
	{
		Camera.main.ScreenToWorldPoint(Input.mousePosition);
		Vector2 mousePosition = new Vector2 (Input.mousePosition.x, Input.mousePosition.y);
		Debug.DrawLine(Camera.main.transform.position, mousePosition);
		Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
	}
}
