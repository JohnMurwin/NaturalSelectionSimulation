//Name: Robert MacGillivray
//File: BoundaryBuilderEditor.cs
//Date: Jul.21.2016
//Purpose: To handle editor logic/formatting for the BoundaryBuilder script

//Last Updated: Apr.05.2021 by Robert MacGillivray

using UnityEngine;
using UnityEditor;

namespace UmbraEvolution
{
    [CustomEditor(typeof(BoundaryBuilder))]
    [DisallowMultipleComponent]
    public class BoundaryBuilderEditor : Editor
    {
        BoundaryBuilder boundaryBuilder;

        #region Serialized Properties
        SerializedProperty gizmosAlwaysOnProperty;
        SerializedProperty usePhysicalGizmosProperty;
        SerializedProperty physicalGizmoMaterialProperty;
        #endregion

        private void OnEnable()
        {
            boundaryBuilder = (BoundaryBuilder)target;
            gizmosAlwaysOnProperty = serializedObject.FindProperty("gizmosAlwaysOn");
            usePhysicalGizmosProperty = serializedObject.FindProperty("usePhysicalGizmos");
            physicalGizmoMaterialProperty = serializedObject.FindProperty("physicalGizmoMaterial");

            if (BoundaryBuilder.Reference == null)
            {
                BoundaryBuilder.Reference = boundaryBuilder;
            }
            else if (BoundaryBuilder.Reference != boundaryBuilder)
            {
                Debug.LogError("There is more than one BoundaryBuilder in the scene. Please delete one.");
            }
        }

        /// <summary>
        /// Adds a menu item in the toolbar that adds a BoundaryBuilder to the open scene
        /// </summary>
        /// <param name="menuCommand"></param>
        [MenuItem("Tools/UmbraEvolution/UmbraBoundaryBuilder/Add Boundary Builder")]
        static void CreateBoundaryBuilder()
        {
            if (!BoundaryBuilder.Reference)
            {
                GameObject boundaryBuilder = new GameObject("UmbraBoundaryBuilder", typeof(BoundaryBuilder));
                Undo.RegisterCreatedObjectUndo(boundaryBuilder, "Created Boundary Builder");
                Selection.activeObject = boundaryBuilder;
            }
            else
            {
                Debug.LogError("There is already one BoundaryBuilder in the scene. A new one cannot be created.");
            }
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            if (boundaryBuilder.transform.position != Vector3.zero)
            {
                boundaryBuilder.transform.position = Vector3.zero;
            }

            EditorGUILayout.PropertyField(usePhysicalGizmosProperty);
            if (!usePhysicalGizmosProperty.boolValue)
            {
                EditorGUILayout.HelpBox("Physical gizmos are a computationally expensive, experimental feature compared to Unity's built-in gizmos. Only use them if the default gizmos aren't working, or you need to see the gizmos in play-mode and/or a dev build.", MessageType.Warning);
            }
            else
            {
                EditorGUILayout.PropertyField(physicalGizmoMaterialProperty);
                EditorGUILayout.HelpBox("Physical gizmos are enabled. Remember to disable them before distributing your project, or else they will remain visible in the build.", MessageType.Warning);
            }

            if (!usePhysicalGizmosProperty.boolValue)
            {
                EditorGUILayout.PropertyField(gizmosAlwaysOnProperty);
            }

            //The following is all formatting to make the inspector for this object look pretty as well as provide a way to
            //manage all boundaries easily (i.e. buttons to delete/create or go to a specific boundary. A colour picker to change their gizmo colours for identification, etc.)

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Boundaries", EditorStyles.whiteLargeLabel);
            if (boundaryBuilder.Boundaries != null && boundaryBuilder.Boundaries.Length > 0)
            {
                EditorGUILayout.BeginVertical("Box");
                foreach (Boundary boundary in boundaryBuilder.Boundaries)
                {
                    if (boundary)
                    {
                        EditorGUILayout.Space();
                        EditorGUILayout.BeginHorizontal("Box");
                        GUIContent goToButtonContent = new GUIContent("Go To", "Pressing this button will take you to the boundary in question.");
                        if (GUILayout.Button(goToButtonContent))
                        {
                            Selection.activeObject = boundary;
                        }

                        EditorGUILayout.Space();
                        string nameFieldString = EditorGUILayout.TextField(boundary.gameObject.name);
                        if (!nameFieldString.Equals(boundary.gameObject.name))
                        {
                            Undo.RecordObject(boundary.gameObject, "Change Boundary Name");
                            boundary.gameObject.name = nameFieldString;
                        }

                        EditorGUILayout.Space();
                        Color colourFieldColour = EditorGUILayout.ColorField(boundary.boundaryGizmoColour);
                        if (!colourFieldColour.Equals(boundary.boundaryGizmoColour))
                        {
                            Undo.RecordObject(boundary, "Change Boundary Colour");
                            boundary.boundaryGizmoColour = colourFieldColour;
                        }

                        EditorGUILayout.Space();
                        GUIContent deleteButtonContent = new GUIContent("Delete", "Pressing this button will delete this boundary.");
                        if (GUILayout.Button(deleteButtonContent))
                        {
                            Undo.DestroyObjectImmediate(boundary.gameObject);
                        }
                        EditorGUILayout.EndHorizontal();
                        EditorGUILayout.Space();
                    }
                }
                EditorGUILayout.EndVertical();
            }
            else
            {
                EditorGUILayout.HelpBox("This boundary builder has no boundaries associated with it. When new boundaries are created, they will be visible here.", MessageType.Info);
            }

            GUIContent addBoundaryButtonContent = new GUIContent("Add New Boundary", "Pressing this button will set-up and create a new boundary.");
            if (GUILayout.Button(addBoundaryButtonContent))
            {
                Undo.RegisterCreatedObjectUndo(boundaryBuilder.AddBoundary(), "Create New Boundary");
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
}
