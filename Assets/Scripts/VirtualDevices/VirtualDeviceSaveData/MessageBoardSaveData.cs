using System;
using UnityEngine;

namespace DeviceControlSystem.Devices.SaveData
{
	[Serializable]
	public class MessageBoardSaveData : VirtualDeviceSaveData
	{
		public Vector3 CurrentPosition;
		public string[] Messages;
	}
}
