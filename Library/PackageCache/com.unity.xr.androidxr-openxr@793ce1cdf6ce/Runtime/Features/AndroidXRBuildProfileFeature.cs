using System;
using System.Runtime.InteropServices;
using UnityEngine.XR.OpenXR.NativeTypes;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.XR.OpenXR.Features;
#endif

namespace UnityEngine.XR.OpenXR.Features.Android
{
    /// <summary>
    /// Enables Android XR build profile-specific behavior such as FDM attachment disablement.
    /// This feature is enabled at build time when the Android XR build target is active.
    /// </summary>
#if UNITY_EDITOR
    [OpenXRFeature(Hidden = true,
        UiName = "",
        OpenxrExtensionStrings = "",
        BuildTargetGroups = new[] { BuildTargetGroup.Android, BuildTargetGroup.Standalone },
        Company = Constants.k_CompanyName,
        Desc = "Enables Android XR build profile-specific behavior.",
        Category = FeatureCategory.Feature,
        FeatureId = featureId,
        Version = "0.1.0")]
#endif
    class AndroidXRBuildProfileFeature : OpenXRFeature
    {
        /// <summary>
        /// The feature id string. This is used to give the feature an id for reference.
        /// </summary>
        public const string featureId = "com.unity.openxr.feature.androidxr-build-profile";

        /// <summary>
        /// Indicates whether FDM attachment is configured to be disabled at foveation level zero.
        /// </summary>
        internal bool disableFDMAttachmentAtLevelZero { get; private set; }

        /// <summary>
        /// Called after xrCreateSession. Initializes Android XR build profile-specific behavior.
        /// </summary>
        /// <param name="xrSession">Handle of the xrSession.</param>
        protected override void OnSessionCreate(ulong xrSession)
        {
            var foveatedRenderingFeature = OpenXRSettings.Instance.GetFeature<FoveatedRenderingFeature>();
            if (foveatedRenderingFeature == null || !foveatedRenderingFeature.enabled)
                return;

            try
            {
                var result = NativeApi.MetaSetDisableFDMAttachmentAtLevelZero(true);

                if (result.IsSuccess())
                    disableFDMAttachmentAtLevelZero = true;
            }
            catch (EntryPointNotFoundException)
            {
                Debug.LogWarning($"{nameof(AndroidXRBuildProfileFeature)}: MetaSetDisableFDMAttachmentAtLevelZero not found in native plugin. FDM toggling will not be enabled.");
            }
        }

        static class NativeApi
        {
            [DllImport(Constants.k_OpenXRLibrary)]
            public static extern XrResult MetaSetDisableFDMAttachmentAtLevelZero(bool shouldDisable);
        }
    }
}
