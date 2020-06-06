using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Mapper2 : MonoBehaviour {

    public TextureEvent Changed = new TextureEvent();

    public Texture defaultInputTexture;
    public Camera targetCamera;
    public Renderer panel;

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
    protected void SetAsOutput(RenderTexture tex) {
        if (targetCamera != null)
            targetCamera.targetTexture = tex;
        Changed.Invoke(tex);
    }
    protected void ReleaseOutputTex() {
        SetAsOutput(null);
        if (outputTex != null) {
            outputTex.DestroySelf();
            outputTex = null;
        }
    }
    #endregion
}
