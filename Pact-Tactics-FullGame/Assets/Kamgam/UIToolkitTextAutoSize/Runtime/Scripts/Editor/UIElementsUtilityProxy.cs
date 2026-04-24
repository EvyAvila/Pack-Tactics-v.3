// (c) KAMGAM e.U.
// Published under the Unit Asset Store Tools License.
// https://unity.com/legal/as-terms

#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine.UIElements;
using System;
using System.Reflection;
using System.Collections;

namespace Kamgam.UIToolkitTextAutoSize
{
    /// <summary>
    /// A proxy for the internal UIElementsUtility class of Unity.
    /// https://github.com/Unity-Technologies/UnityCsReference/blob/master/Modules/UIElements/Core/UIElementsUtility.cs
    /// </summary>
    public static class UIElementsUtilityProxy
    {
        static bool _typesRequested;
        static Type _utilityType;
        static Type _panelType;

        static Type _panelIteratorKeyValueType;
        static PropertyInfo _panelIteratorValuePropertyInfo;

        static Type getUtilityType()
        {
            if (_utilityType == null && _typesRequested)
                return null;

            if (_utilityType == null)
            {
                _typesRequested = true;

                var iPanelType = typeof(IPanel); // We assume UIElementsUtility is in the same assembly as IPanel.
                _utilityType = iPanelType.Assembly.GetType("UnityEngine.UIElements.UIElementsUtility");
                _panelType = iPanelType.Assembly.GetType("UnityEngine.UIElements.Panel");
            }

            return _utilityType;
        }

        static bool _getPanelsIteratorMethodInfoRequested;
        static MethodInfo _getPanelsIteratorMethodInfo;

        public static List<IPanel> GetAllPanels(List<IPanel> panels, ContextType contextType)
        {
            return getAllPanels(panels, contextType);
        }

        public static List<IPanel> GetAllPanels(List<IPanel> panels)
        {
            return getAllPanels(panels, contextType: null);
        }

        static List<IPanel> getAllPanels(List<IPanel> panels, ContextType? contextType = null)
        {
            if (panels == null)
                panels = new List<IPanel>();
            
            var iterator = getPanelsIterator(); 
            if (iterator != null) 
            {
                // Sadly the type of the iterator is KeyValue<int, Panel> and Panel is NOT a public type.
                // Therefore we have to jump through some hoops to get the actual panel value as an IPanel.
                while (iterator.MoveNext())
                {
                    if (_panelIteratorKeyValueType == null)
                    {
                        _panelIteratorKeyValueType = iterator.Current.GetType();
                        _panelIteratorValuePropertyInfo = _panelIteratorKeyValueType.GetProperty("Value", BindingFlags.Instance | BindingFlags.Public);
                    }

                    if (_panelIteratorValuePropertyInfo != null)
                    {
                        var panel = (IPanel)_panelIteratorValuePropertyInfo.GetValue(iterator.Current);
                        if (panel != null && (!contextType.HasValue || panel.contextType == contextType.Value))
                        {
                            panels.Add(panel);
                        }
                    }
                }
            }

            return panels;
        }

        static IEnumerator getPanelsIterator()
        {
            if (_getPanelsIteratorMethodInfo == null && _getPanelsIteratorMethodInfoRequested)
                return null;

            if (_getPanelsIteratorMethodInfo == null && !_getPanelsIteratorMethodInfoRequested)
            {
                _getPanelsIteratorMethodInfoRequested = true;

                var type = getUtilityType();
                if (type == null)
                    return null;

                _getPanelsIteratorMethodInfo = type.GetMethod("GetPanelsIterator", BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            }

            if (_getPanelsIteratorMethodInfo != null && _panelType != null)
            {
                var enumerator = (IEnumerator) _getPanelsIteratorMethodInfo.Invoke(null, null);
                return enumerator;
            }

            return null;
        }
        
        public const string UI_BUILDER_VIEWPORT_CLASSNAME = "unity-builder-viewport";

        public static bool IsUIBuilderPanel(IPanel panel)
        {
            // We assume it's the builder if the builder-viewport class is present.
            var builderViewport = panel.visualTree.Q(className: UI_BUILDER_VIEWPORT_CLASSNAME);
            return builderViewport != null && panel.contextType == ContextType.Editor;
        }

        public static bool IsChildOfUIBuilderViewport(IPanel panel, VisualElement element)
        {
            var builderViewport = panel.visualTree.Q(className: UI_BUILDER_VIEWPORT_CLASSNAME);
            if (builderViewport == null)
                return false;

            return builderViewport.Contains(element);
        }

        public static bool IsGameViewPanel(IPanel panel)
        {
            return panel.contextType == ContextType.Player;
        }
    }
}

#endif