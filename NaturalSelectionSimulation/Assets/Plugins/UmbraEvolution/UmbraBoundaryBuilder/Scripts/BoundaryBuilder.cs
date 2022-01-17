//Name: Robert MacGillivray
//File: BoundaryBuilder.cs
//Date: Jul.21.2016
//Purpose: To build world boundaries in a simple, intuitive way

//Last Updated: Apr.05.2021 by Robert MacGillivray

using System;
using UnityEngine;

namespace UmbraEvolution
{
    public class BoundaryBuilder : MonoBehaviour
    {
        #region Constants
        public const string PHYSICAL_NODE_GIZMO_NAME = "NodeGizmo";
        public const string PHYSICAL_BOUNDARY_GIZMO_NAME = "BoundaryGizmo";
        #endregion

        [Tooltip("If checked, gizmos for boundaries and nodes will always be shown in the editor. If unchecked, gizmos will be shown when objects are selected in the hierarchy.")]
        public bool gizmosAlwaysOn = false;
        [Tooltip("If checked, will create physical gizmos that are present in the scene. These will persist until this is unchecked, including into test builds.")]
        public bool usePhysicalGizmos = false;
        [Tooltip("The material to use for physical gizmos. Should support setting colour through the Material.color property.")]
        public Material physicalGizmoMaterial;

        // A list of all boundaries that are associated with this BoundaryBuilder
        public Boundary[] Boundaries
        {
            get { return GetComponentsInChildren<Boundary>(); }
        }

        // A boundry that is in the process of placing nodes
        public Boundary BoundaryPlacingNodes
        {
            get { return Array.Find(Boundaries, x => x.placingNodes); }
        }

        public static BoundaryBuilder Reference { get; set; }

        private void OnEnable()
        {
            if (Reference == null)
            {
                Reference = this;
            }
            else if (Reference != this)
            {
                Debug.LogError("There is more than one BoundaryBuilder in the scene. Please delete one.");
            }
        }

        /// <summary>
        /// Creates a new boundary and adds it as a child of the boundary builder
        /// </summary>
        /// <returns>Returns the GameObject that has been created (useful for registering an undo in an editor script)</returns>
        public GameObject AddBoundary()
        {
            GameObject newBoundary = new GameObject("New Boundary", typeof(Boundary));
            newBoundary.transform.parent = transform;
            newBoundary.transform.localPosition = Vector3.zero;
            newBoundary.transform.rotation = transform.rotation;
            newBoundary.GetComponent<Boundary>().physicsLayer = newBoundary.layer;
            return newBoundary;
        }
    }
}
