/// <Licensing>
/// � 2011 (Copyright) Path-o-logical Games, LLC
/// If purchased from the Unity Asset Store, the following license is superseded 
/// by the Asset Store license.
/// Licensed under the Unity Asset Package Product License (the "License");
/// You may not use this file except in compliance with the License.
/// You may obtain a copy of the License at: http://licensing.path-o-logical.com
/// </Licensing>

using Assets.Editor.PathologicalGames.Common;
using PathologicalGames;
using UnityEditor;
using UnityEngine;


// Only compile if not using Unity iPhone
namespace Assets.Editor.PathologicalGames.PoolManager
{
    [CustomEditor(typeof(SpawnPool))]
    public class SpawnPoolInspector : UnityEditor.Editor
    {
        public bool expandPrefabs = true;

        public override void OnInspectorGUI()
        {
            var script = (SpawnPool)target;

            EditorGUI.indentLevel = 0;
            PGEditorUtils.LookLikeControls();

            script.poolName = EditorGUILayout.TextField("Pool Name", script.poolName);

            script.matchPoolScale = EditorGUILayout.Toggle("Match Pool Scale", script.matchPoolScale);
            script.matchPoolLayer = EditorGUILayout.Toggle("Match Pool Layer", script.matchPoolLayer);
        
            script.dontReparent = EditorGUILayout.Toggle("Don't Reparent", script.dontReparent);

            script._dontDestroyOnLoad = EditorGUILayout.Toggle("Don't Destroy On Load", script._dontDestroyOnLoad);
        
            script.logMessages = EditorGUILayout.Toggle("Log Messages", script.logMessages);

            this.expandPrefabs = PGEditorUtils.SerializedObjFoldOutList<PrefabPool>
            (
                "Per-Prefab Pool Options", 
                script._perPrefabPoolOptions,
                this.expandPrefabs,
                ref script._editorListItemStates,
                true
            );

            // Flag Unity to save the changes to to the prefab to disk
            if (GUI.changed)
                EditorUtility.SetDirty(target);
        }

    }
}


