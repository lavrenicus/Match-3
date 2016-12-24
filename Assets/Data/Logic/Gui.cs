using UnityEngine;
using System.Collections;

public class Gui : MonoBehaviour
{
	public float score=0;
	public float AvaibleMovies=0;
	

	void Start () 
	{
	
	}
	
	void OnGUI()
	{
		if(GUI.Button (new Rect(10,10,50,50),"Restart"))
		{
			//Application.LoadLevel (0);
			Field CurField = (Field)gameObject.GetComponent<Field>();
			CurField.MixingField ();
		}
		GUI.TextArea(new Rect(60, 10, 100, 20), "Score: "+score.ToString(), 25);
		GUI.TextField(new Rect(60, 30, 100, 20), "Avaible Movies:", 25);
		GUI.TextField(new Rect(60, 50, 100, 20), AvaibleMovies.ToString (), 25);
	}
	
	void Update () 
	{
	
	}
}
