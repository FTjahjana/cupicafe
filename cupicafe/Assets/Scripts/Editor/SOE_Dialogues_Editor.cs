#if UNITY_EDITOR
using UnityEngine; using UnityEditor;

[CustomEditor(typeof(SOE_Dialogues))]
public class SOE_Dialogues_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        //Main Ref: https://docs.unity3d.com/6000.2/Documentation/Manual/editor-CustomEditors.html

        serializedObject.Update();
        //P.S. dont look up the method alone its confusing. Use the main ref above to get this one.

        var dialogues_List = serializedObject.FindProperty("dialogues");
        EditorGUILayout.HelpBox("P.S. dont use the plus button itll be empty element, instead use the specific add dialogue elements buttons", MessageType.Info);

        /* https://docs.unity3d.com/6000.2/Documentation/ScriptReference/EditorGUILayout.PropertyField.html
        the parts im calling out are SerializedProperty property, bool includeChildren. (see link)

        Include Children for list objects are basically the objects. like the dialogues in this case.
        */
        EditorGUILayout.PropertyField(dialogues_List, true);
        /*This one's an easy one. Link for syntax: (Also see: GUIStyle (linked in the syntax itself))
        https://docs.unity3d.com/6000.2/Documentation/ScriptReference/EditorGUILayout.LabelField.html*/
        EditorGUILayout.LabelField("Add Dialogue Elements", EditorStyles.boldLabel);
        
        if (GUILayout.Button("Add Dialogue"))
        {
            var newElem = new Dialogue();
            dialogues_List.arraySize++;
            dialogues_List.GetArrayElementAtIndex(dialogues_List.arraySize - 1).managedReferenceValue = newElem;
            //https://docs.unity3d.com/6000.2/Documentation/ScriptReference/SerializedProperty-managedReferenceValue.html
        }
        if (GUILayout.Button("Add Notif Dialogue"))
        {
            var newElem = new Notif_Dialogue();
            dialogues_List.arraySize++;
            dialogues_List.GetArrayElementAtIndex(dialogues_List.arraySize - 1).managedReferenceValue = newElem;
        }
        if (GUILayout.Button("Add Input Dialogue"))
        {
            var newElem = new Input_Dialogue();
            dialogues_List.arraySize++;
            dialogues_List.GetArrayElementAtIndex(dialogues_List.arraySize - 1).managedReferenceValue = newElem;
        }

        serializedObject.ApplyModifiedProperties();
    }
}
#endif
