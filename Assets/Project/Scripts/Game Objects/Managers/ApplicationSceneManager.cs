using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationSceneManager : MonoBehaviour
{
	public static readonly string VISUALISER_SCENE_NAME = "VisualiserScene";

	private FadeScreenPanelUI fadeScreenPanelUI;
	
	public void LoadSceneByName(string sceneName)
	{
		void OnFadeWasCompleted() => SceneManager.LoadScene(sceneName);

		if(fadeScreenPanelUI != null)
		{
			fadeScreenPanelUI.FadeIn(OnFadeWasCompleted);
		}
		else
		{
			OnFadeWasCompleted();
		}
	}

	private void Awake()
	{
		fadeScreenPanelUI = ObjectMethods.FindComponentOfType<FadeScreenPanelUI>();
	}
}