using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialougeManager : MonoBehaviour {

	private Queue<string> lines;
	private Animator anim;

	private void Start()
	{
		lines = new Queue<string>();
	}

	public void BeginTutorial(Line textField) {
		// update UI

		foreach (string i in textField.tutorialLines)
		{
			lines.Enqueue(i);
		}

		NextLine();
	}

	public void NextLine() {
		if (lines.Count == 0)
		{
			End();
			return;
		}
	}

	public void End() {

	}
}
