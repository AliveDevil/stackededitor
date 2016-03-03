﻿// Copyright (c) 2016, Jöran Malek
// This file may be modified to your likes.
// But remember http://unity3d.com/legal/as_terms
// If you have any additions for this that could be generally
// useful, feel free to contact me.
// This package is provided free-of-charge.
// If you paid for it
//  unless you donated here http://www.donation-tracker.com/u/alivedevil
// request a back-fund.

using System;
using UnityEditor;
using UnityEngine;

[Serializable]
[CustomEditor(typeof(ChildObject))]
public class ChildObjectEditor : StackEditorBase
{
    public override void OnInspectorGUI()
    {
        if (GUILayout.Button("Back"))
            StackEditor.Pop();
        else
        {

        }
    }
}
