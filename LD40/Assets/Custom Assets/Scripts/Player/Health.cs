using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public int startingHealth = 100;
	public GameObject canvas;

	AudioSource audioSource;
	public AudioClip hitEffect;

	[SerializeField] private int currentHealth;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
		currentHealth = startingHealth;
	}

	public void TakeDamage(int damage) {
		currentHealth -= damage;
		if (this.gameObject.CompareTag("Player"))
		{
			audioSource.PlayOneShot(hitEffect);
			canvas.GetComponent<Animator>().SetTrigger("Damage Trigger");
		}

		if (this.gameObject.CompareTag("Enemy"))
		{
			canvas.GetComponent<Animator>().SetTrigger("Deal DMG Trigger");
		}

		if (currentHealth <= 0)
		{
			Die();
		}
	}

	private void Die() {
		if (this.gameObject.CompareTag("Player"))
		{
			OnPlayerDeath();
		}

		Destroy(this.gameObject);
	}

	public virtual void OnPlayerDeath() {

	}
}
