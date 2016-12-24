using UnityEngine;
using System.Collections;

public class ChipCore : MonoBehaviour
{
	public int MatPosX;
	public int MatPosY;
	public int Type;
	public float Speed = 5;
	private GameObject Self;
	private GameObject Chip;
	private bool Match=false;
	public Vector3 temp;
	
	void Start()
	{
		Self = gameObject;
	}
	
	void Update()
	{
		float step = Speed * Time.deltaTime;
		Self.transform.position=Vector3.MoveTowards(transform.position, new Vector3 (MatPosX,0,MatPosY), step);
		//Self.transform.position = new Vector3 (MatPosX,0,MatPosY);
	}
	
	public void Move(int InX, int InY)
	{
		SetPosX(InX);
		SetPosY(InY);
	}
	
	public void SetMFlag(bool flag)
	{
		Match=flag;
	}
	public bool GetMFlag()
	{
		return Match;
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
