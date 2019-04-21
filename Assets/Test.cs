using System.Collections;
using System.Collections.Generic;
using PathologicalGames;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

	public Transform Cube;
	private List<Transform> _cubeList = new List<Transform>();
	private float _posCube = 0;
	public Transform CubeFather;
	
	public Transform Sphere;
	private List<Transform> _sphereList = new List<Transform>();
	private float _posSphere = 0;
	public Transform SPhereFather;

	private PoolMgr _poolMgr = new PoolMgr();
	
	private void OnGUI()
	{	
		if (GUI.Button(new Rect(0,0,100,100),"生成Cube"))
		{
			_posCube += 0.5f;
			Transform cube = _poolMgr.Spawn(Cube,new Vector3(_posCube,_posCube,_posCube),Quaternion.identity);
			_cubeList.Add(cube);
		}
		if (GUI.Button(new Rect(0,100,100,100),"生成Cube到" ))
		{
			_posCube += 0.5f;
			Transform cube = _poolMgr.Spawn(Cube,new Vector3(-_posCube,_posCube,_posCube),Quaternion.identity,CubeFather);
			_cubeList.Add(cube);
		}
		if (GUI.Button(new Rect(100,0,100,100),"生成Sphere"))
		{
			_posSphere += 0.5f;
			Transform sphere = _poolMgr.Spawn(Sphere,new Vector3(-_posSphere,-_posSphere,-_posSphere),Quaternion.identity);
			_sphereList.Add(sphere);
		}
		if (GUI.Button(new Rect(100,100,100,100),"生成Sphere到" ))
		{
			_posSphere += 0.5f;
			Transform sphere = _poolMgr.Spawn(Sphere,new Vector3(_posSphere,-_posSphere,-_posSphere),Quaternion.identity,SPhereFather);
			_sphereList.Add(sphere);
		}
		if (GUI.Button(new Rect(200,0,100,100),"释放Cube"))
		{
			if (_cubeList.Count>0)
			{
				_poolMgr.Despawn(_cubeList[0]);
				_cubeList.Remove(_cubeList[0]);
			}
		}
		if (GUI.Button(new Rect(300,0,100,100),"释放Sphere"))
		{
			if (_sphereList.Count>0)
			{
				_poolMgr.Despawn(_sphereList[0]);
				_sphereList.Remove(_sphereList[0]);
			}
		}
		if (GUI.Button(new Rect(400,0,100,100),"释放所有"))
		{
			_poolMgr.DespawnAll();
		}
		
		
		
	}
}
