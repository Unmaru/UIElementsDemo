using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DeviceControlSystem.Devices
{
    public abstract class VirtualDevice : MonoBehaviour
    {
        
        public enum DeviceStatus {Offline, Ready, Busy}

        protected DeviceStatus Status = DeviceStatus.Offline;

        public int Id { get; private set; }

        public abstract void SetupProperties();
        public abstract List<DevicePropertyEditor> GetPropertyEditors();

        private void Start()
		{
            Init();
		}
        private void OnDestroy()
        {
            Destroy();
        }

        private void Init()
		{
            SetupProperties();
            Id = DeviceController.Instance.RegisterDevice(this);           
		}

        private void Destroy()
		{
            DeviceController.Instance.RemoveDevice(Id);
        }

		public string ToJSON() 
        {
            return JsonUtility.ToJson(this);
        }
        public void FromJSON(string json)
		{
            JsonUtility.FromJsonOverwrite(json, this);
		}
	}
}
