using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.XR.OpenXR.TestTooling;

[assembly: UnityPlatform(RuntimePlatform.WindowsEditor, RuntimePlatform.Android, RuntimePlatform.OSXEditor)]
namespace UnityEngine.XR.OpenXR.Features.Android.Tests
{
    /// <summary>
    /// Mock runtime tests for AndroidXRBuildProfileFeature.
    /// </summary>
    class AndroidXRBuildProfileFeatureTests
    {
        MockOpenXREnvironment m_MockEnvironment;

        static readonly string[] k_OculusFoveationExtensions =
        {
            "XR_FB_foveation",
            "XR_FB_foveation_configuration",
            "XR_FB_swapchain_update_state",
            "XR_FB_foveation_vulkan",
        };

        [SetUp]
        public void SetUp()
        {
            m_MockEnvironment = MockOpenXREnvironment.CreateEnvironment();

            foreach (var ext in k_OculusFoveationExtensions)
                m_MockEnvironment.AddSupportedExtension(ext, 1);

            m_MockEnvironment.Settings.EnableFeature<AndroidXRBuildProfileFeature>(true);
            m_MockEnvironment.Settings.EnableFeature<FoveatedRenderingFeature>(true);
        }

        [TearDown]
        public void TearDown()
        {
            m_MockEnvironment.Dispose();
        }

        /// <summary>
        /// Verifies that the native plugin is successfully called to disable FDM attachment at foveation level zero when Foveated Rendering is enabled.
        /// </summary>
        [Test]
        public void FDMToggling_WhenFoveatedRenderingEnabled_IsEnabledAfterSessionCreate()
        {
            var feature = m_MockEnvironment.Settings.GetFeature<AndroidXRBuildProfileFeature>();
            m_MockEnvironment.Start();
            Assert.IsTrue(feature.disableFDMAttachmentAtLevelZero);
        }
    }
}
