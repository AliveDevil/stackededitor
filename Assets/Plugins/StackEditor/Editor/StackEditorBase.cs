// Copyright (c) 2016, Jöran Malek
// This file may be modified to your likes.
// But remember http://unity3d.com/legal/as_terms
// If you have any additions for this that could be generally
// useful, feel free to contact me.
// This package is provided free-of-charge.
// If you paid for it
//  unless you donated here http://www.donation-tracker.com/u/alivedevil
// request a back-fund.
//
// File Version: v0.2

using System;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[Serializable]
public abstract class StackEditorBase : Editor
{
    public MethodInfo HeaderIconMethod = typeof(Editor).GetMethod("OnHeaderIconGUI", BindingFlags.NonPublic | BindingFlags.Instance);
    public MethodInfo HeaderTitleMethod = typeof(Editor).GetMethod("OnHeaderTitleGUI", BindingFlags.NonPublic | BindingFlags.Instance);
    public PropertyInfo TargetTitleProperty = typeof(Editor).GetProperty("targetTitle", BindingFlags.NonPublic | BindingFlags.Instance);

    public StackEditor StackEditor
    {
        get; set;
    }

    public virtual string Title
    {
        get
        {
            return GetType().Name;
        }
    }

    public void Load()
    {
        OnLoad();
    }

    protected override void OnHeaderGUI()
    {
        GUILayout.BeginHorizontal(EditorGUIUtility.GetBuiltinSkin(EditorSkin.Inspector).FindStyle("In BigTitle"));
        GUILayout.Space(38f);
        GUILayout.BeginVertical();
        GUILayout.Space(19f);
        GUILayout.BeginHorizontal();
        NavigationControls();
        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
        GUILayout.EndHorizontal();

        Rect lastRect = GUILayoutUtility.GetLastRect();
        Rect r = new Rect(lastRect.x, lastRect.y, lastRect.width, lastRect.height);
        Rect rect = new Rect(r.x + 6f, r.y + 6f, 32f, 32f);
        HeaderIconMethod.Invoke(this, new object[] { rect });
        Rect rect2 = new Rect(r.x + 44f, r.y + 6f, r.width - 44f - 38f - 4f, 16f);
        HeaderTitleMethod.Invoke(this, new object[] { rect2, (string)TargetTitleProperty.GetValue(this, null) });
    }

    protected virtual void OnLoad()
    {
    }

    private void NavigationControls()
    {
        GUILayoutUtility.GetRect(10f, 10f, 16f, 16f, EditorStyles.layerMaskField);
        GUILayout.FlexibleSpace();

        if (StackEditor.Active != this && GUILayout.Button("Navigate Here", EditorStyles.miniButton))
        {
            StackEditor.Back(this);
            GUIUtility.ExitGUI();
        }
    }
}
