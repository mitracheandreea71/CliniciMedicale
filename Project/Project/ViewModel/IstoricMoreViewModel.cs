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
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Windows.Controls;
using System.Windows.Markup;


namespace Project.ViewModel
{
    internal class IstoricMoreViewModel : BaseViewModel
    {
        public struct IstoricItem
        {
            public string Tip { get; set; } 
            public string Data { get; set; } 
            public int IdItem { get; set; }

            public string ImageSource
            {
                get
                {
                    return Tip == "Analiza" ? "/Images/analiza.png" : "/Images/consultatie.png";

                }
            }
        }

        private string _cnp;
        public string Cnp
        {
            get => _cnp;
            set
            {
                _cnp = value;
                OnPropertyChanged(nameof(Cnp));
            }
        }

        private ObservableCollection<IstoricItem> _istoricItems;
        public ObservableCollection<IstoricItem> IstoricItems
        {
            get => _istoricItems;
            set
            {
                _istoricItems = value;
                OnPropertyChanged(nameof(IstoricItems));
            }
        }

        public ICommand VizualizareIstoricCommand { get; }
        public ICommand VizualizareRezultatCommand { get; }
        public ICommand SavePdfCommand { get; }

        private readonly BuletinAnalizeModel _buletinAnalizeModel;
        private readonly ConsultatieModel _consultatieModel;
        private readonly CliniciDataContext _context;
        private readonly MediciModel _medicModel;
        public IstoricMoreViewModel()
        {
            IstoricItems = new ObservableCollection<IstoricItem>();
            _buletinAnalizeModel = new BuletinAnalizeModel();
            _consultatieModel = new ConsultatieModel();
            _context = new CliniciDataContext();
            _medicModel = new MediciModel();
            VizualizareIstoricCommand = new BaseCommand(VizualizareIstoric, CanExecuteVizualizareIstoric);
            VizualizareRezultatCommand = new BaseCommand(VizualizareRezultat);
            SavePdfCommand = new BaseCommand(SaveAsPdf);
        }

        private bool CanExecuteVizualizareIstoric(object parameter)
        {
            return !string.IsNullOrWhiteSpace(Cnp);
        }

        private void VizualizareIstoric(object parameter)
        {
            int idP;
            if (!int.TryParse(Cnp, out idP))
            {
                MessageBox.Show("CNP-ul introdus nu este valid.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var pacient = _context.Pacients.FirstOrDefault(p => p.id_pacient == idP);
            if (pacient == null)
            {
                MessageBox.Show("Pacientul nu a fost găsit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            IstoricItems.Clear();
            idP = int.Parse(Cnp);
            var analize = _buletinAnalizeModel.GetBuletineByPacientID(idP);
            foreach (var analiza in analize)
            {
                IstoricItems.Add(new IstoricItem
                {
                    Tip = "Analiza",
                    Data = analiza.DataRecoltare.ToString("dd/MM/yyyy"),
                    IdItem = analiza.IDBuletin
                });
            }
            var consultatii = _consultatieModel.GetConsultatiiByPacientID(idP);
            foreach (var consultatie in consultatii)
            {
                IstoricItems.Add(new IstoricItem
                {
                    Tip = "Consultatie",
                    Data = consultatie.DataConsultatie?.ToString("dd/MM/yyyy") + " Ora: " + consultatie.Ora,
                    IdItem = consultatie.IdConsultatie
                });
            }
           
        }

        private void VizualizareRezultat(object parameter)
        {
            if (parameter is IstoricItem item && item.Tip == "Analiza")
            {
                DateTime now = DateTime.Now;
                string data = item.Data.Replace('/', '-');
                string[] parts = data.Split('-');

                string newDate = $"{parts[1]}-{parts[0]}-{parts[2]}";
                if (DateTime.TryParse(newDate, out DateTime parsedDate))
                {
                    if (parsedDate > DateTime.Now)
                    {
                        MessageBox.Show($"Nu poti sa vezi rezultatul inca", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                int idBuletin = item.IdItem;
                int idPacient = int.Parse(Cnp);

                var buletin = _buletinAnalizeModel.GetBuletineByPacientID(idPacient)
                                                  .FirstOrDefault(b => b.IDBuletin == idBuletin);

                if (buletin == null)
                {
                    MessageBox.Show("Analiza selectată nu aparține pacientului curent.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var rezultatWindow = new RezultatAnalizeWindow(idBuletin, idPacient);
                rezultatWindow.ShowDialog();
            }
            else if (parameter is IstoricItem consultatieItem && consultatieItem.Tip == "Consultatie")
            {
                int index = consultatieItem.Data.IndexOf(" ");

                string result = (index != -1) ? consultatieItem.Data.Substring(0, index) : consultatieItem.Data;
                result = result.Replace('/', '-');
                string[] parts = result.Split('-');

                string newDate = $"{parts[1]}-{parts[0]}-{parts[2]}";
                if (DateTime.TryParse(newDate, out DateTime parsedDate))
                {
                    if (parsedDate > DateTime.Now)
                    {
                        MessageBox.Show($"Nu poti sa vezi rezultatul inca", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                int idConsultatie = consultatieItem.IdItem;
                int idPacient = int.Parse(Cnp);

                var consultatie = _consultatieModel.GetConsultatiiByPacientID(idPacient)
                                                   .FirstOrDefault(c => c.IdConsultatie == idConsultatie);

                if (consultatie == null)
                {
                    MessageBox.Show("Consultația selectată nu aparține pacientului curent.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                string id_medic = consultatie.IdDoctor.ToString();
                int im = int.Parse(id_medic);
                MediciModel medic = _medicModel.GetMedicById(im);
                var vizualizareDW = new VizualizareDiagnosticWindow(consultatie,medic);
                vizualizareDW.Show();
            }
            else
            {
                MessageBox.Show("Doar rezultatele analizelor sau consultațiilor pot fi vizualizate.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveAsPdf(object parameter)
        {
            if (parameter is IstoricItem item)
            {
                string result=item.Data;
                if(item.Tip=="Consultatie")
                {
                    int index = item.Data.IndexOf(" ");

                    result = (index != -1) ? item.Data.Substring(0, index) : item.Data;
                }
                
                string data = result.Replace('/', '-');
                string[] parts = data.Split('-');

                string newDate = $"{parts[1]}-{parts[0]}-{parts[2]}";
                if (DateTime.TryParse(newDate, out DateTime parsedDate))
                {
                    if (parsedDate > DateTime.Now)
                    {
                        MessageBox.Show($"Nu poti sa salvezi rezultatul inca", "Ups...Ceva nu a mers bine", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }
                try
                {
                    var saveDialog = new Microsoft.Win32.SaveFileDialog
                    {
                        Filter = "PDF files (*.pdf)|*.pdf",
                        DefaultExt = ".pdf",
                        FileName = $"{item.Tip}_{item.Data.Replace("/", "_")}"
                    };

                    if (saveDialog.ShowDialog() == true)
                    {
                        if (item.Tip == "Analiza")
                        {
                            var rezultatView = new AnalizeRezultatView(item.IdItem,int.Parse(Cnp));

                            rezultatView.Width = 1100; 
                            rezultatView.Height = 1000;
                            rezultatView.DataContext = new AnalizeRezultatViewModel(item.IdItem, int.Parse(Cnp));

                            rezultatView.Measure(new Size(double.PositiveInfinity, double.PositiveInfinity));
                            rezultatView.Arrange(new Rect(0, 0, rezultatView.Width, rezultatView.Height));
                            rezultatView.UpdateLayout();

                            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

                            var document = new PdfDocument();
                            var page = document.AddPage();

                            page.Width = XUnit.FromMillimeter(210); 
                            page.Height = XUnit.FromMillimeter(297);

                            var gfx = XGraphics.FromPdfPage(page);

                            // cea mai mare rezolutie pentru bitmap 300
                            var dpi = 300.0;
                            var scale = dpi / 96.0; // 96 este DPI-ul standard Windows

                            var renderBitmap = new RenderTargetBitmap(
                                (int)(rezultatView.Width * scale),
                                (int)(rezultatView.Height * scale),
                                dpi,
                                dpi,
                                PixelFormats.Pbgra32);

                            renderBitmap.Render(rezultatView);
                            var encoder = new PngBitmapEncoder();
                            encoder.Frames.Add(BitmapFrame.Create(renderBitmap));

                            using (var stream = new MemoryStream())
                            {
                                encoder.Save(stream);
                                stream.Position = 0;

                                var xImage = XImage.FromStream(stream);

                                double pageWidth = page.Width.Point;
                                double pageHeight = page.Height.Point;
                                double imageWidth = rezultatView.Width;
                                double imageHeight = rezultatView.Height;

                                double scaleFactor = Math.Min(pageWidth / imageWidth, pageHeight / imageHeight);
                                double finalWidth = imageWidth * scaleFactor;
                                double finalHeight = imageHeight * scaleFactor;

                                double x = (pageWidth - finalWidth) / 2;
                                double y = (pageHeight - finalHeight) / 2;

                                gfx.DrawImage(xImage, x, y, finalWidth, finalHeight);
                            }

                            document.Save(saveDialog.FileName);
                            MessageBox.Show("PDF-ul a fost salvat cu succes!", "Succes",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        if (item.Tip == "Consultatie")
                        {

                            var consultatie = new ConsultatieModel().GetConsultatieByID(item.IdItem);
                            string id_medic = consultatie.IdDoctor.ToString();
                            int im = int.Parse(id_medic);
                            MediciModel medic = _medicModel.GetMedicById(im);
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

                            MessageBox.Show("PDF-ul a fost salvat cu succes!", "Succes",
                                MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Eroare la salvarea PDF-ului: {ex.Message}", "Eroare",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}