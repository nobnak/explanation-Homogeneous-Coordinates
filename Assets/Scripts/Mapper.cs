using nobnak.Gist.Extensions.GeometryExt;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[ExecuteAlways]
public class Mapper : MonoBehaviour {

    public static readonly Vector2[] KEYSTONES = new Vector2[] {
        new Vector2(0f, 0f),
        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f)
    };
    public static readonly Vector2[] UV = new Vector2[] {
        new Vector2(0f, 0f),
        new Vector2(0f, 1f),
        new Vector2(1f, 1f),
        new Vector2(1f, 0f)
    };
    public static readonly int[] INDICES = new int[] {
        0,1,2,
        0,2,3
    };

    [SerializeField]
    protected bool enabledMapping = true;
    [SerializeField]
    protected float distance = 1f;
    [SerializeField]
    protected Vector2[] keystones = KEYSTONES.ToArray();
    [SerializeField]
    protected MeshFilter meshfilter = null;
    [SerializeField]
    protected Texture defaultInputTexture = null;

    [SerializeField]
    protected TextureEvent Changed = new TextureEvent();

    protected Mesh mesh;
    protected Camera attachedCamera;
    protected Texture inputTexture;
    protected RenderTexture outputTexture;
    protected MaterialPropertyBlock block;

    private void OnEnable() {
        mesh = new Mesh();
    }
    private void OnDisable() {
        if (mesh != null) {
            mesh.DestroySelf();
            mesh = null;
        }
        ReleaseOutputTexture();
    }

    private void Update() {
        System.Array.Resize(ref keystones, 4);
        for (var i = 0; i < keystones.Length; i++) {
            var ks = keystones[i];
            for (var j = 0; j < 2; j++)
                ks[j] = Mathf.Clamp01(ks[j]);
            keystones[i] = ks;
        }

        var c = GetCamera();
        var vertices = new Vector3[4];
        var inputTex = GetInputTexture();

        if ((enabledMapping ? keystones : KEYSTONES)
            .TryBuildParallelorism(vertices)) {

            if (c != null && meshfilter != null) {
                var m = meshfilter.transform.worldToLocalMatrix;

                for (var i = 0; i < vertices.Length; i++) {
                    var v = vertices[i];
                    v.z *= distance;
                    v = c.ViewportToWorldPoint(v);
                    vertices[i] = m.MultiplyPoint3x4(v);
                }
            }

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.uv = UV;
            mesh.triangles = INDICES;
        } else {
            Debug.LogWarning("Failed to find a plane");
            keystones = KEYSTONES.ToArray();
        }

        if (meshfilter != null) {
            meshfilter.sharedMesh = mesh;
        }


        if (inputTex != null) {
            if (outputTexture == null 
                || outputTexture.width != inputTex.width
                || outputTexture.height != inputTex.height) {

                ReleaseOutputTexture();

                outputTexture = new RenderTexture(inputTex.width, inputTex.height, 24);
                SetOutputTexture(outputTexture);
            }
        }

        if (meshfilter != null) {
            var r = meshfilter.GetComponent<Renderer>();
            if (block == null)
                block = new MaterialPropertyBlock();
            if (r != null)
                r.Set(block, inputTex);
        }
    }

    #region methods
    public void SetInputTexture(Texture tex) {
        this.inputTexture = tex;
    }
    protected Texture GetInputTexture() {
        return (inputTexture != null) ? inputTexture : defaultInputTexture;
    }

    protected void SetOutputTexture(RenderTexture tex) {
        var c = GetCamera();
        if (c != null) {
            c.targetTexture = tex;
        }
        Changed.Invoke(tex);
    }
    private void ReleaseOutputTexture() {
        SetOutputTexture(null);
        outputTexture.DestroySelf();
        outputTexture = null;
    }
    protected Camera GetCamera() {
        if (attachedCamera == null)
            attachedCamera = GetComponent<Camera>();
        return attachedCamera;
    }
    #endregion
}
