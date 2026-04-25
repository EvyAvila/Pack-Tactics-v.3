// (c) KAMGAM e.U.
// Published under the Unit Asset Store Tools License.
// https://unity.com/legal/as-terms

#if UNITY_EDITOR
using System.Collections.Generic;
using System.Globalization;
using System.Net.Mime;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kamgam.UIToolkitTextAutoSize
{
    public static class UIBuilderWindowInspector
    {
        private static TextElement _target;

        public static void Init()
        {
            UIBuilderWindowWrapper.Instance.OnSelectionChanged -= onSelectionChanged;
            UIBuilderWindowWrapper.Instance.OnSelectionChanged += onSelectionChanged;
            
            UIBuilderWindowWrapper.Instance.OnClassNameAdded -= onClassNameAdded;
            UIBuilderWindowWrapper.Instance.OnClassNameAdded += onClassNameAdded;
            
            UIBuilderWindowWrapper.Instance.OnClassNameRemoved -= onClassNameRemoved;
            UIBuilderWindowWrapper.Instance.OnClassNameRemoved += onClassNameRemoved;
            
            UIBuilderWindowWrapper.Instance.OnPlayerWindowUIUpdated -= onPlayerWindowUIUpdated;
            UIBuilderWindowWrapper.Instance.OnPlayerWindowUIUpdated += onPlayerWindowUIUpdated;
            
            AssemblyReloadEvents.beforeAssemblyReload -= onBeforeAssemblyReload;
            AssemblyReloadEvents.beforeAssemblyReload += onBeforeAssemblyReload;

            AssemblyReloadEvents.afterAssemblyReload -= onAfterAssemblyReload;
            AssemblyReloadEvents.afterAssemblyReload += onAfterAssemblyReload;

            EditorApplication.playModeStateChanged -= onPlayModeStateChanged;
            EditorApplication.playModeStateChanged += onPlayModeStateChanged;
            
            TextAutoSizeManipulator.CreateOrUpdateInHierarchy(UIBuilderWindowWrapper.Instance.DocumentRootElement);
        }

        private static void onBeforeAssemblyReload()
        {
            TextAutoSizeManipulator.RemoveAllManipulators();
        }
        
        private static void onAfterAssemblyReload()
        {
            TextAutoSizeManipulator.RemoveAllManipulators();
            TextAutoSizeManipulator.CreateOrUpdateInHierarchy(UIBuilderWindowWrapper.Instance.DocumentRootElement);

            foreach (var manipulator in TextAutoSizeManipulator.Manipulators)
            {
                manipulator.UpdateFontSize();
            }
        }

        private static void onClassNameAdded(string className)
        {
            if (className == TextAutoSizeManipulator.CLASSNAME)
            {
                TextAutoSizeManipulator.CreateOrUpdate(_target);
                rebuildUI(UIBuilderWindowWrapper.Instance.Selection);
                return;
            }

            if (className.StartsWith(TextAutoSizeManipulator.CLASSNAME))
            {
                foreach (var m in TextAutoSizeManipulator.Manipulators)
                {
                    m.OnClassNamesChanged();
                }
            }
        }
        
        private static void onClassNameRemoved(string className)
        {
            if (className == TextAutoSizeManipulator.CLASSNAME)
            {
                TextAutoSizeManipulator.CreateOrUpdate(_target);
                rebuildUI(UIBuilderWindowWrapper.Instance.Selection);
                return;
            }
            
            if (className.StartsWith(TextAutoSizeManipulator.CLASSNAME))
            {
                foreach (var manipulator in TextAutoSizeManipulator.Manipulators)
                {
                    manipulator.OnClassNamesChanged();
                }
            }

            if (className == TextAutoSizeManipulator.CLASSNAME)
                rebuildUI(UIBuilderWindowWrapper.Instance.Selection);
        }

        private static void onSelectionChanged(List<VisualElement> elements)
        {
            if (UIBuilderWindowWrapper.TimeSinceLastCompilation < 1f)
            {
                // After compilation we have to wait for the UI to be rebuilt.
                // A logic based approach would be better but this is good enough for now.
                EditorScheduler.Schedule(0.1f, () => rebuildUI(elements));
            }
            else
            {
                rebuildUI(elements);
            }
        }

        private static void onPlayerWindowUIUpdated(IPanel panel)
        {
            TextAutoSizeManipulator.CreateOrUpdateInHierarchy(panel.visualTree);
            
            // Update font size
            foreach (var manipulator in TextAutoSizeManipulator.Manipulators)
            {
                manipulator.UpdateFontSize();
            }
        }
        
        private static void rebuildUI(List<VisualElement> elements)
        {
            EditorApplication.playModeStateChanged -= onPlayModeStateChanged;
            EditorApplication.playModeStateChanged += onPlayModeStateChanged;
            
            // Skip if multi select
            if (elements.Count > 1)
                return;

            _target = null;
            if (elements.Count > 0)
                _target = elements[0] as TextElement;
            
            var builder = UIBuilderWindowWrapper.Instance;
            if (builder == null)
                return;

            if (builder.RootVisualElement == null)
                return;

            // Search for insertion point for custom inspector.
            var inspectorScrollView = builder.RootVisualElement.Q<ScrollView>(name: "inspector-scroll-view");
            if (inspectorScrollView == null || inspectorScrollView.contentContainer == null)
                return;

            var inspectorContainer = inspectorScrollView.contentContainer.Q<VisualElement>(name: "text-section");
            if (inspectorContainer == null)
            {
                inspectorContainer = inspectorScrollView;
            }
            // Create Custom inspector
            string customInspectorName = "kamgam-text-auto-size";
            var customInspectorRoot = inspectorScrollView.Q<VisualElement>(customInspectorName);
            // Delete if existing.
            if (customInspectorRoot != null)
            {
                customInspectorRoot.UnregisterCallback<DetachFromPanelEvent>(onDetachFromPanel);
                customInspectorRoot.parent.Remove(customInspectorRoot);
            }
    
            // Abort if it is not a text element.
            if (_target != null)
            {
                var manipulator = TextAutoSizeManipulator.CreateOrUpdate(_target);
                customInspectorRoot = new VisualElement();
                customInspectorRoot.name = customInspectorName;
                createCustomInspectorGUI(customInspectorRoot, manipulator);
                inspectorContainer.Add(customInspectorRoot);
            }
        }
        
        private static readonly System.Action NoOp = () => { };

        private static Toggle _onOffToggle;
        private static FloatField _minSizeField;
        private static FloatField _maxSizeField;

        private static void createCustomInspectorGUI(VisualElement root, TextAutoSizeManipulator manipulator)
        {
            var divider = new VisualElement();
            divider.AddToClassList("unity-builder-inspector__divider");
            root.Add(divider);
            
            _onOffToggle = new Toggle("Text Auto Size");
            _onOffToggle.name = "kamgam-text-auto-size-toggle";
            _onOffToggle.value = manipulator != null;
            _onOffToggle.RegisterValueChangedCallback(onOffToggleChanged);
            _onOffToggle.style.marginLeft = 20;
            root.Add(_onOffToggle);
            
            _minSizeField = new FloatField("Text Auto Size Min:");
            _minSizeField.name = "kamgam-text-auto-size-min";
            //_minSizeField.style.display = onOffToggle.value ? DisplayStyle.Flex : DisplayStyle.None;
            _minSizeField.SetEnabled(_onOffToggle.value);
            _minSizeField.value = manipulator != null ? manipulator.MinSize : TextAutoSizeManipulator.DefaultMinSize;
            _minSizeField.RegisterValueChangedCallback(minSizeChanged);
            _minSizeField.style.marginLeft = 20;
            root.Add(_minSizeField);
            
            _maxSizeField = new FloatField("Text Auto Size Max:");
            _maxSizeField.name = "kamgam-text-auto-size-max";
            //_maxSizeField.style.display = onOffToggle.value ? DisplayStyle.Flex : DisplayStyle.None;
            _maxSizeField.SetEnabled(_onOffToggle.value);
            _maxSizeField.value = manipulator != null ? manipulator.MaxSize : TextAutoSizeManipulator.DefaultMaxSize;
            _maxSizeField.RegisterValueChangedCallback(maxSizeChanged);
            _maxSizeField.style.marginLeft = 20;
            root.Add(_maxSizeField);
        }

        private static void onOffToggleChanged(ChangeEvent<bool> evt)
        {
            if (evt.newValue)
            {
                UIBuilderWindowWrapper.Instance.AddClassNameToSelectedElement(TextAutoSizeManipulator.CLASSNAME, propagateChangeEvent: true);
            }
            else
            {
                UIBuilderWindowWrapper.Instance.RemoveClassNameFromSelectedElement(TextAutoSizeManipulator.CLASSNAME, propagateChangeEvent: true);
                
                // Sadly this doesn't work. Have not yet found out how to trigger an update in the UI Builder.
                // var inspectorScrollView = UIBuilderWindowWrapper.Instance.RootVisualElement.Q<ScrollView>(name: "inspector-scroll-view");
                // var fontSizeStyleRow = inspectorScrollView.contentContainer.Query<BindableElement>(className: "unity-builder-style-row")
                //                            .Where(s => s.bindingPath == "font-size").First();
                //if (fontSizeStyleRow != null)
                //{
                //    var textfield = fontSizeStyleRow.Q<TextField>(className: "unity-base-text-field");
                //    if (textfield != null)
                //    {
                //        textfield.value = (int.Parse(textfield.value) + 1).ToString();
                //        UIBuilderWindowWrapper.TriggerValueChanged(textfield, textfield.value);
                //    }
                //}
            }
            
            _minSizeField.SetEnabled(evt.newValue);
            _maxSizeField.SetEnabled(evt.newValue);
            
        }
        
        private static List<string> _tmpClassNamesList = new List<string>();
        
        private static IVisualElementScheduledItem _scheduledClassMaxUpdate;
        private static float _scheduledNewMaxValue;
        
        private static IVisualElementScheduledItem _scheduledClassMinUpdate;
        private static float _scheduledNewMinValue;
        
        private static void minSizeChanged(ChangeEvent<float> evt)
        {
            if (_target == null)
                return;

            // Update manipulator immediately
            var manipulator = TextAutoSizeManipulator.GetManipulator(_target);
            manipulator.MinSize = evt.newValue;
            manipulator.UpdateFontSize();
            _scheduledNewMinValue = evt.newValue;

            // Delay the heavy operation of persisting the values in classnames.
            if (_scheduledClassMinUpdate == null)
            {
                _scheduledClassMinUpdate = _target.schedule.Execute(() =>
                {
                    // Remove existing size class name.
                    _tmpClassNamesList.Clear();
                    foreach (var className in _target.GetClasses())
                    {
                        if (className.StartsWith(TextAutoSizeManipulator.CLASSNAME_MIN_SIZE_PREFIX))
                        {
                            _tmpClassNamesList.Add(className);
                        }
                    }
                    foreach (var className in _tmpClassNamesList)
                    {
                        UIBuilderWindowWrapper.Instance.RemoveClassNameFromSelectedElement(className, propagateChangeEvent: false);
                    }
                    _tmpClassNamesList.Clear();

                    // Add new
                    string floatClassName =
                        TextAutoSizeManipulator.EncodeFloatAsClassName(
                            TextAutoSizeManipulator.CLASSNAME_MIN_SIZE_PREFIX, _scheduledNewMinValue);
                    UIBuilderWindowWrapper.Instance.AddClassNameToSelectedElement(floatClassName,
                        propagateChangeEvent: true);

                    _scheduledClassMinUpdate = null;
                });
            }
            _scheduledClassMinUpdate.ExecuteLater(50);
        }

        private static void maxSizeChanged(ChangeEvent<float> evt)
        {
            if (_target == null)
                return;

            // Update manipulator immediately
            var manipulator = TextAutoSizeManipulator.GetManipulator(_target);
            manipulator.MaxSize = evt.newValue;
            manipulator.UpdateFontSize();
            _scheduledNewMaxValue = evt.newValue;

            // Delay the heavy operation of persisting the values in classnames.
            if (_scheduledClassMaxUpdate == null)
            {
                _scheduledClassMaxUpdate = _target.schedule.Execute(() =>
                {
                    // Remove existing size class name.
                    _tmpClassNamesList.Clear();
                    foreach (var className in _target.GetClasses())
                    {
                        if (className.StartsWith(TextAutoSizeManipulator.CLASSNAME_MAX_SIZE_PREFIX))
                        {
                            _tmpClassNamesList.Add(className);
                        }
                    }
                    foreach (var className in _tmpClassNamesList)
                    {
                        UIBuilderWindowWrapper.Instance.RemoveClassNameFromSelectedElement(className, propagateChangeEvent: false);
                    }
                    _tmpClassNamesList.Clear();

                    // Add new
                    string floatClassName =
                        TextAutoSizeManipulator.EncodeFloatAsClassName(
                            TextAutoSizeManipulator.CLASSNAME_MAX_SIZE_PREFIX, _scheduledNewMaxValue);
                    UIBuilderWindowWrapper.Instance.AddClassNameToSelectedElement(floatClassName,
                        propagateChangeEvent: true);

                    _scheduledClassMaxUpdate = null;
                });
            }
            _scheduledClassMaxUpdate.ExecuteLater(50);
        }

        private static void onPlayModeStateChanged(PlayModeStateChange obj)
        {
            if (obj == PlayModeStateChange.ExitingPlayMode)
            {
                rebuildUI(UIBuilderWindowWrapper.Instance.Selection);
            }

            if (obj == PlayModeStateChange.EnteredEditMode)
            {
                // Add to player window.
                var playerPanels = UIElementsUtilityProxy.GetAllPanels(null, ContextType.Player);
                foreach (var panel in playerPanels)
                {
                    TextAutoSizeManipulator.CreateOrUpdateInHierarchy(panel.visualTree);
                }
                
                // Update font size
                foreach (var manipulator in TextAutoSizeManipulator.Manipulators)
                {
                    manipulator.UpdateFontSize();
                }
            }
        }

        private static void onDetachFromPanel(DetachFromPanelEvent evt)
        {
            if (UIBuilderWindowWrapper.Instance == null || !UIBuilderWindowWrapper.Instance.HasWindow || UIBuilderWindowWrapper.Instance.RootVisualElement == null)
                return;
            
            // Auto rebuild UI after detach due to inspector rebuild.
            (evt.target as VisualElement)?.UnregisterCallback<DetachFromPanelEvent>(onDetachFromPanel);
            EditorScheduler.Schedule(0.1f, () => rebuildUI(UIBuilderWindowWrapper.Instance.Selection));
        }

        private static bool s_ignoreNextClassAddEvent;

        private static void Submit(VisualElement btn)
        {
            using (var e = new NavigationSubmitEvent() { target = btn })
            {
                btn.SendEvent(e);
            }
        }
    }
}
#endif