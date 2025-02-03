using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Project.Commands;
using Project.Model;
using Project.View;
using Project.Windows;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace Project.ViewModel
{
    public class ViewProgramariAsistViewModel : BaseViewModel
    {
        private AsistentiModel _asistent;
        private ObservableCollection<ConsultatieModel> _consultatii;
        private ObservableCollection<ConsultatieModel> _consultatiiFiltrate;

        private DateTime? _selectedDate;
        public ICommand EmiteFactura { get; }
        public ICommand SaveResultsAsPdf { get; }
        public ICommand BackButton { get; }
        public ViewProgramariAsistViewModel(AsistentiModel asistent)
        {
            _asistent = asistent;
            BackButton = new BaseCommand(c => { EventAggregator.Instance.Publish(new ViewChangeMessage<AsistentiModel>("HomePage", _asistent)); });
            _consultatii = (new ConsultatieModel()).GetAllConsultatiiForAsistent(_asistent.IdAngajat);
            SaveResultsAsPdf = new BaseCommand(c => verifyDateForSave(c as ConsultatieModel));
            EmiteFactura = new BaseCommand(c => verifyDateForFactura(c as ConsultatieModel));
        }
        void verifyDateForFactura(ConsultatieModel buletin)
        {
            if (DateTime.Now < buletin.DataConsultatie)
            {
                MessageBox.Show($"Nu poti emita factura inca", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveAsPdfFactura(buletin);
        }
        void verifyDateForSave(ConsultatieModel buletin)
        {
            if (DateTime.Now < buletin.DataConsultatie)
            {
                MessageBox.Show($"Nu poti salva rezultatul inca", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveAsPdf(buletin);
        }
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                FiltreazaConsultatii();
                OnPropertyChanged(nameof(SelectedDate));
            }
        }
        public void FiltreazaConsultatii()
        {
            if (_selectedDate.HasValue)
            {
                string selectedDateString = _selectedDate.Value.ToString("yyyy-MM-dd");
                _consultatiiFiltrate = new ObservableCollection<ConsultatieModel>(
                    _consultatii.Where(b => b.DataConsultatie.Value.ToString("yyyy-MM-dd") == selectedDateString)
                );
            }
            else
            {
                _consultatiiFiltrate = _consultatii;
            }
            OnPropertyChanged(nameof(Programari ));
        }
        public ObservableCollection<ConsultatieModel> Programari
        {
            get => _consultatiiFiltrate ?? _consultatii;
            set
            {
                _consultatii = value;
                FiltreazaConsultatii();
                OnPropertyChanged(nameof(Programari));
            }
        }
        private void SaveAsPdfFactura(object parameter)
        {
            if (parameter is ConsultatieModel item)
            {
                try
                {
                    var saveDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "PDF files (*.pdf)|*.pdf",
                        DefaultExt = ".pdf",
                        FileName = $"Factura{item.Data.Replace("/", "_")}"
                    };
                    if (saveDialog.ShowDialog() == true)
                    {
                        var rezultatWindow = new FacturaWindow(item);
                        rezultatWindow.Width = 1100;
                        rezultatWindow.Height = 700;

                        rezultatWindow.WindowStyle = WindowStyle.None;
                        rezultatWindow.WindowState = WindowState.Normal;
                        rezultatWindow.ShowInTaskbar = false;
                        rezultatWindow.AllowsTransparency = true;
                        rezultatWindow.Background = Brushes.White;

                        rezultatWindow.Show();

                        rezultatWindow.Measure(new Size(rezultatWindow.Width, rezultatWindow.Height));
                        rezultatWindow.Arrange(new Rect(0, 0, rezultatWindow.Width, rezultatWindow.Height));
                        rezultatWindow.UpdateLayout();

                        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                        var document = new PdfDocument();
                        var page = document.AddPage();
                        page.Width = XUnit.FromMillimeter(210);
                        page.Height = XUnit.FromMillimeter(297);
                        var gfx = XGraphics.FromPdfPage(page);

                        var dpi = 300.0;
                        var scale = dpi / 96.0;
                        var renderBitmap = new RenderTargetBitmap(
                            (int)(rezultatWindow.Width * scale),
                            (int)(rezultatWindow.Height * scale),
                            dpi,
                            dpi,
                            PixelFormats.Pbgra32);
                        renderBitmap.Render(rezultatWindow);

                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                        using (var stream = new MemoryStream())
                        {
                            encoder.Save(stream);
                            stream.Position = 0;
                            var xImage = XImage.FromStream(stream);

                            double pageWidth = page.Width.Point;
                            double pageHeight = page.Height.Point;
                            double imageWidth = rezultatWindow.Width;
                            double imageHeight = rezultatWindow.Height;
                            double scaleFactor = Math.Min(pageWidth / imageWidth, pageHeight / imageHeight);
                            double finalWidth = imageWidth * scaleFactor;
                            double finalHeight = imageHeight * scaleFactor;

                            double x = (pageWidth - finalWidth) / 2;
                            double y = (pageHeight - finalHeight) / 2;

                            gfx.DrawImage(xImage, x, y, finalWidth, finalHeight);
                        }

                        document.Save(saveDialog.FileName);

                        rezultatWindow.Close();

                        MessageBox.Show("PDF-ul a fost salvat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la salvarea PDF-ului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }


        private void SaveAsPdf(object parameter)
        {
            if (parameter is ConsultatieModel item)
            {
                try
                {
                    var saveDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "PDF files (*.pdf)|*.pdf",
                        DefaultExt = ".pdf",
                        FileName = $"Consultatie_{item.Data.Replace("/", "_")}"
                    };

                    if (saveDialog.ShowDialog() == true)
                    {
                        var consultatie = parameter as ConsultatieModel;
                        string id_medic = consultatie.IdDoctor.ToString();
                        int im = int.Parse(id_medic);
                        MediciModel medic = (new MediciModel()).GetMedicById(im);
                        var rezultatWindow = new VizualizareDiagnosticWindow(consultatie, medic);
                        rezultatWindow.Width = 900;
                        rezultatWindow.Height = 850;

                        rezultatWindow.WindowStyle = WindowStyle.None;
                        rezultatWindow.WindowState = WindowState.Normal;
                        rezultatWindow.ShowInTaskbar = false;
                        rezultatWindow.AllowsTransparency = true;
                        rezultatWindow.Background = Brushes.White;

                        rezultatWindow.Show();

                        rezultatWindow.Measure(new Size(rezultatWindow.Width, rezultatWindow.Height));
                        rezultatWindow.Arrange(new Rect(0, 0, rezultatWindow.Width, rezultatWindow.Height));
                        rezultatWindow.UpdateLayout();

                        System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                        var document = new PdfDocument();
                        var page = document.AddPage();
                        page.Width = XUnit.FromMillimeter(210);
                        page.Height = XUnit.FromMillimeter(297);
                        var gfx = XGraphics.FromPdfPage(page);

                        var dpi = 300.0;
                        var scale = dpi / 96.0;
                        var renderBitmap = new RenderTargetBitmap(
                            (int)(rezultatWindow.Width * scale),
                            (int)(rezultatWindow.Height * scale),
                            dpi,
                            dpi,
                            PixelFormats.Pbgra32);
                        renderBitmap.Render(rezultatWindow);

                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                        using (var stream = new MemoryStream())
                        {
                            encoder.Save(stream);
                            stream.Position = 0;
                            var xImage = XImage.FromStream(stream);

                            double pageWidth = page.Width.Point;
                            double pageHeight = page.Height.Point;
                            double imageWidth = rezultatWindow.Width;
                            double imageHeight = rezultatWindow.Height;
                            double scaleFactor = Math.Min(pageWidth / imageWidth, pageHeight / imageHeight);
                            double finalWidth = imageWidth * scaleFactor;
                            double finalHeight = imageHeight * scaleFactor;

                            double x = (pageWidth - finalWidth) / 2;
                            double y = (pageHeight - finalHeight) / 2;

                            gfx.DrawImage(xImage, x, y, finalWidth, finalHeight);
                        }

                        document.Save(saveDialog.FileName);

                        rezultatWindow.Close();

                        MessageBox.Show("PDF-ul a fost salvat cu succes!", "Succes",MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la salvarea PDF-ului: {ex.Message}", "Eroare",MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
