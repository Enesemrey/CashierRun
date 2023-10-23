using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbianceManager : MonoSingleton<AmbianceManager>
{
    [Header("Skybox")]
    public Color horizonColor;
    public Color skyColor;
    [Range(0.01f, 1f)] public float horizonLength;
    [Range(-1f, 1f)] public float horizonShift;

    [Header("Fog")]
    public Color fogColor;

    public float linearFogStart = 100;
    public float linearFogDepth = 100;
    [Range(0.1f, 15)] public float linearFogBlend = 1;

    public float verticalFogStart = -1;
    public float verticalFogDepth = 10;
    [Range(0.1f, 15)] public float verticalFogBlend = 1;

    private void OnValidate()
    {
        ApplyParams();
    }

    protected override void Awake()
    {
        base.Awake();

        ApplyParams();
    }

    public void ApplyParams()
    {
        Shader.SetGlobalColor("_horizonColor", horizonColor.linear);
        Shader.SetGlobalColor("_skyColor", skyColor.linear);
        Shader.SetGlobalFloat("_horizonLength", horizonLength);
        Shader.SetGlobalFloat("_horizonShift", -horizonShift);

        Shader.SetGlobalColor("_fogColor", fogColor.linear);

        Shader.SetGlobalFloat("_linearFogStart", linearFogStart);
        Shader.SetGlobalFloat("_linearFogDepth", linearFogDepth);
        Shader.SetGlobalFloat("_linearFogPow", linearFogBlend);

        Shader.SetGlobalFloat("_verticalFogStartHeight", verticalFogStart);
        Shader.SetGlobalFloat("_verticalFogDepth", verticalFogDepth);
        Shader.SetGlobalFloat("_verticalFogBlend", verticalFogBlend);
    }
}
