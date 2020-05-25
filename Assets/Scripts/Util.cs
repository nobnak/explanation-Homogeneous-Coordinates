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

    public static Vector2Int LOD(this Vector2Int resolusion, int lod = 0) {
        var r = resolusion;
        if (lod > 0) {
            r.x >>= lod;
            r.y >>= lod;
        }
        if (lod < 0) {
            lod = Mathf.Abs(lod);
            r.x <<= lod;
            r.y <<= lod;
        }
        return r;
    }
}
