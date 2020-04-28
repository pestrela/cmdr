﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

// https://stackoverflow.com/questions/2796470/wpf-create-a-dialog-prompt
namespace cmdr.Editor.Utils
{

    public class SedResult
    {
        public string _search;
        public string _replace;

        public SedResult(string search, string replace)
        {
            _search = search;
            _replace = replace;
        }
    }

    public partial class SedWindow : Window
    {
        private InputType _inputType = InputType.Text;

        public SedWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(SedWindow_Loaded);

            /*
            txtQuestion.Text = question;
            Title = "title";
            txtResponse.Text = defaultValue;
            _inputType = inputType;*/
        }

        // call it with: string repeatPassword = PromptDialog.Prompt("Repeat password", "Password confirm", inputType: PromptDialog.InputType.Password)

        // SedResult ret = SedWindow.Prompt();
        void SedWindow_Loaded(object sender, RoutedEventArgs e)
        {
            SearchTB.Focus();
        }

        public static SedResult Prompt()
        {
            SedWindow inst = new SedWindow();
            inst.ShowDialog();
            if (inst.DialogResult == true) {
                var ret = new SedResult(inst.SearchTB.Text, inst.ReplaceTB.Text);
                return ret;
            }
            return null;
        }

  
        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }


}