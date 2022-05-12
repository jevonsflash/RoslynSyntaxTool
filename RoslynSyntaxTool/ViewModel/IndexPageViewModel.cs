using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ICSharpCode.AvalonEdit.Document;
using ICSharpCode.AvalonEdit.Highlighting;
using RoslynQuoter;
using Workshop.Common;
using Workshop.Control;
using Workshop.Helper;
using Workshop.Model;
using Workshop.Service;

namespace Workshop.ViewModel
{
    public class IndexPageViewModel : ObservableObject
    {

        public IndexPageViewModel()
        {
            CurrentContent = new TextDocument();
            ResponseContent = new TextDocument();
            ContinueCommand = new RelayCommand(ContinueAction);
            CopyToClipboardCommand = new RelayCommand(CopyToClipboardAction);
        }

        private void CopyToClipboardAction()
        {

            if (string.IsNullOrEmpty(ResponseContentText))
            {
                return;
            }
            Clipboard.SetText(ResponseContentText);
        }


        private void ContinueAction()
        {
            var currentContentText = CurrentContentText;

            var task = InvokeHelper.InvokeOnUi(null, () =>
            {
                var settingInfo = LocalDataService.ReadObjectLocal<SettingInfo>();

                var responseText = String.Empty;

                if (string.IsNullOrEmpty(currentContentText))
                {
                    responseText = "Please specify the source text.";
                }
                else if (currentContentText.Length > 2000)
                {
                    responseText = "Only strings shorter than 2000 characters are supported; your input string is " + currentContentText.Length + " characters long.";
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

                        responseText = quoter.QuoteText(currentContentText);
                    }
                    catch (Exception ex)
                    {
                        responseText = ex.ToString();

                    }
                }

                //ResponseContent.Remove(0, ResponseContent.TextLength);
                //MessageBox.Show(responseText);
                return responseText;

            }, (t) =>
            {
                ResponseContent.Text = t;

            });
        }



        public string CurrentContentText => CurrentContent.Text;
        public string ResponseContentText => ResponseContent.Text;

        private TextDocument _currentContent;

        public TextDocument CurrentContent
        {
            get { return _currentContent; }
            set
            {
                _currentContent = value;
                OnPropertyChanged(nameof(CurrentContent));
            }
        }

        private TextDocument _responseContent;

        public TextDocument ResponseContent
        {
            get { return _responseContent; }
            set
            {
                _responseContent = value;
                OnPropertyChanged(nameof(ResponseContent));
            }
        }
        public RelayCommand ContinueCommand { get; set; }
        public RelayCommand CopyToClipboardCommand { get; set; }
    }
}
