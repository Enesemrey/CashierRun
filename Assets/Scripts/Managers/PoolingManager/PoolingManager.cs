using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolingManager : MonoSingleton<PoolingManager>
{
    [SerializeField] private bool persistent;
    [SerializeField] private PoolRecipe[] poolRecipes;

    private Dictionary<PoolRecipe, Pool> pools;

    #region Unity Callbacks
    protected override void Awake()
    {
        base.Awake();

        if (IsStray()) return; // we are a stray singleton so we should not init

        if (persistent)
        {
            transform.parent = null; // we need to be a root gameobject for DontDestroyOnLoad
            DontDestroyOnLoad(gameObject);
        }

        // we can init now
        pools = new Dictionary<PoolRecipe, Pool>();
        foreach (PoolRecipe poolRecipe in poolRecipes)
        {
            Pool pool = new Pool(poolRecipe, transform);
            pools.Add(poolRecipe, pool);
        }
    }

    private void Start()
    {
        if (persistent) GameManager.instance.LevelAboutToChangeEvent += ReclaimAllPools;
    }
    #endregion

    #region API
    public StandardPoolable Request(PoolRecipe recipe)
    {
        return pools[recipe].Request();
    }

    public T Request<T>(PoolRecipe recipe) where T : StandardPoolable
    {
        return Request(recipe) as T;
    }

    public void ReclaimPool(PoolRecipe recipe)
    {
        pools[recipe].ReclaimAll();
    }

    public void ReclaimAllPools()
    {
        foreach (Pool pool in pools.Values)
        {
            pool.ReclaimAll();
        }
    }
    #endregion
}
