using UnityEngine;
using System.Collections;

public class Field : MonoBehaviour 
{
	#region Var
	public GameObject [] dibs;
	public GameObject [,] chipField = new GameObject [fieldSize,fieldSize];
	public Mesh UniCollider;
	public int DibsSize;
	public GameObject selected;
	public float timer=0;
	public float WaitTime = 0.5f;
	public Ray _ray;
	public RaycastHit hit;
	public float ChSpeed=5;
	public int _Matches=0;
	
	public float TimerD;
	private const int fieldSize = 8;
	private GameObject Self;
	private bool WaitBool=true;
	#endregion
	
	void Start () 
	{
		Self = gameObject;
		DibsSize = dibs.Length;
		CreatField();
	}
	
	void Update () 
	{
		if(WaitBool)
		{
		timer+=Time.deltaTime;
		_ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		Debug.DrawRay (_ray.origin,_ray.direction*100,Color.red);
		Physics.Raycast (_ray,out hit);
		if(hit.collider!=null)
		if(Input.GetMouseButton(0))
		{
			if(selected!=null && selected!=hit.collider.gameObject) //Если что-то выделено
			{
				PlCouple (selected, hit.collider.gameObject);
				selected.GetComponent<Renderer>().material.SetColor ("_Color",Color.white);//Обычный цвет
				selected=null;
			}
			else
			{
				selected = hit.collider.gameObject;
				selected.GetComponent<Renderer>().material.SetColor ("_Color",Color.red); //Цвет выделения
			}
		}
		NullCheck ();
		if(timer>=TimerD)
		{
			MatrCheckDown ();
			_Matches = Match();
		}
		if(timer>100)
		{
			timer=0;
			TimerD=timer;
		}
		if(AvaibleMoves ()==0)
		{
			MixingField();
		}
		}
	}
		
	public void CreatField()//Создание поля
	{
		for(int ix=0;ix<fieldSize;ix++)
		{
			for(int iy=0; iy<fieldSize;iy++)
			{
				CreatChip(ix,iy);
			}
		}
		Match ();
			
	}
	#region Move	
	
	public void PlCouple(GameObject First, GameObject Second)
	{
		
		if(couple (First, Second)== 0)
		{
			//StartCoroutine (WaitCouple(First,Second));
			couple (First, Second);
		}
	}
	
	public int couple (GameObject First, GameObject Second)
	{
		if(First!=null && Second!=null)
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
		return Match ();
	}

	
	public void MatrMove(int FirstX,int FirstY,int SecX, int SecY)
	{
		GameObject temp = chipField[FirstX,FirstY];
		chipField[FirstX,FirstY]=chipField[SecX,SecY];
		chipField[SecX,SecY]=temp;
	}
	
	public int Match()
	{
		int matches=0;
		for(int iy=0; iy<fieldSize;iy++)
		{
			matches+= GorTriMatch(1,iy);
		}
		for(int ix=0; ix<fieldSize;ix++)
		{
			matches+= VertTriMatch(ix,1);
		}
		return matches;
	}
	
	public void MixingField()
	{
		for(int ix = 0; ix<fieldSize;ix++)
		{
			for(int iy = 0; iy<fieldSize;iy++)
			{
				int randx=Random.Range (1,fieldSize-1);
				int randy=Random.Range (1,fieldSize-1);
				MatrMove (ix,iy,randx,randy);
			}
		}
	}
	
	#endregion
	#region Match
	public int GorTriMatch(int x, int y)
	{
		int gormatch=0;
		if(x>0&&x<fieldSize-1)
		{
			ChipCore ChCo = (ChipCore)chipField[x-1,y].gameObject.GetComponent<ChipCore>();
			ChipCore PrevCh = (ChipCore)chipField[x,y].gameObject.GetComponent<ChipCore>();
			ChipCore NextCh = (ChipCore)chipField[x+1,y].gameObject.GetComponent<ChipCore>();
			if(PrevCh.GetCurType()==ChCo.GetCurType()&&ChCo.GetCurType ()==NextCh.GetCurType ()&&ChCo.GetCurType ()!=0)
			{
				PrevCh.SetMFlag (true);
				ChCo.SetMFlag (true);
				NextCh.SetMFlag (true);
				TimerD=timer+WaitTime;
				gormatch+=1;
			}
			gormatch+= GorTriMatch (x+1,y);
		}
		return gormatch;
		
	}
	
	public int VertTriMatch(int x, int y)
	{
		int vertmatch=0;
		if(y>0&&y<fieldSize-1)
		{
			ChipCore ChCo = (ChipCore)chipField[x,y-1].gameObject.GetComponent<ChipCore>();
			ChipCore PrevCh = (ChipCore)chipField[x,y].gameObject.GetComponent<ChipCore>();
			ChipCore NextCh = (ChipCore)chipField[x,y+1].gameObject.GetComponent<ChipCore>();
			if(PrevCh.GetCurType()==ChCo.GetCurType()&&ChCo.GetCurType ()==NextCh.GetCurType ()&&ChCo.GetCurType ()!=0)
			{
				PrevCh.SetMFlag (true);
				ChCo.SetMFlag (true);
				NextCh.SetMFlag (true);
				TimerD=timer+WaitTime;
				vertmatch+=1;
			}
			vertmatch+=VertTriMatch (x,y+1);
		}
		return vertmatch;
		
	}
	#endregion
	#region Update
	
	public void NullCheck()
	{
		for(int ix=0;ix<fieldSize;ix++)
		{
			for(int iy=0; iy<fieldSize;iy++)
			{
				ChipCore ChCo = (ChipCore)chipField[ix,iy].gameObject.GetComponent<ChipCore>();
				if(ChCo.GetMFlag()==true)
				{
					ChCo.SetType (0);
				}
			}
		}
		MatrUpdate();
	}
	
	public void MatrCheckDown()
	{		
		for(int ix=0;ix<fieldSize;ix++)
		{
			for(int iy=0; iy<fieldSize;iy++)
			{
				ChipCore SelCore = (ChipCore)chipField[ix,iy].gameObject.GetComponent ("ChipCore");
				if(SelCore.GetCurType ()==0)
				{
					SelCore.GetComponent<Renderer>().enabled=false;
					ChipDown (ix,iy);
				}
			}
		}
	}
	
	public void MatrUpdate()
	{
		for(int ix=0;ix<fieldSize;ix++)
		{
			for(int iy=0; iy<fieldSize;iy++)
			{
				ChipCore SelCore = (ChipCore)chipField[ix,iy].gameObject.GetComponent ("ChipCore");
				SelCore.SetPosX(ix);
				SelCore.SetPosY(iy);
			}
		}
	}
	#region AvaibleCheck
	public int AvaGorCheck(int inx, int iny)
	{
		int AvMatchs = 0;
		ChipCore ChCo = (ChipCore)chipField[inx,iny].gameObject.GetComponent<ChipCore>();
		if(inx+1<fieldSize)
		{
			ChipCore ChCo2=(ChipCore)chipField[inx+1,iny].gameObject.GetComponent<ChipCore>();
			if(ChCo.GetCurType ()==ChCo2.GetCurType())
			{
				if(inx-1>=0)
				{
					if(iny-1>=0)
					{
						ChipCore ChCoUpR=(ChipCore)chipField[inx-1,iny-1].gameObject.GetComponent<ChipCore>();
						if(ChCo.GetCurType ()==ChCoUpR.GetCurType ())
						{
							AvMatchs++;
						}
					}
					if(iny+1<fieldSize)
					{
						ChipCore ChCoDnR=(ChipCore)chipField[inx-1,iny+1].gameObject.GetComponent<ChipCore>();
						if(ChCo.GetCurType()==ChCoDnR.GetCurType ())
						{
							AvMatchs++;
						}
					}

				}
				if(inx-2>=0)
				{
					ChipCore ChCoAOR=(ChipCore)chipField[inx-2,iny].gameObject.GetComponent<ChipCore>();
					{
						if(ChCo.GetCurType()==ChCoAOR.GetCurType ())
						{
							AvMatchs++;
						}
					}
				}
				if(inx+2<fieldSize)
				{
					if(iny-1>=0)
					{
						ChipCore ChCoUpL=(ChipCore)chipField[inx+2,iny-1].gameObject.GetComponent<ChipCore>();
						if(ChCoUpL.GetCurType()==ChCo.GetCurType ())
						{
							AvMatchs++;
						}
					}
					if(iny<fieldSize-1)
					{
						ChipCore ChCoDnL=(ChipCore)chipField[inx+2,iny+1].gameObject.GetComponent<ChipCore>();
						if(ChCo.GetCurType ()==ChCoDnL.GetCurType ())
						{
							AvMatchs++;
						}
					}
				}
				
				if(inx<fieldSize-3)
				{
				ChipCore ChCoAOL=(ChipCore)chipField[inx+3,iny].gameObject.GetComponent<ChipCore>();
					if(ChCoAOL.GetCurType ()==ChCo.GetCurType ())
					{
						AvMatchs++;
					}
				}
			}
		}
		if((inx>=1 && inx<fieldSize-1)&&(iny>=1 && iny<fieldSize-1))
		{
			ChipCore Left =(ChipCore)chipField[inx+1,iny].gameObject.GetComponent<ChipCore>();
			ChipCore Rigth=(ChipCore)chipField[inx-1,iny].gameObject.GetComponent<ChipCore>();
			ChipCore Up=(ChipCore)chipField[inx,iny-1].gameObject.GetComponent<ChipCore>();
			ChipCore Down=(ChipCore)chipField[inx,iny+1].gameObject.GetComponent<ChipCore>();
			if(Left.GetCurType ()==Up.GetCurType ()&&Up.GetCurType ()==Rigth.GetCurType ())
			{
				AvMatchs++;
			}
			if(Left.GetCurType ()==Down.GetCurType ()&&Down.GetCurType ()==Rigth.GetCurType ())
			{
				AvMatchs++;
			}
		}
		
		
		return AvMatchs;
	}
	
	public int AvaVertCheck(int inx, int iny)
	{
		int AvMatchs = 0;
		ChipCore ChCo = (ChipCore)chipField[inx,iny].gameObject.GetComponent<ChipCore>();
		if(iny+1<fieldSize)
		{
			ChipCore ChCo2=(ChipCore)chipField[inx,iny+1].gameObject.GetComponent<ChipCore>();
			if(ChCo.GetCurType ()==ChCo2.GetCurType())
			{
				if(iny-1>=0)
				{
					if(inx-1>=0)
					{
						ChipCore ChCoUpR=(ChipCore)chipField[inx-1,iny-1].gameObject.GetComponent<ChipCore>();
						if(ChCo.GetCurType ()==ChCoUpR.GetCurType ())
						{
							AvMatchs++;
						}
					}
					if(inx+1<fieldSize)
					{
						ChipCore ChCoDnR=(ChipCore)chipField[inx+1,iny-1].gameObject.GetComponent<ChipCore>();
						if(ChCo.GetCurType()==ChCoDnR.GetCurType ())
						{
							AvMatchs++;
						}
					}

				}
				if(iny-2>=0)
				{
					ChipCore ChCoAOR=(ChipCore)chipField[inx,iny-2].gameObject.GetComponent<ChipCore>();
					{
						if(ChCo.GetCurType()==ChCoAOR.GetCurType ())
						{
							AvMatchs++;
						}
					}
				}
				if(iny+2<fieldSize)
				{
					if(inx-1>=0)
					{
						ChipCore ChCoUpL=(ChipCore)chipField[inx-1,iny+2].gameObject.GetComponent<ChipCore>();
						if(ChCoUpL.GetCurType()==ChCo.GetCurType ())
						{
							AvMatchs++;
						}
					}
					if(inx<fieldSize-1)
					{
						ChipCore ChCoDnL=(ChipCore)chipField[inx+1,iny+2].gameObject.GetComponent<ChipCore>();
						if(ChCo.GetCurType ()==ChCoDnL.GetCurType ())
						{
							AvMatchs++;
						}
					}
				}
				
				if(iny<fieldSize-3)
				{
				ChipCore ChCoAOL=(ChipCore)chipField[inx,iny+3].gameObject.GetComponent<ChipCore>();
					if(ChCoAOL.GetCurType ()==ChCo.GetCurType ())
					{
						AvMatchs++;
					}
				}
			}
		}
		if((inx>=1 && inx<fieldSize-1)&&(iny>=1 && iny<fieldSize-1))
		{
			ChipCore Left =(ChipCore)chipField[inx,iny+1].gameObject.GetComponent<ChipCore>();
			ChipCore Rigth=(ChipCore)chipField[inx,iny-1].gameObject.GetComponent<ChipCore>();
			ChipCore Up=(ChipCore)chipField[inx-1,iny].gameObject.GetComponent<ChipCore>();
			ChipCore Down=(ChipCore)chipField[inx+1,iny].gameObject.GetComponent<ChipCore>();
			if(Left.GetCurType ()==Up.GetCurType ()&&Up.GetCurType ()==Rigth.GetCurType ())
			{
				AvMatchs++;
			}
			if(Left.GetCurType ()==Down.GetCurType ()&&Down.GetCurType ()==Rigth.GetCurType ())
			{
				AvMatchs++;
			}
		}
		
		
		return AvMatchs;
	}
	#endregion
	
	public void ChipDown(int ix, int iy)
	{
		if(iy>0)
		{
			MatrMove (ix,iy,ix,iy-1);
		}
		else
		{
			SpawnChip(ix);
		}
	}
	
	public void SpawnChip(int ix)
	{
		Gui ThisGUI = (Gui)Self.gameObject.GetComponent<Gui>();
		ThisGUI.score++;
		Destroy (chipField[ix,0]);
		chipField[ix,0]=null;
		CreatChip (ix,0);

	}
	
	public void CreatChip(int ix, int iy)
	{
		int rand=Random.Range (1,DibsSize);
		chipField[ix,iy] =(GameObject) Instantiate (dibs[rand],new  Vector3 (ix,0,iy),Quaternion.Euler (-90,0,0));
		ChipCore ChCo=chipField[ix,iy].gameObject.AddComponent <ChipCore>();
		chipField[ix,iy].GetComponent<Renderer>().material.SetColor ("_Color",Color.white);
		if(UniCollider!=null)
		{
			chipField[ix,iy].GetComponent<MeshCollider>().sharedMesh = UniCollider;
		}
		ChCo.SetPosX(ix);
		ChCo.SetPosY(iy);
		ChCo.Speed=ChSpeed;
		ChCo.SetType (rand);

	}
	
	public int AvaibleMoves()
	{
		int AvMatch = 0;
		for(int ix=0;ix<fieldSize;ix++)
			for(int iy=0;iy<fieldSize;iy++)
			{
				AvMatch+=AvaGorCheck(ix,iy);
				AvMatch+=AvaVertCheck (ix,iy);
			}
		Gui ThisGUI = (Gui)Self.gameObject.GetComponent<Gui>();
		ThisGUI.AvaibleMovies=AvMatch;
		return AvMatch;
	}
	#endregion
	
	IEnumerator WaitCouple(GameObject First, GameObject Second)
	{
		WaitBool=false;
		NullCheck ();
		MatrCheckDown ();
        yield return new WaitForSeconds(WaitTime);
		couple (First,Second);
		WaitBool=true;
    }
}
