// Copyright (c) 2016, Jöran Malek
// This file may be modified to your likes.
// But remember http://unity3d.com/legal/as_terms
// If you have any additions for this that could be generally
// useful, feel free to contact me.
// This package is provided free-of-charge.
// If you paid for it
//  unless you donated here http://www.donation-tracker.com/u/alivedevil
// request a back-fund.

using System;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[Serializable]
[CustomEditor(typeof(ContainerObject))]
public class ContainerObjectEditor : StackEditorBase
{
    private ReorderableList childrenList;

    [MenuItem("Assets/Create/Container")]
    [ContextMenu("Create/Container")]
    public static void CreateContainer()
    {
        var path = "";
        var obj = Selection.activeObject;
        if (!obj)
            path = "Assets";
        else
            path = AssetDatabase.GetAssetPath(obj.GetInstanceID());

        if (path.Length > 0)
        {
            if (!AssetDatabase.IsValidFolder(path))
                path = Path.GetDirectoryName(path);
        }

        var instance = CreateInstance<ContainerObject>();
        ProjectWindowUtil.CreateAsset(instance, AssetDatabase.GenerateUniqueAssetPath(path + "/Container.asset"));
        Selection.activeObject = instance;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        childrenList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    private void AddChild(ReorderableList list)
    {
        var children = serializedObject.FindProperty("children");
        var index = children.arraySize++;
        var element = children.GetArrayElementAtIndex(index);

        var instance = CreateInstance<ChildObject>();
        instance.hideFlags |= HideFlags.HideInHierarchy;
        AssetDatabase.AddObjectToAsset(instance, serializedObject.targetObject);

        element.objectReferenceValue = instance;
        serializedObject.ApplyModifiedProperties();
        AssetDatabase.SaveAssets();
    }

    private void DrawChildElement(Rect rect, int index, bool isActive, bool isFocused)
    {
        var children = serializedObject.FindProperty("children");
        var element = children.GetArrayElementAtIndex(index);
        var child = element.objectReferenceValue as ChildObject;

        var labelRect = new Rect(rect.x, rect.y, rect.width - 40, rect.height);
        var buttonRect = new Rect(rect.x + rect.width - 40, rect.y, 40, rect.height);

        if (isActive)
        {
            child.name = EditorGUI.TextField(labelRect, child.name, new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft
            });
        }
        else
            EditorGUI.LabelField(labelRect, child.name, new GUIStyle(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleLeft
            });

        if (GUI.Button(buttonRect, "Edit"))
            StackEditor.Push(element.objectReferenceValue);
    }

    private void DrawChildrenHeader(Rect rect)
    {
        EditorGUI.LabelField(rect, "Children");
    }

    private void OnDisable()
    {
        childrenList.drawElementCallback -= DrawChildElement;
        childrenList.drawHeaderCallback -= DrawChildrenHeader;
        childrenList.onRemoveCallback -= RemoveChild;
        childrenList.onAddCallback -= AddChild;
    }

    private void OnEnable()
    {
        var children = serializedObject.FindProperty("children");
        childrenList = new ReorderableList(serializedObject, children, true, true, true, true);
        childrenList.drawElementCallback += DrawChildElement;
        childrenList.drawHeaderCallback += DrawChildrenHeader;
        childrenList.onRemoveCallback += RemoveChild;
        childrenList.onAddCallback += AddChild;
    }

    private void RemoveChild(ReorderableList list)
    {
        if (EditorUtility.DisplayDialog("Confirm deletion of child", "Do you really want to delete this child? This can't be undone and it won't ever come back.", "Yes", "No"))
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(list.index);
            DestroyImmediate(element.objectReferenceValue, true);
            element.objectReferenceValue = null;

            ReorderableList.defaultBehaviours.DoRemoveButton(list);

            serializedObject.ApplyModifiedPropertiesWithoutUndo();
            AssetDatabase.SaveAssets();
        }
    }
}
