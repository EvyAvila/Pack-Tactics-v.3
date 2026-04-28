using System.Collections.Generic;
using UnityEngine.UIElements;

namespace Kamgam.UIToolkitTextAutoSize.UnityUIInternals
{
    /// <summary>
    /// It uses the UnityEngine.UI Assembly to gain access to the internals of the UI code, see:
    /// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Modules/UIElements/AssemblyInfo.cs
    /// and: https://discussions.unity.com/t/how-to-access-useful-unity-editor-engine-internal-methods/251479/3
    /// This works because the text core is visible to UnityEngine.UI and thus is we reference UnityEngine.UI we also have
    /// access to the internals.
    /// </summary>
    internal static class UnityEngineUIInternalExtensions
    {
        internal static PanelSettings GetPanelSettings(this VisualElement element)
        {
            if (element == null)
                return null;

            if (element.panel is IRuntimePanel runtimePanel)
            {
                return runtimePanel.panelSettings;
            }

            return null;
        }

        /// <summary>
        /// Adds the found ui documents to the results list.
        /// NOTICE: It does NOT clear the list before adding though it creates a list if results is null.
        /// </summary>
        /// <param name="element"></param>
        /// <param name="results"></param>
        /// <returns></returns>
        internal static List<UIDocument> GetUIDocuments(this VisualElement element, List<UIDocument> results = null)
        {
            if (element == null)
                return results;

            if (results == null)
                results = new List<UIDocument>();

            if (element.panel is IRuntimePanel runtimePanel)
            {
                if (runtimePanel.panelSettings == null
                    || runtimePanel.panelSettings.m_AttachedUIDocumentsList == null
                    || runtimePanel.panelSettings.m_AttachedUIDocumentsList.m_AttachedUIDocuments == null)
                {
                    return results;
                }

                results.AddRange(runtimePanel.panelSettings.m_AttachedUIDocumentsList.m_AttachedUIDocuments);
            }

            return results;
        }
    }
}
