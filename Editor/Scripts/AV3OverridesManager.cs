﻿using BMBLibraries.Classes;
using BMBLibraries.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Networking;
using VRC.SDK3.Avatars.Components;

public class AV3OverridesManager : UnityEngine.Object
{
    public VRCAvatarDescriptor avatar;

    private RuntimeAnimatorController dummy;
    public AnimatorOverrideController overrides;
    public bool replaceAnimators;

    private AnimatorController templateFX;

    public string relativePath;
    public string outputPath;
    public bool autoOverwrite = false;

    private Backup backupManager;
    private AssetList generated;

    public AV3OverridesManager() { }

    public void GenerateAnimators()
    {
        try 
        {
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Starting", 0f);
            if (avatar == null)
            {
                EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: No avatar is selected.", "Close");
                return;
            }
            else if (overrides == null)
            {
                EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: No override controller provided.", "Close");
                return;
            }
            else if (dummy == null)
            {
                EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to locate dummy controller.", "Close");
                return;
            }
            else if (overrides.runtimeAnimatorController != dummy)
            {
                EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Provided Override Controller is invalid.", "Close");
                return;
            }

            //Check Animations for incompatibilities

            List<KeyValuePair<AnimationClip, AnimationClip>> clips = new List<KeyValuePair<AnimationClip, AnimationClip>>();
            overrides.GetOverrides(clips);

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Checking Animation Compatibility", 0f);
            foreach (KeyValuePair<AnimationClip, AnimationClip> pair in clips)
            {
                if (!CheckCompatibility(pair, out Type problem, out string propertyName))
                {
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: " + pair.Value.name + " cannot be used for " + pair.Key.name + " because it modifies an invalid property type!\n\nInvalid Property Type: " + problem.Name + "\nName: " + propertyName, "Close");
                    return;
                }
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Checking Animation Compatibility", 0.2f);

            //Check destination / make backup

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Initializing Restore Point", 0.2f);
            VerifyDestination();
            backupManager = new Backup();

            //Copy templates

            generated = new AssetList();

            AssetDatabase.SaveAssets();

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.25f);
            switch (CopySDKTemplate(avatar.name + "_Base.controller", "vrc_AvatarV3LocomotionLayer"))
            {
                case 1:
                    EditorUtility.DisplayDialog("AV3 Overrides", "Cancelled.", "Close");
                    RevertChanges();
                    return;
                case 3:
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more files!", "Close");
                    RevertChanges();
                    return;
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.3f);
            switch (CopySDKTemplate(avatar.name + "_Additive.controller", "vrc_AvatarV3IdleLayer"))
            {
                case 1:
                    EditorUtility.DisplayDialog("AV3 Overrides", "Cancelled.", "Close");
                    RevertChanges();
                    return;
                case 3:
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more files!", "Close");
                    RevertChanges();
                    return;
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.35f);
            switch (CopySDKTemplate(avatar.name + "_Gesture.controller", "vrc_AvatarV3HandsLayer"))
            {
                case 1:
                    EditorUtility.DisplayDialog("AV3 Overrides", "Cancelled.", "Close");
                    RevertChanges();
                    return;
                case 3:
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more files!", "Close");
                    RevertChanges();
                    return;
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.4f);
            switch (CopySDKTemplate(avatar.name + "_Action.controller", "vrc_AvatarV3ActionLayer"))
            {
                case 1:
                    EditorUtility.DisplayDialog("AV3 Overrides", "Cancelled.", "Close");
                    RevertChanges();
                    return;
                case 3:
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more files!", "Close");
                    RevertChanges();
                    return;
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.45f);
            switch (CopySDKTemplate(avatar.name + "_FX.controller", "vrc_AvatarV3FaceLayer"))
            {
                case 1:
                    EditorUtility.DisplayDialog("AV3 Overrides", "Cancelled.", "Close");
                    RevertChanges();
                    return;
                case 3:
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more files!", "Close");
                    RevertChanges();
                    return;
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.5f);
            switch (CopySDKTemplate(avatar.name + "_Sitting.controller", "vrc_AvatarV3SittingLayer"))
            {
                case 1:
                    EditorUtility.DisplayDialog("AV3 Overrides", "Cancelled.", "Close");
                    RevertChanges();
                    return;
                case 3:
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more files!", "Close");
                    RevertChanges();
                    return;
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.55f);
            switch (CopySDKTemplate(avatar.name + "_TPose.controller", "vrc_AvatarV3UtilityTPose"))
            {
                case 1:
                    EditorUtility.DisplayDialog("AV3 Overrides", "Cancelled.", "Close");
                    RevertChanges();
                    return;
                case 3:
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more files!", "Close");
                    RevertChanges();
                    return;
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.6f);
            switch (CopySDKTemplate(avatar.name + "_IKPose.controller", "vrc_AvatarV3UtilityIKPose"))
            {
                case 1:
                    EditorUtility.DisplayDialog("AV3 Overrides", "Cancelled.", "Close");
                    RevertChanges();
                    return;
                case 3:
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more files!", "Close");
                    RevertChanges();
                    return;
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.6125f);
            switch (CopySDKTree(avatar.name + "_vrc_CrouchingLocomotion.asset", "vrc_CrouchingLocomotion"))
            {
                case 1:
                    EditorUtility.DisplayDialog("AV3 Overrides", "Cancelled.", "Close");
                    RevertChanges();
                    return;
                case 3:
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more files!", "Close");
                    RevertChanges();
                    return;
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.625f);
            switch (CopySDKTree(avatar.name + "_vrc_ProneLocomotion.asset", "vrc_ProneLocomotion"))
            {
                case 1:
                    EditorUtility.DisplayDialog("AV3 Overrides", "Cancelled.", "Close");
                    RevertChanges();
                    return;
                case 3:
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more files!", "Close");
                    RevertChanges();
                    return;
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.6375f);
            switch (CopySDKTree(avatar.name + "_vrc_StandingLocomotion.asset", "vrc_StandingLocomotion"))
            {
                case 1:
                    EditorUtility.DisplayDialog("AV3 Overrides", "Cancelled.", "Close");
                    RevertChanges();
                    return;
                case 3:
                    EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more files!", "Close");
                    RevertChanges();
                    return;
            }
            EditorUtility.DisplayProgressBar("AV3 Overrides", "Copying Templates", 0.65f);

            AnimatorController baseAnimator = null;
            AnimatorController additiveAnimator = null;
            AnimatorController gestureAnimator = null;
            AnimatorController actionAnimator = null;
            AnimatorController fxAnimator = null;
            AnimatorController sittingAnimator = null;
            AnimatorController tposeAnimator = null;
            AnimatorController ikposeAnimator = null;

            List<BlendTree> trees = new List<BlendTree>();

            AssetDatabase.SaveAssets();

            //find them
            string[] results = AssetDatabase.FindAssets(avatar.name, new string[] { outputPath + Path.DirectorySeparatorChar + "Animators", outputPath + Path.DirectorySeparatorChar + "BlendTrees" });

            foreach (string guid in results)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);

                if (path.Contains(avatar.name + "_Base.controller"))
                {
                    baseAnimator = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));
                }
                else if (path.Contains(avatar.name + "_Additive.controller"))
                {
                    additiveAnimator = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));
                }
                else if (path.Contains(avatar.name + "_Gesture.controller"))
                {
                    gestureAnimator = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));
                }
                else if (path.Contains(avatar.name + "_Action.controller"))
                {
                    actionAnimator = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));
                }
                else if (path.Contains(avatar.name + "_FX.controller"))
                {
                    fxAnimator = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));
                }
                else if (path.Contains(avatar.name + "_Sitting.controller"))
                {
                    sittingAnimator = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));
                }
                else if (path.Contains(avatar.name + "_TPose.controller"))
                {
                    tposeAnimator = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));
                }
                else if (path.Contains(avatar.name + "_IKPose.controller"))
                {
                    ikposeAnimator = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));
                }
                else if (path.Contains(avatar.name + "_vrc_CrouchingLocomotion.asset") || path.Contains(avatar.name + "_vrc_ProneLocomotion.asset") || path.Contains(avatar.name + "_vrc_StandingLocomotion.asset"))
                {
                    trees.Add((BlendTree)AssetDatabase.LoadAssetAtPath(path, typeof(BlendTree)));
                }
            }

            if (baseAnimator == null || additiveAnimator == null || gestureAnimator == null || actionAnimator == null || fxAnimator == null || sittingAnimator == null || tposeAnimator == null || ikposeAnimator == null)
            {
                EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more Animators.", "Close");
                RevertChanges();
                return;
            }
            else if (trees.Count != 3)
            {
                EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to create one or more BlendTrees.", "Close");
                RevertChanges();
                return;
            }

            //Add extra layers to FX

            if (templateFX != null)
            {
                AddLayersParameters(fxAnimator, templateFX);
            }
            else
            {
                return;
            }

            //Replace Animations

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Applying Override", 0.7f);
            foreach (BlendTree tree in trees)
            {
                string treeName = tree.name.Substring(tree.name.LastIndexOf("vrc"));
                //Debug.Log(treeName);
                foreach (AnimatorControllerLayer layer in baseAnimator.layers)
                {
                    if (!ReplaceMotion(baseAnimator, layer.name, (BlendTree)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(treeName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "BlendTrees" })[0]), typeof(BlendTree)), tree))
                    {
                        EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + treeName + " in Base.", "Close");
                        RevertChanges();
                        return;
                    }
                }
                foreach (AnimatorControllerLayer layer in additiveAnimator.layers)
                {
                    if (!ReplaceMotion(additiveAnimator, layer.name, (BlendTree)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(treeName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "BlendTrees" })[0]), typeof(BlendTree)), tree))
                    {
                        EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + treeName + " in Additive.", "Close");
                        RevertChanges();
                        return;
                    }
                }
                foreach (AnimatorControllerLayer layer in gestureAnimator.layers)
                {
                    if (!ReplaceMotion(gestureAnimator, layer.name, (BlendTree)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(treeName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "BlendTrees" })[0]), typeof(BlendTree)), tree))
                    {
                        EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + treeName + " in Gesture.", "Close");
                        RevertChanges();
                        return;
                    }
                }
                foreach (AnimatorControllerLayer layer in actionAnimator.layers)
                {
                    if (!ReplaceMotion(actionAnimator, layer.name, (BlendTree)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(treeName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "BlendTrees" })[0]), typeof(BlendTree)), tree))
                    {
                        EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + treeName + " in Action.", "Close");
                        RevertChanges();
                        return;
                    }
                }
                foreach (AnimatorControllerLayer layer in fxAnimator.layers)
                {
                    if (!ReplaceMotion(fxAnimator, layer.name, (BlendTree)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(treeName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "BlendTrees" })[0]), typeof(BlendTree)), tree))
                    {
                        EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + treeName + " in FX.", "Close");
                        RevertChanges();
                        return;
                    }
                }
                foreach (AnimatorControllerLayer layer in sittingAnimator.layers)
                {
                    if (!ReplaceMotion(sittingAnimator, layer.name, (BlendTree)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(treeName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "BlendTrees" })[0]), typeof(BlendTree)), tree))
                    {
                        EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + treeName + " in Sitting.", "Close");
                        RevertChanges();
                        return;
                    }
                }
                foreach (AnimatorControllerLayer layer in tposeAnimator.layers)
                {
                    if (!ReplaceMotion(tposeAnimator, layer.name, (BlendTree)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(treeName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "BlendTrees" })[0]), typeof(BlendTree)), tree))
                    {
                        EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + treeName + " in TPose.", "Close");
                        RevertChanges();
                        return;
                    }
                }
                foreach (AnimatorControllerLayer layer in ikposeAnimator.layers)
                {
                    if (!ReplaceMotion(ikposeAnimator, layer.name, (BlendTree)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(treeName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "BlendTrees" })[0]), typeof(BlendTree)), tree))
                    {
                        EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + treeName + " in IKPose.", "Close");
                        RevertChanges();
                        return;
                    }
                }
            }

            foreach (KeyValuePair<AnimationClip, AnimationClip> pair in clips)
            {
                string animName = "proxy_" + pair.Key.name.ToLower();

                if (pair.Value != null)
                {
                    foreach (AnimatorControllerLayer layer in baseAnimator.layers)
                    {
                        if (!ReplaceMotion(baseAnimator, layer.name, (AnimationClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(animName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "ProxyAnim", "Assets" + Path.DirectorySeparatorChar + "AV3 Overrides" + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animations" + Path.DirectorySeparatorChar + "ProxyAnim" })[0]), typeof(AnimationClip)), pair.Value))
                        {
                            EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + animName + " in Base.", "Close");
                            RevertChanges();
                            return;
                        }
                    }
                    foreach (AnimatorControllerLayer layer in additiveAnimator.layers)
                    {
                        if (!ReplaceMotion(additiveAnimator, layer.name, (AnimationClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(animName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "ProxyAnim", "Assets" + Path.DirectorySeparatorChar + "AV3 Overrides" + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animations" + Path.DirectorySeparatorChar + "ProxyAnim" })[0]), typeof(AnimationClip)), pair.Value))
                        {
                            EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + animName + " in Additive.", "Close");
                            RevertChanges();
                            return;
                        }
                    }
                    foreach (AnimatorControllerLayer layer in gestureAnimator.layers)
                    {
                        if (!ReplaceMotion(gestureAnimator, layer.name, (AnimationClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(animName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "ProxyAnim", "Assets" + Path.DirectorySeparatorChar + "AV3 Overrides" + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animations" + Path.DirectorySeparatorChar + "ProxyAnim" })[0]), typeof(AnimationClip)), pair.Value))
                        {
                            EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + animName + " in Gesture.", "Close");
                            RevertChanges();
                            return;
                        }
                    }
                    foreach (AnimatorControllerLayer layer in actionAnimator.layers)
                    {
                        if (!ReplaceMotion(actionAnimator, layer.name, (AnimationClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(animName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "ProxyAnim", "Assets" + Path.DirectorySeparatorChar + "AV3 Overrides" + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animations" + Path.DirectorySeparatorChar + "ProxyAnim" })[0]), typeof(AnimationClip)), pair.Value))
                        {
                            EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + animName + " in Action.", "Close");
                            RevertChanges();
                            return;
                        }
                    }
                    foreach (AnimatorControllerLayer layer in fxAnimator.layers)
                    {
                        if (!ReplaceMotion(fxAnimator, layer.name, (AnimationClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(animName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "ProxyAnim", "Assets" + Path.DirectorySeparatorChar + "AV3 Overrides" + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animations" + Path.DirectorySeparatorChar + "ProxyAnim" })[0]), typeof(AnimationClip)), pair.Value))
                        {
                            EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + animName + " in FX.", "Close");
                            RevertChanges();
                            return;
                        }
                    }
                    foreach (AnimatorControllerLayer layer in sittingAnimator.layers)
                    {
                        if (!ReplaceMotion(sittingAnimator, layer.name, (AnimationClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(animName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "ProxyAnim", "Assets" + Path.DirectorySeparatorChar + "AV3 Overrides" + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animations" + Path.DirectorySeparatorChar + "ProxyAnim" })[0]), typeof(AnimationClip)), pair.Value))
                        {
                            EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + animName + " in Sitting.", "Close");
                            RevertChanges();
                            return;
                        }
                    }
                    foreach (AnimatorControllerLayer layer in tposeAnimator.layers)
                    {
                        if (!ReplaceMotion(tposeAnimator, layer.name, (AnimationClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(animName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "ProxyAnim", "Assets" + Path.DirectorySeparatorChar + "AV3 Overrides" + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animations" + Path.DirectorySeparatorChar + "ProxyAnim" })[0]), typeof(AnimationClip)), pair.Value))
                        {
                            EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + animName + " in TPose.", "Close");
                            RevertChanges();
                            return;
                        }
                    }
                    foreach (AnimatorControllerLayer layer in ikposeAnimator.layers)
                    {
                        if (!ReplaceMotion(ikposeAnimator, layer.name, (AnimationClip)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(animName, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "ProxyAnim", "Assets" + Path.DirectorySeparatorChar + "AV3 Overrides" + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animations" + Path.DirectorySeparatorChar + "ProxyAnim" })[0]), typeof(AnimationClip)), pair.Value))
                        {
                            EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: Failed to replace " + animName + " in IKPose.", "Close");
                            RevertChanges();
                            return;
                        }
                    }
                }
            }            

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Saving Assets", 0.8f);

            baseAnimator.SaveController();
            additiveAnimator.SaveController();
            gestureAnimator.SaveController();
            actionAnimator.SaveController();
            fxAnimator.SaveController();
            sittingAnimator.SaveController();
            tposeAnimator.SaveController();
            ikposeAnimator.SaveController();

            AssetDatabase.SaveAssets();

            //Place in descriptor

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Modifying Avatar Descriptor", 0.9f);
            if (avatar.customizeAnimationLayers == false)
            {
                avatar.customizeAnimationLayers = true;
            }

            if (replaceAnimators || avatar.baseAnimationLayers[0].animatorController == null)
            {
                avatar.baseAnimationLayers[0].isEnabled = true;
                avatar.baseAnimationLayers[0].isDefault = false;
                avatar.baseAnimationLayers[0].animatorController = baseAnimator;
            }

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Modifying Avatar Descriptor", 0.915f);
            if (replaceAnimators || avatar.baseAnimationLayers[1].animatorController == null)
            {
                avatar.baseAnimationLayers[1].isEnabled = true;
                avatar.baseAnimationLayers[1].isDefault = false;
                avatar.baseAnimationLayers[1].animatorController = additiveAnimator;
            }

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Modifying Avatar Descriptor", 0.93f);
            if (replaceAnimators || avatar.baseAnimationLayers[2].animatorController == null)
            {
                avatar.baseAnimationLayers[2].isEnabled = true;
                avatar.baseAnimationLayers[2].isDefault = false;
                avatar.baseAnimationLayers[2].animatorController = gestureAnimator;
            }

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Modifying Avatar Descriptor", 0.945f);
            if (replaceAnimators || avatar.baseAnimationLayers[3].animatorController == null)
            {
                avatar.baseAnimationLayers[3].isEnabled = true;
                avatar.baseAnimationLayers[3].isDefault = false;
                avatar.baseAnimationLayers[3].animatorController = actionAnimator;
            }

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Modifying Avatar Descriptor", 0.96f);
            if (replaceAnimators || avatar.baseAnimationLayers[4].animatorController == null)
            {
                avatar.baseAnimationLayers[4].isEnabled = true;
                avatar.baseAnimationLayers[4].isDefault = false;
                avatar.baseAnimationLayers[4].animatorController = fxAnimator;
            }

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Modifying Avatar Descriptor", 0.975f);
            if (replaceAnimators || avatar.specialAnimationLayers[0].animatorController == null)
            {
                avatar.specialAnimationLayers[0].isEnabled = true;
                avatar.specialAnimationLayers[0].isDefault = false;
                avatar.specialAnimationLayers[0].animatorController = sittingAnimator;
            }

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Modifying Avatar Descriptor", 0.99f);
            if (replaceAnimators || avatar.specialAnimationLayers[1].animatorController == null)
            {
                avatar.specialAnimationLayers[1].isEnabled = true;
                avatar.specialAnimationLayers[1].isDefault = false;
                avatar.specialAnimationLayers[1].animatorController = tposeAnimator;
            }

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Modifying Avatar Descriptor", 1f);
            if (replaceAnimators || avatar.specialAnimationLayers[2].animatorController == null)
            {
                avatar.specialAnimationLayers[2].isEnabled = true;
                avatar.specialAnimationLayers[2].isDefault = false;
                avatar.specialAnimationLayers[2].animatorController = ikposeAnimator;
            }

            AssetDatabase.SaveAssets();

            EditorUtility.DisplayProgressBar("AV3 Overrides", "Finished", 1f);

            EditorUtility.DisplayDialog("AV3 Overrides", "Success!", "Close");
            Selection.activeObject = avatar;
            return;
        }
        catch (Exception err)
        {
            EditorUtility.DisplayDialog("AV3 Overrides", "ERROR: An exception has occured!\nCheck the console for more details.", "Close");
            Debug.LogError(err);
            RevertChanges();
            return;
        }      
    }

    private readonly List<KeyValuePair<string, string>> animTypes = new List<KeyValuePair<string, string>> { 
        new KeyValuePair<string, string>("AFK", "Action"),
        new KeyValuePair<string, string>("BACKFLIP", "Action"),
        new KeyValuePair<string, string>("CROUCH_STILL", "Base"),
        new KeyValuePair<string, string>("CROUCH_WALK_FORWARD", "Base"),
        new KeyValuePair<string, string>("CROUCH_WALK_RIGHT", "Base"),
        new KeyValuePair<string, string>("CROUCH_WALK_RIGHT_135", "Base"),
        new KeyValuePair<string, string>("CROUCH_WALK_RIGHT_45", "Base"),
        new KeyValuePair<string, string>("DANCE", "Action"),
        new KeyValuePair<string, string>("DIE", "Action"),
        new KeyValuePair<string, string>("EYES_DIE", "FX"),
        new KeyValuePair<string, string>("EYES_OPEN", "FX"),
        new KeyValuePair<string, string>("EYES_SHUT", "FX"),
        new KeyValuePair<string, string>("FACE_FIST", "FX"),
        new KeyValuePair<string, string>("FACE_GUN", "FX"),
        new KeyValuePair<string, string>("FACE_IDLE", "FX"),
        new KeyValuePair<string, string>("FACE_OPEN", "FX"),
        new KeyValuePair<string, string>("FACE_PEACE", "FX"),
        new KeyValuePair<string, string>("FACE_POINT", "FX"),
        new KeyValuePair<string, string>("FACE_ROCK", "FX"),
        new KeyValuePair<string, string>("FACE_THUMBS_UP", "FX"),
        new KeyValuePair<string, string>("FALL_LONG", "Base"),
        new KeyValuePair<string, string>("FALL_SHORT", "Base"),
        new KeyValuePair<string, string>("HANDS_FIST", "Gesture"),
        new KeyValuePair<string, string>("HANDS_GUN", "Gesture"),
        new KeyValuePair<string, string>("HANDS_IDLE", "Gesture"),
        new KeyValuePair<string, string>("HANDS_OPEN", "Gesture"),
        new KeyValuePair<string, string>("HANDS_PEACE", "Gesture"),
        new KeyValuePair<string, string>("HANDS_POINT", "Gesture"),
        new KeyValuePair<string, string>("HANDS_ROCK", "Gesture"),
        new KeyValuePair<string, string>("HANDS_THUMBS_UP", "Gesture"),
        new KeyValuePair<string, string>("IDLE", "Additive"),
        new KeyValuePair<string, string>("IKPOSE", "IKPose"),
        new KeyValuePair<string, string>("LANDING", "Base"),
        new KeyValuePair<string, string>("LOW_CRAWL_FORWARD", "Base"),
        new KeyValuePair<string, string>("LOW_CRAWL_RIGHT", "Base"),
        new KeyValuePair<string, string>("LOW_CRAWL_STILL", "Base"),
        new KeyValuePair<string, string>("MOOD_ANGRY", "FX"),
        new KeyValuePair<string, string>("MOOD_HAPPY", "FX"),
        new KeyValuePair<string, string>("MOOD_NEUTRAL", "FX"),
        new KeyValuePair<string, string>("MOOD_SAD", "FX"),
        new KeyValuePair<string, string>("MOOD_SURPRISED", "FX"),
        new KeyValuePair<string, string>("RUN_BACKWARD", "Base"),
        new KeyValuePair<string, string>("RUN_FORWARD", "Base"),
        new KeyValuePair<string, string>("RUN_STRAFE_RIGHT", "Base"),
        new KeyValuePair<string, string>("RUN_STRAFE_RIGHT_135", "Base"),
        new KeyValuePair<string, string>("RUN_STRAFE_RIGHT_45", "Base"),
        new KeyValuePair<string, string>("SEATED_CLAP", "Action"),
        new KeyValuePair<string, string>("SEATED_DISAPPROVE", "Action"),
        new KeyValuePair<string, string>("SEATED_DISBELIEF", "Action"),
        new KeyValuePair<string, string>("SEATED_DRUM", "Action"),
        new KeyValuePair<string, string>("SEATED_LAUGH", "Action"),
        new KeyValuePair<string, string>("SEATED_POINT", "Action"),
        new KeyValuePair<string, string>("SEATED_RAISE_HAND", "Action"),
        new KeyValuePair<string, string>("SEATED_SHAKE_FIST", "Action"),
        new KeyValuePair<string, string>("SIT", "Sitting"),
        new KeyValuePair<string, string>("SPRINT_FORWARD", "Base"),
        new KeyValuePair<string, string>("STAND_CHEER", "Action"),
        new KeyValuePair<string, string>("STAND_CLAP", "Action"),
        new KeyValuePair<string, string>("STAND_POINT", "Action"),
        new KeyValuePair<string, string>("STAND_SADKICK", "Action"),
        new KeyValuePair<string, string>("STAND_STILL", "Action"),
        new KeyValuePair<string, string>("STAND_WAVE", "Action"),
        new KeyValuePair<string, string>("STRAFE_RIGHT", "Base"),
        new KeyValuePair<string, string>("STRAFE_RIGHT_135", "Base"),
        new KeyValuePair<string, string>("STRAFE_RIGHT_45", "Base"),
        new KeyValuePair<string, string>("SUPINE_GETUP", "Base"),
        new KeyValuePair<string, string>("TPOSE", "TPose"),
        new KeyValuePair<string, string>("WALK_BACKWARD", "Base"),
        new KeyValuePair<string, string>("WALK_FORWARD", "Base"),
    };

    private bool CheckCompatibility(KeyValuePair<AnimationClip, AnimationClip> pair, out Type problem, out string name)
    {
        string type = "Null";
        foreach (KeyValuePair<string, string> map in animTypes)
        {
            if (map.Key == pair.Key.name)
            {
                type = map.Value;
            }
        }

        switch(type)
        {
            case "FX":
                if (!CheckCompatibilityHelper(pair.Value, false, out problem, out name))
                {
                    return false;
                }
                break;
            case "Null":
                problem = null;
                name = "I am error plz report to dev.";
                return false;
            default:
                if (!CheckCompatibilityHelper(pair.Value, true, out problem, out name))
                {
                    return false;
                }
                break;
        }

        problem = null;
        return true;
    }

    private bool CheckCompatibilityHelper(AnimationClip clip, bool transformsOnly, out Type problem, out string name)
    {
        if (clip != null)
        {
            foreach (var binding in AnimationUtility.GetCurveBindings(clip))
            {
                if ((transformsOnly && binding.type != typeof(Transform) && binding.type != typeof(Animator)) || (!transformsOnly && (binding.type == typeof(Transform) || binding.type == typeof(Animator))))
                {
                    problem = binding.type;
                    name = binding.propertyName;
                    return false;
                }                   
            }
        }

        problem = null;
        name = "";
        return true;
    }

    private void VerifyDestination()
    {
        if (!AssetDatabase.IsValidFolder(outputPath))
        {
            if (!AssetDatabase.IsValidFolder(relativePath + Path.DirectorySeparatorChar + "Output"))
            {
                string guid = AssetDatabase.CreateFolder(relativePath, "Output");
                outputPath = AssetDatabase.GUIDToAssetPath(guid);
            }
            else
            {
                outputPath = relativePath + Path.DirectorySeparatorChar + "Output";
            }
        }
    }

    private bool ReplaceMotion(AnimatorController source, string layerName, Motion oldMotion, Motion newMotion)
    {
        AnimatorControllerLayer[] layers = source.layers;
        AnimatorControllerLayer selectedLayer = new AnimatorControllerLayer();

        foreach (AnimatorControllerLayer layer in layers)
        {
            if (layer.name == layerName)
            {
                selectedLayer = layer;
                break;
            }
        }
        if (selectedLayer == null)
        {
            return false;
        }

        for (int i = 0; i < selectedLayer.stateMachine.states.Length; i++)
        {
            if (AssetDatabase.GetAssetPath(selectedLayer.stateMachine.states[i].state.motion) == AssetDatabase.GetAssetPath(oldMotion))
            {
                selectedLayer.stateMachine.states[i].state.motion = newMotion;
            }
            else if (selectedLayer.stateMachine.states[i].state.motion != null && selectedLayer.stateMachine.states[i].state.motion.GetType() == typeof(BlendTree))
            {
                ReplaceMotion((BlendTree)selectedLayer.stateMachine.states[i].state.motion, oldMotion, newMotion);
            }
        }

        for (int i = 0; i < layers.Length; i++)
        {
            if (layers[i].name == layerName)
            {
                layers[i] = selectedLayer;
                break;
            }
        }

        source.layers = layers;

        return true;
    }

    private void ReplaceMotion(BlendTree source, Motion oldAnim, Motion newAnim)
    {
        //Debug.Log(source.name + ", " + oldAnim.name + ", " + newAnim.name);
        ChildMotion[] motions = source.children;

        for (int i = 0; i < motions.Length; i++)
        {
            if (AssetDatabase.GetAssetPath(motions[i].motion) == AssetDatabase.GetAssetPath(oldAnim))
            {
                motions[i].motion = newAnim;
            }
        }

        source.children = motions;
    }

    private bool AddLayersParameters(AnimatorController source, AnimatorController target)
    {
        //Clone Target Animator
        if (!AssetDatabase.CopyAsset(AssetDatabase.GetAssetPath(target), relativePath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animators" + Path.DirectorySeparatorChar + "Temporary.controller"))
        {
            return false;
        }
        AssetDatabase.Refresh();

        AnimatorController cloned = null;

        string[] results = AssetDatabase.FindAssets("Temporary", new string[] { relativePath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animators" });

        foreach (string guid in results)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            if (path.Contains("Temporary.controller"))
            {
                cloned = (AnimatorController)AssetDatabase.LoadAssetAtPath(path, typeof(AnimatorController));
            }
        }

        if (cloned == null)
        {
            return false;
        }

        //Check and Add Parameters
        AnimatorControllerParameter[] srcParam = source.parameters;
        AnimatorControllerParameter[] tarParam = cloned.parameters;

        foreach (AnimatorControllerParameter param in tarParam)
        {
            bool exists = false;
            for (int i = 0; i < srcParam.Length; i++)
            {
                if (param.name == srcParam[i].name)
                {
                    if (param.type == srcParam[i].type)
                    {
                        srcParam[i] = param;
                        exists = true;
                        break;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            if (!exists)
            {
                AnimatorControllerParameter[] temp = new AnimatorControllerParameter[srcParam.Length + 1];
                srcParam.CopyTo(temp, 0);
                temp[temp.Length - 1] = param;
                srcParam = temp;
            }
        }

        source.parameters = srcParam;

        //Check and Add/Replace Layers
        AnimatorControllerLayer[] srcLayers = source.layers;
        AnimatorControllerLayer[] tarLayers = cloned.layers;

        foreach (AnimatorControllerLayer layer in tarLayers)
        {
            for (int i = 0; i < srcLayers.Length; i++)
            {
                if (layer.name == srcLayers[i].name)
                {
                    AssetDatabase.RemoveObjectFromAsset(srcLayers[i].stateMachine);
                    source.RemoveLayer(i);
                    break;
                }
            }
            source.AddLayer(layer.name);
            srcLayers = source.layers;
            srcLayers[srcLayers.Length - 1] = layer.DeepClone();
            source.layers = srcLayers;
        }

        source.SaveController();

        //Delete Clone
        if (!AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(cloned)))
        {
            return false;
        }
        AssetDatabase.Refresh();

        return true;
    }

    private int CopySDKTree(string outFile, string SDKfile)
    {
        if (!AssetDatabase.IsValidFolder(outputPath + Path.DirectorySeparatorChar + "BlendTrees"))
            AssetDatabase.CreateFolder(outputPath, "BlendTrees");
        bool existed = true;
        if (File.Exists(outputPath + Path.DirectorySeparatorChar + "BlendTrees" + Path.DirectorySeparatorChar + outFile))
        {
            if (!autoOverwrite)
            {
                switch (EditorUtility.DisplayDialogComplex("AV3 Overrides", outFile + " already exists!\nOverwrite the file?", "Overwrite", "Cancel", "Skip"))
                {
                    case 1:
                        return 1;
                    case 2:
                        return 2;
                }
            }
            backupManager.AddToBackup(new Asset(outputPath + Path.DirectorySeparatorChar + "BlendTrees" + Path.DirectorySeparatorChar + outFile));
        }
        else
        {
            existed = false;
        }
        if (!AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(SDKfile, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "BlendTrees" })[0]), outputPath + Path.DirectorySeparatorChar + "BlendTrees" + Path.DirectorySeparatorChar + outFile))
        {
            return 3;
        }
        else
        {
            AssetDatabase.Refresh();
            if (!existed)
                generated.Add(new Asset(outputPath + Path.DirectorySeparatorChar + "BlendTrees" + Path.DirectorySeparatorChar + outFile));
        }
        return 0;
    }

    private int CopySDKTemplate(string outFile, string SDKfile)
    {
        if (!AssetDatabase.IsValidFolder(outputPath + Path.DirectorySeparatorChar + "Animators"))
            AssetDatabase.CreateFolder(outputPath, "Animators");
        bool existed = true;
        if (File.Exists(outputPath + Path.DirectorySeparatorChar + "Animators" + Path.DirectorySeparatorChar + outFile))
        {
            if (!autoOverwrite)
            {
                switch (EditorUtility.DisplayDialogComplex("AV3 Overrides", outFile + " already exists!\nOverwrite the file?", "Overwrite", "Cancel", "Skip"))
                {
                    case 1:
                        return 1;
                    case 2:
                        return 2;
                }
            }
            backupManager.AddToBackup(new Asset(outputPath + Path.DirectorySeparatorChar + "Animators" + Path.DirectorySeparatorChar + outFile));
        }
        else
        {
            existed = false;
        }
        if (!AssetDatabase.CopyAsset(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets(SDKfile, new string[] { "Assets" + Path.DirectorySeparatorChar + "VRCSDK" + Path.DirectorySeparatorChar + "Examples3" + Path.DirectorySeparatorChar + "Animation" + Path.DirectorySeparatorChar + "Controllers" })[0]), outputPath + Path.DirectorySeparatorChar + "Animators" + Path.DirectorySeparatorChar + outFile))
        {
            return 3;
        }
        else
        {
            AssetDatabase.Refresh();
            if (!existed)
                generated.Add(new Asset(outputPath + Path.DirectorySeparatorChar + "Animators" + Path.DirectorySeparatorChar + outFile));
        }
        return 0;
    }

    private void RevertChanges()
    {
        AssetDatabase.SaveAssets();
        if (backupManager != null && !backupManager.RestoreAssets())
            Debug.LogError("[AV3 Overrides] Failed to revert all changes.");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        for (int i = 0; i < generated.ToArray().Length; i++)
            if (!AssetDatabase.DeleteAsset(generated[i].path))
                Debug.LogError("[AV3 Overrides] Failed to revert all changes.");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        if (AssetDatabase.IsValidFolder(outputPath + Path.DirectorySeparatorChar + "Animators") && AssetDatabase.FindAssets("", new string[] { outputPath + Path.DirectorySeparatorChar + "Animators" }).Length == 0)
            if (!AssetDatabase.DeleteAsset(outputPath + Path.DirectorySeparatorChar + "Animators"))
                Debug.LogError("[AV3 Overrides] Failed to revert all changes.");
        if (AssetDatabase.IsValidFolder(outputPath + Path.DirectorySeparatorChar + "BlendTrees") && AssetDatabase.FindAssets("", new string[] { outputPath + Path.DirectorySeparatorChar + "BlendTrees" }).Length == 0)
            if (!AssetDatabase.DeleteAsset(outputPath + Path.DirectorySeparatorChar + "BlendTrees"))
                Debug.LogError("[AV3 Overrides] Failed to revert all changes.");
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }

    private void UpdatePaths()
    {
        string old = relativePath;

        // Get the relative path.
        string[] guids = AssetDatabase.FindAssets(GetType().ToString());
        foreach (string guid in guids)
        {
            string tempPath = AssetDatabase.GUIDToAssetPath(guid);
            if (tempPath.LastIndexOf(typeof(AV3OverridesManager).ToString()) == tempPath.Length - typeof(AV3OverridesManager).ToString().Length - 3)
            {
                relativePath = tempPath.Substring(0, tempPath.LastIndexOf("Editor") - 1);
                break;
            }
        }

        if (relativePath == old)
        {
            // The path was blank.
            if (outputPath == "")
                outputPath = "Assets";
            return;
        }
        else if (outputPath == null || !AssetDatabase.IsValidFolder(outputPath))
        {
            outputPath = relativePath + Path.DirectorySeparatorChar + "Output";
        }
    }

    public bool FindTemplates()
    {
        UpdatePaths();
        dummy = (AssetDatabase.FindAssets("AV3OverridesDummyController", new string[] { relativePath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Dummy" }).Length != 0) ? (AnimatorController)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("AV3OverridesDummyController", new string[] { relativePath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Dummy" })[0]), typeof(AnimatorController)) : null;
        templateFX = (AssetDatabase.FindAssets("FX (AV3Overrides)", new string[] { relativePath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animators" }).Length != 0) ? (AnimatorController)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("FX (AV3Overrides)", new string[] { relativePath + Path.DirectorySeparatorChar + "Templates" + Path.DirectorySeparatorChar + "Animators" })[0]), typeof(AnimatorController)) : null;
        if (dummy == null || templateFX == null)
        {
            return false;
        }
        return true;
    }

    private class NetworkManager : MonoBehaviour { }

    public static void CheckForUpdates()
    {
        string[] guids = AssetDatabase.FindAssets(typeof(AV3OverridesManager).ToString());
        string relativePath = "";
        foreach (string guid in guids)
        {
            string tempPath = AssetDatabase.GUIDToAssetPath(guid);
            if (tempPath.LastIndexOf(typeof(AV3OverridesManager).ToString()) == tempPath.Length - typeof(AV3OverridesManager).ToString().Length - 3)
            {
                relativePath = tempPath.Substring(0, tempPath.LastIndexOf("Editor") - 1);
                break;
            }
        }
        if (relativePath == "")
        {
            EditorUtility.DisplayDialog("AV3 Overrides", "Failed to identify installed version.\n(VERSION file was not found.)", "Close");
            return;
        }        

        string installedVersion = (AssetDatabase.FindAssets("VERSION", new string[] { relativePath }).Length > 0) ? File.ReadAllText(AssetDatabase.GUIDToAssetPath(AssetDatabase.FindAssets("VERSION", new string[] { relativePath })[0])) : "";

        GameObject netMan = new GameObject { hideFlags = HideFlags.HideInHierarchy };
        netMan.AddComponent<NetworkManager>().StartCoroutine(GetText("https://raw.githubusercontent.com/Joshuarox100/VRC-AV3-Overrides/master/Editor/VERSION", latestVersion => {
            if (latestVersion == "")
            {
                EditorUtility.DisplayDialog("AV3 Overrides", "Failed to fetch the latest version.\n(Check console for details.)", "Close");
            }
            else if (installedVersion == "")
            {
                EditorUtility.DisplayDialog("AV3 Overrides", "Failed to identify installed version.\n(VERSION file was not found.)", "Close");
            }
            else if (latestVersion == "RIP")
            {
                EditorUtility.DisplayDialog("AV3 Overrides", "Project has been put on hold indefinitely.", "Close");
            }
            else if (installedVersion != latestVersion)
            {
                if (EditorUtility.DisplayDialog("AV3 Overrides", "A new update is available! (" + latestVersion + ")\nOpen the Releases page?", "Yes", "No"))
                {
                    Application.OpenURL("https://github.com/Joshuarox100/VRC-AV3-Overrides/releases");
                }
            }
            else
            {
                EditorUtility.DisplayDialog("AV3 Overrides", "You are using the latest version.", "Close");
            }
            DestroyImmediate(netMan);
        }));
    }

    private static IEnumerator GetText(string url, Action<string> result)
    {
        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.LogError(www.error);
            result?.Invoke("");
        }
        else
        {
            result?.Invoke(www.downloadHandler.text);
        }
    }
}
