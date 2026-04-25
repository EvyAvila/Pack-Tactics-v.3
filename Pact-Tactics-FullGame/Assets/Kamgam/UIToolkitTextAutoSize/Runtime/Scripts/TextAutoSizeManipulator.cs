using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kamgam.UIToolkitTextAutoSize
{
    public class TextAutoSizeManipulator : Manipulator
    {
        public static string CLASSNAME = "text-auto-size";
        public static string CLASSNAME_MIN_SIZE_PREFIX = "text-auto-size-min-";
        public static string CLASSNAME_MAX_SIZE_PREFIX = "text-auto-size-max-";

        public static float DefaultMinSize = 4f;
        public static float DefaultMaxSize = 100f;
        
        [System.NonSerialized]
        public static List<TextAutoSizeManipulator> Manipulators = new List<TextAutoSizeManipulator>();
        
        public static void CreateOrUpdateInHierarchy(VisualElement root)
        {
            if (root == null)
                return;
            
            var elements = root.Query(className: CLASSNAME).Build();
            foreach (var element in elements)
            {
                CreateOrUpdate(element);
            }
        }
        
        public static TextAutoSizeManipulator CreateOrUpdate(VisualElement element)
        {
            if (element == null || element.panel == null)
                return null;

            if (element is not TextElement)
                return null;
            
            var manipulator = TextAutoSizeManipulator.GetManipulator(element);
            
            // Add new manipulator if needed if class exists.
            if (manipulator == null && element.ClassListContains(CLASSNAME))
            {
                manipulator = new TextAutoSizeManipulator();
                manipulator.target = element;
            }
            // Remove manipulator if class is missing.
            else if (manipulator != null && !element.ClassListContains(CLASSNAME))
            {
                manipulator.target = null;
                manipulator = null;
            }

            return manipulator;
        }

        public static TextAutoSizeManipulator GetManipulator(VisualElement element)
        {
            foreach (var manipulator in Manipulators)
            {
                if (manipulator != null && manipulator.target == element)
                    return manipulator;
            }
            
            return null;
        }
        
        public static void RemoveManipulator(VisualElement element)
        {
            foreach (var manipulator in Manipulators)
            {
                if (manipulator != null && manipulator.target == element)
                {
                    manipulator.target = null;
                    return;
                }
            }
        }

        public static void RemoveAllManipulators()
        {
            for (int i = Manipulators.Count-1; i >= 0; i--)
            {
                var manipulator = Manipulators[i];
                if (manipulator != null && manipulator.target != null)
                {
                    manipulator.target.RemoveManipulator(manipulator);
                }
            }
            
            Manipulators.Clear();
        }


        public TextElement TextTarget;
        public float MinSize = DefaultMinSize;
        public float MaxSize = DefaultMaxSize;
        
        protected override void RegisterCallbacksOnTarget()
        {
            Manipulators.Add(this);
            
            TextTarget = target as TextElement;
            TextTarget.RegisterCallback<GeometryChangedEvent>(onGeometryChanged);
            TextTarget.RegisterCallback<ChangeEvent<string>>(onTextChanged);
            UpdateMinMaxFromClassNames();
            UpdateFontSize();
        }
        
        protected override void UnregisterCallbacksFromTarget()
        {
            Manipulators.Remove(this);
            TextTarget.UnregisterCallback<GeometryChangedEvent>(onGeometryChanged);
            TextTarget.UnregisterCallback<ChangeEvent<string>>(onTextChanged);
            
            // Make sure no font size style is lingering
            TextTarget.style.fontSize = StyleKeyword.Null;
        }

        public void OnClassNamesChanged()
        {
            UpdateMinMaxFromClassNames();
            UpdateFontSize();
        }

        public void UpdateMinMaxFromClassNames()
        {
            MinSize = DefaultMinSize;
            MaxSize = DefaultMaxSize;
            
            foreach (var className in TextTarget.GetClasses())
            {
                if (className.StartsWith(CLASSNAME_MIN_SIZE_PREFIX))
                {
                    // Trim prefix nd interpret any _ or - as commas.
                    var numberStr = className.Replace(CLASSNAME_MIN_SIZE_PREFIX, "0").Replace("-", ".").Replace("_", ".");
                    if (float.TryParse(numberStr, NumberStyles.Float, CultureInfo.InvariantCulture, out float number))
                    {
                        MinSize = number;
                    }
                }
                if (className.StartsWith(CLASSNAME_MAX_SIZE_PREFIX))
                {
                    var numberStr = className.Replace(CLASSNAME_MAX_SIZE_PREFIX, "0").Replace("-", ".").Replace("_", ".");
                    if (float.TryParse(numberStr, NumberStyles.Float, CultureInfo.InvariantCulture, out float number))
                    {
                        MaxSize = Mathf.Max(MinSize + 0.01f, number);
                    }
                }
            }
        }
        
        public static string EncodeFloatAsClassName(string prefix, float number)
        {
            if (number % 1 == 0)
            {
                return prefix + ((int)number).ToString(CultureInfo.InvariantCulture);
            }
            else
            {
                return prefix + (number.ToString("F2", CultureInfo.InvariantCulture)
                                       .Replace('.', '-'));
            }
        }

        private void onGeometryChanged(GeometryChangedEvent evt)
        {
            UpdateFontSize();
        }
        
        private static readonly System.Action NoOp = () => { };
        
        private void onTextChanged(ChangeEvent<string> evt)
        {
            _lastUpdateDimensions = Vector3.zero;
            
            // Wait for layout to return a valid size or wait for text layouting after text change.
            TextTarget.schedule.Execute(NoOp).Until(() =>
            {
                if (!float.IsNaN(TextTarget.resolvedStyle.width))
                {
                    UpdateFontSize();
                    return true;
                }
        
                return false;
            });
        }

        protected Vector3 _lastUpdateDimensions;

        public void UpdateFontSize()
        {
            // n2h: Check why sometimes the result is float.NaN in the inspector. Probably due to some timing / layouting issues.
            
            if (float.IsNaN(TextTarget.resolvedStyle.fontSize) || float.IsNaN(TextTarget.resolvedStyle.width))
                return;

            TextTarget.UnregisterCallback<GeometryChangedEvent>(onGeometryChanged);

            try
            {
                // Calculate the available space by taking padding into account.
                var availableHeight = TextTarget.resolvedStyle.height
                                      - TextTarget.resolvedStyle.paddingTop
                                      - TextTarget.resolvedStyle.paddingBottom;

                // Calculate the available space by taking padding into account.
                var availableWidth = TextTarget.resolvedStyle.width
                                     - TextTarget.resolvedStyle.paddingLeft
                                     - TextTarget.resolvedStyle.paddingRight;
                
                bool isWrapping = TextTarget.resolvedStyle.whiteSpace == WhiteSpace.Normal;
                Vector2 size;
                if (isWrapping)
                {
                    // With wrapping then the height is the limiting factor.
                    size = TextTarget.MeasureTextSize(TextTarget.text,
                        availableWidth, VisualElement.MeasureMode.AtMost,
                        0, VisualElement.MeasureMode.Undefined);
                }
                else
                {
                    // If no wrap is enabled then there are no limiting factors.
                    size = TextTarget.MeasureTextSize(TextTarget.text,
                        0, VisualElement.MeasureMode.Undefined,
                        0, VisualElement.MeasureMode.Undefined);
                }


                // Reduce flickering for wrapped text (due to resoledStyle measurements changing +/- 1 px).
                var current = new Vector3(availableWidth, availableHeight, TextTarget.resolvedStyle.fontSize);
                if (isWrapping && Vector3.Distance(_lastUpdateDimensions, current) < 2f)
                    return;

                // Calc ratios for new size.
                float ratioWidth = availableWidth / size.x;
                float ratioHeight = availableHeight / size.y;
                float ratio = Mathf.Min(ratioWidth, ratioHeight);

                var fontSize = TextTarget.resolvedStyle.fontSize;
                var scaledFontSize = fontSize * ratio;

                // Apply new size.
                if(!float.IsNaN(scaledFontSize))
                    TextTarget.style.fontSize = scaledFontSize;
                
                // If the textfield is wrapping then iterate until the delta is small enough.
                if (isWrapping)
                {
                    // First iteration is percentage based on the gap.
                    size = TextTarget.MeasureTextSize(TextTarget.text,
                        availableWidth, VisualElement.MeasureMode.AtMost,
                        0, VisualElement.MeasureMode.Undefined);

                    int maxIterations = 50;
                    int iterations = 0;
                    float delta = Mathf.Max((availableHeight - size.y) * 0.05f, 0.6f);
                    float lastFontSize = scaledFontSize;
                    while (availableHeight > size.y && iterations < maxIterations)
                    {
                        iterations++;
                        lastFontSize = scaledFontSize;
                        scaledFontSize += delta;
                        if (!float.IsNaN(scaledFontSize))
                            TextTarget.style.fontSize = scaledFontSize;

                        size = TextTarget.MeasureTextSize(TextTarget.text,
                            availableWidth, VisualElement.MeasureMode.AtMost,
                            0, VisualElement.MeasureMode.Undefined);
                    }

                    if (iterations > 0)
                    {
                        if (!float.IsNaN(lastFontSize))
                        {
                            TextTarget.style.fontSize = lastFontSize;
                            scaledFontSize = lastFontSize;
                        }
                    }

                    // Second iteration is for fine tuning (smaller step size).
                    iterations = 0;
                    size = TextTarget.MeasureTextSize(TextTarget.text,
                        availableWidth, VisualElement.MeasureMode.AtMost,
                       0, VisualElement.MeasureMode.Undefined);

                    while (availableHeight > size.y && iterations < maxIterations)
                    {
                       iterations++;
                       lastFontSize = scaledFontSize;
                       scaledFontSize += 0.5f; 
                       if (!float.IsNaN(scaledFontSize))
                           TextTarget.style.fontSize = scaledFontSize;

                       size = TextTarget.MeasureTextSize(TextTarget.text,
                           availableWidth, VisualElement.MeasureMode.AtMost,
                           0, VisualElement.MeasureMode.Undefined);
                    }

                    if (iterations > 0)
                    {
                        if (!float.IsNaN(lastFontSize))
                            TextTarget.style.fontSize = lastFontSize;
                    }
                }
                
                // Clamp font size to min ma
                float clampedFontSize = Mathf.Clamp(TextTarget.style.fontSize.value.value, MinSize, MaxSize);
                if (!float.IsNaN(clampedFontSize))
                    TextTarget.style.fontSize = clampedFontSize;

                size = TextTarget.MeasureTextSize(TextTarget.text,
                    availableWidth, VisualElement.MeasureMode.AtMost,
                    0, VisualElement.MeasureMode.Undefined);

                _lastUpdateDimensions = new Vector3(availableWidth, availableHeight, TextTarget.style.fontSize.value.value);

            }
            finally
            {
            
                TextTarget.RegisterCallback<GeometryChangedEvent>(onGeometryChanged);
            }
        }

    }
}