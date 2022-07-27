using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MeshMaskUI : MaskableGraphic {


    protected override void OnPopulateMesh(VertexHelper vertexHelper) {
        vertexHelper.Clear();

        Vector3 vec_00 = new Vector3(0, 0);
        Vector3 vec_01 = new Vector3(0, 50);
        Vector3 vec_10 = new Vector3(50 + Time.realtimeSinceStartup * 10f, 0);
        Vector3 vec_11 = new Vector3(50 + Time.realtimeSinceStartup * 10f, 50);

        vertexHelper.AddUIVertexQuad(new UIVertex[] {
            new UIVertex { position = vec_00, color  = Color.green },
            new UIVertex { position = vec_01, color  = Color.green },
            new UIVertex { position = vec_11, color  = Color.green },
            new UIVertex { position = vec_10, color  = Color.green },
        });
    }

    private void Update() {
        SetVerticesDirty();
    }

}
