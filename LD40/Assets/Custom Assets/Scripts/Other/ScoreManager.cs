using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreManager : MonoBehaviour {

	public int bagWorth = 100;
	public int numberOfGuardsActive;
	public int startingGuards;

	public GameObject[] Guards;

	private TextMeshProUGUI scoreValue;

	private int score;
	private int guardsActive;

	private void Start()
	{
		scoreValue = GetComponentInChildren<TextMeshProUGUI>();
		if (Guards.Length > 0)
		{
			for (int i = 0; i < Guards.Length; i++)
			{
				Guards[i].SetActive(false);
			}
			if (startingGuards <= (Guards.Length - 1))
			{
				for (int i = 0; i < startingGuards; i++)
				{
					ActivateNewGuard(i);
				}
			}
		}
	}

	public void GotBag()
	{
		score += bagWorth;
		scoreValue.SetText("$ " + score.ToString() + "k");
		if ((Guards.Length - 1) > numberOfGuardsActive)
		{
			ActivateNewGuard(numberOfGuardsActive + 1);
		}
	}

	private void ActivateNewGuard(int i) {
		Guards[i].SetActive(true);
		numberOfGuardsActive++;
	}
}
