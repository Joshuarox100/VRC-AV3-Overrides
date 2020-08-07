using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

    private void OnGUI()
    {
        GUILayout.Box("Test");
    }
}
