﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;

using UnityEngine;

namespace ThunderRoad
{
    public static class WeaponPreviewUtility
    {

        [MenuItem("Assets/ThunderRoad/Generate item preview")]
        static void GenerateWeaponsPreview()
        {
            foreach (GameObject weapon in Selection.objects)
            {
                UnityEditor.SceneManagement.PrefabStage.prefabStageOpened += OnPrefabStageOpened;

                string prefabPath = AssetDatabase.GetAssetPath(weapon);
                AssetDatabase.OpenAsset(AssetDatabase.LoadMainAssetAtPath(prefabPath));
            }

            Debug.Log("Done creating previews.");
        }


        private static void OnPrefabStageOpened(UnityEditor.SceneManagement.PrefabStage stage)
        {
            Preview p = stage.prefabContentsRoot.GetComponentInChildren<Preview>();

            if (p)
            {
                p.GenerateIcon();

                Debug.Log("Creating preview for weapon " + stage.prefabContentsRoot.name);

                AddressableAssetSettings settings = AddressableAssetSettingsDefaultObject.Settings;

                if (settings != null)
                {
                    string prefabGuid = AssetDatabase.AssetPathToGUID(stage.assetPath);
                    AddressableAssetEntry prefabEntry = settings.FindAssetEntry(prefabGuid);

                    if (prefabEntry == null)
                    {
                        Debug.LogError("Prefab entry not found for prefab " + stage.assetPath);
                        return;
                    }
                }
            }
            else
            {
                Debug.LogError("Preview component not found on weapon " + stage.prefabContentsRoot.name + ". Please make sure your prefab contains a Preview Script on a children GameObject.");
            }

            stage.ClearDirtiness();
            UnityEditor.SceneManagement.PrefabStage.prefabStageOpened -= OnPrefabStageOpened;
        }
    }
}