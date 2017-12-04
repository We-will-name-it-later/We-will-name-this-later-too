using UnityEngine.SceneManagement;
using UnityEngine;

public class SceneChange : MonoBehaviour {

	public void ChangeScene(int index)
	{
		SceneManager.LoadScene(index);
	}
}
