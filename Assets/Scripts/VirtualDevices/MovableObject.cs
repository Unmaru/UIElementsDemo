
using DeviceControlSystem.Devices.SaveData;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeviceControlSystem.Devices
{
	class MovableObject : VirtualDevice
	{
		private enum ActionState { Idle, Moving};

		[SerializeField] private float _speed;
		private DeviceProperty<Vector3> _targetPosition;
		private DeviceProperty<Vector3> _currentPosition;

		private ActionState State;

		public override List<DevicePropertyEditor> GetPropertyEditors()
		{
			return new List<DevicePropertyEditor>
			{
				new DevicePropertyEditor
				{
					Description = "Target Position",
					EditorName = UIPropertyEditorComponents.Vector3WithPicker,
					Properties = { _targetPosition, _currentPosition }
				}
			};
		}

		public override void SetupProperties()
		{
			_targetPosition = new DeviceProperty<Vector3>();
			_currentPosition = new DeviceProperty<Vector3>();

			_targetPosition.OnPropertyChanged += OnTargetPositionChanged;
		}

		public override VirtualDeviceSaveData GetDeviceSaveData()
		{
			return new MovableObjectSaveData
			{
				Id = this.Id,
				VirtualDeviceType = this.GetType().Name,
				CurrentPosition = _currentPosition.Value,
				TargetPosition = _targetPosition.Value
			};
		}

		protected override void SetDeviceSaveDataInternal(VirtualDeviceSaveData deviceData)
		{
			var data = (MovableObjectSaveData)deviceData;

			this.Id = data.Id;
			_currentPosition.Value = data.CurrentPosition;
			_targetPosition.Value = data.TargetPosition;

			transform.position = data.CurrentPosition;
		}

		private void OnTargetPositionChanged(DeviceProperty newPos)
		{
			Move(_targetPosition.Value);
		}

		private void Move(Vector3 position)
		{
			State = ActionState.Moving; // We can issue a new movement command even if the object is already moving
		}

		private void FixedUpdate()
		{
			switch(State)
			{
				case ActionState.Moving:
					Vector3 diff = _targetPosition.Value - this.transform.position;
					if (diff.magnitude < _speed * Time.fixedDeltaTime) //if current leap is further than the distance
					{
						this.transform.position = _targetPosition.Value; //set current position to destination
						_targetPosition.IsDirty = false;
						State = ActionState.Idle;
					}
					else
					{
						this.transform.position += diff.normalized * _speed * Time.fixedDeltaTime; // else just move in direction
					}
					_currentPosition.SetValue(this.transform.position);
					break;
				default:
					break;
			}
		}
	}
}
