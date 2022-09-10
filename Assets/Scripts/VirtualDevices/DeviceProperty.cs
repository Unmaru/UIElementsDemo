
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
		public T Value { get { return _value; } set { SetOldValue(value); } }
		public T EditedValue { get { return _editedValue; } set { SetValue(value); } }

		private T _value;
		private T _editedValue;

		public void SetValue(T value)
		{
			_editedValue = value;
			IsDirty = true;
			OnPropertyChanged?.Invoke(this);
		}

		public void SetOldValue(T value)
		{
			_value = value;
			OnPropertyChanged?.Invoke(this);
		}

		public void SetOldAndEdited(T value)
		{
			_value = value;
			_editedValue = value;
			IsDirty = false;
			OnPropertyChanged?.Invoke(this);
		}

		// Method group to change values without triggering on change events
		public void SetValueSilent(T value)
		{
			_editedValue = value;
		}

		public void SetOldValueSilent(T value)
		{
			_value = value;
		}
	}
}
