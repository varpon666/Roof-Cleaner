/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */
 
using UnityEngine;
using System.Collections;

namespace GridPathfindingSystem {

    public class PathQueue {

        public int startX;
        public int startY;
        public int endX;
        public int endY;
        public GridPathfinding.OnPathCallback callback;

        public PathQueue(int _startX, int _startY, int _endX, int _endY, GridPathfinding.OnPathCallback _callback) {
            startX = _startX;
            startY = _startY;
            endX = _endX;
            endY = _endY;
            callback = _callback;
        }
    }

}