using UnityEngine;
using System.Collections;

public class RoadScrollMovement : MonoBehaviour {

	public float scrollSpeed;

	// Update is called once per frame
	private void Update () {

		Vector2 offSet = new Vector2(Time.time * scrollSpeed, 0);

		gameObject.GetComponent<Renderer>().material.mainTextureOffset = offSet;
		
	}
}
