
using System;

namespace DeviceControlSystem.Devices.SaveData
{
	[Serializable]
	public abstract class VirtualDeviceSaveData
	{
		public string Id;
		public string VirtualDeviceType;
	}
}
