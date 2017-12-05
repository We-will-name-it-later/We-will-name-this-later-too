using UnityEngine.SceneManagement;
using UnityEngine;

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
