
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

		public override List<DevicePropertyEditor> GetPropertyEditors()
		{
			// It is possible to achive the same effect with making a custom editor with a field and a button, but for the sake of
			// the demo let's make it like that, to show the capability of the system to create some custom forms out of predefined editors
			return new List<DevicePropertyEditor>
			{
				new DevicePropertyEditor
				{
					Description = "Message:",
					EditorName = UIPropertyEditorComponents.StringField,
					Properties = { _messageToSend }
				},
				new DevicePropertyEditor
				{
					Description = "Send",
					EditorName = UIPropertyEditorComponents.BoolButton,
					Properties = { _sendButton }
				},
			};
		}

		public override void SetupProperties()
		{
			_messageToSend = new DeviceProperty<string>();
			_sendButton = new DeviceProperty<bool>();

			_sendButton.OnPropertyChanged += OnSendMessage;
		}

		private void OnSendMessage(DeviceProperty msg)
		{
			Text messageText = Instantiate(_messagePrefab, _messageContainer);
			messageText.text = _messageToSend.Value;
			LayoutRebuilder.ForceRebuildLayoutImmediate(_messageContainer); //Force layout update

			// Clear the message field and update it
			_messageToSend.SetValue("");			
		}
	}
}
