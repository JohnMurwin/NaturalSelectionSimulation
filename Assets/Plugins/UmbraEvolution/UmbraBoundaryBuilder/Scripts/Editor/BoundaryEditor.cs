//Name: Robert MacGillivray
//File: BoundaryEditor.cs
//Date: Jul.22.2016
//Purpose: To handle editor logic and formatting for boundaries

//Last Updated: May.24.2021 by Robert MacGillivray

using UnityEngine;
using UnityEditor;

namespace UmbraEvolution
{
    [CustomEditor(typeof(Boundary))]
    [DisallowMultipleComponent]
    [CanEditMultipleObjects]
    public class BoundaryEditor : Editor
    {
        private Boundary boundary;

        private GUIContent startPlacingButtonContent;
        private GUIContent stopPlacingButtonContent;
        private GUIContent generateMeshButtonContent;
        private GUIContent destroyMeshButtonContent;
        private GUIContent goToBoundaryButtonContent;

        StaticEditorFlags boundaryFlags;

        private void OnEnable()
        {
            boundary = (Boundary)target;

            startPlacingButtonContent = new GUIContent("Start Placing Nodes", "Pressing this button will allow placing nodes in the scene.");
            stopPlacingButtonContent = new GUIContent("Stop Placing Nodes", "Pressing this button will stop placing nodes in the scene.");
            generateMeshButtonContent = new GUIContent("Generate Mesh", "Pressing this button will generate and save a mesh that matches the editor preview. This mesh will be used to generate nav-meshes, but should be invisible.");
            destroyMeshButtonContent = new GUIContent("Destroy Mesh", "Pressing this button will destroy any mesh created by the Generate Mesh button.");
            goToBoundaryButtonContent = new GUIContent("Go To Boundary", "Pressing this button will take you to the boundary that is trying to place nodes.");

            //Automatically sets the static flags that should be set for an invisible boundary mesh used to calculate navmesh
            boundaryFlags = StaticEditorFlags.BatchingStatic | StaticEditorFlags.NavigationStatic | StaticEditorFlags.OccludeeStatic;
            GameObjectUtility.SetStaticEditorFlags(boundary.gameObject, boundaryFlags);
            boundary.MatchPhysicsLayerRecursive();
            foreach (Transform child in boundary.transform)
            {
                GameObjectUtility.SetStaticEditorFlags(child.gameObject, boundaryFlags);
            }

#if UNITY_2019_1_OR_NEWER
            // This fixes the new behaviour of OnSceneGUI not being called when gizmos disabled
            SceneView.duringSceneGui += OnSceneOnGUI;
#endif
        }

#if UNITY_2019_1_OR_NEWER
        private void OnDisable()
        {
            SceneView.duringSceneGui -= OnSceneOnGUI;
        }
#endif

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

#if UNITY_2019_1_OR_NEWER
            EditorGUILayout.HelpBox("Make sure gizmos are not disabled globally, or you may not see the expected result when placing nodes!", MessageType.Info);
#endif

            // Boundary layer hasn't been updated to reflect changed physicsLayer property. Update it.
            if (boundary.physicsLayer != boundary.gameObject.layer)
            {
                boundary.MatchPhysicsLayerRecursive();
            }

            if (!boundary.placingNodes)
            {
                Boundary nodePlacer = boundary.ParentBoundaryBuilder.BoundaryPlacingNodes;
                if (nodePlacer)
                {
                    EditorGUILayout.HelpBox("Another boundary is placing nodes. Turn that off before doing anything else.", MessageType.Error);
                    if (GUILayout.Button(goToBoundaryButtonContent))
                    {
                        Selection.activeObject = nodePlacer;
                    }
                    return;
                }
            }

            DrawDefaultInspector();

            EditorGUILayout.Space();
            //If placing nodes, disable the ability to generate a mesh, display a button that stops placement as well as a message reminding the user that they are in placement mode (which overrides many basic editor functions)
            if (boundary.placingNodes)
            {
                if (GUILayout.Button(stopPlacingButtonContent))
                {
                    boundary.placingNodes = false;
                }
                EditorGUILayout.HelpBox("REMINDER: You are currently placing nodes. In order to resume normal control of your editor, you must press the button above. This includes adjusting the position of any node points by hand.", MessageType.Warning, true);
            }
            else
            {
                //If we're not placing nodes, show a button to start placing nodes as long as a scene window is open to focus on
                if (SceneView.sceneViews.Count > 0)
                {
                    if (GUILayout.Button(startPlacingButtonContent))
                    {
                        //Sets the bool to indicate we're placing nodes as well as focuses the first available scene window for placement
                        boundary.placingNodes = true;
                        SceneView sceneView = (SceneView)SceneView.sceneViews[0];
                        sceneView.Focus();
                    }
                }
                else
                {
                    EditorGUILayout.HelpBox("You need to have a scene window open to place nodes.", MessageType.Error, true);
                }

                EditorGUILayout.Space();

                //As long as we're not placing nodes, we can enable a button to generate an invisible mesh to be used for baking navmesh
                if (GUILayout.Button(generateMeshButtonContent))
                {
                    boundary.GenerateMesh();
                }

                if (boundary.BoundaryMeshRenderer != null || boundary.BoundaryMeshFilter != null)
                {
                    if (GUILayout.Button(destroyMeshButtonContent))
                    {
                        boundary.DestroyBoundaryMeshImmediate();
                    }
                }

                EditorGUILayout.Space();
            }

            serializedObject.ApplyModifiedProperties();
        }

#if UNITY_2019_1_OR_NEWER
        /// <summary>
        /// Handles clicking to place nodes by preventing the selection of clicked objects and creating nodes at clicked point
        /// </summary>
        /// <param name="sceneView"></param>
        public void OnSceneOnGUI(SceneView sceneView)
        {
            //Use OnSceneGUI because it is called while we're focused on the scene window (which we should be while we're placing nodes)
            if (boundary.placingNodes)
            {
                //If we click the left mouse button in the scene, raycast from the mouse into the scene to find where to create a node
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    if (Camera.current)
                    {
                        Ray detectionRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                        RaycastHit hitInfo;
                        if (Physics.Raycast(detectionRay, out hitInfo, float.MaxValue, boundary.placeableLayers, boundary.queryTriggerInteraction))
                        {
                            GameObject newNode = boundary.AddNode(hitInfo.point);
                            GameObjectUtility.SetStaticEditorFlags(newNode, boundaryFlags);
                            Undo.RegisterCreatedObjectUndo(newNode, "Created New Node");
                        }
                    }
                }
                //Make sure that we keep the boundary selected. Without this line, clicking on the terrain to place a node there would also select the terrain, halting the process
                Selection.activeObject = boundary;
            }
        }
#else
        // Uses OnSceneGUI to handle clicking to place nodes by preventing the selection of clicked objects and creating nodes at clicked point
        public void OnSceneGUI()
        {
            //Use OnSceneGUI because it is called while we're focused on the scene window (which we should be while we're placing nodes)
            if (boundary.placingNodes)
            {
                //If we click the left mouse button in the scene, raycast from the mouse into the scene to find where to create a node
                if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
                {
                    if (Camera.current)
                    {
                        Ray detectionRay = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                        RaycastHit hitInfo;
                        if (Physics.Raycast(detectionRay, out hitInfo, float.MaxValue, boundary.placeableLayers, boundary.queryTriggerInteraction))
                        {
                            GameObject newNode = boundary.AddNode(hitInfo.point);
                            GameObjectUtility.SetStaticEditorFlags(newNode, boundaryFlags);
                            Undo.RegisterCreatedObjectUndo(newNode, "Created New Node");
                        }
                    }
                }
                //Make sure that we keep the boundary selected. Without this line, clicking on the terrain to place a node there would also select the terrain, halting the process
                Selection.activeObject = boundary;
            }
        }
#endif
    }
}
