using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public PlayerMovementMouse target;
	public Vector2 deadZoneSize;
	[Range (0,3)]
	public float lookAhead;

	private DeadZone deadZone;

	private void Start()
	{
		deadZone = new DeadZone(target.box.bounds, deadZoneSize);
	}

    private void FixedUpdate() {
        MoveCam();
    }

	private void MoveCam()
	{
		if (target != null)
		{
			deadZone.Update(target.box.bounds);

			Vector3 pos = deadZone.centre;

			pos += Camera.main.ScreenToWorldPoint(Input.mousePosition).normalized * lookAhead;

			transform.position = pos + Vector3.forward * -10f;
		}
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(1, 0, 0, 0.5f);
		Gizmos.DrawCube(deadZone.centre, deadZoneSize);
	}

	struct DeadZone {
		public Vector2 velocity;
		public Vector2 centre;
		float top, bot;
		float left, right;

		public DeadZone(Bounds targetBounds, Vector2 size) {
			left = targetBounds.center.x - size.x / 2;
			right = targetBounds.center.x + size.x / 2;
			bot = targetBounds.min.y;
			top = targetBounds.min.y + size.y;

			velocity = Vector2.zero;
			centre = new Vector2((left + right) / 2, (top + bot) / 2);
		}

		public void Update(Bounds targetBounds) {
			float shiftX = 0;
			if (targetBounds.min.x < left)
			{
				shiftX = targetBounds.min.x - left;
			}
			else if (targetBounds.max.x > right) {
				shiftX = targetBounds.max.x - right;
			}
			left += shiftX;
			right += shiftX;

			float shiftY = 0;
			if (targetBounds.min.y < bot)
			{
				shiftY = targetBounds.min.y - bot;
			}
			else if (targetBounds.max.y > top)
			{
				shiftY = targetBounds.max.y - top;
			}
			top += shiftY;
			bot += shiftY;

			centre = new Vector2((left + right) / 2, (top + bot) / 2);
			velocity = new Vector2(shiftX, shiftY);
		}
	}
}
