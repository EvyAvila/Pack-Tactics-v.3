// (c) KAMGAM e.U.
// Published under the Unit Asset Store Tools License.
// https://unity.com/legal/as-terms

using System;
using System.IO;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.SceneManagement;
#endif
namespace Kamgam.UIToolkitTextAutoSize
{
    public class Installer
    {
        public const string AssetName = "UI Toolkit Text Auto Size";
        public const string Version = "1.0.0";
        public const string ManualUrl = "https://kamgam.com/unity/UIToolkitTextAutoSizeManual.pdf";
        
        private static string _assetRootPathDefault = "Assets/Kamgam/UIToolkitTextAutoSize/";
        public static string AssetRootPath
        {
            get
            {
#if UNITY_EDITOR
                if (System.IO.File.Exists(_assetRootPathDefault))
                {
                    return _assetRootPathDefault;
                }

                // The the tool was moved then search for the installer script and derive the root
                // path from there. Used Assets/ as ultimate fallback.
                string finalPath = "Assets/";
                string assetRootPathRelative = _assetRootPathDefault.Replace("Assets/", "");
                var installerGUIDS = AssetDatabase.FindAssets("t:Script Installer");
                foreach (var guid in installerGUIDS)
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    if (path.Contains(assetRootPathRelative))
                    {
                        int index = path.IndexOf(assetRootPathRelative);
                        return path.Substring(0, index) + assetRootPathRelative;
                    }
                }

                return finalPath;
#else
                return _assetRootPathDefault;
#endif
            }
        }
        public static string ExamplePath = AssetRootPath + "Examples/UIToolkitTextAutoSizeDemo.unity";

        public static Version GetVersion() => new Version(Version);

#if UNITY_EDITOR
        [UnityEditor.Callbacks.DidReloadScripts(998001)]
        public static void InstallIfNeeded()
        {
            bool versionChanged = VersionHelper.UpgradeVersion(GetVersion, out Version oldVersion, out Version newVersion);
            if (versionChanged)
            {
                if (versionChanged)
                {
                    Debug.Log(AssetName + " version changed from " + oldVersion + " to " + newVersion);
                }
            }
        }

        [MenuItem("Tools/" + AssetName + "/Open Example Scene", priority = 103)]
        public static void OpenExample()
        {
            EditorApplication.delayCall += () => 
            {
                var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(ExamplePath);
                EditorGUIUtility.PingObject(scene);
                EditorSceneManager.OpenScene(ExamplePath);
            };
        }
        
        [MenuItem("Tools/" + AssetName + "/Manual", priority = 101)]
        public static void OpenManual()
        {
            Application.OpenURL(ManualUrl);
        }
        
        [MenuItem("Tools/" + AssetName + "/More Assets by KAMGAM", priority = 511)]
        public static void MoreAssets()
        {
            Application.OpenURL("https://kamgam.com/unity-assets?ref=asset");
        }

        [MenuItem("Tools/" + AssetName + "/Version " + Version, priority = 512)]
        public static void LogVersion()
        {
            Debug.Log(AssetName + " v" + Version + ", Unity: " + Application.unityVersion);
        }
#endif
    }
}