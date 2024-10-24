﻿using ReactiveUI;
using RegexGenerator.ViewModels;
using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RegexGenerator.Views
{
    /// <summary>
    /// GeneratorView.xaml 的交互逻辑
    /// </summary>
    public partial class GeneratorView : ReactiveUserControl<GeneratorViewModel>
    {
        public GeneratorView()
        {
            InitializeComponent();
            this.WhenActivated(d => { });
        }
    }
}
