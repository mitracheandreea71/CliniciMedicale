using PdfSharp.Drawing;
using PdfSharp.Pdf;
using Project.Commands;
using Project.Model;
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
    public class ViewAnalizeAsistViewModel : BaseViewModel
    {
        private AsistentiModel _asistent;

        private ObservableCollection<BuletinAnalizeModel> _buletine;
        private ObservableCollection<BuletinAnalizeModel> _buletineFiltrate;
        private DateTime? _selectedDate;
        public ICommand BackButton { get; }
        public ICommand SaveResultsAsPdf { get; }
        public ICommand EmiteFactura { get; }
        public DateTime? SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                FiltreazaAnalizele();
                OnPropertyChanged(nameof(SelectedDate));
            }
        }

        public ViewAnalizeAsistViewModel(AsistentiModel asistent)
        {
            _asistent = asistent;
            BackButton = new BaseCommand(c => { EventAggregator.Instance.Publish(new ViewChangeMessage<AsistentiModel>("HomePage", _asistent)); });
            _buletine = (new BuletinAnalizeModel()).GetBuletineAnalizaByClinicaId((int)_asistent.IdClinica);
            SaveResultsAsPdf = new BaseCommand(c => verifyDateForSave(c as BuletinAnalizeModel));
            EmiteFactura = new BaseCommand(c => verifyDateForFactura(c as BuletinAnalizeModel));
        }
        void verifyDateForFactura(BuletinAnalizeModel buletin)
        {
            if (DateTime.Now < buletin.DataRecoltare)
            {
                MessageBox.Show($"Nu poti emite factura inca", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveAsPdfFactura(buletin);
        }
        void verifyDateForSave(BuletinAnalizeModel buletin)
        {
            if (DateTime.Now < buletin.DataRecoltare)
            {
                MessageBox.Show($"Nu poti salva rezultatul inca", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SaveAsPdf(buletin);
        }
        public ObservableCollection<BuletinAnalizeModel> Analize
        {
            get => _buletineFiltrate ?? _buletine;
            set
            {
                _buletine = value;
                OnPropertyChanged(nameof(Analize));
            }
        }
        private void SaveAsPdfFactura(object parameter)
        {
            if (parameter is BuletinAnalizeModel item)
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
        public void FiltreazaAnalizele()
        {
            if (_selectedDate.HasValue)
            {
                string selectedDateString = _selectedDate.Value.ToString("yyyy-MM-dd");
                _buletineFiltrate = new ObservableCollection<BuletinAnalizeModel>(
                    _buletine.Where(b => b.Data == selectedDateString)
                );
            }
            else
            {
                _buletineFiltrate = _buletine;
            }
            OnPropertyChanged(nameof(Analize));
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

                        MessageBox.Show("PDF-ul a fost salvat cu succes!", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la salvarea PDF-ului: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
