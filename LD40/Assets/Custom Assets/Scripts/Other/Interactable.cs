using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]
public class Interactable : MonoBehaviour {

	CircleCollider2D circle;

	public float interactionRadius = 1f;

	protected void Awake()
	{
		circle = GetComponent<CircleCollider2D>();
		circle.radius = interactionRadius;
	}

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = Color.cyan;
		Gizmos.DrawWireSphere(this.transform.position, interactionRadius);
	}

	private void OnTriggerEnter2D(Collider2D c)
	{
		if (c.CompareTag("Player"))
		{
			Interact();
		}
		if (c.CompareTag("DropZone"))
		{
			Sink();
		}
        if (c.CompareTag("Enemy"))
        {
            print("enemy entered");
            ResetPos();
        }
    }

	private void OnTriggerStay2D(Collider2D c)
	{
		if (c.CompareTag("DropZone"))
		{
			Sink();
		}
		if (c.CompareTag("Player"))
		{
			Interact();
		}
        if (c.CompareTag("Enemy"))
        {
            print("enemy entered");
            ResetPos();
        }
    }

	public virtual void Interact() {

		// add thing to player
	}

	public virtual void Sink() {
		// call when to remove object from world
	}

    public virtual void ResetPos() {
        // do the reset code
    }
}
