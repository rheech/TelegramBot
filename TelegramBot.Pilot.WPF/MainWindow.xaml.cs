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
using TelegramBot.Util.Bot;
using TelegramBot.Util.PostCommand;
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
            //MessageKeeper keeper = new MessageKeeper();

            SettingsManager _settings = new SettingsManager("CheongBot");
            MessageBox.Show(_settings.LastQueriedDate.Ticks.ToString());

            _settings.LastQueriedDate = DateTime.Now;

            //keeper.SaveMessage("A", "A");

            /*SettingsManager mgr = new SettingsManager("CheongBot");

            //MessageBox.Show(mgr.BotToken);

            BotCommander commander = new BotCommander(mgr);

            string rtn;

            if (commander.ProcessCommand("/퇴근시간", out rtn))
            {
                MessageBox.Show(rtn);
            }*/

            //MessageCommunicator comm = new MessageCommunicator(mgr.BotToken);

            //comm.SendMessage(mgr.DefaultRcpt, "Hello world!");
             

            // Weather test
            //TelegramBot.Util.RPC.PyRPCMethods rpc = new Util.RPC.PyRPCMethods();
            //rpc.ExecutePythonCommand("날씨", "서울");

            

        }
    }
}
