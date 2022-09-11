
using System;
using UnityEngine;

namespace DeviceControlSystem.Devices
{
	//TODO
	// implement OnProeprtyChanged cooldown and queueing, in order to limit external value update rate (optional)
	public abstract class DeviceProperty
	{
		public bool IsReadOnly;
		public bool IsDirty;
		public Action<DeviceProperty> OnPropertyChanged;
	}
	public class DeviceProperty<T> : DeviceProperty 
	{
		public T Value { get { return _value; } set { SetValue(value); } }

		private T _value;

		public void SetValue(T value)
		{
			if (IsReadOnly)
				return;

			_value = value;
			IsDirty = true;
			OnPropertyChanged?.Invoke(this);
		}

		// Set new value without triggering change events, useful for initialization
		public void SetValueSilent(T value)
		{
			if (IsReadOnly)
				return;

			_value = value;
		}
	}
}
