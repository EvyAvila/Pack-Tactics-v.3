using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using Kamgam.UIToolkitTextAutoSize.UnityUIInternals;

namespace Kamgam.UIToolkitTextAutoSize.UIDocumentFinder
{
    public static class UIDocumentFinder
    {

        /// <summary>
        /// Finds all UI documents in all loaded scenes (at runtime).
        /// NOTICE: It does not clear the list before adding the results though it creates a results list
        /// if it is null.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="results"></param>
        public static List<UIDocument> GetUIDocuments(VisualElement element, List<UIDocument> results = null)
        {
            return element.GetUIDocuments(results);
        }

        /// <summary>
        /// Finds all UI documents in all loaded scenes (at runtime).
        /// NOTICE: It does not clear the list before adding the results though it creates a results list
        /// if it is null.
        /// </summary>
        /// <param name="results"></param>
        public static List<UIDocument> GetAllUIDocuments(List<UIDocument> results = null)
        {
            if (results == null)
            {
                results = new List<UIDocument>();
            }
            
            for (int i = 0; i < SceneManager.sceneCount; i++)
            {
                var scene = SceneManager.GetSceneAt(i);
                if (scene.IsValid())
                {
                    GetUIDocumentsInScene(scene, results);
                }
            }

            return results;
        }

        /// <summary>
        /// Finds all UI documents in the given scene.
        /// NOTICE: It does not clear the list before adding the results though it creates a results list
        /// if it is null.
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="results"></param>
        public static List<UIDocument> GetUIDocumentsInScene(Scene scene, List<UIDocument> results = null)
        {
            if (results == null)
            {
                results = new List<UIDocument>();
            }
            
            var roots = scene.GetRootGameObjects();
            foreach (var root in roots)
            {
                var uiDocuments = root.GetComponentsInChildren<UIDocument>(includeInactive: true);
                foreach (var doc in uiDocuments)
                {
                    results.Add(doc);
                }
            }

            return results;
        }
    }
}