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
using System.Windows.Navigation;
using System.Windows.Shapes;
using TelegramBot.Util.Bot;
using TelegramBot.Util.Settings;

namespace TelegramBot.Pilot.WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SettingsManager mgr = new SettingsManager("CheongBot");

            //MessageBox.Show(mgr.BotToken);

            MessageCommunicator comm = new MessageCommunicator(mgr.BotToken);


            comm.SendMessage(mgr.DefaultRcpt, "Hello world!");
        }
    }
}
