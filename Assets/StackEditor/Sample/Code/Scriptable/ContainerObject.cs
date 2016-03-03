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
using UnityEngine;

[Serializable]
public class ContainerObject : ScriptableObject
{
    [SerializeField]
    private ChildObject[] children;

    public ChildObject[] Children
    {
        get
        {
            return children;
        }
    }
}
