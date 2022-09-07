
using System;
using UnityEngine;

namespace DeviceControlSystem.Devices
{
	class MovingBox : VirtualDevice
	{
		private enum ActionState { Idle, Moving};

		[SerializeField] private float _speed;
		[SerializeField] public DeviceProperty<Vector3> Position { get; private set; }

		private ActionState State;

		public override void ExecuteCommand(string name, object args)
		{
			if (Status == DeviceStatus.Busy || Status == DeviceStatus.Offline)
				return;

			switch(name)
			{
				case "move":
					Move((Vector3)args);
					break;
				default:
					Debug.LogError("Unknown command");
					break;
			}
		}

		private void Move(Vector3 position)
		{
			State = ActionState.Moving; // We can issue a new movement command even if the object is already moving
			Position.EditedValue = position;
		}

		private void FixedUpdate()
		{
			switch(State)
			{
				case ActionState.Moving:
					Vector3 diff = Position.EditedValue - Position.Value;
					if (diff.magnitude < _speed * Time.fixedDeltaTime) //if current leap is further than the distance
					{
						Position.Value = Position.EditedValue;
					}
					break;
				default:
					break;
			}
		}
	}
}
