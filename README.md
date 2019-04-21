UnityPool_PoolManager——对象池管理
【基于PoolManager插件】
1、图解

![image](https://github.com/KingSun5/UnityPool_PoolManager/blob/master/Images/PoolMgr.png)

2、用法

		//实例化
		PoolMgr poolMgr = new PoolMgr();
		//直接获取实例
		poolMgr.Spawn(prefab);
		//获取实例并设置父物体
		poolMgr.Spawn(prefab,parent);
		//获取实例并设置位置旋转
		poolMgr.Spawn(prefab,pos,rot);
		//获取实例并设置位置旋转，父级
		poolMgr.Spawn(prefab,pos,rot,parent);
		
		//释放物体
		poolMgr.Despawn(prefab);
		//释放物体到parent下
		poolMgr.Despawn(prefab,parent);
		//释放池子内所有活跃物体
		poolMgr.DespawnAll();


