using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteAlways]
public class Building : MonoBehaviour {

    [SerializeField]
    protected GameObject fab;
    [SerializeField]
    protected int count = 20;
    [SerializeField]
    protected Vector4 size = new Vector4(1, 5, 1, 1);
    [SerializeField]
    protected Vector4 perturb = new Vector4(0.2f, 0f, 0f, 0f);

    protected bool invalidated = true;
    protected List<GameObject> buildings = new List<GameObject>();

    private void OnEnable() {
        Invalidate();
    }
    private void OnDisable() {
        DestroyAll();
    }
    private void OnValidate() {
        Invalidate();
    }
    private void Update() {
        CheckCreation();
    }

    #region method

    private void CheckCreation() {
        if (invalidated) {
            DestroyAll();
            CreateAll();
            invalidated = false;
        }
    }

    protected void Invalidate() {
        invalidated = true;
    }
    private void CreateAll() {
        if (fab == null)
            return;
        var bounds = new Vector2(
            size.x * count + size.w * (count - 1),
            size.z * count + size.w * (count - 1)
            );
        var frontleft = -0.5f * bounds;

        var offsetY = frontleft.y;
        for (var y = 0; y < count; y++) {
            var offsetX = frontleft.x;
            for (var x = 0; x < count; x++) {
                var pos = new Vector3(
                    offsetX + (0.5f + perturb.x.SRange()) * size.x,
                    0f,
                    offsetY + (0.5f + perturb.x.SRange()) * size.y);
                var inst = Instantiate(fab);
                inst.hideFlags = HideFlags.DontSave;
                inst.transform.SetParent(transform);
                inst.transform.localPosition = pos;
                inst.transform.localScale = size;
                buildings.Add(inst);

                offsetX += size.x + size.w;
            }
            offsetY += size.y + size.w;
        }
    }

    private void DestroyAll() {
        for (var i = 0; i < buildings.Count; i++) {
            buildings[i].DestroyGo();
        }
        buildings.Clear();
    }
    #endregion
}
