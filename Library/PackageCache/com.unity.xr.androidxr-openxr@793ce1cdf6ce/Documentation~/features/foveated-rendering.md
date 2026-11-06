---
uid: androidxr-openxr-foveated-rendering
---
# Eye tracking and foveated rendering

Android XR supports foveated rendering. This page supplements the [OpenXR foveated rendering](xref:openxr-foveated-rendering) documentation and outlines how to configure foveated rendering on Android XR.

## Enable eye tracking and foveated rendering

To enable eye tracking and foveated rendering, you must configure the following settings in the **OpenXR** section of the **XR Plug-in Management** settings:

1. Add the **Eye Gaze Interaction Profile** under **Enabled Interaction Profiles**.
2. Enable the **Foveated Rendering** feature from **All features** under **OpenXR Feature Groups**.

For information about how to configure your interaction profiles and OpenXR features., refer to [Configure project settings](xref:androidxr-openxr-project-setup).

## Request permissions

To use foveated rendering, you must request the `android.permission.EYE_TRACKING_FINE` permission. For more information, refer to [Request eye tracking permission on Android](xref:openxr-foveated-rendering#request-eye-tracking-permission-on-android).

## Build profile configuration

The Android XR build target from the **Build Profiles** window automatically enables the Android XR feature group and the Foveated Rendering feature in the **XR Plug-in Management** settings.

For more information, refer to [Android XR build profile](xref:um-xr-android-xr-build-profile) in the Unity Manual.

## Foveation level 0 rendering optimization

To use the automatic fragment density map (FDM)-disablement optimization when you set the foveation level to `0`, your project must meet all the following requirements:

- Change to the Android XR build target or an Android XR build profile.
- Enable the **Foveated Rendering** feature in OpenXR settings.

When your project meets these prerequisites, Android XR automatically applies this optimization. You don't need additional APIs or manual package toggles.

When foveation intensity changes to or from `0`, the renderer can trigger a one-time pipeline state object (PSO) rebake for rendered scene elements. This rebake generates and caches both FDM-enabled and FDM-disabled PSO variants.

After this pair of PSO variants exists for an element, additional toggles between `0` and values greater than `0` don't add the same rebake overhead for that element. However, scene elements encountered for the first time can still incur this one-time rebake overhead the first time they render under the alternate foveation state.

If your application frequently transitions between `0` and values greater than `0`, prewarm PSO variants during loading screens so one-time rebake work happens outside active gameplay.

For more information, refer to [Warm up PSOs](https://docs.unity3d.com/6000.6/Documentation/Manual/shader-prewarm.html) in the Unity Manual.

## Additional resources

* [OpenXR foveated rendering](xref:openxr-foveated-rendering)
* [Foveated rendering](xref:um-xr-foveated-rendering) (Unity Manual)
