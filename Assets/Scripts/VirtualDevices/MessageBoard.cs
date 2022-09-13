
using DeviceControlSystem.Devices.SaveData;
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

		private List<Text> _messages = new List<Text>();

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

		public override VirtualDeviceSaveData GetDeviceSaveData()
		{
			string[] messages = new string[_messages.Count];
			for(int i = 0; i < _messages.Count; i++)
			{
				messages[i] = _messages[i].text;
			}

			return new MessageBoardSaveData
			{
				Id = this.Id,
				VirtualDeviceType = this.GetType().Name,
				CurrentPosition = this.transform.position,
				Messages = messages
			};
		}

		protected override void SetDeviceSaveDataInternal(VirtualDeviceSaveData deviceData)
		{
			var data = (MessageBoardSaveData)deviceData;

			this.Id = data.Id;

			transform.position = data.CurrentPosition;

			ClearMessages();

			for (int i = 0; i < data.Messages.Length; i++)
			{
				AddMessage(data.Messages[i]);
			}
		}

		public void AddMessage(string message)
		{
			Text messageText = Instantiate(_messagePrefab, _messageContainer);
			messageText.text = message;
			LayoutRebuilder.ForceRebuildLayoutImmediate(_messageContainer); //Force layout update

			_messages.Add(messageText);
		}

		public void ClearMessages()
		{
			for(int i = 0; i < _messages.Count; i++)
			{
				Destroy(_messages[i].gameObject);
			}
			_messages.Clear();
		}

		private void OnSendMessage(DeviceProperty msg)
		{
			AddMessage(_messageToSend.Value);

			// Clear the message field
			_messageToSend.SetValue("");			
		}
	}
}
