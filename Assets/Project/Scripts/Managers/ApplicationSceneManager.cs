using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationSceneManager : MonoBehaviour
{
	public static readonly string VISUALISER_SCENE_NAME = "AStarVisualiserScene";
	
	public void LoadSceneByName(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
}