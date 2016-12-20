using UnityEngine;
using System.Collections;

public class ChipCore : MonoBehaviour
{
	public int MatPosX;
	public int MatPosY;
	public int Type;
	private GameObject Self;
	//private GameObject fieldus; 
	private GameObject Chip;
	private Vector3 temp;
	
	void Start()
	{
		//fieldus = GameObject.FindGameObjectWithTag ("Fieldus");
		Self = gameObject;
	}
	
	void Update()
	{
		Self.transform.position = new Vector3 (MatPosX,0,MatPosY);
		//Self.transform.position = Vector3.Lerp (temp,new Vector3(MatPosX,0,MatPosY),Time.deltaTime);
	}
	
	public void Move(int InX, int InY)
	{
		SetPosX(InX);
		SetPosY(InY);
	}
	
	public void SetPosX(int InX)
	{
		MatPosX=InX;
	}
	public void SetPosY(int InY)
	{
		MatPosY=InY;
	}
	
	public void SetType(int InType)
	{
		Type=InType;
	}
	public int GetCurType()
	{
		return Type;
	}
	
	public int GetPosX()
	{
		return MatPosX;
	}
	public int GetPosY()
	{
		return MatPosY;
	}
	
}
