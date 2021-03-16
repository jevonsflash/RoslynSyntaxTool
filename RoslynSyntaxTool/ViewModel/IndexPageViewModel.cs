using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using GalaSoft.MvvmLight.Threading;
using RoslynQuoter;
using Workshop.Common;
using Workshop.Control;
using Workshop.Helper;
using Workshop.Model;
using Workshop.Service;

namespace Workshop.ViewModel
{
    public class IndexPageViewModel : ViewModelBase
    {

        public IndexPageViewModel()
        {
            ContinueCommand = new RelayCommand(ContinueAction);
            CopyToClipboardCommand = new RelayCommand(CopyToClipboardAction);
        }

        private void CopyToClipboardAction()
        {

            if (string.IsNullOrEmpty(ResponseContent))
            {
                return;
            }
            Clipboard.SetText(ResponseContent);
        }


        private void ContinueAction()
        {

            Task.Factory.StartNew(() =>
            {
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    ProgressWindow progressWindow = new ProgressWindow();
                    progressWindow.ShowDialog("获取数据中");

                });
                var settingInfo = LocalDataService.ReadObjectLocal<SettingInfo>();

                var responseText = String.Empty;

                if (string.IsNullOrEmpty(CurrentContent))
                {
                    responseText = "Please specify the source text.";
                }
                else if (CurrentContent.Length > 2000)
                {
                    responseText = "Only strings shorter than 2000 characters are supported; your input string is " + CurrentContent.Length + " characters long.";
                }
                else
                {
                    try
                    {
                        var quoter = new Quoter
                        {
                            OpenParenthesisOnNewLine = settingInfo.OpenCurlyOnNewLine,
                            ClosingParenthesisOnNewLine = settingInfo.CloseCurlyOnNewLine,
                            UseDefaultFormatting = !settingInfo.PreserveOriginalWhitespace,
                            RemoveRedundantModifyingCalls = !settingInfo.KeepRedundantApiCalls,
                            ShortenCodeWithUsingStatic = !settingInfo.AvoidUsingStatic
                        };

                        responseText = quoter.QuoteText(CurrentContent);
                    }
                    catch (Exception ex)
                    {
                        responseText = ex.ToString();

                    }
                }

                ResponseContent = responseText;
                DispatcherHelper.CheckBeginInvokeOnUI(() =>
                {
                    Messenger.Default.Send("", MessengerToken.CLOSEPROGRESS);

                });

            });
        }
        private string _currentContent;

        public string CurrentContent
        {
            get { return _currentContent; }
            set
            {
                _currentContent = value;
                RaisePropertyChanged(nameof(CurrentContent));
            }
        }

        private string _responseContent;

        public string ResponseContent
        {
            get { return _responseContent; }
            set
            {
                _responseContent = value;
                RaisePropertyChanged(nameof(ResponseContent));
            }
        }
        public RelayCommand ContinueCommand { get; set; }
        public RelayCommand CopyToClipboardCommand { get; set; }

    }
}
