using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParabolaController : MonoBehaviour {

	public GameObject trajectoryPrefab;
	public  float trajectPointCount=20;
	private List<GameObject> trajectPointsList;

	void Start()
	{
		trajectPointsList=new List<GameObject>();
		for (int i = 0; i < trajectPointCount; i++)
		{
			GameObject dot=Instantiate(trajectoryPrefab);
			dot.GetComponent<SpriteRenderer>().enabled=false;
			trajectPointsList.Add(dot);
		}
	} 
	/// <summary>
	/// 设置弹道点的位置
	/// </summary>
	/// <param name="startPos">炮弹的起始坐标</param>
	/// <param name="pVelocity">炮弹在对应时间点的速度</param>
	public void SetTrajectPointPos(Vector3 startPos,Vector3 pVelocity){

		float velocity=Mathf.Sqrt((pVelocity.x*pVelocity.x)+(pVelocity.y*pVelocity.y));
		float angle=Mathf.Atan2(pVelocity.y,pVelocity.x)*Mathf.Rad2Deg;

		float stepTime=0;
		stepTime+=0.1f;

		for (int i = 0; i < trajectPointsList.Count; i++)
		{
			float dx=velocity*stepTime*Mathf.Cos(angle*Mathf.Deg2Rad);
			float dy=velocity*stepTime*Mathf.Sin(angle*Mathf.Deg2Rad)-(Physics.gravity.magnitude*stepTime*stepTime/2);
			Vector3 pos=new Vector3(startPos.x+dx,startPos.y+dy,2);
			trajectPointsList[i].transform.position=pos;
			trajectPointsList[i].GetComponent<SpriteRenderer>().enabled=true;
			trajectPointsList[i].transform.eulerAngles=
			new Vector3(0,0,Mathf.Atan2(pVelocity.y - (Physics.gravity.magnitude)*stepTime,pVelocity.x)*Mathf.Rad2Deg);
			stepTime+=0.1f;
		}
	}
}
