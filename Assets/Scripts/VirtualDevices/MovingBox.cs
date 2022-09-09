
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DeviceControlSystem.Devices
{
	class MovingBox : VirtualDevice
	{
		private enum ActionState { Idle, Moving};

		[SerializeField] private float _speed;
		[SerializeField] private Vector3 _position;
		private DeviceProperty<Vector3> _targetPosition;

		private ActionState State;

		public override List<DeviceProperty> GetProperties()
		{
			return new List<DeviceProperty>
			{
				_targetPosition
			};
		}

		public override void SetupProperties()
		{
			_targetPosition = new DeviceProperty<Vector3>() { Description = "Target Position", EditorName = "Vector3Simple" };
		}

		private void Move(Vector3 position)
		{
			State = ActionState.Moving; // We can issue a new movement command even if the object is already moving
			_targetPosition.EditedValue = position;
		}

		private void FixedUpdate()
		{
			switch(State)
			{
				case ActionState.Moving:
					Vector3 diff = _targetPosition.EditedValue - this.transform.position;
					if (diff.magnitude < _speed * Time.fixedDeltaTime) //if current leap is further than the distance
					{
						this.transform.position = _targetPosition.EditedValue; //set current position to destination
					}
					else
					{
						this.transform.position += diff.normalized * _speed * Time.fixedDeltaTime; // else just move in direction
					}
					_targetPosition.Value = this.transform.position;
					break;
				default:
					break;
			}
		}
	}
}
