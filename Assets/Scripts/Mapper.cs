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
    protected TextureEvent Changed = new TextureEvent();

    protected Mesh mesh;
    protected Camera attachedCamera;
    protected Texture inputTexture;
    protected RenderTexture outputTexture;

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

        var c = GetCamera();
        var vertices = new Vector3[4];
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
        }

        if (meshfilter != null) {
            meshfilter.sharedMesh = mesh;
        }

        if (c != null) {
            if (outputTexture == null 
                || outputTexture.width != c.pixelWidth 
                || outputTexture.height != c.pixelHeight) {

                ReleaseOutputTexture();

                outputTexture = new RenderTexture(c.pixelWidth, c.pixelHeight, 24);
                Changed.Invoke(outputTexture);
            }
        }
    }
    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, outputTexture);
    }

    #region methods
    public void SetInputTexture(Texture tex) {
        this.inputTexture = tex;

        if (meshfilter != null) {
            var r = meshfilter.GetComponent<Renderer>();
            if (r != null && r.sharedMaterial != null)
                r.sharedMaterial.mainTexture = tex;
        }
    }

    private void ReleaseOutputTexture() {
        Changed.Invoke(null);
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
