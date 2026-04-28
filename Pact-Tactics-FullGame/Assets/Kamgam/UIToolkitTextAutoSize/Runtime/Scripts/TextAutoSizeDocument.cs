using System;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Kamgam.UIToolkitTextAutoSize
{
    [RequireComponent(typeof(UIDocument))]
    public class TextAutoSizeDocument : MonoBehaviour
    {
        public static bool AutoDetectUIDocuments = true;
        
        public static List<TextAutoSizeDocument> Documents = new List<TextAutoSizeDocument>();
        
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        public static void InitializeOnLoadAttribute()
        {
            // Called upon play mode start (even if domain reload is disabled).
            SceneManager.sceneLoaded -= onSceneLoaded;
            SceneManager.sceneLoaded += onSceneLoaded;
            
            Documents.Clear();
        }

        private static void onSceneLoaded(Scene scene, LoadSceneMode mode)
        {
#if UNITY_EDITOR
            if (!EditorApplication.isPlayingOrWillChangePlaymode)
                return;
#endif
            
            // Abort if auto-detect is off.
            if (!AutoDetectUIDocuments)
                return;
            
            AddToAllUIDocumentsInScene(scene);
        }
        
        public static void AddToAllUIDocumentsInAllScenes(bool allowAbort = true)
        {
            // Abort if auto-detect is off.
            if (allowAbort && !AutoDetectUIDocuments)
                return;

            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.IsValid())
                {
                    AddToAllUIDocumentsInScene(scene);
                }
            }
        }

        public static void AddToAllUIDocumentsInScene(Scene scene)
        {
            var roots = scene.GetRootGameObjects();
            foreach (var root in roots)
            {
                var uiDocuments = root.GetComponentsInChildren<UIDocument>(includeInactive: true);
                foreach (var doc in uiDocuments)
                {
                    AddToUIDocument(doc);
                }
            }
        }
        
        public static TextAutoSizeDocument AddToUIDocument(UIDocument uiDocument)
        {
            if (uiDocument == null)
                return null;
            
            var soundEffectDocument = uiDocument.gameObject.GetComponent<TextAutoSizeDocument>();
            if (soundEffectDocument == null)
            {
                soundEffectDocument = uiDocument.gameObject.AddComponent<TextAutoSizeDocument>();
                soundEffectDocument.CreateOrUpdateManipulators();
            }

            return soundEffectDocument;
        }

        protected UIDocument document;

        public UIDocument Document
        {
            get
            {
                if (document == null)
                {
                    document = this.GetComponent<UIDocument>();
                }

                return document;
            }
        }

        

        public void Start()
        {
            Documents.Add(this);
            CreateOrUpdateManipulators();
        }

        public void OnDestroy()
        {
            Documents.Remove(this);
        }
        
        /// <summary>
        /// Calls CreateOrUpdateManipulators() on all known UI Documents.
        /// </summary>
        public static void CreateOrUpdateManipulatorsOnAll()
        {
            foreach (var doc in Documents)
            {
                doc.CreateOrUpdateManipulators();
            }
        }

        /// <summary>
        /// This is called automatically from Start() but you can (should) also call it any time after the visual tree contents have change. 
        /// </summary>
        public void CreateOrUpdateManipulators()
        {
            if (Document == null || Document.rootVisualElement == null)
                return;

            TextAutoSizeManipulator.CreateOrUpdateInHierarchy(document.rootVisualElement);
        }
    }
}