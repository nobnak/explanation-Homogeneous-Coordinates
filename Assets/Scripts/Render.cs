using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteAlways]
public class Render : MonoBehaviour {

    [SerializeField]
    protected TextureEvent Changed = new TextureEvent();

    protected RenderTexture captured = null;
    protected Camera attachedCam = null;

    private void Update() {
        var c = GetCamera();
        if (c != null) {
            var resolution = new Vector2Int(c.pixelWidth, c.pixelHeight);
            if (captured == null
                || captured.width != resolution.x
                || captured.height != resolution.y) {
                ReleaseCapturedTexture();
                captured = CreateTexture(resolution);
                SetTexture(captured);
            }
        }
    }
    private void OnDisable() {
        ReleaseCapturedTexture();
        attachedCam = null;
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
        var c = GetCamera();
        if (c != null) {
            c.targetTexture = captured;
        }

        Changed.Invoke(captured);
    }

    #endregion
}
