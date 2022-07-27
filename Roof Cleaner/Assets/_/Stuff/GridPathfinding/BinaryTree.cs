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

    public class BinaryTree {

        private class BinaryNode {
            //public static int nextId = 0;
            //public int id;
            public PathNode pathNode;
            public BinaryNode left;
            public BinaryNode right;

            public BinaryNode(PathNode _pathNode, BinaryNode _left, BinaryNode _right) {
                //id = nextId++;
                pathNode = _pathNode;
                left = _left;
                right = _right;
            }
            /*public override string ToString() {
                return "n_" + id;
            }*/
        }

        private BinaryNode root;
        public int totalNodes = 0;

        //private string debugString;
        //private string debugCodePath;

        public BinaryTree() {
            //BinaryNode.nextId = 0;
            root = null;
            //debugCodePath = "NEW: ";
        }

        public void AddNode(PathNode pathNode) {
            //debugCodePath += "AD; ";
            if (root == null) {
                root = new BinaryNode(pathNode, null, null);
                totalNodes++;
            } else {
                // Has root
                //debugString = "";
                try {
                    addNode(root, pathNode, pathNode.fValue);
                } catch (System.Exception e) {
                    Debug.Log(e);
                    //Debug.LogError("Error: "+debugString);
                    //Debug.LogError(debugCodePath);
                }
                totalNodes++;
            }
        }

        private void addNode(BinaryNode node, PathNode pathNode, int pathFvalue) {
            //debugString += node.id + " " + ((node.left == null) ? "-1" : ""+node.left.id) + " " + (node.right == null ? "-1" : ""+node.right.id) + " ";
            if (pathFvalue <= node.pathNode.fValue) {
                // Left
                if (node.left == null) {
                    // Become left
                    //debugString += " LEFT" + "; \n";
                    node.left = new BinaryNode(pathNode, null, null);
                    //debugCodePath += "AD" + node.left + "; ";
                } else {
                    // Check left
                    //debugString += "L; \n";
                    //debugString += node.id + " " + (node == (node.left));
                    addNode(node.left, pathNode, pathFvalue);
                }
            } else {
                // Right
                if (node.right == null) {
                    // Become right
                    //debugString += " RIGHT" + "; \n";
                    node.right = new BinaryNode(pathNode, null, null);
                    //debugCodePath += "AD" + node.right + "; ";
                } else {
                    // Check right
                    //debugString += node.id + " ";
                    //debugString += "R; \n";
                    addNode(node.right, pathNode, pathFvalue);
                }
            }
        }

        public void RemoveNode(PathNode pathNode) {
            if (root.pathNode == pathNode) {
                // It's the root
                //debugCodePath += "RR" + root + "; ";
                BinaryNode prevRoot = root;
                if (root.left == null && root.right == null) {
                    // Both are null
                    root = null;
                    // Tree is dead
                } else {
                    // Atleast one has something
                    if (root.left == null && root.right != null) {
                        // Has right but no left
                        root = root.right;
                    } else {
                        if (root.right == null && root.left != null) {
                            // Has left but no right
                            root = root.left;
                        } else {
                            // Has both right and left
                            if (root.left.right == null) {
                                // Root left has no right
                                root.left.right = root.right;
                                root = root.left;
                            } else {
                                // Root left has a right
                                BinaryNode leafRight = getLeafRight(root.left);
                                root = leafRight.right;
                                if (leafRight.right.left != null) {
                                    // This leaf has a left
                                    leafRight.right = leafRight.right.left;
                                }
                                leafRight.right = null;
                                root.left = leafRight;
                                root.right = prevRoot.right;
                            }
                        }
                    }
                }
            } else {
                int pathFvalue = pathNode.fValue;
                removeNode(root, pathNode, pathFvalue);
            }
            totalNodes--;
        }

        private void removeNode(BinaryNode node, PathNode pathNode, int pathFvalue) {
            if (pathFvalue <= node.pathNode.fValue) {
                // Check left
                // Is it this left?
                if (node.left != null) {
                    if (node.left.pathNode == pathNode) {
                        // It's this left one!
                        BinaryNode del = node.left;
                        //debugCodePath += "RNL" + del + "; ";
                        if (del.left == null && del.right == null) {
                            // Both are null
                            node.left = null;
                        } else {
                            // Atleast one has something
                            if (del.left == null && del.right != null) {
                                // Has right but no left
                                node.left = del.right;
                            } else {
                                if (del.right == null && del.left != null) {
                                    // Has left but no right
                                    node.left = del.left;
                                } else {
                                    // Has both right and left
                                    if (del.left.right == null) {
                                        // Root left has no right
                                        del.left.right = del.right;
                                        node.left = del.left;
                                    } else {
                                        // Root left has a right
                                        BinaryNode leafRight = getLeafRight(del.left);
                                        node.left = leafRight.right;
                                        if (leafRight.right.left != null) {
                                            // This leaf has a left
                                            leafRight.right = leafRight.right.left;
                                        }
                                        leafRight.right = null;
                                        node.left.left = leafRight;
                                        node.left.right = del.right;
                                    }
                                }
                            }
                        }
                    } else {
                        // Not this left
                        // Check next one
                        removeNode(node.left, pathNode, pathFvalue);
                    }
                }
            } else {
                // Check right
                // Is it this right?
                if (node.right != null) {
                    if (node.right.pathNode == pathNode) {
                        // It's this right one!
                        BinaryNode del = node.right;
                        //debugCodePath += "RNR" + del + "; ";
                        if (del.left == null && del.right == null) {
                            // Both are null
                            node.right = null;
                        } else {
                            // Atleast one has something
                            if (del.left == null && del.right != null) {
                                // Has right but no left
                                node.right = del.right;
                            } else {
                                if (del.right == null && del.left != null) {
                                    // Has left but no right
                                    node.right = del.left;
                                } else {
                                    // Has both right and left
                                    if (del.left.right == null) {
                                        // Root left has no right
                                        del.left.right = del.right;
                                        node.right = del.left;
                                    } else {
                                        // Root left has a right
                                        BinaryNode leafRight = getLeafRight(del.left);
                                        node.right = leafRight.right;
                                        if (leafRight.right.left != null) {
                                            // This leaf has a left
                                            leafRight.right = leafRight.right.left;
                                        }
                                        leafRight.right = null;
                                        node.right.left = leafRight;
                                        node.right.right = del.right;
                                    }
                                }
                            }
                        }
                    } else {
                        // Not this right
                        // Check next one
                        removeNode(node.right, pathNode, pathFvalue);
                    }
                }
            }
        }

        private BinaryNode getLeafRight(BinaryNode node) {
            if (node.right.right == null)
                return node;
            else
                return getLeafRight(node.right);
        }

        public int getTotalHeight() {
            return getHeight(root);
        }

        private int getHeight(BinaryNode node) {
            if (node == null) return 0;
            else {
                return 1 + (int)Mathf.Max(getHeight(node.left), getHeight(node.right));
            }
        }

        public PathNode GetSmallest() {
            PathNode pathNode = null;
            int count = 0;
            try {
                pathNode = getSmallest(root, ref count);
            } catch {
                Debug.LogError("GetSmallest: " + count);
            }
            return pathNode;
            //return getSmallest(root, 0);
        }

        private PathNode getSmallest(BinaryNode node, ref int count) {
            if (node == null) return null;
            if (node.left != null) {
                count++;
                return getSmallest(node.left, ref count);
            } else {
                // No more left nodes
                return node.pathNode;
            }
        }
    }
}