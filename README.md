UnityPool_PoolManager��������ع���
������PoolManager�����
1��ͼ��

![image](https://github.com/KingSun5/UnityPool_PoolManager/blob/master/Images/PoolMgr.png)

2���÷�

		//ʵ����
		PoolMgr poolMgr = new PoolMgr();
		//ֱ�ӻ�ȡʵ��
		poolMgr.Spawn(prefab);
		//��ȡʵ�������ø�����
		poolMgr.Spawn(prefab,parent);
		//��ȡʵ��������λ����ת
		poolMgr.Spawn(prefab,pos,rot);
		//��ȡʵ��������λ����ת������
		poolMgr.Spawn(prefab,pos,rot,parent);
		
		//�ͷ�����
		poolMgr.Despawn(prefab);
		//�ͷ����嵽parent��
		poolMgr.Despawn(prefab,parent);
		//�ͷų��������л�Ծ����
		poolMgr.DespawnAll();


