using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour {

	public void ChangeScene(int index)
	{
		SceneManager.LoadScene(index);
	}

	public void QuitGame ()
	{
		print("Quit!");
		Application.Quit();
	}
}
