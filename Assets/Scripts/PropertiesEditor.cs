using DeviceControlSystem.Devices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DeviceControlSystem
{
    public class PropertiesEditor : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private string _deviceNotSelectedLabelName;
        [SerializeField] private string _propertyListName;
        [SerializeField] private VisualTreeAsset _editorContainerTemplate;
        [SerializeField] private List<VisualTreeAsset> _editorTemplates;

        private Label _deviceNotSelectedLabel;
        private ScrollView _propertyList;
        private Dictionary<string, VisualTreeAsset> _editorTemplatesDictionary = new Dictionary<string, VisualTreeAsset>();
        //Those callbacks are stored in order to be removed when the editor is closed
        private List<Tuple<DeviceProperty, Action<DeviceProperty>>> _registeredExternalCallbacks = new List<Tuple<DeviceProperty, Action<DeviceProperty>>>();
        private void Awake()
        {
            DeviceController.OnDeviceControllerInitialized += Init; // call init each time DeviceController is (re)initialized

            if (DeviceController.Instance != null)
            {
                Init(); //in case we skipped the initialization, init self
            }
        }

        private void Init()
        {
            // Copy list of templates into dictionary for easier access
            for(int i = 0; i < _editorTemplates.Count; i++)
			{
                _editorTemplatesDictionary.Add(_editorTemplates[i].name, _editorTemplates[i]);
			}

            _deviceNotSelectedLabel = _document.rootVisualElement.Q<Label>(_deviceNotSelectedLabelName);
            _propertyList = (ScrollView)_document.rootVisualElement.Q(_propertyListName);
            _deviceNotSelectedLabel.Show();
            _propertyList.Hide();

            DeviceController.Instance.OnSelectedDeviceChanged += OnDeviceSelectionChange;
        }

        private void OnDeviceSelectionChange(int deviceId)
		{
            var device = DeviceController.Instance.GetDevice(deviceId);
            if (device != null)
			{
                _deviceNotSelectedLabel.Hide();
                _propertyList.Show();

                //Remove all callbacks of previous editor
                RemoveExternalCallbacks();
                //Remove all properties of previous editor
                _propertyList.Clear();

                //Add new properties

                var properties = device.GetProperties();

                for(int i = 0; i < properties.Count; i++)
				{
                    var p = properties[i];
                    if (p.EditorName == "") //if the property editor is not specified, then skip property
                        continue;
                    CreatePropertyEditor(p);
                }
            }
            else
			{
                _deviceNotSelectedLabel.Show();
                _propertyList.Hide();
            }
		}
        private void RemoveExternalCallbacks()
		{
            foreach(var i in _registeredExternalCallbacks)
			{
                i.Item1.OnPropertyChanged -= i.Item2;
			}
		}
        private void CreatePropertyEditor(DeviceProperty property)
		{
            TemplateContainer editor = null;

            // If the editor with given name doesn't exist, log error and exit
            if (_editorTemplatesDictionary.TryGetValue(property.EditorName, out var editorTemplate))
			{
                editor = editorTemplate.Instantiate();
            }
            else
			{
                Debug.Log($"Property editor with name {property.EditorName} not found");
                return;
			}
            TemplateContainer editorContainer = _editorContainerTemplate.Instantiate();
            Foldout foldout = editorContainer.Q<Foldout>("Container");

            foldout.text = property.Description;

            foldout.Add(editor);
            _propertyList.Add(foldout);

            //Setup callbacks
            switch (property.EditorName)
			{
                case "Vector3Simple":
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

                    _registeredExternalCallbacks.Add(new Tuple<DeviceProperty, Action<DeviceProperty>>(p, onValueChange));
                    break;
                default:
                    break;
			}           
        }
    }
}
