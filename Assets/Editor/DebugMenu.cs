﻿using UnityEngine;
using UnityEditor;

#if UNITY_EDITOR
public static class DebugMenu
{
    [MenuItem("Debug/Print Global Position")]
     public static void PrintGlobalPosition()
     {
         if (Selection.activeGameObject != null)
         {
             Debug.Log(Selection.activeGameObject.name + " is at " + Selection.activeGameObject.transform.position);
         }
     }
}
#endif