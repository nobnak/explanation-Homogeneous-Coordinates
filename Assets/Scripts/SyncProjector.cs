using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class SyncProjector : MonoBehaviour {

    [SerializeField]
    protected Camera sourceCamera;

    protected Projector attachedProjector;

    #region unity
    private void Update() {
        var c = sourceCamera;
        if (c != null) {
            var p = GetProjector();

            p.transform.localPosition = c.transform.localPosition;
            p.transform.localRotation = c.transform.localRotation;

            p.aspectRatio = c.aspect;
            p.farClipPlane = c.farClipPlane;
            p.nearClipPlane = c.nearClipPlane;
            p.fieldOfView = c.fieldOfView;
            p.orthographic = c.orthographic;
            p.orthographicSize = c.orthographicSize;
        }
    }
    #endregion


    #region member
    public Projector GetProjector() {
        if (attachedProjector == null)
            attachedProjector = GetComponent<Projector>();
        return attachedProjector;
    }
    #endregion
}
