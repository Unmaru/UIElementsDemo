using System;
using UnityEngine;

namespace DeviceControlSystem.Devices.SaveData
{
	[Serializable]
	public class MovableObjectSaveData : VirtualDeviceSaveData
	{
		public Vector3 CurrentPosition;
		public Vector3 TargetPosition;
	}
}
