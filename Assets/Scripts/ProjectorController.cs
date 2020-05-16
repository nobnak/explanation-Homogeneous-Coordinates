using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class ProjectorController : MonoBehaviour {

    protected Texture cookie;
    protected Projector projector;

    private void Update() {
        var proj = GetProjector();
        if (proj != null) {
            var mat = proj.material;
            mat.mainTexture = cookie;

            if (cookie != null) {
                var aspect = (float)cookie.width / cookie.height;
                proj.aspectRatio = aspect;
            }
        }
    }

    #region methods
    public void SetTexture(Texture cookie) {
        this.cookie = cookie;
    }

    protected Projector GetProjector() {
        if (projector == null)
            projector = GetComponent<Projector>();
        return projector;
    }
    #endregion
}
