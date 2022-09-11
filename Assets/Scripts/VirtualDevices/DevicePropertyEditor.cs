
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeviceControlSystem.Devices
{
	// Class to allow single Property Editor element to have access to multiple properties, which should make the code cleaner
	// Consider replacing List with Dictionary, might have higher usability, but atm seems more like an alternative approach
	public class DevicePropertyEditor
	{
		public string Description;
		public string EditorName;
		public List<DeviceProperty> Properties = new List<DeviceProperty>();
	}
}
