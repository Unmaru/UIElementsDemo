using DeviceControlSystem.Devices.SaveData;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeviceControlSystem.Devices
{
    [Serializable]
    public abstract class VirtualDevice : MonoBehaviour
    {
        
        public enum DeviceStatus {Offline, Ready, Busy}

        protected DeviceStatus Status = DeviceStatus.Offline;

        private bool _isInitialized = false;

        public string Id { get; protected set; } = "";

        public abstract void SetupProperties();
        public abstract List<DevicePropertyEditor> GetPropertyEditors();
        public abstract VirtualDeviceSaveData GetDeviceSaveData();
        protected abstract void SetDeviceSaveDataInternal(VirtualDeviceSaveData data);

        private void Start()
		{
            if (!_isInitialized)
            {
                Init(null);
            }
		}
        private void OnDestroy()
        {
            Destroy();
        }

        public void Init(VirtualDeviceSaveData data)
		{
            SetupProperties();

            if (data != null)
            {
                SetDeviceSaveData(data);
            }

            Id = DeviceController.Instance.RegisterDevice(this);

            _isInitialized = true;
        }

        public bool SetDeviceSaveData(VirtualDeviceSaveData data)
		{
            if(data.VirtualDeviceType != this.GetType().Name) //Check if the data type is matching this device
			{
                return false;
			}
			{
                SetDeviceSaveDataInternal(data);
                return true;
			}
		}

        private void Destroy()
		{
            DeviceController.Instance.RemoveDevice(Id);
        }
	}
}
