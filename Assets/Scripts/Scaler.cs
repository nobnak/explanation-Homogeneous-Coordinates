using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class Scaler : MonoBehaviour {

    [SerializeField]
    protected Camera targetCamera = null;
    [SerializeField]
    protected float distance = 1f;
    [SerializeField]
    [Range(0f, 1f)]
    protected float scale = 1f;

    private void OnEnable() {
    }
    private void Update() {
        if (targetCamera != null) {
            var c = targetCamera;
            var center = c.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
            var size = Vector3.Distance(
                    c.ViewportToWorldPoint(new Vector3(0f, 0f, distance)),
                    c.ViewportToWorldPoint(new Vector3(0f, 1f, distance))
                    );

            transform.position = center;
            transform.localScale = new Vector3(size * scale, size * scale, 1);
        }
    }
}
