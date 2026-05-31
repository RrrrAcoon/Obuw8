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
using Obuw14.Modeli;
using System.Data.Entity;

namespace Obuw14
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Polzovatel _pol;
        ObuwKontext db = new ObuwKontext();
        RedaktTovar _oknoTovar;
        public MainWindow(Polzovatel pol)
        {
            InitializeComponent();
            _pol = pol;
            txtPolzovatel.Text = _pol == null ? "Гость" : $"{_pol.FIO} ({_pol.Rol.Nazvanie})";
            NastroikaDostupa();
            Zagruzka();
        }
        void Zagruzka()
        {
            var list = db.Postavchiki.ToList();
            list.Insert(0, new Postavchik { Nazvanie = "Все поставщики" });
            ComboFiltr.ItemsSource = list;
            ComboFiltr.DisplayMemberPath = "Nazvanie";
            ComboFiltr.SelectedIndex = 0;
            ComboSort.SelectedIndex= 0;
        }

        void ObnovlenieDannih()
        {
            var tovari = db.Tovari
                .Include(t => t.Postavchik)
                .Include(t => t.Proizvoditel)
                .Include(t => t.Kategoriya)
                .Include(t => t.EdinicaEzmereniya)
                .ToList();

            if(!string.IsNullOrWhiteSpace(txtPoisk.Text))
            {
                var q = txtPoisk.Text.ToLower();
                tovari = tovari.Where(p => 
                p.Naimenovanie.ToLower().Contains(q) ||
                (p.Opisanie!=null&&p.Opisanie.ToLower().Contains(q)) ||
                (p.Kategoriya!=null && p.Kategoriya.Nazvanie.ToLower().Contains(q)) ||
                (p.Proizvoditel != null && p.Proizvoditel.Nazvanie.ToLower().Contains(q)) ||
                (p.Postavchik != null && p.Postavchik.Nazvanie.ToLower().Contains(q)) 
                ).ToList();
            }

            if(ComboFiltr.SelectedIndex >0)
            {
                var ps = ComboFiltr.SelectedItem as Postavchik;
                tovari = tovari.Where(p => p.PostavchikId == ps.Id).ToList();
            }

            if(ComboSort.SelectedIndex == 1) tovari = tovari.OrderBy(p => p.Kolichestvo).ToList();
            if (ComboSort.SelectedIndex == 2) tovari = tovari.OrderByDescending(p => p.Kolichestvo).ToList();

            LvElement.ItemsSource = tovari;
        }

        void NastroikaDostupa()
        {
            if (_pol?.RolId == 1) return;
            BthDobavit.Visibility= Visibility.Collapsed;
            BthRedakt.Visibility = Visibility.Collapsed;
            BthUdalit.Visibility = Visibility.Collapsed;
            if(_pol == null || _pol.RolId==3)
            {
                PanelPoiska.Visibility=Visibility.Collapsed;
                Zakazi.Visibility=Visibility.Collapsed;
            }

        }

        bool OknoUzheOtkrito()
        {
            if (_oknoTovar !=null && _oknoTovar.IsLoaded)
            {
                MessageBox.Show("Окно уже открыто", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                _oknoTovar.Focus();
                return true;
            }
            return false;
        }

        void OtkritTovar(Tovar tovar)
        {
            if (OknoUzheOtkrito()) return;
                _oknoTovar = new RedaktTovar(tovar, db);
                _oknoTovar.ShowDialog();
                ObnovlenieDannih();
        }

        private void Vihod(object sender, RoutedEventArgs e)
        {
            new Login().Show();
            Close();
        }

        private void ZakaziBth(object sender, RoutedEventArgs e)
        {
            new OknoZakazi(_pol,db).Show();
            Close();
        }

        private void Poisk(object sender, TextChangedEventArgs e)
        {
            ObnovlenieDannih();
        }

        private void Vibor(object sender, SelectionChangedEventArgs e)
        {
            ObnovlenieDannih();
        }

        private void Dobavit(object sender, RoutedEventArgs e)
        {
            OtkritTovar(null);
        }

        private void Redakt(object sender, RoutedEventArgs e)
        {
            var t = LvElement.SelectedItem as Tovar;
            if(t==null)
            {
                MessageBox.Show("Выбирите товар!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            OtkritTovar(t);
        }

        private void Udalit(object sender, RoutedEventArgs e)
        {
            var t = LvElement.SelectedItem as Tovar;
            if (t == null)
            {
                MessageBox.Show("Выбирите товар!", "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            if (db.ZakaziTovarov.Any(zt => zt.TovarId == t.Id)) { MessageBox.Show("Товар присутсвует в заказах!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error); return; }
            if (MessageBox.Show("Удалить товар?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) != MessageBoxResult.Yes) return;
            db.Tovari.Remove(t);
            db.SaveChanges();
            ObnovlenieDannih();
        }

        private void ClickLv(object sender, MouseButtonEventArgs e)
        {
            if (_pol?.RolId == 1 && LvElement.SelectedItem is Tovar)
                OtkritTovar(LvElement.SelectedItem as Tovar);
        }
    }
}
