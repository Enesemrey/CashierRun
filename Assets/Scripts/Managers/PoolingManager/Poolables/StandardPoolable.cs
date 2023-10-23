using System;
using UnityEngine;

public class StandardPoolable : MonoBehaviour, IDisposable
{
    [HideInInspector] public bool insidePool;
    protected Pool pool;
    protected Transform poolParent;

    #region Events
    public event Action<StandardPoolable> PoolLeaveEvent;
    public event Action<StandardPoolable> PoolReturnEvent;
    #endregion

    #region Handling Pooling
    public void HandlePoolLeave()
    {
        insidePool = false;
        gameObject.SetActive(true);

        PoolLeaveEvent?.Invoke(this);
    }

    public void HandlePoolReturn()
    {
        if (transform.parent != poolParent)
        {
            transform.parent = poolParent;
        }

        gameObject.SetActive(false);
        insidePool = true;

        PoolReturnEvent?.Invoke(this);
    }
    #endregion

    public void Dispose()
    {
        if (pool != null)
        {
            pool.Reclaim(this);
        }

        else
        {
            Destroy(gameObject);
        }
    }

    public void InitPoolable(Pool pool, Transform parent)
    {
        this.pool = pool;
        this.poolParent = parent;

        insidePool = true;
        transform.parent = poolParent;
        gameObject.SetActive(false);
    }
}
