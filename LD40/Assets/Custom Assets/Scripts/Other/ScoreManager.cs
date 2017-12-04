using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour {

	public int bagWorth = 100;

	private TextMeshProUGUI scoreValue;

	private int score;


	private void Start()
	{
		scoreValue = GetComponentInChildren<TextMeshProUGUI>();
	}

	public void GotBag() {
		score += bagWorth;
		scoreValue.SetText("$ " + score.ToString());
	}
}
