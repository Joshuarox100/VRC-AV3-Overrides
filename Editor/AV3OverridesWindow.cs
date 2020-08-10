using System;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

public class AV3OverridesWindow : EditorWindow
{
    private readonly AV3OverridesManager manager = new AV3OverridesManager();

    [MenuItem("Window/AV3 Overrides/Convert Override")]
    public static void ConvertOverride()
    {
        AV3OverridesWindow window = (AV3OverridesWindow)GetWindow(typeof(AV3OverridesWindow), false, "AV3 Overrides");
        //window.minSize = new Vector2(375f, 525f);
        window.wantsMouseMove = true;
        window.Show();
    }

    private void OnFocus()
    {
        manager.UpdatePaths();
    }

    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        manager.avatar = (VRCAvatarDescriptor)EditorGUILayout.ObjectField("Avatar", manager.avatar, typeof(VRCAvatarDescriptor), true);
        manager.overrides = (AnimatorOverrideController)EditorGUILayout.ObjectField("Override", manager.overrides, typeof(AnimatorOverrideController), false);
        GUILayout.Label(manager.outputPath);
        manager.autoOverwrite = Convert.ToBoolean(GUILayout.Toolbar(Convert.ToInt32(manager.autoOverwrite), new string[] { "No", "Yes" }));
        if (GUILayout.Button("Generate"))
        {
            manager.GenerateAnimators();
        }
        EditorGUILayout.EndVertical();
    }
}
