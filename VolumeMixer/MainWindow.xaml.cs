using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
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

namespace VolumeMixer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<DialViewHolder> dialViewHolders = new List<DialViewHolder>();
        ObservableCollection<IAudioSession> audioSessions = new ObservableCollection<IAudioSession>();

        public MainWindow()
        {
            InitializeComponent();

            dialViewHolders.Add(new DialViewHolder(lblDial1, imgDial1, cmbDial1, proDial1));
            dialViewHolders.Add(new DialViewHolder(lblDial2, imgDial2, cmbDial2, proDial2));
            dialViewHolders.Add(new DialViewHolder(lblDial3, imgDial3, cmbDial3, proDial3));
            dialViewHolders.Add(new DialViewHolder(lblDial4, imgDial4, cmbDial4, proDial4));

            foreach(DialViewHolder view in dialViewHolders)
            {
                view.combobox.ItemsSource = audioSessions;
                view.combobox.SelectionChanged += cmb_SelectionChanged;
            }

            refreshButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            PacketManager pm = new PacketManager(SerialPortFinder.FindCorrectPortname(), dialViewHolders, Dispatcher);
        }

        void cmb_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox cmb = sender as ComboBox;
            IAudioSession a = (IAudioSession)cmb.SelectedItem;

            if (a == null)
                return;

            DialViewHolder view = dialViewHolders.Single(s => s.combobox == cmb); // Get viewholder of correct dial
            view.image.Source = a.imageSource;
        }

        private void refreshButton_Click(object sender, RoutedEventArgs e)
        {
            audioSessions.Clear();
            foreach (AudioSession a in AudioUtilities.GetAllSessions())
            {
                audioSessions.Add(a);
            }
        }
    }
}
