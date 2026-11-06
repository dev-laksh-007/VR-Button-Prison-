using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.XR.OpenXR;
using UnityEngine.XR.OpenXR.Features.Android;

namespace UnityEditor.XR.OpenXR.Features.Android
{
    /// <summary>
    /// Enables AndroidXRBuildProfileFeature at build time when the Android XR build target is active.
    /// </summary>
    class AndroidXRBuildProfileFeatureBuildHook : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport report)
        {
#if UNITY_ANDROID_XR
            var feature = OpenXRSettings.ActiveBuildTargetInstance?.GetFeature<AndroidXRBuildProfileFeature>();
            if (feature != null)
                feature.enabled = true;
            else
                UnityEngine.Debug.LogWarning("AndroidXRBuildProfileFeature could not be enabled at build time.");
#endif
        }
    }
}
