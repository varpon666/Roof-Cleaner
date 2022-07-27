using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Window_Graph : MonoBehaviour {

    [SerializeField] private Window_Graph graph = null;

    private void Start() {
        List<int> valueList = new List<int>();

        for (int i = 0; i < Random.Range(50, 100); i++) {
            valueList.Add(Random.Range(0, 20));
        }

        graph.ShowGraph(valueList);
    }

}
