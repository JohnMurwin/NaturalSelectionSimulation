//Name: Robert MacGillivray
//File: Boundary.cs
//Date: Jul.21.2016
//Purpose: To handle all logic relating to an individual boundary (things like node management, gizmo drawing, etc.)

//Last Updated: May.24.2021 by Robert MacGillivray

using UnityEngine;
using System.Collections.Generic;

namespace UmbraEvolution
{
    public class Boundary : MonoBehaviour
    {
        [Tooltip("The colour of the gizmo that will preview the dimensions of the boundary")]
        public Color boundaryGizmoColour = new Color(0f, 1f, 0.5f, 0.6f);
        [Tooltip("The colour of the gizmo that will show where nodes have been placed.")]
        public Color nodeGizmoColour = new Color(1f, 0f, 0f, 0.6f);
        [Tooltip("The radius of the gizmo that will show where nodes have been placed.")]
        public float nodeGizmoRadius = 1f;
        [Tooltip("If set to true, the first and last nodes in the list will also connect.")]
        public bool closedLoop = true;
        [Tooltip("If set to true, all nodes will have a box collider component that matches the gizmo/mesh for their segment. This is recommended over a single mesh collider.")]
        public bool useBoxColliders = true;
        [Tooltip("The physics layer you want boundaries to be on. Affects collision properties."), SingleLayer]
        public int physicsLayer = 0;
        [Tooltip("How thick the boundary should be. Note that a thicker boundary is generally more effective when baking navmesh")]
        [GreaterThanFloat(0f, false)]
        public float boundaryThickness = 1;
        [Tooltip("The height of the boundary. Note that the higher the boundary, the less likely something is going to escape your level by accident (you could also alternatively add a box collider as an invisible ceiling.")]
        [GreaterThanFloat(0f, false)]
        public float boundaryHeight = 10;
        [Tooltip("The offset in the Y-direction for this boundary. Generally a negative number in order to sink the boundary into terrain.")]
        public float verticalOffset = 0;
        [Tooltip("The material that will be applied to the boundary mesh when it is generated.")]
        public Material boundaryMeshMaterial;
        [Tooltip("The layers that boundaries can be placed on. Prevents raycasts from hitting objects you want to ignore.")]
        public LayerMask placeableLayers = ~0;
        [Tooltip("The setting to use when raycasting against the terrain. Set to match project settings, hit triggers, or ignore triggers.")]
        public QueryTriggerInteraction queryTriggerInteraction;

        private bool rebuildNodeList = true;
        private List<BoundaryNode> _boundaryNodes;
        public List<BoundaryNode> BoundaryNodes
        {
            get
            {
                if (rebuildNodeList || _boundaryNodes == null || _boundaryNodes.Count != transform.childCount)
                {
                    _boundaryNodes = new List<BoundaryNode>();
                    // Nodes have been changed. No guarentee that layers are correct, so reset them.
                    MatchPhysicsLayerRecursive();
                    for (int index = 0; index < transform.childCount; index++)
                    {
                        BoundaryNode nextNode = transform.GetChild(index).GetComponent<BoundaryNode>();
                        if (nextNode)
                        {
                            _boundaryNodes.Add(nextNode);
                        }
                        else
                        {
                            Debug.LogWarning("There is a child of a boundary that isn't a node. This causes efficiency issues.");
                        }
                    }
                    rebuildNodeList = false;
                }
                return _boundaryNodes;
            }
        }

        [ReadOnlyInInspector]
        public bool placingNodes;

        private BoundaryBuilder _parent;
        public BoundaryBuilder ParentBoundaryBuilder
        {
            get
            {
                if (!_parent)
                {
                    _parent = transform.parent.GetComponent<BoundaryBuilder>();
                    if (!_parent)
                    {
                        Debug.LogError("This Boundary does not appear to have a BoundaryBuilder parent. That shouldn't be the case.");
                    }
                }
                return _parent;
            }
        }

        public MeshRenderer BoundaryMeshRenderer { get; private set; }
        public MeshFilter BoundaryMeshFilter { get; private set; }

        private void Awake()
        {
            BoundaryMeshRenderer = GetComponent<MeshRenderer>();
            BoundaryMeshFilter = GetComponent<MeshFilter>();
        }

        public void OnTransformChildrenChanged()
        {
            rebuildNodeList = true;
        }

        public void OnDrawGizmos()
        {
            if (ParentBoundaryBuilder.usePhysicalGizmos)
            {
                DrawPhysicalGizmos();
            }
            else if (ParentBoundaryBuilder.gizmosAlwaysOn)
            {
                DrawGizmos();
            }

            if (!ParentBoundaryBuilder.usePhysicalGizmos)
            {
                if (Application.isEditor && !Application.isPlaying)
                {
                    DestroyPhysicalGizmosImmediate();
                }
                else
                {
                    DestroyPhysicalGizmos();
                }
            }
        }

        public void OnDrawGizmosSelected()
        {
            if (!ParentBoundaryBuilder.gizmosAlwaysOn && !ParentBoundaryBuilder.usePhysicalGizmos)
            {
                DrawGizmos();
            }
        }

        /// <summary>
        /// Draws gizmos for boundaries and their nodes
        /// </summary>
        /// <param name="triggeringNode">The node that called this method. Default is null, but when a node triggers gizmos to be drawn, we need to know which node did so.</param>
        public void DrawGizmos(BoundaryNode triggeringNode = null)
        {
            // We don't want to follow through if gizmos have already been drawn
            if (triggeringNode != null && ParentBoundaryBuilder.gizmosAlwaysOn)
                return;

            //If we have at least two nodes and there aren't any null nodes, we can do a bunch of math to figure out where to draw the gizmos as per the values in the inspector
            if (BoundaryNodes.Count > 1)
            {
                Gizmos.color = boundaryGizmoColour;

                int indexLimit = closedLoop ? BoundaryNodes.Count : BoundaryNodes.Count - 1;

                for (int index = 0; index < indexLimit; index++)
                {
                    int nextIndex = (index + 1) % BoundaryNodes.Count;

                    //The center of the gizmo will be between the current node and the next one in the list
                    //The exception to the above is the y-dimension, within which the bottom of the gizmo will line up with the lowest node (plus or minus Vertical Offset)
                    float gizmoCenterX = (BoundaryNodes[index].transform.position.x + BoundaryNodes[nextIndex].transform.position.x) / 2f;
                    float gizmoCenterY = Mathf.Min(BoundaryNodes[index].transform.position.y, BoundaryNodes[nextIndex].transform.position.y) + (boundaryHeight / 2f) + verticalOffset;
                    float gizmoCenterZ = (BoundaryNodes[index].transform.position.z + BoundaryNodes[nextIndex].transform.position.z) / 2f;
                    Vector3 gizmoCenter = new Vector3(gizmoCenterX, gizmoCenterY, gizmoCenterZ);

                    //A fictitious point for the current node that removes disparity in y-position between it and the next node
                    Vector3 nodeOneAdjusted = new Vector3(BoundaryNodes[index].transform.position.x, gizmoCenter.y, BoundaryNodes[index].transform.position.z);
                    //A fictitious point for the next node that removes disparity in y-position between it and the current node
                    Vector3 nodeTwoAdjusted = new Vector3(BoundaryNodes[nextIndex].transform.position.x, gizmoCenter.y, BoundaryNodes[nextIndex].transform.position.z);
                    //The distance between the two nodes plus an offset based on the thickness of the gizmo (this will be the width of the gizmo)
                    float gizmoWidth = Vector3.Distance(nodeOneAdjusted, nodeTwoAdjusted) + boundaryThickness;
                    //Adjusts the gizmo matrix so that we have correct orientation
                    Gizmos.matrix = Matrix4x4.TRS(gizmoCenter, Quaternion.LookRotation(nodeTwoAdjusted - nodeOneAdjusted), new Vector3(1f, 1f, 1f));
                    //Draws a cube using everything calculated above. The position is at 0,0,0 because we've already adjusted the matrix to account for that
                    Gizmos.DrawCube(Vector3.zero, new Vector3(boundaryThickness, boundaryHeight, gizmoWidth));

                    //Normally you wouldn't set colliders or rotation in OnDrawGizmosSelected() code, but we've already done all the math and it's more efficient that sticking it in an update
                    BoundaryNodes[index].SetRotation(nodeOneAdjusted, nodeTwoAdjusted);
                    BoundaryNodes[index].BoxColliderCheck(new Vector3(boundaryThickness, boundaryHeight, gizmoWidth), new Vector3(0f, gizmoCenter.y - BoundaryNodes[index].transform.position.y, Vector3.Distance(nodeOneAdjusted, nodeTwoAdjusted) / 2f), useBoxColliders);
                }

                if (!closedLoop)
                {
                    //If we can't/aren't generating anything between the last and first nodes, we need to make sure that a box collider that may have been there in the past is no longer there
                    BoundaryNodes[BoundaryNodes.Count - 1].GetComponent<BoundaryNode>().BoxColliderCheck(Vector3.zero, Vector3.zero, false);
                }
            }

            // manually trigger the gizmo drawing here so that they don't each control it and cause tremendous overhead
            foreach (BoundaryNode node in BoundaryNodes)
            {
                // Don't want to draw the same node twice
                if (node != triggeringNode)
                    node.DrawNodeGizmosManually();
            }
        }

        /// <summary>
        /// Draws physically-based gizmos for boundaries and their nodes
        /// </summary>
        /// <param name="triggeringNode">The node that called this method. Default is null, but when a node triggers gizmos to be drawn, we need to know which node did so.</param>
        public void DrawPhysicalGizmos(BoundaryNode triggeringNode = null)
        {
            // We don't want to follow through if gizmos have already been drawn
            if (triggeringNode != null && ParentBoundaryBuilder.gizmosAlwaysOn)
                return;

            //If we have at least two nodes and there aren't any null nodes, we can do a bunch of math to figure out where to draw the gizmos as per the values in the inspector
            if (BoundaryNodes.Count > 1)
            {
                Gizmos.color = boundaryGizmoColour;

                int indexLimit = closedLoop ? BoundaryNodes.Count : BoundaryNodes.Count - 1;

                for (int index = 0; index < indexLimit; index++)
                {
                    int nextIndex = (index + 1) % BoundaryNodes.Count;

                    //The center of the gizmo will be between the current node and the next one in the list
                    //The exception to the above is the y-dimension, within which the bottom of the gizmo will line up with the lowest node (plus or minus Vertical Offset)
                    float gizmoCenterX = (BoundaryNodes[index].transform.position.x + BoundaryNodes[nextIndex].transform.position.x) / 2f;
                    float gizmoCenterY = Mathf.Min(BoundaryNodes[index].transform.position.y, BoundaryNodes[nextIndex].transform.position.y) + (boundaryHeight / 2f) + verticalOffset;
                    float gizmoCenterZ = (BoundaryNodes[index].transform.position.z + BoundaryNodes[nextIndex].transform.position.z) / 2f;
                    Vector3 gizmoCenter = new Vector3(gizmoCenterX, gizmoCenterY, gizmoCenterZ);

                    //A fictitious point for the current node that removes disparity in y-position between it and the next node
                    Vector3 nodeOneAdjusted = new Vector3(BoundaryNodes[index].transform.position.x, gizmoCenter.y, BoundaryNodes[index].transform.position.z);
                    //A fictitious point for the next node that removes disparity in y-position between it and the current node
                    Vector3 nodeTwoAdjusted = new Vector3(BoundaryNodes[nextIndex].transform.position.x, gizmoCenter.y, BoundaryNodes[nextIndex].transform.position.z);
                    //The distance between the two nodes plus an offset based on the thickness of the gizmo (this will be the width of the gizmo)
                    float gizmoWidth = Vector3.Distance(nodeOneAdjusted, nodeTwoAdjusted) + boundaryThickness;

                    //Normally you wouldn't set colliders or rotation in OnDrawGizmosSelected() code, but we've already done all the math and it's more efficient than sticking it in an update
                    BoundaryNodes[index].SetRotation(nodeOneAdjusted, nodeTwoAdjusted);
                    Vector3 gizmoSize = new Vector3(boundaryThickness, boundaryHeight, gizmoWidth);
                    Vector3 gizmoLocalCenter = new Vector3(0f, gizmoCenter.y - BoundaryNodes[index].transform.position.y, Vector3.Distance(nodeOneAdjusted, nodeTwoAdjusted) / 2f);
                    BoundaryNodes[index].BoxColliderCheck(gizmoSize, gizmoLocalCenter, useBoxColliders);

                    //Creates a gizmo cube using everything calculated above
                    BoundaryNodes[index].DrawPhysicalBoundaryNodeGizmosManually(boundaryThickness, boundaryHeight, gizmoWidth, gizmoLocalCenter);
                }

                if (!closedLoop)
                {
                    //If we can't/aren't generating anything between the last and first nodes, we need to make sure that a box collider that may have been there in the past is no longer there
                    BoundaryNodes[BoundaryNodes.Count - 1].GetComponent<BoundaryNode>().BoxColliderCheck(Vector3.zero, Vector3.zero, false);
                }
            }

            // manually trigger the gizmo drawing here so that they don't each control it and cause tremendous overhead
            foreach (BoundaryNode node in BoundaryNodes)
            {
                // Don't want to draw the same node twice
                if (node != triggeringNode)
                    node.DrawPhysicalNodeGizmosManually();
            }
        }

        public void DestroyPhysicalGizmos()
        {
            foreach (BoundaryNode node in BoundaryNodes)
            {
                node.DestroyPhysicalGizmos();
            }
        }

        public void DestroyPhysicalGizmosImmediate()
        {
            foreach (BoundaryNode node in BoundaryNodes)
            {
                node.DestroyPhysicalGizmosImmediate();
            }
        }

        /// <summary>
        /// Adds a new node as a child of this boundary
        /// </summary>
        /// <param name="position">The position in which to instantiate the node</param>
        /// <returns>Returns the GameObject with the BoundaryNode component (useful for registering undos in an editor script)</returns>
        public GameObject AddNode(Vector3 position)
        {
            GameObject newNode = new GameObject("Node(" + (transform.childCount + 1) + ")", typeof(BoundaryNode));
            newNode.transform.position = position;
            newNode.transform.parent = transform;
            newNode.layer = physicsLayer;
            rebuildNodeList = true;
            return newNode;
        }

        /// <summary>
        /// Generates a 3D mesh from scratch using the nodes that have been placed that will match the gizmos/colliders. Used for generating navmesh.
        /// </summary>
        public void GenerateMesh()
        {
            //Clear any renderer and filter to start fresh (supports upgrade from legacy systems)
            DestroyBoundaryMeshImmediate();
            BoundaryMeshRenderer = gameObject.AddComponent<MeshRenderer>();
            BoundaryMeshFilter = gameObject.AddComponent<MeshFilter>();

            //No shadow interaction
            BoundaryMeshRenderer.receiveShadows = false;
            BoundaryMeshRenderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

            //No probe interaction
            BoundaryMeshRenderer.lightProbeUsage = UnityEngine.Rendering.LightProbeUsage.Off;
            BoundaryMeshRenderer.reflectionProbeUsage = UnityEngine.Rendering.ReflectionProbeUsage.Off;

            //Don't waste time with dynamic occlusion
            BoundaryMeshRenderer.allowOcclusionWhenDynamic = false;

            //Assigns the proper material (an invisible one)
            BoundaryMeshRenderer.sharedMaterial = boundaryMeshMaterial;

            BoundaryMeshFilter.sharedMesh = new Mesh();

            //Creates a new list of Vector3s for the vertices of the mesh and grabs them from all of the nodes in this boundary
            List<Vector3> meshVertices = new List<Vector3>();
            for (int index = 0; index < BoundaryNodes.Count - 1; index++)
            {
                float offset = Mathf.Min(0f, BoundaryNodes[index + 1].transform.position.y - BoundaryNodes[index].transform.position.y);
                foreach (Vector3 vertex in BoundaryNodes[index].ReturnVertices(offset))
                {
                    meshVertices.Add(vertex);
                }
            }

            //If the boundary is a closed loop, we must get the vertices to extend the mesh from the last node to the first node
            if (closedLoop)
            {
                float offset = Mathf.Min(0f, BoundaryNodes[0].transform.position.y - BoundaryNodes[BoundaryNodes.Count - 1].transform.position.y);
                foreach (Vector3 vertex in BoundaryNodes[BoundaryNodes.Count - 1].ReturnVertices(offset))
                {
                    meshVertices.Add(vertex);
                }
            }

            //Assigns the vertices we have calculated to the mesh
            BoundaryMeshFilter.sharedMesh.SetVertices(meshVertices);

            //Creates a new list of ints to be used when using the vertices to create triangles and grabs the necessary ints from the nodes
            List<int> meshTriangles = new List<int>();
            int counter = 0;
            for (int index = 0; index < BoundaryNodes.Count - 1; index++)
            {
                foreach (int triangleVertex in BoundaryNodes[index].ReturnTriangles())
                {
                    meshTriangles.Add(triangleVertex + counter * 8);
                }
                counter++;
            }
            //if the boundary is a closed loop, we must grab the triangles for the mesh generated between the last and first nodes
            if (closedLoop)
            {
                foreach (int triangleVertex in BoundaryNodes[BoundaryNodes.Count - 1].ReturnTriangles())
                {
                    meshTriangles.Add(triangleVertex + counter * 8);
                }
            }
            //Sets the triangles in the mesh based on the list of ints we've collected
            BoundaryMeshFilter.sharedMesh.SetTriangles(meshTriangles, 0);
            //Names the mesh so we can see it in the inspector properly
            BoundaryMeshFilter.sharedMesh.name = gameObject.name + " Mesh";
            //Optimizes the mesh (not necessary, but might as well)
            var o_198_12_636270693662772814 = BoundaryMeshFilter.sharedMesh;
            //Calculates the bounds of the mesh (probably not necessary, but doing it to be safe)
            BoundaryMeshFilter.sharedMesh.RecalculateBounds();
            //NOTE: We do not need to calculate UVs or normals as this mesh will be completely invisible and only used for baking navmesh
        }

        /// <summary>
        /// Removes the mesh on the boundary used to bake the navmesh (does not work in-editor)
        /// </summary>
        public void DestroyBoundaryMesh()
        {
            if (Application.isEditor && !Application.isPlaying)
            {
                Debug.LogError("DestroyBoundaryMesh() will not work in the editor, outside of play mode.");
                return;
            }

            MeshRenderer tempRenderer = GetComponent<MeshRenderer>();
            if (tempRenderer)
                Destroy(tempRenderer);

            MeshFilter tempFilter = GetComponent<MeshFilter>();
            if (tempFilter)
                Destroy(tempFilter);

            BoundaryMeshFilter = null;
            BoundaryMeshRenderer = null;
        }

        /// <summary>
        /// Removes the mesh on the boundary used to bake the navemesh immediately (used in-editor)
        /// </summary>
        public void DestroyBoundaryMeshImmediate()
        {
            MeshRenderer tempRenderer = GetComponent<MeshRenderer>();
            if (tempRenderer)
                DestroyImmediate(tempRenderer);

            MeshFilter tempFilter = GetComponent<MeshFilter>();
            if (tempFilter)
                DestroyImmediate(tempFilter);

            BoundaryMeshFilter = null;
            BoundaryMeshRenderer = null;
        }

        /// <summary>
        /// Recursively sets the physics layer for this boundary and its children recursively,
        /// similar to how the editor would do it. Used to synchronize boundaries and their children
        /// with the physicsLayer property.
        /// </summary>
        public void MatchPhysicsLayerRecursive()
        {
            MatchPhysicsLayerRecursive(gameObject);
        }

        private void MatchPhysicsLayerRecursive(GameObject go)
        {
            go.layer = physicsLayer;
            foreach (Transform child in go.transform)
            {
                MatchPhysicsLayerRecursive(child.gameObject);
            }
        }
    }
}
