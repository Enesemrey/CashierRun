using UnityEngine;

public abstract class MonoSingleton<T> : MonoBehaviour where T : Component
{
    public static T instance;

    protected virtual void Awake()
    {
        if (instance != null & instance != this)
        {
            Logger.Log("Duplicate singleton found for " + instance.name + ". Destroying this: " + gameObject.name, Logger.LogLevel.Warning);
            Destroy(gameObject);
        }

        else
        {
            instance = this as T;
        }
    }

    protected bool IsStray()
    {
        return this != instance;
    }
}