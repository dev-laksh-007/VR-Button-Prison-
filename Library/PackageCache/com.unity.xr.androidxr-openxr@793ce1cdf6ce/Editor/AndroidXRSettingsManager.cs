using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;
using UnityEngine.Serialization;
using UnityEngine.XR.OpenXR.Features;
using UnityEngine.XR.OpenXR.Features.Android;

namespace UnityEditor.XR.OpenXR.Features.Android
{
    [MovedFrom(false, null, null, "AndroidXRSettingsInitializer")]
    [FilePath("ProjectSettings/AndroidXRSettingsManager.asset", FilePathAttribute.Location.ProjectFolder)]
    class AndroidXRSettingsManager : ScriptableSingleton<AndroidXRSettingsManager>
    {
        static readonly string k_ProjectSettingsPath = Path.Combine("ProjectSettings", "AndroidXRSettingsManager.asset");
        static readonly string k_LegacyAssetDirectoryPath = Path.Combine("Assets", "XR", "AndroidXR");
        static readonly string k_LegacyAssetInitializerPath = Path.Combine(k_LegacyAssetDirectoryPath, "AndroidXRSettingsInitializer");

        static readonly string k_SessionStateLegacyInitializerFileCheckKey = "AndroidXRLegacySettingsInitializerFileChecked";

        static readonly string[] s_RecommendedAndroidXRFeatureIds =
        {
            "com.unity.openxr.feature.input.handinteraction",
            "com.unity.openxr.feature.androidxr-support",
            "com.unity.openxr.feature.foveatedrendering"
        };

        [SerializeField, HideInInspector, FormerlySerializedAs("isInitialized")]
        bool hasInitializedXRSettings;

        [InitializeOnLoadMethod]
        static void OnLoad()
        {
            if (!SessionState.GetBool(k_SessionStateLegacyInitializerFileCheckKey, false))
            {
                if (AssetDatabase.AssetPathExists(k_LegacyAssetInitializerPath))
                {
                    MigrateFromLegacyInitializer();
                    instance.Save(true);
                }

                SessionState.SetBool(k_SessionStateLegacyInitializerFileCheckKey, true);
            }

            EditorApplication.delayCall += OnEditorReloadedDelayed;
        }

        static void OnEditorReloadedDelayed()
        {
#if UNITY_ANDROID_XR
            if (!instance.hasInitializedXRSettings)
            {
                instance.InitializeAndroidXRSettings();
                instance.hasInitializedXRSettings = true;
                instance.Save(true);
            }
#else
            if (instance.hasInitializedXRSettings)
            {
                instance.DisableAndroidXRSettings();
                instance.hasInitializedXRSettings = false;
                instance.Save(true);
            }
#endif
        }

        static void MigrateFromLegacyInitializer()
        {
            Debug.Log("Migrating Android XR Settings asset file to project settings.");

            File.Copy(k_LegacyAssetInitializerPath, k_ProjectSettingsPath, true);
            AssetDatabase.DeleteAsset(k_LegacyAssetInitializerPath);

            var assetDirectoryIsEmpty = !Directory.EnumerateFileSystemEntries(k_LegacyAssetDirectoryPath)
                .Any(entry =>
                {
                    var fileName = Path.GetFileName(entry);
                    return fileName != ".DS_Store" && fileName != "Thumbs.db";
                });

            if (assetDirectoryIsEmpty)
            {
                AssetDatabase.DeleteAsset(k_LegacyAssetDirectoryPath);
            }
        }

        void InitializeAndroidXRSettings()
        {
            var androidXRFeatureSet = OpenXRFeatureSetManager.GetFeatureSetWithId(BuildTargetGroup.Android, AndroidFeatureSet.featureSetId);

            if (androidXRFeatureSet == null)
                return;

            HashSet<OpenXRFeature> prevDisabledFeatures = new HashSet<OpenXRFeature>();

            foreach (var featureId in androidXRFeatureSet.defaultFeatureIds)
            {
                var feature = FeatureHelpers.GetFeatureWithIdForBuildTarget(BuildTargetGroup.Android, featureId);
                if (feature != null && !feature.enabled)
                {
                    prevDisabledFeatures.Add(feature);
                }
            }

            androidXRFeatureSet.isEnabled = true;
            OpenXRFeatureSetManager.SetFeaturesFromEnabledFeatureSets(BuildTargetGroup.Android);

            foreach (var feature in prevDisabledFeatures)
            {
                feature.enabled = false;
            }

            //Enable recommended Android XR features
            foreach (var featureId in s_RecommendedAndroidXRFeatureIds)
            {
                var feature = FeatureHelpers.GetFeatureWithIdForBuildTarget(BuildTargetGroup.Android, featureId);
                if (feature != null)
                    feature.enabled = true;
            }
        }

        void DisableAndroidXRSettings()
        {
            var androidXRFeatureSet = OpenXRFeatureSetManager.GetFeatureSetWithId(BuildTargetGroup.Android, AndroidFeatureSet.featureSetId);

            if (androidXRFeatureSet == null)
                return;

            androidXRFeatureSet.isEnabled = false;

            //Disable recommended Android XR features
            foreach (var featureId in s_RecommendedAndroidXRFeatureIds)
            {
                var feature = FeatureHelpers.GetFeatureWithIdForBuildTarget(BuildTargetGroup.Android, featureId);
                if (feature != null)
                    feature.enabled = false;
            }
        }
    }
}
