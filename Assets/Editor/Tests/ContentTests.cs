using NUnit.Framework;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace DeviceControlSystem.Tests
{
    public class ContentTests : MonoBehaviour
    {
        [Test]
        public void Content_All_Property_Editor_UI_Components_Exist()
		{
            // Get required UI components data from UI manager prefab
            GameObject UIManager = (GameObject)AssetDatabase.LoadAssetAtPath("Assets/Prefabs/UIManager.prefab", typeof(GameObject));
            var propEdit = UIManager.GetComponent<PropertiesEditor>();
            // Get required UI components data from PropertiesEditor private list field
            FieldInfo type = typeof(PropertiesEditor).GetField("_editorTemplates", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            List<VisualTreeAsset> assets = (List<VisualTreeAsset>)type.GetValue(propEdit);

            // Copy assets names into separate list in order to prevent data changes to the prefab
            List<string> names = new List<string>();
            for(int i = 0; i < assets.Count; i++)
			{
                names.Add(assets[i].name);
			}

            // Check if the names in the class have a matchin VisualTreeAsset
            var namesFields = typeof(UIPropertyEditorComponents).GetFields();

            foreach (var item in namesFields)
			{
                // Only process string fields
                if(item.FieldType == typeof(string))
				{
                    string assetName = (string)item.GetRawConstantValue();

                    if(!names.Remove(assetName))
					{
                        // if removal has failed, it means that the asset is missing
                        Assert.Fail($"Asset {assetName} is not present in PropertiesEditor");
                    }
				}
                else
				{
                    Assert.Inconclusive("Improper usage of UIPropertyEditorComponents. Read the comments on top of the class for the proper usage instructions");
				}
			}
		}
    }
}
