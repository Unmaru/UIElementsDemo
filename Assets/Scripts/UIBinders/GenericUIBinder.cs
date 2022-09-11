using DeviceControlSystem.Devices;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DeviceControlSystem
{
    // Storage class for all the UI binders, this file contains all the UI binding methods, but for better scalability it may be useful to
    // split the class into multiple files, one file per binder group, via making this class partial.
    public static class UIBinder
    {
        public static void BindVector3WithPicker(
                DevicePropertyEditor propertyEditor,
                TemplateContainer editor,
                List<Tuple<DeviceProperty, Action<DeviceProperty>>> registeredExternalCallbacks,
                Action<DeviceProperty<Vector3>> onPositionPickerActivated
            )
        {
            BindVector3Simple(propertyEditor, editor, registeredExternalCallbacks);

            var pickButton = editor.Q<Button>("PickButton");
            pickButton.clickable.clicked += () =>
            {
                onPositionPickerActivated((DeviceProperty<Vector3>)propertyEditor.Properties[0]);
            };
        }
        public static void BindVector3Simple
            (
                DevicePropertyEditor propertyEditor,
                TemplateContainer editor,
                List<Tuple<DeviceProperty, Action<DeviceProperty>>> registeredExternalCallbacks
            )
        {
            DeviceProperty<Vector3> editableProperty = (DeviceProperty<Vector3>)propertyEditor.Properties[0];
            var xFieldContent = editor.Q("FieldX");
            var xField = xFieldContent.Q<TextField>("FloatField");
            xField.value = editableProperty.Value.x.ToString(); // Set x field to the current x value of the property
            xField.RegisterValueChangedCallback((newValue) =>
            {
                Vector3 val = editableProperty.Value;
                if (float.TryParse(newValue.newValue, out val.x))
                {
                    editableProperty.SetValue(val);
                }
            });

            var yFieldContent = editor.Q("FieldY");
            var yField = yFieldContent.Q<TextField>("FloatField");
            yField.value = editableProperty.Value.y.ToString();
            yField.RegisterValueChangedCallback((newValue) =>
            {
                Vector3 val = editableProperty.Value;
                if (float.TryParse(newValue.newValue, out val.y))
                {
                    editableProperty.SetValue(val);
                }
            });

            var zFieldContent = editor.Q("FieldZ");
            var zField = zFieldContent.Q<TextField>("FloatField");
            zField.value = editableProperty.Value.z.ToString();
            zField.RegisterValueChangedCallback((newValue) =>
            {
                Vector3 val = editableProperty.Value;
                if (float.TryParse(newValue.newValue, out val.z))
                {
                    editableProperty.SetValue(val);
                }
            });

            var xCurrentValue = xFieldContent.Q<Label>("Value");
            var yCurrentValue = yFieldContent.Q<Label>("Value");
            var zCurrentValue = zFieldContent.Q<Label>("Value");

            Action<DeviceProperty> onEditableValueChange = (changedProp) =>
            {
                var changedProperty = (DeviceProperty<Vector3>)changedProp;
                xField.value = changedProperty.Value.x.ToString();
                yField.value = changedProperty.Value.y.ToString();
                zField.value = changedProperty.Value.z.ToString();
            };

            Action<DeviceProperty> onCurrentValueChange = (changedProp) =>
            {
                var changedProperty = (DeviceProperty<Vector3>)changedProp;
                xCurrentValue.text = changedProperty.Value.x.ToString("N5");
                yCurrentValue.text = changedProperty.Value.y.ToString("N5");
                zCurrentValue.text = changedProperty.Value.z.ToString("N5");
            };

            DeviceProperty<Vector3> currentValueProperty = (DeviceProperty<Vector3>)propertyEditor.Properties[1];

            editableProperty.OnPropertyChanged += onEditableValueChange;
            currentValueProperty.OnPropertyChanged += onCurrentValueChange;

            registeredExternalCallbacks.Add(new Tuple<DeviceProperty, Action<DeviceProperty>>(currentValueProperty, onCurrentValueChange));
            registeredExternalCallbacks.Add(new Tuple<DeviceProperty, Action<DeviceProperty>>(editableProperty, onEditableValueChange));
        }
        public static void BindBoolButton
            (
                DevicePropertyEditor propertyEditor,
                TemplateContainer editor
            )
        {
            DeviceProperty<bool> p = (DeviceProperty<bool>)propertyEditor.Properties[0];

            var button = editor.Q<Button>("Button");

            button.text = propertyEditor.Description;

            button.clickable.clicked += () =>
            {
                p.SetValue(true);
            };
        }
        public static void BindStringField
            (
                DevicePropertyEditor propertyEditor,
                TemplateContainer editor,
                List<Tuple<DeviceProperty, Action<DeviceProperty>>> registeredExternalCallbacks
            )
        {
            DeviceProperty<string> p = (DeviceProperty<string>)propertyEditor.Properties[0];

            var textField = editor.Q<TextField>("TextField");
            textField.RegisterValueChangedCallback((newValue) =>
            {
                p.SetValueSilent(textField.value);
            });

            Action<DeviceProperty> onValueChange = (p) =>
            {
                var changedProperty = (DeviceProperty<string>)p;
                textField.value = changedProperty.Value;
            };

            p.OnPropertyChanged += onValueChange;

            registeredExternalCallbacks.Add(new Tuple<DeviceProperty, Action<DeviceProperty>>(p, onValueChange));
        }
    }
}
