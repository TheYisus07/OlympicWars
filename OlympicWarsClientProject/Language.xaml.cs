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
using System.Windows.Shapes;
 

namespace OlympicWarsClientProject
{
    /// <summary>
    /// Lógica de interacción para Language.xaml
    /// </summary>
    public partial class Language : Window
    {
        public Language()
        {
            InitializeComponent();
        }

        private void Button_OpenLogin(object sender, RoutedEventArgs e)
        {
            new Register().Show();
            this.Close();
        }

        private void ComboBox_ChooseLanguage(object sender, SelectionChangedEventArgs e)
        {
            if (cmb.SelectedIndex == 0)
            {
                Properties.Settings.Default.languageCode = "en-US";
            } 
            else
            {
                Properties.Settings.Default.languageCode = "es-MX";
            }
            Properties.Settings.Default.Save();
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(Properties.Langs.Lang.enter);
        }*/
    }
}
