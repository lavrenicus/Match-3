using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour 
{
	public GameObject [] dibs;
	public GameObject [,] chipField = new GameObject [fieldSize,fieldSize];
	public int DibsSize;
	public GameObject selected;
	
	public Ray _ray;
	public RaycastHit hit;
	
	private const int fieldSize = 8;
	private int count=0;
	

	void Start () 
	{
		DibsSize = dibs.Length;
		//GenField();
		CreatField();
	}
	
	void Update () 
	{
		_ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Physics.Raycast (_ray,out hit);
		if(hit.collider!=null)
		if(Input.GetMouseButtonDown(0))
		{
			if(selected!=null) //Если что-то выделено
			{
				couple (selected, hit.collider.gameObject);
				selected.renderer.material.SetColor ("_Color",Color.white);//Обычный цвет
				selected=null;
			}
			else
			{
				selected = hit.collider.gameObject;
				selected.renderer.material.SetColor ("_Color",Color.red); //Цвет выделения
			}
		}
		MatrUpdate ();
		Match();
	}
	
	public void couple (GameObject First, GameObject Second)
	{
		if((First.transform.position.x+1==Second.transform.position.x||First.transform.position.x-1==Second.transform.position.x)&&(First.transform.position.z==Second.transform.position.z))
		{
			MatrMove((int)First.transform.position.x,(int)First.transform.position.z,(int)Second.transform.position.x,(int)Second.transform.position.z);
		}
		if((First.transform.position.z+1==Second.transform.position.z||First.transform.position.z-1==Second.transform.position.z)&&(First.transform.position.x==Second.transform.position.x))
		{
			MatrMove((int)First.transform.position.x,(int)First.transform.position.z,(int)Second.transform.position.x,(int)Second.transform.position.z);
		}
	}
	
	public void CreatField()
	{
		for(int ix=0;ix<fieldSize;ix++)
		{
			for(int iy=0; iy<fieldSize;iy++)
			{
				int rand=Random.Range (1,DibsSize);
				chipField[ix,iy] =(GameObject) Instantiate (dibs[rand],new  Vector3 (ix,0,iy),Quaternion.Euler (-90,0,0));
				chipField[ix,iy].gameObject.AddComponent ("ChipCore");
				ChipCore ChCo=(ChipCore)chipField[ix,iy].gameObject.GetComponent<ChipCore>();
				ChCo.SetPosX(ix);
				ChCo.SetPosY(iy);
				ChCo.SetType (rand);
			}
		}
		Match ();
	}
	
	public void MatrUpdate()
	{
		for(int ix=0;ix<fieldSize;ix++)
		{
			for(int iy=0; iy<fieldSize;iy++)
			{
				ChipCore SelCore = (ChipCore)chipField[ix,iy].gameObject.GetComponent ("ChipCore");
				if(SelCore.GetCurType ()!=0)
				{

					SelCore.SetPosX(ix);
					SelCore.SetPosY(iy);
				}
				else 
				{
					NullCheck (ix,iy);
				}
			}
		}
	}
	
	public void MatrMove(int FirstX,int FirstY,int SecX, int SecY)
	{
		GameObject temp = chipField[FirstX,FirstY];
		chipField[FirstX,FirstY]=chipField[SecX,SecY];
		chipField[SecX,SecY]=temp;

		//swap(chipField[FirstX,FirstY],chipField[SecX,SecY]);
	}
	public void Match()
	{
		for(int iy=0; iy<fieldSize;iy++)
		{
			GorNextMatch (0,iy);
		}
		for(int ix=0; ix<fieldSize;ix++)
		{
			VertNextMatch (ix,0);
		}
	}
	public void GorNextMatch(int x,int y)
	{
		ChipCore ChCo = (ChipCore)chipField[x,y].gameObject.GetComponent <ChipCore>();
		if(x<fieldSize-1)
		{
			ChipCore ChCo2 = (ChipCore)chipField[x+1,y].gameObject.GetComponent <ChipCore>();
			if(ChCo.GetCurType () == ChCo2.GetCurType ())
			{
				count++;
				GorNextMatch (x+1,y);
			}
			else
			{
				if(count>=2)
				{
					GameObject[] Match = new GameObject [count+1];
					for(int c=0;c<=count;c++)
					{
						Match[c] = chipField[x-c,y];
					}
					ChipDelete(Match);
					count=0;
				}
				count=0;
				GorNextMatch (x+1,y);
			}
		}
		if(x==fieldSize-1&&count>=2)
		{
			GameObject[] Match = new GameObject [count+1];
				for(int c=0;c<=count;c++)
					{
						Match[c] = chipField[x-c,y];
						
					}
					ChipDelete(Match);
					count=0;
		}
	}
	
	
	public void VertNextMatch(int x,int y)
	{
		ChipCore ChCo = (ChipCore)chipField[x,y].gameObject.GetComponent <ChipCore>();
		if(y<fieldSize-1)
		{
			ChipCore ChCo2 = (ChipCore)chipField[x,y+1].gameObject.GetComponent <ChipCore>();
			if(ChCo.GetCurType () == ChCo2.GetCurType ())
			{
				count++;
				VertNextMatch (x,y+1);
			}
			else
			{
				if(count>=2)
				{
					GameObject[] Match = new GameObject [count+1];
					for(int c=0;c<=count;c++)
					{
						Match[c] = chipField[x,y-c];
					}
					ChipDelete(Match);
					count=0;
				}
				count=0;
				VertNextMatch (x,y+1);
			}
		}
		if(y==fieldSize-1&&count>=2)
		{
			GameObject[] Match = new GameObject [count+1];
				for(int c=0;c<=count;c++)
					{
						Match[c] = chipField[x,y-c];
						
					}
					ChipDelete(Match);
					count=0;
		}
	}
	
	public void ChipDelete(GameObject[] MatchArr)
	{
		int MatchSize = MatchArr.Length; 
		if(MatchSize>0)
		{
			for(int c=0;c<MatchSize;c++)
			{
				//Destroy (MatchArr[c]);
				//chipField[mx,my] = null;
				ChipCore ChCo = (ChipCore)MatchArr[c].gameObject.GetComponent<ChipCore>();
				ChCo.SetType (0);
				ChCo.gameObject.renderer.material.SetColor ("_Color",Color.blue);
			}
			for(int c=0;c<MatchSize;c++)
			{
				MatchArr[c]=null;
			}
		}
	}
	
	public void NullCheck(int ix,int iy)
	{
			ChipDown (ix,iy);
	}
	
	public void ChipDown(int ix, int iy)
	{
			if(iy>0)
			{
				//couple (chipField[ix,iy],chipField[ix,iy-1]);
				MatrMove (ix,iy,ix,iy-1);
				Debug.Log ("ChipDown: "+ix.ToString ()+" "+iy.ToString ());
			}
			else
			{
				SpawnChip();
			}
	}
	public void SpawnChip()
	{
		
	}
}
