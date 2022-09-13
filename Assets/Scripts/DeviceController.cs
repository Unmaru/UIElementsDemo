using DeviceControlSystem.Devices;
using DeviceControlSystem.Devices.SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace DeviceControlSystem
{
	// Main class of the device control system
    public class DeviceController : MonoBehaviour
    {
		[SerializeField] private List<GameObject> _devicePrefabs;
		[SerializeField] private Transform _deviceContainer;

        static public DeviceController Instance { get; private set; }
		static public Action OnDeviceControllerInitialized;

		public Action<string, VirtualDevice> OnAddDevice; // Device id, device
		public Action<string> OnRemoveDevice; // Device id
		public Action<string> OnSelectedDeviceChanged; //Device id

		private Dictionary<string, VirtualDevice> _devices = new Dictionary<string, VirtualDevice>();
		private Dictionary<string, GameObject> _devicePrefabsDictionary = new Dictionary<string, GameObject>();

		private void Awake()
		{
			if(Instance == null)
			{
				Instance = this;
			}
			else
			{
				if(Instance != this)
				{
					Destroy(this);
				}
			}

			for(int i = 0; i < _devicePrefabs.Count; i++)
			{
				_devicePrefabsDictionary.Add(_devicePrefabs[i].name, _devicePrefabs[i]);
			}

			OnDeviceControllerInitialized?.Invoke();

			LoadDeviceConfiguration("devices.json");
		}
		public string RegisterDevice(VirtualDevice device)
		{
			string deviceId;
			if (device.Id != "")
			{
				if(_devices.ContainsKey(device.Id)) // If there is already a device with such Id, then create an error message
				{
					Debug.LogError($"Device with Id:{device.Id} is already registered");
					return device.Id;
				}
				// if device with such Id doesn't exist yet, then grant the device its desired Id
				deviceId = device.Id;
			}
			else
			{
				deviceId = GenerateNewDeviceId();
			}
			
			_devices.Add(deviceId, device);
			OnAddDevice?.Invoke(deviceId, device);
			return deviceId;
		}

		public void RemoveDevice(string deviceId)
		{
			OnRemoveDevice(deviceId);
			_devices.Remove(deviceId);
		}

		public VirtualDevice GetDevice(string deviceId)
		{
			return _devices[deviceId];
		}

		public DeviceConfigurationData GetDeviceConfiguration()
		{
			var devicesSaveData = new List<VirtualDeviceSaveData>();

			foreach (var device in _devices)
			{
				devicesSaveData.Add(device.Value.GetDeviceSaveData());
			}

			return new DeviceConfigurationData { Devices = devicesSaveData };
		}

		public void SaveDeviceConfiguration(string fileName)
		{
			string json = JsonUtility.ToJson(GetDeviceConfiguration(), true);

			using(StreamWriter file = new StreamWriter(fileName))
			{
				file.Write(json);
			}
		}

		public void LoadDeviceConfiguration(string fileName)
		{
			string json;
			using (StreamReader file = new StreamReader(fileName))
			{
				json = file.ReadToEnd();
			}

			DeviceConfigurationData devicesConfiguration = JsonUtility.FromJson<DeviceConfigurationData>(json);

			for(int i = 0; i < devicesConfiguration.Devices.Count; i++)
			{
				VirtualDeviceSaveData deviceData = devicesConfiguration.Devices[i];
				if (!_devices.TryGetValue(deviceData.Id, out var virtualDevice)) // if device dict doesn't contain the Id, create new device
				{
					GameObject device = Instantiate(_devicePrefabsDictionary[devicesConfiguration.Devices[i].VirtualDeviceType], _deviceContainer);
					VirtualDevice deviceComponent = device.GetComponent<VirtualDevice>();
					deviceComponent.Init(deviceData);
				}
				else
				{
					virtualDevice.SetDeviceSaveData(deviceData);
				}
			}
		}

		private string GenerateNewDeviceId()
		{
			for(; ;)
			{
				string id = Guid.NewGuid().ToString();
				if (!_devices.ContainsKey(id))
				{
					return id;
				}
			}
		}
	}
}
