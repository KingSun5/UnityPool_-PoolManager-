using System;
using PathologicalGames;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Scenes
{
	/// <summary>
	/// time:2019/4/20 15:53
	/// author:Sun
	/// des:对象池管理——基于PoolManager插件
	/// </summary>
	public class PoolMgr
	{

		#region //////////////////////管理默认设置//////////////////////
		
		/// <summary>
		/// 对象池名字
		/// </summary>
		private string _poolName = "SunPool";
		/// <summary>
		/// 实例对象缩放比 true:1 false:Prefab默认缩放
		/// </summary>
		private bool _matchPoolScale = false;
		/// <summary>
		/// true:实例化使用Prefab默认 false:default
		/// </summary>
		private bool _matchPoolLayer = false;
		/// <summary>
		/// true:实例对象无父节点 false:prefab默认
		/// </summary>
		private bool _dontReparent = false;
		/// <summary>
		/// 对象池不销毁
		/// </summary>
		private bool _dontDestroyOnLoad = true;
		/// <summary>
		/// 是否打印控制带信息
		/// </summary>
		private bool _logMessage = false;
		/// <summary>
		/// 唯一对象池
		/// </summary>
		private SpawnPool _objectPool;
		
		#endregion ////////////////////////////////////////////////////

		#region ////////////////////池子公共默认属性/////////////////////
		/// <summary>
		/// 缓存吃保存最大数量
		/// </summary>
		private int _preloadAmount = 0;
		
		
		/// <summary>
		/// 池子内物体是否异步加载
		/// </summary>
		private bool _preloadTime = false;
		/// <summary>
		/// 几帧加载一个
		/// </summary>
		private int _preloadFrames = 2;
		/// <summary>
		/// 延迟几帧加载
		/// </summary>
		private int _preloadDelay = 0;
		
		
		/// <summary>
		/// 是否限制最大数量
		/// </summary>
		private bool _limitInstances = false;
		/// <summary>
		/// 最大实例数量 和 上preloadAmount冲突 以LimitAmount为准
		/// </summary>
		private int _limitAmount = 50;
		/// <summary>
		/// 如果我们限制了缓存池里面只能有10个Prefab，如果不勾选它，那么你拿第11个的时候就会返回null。
		/// 如果勾选它在取第11个的时候他会返回给你前10个里最不常用的那个。
		/// </summary>
		private bool _limitFIFO = false;

		
		/// <summary>
		/// 是否自动清理池子内不活跃物体
		/// </summary>
		private bool _cullDespawned = true;
		/// <summary>
		/// 池子内始终保持的数量
		/// </summary>
		private int _cullAbove = 0;
		/// <summary>
		/// 多久清理一次池子 单位S
		/// </summary>
		private int _cullDelay = 30;
		/// <summary>
		/// 每次清理多少
		/// </summary>
		private int _cullMaxPerPass = 60;
		
		/// <summary>
		/// 是否打印相关日志
		/// </summary>
		private bool _logPoolMessage = false;

		#endregion ///////////////////////////////////////////////////

		/// <summary>
		/// 获取目标实例
		/// </summary>
		/// <param name="prefab"></param>
		/// <returns></returns>
		public Transform Spawn(Transform prefab)
		{
			_objectPool = GetSpawnPool();
			InitPrefabPool(prefab);
			return _objectPool.Spawn(prefab);
		}

		/// <summary>
		/// 获取实例并设置父级
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public Transform Spawn(Transform prefab, Transform parent)
		{
			_objectPool = GetSpawnPool();
			InitPrefabPool(prefab);
			return _objectPool.Spawn(prefab, parent);
		}

		/// <summary>
		/// 获取实例并设置位置和旋转
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		/// <returns></returns>
		public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot)
		{
			_objectPool = GetSpawnPool();
			InitPrefabPool(prefab);
			return _objectPool.Spawn(prefab, pos,rot);
		}

		/// <summary>
		/// 获取实例并设置位置旋转父级
		/// </summary>
		/// <param name="prefab"></param>
		/// <param name="pos"></param>
		/// <param name="rot"></param>
		/// <param name="parent"></param>
		/// <returns></returns>
		public Transform Spawn(Transform prefab, Vector3 pos, Quaternion rot, Transform parent)
		{
			_objectPool = GetSpawnPool();
			InitPrefabPool(prefab);
			return _objectPool.Spawn(prefab, pos,rot,parent);
		}
		
		/// <summary>
		/// 释放目标
		/// </summary>
		/// <param name="instance"></param>
        public void Despawn(Transform instance)
        {
           _objectPool.Despawn(instance);
        }

		/// <summary>
		/// 释放目标到固定目标下
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="parent"></param>
        public void Despawn(Transform instance, Transform parent)
        {
	        _objectPool.Despawn(instance,parent);
        }

		/// <summary>
		/// 延迟释放目标
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="seconds"></param>
        public void Despawn(Transform instance, float seconds)
        {
            _objectPool.Despawn(instance,seconds);
        }

		/// <summary>
		/// 延迟释放目标到
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="seconds"></param>
		/// <param name="parent"></param>
        public void Despawn(Transform instance, float seconds, Transform parent)
        {
	        _objectPool.Despawn(instance,seconds,parent);
        }
		
		/// <summary>
		/// 清空池子
		/// </summary>
		public void DespawnAll()
		{
			_objectPool.DespawnAll();
		}


		/// <summary>
		/// 获取池子
		/// </summary>
		/// <returns></returns>
		private SpawnPool GetSpawnPool()
		{
			if (_objectPool!=null&&_objectPool.poolName == _poolName)
			{
				return _objectPool;
			}
			
			GameObject poolManager = GameObject.Find("PoolManager");
			if (poolManager==null)
			{
				poolManager = new GameObject("PoolManager");
				if (_dontDestroyOnLoad) Object.DontDestroyOnLoad(poolManager);
			}

			SpawnPool spawnPool = poolManager.GetComponent<SpawnPool>();
			if (spawnPool==null)
			{
				spawnPool = poolManager.AddComponent<SpawnPool>();
				spawnPool.poolName = _poolName;
				spawnPool.matchPoolScale = _matchPoolScale;
				spawnPool.matchPoolLayer = _matchPoolLayer;
				spawnPool.dontReparent = _dontReparent;
				spawnPool.logMessages = _logMessage;
			}
			return spawnPool;
		}

		/// <summary>
		/// 初始化单一对象池子，并设置默认属性
		/// </summary>
		/// <param name="prefab"></param>
		private void InitPrefabPool(Transform prefab)
		{
			if (_objectPool.GetPrefabPool(prefab)==null)
			{
				PrefabPool prefabPool = new PrefabPool(prefab);
				prefabPool.preloadAmount = _preloadAmount;
				prefabPool.preloadTime = _preloadTime;
				if (_preloadTime)
				{
					prefabPool.preloadFrames = _preloadFrames;
					prefabPool.preloadDelay = _preloadDelay;
				}
				prefabPool.limitInstances = _limitInstances;
				if (_limitInstances)
				{
					prefabPool.limitAmount = _limitAmount;
					prefabPool.limitFIFO = _limitFIFO;
				}
				prefabPool.cullDespawned = _cullDespawned;
				if (_cullDespawned)
				{
					prefabPool.cullAbove = _cullAbove;
					prefabPool.cullDelay = _cullDelay;
					prefabPool.cullMaxPerPass = _cullMaxPerPass;
				}
				prefabPool._logMessages = _logPoolMessage;
				
				_objectPool._perPrefabPoolOptions.Add(prefabPool);
				_objectPool.CreatePrefabPool(prefabPool);
			}
		}

	}
}
