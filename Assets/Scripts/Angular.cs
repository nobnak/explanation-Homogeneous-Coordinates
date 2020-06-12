using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Angular : MonoBehaviour {

    [SerializeField]
    protected Transform sourceFrom;
    [SerializeField]
    protected Transform sourceTo;
    [SerializeField]
    protected Transform angleBasedOn;
    [SerializeField]
    [Range(0f, 1f)]
    protected float throttle = 0f;

    void Update() {
        if (sourceFrom != null && sourceTo != null) {
            var rot = sourceTo.rotation * Quaternion.Inverse(sourceFrom.rotation);

            var targetFrom = Quaternion.identity;
            if (angleBasedOn != null) {
                targetFrom = Quaternion.Inverse(angleBasedOn.rotation);
            }
            transform.rotation = Quaternion.Lerp(Quaternion.identity, rot, throttle)
                * targetFrom;
        }

    }
}
