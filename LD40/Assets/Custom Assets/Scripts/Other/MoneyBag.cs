using UnityEngine;

public class MoneyBag : Interactable {

    public Vector2 resetPosition;
 
	private PlayerMovementMouse player;
	private ScoreManager scoreManager;
    private Animator anim;

	private void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovementMouse>();
		scoreManager = FindObjectOfType<ScoreManager>();
        anim = GameObject.FindGameObjectWithTag("Screen").GetComponent<Animator>();

    }

	public override void Interact() {
		base.Interact();
		if (player.bagsHeld < player.maxAmountOfBags && player.wantToPickUp)
		{
			player.bagsHeld++;
			player.ChangeMovement(true);
			Destroy(this.gameObject);
		}
	}

	public override void Sink()
	{
		base.Sink();
		Destroy(this.gameObject);
		scoreManager.GotBag();
        anim.SetTrigger("MoneyTrigger");
	}

    public override void ResetPos()
    {
        base.ResetPos();
        transform.position = resetPosition;
    }
}
