using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FpsCounter : MonoBehaviour
{
	string label = "";
	float count;

	public Text text;

	IEnumerator Start()
	{
		GUI.depth = 2;
		while (true)
		{
			if (Time.timeScale == 1)
			{
				yield return new WaitForSeconds(0.1f);
				count = (1 / Time.deltaTime);
				label = "FPS :" + (Mathf.Round(count)) + "With: "+ Screen.width + "Height: " + Screen.height;
			}
			else
			{
				label = "Pause";
			}
			text.text = label;
			yield return new WaitForSeconds(0.1f);
		}
	}

    private void Update()
    {
        
    }
}
