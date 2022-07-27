/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using System.Collections.Generic;

namespace GridPathfindingSystem {

    public class MapPos {

        public int x;
        public int y;
        public float offsetX;
        public float offsetY;
        public bool straightToOffset = true;

        public MapPos(int _x, int _y, float _offsetX = 0f, float _offsetY = 0f, bool _straightToOffset = true) {
            x = _x;
            y = _y;
            offsetX = _offsetX;
            offsetY = _offsetY;
            straightToOffset = _straightToOffset;
        }
        public bool Equals(MapPos p2) {
            // Check if this one equals that one
            return x == p2.x && y == p2.y;
        }
        public bool EqualsDeep(MapPos p2) {
            // Check if this one equals that one
            return x == p2.x && y == p2.y && offsetX == p2.offsetX && offsetY == p2.offsetY && straightToOffset == p2.straightToOffset;
        }
        public override string ToString() {
            return "x:" + x + ", y:" + y;
        }
        public string ToStringThorough() {
            return "x:" + x + ", y:" + y + "; ox:" + offsetX + ", oy:" + offsetY + ", s: " + straightToOffset;
        }
        public void ResetOffset() {
            offsetX = 0f;
            offsetY = 0f;
        }
        public MapPos ClearOffset() {
            return new MapPos(x, y);
        }
        public MapPos Clone() {
            return new MapPos(x, y, offsetX, offsetY, straightToOffset);
        }
        public MapPos AddPosCopy(int x, int y) {
            return new MapPos(this.x + x, this.y + y);
        }

        public static int Distance(MapPos p1, MapPos p2) {
            return System.Math.Abs(p1.x - p2.x) + System.Math.Abs(p1.y - p2.y);
        }
        public static bool ListContains(List<MapPos> list, MapPos mapPos) {
            // Check if map pos is in list
            foreach (MapPos pos in list)
                if (pos.Equals(mapPos))
                    return true;
            return false;
        }

        public string Save() {
            // Returns a string to be used in savefiles
            string[] content = new string[]{
            ""+x,
            ""+y,
            ""+offsetX,
            ""+offsetY,
            ""+straightToOffset,
        };
            return string.Join("#MAPPOS#", content);
        }
        public static MapPos Load(string save) {
            return new MapPos(save);
        }
        public MapPos(string save) {
            // Loads a MapPos object form a given savefile string
            string[] content = save.Split(new string[] { "#MAPPOS#" }, System.StringSplitOptions.None);
            x = int.Parse(content[0]);
            y = int.Parse(content[1]);
            offsetX = float.Parse(content[2]);
            offsetY = float.Parse(content[3]);
            straightToOffset = bool.Parse(content[4]);
        }
    }

}