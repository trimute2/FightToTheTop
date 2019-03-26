using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoaderComp : MonoBehaviour
{
	public void LoadScene(int Scene)
	{
		SceneManager.LoadScene(Scene);
	}

	public void AddScene(int Scene)
	{
		SceneManager.LoadScene(Scene, LoadSceneMode.Additive);
	}
}
