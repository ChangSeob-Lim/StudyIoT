using Caliburn.Micro;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MqttMoniteringWpfApp.ViewModels
{
    public class ErrorPopupViewModel : Conductor<object>
    {
        string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                NotifyOfPropertyChange(() => ErrorMessage);
            }
        }

        public void ConfirmClose()
        {
            TryClose(true);
        }

        public ErrorPopupViewModel(string param)
        {
            var tmp = param.Split('|');
            DisplayName = tmp[0];
            ErrorMessage = tmp[1];
        }
    }
}
