using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {

	public int bagWorth = 100;
	public int numberOfGuardsActive = 3;

	public GameObject[] Guards;

	private TextMeshProUGUI scoreValue;

	private int score;
	private int guardsActive;

	private void Start()
	{
		scoreValue = GetComponentInChildren<TextMeshProUGUI>();
		if (Guards.Length > 0)
		{
			for (int i = 0; i < (Guards.Length - 1); i++)
			{
				Guards[i].SetActive(false);
			}
			int numberOfActualGuards = (numberOfGuardsActive < (Guards.Length - 1)) ? numberOfGuardsActive : Guards.Length;
			for (int i = 0; i < numberOfActualGuards; i++)
			{
				print(Guards.Length - 1);
				ActivateNewGuard(i);
			}
		}
	}

	public void GotBag()
	{
		score += bagWorth;
		scoreValue.SetText("$ " + score.ToString());
		if ((Guards.Length - 1) > numberOfGuardsActive)
		{
			ActivateNewGuard(numberOfGuardsActive + 1);
		}
	}

	private void ActivateNewGuard(int i) {
		print(i);
		Guards[i].SetActive(true);
		numberOfGuardsActive++;
	}
}
