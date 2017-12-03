using UnityEngine;

public class Health : MonoBehaviour {

	public int startingHealth = 100;

	private int currentHealth;

	private void Start()
	{
		currentHealth = startingHealth;
	}

	public void TakeDamage(int damage) {
		currentHealth -= damage;
		if (currentHealth <= 0)
		{
			Die();
		}
	}

	private void Die() {
		if (this.gameObject.CompareTag("Player"))
		{
			// create game over screen
		}

		Destroy(this.gameObject);
	}
}
