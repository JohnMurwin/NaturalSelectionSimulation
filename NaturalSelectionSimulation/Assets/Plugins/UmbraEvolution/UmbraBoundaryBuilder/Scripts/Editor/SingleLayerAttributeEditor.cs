// Name: Robert MacGillivray
// File: SingleLayerAttributeEditor.cs
// Date: Dec.12.2019
// Purpose: To decorate an int as a single layer for things that can't have a layer mask

// Last Updated: Dec.12.2019 by Robert MacGillivray

using UnityEditor;
using UnityEngine;

namespace UmbraEvolution
{
    [CustomPropertyDrawer(typeof(SingleLayerAttribute))]
    class SingleLayerAttributeEditor : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            int layerSelection = EditorGUI.LayerField(position, label, property.intValue);
            if (layerSelection < 0)
            {
                property.intValue = 0;
                Debug.LogError("Can't set layers to a value less than 0. Setting to 0.");
            }
            else if (layerSelection > 31)
            {
                property.intValue = 31;
                Debug.LogError("Can't set layers to a value greater than 31. Setting to 31.");
            }
            else
            {
                property.intValue = layerSelection;
            }
        }
    }
}
