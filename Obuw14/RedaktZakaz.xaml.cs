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
using System.Windows.Shapes;
using Obuw14.Modeli;

namespace Obuw14
{
    /// <summary>
    /// Логика взаимодействия для RedaktZakaz.xaml
    /// </summary>
    public partial class RedaktZakaz : Window
    {
        Zakaz _zakaz;
        ObuwKontext _db;
        public RedaktZakaz(Zakaz zakaz,ObuwKontext db)
        {
            InitializeComponent();
            _db = db;

            ComboPunkt.ItemsSource = _db.PunktiVidachi.ToList();
            ComboClient.ItemsSource = _db.Polzovateli.ToList();
            ComboStatus.ItemsSource = _db.StatusiZakazov.ToList();

            if(zakaz==null)
            {
                _zakaz = new Zakaz
                {
                    DataZakaza = DateTime.Now,
                    DataVidachi = DateTime.Now.AddDays(7)
                };

                LbArtikul.Visibility = Visibility.Collapsed;
                txtArtikul.Visibility = Visibility.Collapsed;
            }
            else
            {
                _zakaz = zakaz;
            }
            DataContext = _zakaz;
        }

        private void Sphranit(object sender, RoutedEventArgs e)
        {
            if(_zakaz.PunktVidachiId ==0|| _zakaz.PolzovatelId ==0||_zakaz.StatusZakazaId==0)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_zakaz.Id == 0) _db.Zakazi.Add(_zakaz);
            _db.SaveChanges();
            MessageBox.Show("Сохранено", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
