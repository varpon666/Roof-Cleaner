using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplineMesh : MonoBehaviour {

    [SerializeField] private Spline spline;
    [SerializeField] private float meshWidth = 1.5f;

    private Mesh mesh;
    private MeshFilter meshFilter;

    private void Awake() {
        if (spline == null) spline = GetComponent<Spline>();
        meshFilter = GetComponent<MeshFilter>();

        transform.position = Vector3.zero;
    }

    private void Start() {
        transform.position = spline.transform.position;

        UpdateMesh();

        spline.OnDirty += Spline_OnDirty;
    }

    private void Spline_OnDirty(object sender, EventArgs e) {
        UpdateMesh();
    }

    private void UpdateMesh() {
        if (mesh != null) {
            mesh.Clear();
            Destroy(mesh);
            mesh = null;
        }

        List<Spline.Point> pointList = spline.GetPointList();
        if (pointList.Count > 2) {
            Spline.Point point = pointList[0];
            Spline.Point secondPoint = pointList[1];
            mesh = MeshUtils.CreateLineMesh(point.position - transform.position, secondPoint.position - transform.position, point.normal, meshWidth);

            for (int i = 2; i < pointList.Count; i++) {
                Spline.Point thisPoint = pointList[i];
                MeshUtils.AddLinePoint(mesh, thisPoint.position - transform.position, thisPoint.forward, point.normal, meshWidth);
            }

            meshFilter.mesh = mesh;
        }
    }

}
