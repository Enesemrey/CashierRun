using System.Collections.Generic;
using UnityEngine;

public class Pool
{
    public enum PoolType
    {
        Static, Dynamic
    }

    private PoolType poolType;
    private StandardPoolable poolablePrefab;
    private Transform poolableParent;
    private Queue<StandardPoolable> queue;
    private List<StandardPoolable> actives;

    public Pool(PoolRecipe recipe, Transform manager)
    {
        poolableParent = new GameObject().transform;
        poolableParent.parent = manager;
        poolableParent.gameObject.name = recipe.poolablePrefab.name + "_Parent";

        poolType = recipe.poolType;
        poolablePrefab = recipe.poolablePrefab;

        actives = new List<StandardPoolable>();
        queue = new Queue<StandardPoolable>();

        for (int i = 0; i < recipe.startingPoolSize; i++)
        {
            queue.Enqueue(CreatePoolable());
        }
    }

    public StandardPoolable CreatePoolable()
    {
        StandardPoolable poolable = Object.Instantiate(poolablePrefab);
        poolable.InitPoolable(this, poolableParent);
        return poolable;
    }

    public StandardPoolable Request()
    {
        StandardPoolable poolable;

        if (queue.Count > 0)
        {
            poolable = queue.Dequeue();
        }

        else
        {
            Logger.Log("No more inactive objects inside pool with parent: " + poolableParent.name + ". Pool type: " + poolType.ToString(), Logger.LogLevel.Warning);

            if (poolType == PoolType.Static)
            {
                poolable = actives[0];
                actives.RemoveAt(0);
            }

            else
            {
                poolable = CreatePoolable();
            }
        }

        poolable.HandlePoolLeave();
        actives.Add(poolable);
        return poolable;
    }

    public void Reclaim(StandardPoolable poolable)
    {
        if (!poolable.insidePool)
        {
            poolable.HandlePoolReturn();
            queue.Enqueue(poolable);
            actives.Remove(poolable);
        }
    }

    public void ReclaimAll()
    {
        while (actives.Count > 0)
        {
            Reclaim(actives[0]);
        }
    }
}