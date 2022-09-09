
using System;
using UnityEngine;

namespace DeviceControlSystem.Devices
{
	//TODO
	// implement OnProeprtyChanged cooldown and queueing, in order to limit external value update rate (optional)
	public abstract class DeviceProperty
	{
		public string Description;
		public bool IsReadOnly;
		public bool IsDirty;
		public string EditorName;
		public Action<DeviceProperty> OnPropertyChanged;
	}
	public class DeviceProperty<T> : DeviceProperty 
	{
		public T Value;
		public T EditedValue;
		public void SetValue(T value)
		{
			EditedValue = value;
			IsDirty = true;
			OnPropertyChanged?.Invoke(this);
		}
	}
}
