
using System;

namespace DeviceControlSystem.Devices
{
	public class DeviceProperty<T>
	{
		public T Value;
		public bool IsReadOnly;
		public T EditedValue;
	}
}
