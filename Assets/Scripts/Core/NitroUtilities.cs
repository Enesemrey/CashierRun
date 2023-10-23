using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class NitroUtilities
{
    static readonly Vector3 redHSV = new Vector3(0, 1, 1);
    static readonly Vector3 greenHSV = new Vector3(120f/360f, 1, 1);

    public static Color LerpFromRedToGreen(float t)
    {
        Vector3 hsvColor = Vector3.Lerp(redHSV, greenHSV, t);
        return Color.HSVToRGB(hsvColor.x, hsvColor.y, hsvColor.z);
    }

    public static void Shuffle<T>(this IList<T> list)
    {
        int count = list.Count;
        int last = count - 1;
        for (int i = 0; i < last; ++i)
        {
            int r = Random.Range(i, count);
            T tmp = list[i];
            list[i] = list[r];
            list[r] = tmp;
        }
    }

    public static Vector3 ParabolicLerp(Vector3 start, Vector3 end, float height, float t)
    {
        static float ParabolaFunction(float x, float h)
        {
            return (-2 * h) * x * x + ((2 * h) * x);
        }

        Vector3 tPoint = Vector3.Lerp(start, end, t);
        return new Vector3(tPoint.x, ParabolaFunction(t, height) + Mathf.Lerp(start.y, end.y, t), tPoint.z);
    }

    static public void DrawTransformGizmo(Transform target, Color color, float size = 0.25f)
    {
        if (target != null)
        {
            Gizmos.color = color;
            Gizmos.DrawSphere(target.position, size);
        }
    }

    public static Vector3 GetScreenRelativeTouchPos()
    {
        return Input.mousePosition / Screen.width;
    }
}
