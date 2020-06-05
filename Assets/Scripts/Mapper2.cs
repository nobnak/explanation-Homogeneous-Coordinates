using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Mapper2 : MonoBehaviour {

    public TextureEvent Changed = new TextureEvent();

    public Texture defaultInputTexture;
    public Camera targetCamera;
    public Renderer panel;
    public Transform viewer;
    public Transform projector;

    protected Texture inputTex;
    protected RenderTexture outputTex;
    protected MaterialPropertyBlock block;

    #region unity
    private void OnEnable() {
        block = new MaterialPropertyBlock();
    }
    private void OnDisable() {
        ReleaseOutputTex();
    }
    private void Update() {
        var c = GetCamera();
        var input = GetInputTexture();

        if (input == null)
            return;

        if (outputTex == null
            || outputTex.width != input.width 
            || outputTex.height != input.height) {
            ReleaseOutputTex();
            outputTex = new RenderTexture(input.width, input.height, 24);
            SetAsOutput(outputTex);
        }

        if (panel != null) {
            panel.Set(block, input);
        }

        if (panel != null && viewer != null && projector != null) {
            var q = viewer.rotation * Quaternion.Inverse(projector.rotation);
            panel.transform.rotation = q;
        }
    }
    #endregion

    #region interface
    public void SetInputTexture(Texture tex) {
        this.inputTex = tex;
    }
    #endregion

    #region method
    protected Texture GetInputTexture() {
        return inputTex == null ? defaultInputTexture : inputTex;
    }
    protected Camera GetCamera() {
        return (targetCamera == null) ? Camera.main : targetCamera;
    }
    protected void SetAsOutput(RenderTexture tex) {
        var c = GetCamera();
        c.targetTexture = tex;
        Changed.Invoke(tex);
    }
    protected void ReleaseOutputTex() {
        SetAsOutput(null);
        outputTex.DestroySelf();
    }

    #endregion
}
