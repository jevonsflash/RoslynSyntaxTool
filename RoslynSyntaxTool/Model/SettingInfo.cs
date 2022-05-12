using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RoslynQuoter;

namespace Workshop.Model
{
    public class SettingInfo : ObservableObject
    {
       


        public NodeKind _nodeKind;

        public NodeKind NodeKind
        {
            get { return _nodeKind; }
            set
            {
                _nodeKind = value;
                OnPropertyChanged(nameof(NodeKind));

            }
        }

        public bool _openCurlyOnNewLine;

        public bool OpenCurlyOnNewLine
        {
            get { return _openCurlyOnNewLine; }
            set
            {
                _openCurlyOnNewLine = value;
                OnPropertyChanged(nameof(OpenCurlyOnNewLine));

            }
        }

        public bool _closeCurlyOnNewLine;

        public bool CloseCurlyOnNewLine
        {
            get { return _closeCurlyOnNewLine; }
            set
            {
                _closeCurlyOnNewLine = value;
                OnPropertyChanged(nameof(CloseCurlyOnNewLine));

            }
        }

        public bool _preserveOriginalWhitespace;

        public bool PreserveOriginalWhitespace
        {
            get { return _preserveOriginalWhitespace; }
            set
            {
                _preserveOriginalWhitespace = value;
                OnPropertyChanged(nameof(PreserveOriginalWhitespace));

            }
        }

        public bool _keepRedundantApiCalls;

        public bool KeepRedundantApiCalls
        {
            get { return _keepRedundantApiCalls; }
            set
            {
                _keepRedundantApiCalls = value;
                OnPropertyChanged(nameof(KeepRedundantApiCalls));

            }
        }

        public bool _avoidUsingStatic;

        public bool AvoidUsingStatic
        {
            get { return _avoidUsingStatic; }
            set
            {
                _avoidUsingStatic = value;
                OnPropertyChanged(nameof(AvoidUsingStatic));

            }
        }

      
    }
}
