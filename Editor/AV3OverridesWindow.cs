using System;
using System.IO;
using UnityEditor;
using UnityEngine;
using VRC.SDK3.Avatars.Components;

public class AV3OverridesWindow : EditorWindow
{
    private readonly AV3OverridesManager manager = new AV3OverridesManager();

    private bool found;
    private bool focused;
    private int windowTab;

    [MenuItem("Tools/Joshuarox100/AV3 Overrides/Convert Overrides")]
    public static void ConvertOverride()
    {
        AV3OverridesWindow window = (AV3OverridesWindow)GetWindow(typeof(AV3OverridesWindow), false, "AV3 Overrides");
        window.minSize = new Vector2(375f, 294f);
        window.wantsMouseMove = true;
        window.Show();
    }

    [MenuItem("Tools/Joshuarox100/AV3 Overrides/Check For Updates")]
    public static void CheckForUpdates()
    {
        AV3OverridesManager.CheckForUpdates();
    }

    private void OnFocus()
    {
        found = manager.FindTemplates();
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.BeginArea(new Rect((position.width / 2f) - (375f / 2f), 5f, 375f, 290f));
        windowTab = GUILayout.Toolbar(windowTab, new string[] { "Convert", "About" });
        DrawLine(false);
        switch (windowTab)
        {
            case 0:
                if (EditorApplication.isPlaying)
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("Overrides can only be converted outside of Play Mode.", MessageType.Info);
                }
                else if (found)
                {
                    GUILayout.BeginVertical();
                    DrawSetupWindow();
                    GUILayout.EndVertical();
                }
                else
                {
                    EditorGUILayout.Space();
                    EditorGUILayout.HelpBox("Templates not found!", MessageType.Error);
                    EditorGUILayout.HelpBox("Make sure that the original Animators and Animations included in the package haven't been renamed!", MessageType.None);
                }
                GUILayout.EndArea();
                GUILayout.FlexibleSpace();
                EditorGUILayout.Space();
                GUILayout.EndVertical();
                break;
            case 1:
                GUILayout.BeginVertical();
                DrawAboutWindow();
                GUILayout.EndVertical();
                GUILayout.EndArea();
                GUILayout.FlexibleSpace();
                EditorGUILayout.Space();
                GUILayout.EndVertical();
                break;
        }
    }

    private void DrawAboutWindow()
    {
        string version = (AssetDatabase.FindAssets("VERSION", new string[] { manager.relativePath }).Length > 0) ? " " + File.ReadAllText(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("VERSION", new string[] { manager.relativePath })[0])) : "";
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box("<b><size=18>AV3 Overrides" + version + "</size></b>", new GUIStyle(GUI.skin.GetStyle("Box")) { richText = true }, GUILayout.Width(300f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box("<size=13>Author: Joshuarox100</size>", new GUIStyle(GUI.skin.GetStyle("Box")) { richText = true, normal = new GUIStyleState() { background = null } });
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        DrawLine();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box("<b><size=15>Summary</size></b>", new GUIStyle(GUI.skin.GetStyle("Box")) { richText = true }, GUILayout.Width(200f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box("Make upgrading to 3.0 a breeze with AV3 Overrides!", new GUIStyle(GUI.skin.GetStyle("Box")) { richText = true, normal = new GUIStyleState() { background = null } }, GUILayout.Width(350f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        DrawLine();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box("<b><size=15>Troubleshooting</size></b>", new GUIStyle(GUI.skin.GetStyle("Box")) { richText = true }, GUILayout.Width(200f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Box("If you're having issues or want to contact me, you can find more information at the Github page linked below!", new GUIStyle(GUI.skin.GetStyle("Box")) { richText = true, normal = new GUIStyleState() { background = null } }, GUILayout.Width(350f));
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        EditorGUILayout.Space();
        DrawLine(false);
        GUILayout.Label("Github Repository", new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.UpperCenter });
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Open in Browser", GUILayout.Width(250)))
        {
            Application.OpenURL("https://github.com/Joshuarox100/VRC-AV3-Overrides");
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
    }

    private void DrawSetupWindow()
    {
        EditorGUILayout.BeginVertical();
        GUILayout.BeginVertical(GUILayout.Height(54f));
        EditorGUI.BeginChangeCheck();
        GUILayout.FlexibleSpace();
        SelectAvatarDescriptor();
        if (manager.avatar == null)
        {
            EditorGUILayout.HelpBox("No Avatars found in the current Scene!", MessageType.Warning);
        }
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();
        DrawLine();
        GUILayout.Label("Override Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.GetStyle("Box")), GUILayout.Height(50f));
        GUILayout.FlexibleSpace();
        manager.overrides = (AnimatorOverrideController)EditorGUILayout.ObjectField(new GUIContent("Custom Override", "The AV3 Override Controller to be converted."), manager.overrides, typeof(AnimatorOverrideController), false);
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Replace Animators", "Replace Animators already present in the Avatar Descriptor."), GUILayout.Width(145));
        manager.replaceAnimators = Convert.ToBoolean(GUILayout.Toolbar(Convert.ToInt32(manager.replaceAnimators), new string[] { "No", "Yes" }));
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        DrawLine();
        GUILayout.Label("Output Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical(new GUIStyle(GUI.skin.GetStyle("Box")), GUILayout.Height(50f));
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField(new GUIContent("Destination", "The folder where generated files will be saved to."), new GUIStyle(GUI.skin.GetStyle("Box")) { alignment = TextAnchor.MiddleLeft, normal = new GUIStyleState() { background = null } });
        GUILayout.FlexibleSpace();
        string displayPath = manager.outputPath.Replace('\\', '/');
        displayPath = ((new GUIStyle(GUI.skin.GetStyle("Box")) { richText = true, hover = GUI.skin.GetStyle("Button").active }).CalcSize(new GUIContent("<i>" + displayPath + "</i>")).x > 210) ? "..." + displayPath.Substring((displayPath.IndexOf('/', displayPath.Length - 29) != -1) ? displayPath.IndexOf('/', displayPath.Length - 29) : displayPath.Length - 29) : displayPath;
        while ((new GUIStyle(GUI.skin.GetStyle("Box")) { richText = true, hover = GUI.skin.GetStyle("Button").active }).CalcSize(new GUIContent("<i>" + displayPath + "</i>")).x > 210)
        {
            displayPath = "..." + displayPath.Substring(4);
        }
        if (GUILayout.Button("<i>" + displayPath + "</i>", new GUIStyle(GUI.skin.GetStyle("Box")) { richText = true, hover = GUI.skin.GetStyle("Button").active }, GUILayout.MinWidth(210)))
        {
            string absPath = EditorUtility.OpenFolderPanel("Destination Folder", "", "");
            if (absPath.StartsWith(Application.dataPath))
            {
                manager.outputPath = "Assets" + absPath.Substring(Application.dataPath.Length);
            }
        }
        EditorGUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label(new GUIContent("Overwrite All", "Automatically overwrite existing files if needed."), GUILayout.Width(145));
        manager.autoOverwrite = Convert.ToBoolean(GUILayout.Toolbar(Convert.ToInt32(manager.autoOverwrite), new string[] { "No", "Yes" }));
        GUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.Space();
        DrawLine();
        if (GUILayout.Button("Generate"))
        {
            manager.GenerateAnimators();
            EditorUtility.ClearProgressBar();
        }
        EditorGUILayout.EndVertical();

        if (mouseOverWindow != null && mouseOverWindow == this)
        {
            Repaint();
            focused = true;
        }
        else if (focused && mouseOverWindow == null)
        {
            Repaint();
            focused = false;
        }
    }

    /*
     * These next two functions are literally just code from the Expression Menu for selecting the avatar.
     */

    void SelectAvatarDescriptor()
    {
        var descriptors = FindObjectsOfType<VRCAvatarDescriptor>();
        if (descriptors.Length > 0)
        {
            //Compile list of names
            string[] names = new string[descriptors.Length];
            for (int i = 0; i < descriptors.Length; i++)
                names[i] = descriptors[i].gameObject.name;

            //Select
            var currentIndex = Array.IndexOf(descriptors, manager.avatar);
            var nextIndex = EditorGUILayout.Popup(new GUIContent("Active Avatar", "The Avatar you want to use an override on."), currentIndex, names);
            if (nextIndex < 0)
                nextIndex = 0;
            if (nextIndex != currentIndex)
                SelectAvatarDescriptor(descriptors[nextIndex]);
        }
        else
            SelectAvatarDescriptor(null);
    }
    void SelectAvatarDescriptor(VRCAvatarDescriptor desc)
    {
        if (desc == manager.avatar)
            return;

        manager.avatar = desc;
    }

    private void DrawLine(bool addSpace = true)
    {
        var rect = EditorGUILayout.BeginHorizontal();
        Handles.color = Color.gray;
        Handles.DrawLine(new Vector2(rect.x - 15, rect.y), new Vector2(rect.width + 15, rect.y));
        EditorGUILayout.EndHorizontal();
        if (addSpace)
        {
            EditorGUILayout.Space();
        }
    }
}
