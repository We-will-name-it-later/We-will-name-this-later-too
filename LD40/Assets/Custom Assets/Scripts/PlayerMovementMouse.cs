using UnityEngine;

public class PlayerMovementMouse: MonoBehaviour
{
	[Header("Bag Variables")]
	public GameObject Bag;
	public bool wantToPickUp = false;

	[Space]

	[Header("Movement Variables")]
	public float baseMoveSpeed = 6;
	public float baseTurnSpeed;

	[Space]

	[Header("Combat Variables")]
	public LayerMask hitMask;

	private float actualMoveSpeed;
	private float actualTurnSpeed;

	public int maxAmountOfBags = 6;
	public int bagsHeld;

	Vector3 movement;
	Rigidbody2D rb;

	private void Awake()
	{
		rb = GetComponent<Rigidbody2D>();
		actualMoveSpeed = baseMoveSpeed;
		actualTurnSpeed = baseTurnSpeed;
	}

	private void Update()
	{
		if (Input.GetButtonDown("Fire2"))
		{
			DropBag();
		}
		if (Input.GetButton("Fire1"))
		{
			wantToPickUp = true;
		}
		else
		{
			wantToPickUp = false;
		}

		if (Input.GetKeyDown(KeyCode.Space) && bagsHeld > 0)
		{
			AttackWithBag();
		}
	}
	private void FixedUpdate()
	{
		float v = Input.GetAxisRaw("Vertical");
		float h = Input.GetAxisRaw("Horizontal");

		Turning();
		Move(v, h);
	}

	public void ChangeMovement(bool down)
	{
		if (down && actualMoveSpeed > 0.5f)
		{
			actualMoveSpeed -= 0.5f;
		}
		else if (!down)
		{
			actualMoveSpeed += 0.5f;
		}
	}

	private void Move(float v, float h)
	{
		movement.Set(h, v, 0f);												// set movement axis to user input

		movement = movement.normalized * actualMoveSpeed * Time.deltaTime;	// change vector to meet per frame req.

		rb.MovePosition(transform.position + movement);						// move the player
	}

	private void Turning()
	{
		Vector2 direction = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;		// find direction to the mouse pos.
		float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;								// find angle of rotation
		Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);									// find the quaternion to get to new rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, rotation, actualTurnSpeed * Time.deltaTime);	// set rotation
	}

	private void DropBag() {
		if (bagsHeld > 0)
		{
			bagsHeld--;
			ChangeMovement(false);
			Instantiate(Bag, this.transform.position, Quaternion.identity);
		}
	}

	private void AttackWithBag() {
		if (bagsHeld > 0 && bagsHeld < 6)
		{
			Debug.DrawLine(this.transform.position,
				(Camera.main.ScreenToWorldPoint(Input.mousePosition)), Color.red);
			RaycastHit2D hit = Physics2D.Linecast(this.transform.position, 
				(Camera.main.ScreenToWorldPoint(Input.mousePosition)), hitMask);
			if (hit && Vector2.Distance(this.transform.position, hit.point) < 1)
			{
				Debug.Log("Hit");
			}
		}
	}
}
