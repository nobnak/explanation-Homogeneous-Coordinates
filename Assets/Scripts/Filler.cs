using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class Filler : MonoBehaviour {

    [SerializeField]
    protected Transform quad;
    [SerializeField]
    protected float distance = 1f;

    [SerializeField]
    protected TextureEvent Changed = new TextureEvent();

    protected RenderTexture captured = null;
    protected Camera attachedCam = null;

    private void Update() {
        var c = GetCamera();
        if (c != null) {
            var center = c.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, distance));
            var size = new Vector3(
                Vector3.Distance(
                    c.ViewportToWorldPoint(new Vector3(0f, 0f, distance)),
                    c.ViewportToWorldPoint(new Vector3(1f, 0f, distance))),
                Vector3.Distance(
                    c.ViewportToWorldPoint(new Vector3(0f, 0f, distance)),
                    c.ViewportToWorldPoint(new Vector3(0f, 1f, distance))),
                1f);

            var resolution = new Vector2Int(c.pixelWidth, c.pixelHeight);
            if (captured == null
                || captured.width != resolution.x
                || captured.height != resolution.y) {
                ReleaseCapturedTexture();
                captured = CreateTexture(resolution);
                SetTexture(captured);
            }

            if (quad != null) {
                quad.position = center;
                quad.rotation = c.transform.rotation;
                quad.localScale = size;
            }
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        if (captured != null) {
            Graphics.Blit(source, captured);
        }
        Graphics.Blit(source, destination);
    }
    private void OnDisable() {
        ReleaseCapturedTexture();
    }

    #region methods
    private RenderTexture CreateTexture(Vector2Int resolution) {
        return new RenderTexture(resolution.x, resolution.y, 0);
    }
    private void ReleaseCapturedTexture() {
        SetTexture(null);
        captured.DestroySelf();
        captured = null;
    }

    private Camera GetCamera() {
        if (attachedCam == null)
            attachedCam = GetComponent<Camera>();
        return attachedCam;
    }

    private void SetTexture(RenderTexture captured) {
        if (quad != null) {
            var renderer = quad.GetComponent<Renderer>();
            if (renderer != null) {
                var mat = renderer.sharedMaterial;
                mat.mainTexture = captured;
            }
        }

        Changed.Invoke(captured);
    }

    #endregion

    [System.Serializable]
    public class TextureEvent : UnityEvent<Texture> { }
}
