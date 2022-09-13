using DeviceControlSystem.Devices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace DeviceControlSystem
{
    public class DeviceListViewer : MonoBehaviour
    {
        [SerializeField] private UIDocument _document;
        [SerializeField] private VisualTreeAsset _deviceEntryTemplate;
        [SerializeField] private string _listName;
        [SerializeField] private string _labelName;

        private List<string> _displayedDevicesIds = new List<string>();
        private ListView _listView;

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
            var dc = DeviceController.Instance;

            dc.OnAddDevice += OnAddDevice;
            dc.OnRemoveDevice += OnRemoveDevice;

            //Bind device list view

            Func<VisualElement> makeItem = () => _deviceEntryTemplate.Instantiate();

            Action<VisualElement, int> bindItem = (e, i) => e.Q<Label>(_labelName).text = dc.GetDevice(_displayedDevicesIds[i]).name;

            _listView = _document.rootVisualElement.Q<ListView>(_listName);
            _listView.makeItem = makeItem;
            _listView.bindItem = bindItem;
            _listView.itemsSource = _displayedDevicesIds;

            _listView.onSelectionChange += objects => //objects contains a list with ints representing VirtualDevice Id if any selected
            {
                if (objects != null)
                {
                    var l = (List<object>)objects;
                    if (l.Count > 0)
                    {
                        dc.OnSelectedDeviceChanged?.Invoke((string)l[0]);
                    }
                }
            };

            // Bind device buttons
            var saveButton = _document.rootVisualElement.Q<Button>("SaveButton");
            saveButton.clickable.clicked += OnSaveDevices;

            var refreshButton = _document.rootVisualElement.Q<Button>("RefreshButton");
            refreshButton.clickable.clicked += OnRefreshDevices;

            var editButton = _document.rootVisualElement.Q<Button>("EditButton");
            editButton.clickable.clicked += OnEditDevices;

        }

        private void OnSaveDevices()
		{
            DeviceController.Instance.SaveDeviceConfiguration("devices.json");
		}

        private void OnRefreshDevices()
		{
            DeviceController.Instance.LoadDeviceConfiguration("devices.json");

        }

        private void OnEditDevices()
		{
            System.Diagnostics.Process.Start("devices.json");
        }

        private void OnAddDevice(string id, VirtualDevice device)
		{
            _displayedDevicesIds.Add(id);
            _listView.Rebuild();
        }

        private void OnRemoveDevice(string id)
		{
            _displayedDevicesIds.Remove(id);
            _listView.Rebuild();
        }


    }
}
