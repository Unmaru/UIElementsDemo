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

        public abstract void ExecuteCommand(string name, object args);

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
