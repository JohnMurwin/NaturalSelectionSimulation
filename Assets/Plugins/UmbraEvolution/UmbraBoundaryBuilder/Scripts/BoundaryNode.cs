//Name: Robert MacGillivray
//File: BoundaryNode.cs
//Date: Jul.21.2016
//Purpose: To do anything specific to just nodes in boundaries (like calling OnDrawGizmoSelected on the boundary when you're fiddling with the node)

//Last Updated: Apr.05.2021 by Robert MacGillivray

using UnityEngine;

namespace UmbraEvolution
{
    public class BoundaryNode : MonoBehaviour
    {
        private Boundary _parent;
        public Boundary ParentBoundary
        {
            get
            {
                if (!_parent)
                {
                    _parent = transform.parent.GetComponent<Boundary>();
                    if (!_parent)
                    {
                        Debug.LogError("This BoundaryNode does not appear to have a Boundary parent. That shouldn't be the case.");
                    }
                }
                return _parent;
            }
        }

        private GameObject PhysicalNodeGizmo;
        private GameObject PhysicalBoundaryGizmo;

        /// <summary>
        /// To assist in drawing gizmos in a reverse-recursive fashion without infinite overhead
        /// </summary>
        public void DrawNodeGizmosManually()
        {
            Gizmos.matrix = Matrix4x4.identity;
            if (ParentBoundary)
            {
                Gizmos.color = ParentBoundary.nodeGizmoColour;
                Gizmos.DrawSphere(transform.position, ParentBoundary.nodeGizmoRadius);
            }
            else
            {
                Gizmos.color = Color.black;
                Gizmos.DrawSphere(transform.position, 1f);
            }
        }

        /// <summary>
        /// Used to update or create a physical gizmo for this node object
        /// </summary>
        public void DrawPhysicalNodeGizmosManually()
        {
            // First, make sure we don't have a rogue gizmo that we should know about
            if (!PhysicalNodeGizmo)
            {
                Transform gizmoTransform = transform.Find(BoundaryBuilder.PHYSICAL_NODE_GIZMO_NAME);
                PhysicalNodeGizmo = gizmoTransform != null ? gizmoTransform.gameObject : null;
            }

            // If we're still sure we don't have a gizmo, make one
            if (!PhysicalNodeGizmo)
            {
                PhysicalNodeGizmo = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                PhysicalNodeGizmo.name = BoundaryBuilder.PHYSICAL_NODE_GIZMO_NAME;
                DestroyImmediate(PhysicalNodeGizmo.GetComponent<Collider>());
                PhysicalNodeGizmo.transform.SetParent(transform);
                PhysicalNodeGizmo.transform.position = transform.position;
                PhysicalNodeGizmo.transform.rotation = transform.rotation;

                Renderer tempRenderer = PhysicalNodeGizmo.GetComponent<Renderer>();
                tempRenderer.sharedMaterial = new Material(ParentBoundary.ParentBoundaryBuilder.physicalGizmoMaterial);
            }

            // Set Gizmo properties in real-time
            if (ParentBoundary)
            {
                PhysicalNodeGizmo.GetComponent<Renderer>().sharedMaterial.color = ParentBoundary.nodeGizmoColour;
                PhysicalNodeGizmo.transform.localScale = Vector3.one * 2f * ParentBoundary.nodeGizmoRadius;
            }
            else
            {
                PhysicalNodeGizmo.GetComponent<Renderer>().sharedMaterial.color = Color.black;
                PhysicalNodeGizmo.transform.localScale = Vector3.one * 2f;
            }
        }

        /// <summary>
        /// Used to update or create a physical gizmo for the chunk of boundary associated with this node
        /// </summary>
        public void DrawPhysicalBoundaryNodeGizmosManually(float boundaryWidth, float boundaryHeight, float boundaryLength, Vector3 localCenter)
        {
            // First, make sure we don't have a rogue gizmo that we should know about
            if (!PhysicalBoundaryGizmo)
            {
                Transform gizmoTransform = transform.Find(BoundaryBuilder.PHYSICAL_BOUNDARY_GIZMO_NAME);
                PhysicalBoundaryGizmo = gizmoTransform != null ? gizmoTransform.gameObject : null;
            }

            // If we're still sure we don't have a gizmo, make one
            if (!PhysicalBoundaryGizmo)
            {
                PhysicalBoundaryGizmo = GameObject.CreatePrimitive(PrimitiveType.Cube);
                PhysicalBoundaryGizmo.name = BoundaryBuilder.PHYSICAL_BOUNDARY_GIZMO_NAME;
                DestroyImmediate(PhysicalBoundaryGizmo.GetComponent<Collider>());
                PhysicalBoundaryGizmo.transform.SetParent(transform);
                PhysicalBoundaryGizmo.transform.position = transform.position;
                PhysicalBoundaryGizmo.transform.rotation = transform.rotation;

                Renderer tempRenderer = PhysicalBoundaryGizmo.GetComponent<Renderer>();
                tempRenderer.sharedMaterial = new Material(ParentBoundary.ParentBoundaryBuilder.physicalGizmoMaterial);
            }

            // Set Gizmo properties in real-time
            if (ParentBoundary)
            {
                PhysicalBoundaryGizmo.GetComponent<Renderer>().sharedMaterial.color = ParentBoundary.boundaryGizmoColour;
            }
            else
            {
                PhysicalBoundaryGizmo.GetComponent<Renderer>().sharedMaterial.color = Color.black;
            }

            transform.localScale = Vector3.one;
            PhysicalBoundaryGizmo.transform.localPosition = localCenter;
            PhysicalBoundaryGizmo.transform.localScale = new Vector3(boundaryWidth, boundaryHeight, boundaryLength);
        }

        /// <summary>
        /// Cleans up this node's physical gizmos at runtime
        /// </summary>
        public void DestroyPhysicalGizmos()
        {
            if (PhysicalNodeGizmo)
                Destroy(PhysicalNodeGizmo);

            if (PhysicalBoundaryGizmo)
                Destroy(PhysicalBoundaryGizmo);
        }

        /// <summary>
        /// Cleans up this node's physical gizmos in the editor
        /// </summary>
        public void DestroyPhysicalGizmosImmediate()
        {
            if (PhysicalNodeGizmo)
                DestroyImmediate(PhysicalNodeGizmo);

            if (PhysicalBoundaryGizmo)
                DestroyImmediate(PhysicalBoundaryGizmo);
        }

#if UNITY_EDITOR
        void OnDrawGizmosSelected()
        {
            //Useful when moving nodes so that you can still see the full boundary gizmo and your effects on it
            if (UnityEditor.Selection.activeObject == gameObject)
                ParentBoundary.OnDrawGizmosSelected();
        }
#endif

        /// <summary>
        /// If there is no box collider, adds one. Resizes and repositions the box collider based on arguments passed in
        /// </summary>
        /// <param name="size">The value that will be applied to the box collider's size property</param>
        /// <param name="center">The value that will be applied to the box collider's center property</param>
        public void BoxColliderCheck(Vector3 size, Vector3 center, bool useBoxColliders)
        {
            BoxCollider myBoxCollider = GetComponent<BoxCollider>();
            if (!myBoxCollider)
            {
                myBoxCollider = gameObject.AddComponent<BoxCollider>();
            }

            myBoxCollider.size = size;
            myBoxCollider.center = center;

            if (useBoxColliders)
            {
                myBoxCollider.enabled = true;
            }
            else
            {
                myBoxCollider.enabled = false;
            }
        }

        /// <summary>
        /// Forces this node to look at the next node it is connected to
        /// </summary>
        /// <param name="nodeOneAdjusted">The position of this node when adjusted to remove height variance.</param>
        /// <param name="nodeTwoAdjusted">The position of the node this node is connected to when adjusted to remove height variance.</param>
        public void SetRotation(Vector3 nodeOneAdjusted, Vector3 nodeTwoAdjusted)
        {
            transform.rotation = Quaternion.LookRotation(nodeTwoAdjusted - nodeOneAdjusted);
        }

        /// <summary>
        /// A method with a lot of hard-coded shtick to place vertices that this node is responsible for while generating the invisible mesh used to bake the navmesh
        /// </summary>
        /// <param name="heightAdjustment">Vertical offset from this node to place the vertices. Used when the next node is lower than this node.</param>
        /// <returns>Returns an array of 8 vertices used to generate mesh. Will match the vertices of the box collider component on this node.</returns>
        public Vector3[] ReturnVertices(float heightAdjustment)
        {
            Vector3[] vertices = new Vector3[8];

            vertices[0] = transform.position + (-transform.forward * ParentBoundary.boundaryThickness / 2f) + (transform.right * ParentBoundary.boundaryThickness / 2f) + (transform.up * (ParentBoundary.verticalOffset + heightAdjustment));
            vertices[1] = transform.position + (-transform.forward * ParentBoundary.boundaryThickness / 2f) + (-transform.right * ParentBoundary.boundaryThickness / 2f) + (transform.up * (ParentBoundary.verticalOffset + heightAdjustment));
            vertices[2] = vertices[0] + (transform.up * ParentBoundary.boundaryHeight);
            vertices[3] = vertices[1] + (transform.up * ParentBoundary.boundaryHeight);
            vertices[4] = vertices[0] + transform.forward * GetComponent<BoxCollider>().size.z;
            vertices[5] = vertices[1] + transform.forward * GetComponent<BoxCollider>().size.z;
            vertices[6] = vertices[4] + transform.up * ParentBoundary.boundaryHeight;
            vertices[7] = vertices[5] + transform.up * ParentBoundary.boundaryHeight;

            return vertices;
        }

        /// <summary>
        /// Some hack-job, hard-coded int array that results in the vertices returned by ReturnVertices() turning into triangles that 
        /// Unity can use to generate a mesh with the faces pointed the correct way. Not my area of expertise, but it works. 
        /// Feel free to contact me @ umbraevolution@gmail.com if you have a better (procedural?) way to do this
        /// </summary>
        /// <returns>Returns an int array used by Unity's Mesh component to turn vertices into renderable triangles</returns>
        public int[] ReturnTriangles()
        {
            return new int[]
            {
                0, 1, 2,
                1, 3, 2,
                1, 0, 4,
                4, 5, 1,
                0, 2, 6,
                6, 4, 0,
                3, 1, 7,
                7, 1, 5,
                7, 6, 3,
                3, 6, 2,
                5, 4, 6,
                6, 7, 5
            };
        }
    }
}
