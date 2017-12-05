using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial : MonoBehaviour {

	public Line line;

	public void ChangeText() {
		FindObjectOfType<DialougeManager>().BeginTutorial(line);
	}
}
