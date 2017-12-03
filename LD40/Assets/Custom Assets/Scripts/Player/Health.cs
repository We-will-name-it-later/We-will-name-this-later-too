﻿using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

	public int startingHealth = 100;
	public GameObject canvas;

	AudioSource audio;
	public AudioClip hitEffect;

	private int currentHealth;

	private void Start()
	{
		audio = GetComponent<AudioSource>();
		currentHealth = startingHealth;
	}

	public void TakeDamage(int damage) {
		currentHealth -= damage;
		audio.PlayOneShot(hitEffect);
		canvas.GetComponent<Animator>().SetTrigger("Damage Trigger");
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
