using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyBag : Interactable {

	private PlayerMovementMouse player;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementMouse>();
	}

	public override void Interact() {
		base.Interact();
		if (player.bagsHeld < player.maxAmountOfBags && player.wantToPickUp)
		{
			player.bagsHeld++;
			player.ChangeMovement(true);
			Debug.Log("picked up money bag");
			Destroy(this.gameObject);
		}
	}

	public override void Sink()
	{
		base.Sink();
		Destroy(this.gameObject);
	}
}
