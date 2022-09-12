using DeviceControlSystem.Devices;
using DeviceControlSystem;
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
        [SerializeField] private string _screenPickerAreaName;
        [SerializeField] private Transform _screenPickerIndicator;
        [SerializeField] private VisualTreeAsset _editorContainerTemplate;
        [SerializeField] private List<VisualTreeAsset> _editorTemplates;

        private Label _deviceNotSelectedLabel;
        private ScrollView _propertyList;

        private VisualElement _screenPickerArea;
        private DeviceProperty<Vector3> _screenPickerTargetProperty;

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

		private void OnDestroy()
		{
            _screenPickerArea.UnregisterCallback<MouseDownEvent>(OnPositionPicked, TrickleDown.TrickleDown);
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

            _screenPickerArea = _document.rootVisualElement.Q(_screenPickerAreaName);
            _screenPickerArea.RegisterCallback<MouseDownEvent>(OnPositionPicked, TrickleDown.TrickleDown);
            _screenPickerIndicator.gameObject.SetActive(false);

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

                var propertyEditors = device.GetPropertyEditors();

                for(int i = 0; i < propertyEditors.Count; i++)
				{
                    var p = propertyEditors[i];
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
        private void CreatePropertyEditor(DevicePropertyEditor propertyEditor)
		{
            TemplateContainer editor = null;

            // If the editor with given name doesn't exist, log error and exit
            if (_editorTemplatesDictionary.TryGetValue(propertyEditor.EditorName, out var editorTemplate))
			{
                editor = editorTemplate.Instantiate();
            }
            else
			{
                Debug.Log($"Property editor with name {propertyEditor.EditorName} not found");
                return;
			}
            TemplateContainer editorContainer = _editorContainerTemplate.Instantiate();
            Foldout foldout = editorContainer.Q<Foldout>("Container");

            foldout.text = propertyEditor.Description;

            foldout.Add(editor);
            _propertyList.Add(foldout);

            //Setup callbacks
            switch (propertyEditor.EditorName)
			{
                case UIPropertyEditorComponents.Vector3Simple:
                    UIBinder.BindVector3Simple(propertyEditor, editor, _registeredExternalCallbacks);
                    break;
                case UIPropertyEditorComponents.Vector3WithPicker:
                    UIBinder.BindVector3WithPicker(propertyEditor, editor, _registeredExternalCallbacks, OnPositionPickerActivated);
                    break;
                case UIPropertyEditorComponents.BoolButton:
                    UIBinder.BindBoolButton(propertyEditor, editor);
                    break;
                case UIPropertyEditorComponents.StringField:
                    UIBinder.BindStringField(propertyEditor, editor, _registeredExternalCallbacks);
                    break;
                default:
                    break;
			}           
        }

        private void OnPositionPickerActivated(DeviceProperty<Vector3> targetProperty)
		{
            _screenPickerArea.Show();
            _screenPickerTargetProperty = targetProperty;
            _screenPickerIndicator.gameObject.SetActive(true);
		}

        private void OnPositionPicked(MouseDownEvent evt)
        {
            _screenPickerIndicator.gameObject.SetActive(false);
            _screenPickerArea.Hide();
            _screenPickerTargetProperty.SetValue(_screenPickerIndicator.position);          
        }
    }
}
