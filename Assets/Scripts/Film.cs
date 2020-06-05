using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class Film : MonoBehaviour {

    [SerializeField]
    protected float distance = 1f;
    [SerializeField]
    protected Camera targetCamera = null;

    protected Renderer attachedRenderer;
    protected Texture texture;
    protected MaterialPropertyBlock block;

    private void OnEnable() {
        block = new MaterialPropertyBlock();
    }
    private void Update() {
        var r = GetRenderer();
        if (r != null) {
            r.Set(block, texture);
        }

        if (targetCamera != null) {
            var c = targetCamera;
            var center = c.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
            var size = Vector3.Distance(
                    c.ViewportToWorldPoint(new Vector3(0f, 0f, distance)),
                    c.ViewportToWorldPoint(new Vector3(0f, 1f, distance))
                    );

            transform.position = center;
            transform.rotation = c.transform.rotation;
            transform.localScale = new Vector3(size, size, 1);
        }
    }

    #region methods
    public void SetTexture(Texture texture) {
        this.texture = texture;
    }

    protected Renderer GetRenderer() {
        if (attachedRenderer == null)
            attachedRenderer = GetComponent<Renderer>();
        return attachedRenderer;
    }   
    #endregion
}
