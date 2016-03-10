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
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UObject = UnityEngine.Object;

/// <summary>
/// Base class for any stacked editor container.
/// </summary>
[Serializable]
public abstract class StackEditor : Editor
{
    private Stack<Editor> activeEditors = new Stack<Editor>();

    /// <summary>
    /// Returns current inspectors active editor.
    /// </summary>
    public Editor Active
    {
        get
        {
            return activeEditors.Peek();
        }
    }

    /// <summary>
    /// Navigate back in hierarchy to given editor.
    /// </summary>
    /// <param name="editor">Target Editor in hierarchy</param>
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

    /// <summary>
    /// Removes current item from stack.
    /// </summary>
    public void Pop()
    {
        DestroyImmediate(activeEditors.Pop());
    }

    /// <summary>
    /// Add generic editor to current editor.
    /// </summary>
    /// <typeparam name="T">Which editor type to create.</typeparam>
    /// <param name="target">Target object to be inspected.</param>
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

    /// <summary>
    /// Add default editor for target.
    /// </summary>
    /// <param name="target">Target object to be inspected.</param>
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
