using DeviceControlSystem.Devices;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeviceControlSystem
{
	// Main class of the device control system
    public class DeviceController : MonoBehaviour
    {
        static public DeviceController Instance { get; private set; }
		static public Action OnDeviceControllerInitialized;

		public Action<int, VirtualDevice> OnAddDevice;
		public Action<int> OnRemoveDevice;

		private Dictionary<int, VirtualDevice> _devices = new Dictionary<int, VirtualDevice>();

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
			OnDeviceControllerInitialized?.Invoke();
		}
		public int RegisterDevice(VirtualDevice device)
		{
			_devices.Add(_devices.Count, device);
			OnAddDevice?.Invoke(_devices.Count - 1, device);
			return _devices.Count - 1;
		}

		public void RemoveDevice(int deviceId)
		{
			OnRemoveDevice(deviceId);
			_devices.Remove(deviceId);
		}

		public VirtualDevice GetDevice(int deviceId)
		{
			return _devices[deviceId];
		}
	}
}
