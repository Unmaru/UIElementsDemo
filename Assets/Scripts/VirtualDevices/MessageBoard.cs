
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DeviceControlSystem.Devices
{
	class MessageBoard : VirtualDevice
	{
		[SerializeField] private Text _messagePrefab;
		[SerializeField] private RectTransform _messageContainer;

		private DeviceProperty<string> _messageToSend;
		private DeviceProperty<bool> _sendButton;

		public override List<DeviceProperty> GetProperties()
		{
			return new List<DeviceProperty>
			{
				_messageToSend,
				_sendButton
			};
		}

		public override void SetupProperties()
		{
			_messageToSend = new DeviceProperty<string>() { Description = "Message:", EditorName = "StringField" };
			_sendButton = new DeviceProperty<bool>() { Description = "Send", EditorName = "BoolButton" };
			_sendButton.OnPropertyChanged += OnSendMessage;
		}

		private void OnSendMessage(DeviceProperty msg)
		{
			Text messageText = Instantiate(_messagePrefab, _messageContainer);
			messageText.text = _messageToSend.EditedValue;
			LayoutRebuilder.ForceRebuildLayoutImmediate(_messageContainer); //Force layout update

			// Clear the message field and update it
			_messageToSend.SetOldAndEdited("");			
		}
	}
}
