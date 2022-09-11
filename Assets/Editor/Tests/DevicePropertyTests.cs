using System.Collections;
using DeviceControlSystem.Devices;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UIElements;

namespace DeviceControlSystem.Tests
{
    public class DevicePropertyTests
    {
        [Test]
        public void DeviceProperty_Create()
        {
            DeviceProperty<Vector3> testProperty = new DeviceProperty<Vector3>();
        }
        [Test]
        public void DeviceProperty_SimpleChange()
        {
            DeviceProperty<Vector3> testProperty = new DeviceProperty<Vector3>();
            testProperty.Value = new Vector3(1, 1, 1);
            Assert.That(testProperty.Value, Is.EqualTo(new Vector3(1, 1, 1)));
            testProperty.SetValue(new Vector3(2, 2, 2));
            Assert.That(testProperty.Value, Is.EqualTo(new Vector3(2, 2, 2)));
            testProperty.SetValueSilent(new Vector3(3, 3, 3));
            Assert.That(testProperty.Value, Is.EqualTo(new Vector3(3, 3, 3)));
        }
        [Test]
        public void DeviceProperty_ChangeWithCallbacks()
        {
            DeviceProperty<Vector3> testProperty = new DeviceProperty<Vector3>();
            Vector3 newValue = new Vector3(1, 2, 3);
            bool isCallbackCompleted = false;
            testProperty.OnPropertyChanged += (prop) =>
            {
                var vector3Prop = (DeviceProperty<Vector3>)prop;
                Assert.That(vector3Prop.Value, Is.EqualTo(newValue));
                Assert.IsTrue(vector3Prop.IsDirty);
                isCallbackCompleted = true;
            };

            testProperty.Value = newValue;

            if (!isCallbackCompleted)
                Assert.Fail("Callback was not called");
        }
        [Test]
        public void DevicePropertyEditor_Create()
        {
            DeviceProperty<Vector3> vProp = new DeviceProperty<Vector3>();
            DeviceProperty<bool> bProp = new DeviceProperty<bool>();

            DevicePropertyEditor testPropertyEditor = new DevicePropertyEditor
            {
                Description = "Test property editor",
                EditorName = "Vector3Simple",
                Properties =
            {
                vProp,
                bProp
            }
            };
        }
        [Test]
        public void DevicePropertyEditor_PropertyAccess()
        {
            DeviceProperty<Vector3> vProp = new DeviceProperty<Vector3>();
            DeviceProperty<bool> bProp = new DeviceProperty<bool>();

            DevicePropertyEditor testPropertyEditor = new DevicePropertyEditor
            {
                Description = "Test property editor",
                EditorName = "Vector3Simple",
                Properties =
            {
                vProp,
                bProp
            }
            };

            Assert.That(vProp.Value, Is.EqualTo(((DeviceProperty<Vector3>)testPropertyEditor.Properties[0]).Value));
        }
    }
}