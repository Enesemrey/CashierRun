using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PoolRecipe", menuName = "Nitrogen/New PoolRecipe")]
public class PoolRecipe : ScriptableObject
{
    public int startingPoolSize;
    public Pool.PoolType poolType;
    public StandardPoolable poolablePrefab;
}
