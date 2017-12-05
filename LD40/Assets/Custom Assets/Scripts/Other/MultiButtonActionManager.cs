using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiButtonActionManager : MonoBehaviour {

	public GameObject panel;
	bool isPaused = false;

	public void UnPause()
	{
		Time.timeScale = 1f;
		isPaused = false;
	}

	private void Update()
	{
		panel.SetActive(isPaused);

		if (Input.GetKeyDown(KeyCode.Escape))
		{
			isPaused = !isPaused;

			if (isPaused)
				Time.timeScale = 0f;

			if (!isPaused)
				Time.timeScale = 1f;
		}
	}
}
