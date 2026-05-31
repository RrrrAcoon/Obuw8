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
using Microsoft.Win32;
using Obuw14.Modeli;
using System.Data.Entity;
using System.IO;

namespace Obuw14
{
    /// <summary>
    /// Логика взаимодействия для RedaktTovar.xaml
    /// </summary>
    public partial class RedaktTovar : Window
    {
        ObuwKontext _db;
        Tovar _tovar =new Tovar();
        public RedaktTovar(Tovar tovar, ObuwKontext db)
        {
            InitializeComponent();
            _db = db;

            // подгружаем связаные таблицы с товарами
            ComboEdinica.ItemsSource = _db.EdiniciEzmereniy.ToList();
            ComboPostavchik.ItemsSource = _db.Postavchiki.ToList();
            ComboProizvoditel.ItemsSource = _db.Proizvoditeli.ToList();
            ComboKategoriya.ItemsSource = _db.Kategorii.ToList();


            if (tovar == null)
            {
                LbId.Visibility = Visibility.Collapsed;
                txtId.Visibility = Visibility.Collapsed;
            }
            else
            {
                _tovar =tovar;
            }
            DataContext = _tovar;


        }

        private void Foto(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog { Filter = "Изображения|*.png;*.jpg;*.jpeg" };
            if (dlg.ShowDialog() != true) return;
            var img = new BitmapImage();
            img.BeginInit();
            img.CacheOption = BitmapCacheOption.OnLoad;
            img.CreateOptions = BitmapCreateOptions.IgnoreImageCache;
            img.UriSource = new Uri(dlg.FileName);
            img.EndInit();
            if(img.PixelWidth>300 ||img.PixelHeight>200)
            {
                MessageBox.Show("Фото не может быть более 300х200 пикселей!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            string papka = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"..\..\Kartinki\");
            string fileImya = Path.GetFileName(dlg.FileName);

            if(!string.IsNullOrEmpty(_tovar.Foto)&&_tovar.Foto!=fileImya&&_tovar.Foto!="picture.png")
            {
                string old = Path.Combine(papka, _tovar.Foto); 
                if(File.Exists(old))File.Delete(old);  
            }
            File.Copy(dlg.FileName, Path.Combine(papka, fileImya), true);
            _tovar.Foto = fileImya;
            MessageBox.Show("Фото добавлено", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void Sphranit(object sender, RoutedEventArgs e)
        {
            if (_tovar.EdinicaEzmereniyaId == 0 || _tovar.KategoriyaId== 0 || _tovar.PostavchikId== 0 || _tovar.ProizvoditelId == 0)
            {
                MessageBox.Show("Заполните все поля!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if(_tovar.Kolichestvo<0 ||_tovar.Skidka<0)
            {
                MessageBox.Show("Количетсво и скидка не могут быть менее 0!", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (_tovar.Id == 0) _db.Tovari.Add(_tovar);
            _db.SaveChanges();
            MessageBox.Show("Сохранено", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
        }
    }
}
