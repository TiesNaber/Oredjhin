using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class ScriptableMove : ScriptableObject {

    public string moveName;
    public int damage;
    
   
    public List<BoxCollider2D> hitBox = new List<BoxCollider2D>();


    public BoxCollider2D[] hitboxes;
    
    FrameData framedata;

}

[CustomEditor(typeof(ScriptableMove))]
public class ScriptableMoveEditor: Editor
{
    bool editCollider;
    ScriptableMove m;
    SerializedProperty thisColliderList;
    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        editCollider = EditorGUILayout.Toggle("", editCollider);
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.BeginHorizontal();
        if (editCollider)
        {
            EditorGUILayout.LabelField("Edit Colliders", GUILayout.MaxWidth(200));
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Add new hit box", GUILayout.MaxWidth(200), GUILayout.MaxHeight(20)))
            {
                m.hitBox.Add(new BoxCollider2D());
                thisColliderList.InsertArrayElementAtIndex(thisColliderList.arraySize++);
            }
            if (thisColliderList.arraySize > 0)
            {
                // Consider custom commands, so you can remove which one you want.
                EditorGUILayout.LabelField("", GUILayout.MaxWidth(15));
                if (GUILayout.Button("-", GUILayout.MaxWidth(15), GUILayout.MaxHeight(20)))
                {
                    thisColliderList.DeleteArrayElementAtIndex(thisColliderList.arraySize - 1);
                }
            }
            EditorGUILayout.EndHorizontal();

            for (int i = 0; i < thisColliderList.arraySize; i++)
            {
                SerializedProperty collider = thisColliderList.GetArrayElementAtIndex(i);
                collider.objectReferenceValue = EditorGUILayout.ObjectField("Collider " + (i + 1), collider.objectReferenceValue, typeof(GameObject), true);
                //Debug.Log(collider.objectReferenceValue.ToString());
               // col.objectReferenceInstanceIDValue = EditorGUILayout.ObjectField("Put collider here", col.objectReferenceInstanceIDValue);
            }
        }

        //Choose how to display the list<> Example purposes only
        EditorGUILayout.EndHorizontal();
    }

}





