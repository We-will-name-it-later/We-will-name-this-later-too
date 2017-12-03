using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : Health {

	public GameObject deathCanvas;

	public override void OnPlayerDeath()
	{
		base.OnPlayerDeath();
		deathCanvas.GetComponent<Animator>().SetTrigger("Death Trigger");
	}
}
