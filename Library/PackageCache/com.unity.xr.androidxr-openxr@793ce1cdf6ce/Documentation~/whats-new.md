---
uid: androidxr-openxr-whats-new
---
# What's new in Unity OpenXR: Android XR <span class="long_version">X.Y.Z</span>

This update improves Android XR project configuration, default feature setup, and build stability across newer Unity versions.

Discover new features and performance improvements in the latest update to Unity OpenXR: Android XR.

For complete change details, refer to the [changelog](xref:androidxr-openxr-changelog).

## API Additions

This update adds no new API methods or properties.

## New Features

This update adds the following new features:

### Performance metrics now enabled by default in Android XR feature groups

The **Android XR: Performance Metrics** feature is now included in the Android XR feature set defaults when you enable the Android XR feature group. This helps you start monitoring runtime performance earlier in development without additional feature setup.

To learn more, refer to [Performance metrics](xref:androidxr-openxr-performance-metrics) and [Android XR Support](xref:androidxr-openxr-androidxr-support).

### Android XR settings asset is now managed in ProjectSettings

The package now stores its settings asset in the `/ProjectSettings` folder and uses the new asset name `AndroidXRSettingsManager.asset`. This update improves settings management and helps avoid import-time asset modification warnings.

To learn more about configuration workflows, refer to [Configure project settings](xref:androidxr-openxr-project-setup).

## Behavior Changes

- Minimum Android SDK requirements now depend on your Unity version: API level 25 for Unity 6.3 and newer, and API level 26 for Unity 6.5 and newer.
- When you enable the Android XR build profile, Unity now enables only key Android XR features by default.
- This update removes scene validation checks from the **Android XR: Camera (Passthrough)** feature.
- When you set foveation level to `0`, Android XR can automatically disable fragment density map (FDM) processing if your project uses the Android XR build target or build profile and has **Foveated Rendering** enabled. The first transition between `0` and non-zero values can trigger a one-time PSO rebake per rendered element.

## Deprecated

- Unity has deprecated `AndroidXRHandMeshData`. Use `MetaOpenXRHandMeshData` from the OpenXR Plug-in package (`XR_FB_hand_tracking_mesh`) instead. If your project still uses the deprecated feature, open **Project Settings** > **XR Plug-in Management** > **Project Validation**. If the migration rule is available for your installed OpenXR Plug-in version, select **Fix** to migrate automatically.

For migration details, refer to [Hand mesh data](xref:androidxr-openxr-hand-mesh-data) and [Android XR Support](xref:androidxr-openxr-androidxr-support).