---
uid: androidxr-openxr-scene-setup
---
# Set up your scene

To set up your scene for Android XR, follow the standard AR Foundation [Scene setup](xref:arfoundation-scene-setup) process to add an **AR Session** component to your scene.

## Add an XR Origin GameObject

To use Android XR, you must add an [XR Origin](xref:xr-core-utils-xr-origin) to your scene. If you created a project from a template, the scene will contain an **XR Origin** that you can access in the **Hierarchy** window.

If you're starting a project from scratch, you need to create an **XR Origin** GameObject yourself.

To create an **XR Origin** in a new project, do one of the following:
  * Menu: **GameObject** > **XR** > **XR Origin (VR)**.
  * Menu: **GameObject** > **XR** > **Convert Main Camera to XR Rig**.

The Android XR package includes the [XR Core Utilities](https://docs.unity3d.com/Packages/com.unity.xr.core-utils@latest/manual/index.html) and [XR Plug-in Management](https://docs.unity3d.com/Packages/com.unity.xr.management@latest/manual/index.html) packages as dependencies. These packages enable the **Convert Main Camera to XR Rig** menu option. The **XR Origin (VR)** option replaces the previous command when you install the [XR Interaction Toolkit](https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@latest/manual/index.html) package.

When you choose either of these two options, Unity creates an **XR Origin** object for Android XR. This process might result in duplicate **Main Camera** objects in your scene. In that case:

1. Move the **XR Origin** object to the root of your scene (if it's not there already).
2. Disable or delete the original **Main Camera** object.
3. Make sure the newly created **XR Origin** > **Camera Offset** > **Main Camera** GameObject remains enabled in your scene.

> [!IMPORTANT]
> Ensure that the **Tracked Pose Driver** component on your XR Origin's camera has `centerEyePosition [XR HMD]` included in the **Position** and **Rotation** input actions. Without this, head tracking will fail, resulting in a poor VR or AR experience.

<a id="camera-background-passthrough"></a>

## Configure camera background for passthrough

If your scene uses the Android XR device's passthrough camera, you need to configure the camera background. Android XR passthrough requires a **Solid Color** with the **Background** color alpha channel value set to zero for your Camera's **Background Color** (Universal Render Pipeline) or **Clear Flags** (Built-In Render Pipeline).

[!IMPORTANT]
> In Unity 6.5 and newer, the Built-In Render Pipeline is deprecated and will be made obsolete in a future release. For more information, refer to [Migrating from the Built-In Render Pipeline to URP](https://docs.unity3d.com/6000.5/Documentation/Manual/urp/upgrading-from-birp.html) and [Render pipeline feature comparison](https://docs.unity3d.com/6000.5/Documentation/Manual/render-pipelines-feature-comparison.html).

If you have completed the AR Foundation scene setup steps, follow these instructions to configure your scene to render with a transparent camera background:

1. Locate the **XR Origin** GameObject in your GameObject hierarchy.
2. Expand the hierarchy to reveal the **Camera Offset** and **Main Camera** GameObjects.
3. Inspect the **Main Camera** GameObject.
4. Select from the following options. The options differ based on the render pipeline you're using:
    * URP: In the **Environment** section, set the **Background Type** to **Solid Color**.
    * Built-In Render Pipeline: Set **Clear Flags** to **Solid Color**.
5. Select the **Background** color to open the color picker.
6. Set the color's **A** value to `0`.

Your scene is now configured to support Android XR passthrough.

> [!IMPORTANT]
> The scene camera renders its image in front of the passthrough video. If you set the camera's background color (or clear flags) to a skybox or an opaque solid color, the camera background will cover the passthrough video, making it invisible and preventing the passthrough feature from working.

## Additional resources

* [Graphics settings](xref:androidxr-openxr-graphics-settings)
* [Scene setup](xref:arfoundation-scene-setup) (AR Foundation)
