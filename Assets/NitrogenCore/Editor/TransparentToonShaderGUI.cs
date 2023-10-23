using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class TransparentToonShaderGUI : ToonShaderGUI
{
    public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
    {
        mainLabel = "NytroToon V3 - Transparent";
        hideAlphaOverride = true;

        base.OnGUI(materialEditor, properties);
    }
}
