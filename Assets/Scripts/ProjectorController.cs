using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ProjectorController : MonoBehaviour {

    [SerializeField]
    protected Texture defaultCookie = null;

    protected Texture cookie;
    protected Projector projector;

    private void Update() {
        var proj = GetProjector();
        if (proj != null) {
            var tex = GetTexture();
            var mat = proj.material;
            mat.mainTexture = tex;

            if (tex != null) {
                var aspect = (float)tex.width / tex.height;
                proj.aspectRatio = aspect;
            }
        }
    }

    #region methods
    public void SetTexture(Texture cookie) {
        this.cookie = cookie;
    }
    private Texture GetTexture() {
        return (cookie != null ? cookie : defaultCookie);
    }

    protected Projector GetProjector() {
        if (projector == null)
            projector = GetComponent<Projector>();
        return projector;
    }
    #endregion
}
