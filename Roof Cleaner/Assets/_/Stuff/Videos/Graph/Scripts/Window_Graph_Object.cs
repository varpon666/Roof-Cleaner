/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CodeMonkey.Utils;

public class Window_Graph_Object : MonoBehaviour {

    private bool isSetup;
    [SerializeField] private Sprite dotSprite = null;
    private RectTransform graphContainer;
    private RectTransform labelTemplateX;
    private RectTransform labelTemplateY;
    private RectTransform dashContainer;
    private RectTransform dashTemplateX;
    private RectTransform dashTemplateY;
    private List<GameObject> gameObjectList;
    private List<IGraphVisualObject> graphVisualObjectList;
    private GameObject tooltipGameObject;
    private List<RectTransform> yLabelList;

    public IGraphVisual lineGraphVisual;
    public IGraphVisual barChartVisual;

    // Cached values
    private IList valueList;
    private Func<object, float> getValueFunc;
    private IGraphVisual graphVisual;
    private int maxVisibleValueAmount;
    private Func<int, string> getAxisLabelX;
    private Func<float, string> getAxisLabelY;
    private Func<float, string> getTooltipLabel;
    private float xSize;
    private bool startYScaleAtZero;
    private float? fixedYMaximum;
    private float? fixedYMinimum;

    private void Awake() {
        Setup();
    }

    private void Setup() {
        if (isSetup) return;
        isSetup = true;
        // Grab base objects references
        graphContainer = transform.Find("graphContainer").GetComponent<RectTransform>();
        labelTemplateX = graphContainer.Find("labelTemplateX").GetComponent<RectTransform>();
        labelTemplateY = graphContainer.Find("labelTemplateY").GetComponent<RectTransform>();
        dashContainer = graphContainer.Find("dashContainer").GetComponent<RectTransform>();
        dashTemplateX = dashContainer.Find("dashTemplateX").GetComponent<RectTransform>();
        dashTemplateY = dashContainer.Find("dashTemplateY").GetComponent<RectTransform>();
        tooltipGameObject = graphContainer.Find("tooltip").gameObject;

        labelTemplateX.gameObject.SetActive(false);
        labelTemplateY.gameObject.SetActive(false);
        dashTemplateX.gameObject.SetActive(false);
        dashTemplateY.gameObject.SetActive(false);

        startYScaleAtZero = true;
        gameObjectList = new List<GameObject>();
        yLabelList = new List<RectTransform>();
        graphVisualObjectList = new List<IGraphVisualObject>();

        SetGetValueFunc(null);

        lineGraphVisual = new LineGraphVisual(graphContainer, dotSprite, Color.green, new Color(1, 1, 1, .5f), this);
        barChartVisual = new BarChartVisual(graphContainer, Color.white, .8f, this);

        valueList = new List<object> { 0 };

        HideTooltip();
    }

    private void ShowTooltip(string tooltipText, Vector2 anchoredPosition) {
        // Show Tooltip GameObject
        tooltipGameObject.SetActive(true);

        tooltipGameObject.GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

        Text tooltipUIText = tooltipGameObject.transform.Find("text").GetComponent<Text>();
        tooltipUIText.text = tooltipText;

        float textPaddingSize = 4f;
        Vector2 backgroundSize = new Vector2(
            tooltipUIText.preferredWidth + textPaddingSize * 2f,
            tooltipUIText.preferredHeight + textPaddingSize * 2f
        );

        tooltipGameObject.transform.Find("background").GetComponent<RectTransform>().sizeDelta = backgroundSize;

        // UI Visibility Sorting based on Hierarchy, SetAsLastSibling in order to show up on top
        tooltipGameObject.transform.SetAsLastSibling();
    }

    private void HideTooltip() {
        tooltipGameObject.SetActive(false);
    }

    public void SetGetValueFunc(Func<object, float> getValueFunc) {
        if (getValueFunc == null) {
            getValueFunc = GetValueFuncDefault;
        }
        this.getValueFunc = getValueFunc;
    }

    public void SetGetAxisLabelX(Func<int, string> getAxisLabelX) {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount, getAxisLabelX, this.getAxisLabelY);
    }

    public void SetGetAxisLabelY(Func<float, string> getAxisLabelY) {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, getAxisLabelY);
    }

    public void SetGetTooltipLabel(Func<float, string> getTooltipLabel) {
        this.getTooltipLabel = getTooltipLabel;
    }

    public void IncreaseVisibleAmount() {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount + 1, this.getAxisLabelX, this.getAxisLabelY);
    }

    public void DecreaseVisibleAmount() {
        ShowGraph(this.valueList, this.graphVisual, this.maxVisibleValueAmount - 1, this.getAxisLabelX, this.getAxisLabelY);
    }

    private void SetGraphVisual(IGraphVisual graphVisual) {
        ShowGraph(this.valueList, graphVisual, this.maxVisibleValueAmount, this.getAxisLabelX, this.getAxisLabelY);
    }

    public void ShowGraph(IList valueList, IGraphVisual graphVisual = null, int maxVisibleValueAmount = -1, Func<int, string> getAxisLabelX = null, Func<float, string> getAxisLabelY = null) {
        Setup();
        if (valueList == null) {
            valueList = new List<object> { 0 };
            //Debug.LogError("valueList is null!");
            //return;
        }
        this.valueList = valueList;

        if (graphVisual == null) {
            graphVisual = barChartVisual;
        }
        this.graphVisual = graphVisual;

        if (maxVisibleValueAmount <= 0) {
            // Show all if no amount specified
            maxVisibleValueAmount = valueList.Count;
        }
        if (maxVisibleValueAmount > valueList.Count) {
            // Validate the amount to show the maximum
            maxVisibleValueAmount = valueList.Count;
        }

        this.maxVisibleValueAmount = maxVisibleValueAmount;

        // Test for label defaults
        if (getAxisLabelX == null) {
            if (this.getAxisLabelX != null) {
                getAxisLabelX = this.getAxisLabelX;
            } else {
                getAxisLabelX = delegate (int _i) { return _i.ToString(); };
            }
        }
        if (getAxisLabelY == null) {
            if (this.getAxisLabelY != null) {
                getAxisLabelY = this.getAxisLabelY;
            } else {
                getAxisLabelY = delegate (float _f) { return Mathf.RoundToInt(_f).ToString(); };
            }
        }

        this.getAxisLabelX = getAxisLabelX;
        this.getAxisLabelY = getAxisLabelY;

        // Clean up previous graph
        foreach (GameObject gameObject in gameObjectList) {
            Destroy(gameObject);
        }
        gameObjectList.Clear();
        yLabelList.Clear();

        foreach (IGraphVisualObject graphVisualObject in graphVisualObjectList) {
            graphVisualObject.CleanUp();
        }
        graphVisualObjectList.Clear();

        graphVisual.CleanUp();

        // Grab the width and height from the container
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMinimum, yMaximum;
        CalculateYScale(out yMinimum, out yMaximum);

        // Set the distance between each point on the graph 
        xSize = graphWidth / (maxVisibleValueAmount + 1);

        // Cycle through all visible data points
        int xIndex = 0;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((GetValue(valueList[i]) - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            // Add data point visual
            string tooltipText;
            if (getTooltipLabel != null) {
                tooltipText = getTooltipLabel(GetValue(valueList[i]));
            } else {
                tooltipText = getAxisLabelY(GetValue(valueList[i]));
            }
            IGraphVisualObject graphVisualObject = graphVisual.CreateGraphVisualObject(new Vector2(xPosition, yPosition), xSize, tooltipText);
            graphVisualObjectList.Add(graphVisualObject);

            // Duplicate the x label template
            RectTransform labelX = Instantiate(labelTemplateX);
            labelX.SetParent(graphContainer, false);
            labelX.gameObject.SetActive(true);
            labelX.anchoredPosition = new Vector2(xPosition, -7f);
            labelX.GetComponent<Text>().text = getAxisLabelX(i);
            gameObjectList.Add(labelX.gameObject);

            // Duplicate the x dash template
            RectTransform dashX = Instantiate(dashTemplateX);
            dashX.SetParent(dashContainer, false);
            dashX.gameObject.SetActive(true);
            dashX.anchoredPosition = new Vector2(xPosition, -3f);
            dashX.sizeDelta = new Vector2(graphHeight, dashX.sizeDelta.y);
            gameObjectList.Add(dashX.gameObject);

            xIndex++;
        }

        // Set up separators on the y axis
        int separatorCount = 10;
        for (int i = 0; i <= separatorCount; i++) {
            // Duplicate the label template
            RectTransform labelY = Instantiate(labelTemplateY);
            labelY.SetParent(graphContainer, false);
            labelY.gameObject.SetActive(true);
            float normalizedValue = i * 1f / separatorCount;
            labelY.anchoredPosition = new Vector2(-7f, normalizedValue * graphHeight);
            labelY.GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
            yLabelList.Add(labelY);
            gameObjectList.Add(labelY.gameObject);

            // Duplicate the dash template
            RectTransform dashY = Instantiate(dashTemplateY);
            dashY.SetParent(dashContainer, false);
            dashY.gameObject.SetActive(true);
            dashY.anchoredPosition = new Vector2(-4f, normalizedValue * graphHeight);
            dashY.sizeDelta = new Vector2(graphWidth, dashY.sizeDelta.y);
            gameObjectList.Add(dashY.gameObject);
        }
    }

    private float GetValue(object value) {
        return getValueFunc(value);
    }

    private float GetValueFuncDefault(object value) {
        if (value is int) {
            return (float)(int)value;
        }
        return (float)value;
    }

    public void UpdateLastIndexValue(object value) {
        UpdateValue(valueList.Count - 1, value);
    }

    public void UpdateValue(int index, object value) {
        float yMinimumBefore, yMaximumBefore;
        CalculateYScale(out yMinimumBefore, out yMaximumBefore);

        valueList[index] = value;

        //float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMinimum, yMaximum;
        CalculateYScale(out yMinimum, out yMaximum);

        bool yScaleChanged = yMinimumBefore != yMinimum || yMaximumBefore != yMaximum;

        if (!yScaleChanged) {
            // Y Scale did not change, update only this value
            int xIndex = index - Mathf.Max(valueList.Count - maxVisibleValueAmount, 0);
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((GetValue(value) - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            // Add data point visual
            string tooltipText = getAxisLabelY(GetValue(value));
            graphVisualObjectList[xIndex].SetGraphVisualObjectInfo(new Vector2(xPosition, yPosition), xSize, tooltipText);
        } else {
            // Y scale changed, update whole graph and y axis labels
            // Cycle through all visible data points
            UpdateAllVisiblePoints();
        }
    }

    public void UpdateAllVisiblePoints() {
        float graphWidth = graphContainer.sizeDelta.x;
        float graphHeight = graphContainer.sizeDelta.y;

        float yMinimum, yMaximum;
        CalculateYScale(out yMinimum, out yMaximum);

        // Cycle through all visible data points
        int xIndex = 0;
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
            float xPosition = xSize + xIndex * xSize;
            float yPosition = ((GetValue(valueList[i]) - yMinimum) / (yMaximum - yMinimum)) * graphHeight;

            // Add data point visual
            string tooltipText = getAxisLabelY(GetValue(valueList[i]));
            graphVisualObjectList[xIndex].SetGraphVisualObjectInfo(new Vector2(xPosition, yPosition), xSize, tooltipText);

            xIndex++;
        }

        for (int i = 0; i < yLabelList.Count; i++) {
            float normalizedValue = i * 1f / (yLabelList.Count - 1);
            yLabelList[i].GetComponent<Text>().text = getAxisLabelY(yMinimum + (normalizedValue * (yMaximum - yMinimum)));
        }
    }

    private void CalculateYScale(out float yMinimum, out float yMaximum) {
        // Identify y Min and Max values
        if (fixedYMinimum != null && fixedYMaximum != null) {
            // Both fixed values
            yMinimum = fixedYMinimum.Value;
            yMaximum = fixedYMaximum.Value;
            return;
        }

        yMaximum = GetValue(valueList[0]);
        yMinimum = GetValue(valueList[0]);
        
        for (int i = Mathf.Max(valueList.Count - maxVisibleValueAmount, 0); i < valueList.Count; i++) {
            float value = GetValue(valueList[i]);
            if (value > yMaximum) {
                yMaximum = value;
            }
            if (value < yMinimum) {
                yMinimum = value;
            }
        }

        float yDifference = yMaximum - yMinimum;
        if (yDifference <= 0) {
            yDifference = 5f;
        }
        yMaximum = yMaximum + (yDifference * 0.2f);
        yMinimum = yMinimum - (yDifference * 0.2f);

        if (startYScaleAtZero) {
            yMinimum = 0f; // Start the graph at zero
        }
        
        if (fixedYMinimum != null) {
            yMinimum = fixedYMinimum.Value;
        }
        if (fixedYMaximum != null) {
            yMaximum = fixedYMaximum.Value;
        }
    }

    public void SetFixedYValues(float? fixedYMinimum, float? fixedYMaximum) {
        this.fixedYMinimum = fixedYMinimum;
        this.fixedYMaximum = fixedYMaximum;
    }



    /*
     * Interface definition for showing visual for a data point
     * */
    public interface IGraphVisual {

        IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
        void CleanUp();

    }

    /*
     * Represents a single Visual Object in the graph
     * */
    public interface IGraphVisualObject {

        void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText);
        void CleanUp();

    }


    /*
     * Displays data points as a Bar Chart
     * */
    private class BarChartVisual : IGraphVisual {

        private RectTransform graphContainer;
        private Color barColor;
        private float barWidthMultiplier;
        private Window_Graph_Object windowGraph;

        public BarChartVisual(RectTransform graphContainer, Color barColor, float barWidthMultiplier, Window_Graph_Object windowGraph) {
            this.graphContainer = graphContainer;
            this.barColor = barColor;
            this.barWidthMultiplier = barWidthMultiplier;
            this.windowGraph = windowGraph;
        }

        public void CleanUp() {
        }

        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
            GameObject barGameObject = CreateBar(graphPosition, graphPositionWidth);

            BarChartVisualObject barChartVisualObject = new BarChartVisualObject(barGameObject, barWidthMultiplier, windowGraph);
            barChartVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);

            return barChartVisualObject;
        }

        private GameObject CreateBar(Vector2 graphPosition, float barWidth) {
            GameObject gameObject = new GameObject("bar", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = barColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
            rectTransform.sizeDelta = new Vector2(barWidth * barWidthMultiplier, graphPosition.y);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.pivot = new Vector2(.5f, 0f);
            
            // Add Button_UI Component which captures UI Mouse Events
            gameObject.AddComponent<Button_UI>();
            //Button_UI barButtonUI = gameObject.AddComponent<Button_UI>();

            return gameObject;
        }


        public class BarChartVisualObject : IGraphVisualObject {

            private GameObject barGameObject;
            private float barWidthMultiplier;
            private Window_Graph_Object windowGraph;

            public BarChartVisualObject(GameObject barGameObject, float barWidthMultiplier, Window_Graph_Object windowGraph) {
                this.barGameObject = barGameObject;
                this.barWidthMultiplier = barWidthMultiplier;
                this.windowGraph = windowGraph;
            }

            public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
                RectTransform rectTransform = barGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(graphPosition.x, 0f);
                rectTransform.sizeDelta = new Vector2(graphPositionWidth * barWidthMultiplier, graphPosition.y);

                Button_UI barButtonUI = barGameObject.GetComponent<Button_UI>();

                // Show Tooltip on Mouse Over
                barButtonUI.MouseOverOnceFunc = () => {
                    windowGraph.ShowTooltip(tooltipText, graphPosition);
                };

                // Hide Tooltip on Mouse Out
                barButtonUI.MouseOutOnceFunc = () => {
                    windowGraph.HideTooltip();
                };
            }

            public void CleanUp() {
                Destroy(barGameObject);
            }


        }

    }


    /*
     * Displays data points as a Line Graph
     * */
    private class LineGraphVisual : IGraphVisual {

        private RectTransform graphContainer;
        private Sprite dotSprite;
        private LineGraphVisualObject lastLineGraphVisualObject;
        private Color dotColor;
        private Color dotConnectionColor;
        private Window_Graph_Object windowGraph;

        public LineGraphVisual(RectTransform graphContainer, Sprite dotSprite, Color dotColor, Color dotConnectionColor, Window_Graph_Object windowGraph) {
            this.graphContainer = graphContainer;
            this.dotSprite = dotSprite;
            this.dotColor = dotColor;
            this.dotConnectionColor = dotConnectionColor;
            this.windowGraph = windowGraph;
            lastLineGraphVisualObject = null;
        }

        public void CleanUp() {
            lastLineGraphVisualObject = null;
        }


        public IGraphVisualObject CreateGraphVisualObject(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
            GameObject dotGameObject = CreateDot(graphPosition);


            GameObject dotConnectionGameObject = null;
            if (lastLineGraphVisualObject != null) {
                dotConnectionGameObject = CreateDotConnection(lastLineGraphVisualObject.GetGraphPosition(), dotGameObject.GetComponent<RectTransform>().anchoredPosition);
            }
            
            LineGraphVisualObject lineGraphVisualObject = new LineGraphVisualObject(dotGameObject, dotConnectionGameObject, lastLineGraphVisualObject, windowGraph);
            lineGraphVisualObject.SetGraphVisualObjectInfo(graphPosition, graphPositionWidth, tooltipText);
            
            lastLineGraphVisualObject = lineGraphVisualObject;

            return lineGraphVisualObject;
        }

        private GameObject CreateDot(Vector2 anchoredPosition) {
            GameObject gameObject = new GameObject("dot", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().sprite = dotSprite;
            gameObject.GetComponent<Image>().color = dotColor;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            rectTransform.anchoredPosition = anchoredPosition;
            rectTransform.sizeDelta = new Vector2(11, 11);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            
            // Add Button_UI Component which captures UI Mouse Events
            gameObject.AddComponent<Button_UI>();
            //Button_UI dotButtonUI = gameObject.AddComponent<Button_UI>();

            return gameObject;
        }

        private GameObject CreateDotConnection(Vector2 dotPositionA, Vector2 dotPositionB) {
            GameObject gameObject = new GameObject("dotConnection", typeof(Image));
            gameObject.transform.SetParent(graphContainer, false);
            gameObject.GetComponent<Image>().color = dotConnectionColor;
            gameObject.GetComponent<Image>().raycastTarget = false;
            RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
            Vector2 dir = (dotPositionB - dotPositionA).normalized;
            float distance = Vector2.Distance(dotPositionA, dotPositionB);
            rectTransform.anchorMin = new Vector2(0, 0);
            rectTransform.anchorMax = new Vector2(0, 0);
            rectTransform.sizeDelta = new Vector2(distance, 3f);
            rectTransform.anchoredPosition = dotPositionA + dir * distance * .5f;
            rectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
            return gameObject;
        }


        public class LineGraphVisualObject : IGraphVisualObject {

            public event EventHandler OnChangedGraphVisualObjectInfo;

            private GameObject dotGameObject;
            private GameObject dotConnectionGameObject;
            private LineGraphVisualObject lastVisualObject;
            private Window_Graph_Object windowGraph;

            public LineGraphVisualObject(GameObject dotGameObject, GameObject dotConnectionGameObject, LineGraphVisualObject lastVisualObject, Window_Graph_Object windowGraph) {
                this.dotGameObject = dotGameObject;
                this.dotConnectionGameObject = dotConnectionGameObject;
                this.lastVisualObject = lastVisualObject;
                this.windowGraph = windowGraph;

                if (lastVisualObject != null) {
                    lastVisualObject.OnChangedGraphVisualObjectInfo += LastVisualObject_OnChangedGraphVisualObjectInfo;
                }
            }

            private void LastVisualObject_OnChangedGraphVisualObjectInfo(object sender, EventArgs e) {
                UpdateDotConnection();
            }

            public void SetGraphVisualObjectInfo(Vector2 graphPosition, float graphPositionWidth, string tooltipText) {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = graphPosition;

                UpdateDotConnection();

                Button_UI dotButtonUI = dotGameObject.GetComponent<Button_UI>();

                // Show Tooltip on Mouse Over
                dotButtonUI.MouseOverOnceFunc = () => {
                    windowGraph.ShowTooltip(tooltipText, graphPosition);
                };
            
                // Hide Tooltip on Mouse Out
                dotButtonUI.MouseOutOnceFunc = () => {
                    windowGraph.HideTooltip();
                };

                if (OnChangedGraphVisualObjectInfo != null) OnChangedGraphVisualObjectInfo(this, EventArgs.Empty);
            }

            public void CleanUp() {
                Destroy(dotGameObject);
                Destroy(dotConnectionGameObject);
            }

            public Vector2 GetGraphPosition() {
                RectTransform rectTransform = dotGameObject.GetComponent<RectTransform>();
                return rectTransform.anchoredPosition;
            }

            private void UpdateDotConnection() {
                if (dotConnectionGameObject != null) {
                    RectTransform dotConnectionRectTransform = dotConnectionGameObject.GetComponent<RectTransform>();
                    Vector2 dir = (lastVisualObject.GetGraphPosition() - GetGraphPosition()).normalized;
                    float distance = Vector2.Distance(GetGraphPosition(), lastVisualObject.GetGraphPosition());
                    dotConnectionRectTransform.sizeDelta = new Vector2(distance, 3f);
                    dotConnectionRectTransform.anchoredPosition = GetGraphPosition() + dir * distance * .5f;
                    dotConnectionRectTransform.localEulerAngles = new Vector3(0, 0, UtilsClass.GetAngleFromVectorFloat(dir));
                }
            }

        }

    }

}
