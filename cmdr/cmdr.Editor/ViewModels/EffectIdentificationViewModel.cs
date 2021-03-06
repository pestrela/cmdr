﻿using cmdr.Editor.AppSettings;
using cmdr.Editor.Utils;
using cmdr.TsiLib;
using cmdr.TsiLib.EventArgs;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace cmdr.Editor.ViewModels
{
    public class EffectIdentificationViewModel : ViewModelBase
    {
        public enum Options
        {
            Option1,
            Option2,
            Option3
        }

        private readonly EffectIdentificationRequest _request;

        public Action CloseAction { get; set; }
        public Window Window { get; set; }

        #region Commands 

        private ICommand _closeCommand;
        public ICommand CloseCommand
        {
            get { return _closeCommand ?? (_closeCommand = new CommandHandler(CloseAction)); }
        }

        private ICommand _okCommand;
        public ICommand OkCommand
        {
            get { return _okCommand ?? (_okCommand = new CommandHandler(ok)); }
        }
        
        private ICommand _browseFileCommand;
        public ICommand BrowseFileCommand
        {
            get { return _browseFileCommand ?? (_browseFileCommand = new CommandHandler<TextBlock>((tb) => browseFile(tb))); }
        }

        private ICommand _selectPreviousCommand;
        public ICommand SelectPreviousCommand
        {
            get { return _selectPreviousCommand ?? (_selectPreviousCommand = new CommandHandler(selectPrevious)); }
        }

        private ICommand _selectNextCommand;
        public ICommand SelectNextCommand
        {
            get { return _selectNextCommand ?? (_selectNextCommand = new CommandHandler(selectNext)); }
        }

        #endregion

        public bool CanUseTraktorSettings { get { return TraktorSettings.Initialized; } }

        public string Title { get { return _request.Id; } }

        private Options _currentOption;
        public Options CurrentOption
        {
            get { return _currentOption; }
            set { _currentOption = value; raisePropertyChanged("CurrentOption"); }
        }

        private string _pathToTsi;
        public string PathToTsi
        {
            get { return _pathToTsi; }
            set { _pathToTsi = value; raisePropertyChanged("PathToTsi"); }
        }


        public EffectIdentificationViewModel(EffectIdentificationRequest request)
        {
            _request = request;
            _currentOption = Options.Option3;
        }


        private void selectPrevious()
        {
            int min = CanUseTraktorSettings ? 0 : 1;
            int current = (int)CurrentOption;
            int previous = (3 + current - 1) % 3;
            if (previous < min)
                previous = 2;
            CurrentOption = (Options)previous;
        }

        private void selectNext()
        {
            int min = CanUseTraktorSettings ? 0 : 1;
            int current = (int)CurrentOption;
            int next = (current + 1) % 3;
            if (next < min)
                next = min;
            CurrentOption = (Options)next;
        }

        private void ok()
        {
            switch (CurrentOption)
            {
                case EffectIdentificationViewModel.Options.Option1:
                    _request.FxSettings = TraktorSettings.Instance.FxSettings;
                    break;
                case EffectIdentificationViewModel.Options.Option2:
                    if (String.IsNullOrEmpty(PathToTsi))
                    {
                        MessageBoxHelper.ShowError("Please specify a path.");
                        return;
                    }
                    else
                    {
                        try
                        {
                            _request.FxSettings = TsiFile.Load(CmdrSettings.Instance.TraktorVersion, PathToTsi, false).FxSettings;
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine(ex.Message);
                            MessageBoxHelper.ShowError("Could not load " + PathToTsi);
                            return;
                        }
                    }
                    break;
                case EffectIdentificationViewModel.Options.Option3:
                    break;
                default:
                    break;
            }

            if (CloseAction != null)
                CloseAction();
        }

        private void browseFile(TextBlock textBlock)
        {
            string folder = BrowseDialogHelper.BrowseTsiFile(Window, false);
            if (folder != null)
                textBlock.Text = folder;
        }

    }
}
