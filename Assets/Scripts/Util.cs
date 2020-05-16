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
}
