
// Storage class for all the UI binders, this file contains all the UI binding methods, but for better scalability it may be useful to
// split the class into multiple files, one file per binder group, via making this class partial.
using DeviceControlSystem.Devices;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DeviceControlSystem
{
    public static class UIBinder
    {
        public static void BindVector3WithPicker(
                DeviceProperty property,
                TemplateContainer editor,
                List<Tuple<DeviceProperty, Action<DeviceProperty>>> registeredExternalCallbacks,
                Action<DeviceProperty<Vector3>> onPositionPickerActivated
            )
        {
            BindVector3Simple(property, editor, registeredExternalCallbacks);

            var pickButton = editor.Q<Button>("PickButton");
            pickButton.clickable.clicked += () =>
            {
                onPositionPickerActivated((DeviceProperty<Vector3>)property);
            };
        }
        public static void BindVector3Simple
            (
                DeviceProperty property,
                TemplateContainer editor,
                List<Tuple<DeviceProperty, Action<DeviceProperty>>> registeredExternalCallbacks
            )
        {
            DeviceProperty<Vector3> p = (DeviceProperty<Vector3>)property;
            Vector3 oldValue = p.EditedValue;
            var xFieldContent = editor.Q("FieldX");
            var xField = xFieldContent.Q<TextField>("FloatField");
            xField.value = oldValue.x.ToString();
            xField.RegisterValueChangedCallback((newValue) =>
            {
                if (float.TryParse(newValue.newValue, out oldValue.x))
                {
                    p.SetValue(oldValue);
                }
            });

            var yFieldContent = editor.Q("FieldY");
            var yField = yFieldContent.Q<TextField>("FloatField");
            yField.value = oldValue.y.ToString();
            yField.RegisterValueChangedCallback((newValue) =>
            {
                if (float.TryParse(newValue.newValue, out oldValue.y))
                {
                    p.SetValue(oldValue);
                }
            });

            var zFieldContent = editor.Q("FieldZ");
            var zField = zFieldContent.Q<TextField>("FloatField");
            zField.value = oldValue.z.ToString();
            zField.RegisterValueChangedCallback((newValue) =>
            {
                if (float.TryParse(newValue.newValue, out oldValue.z))
                {
                    p.SetValue(oldValue);
                }
            });

            var xCurrentValue = xFieldContent.Q<Label>("Value");
            var yCurrentValue = yFieldContent.Q<Label>("Value");
            var zCurrentValue = zFieldContent.Q<Label>("Value");

            Action<DeviceProperty> onValueChange = (p) =>
            {
                var changedProperty = (DeviceProperty<Vector3>)p;
                xField.value = changedProperty.EditedValue.x.ToString();
                yField.value = changedProperty.EditedValue.y.ToString();
                zField.value = changedProperty.EditedValue.z.ToString();

                xCurrentValue.text = changedProperty.Value.x.ToString("N5");
                yCurrentValue.text = changedProperty.Value.y.ToString("N5");
                zCurrentValue.text = changedProperty.Value.z.ToString("N5");
            };

            p.OnPropertyChanged += onValueChange;

            registeredExternalCallbacks.Add(new Tuple<DeviceProperty, Action<DeviceProperty>>(p, onValueChange));
        }
        public static void BindBoolButton
            (
                DeviceProperty property,
                TemplateContainer editor
            )
        {
            DeviceProperty<bool> p = (DeviceProperty<bool>)property;

            var button = editor.Q<Button>("Button");

            button.text = p.Description;

            button.clickable.clicked += () =>
            {
                p.SetValue(true);
            };
        }
        public static void BindStringField
            (
                DeviceProperty property,
                TemplateContainer editor,
                List<Tuple<DeviceProperty, Action<DeviceProperty>>> registeredExternalCallbacks
            )
        {
            DeviceProperty<string> p = (DeviceProperty<string>)property;

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
