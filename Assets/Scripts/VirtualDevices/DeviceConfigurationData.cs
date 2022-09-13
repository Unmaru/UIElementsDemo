
using DeviceControlSystem.Devices.SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeviceControlSystem
{
	[Serializable]
	public class DeviceConfigurationData
	{
		[SerializeReference] public List<VirtualDeviceSaveData> Devices;
	}
}
