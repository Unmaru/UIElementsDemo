using DeviceControlSystem.Devices;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DeviceControlSystem.Tests
{
    public class QoLExtentions
    {
        [Test]
        public void QoL_Hide_And_Show_UI_Element()
		{
            VisualTreeAsset Vector1Simple = (VisualTreeAsset)EditorGUIUtility.Load("Tests/Vector1Simple.uxml");

            var vectorUI = Vector1Simple.Instantiate();
            Assert.AreEqual(vectorUI.ClassListContains("hidden"), false);
            vectorUI.Hide();
            Assert.AreEqual(vectorUI.ClassListContains("hidden"), true);
            vectorUI.Show();
            Assert.AreEqual(vectorUI.ClassListContains("hidden"), false);
        }
    }
}
