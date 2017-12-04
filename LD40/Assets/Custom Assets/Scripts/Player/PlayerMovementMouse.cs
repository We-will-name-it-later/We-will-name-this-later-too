using System.Collections;
using UnityEngine;
using UnityEngine.Animations;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovementMouse: MonoBehaviour
{
	[Header("Bag Variables")]
	public GameObject Bag;
	public bool wantToPickUp = false;
	public SpriteRenderer[] Bags;
    public Transform[] bagSpawnPos;

	[Space]

	[Header("Movement Variables")]
	public float baseMoveSpeed = 6;
	public float baseTurnSpeed;
	public BoxCollider2D box;

	[Space]

	[Header("Combat Variables")]
	public LayerMask hitMask;
	public float timeBetweenAttacks = 2;
	private float timeTillNextAttack = 0;
	private float attackAnimFinTime;

	[Space]

	[Header("Pick Ups")]
	public int maxAmountOfBags = 6;
	public int bagsHeld;
	public Transform moneyHolder;

	private Vector3 movement;
	private Rigidbody2D rb;

	private float actualMoveSpeed;
	private float actualTurnSpeed;

	private Animator anim;

	private void Awake()
	{
		anim = GetComponent<Animator>();
		rb = GetComponent<Rigidbody2D>();
		box = GetComponent<BoxCollider2D>();
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

		if (Time.time > timeTillNextAttack && Input.GetKeyDown(KeyCode.Space) && bagsHeld > 0)
		{
			attackAnimFinTime = Time.time + timeBetweenAttacks;
			timeTillNextAttack += timeBetweenAttacks;
			AttackWithBag();
		}
		else if (Time.time > attackAnimFinTime)
		{
			anim.SetBool("isAttacking", false);
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
			actualMoveSpeed -= 1;
			actualTurnSpeed -= 1f;
			PickUpBag();
		}
		else if (!down)
		{
			actualMoveSpeed += 1;
			actualTurnSpeed += 1f;
		}
	}

	private void Move(float v, float h)
	{
		movement.Set(h, v, 0f);												// set movement axis to user input

		movement = movement.normalized * actualMoveSpeed * Time.deltaTime;  // change vector to meet per frame req.

		bool isWalking = (movement.magnitude > 0) ? true : false;
		anim.SetBool("isWalking", isWalking);

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
			if (Bags[2].enabled == true)
			{
				Bags[2].enabled = false;
			}
			else if (Bags[1].enabled == true)
			{
				Bags[1].enabled = false;
			}
			else if (Bags[0].enabled == true)
			{
				Bags[0].enabled = false;
			}
			ChangeMovement(false);
			MoneyBag bag = Instantiate(Bag, this.transform.position, Quaternion.identity, moneyHolder).GetComponent<MoneyBag>();
            bag.resetPosition = bagSpawnPos[(int)Random.Range(0, bagSpawnPos.Length - 1)].position;
		}
	}

	private void PickUpBag() {
		if (Bags[0].enabled == false)
		{
			Bags[0].enabled = true;
		}
		else if (Bags[1].enabled == false)
		{
			Bags[1].enabled = true;
		}
		else if (Bags[2].enabled == false)
		{
			Bags[2].enabled = true;
		}
	}

	private void AttackWithBag() {
		if (bagsHeld > 0 && bagsHeld < maxAmountOfBags)
		{
			anim.SetBool("isAttacking", true);
			Debug.DrawLine(this.transform.position,
				(Camera.main.ScreenToWorldPoint(Input.mousePosition)), Color.cyan);
			RaycastHit2D hit = Physics2D.Linecast(this.transform.position, 
				(Camera.main.ScreenToWorldPoint(Input.mousePosition)), hitMask);
			if (hit && Vector2.Distance(this.transform.position, hit.point) < 2)
			{
				Debug.Log("Hit");
				Health targetHealth = hit.collider.gameObject.GetComponent<Health>();
				if (targetHealth != null)
				{
					targetHealth.TakeDamage(50 * bagsHeld);
				}
				 
			}
		}
	}
}
