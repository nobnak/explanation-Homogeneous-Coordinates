using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Util {

    public static void DestroySelf(this Object obj) {
        if (obj == null)
            return;

        if (Application.isPlaying)
            Object.Destroy(obj);
        else
            Object.DestroyImmediate(obj);
    }
    public static void DestroyGo(this Component obj) {
        obj?.gameObject?.DestroySelf();
    }
    public static void DestroyGo(this GameObject obj) {
        obj?.DestroySelf();
    }
    public static float SRange(this float size) {
        return Random.Range(-0.5f * size, 0.5f * size);
    }
}
