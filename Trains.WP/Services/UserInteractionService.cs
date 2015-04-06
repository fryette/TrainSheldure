﻿using Chance.MvvmCross.Plugins.UserInteraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Popups;

namespace Trains.WP.Services
{
    public class UserInteractionService : IUserInteraction
    {
        public void Alert(string message, Action done = null, string title = "", string okButton = "OK")
        {
            throw new NotImplementedException();
        }

        public async Task AlertAsync(string message, string title = "", string okButton = "OK")
        {
            await Windows.ApplicationModel.Core.CoreApplication.MainView.CoreWindow.Dispatcher.RunAsync
                (Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
                {
                    var dialog = new MessageDialog(message);
                    await dialog.ShowAsync();
                });
        }

        public void Confirm(string message, Action<bool> answer, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            throw new NotImplementedException();
        }

        public void Confirm(string message, Action okClicked, string title = null, string okButton = "OK", string cancelButton = "Cancel")
        {
            throw new NotImplementedException();
        }

        public Task<bool> ConfirmAsync(string message, string title = "", string okButton = "OK", string cancelButton = "Cancel")
        {
            throw new NotImplementedException();
        }

        public void ConfirmThreeButtons(string message, Action<ConfirmThreeButtonsResponse> answer, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {
            throw new NotImplementedException();
        }

        public Task<ConfirmThreeButtonsResponse> ConfirmThreeButtonsAsync(string message, string title = null, string positive = "Yes", string negative = "No", string neutral = "Maybe")
        {
            throw new NotImplementedException();
        }

        public void Input(string message, Action<bool, string> answer, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            throw new NotImplementedException();
        }

        public void Input(string message, Action<string> okClicked, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            throw new NotImplementedException();
        }

        public Task<InputResponse> InputAsync(string message, string placeholder = null, string title = null, string okButton = "OK", string cancelButton = "Cancel", string initialText = null)
        {
            throw new NotImplementedException();
        }
    }
}