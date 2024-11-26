using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(PlayerBaseState), true)]
public class PlayerBaseStateDropdownDrawer : PropertyDrawer
{
    private const float SpacingAbove = 20f; // Add 5 pixels of spacing above the dropdown

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Calculate height: spacing + dropdown + additional fields if an object is selected
        float height = SpacingAbove + EditorGUIUtility.singleLineHeight; // Spacing + dropdown

        if (property.objectReferenceValue != null)
        {
            SerializedObject so = new(property.objectReferenceValue);
            SerializedProperty iterator = so.GetIterator();
            if (iterator.NextVisible(true)) // Skip to first visible property
            {
                do
                {
                    if (iterator.propertyPath == "m_Script") continue; // Skip script reference
                    height += EditorGUI.GetPropertyHeight(iterator, true) + 2;
                } while (iterator.NextVisible(false));
            }
        }

        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        // Adjust position for spacing
        position.y += SpacingAbove;

        // Dropdown to select the ScriptableObject
        Rect dropdownRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        // Find all ScriptableObjects of type PlayerBaseState or derived types
        string[] guids = AssetDatabase.FindAssets($"t:PlayerBaseState");
        PlayerBaseState[] options = new PlayerBaseState[guids.Length];
        string[] names = new string[guids.Length];

        for (int i = 0; i < guids.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(guids[i]);
            options[i] = AssetDatabase.LoadAssetAtPath<PlayerBaseState>(path);
            names[i] = options[i].name;
        }

        // Get the currently selected value
        PlayerBaseState current = property.objectReferenceValue as PlayerBaseState;
        int currentIndex = current != null ? System.Array.IndexOf(options, current) : -1;

        // Draw the dropdown
        int selectedIndex = EditorGUI.Popup(dropdownRect, label.text, currentIndex, names);

        // Update the selected value
        if (selectedIndex >= 0 && selectedIndex < options.Length)
        {
            property.objectReferenceValue = options[selectedIndex];
        }
        else if (selectedIndex == -1)
        {
            property.objectReferenceValue = null;
        }

        // If a ScriptableObject is selected, render its properties
        if (property.objectReferenceValue != null)
        {
            ScriptableObject so = property.objectReferenceValue as ScriptableObject;
            SerializedObject serializedObject = new SerializedObject(so);

            serializedObject.Update();

            // Draw the properties of the selected ScriptableObject
            Rect fieldRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight + 2, position.width, EditorGUIUtility.singleLineHeight);

            SerializedProperty iterator = serializedObject.GetIterator();
            if (iterator.NextVisible(true)) // Skip to first visible property
            {
                do
                {
                    if (iterator.propertyPath == "m_Script") continue; // Skip script reference
                    float fieldHeight = EditorGUI.GetPropertyHeight(iterator, true);
                    EditorGUI.PropertyField(fieldRect, iterator, true);
                    fieldRect.y += fieldHeight + 2;
                } while (iterator.NextVisible(false));
            }

            serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.EndProperty();
    }
}
