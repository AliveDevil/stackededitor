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
// File Version: v0.1

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UObject = UnityEngine.Object;

[Serializable]
public abstract class StackEditor : Editor
{
    private Stack<Editor> activeEditors = new Stack<Editor>();

    public Editor Active
    {
        get
        {
            return activeEditors.Peek();
        }
    }

    public void Back(Editor editor)
    {
        while (Active != editor)
            Pop();
    }

    public override void OnInspectorGUI()
    {
        foreach (var item in activeEditors.Reverse())
            item.DrawHeader();

        if (activeEditors.Count > 0)
            activeEditors.Peek().OnInspectorGUI();
    }

    public void Pop()
    {
        DestroyImmediate(activeEditors.Pop());
    }

    public void Push<T>(UObject target) where T : Editor, new()
    {
        var editor = CreateEditor(target, typeof(T));
        activeEditors.Push(editor);
        if (editor is StackEditorBase)
        {
            var stackEditor = (StackEditorBase)editor;
            stackEditor.StackEditor = this;
            stackEditor.Load();
        }
        Repaint();
    }

    public void Push(UObject target)
    {
        var editor = CreateEditor(target);
        activeEditors.Push(editor);
        if (editor is StackEditorBase)
        {
            var stackEditor = (StackEditorBase)editor;
            stackEditor.StackEditor = this;
            stackEditor.Load();
        }
        Repaint();
    }

    private void OnDisable()
    {
        while (activeEditors.Count > 0)
            Pop();
    }
}
