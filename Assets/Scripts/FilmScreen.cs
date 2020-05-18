using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class FilmScreen : MonoBehaviour {

    [SerializeField]
    protected float distance = 1f;
    [SerializeField]
    protected Camera targetCamera = null;

    protected Renderer attachedRenderer;
    protected Texture texture;

    private void Update() {
        var r = GetRenderer();
        if (r != null) {
            var mat = r.sharedMaterial;
            mat.mainTexture = texture;
        }

        if (targetCamera != null) {
            var c = targetCamera;
            var center = c.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
            var size = new Vector3(
                Vector3.Distance(
                    c.ViewportToWorldPoint(new Vector3(0f, 0f, distance)),
                    c.ViewportToWorldPoint(new Vector3(1f, 0f, distance))),
                Vector3.Distance(
                    c.ViewportToWorldPoint(new Vector3(0f, 0f, distance)),
                    c.ViewportToWorldPoint(new Vector3(0f, 1f, distance))),
                1f);

            transform.position = center;
            transform.rotation = c.transform.rotation;
            transform.localScale = size;
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
