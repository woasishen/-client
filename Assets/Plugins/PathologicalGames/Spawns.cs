using System.Collections.Generic;
using UnityEngine;

namespace PathologicalGames
{
    public sealed class Spawns
    {
        #region 私有成员
        private readonly GameObject _spawnPools = new GameObject("SpawnPools");
        private readonly List<SpawnPool> _poolList = new List<SpawnPool>();

        private readonly SpawnPool _viewCellPool;

        #endregion

        private static readonly Spawns instance = new Spawns();

        public static Spawns Instance
        {
            get { return instance; }
        }

        public Spawns()
        {
            Object.DontDestroyOnLoad(_spawnPools);

            InitPool(out _viewCellPool, "ViewCell");
        }

        #region 对外Pools

        public SpawnPool ViewCellPool
        {
            get { return _viewCellPool; }
        }

        #endregion

        #region 对外函数

        public bool CheckAndPrepareSpawnPool(SpawnPool spawnPool, string path)
        {
            if (spawnPool.prefabs.ContainsKey(path))
            {
                return false;
            }

            var trsf = Resources.Load<GameObject>(path).transform;
            trsf.name = path;
            spawnPool.CreatePrefabPool(new PrefabPool(trsf)
            {
                cullDespawned = true, //开启自动清理
                cullDelay = 60, //60s自动清理一次
                cullMaxPerPass = 5, //每一帧最多自动清理5个
                cullAbove = 20, //保留20个复制品
            });
            return true;
        }

        public void DeSpawnAll()
        {
            //倒序，根据init的顺序despawn
            for (int i = _poolList.Count; i > 0; i--)
            {
                _poolList[i - 1].DespawnAll();
            }
        }

        #endregion

        private void InitPool(out SpawnPool pool, string poolName)
        {
            var poolRoot = new GameObject(poolName + "Root");
            poolRoot.transform.parent = _spawnPools.transform;
            pool = PoolManager.Pools.Create(poolName, poolRoot);
            pool.dontReparent = false;
            _poolList.Add(pool);
        }
    }
}
