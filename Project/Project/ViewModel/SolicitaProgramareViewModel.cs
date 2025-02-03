using Project.Commands;
using Project.Model;
using Project.ViewModel;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Windows;
using System;

internal class SolicitaProgramareViewModel : BaseViewModel
{
    private int _medicID;
    private string _numeMedic;
    private DateTime? _dataProgramare;

    private string _cnp;
    private string _nume;
    private string _prenume;
    private string _judet;
    private string _adresa;

    private ObservableCollection<string> _availableHours;
    private string _selectedHour;

    public SolicitaProgramareViewModel(int medicID, string numeMedic)
    {
        _medicID = medicID;
        _numeMedic = numeMedic;
        AvailableHours = new ObservableCollection<string>();
        TrimiteProgramareCommand = new BaseCommand(ExecuteTrimiteProgramare, CanExecuteTrimiteProgramare);
    }

    public int MedicID
    {
        get => _medicID;
        set
        {
            _medicID = value;
            OnPropertyChanged(nameof(MedicID));
        }
    }

    public string NumeMedic
    {
        get => _numeMedic;
        set
        {
            _numeMedic = value;
            OnPropertyChanged(nameof(NumeMedic));
        }
    }

    public DateTime? DataProgramare
    {
        get => _dataProgramare;
        set
        {
            _dataProgramare = value;
            OnPropertyChanged(nameof(DataProgramare));
            LoadAvailableHours();
        }
    }

    public string CNP
    {
        get => _cnp;
        set
        {
            _cnp = value;
            OnPropertyChanged(nameof(CNP));
        }
    }

    public string Nume
    {
        get => _nume;
        set
        {
            _nume = value;
            OnPropertyChanged(nameof(Nume));
        }
    }

    public string Prenume
    {
        get => _prenume;
        set
        {
            _prenume = value;
            OnPropertyChanged(nameof(Prenume));
        }
    }

    public string Judet
    {
        get => _judet;
        set
        {
            _judet = value;
            OnPropertyChanged(nameof(Judet));
        }
    }

    public string Adresa
    {
        get => _adresa;
        set
        {
            _adresa = value;
            OnPropertyChanged(nameof(Adresa));
        }
    }

    public string SelectedHour
    {
        get => _selectedHour;
        set
        {
            _selectedHour = value;
            OnPropertyChanged(nameof(SelectedHour));
        }
    }

    public ObservableCollection<string> AvailableHours
    {
        get => _availableHours;
        set
        {
            _availableHours = value;
            OnPropertyChanged(nameof(AvailableHours));
        }
    }

    public ICommand TrimiteProgramareCommand { get; }

    private void LoadAvailableHours()
    {
        if (!DataProgramare.HasValue)
        {
            AvailableHours.Clear();
            return;
        }

        try
        {
            var incadrareModel = new IncadrareDepartamentModel();
            var intervalOrar = incadrareModel.GetByIdMedic(MedicID);

            if (intervalOrar == null)
            {
                MessageBox.Show("Intervalul orar al medicului nu a fost găsit.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
                AvailableHours.Clear();
                return;
            }

            var consultatieModel = new ConsultatieModel();
            var oreProgramate = consultatieModel.GetOreProgramate(MedicID, DataProgramare.Value);

            var oreDisponibile = CalculateAvailableHours(
                TimeSpan.Parse(intervalOrar.intrare_tura),
                TimeSpan.Parse(intervalOrar.iesire_tura),
                oreProgramate);

            AvailableHours.Clear();
            foreach (var ora in oreDisponibile)
            {
                AvailableHours.Add(ora);
            }
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare la actualizarea orelor disponibile: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private void ExecuteTrimiteProgramare(object parameter)
    {
        if (string.IsNullOrWhiteSpace(CNP) ||
            string.IsNullOrWhiteSpace(Nume) ||
            string.IsNullOrWhiteSpace(Prenume) ||
            string.IsNullOrWhiteSpace(Judet) ||
            string.IsNullOrWhiteSpace(Adresa) ||
            !DataProgramare.HasValue ||
            string.IsNullOrWhiteSpace(SelectedHour))
        {
            MessageBox.Show("Toate câmpurile sunt obligatorii.", "Eroare", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        try
        {
            var pacientModel = new PacientModel();
            var pacientID = pacientModel.GetOrCreatePacient(CNP, Nume, Prenume, Judet, Adresa);

            var consultatieModel = new ConsultatieModel();
            consultatieModel.AdaugaConsultatie(MedicID, pacientID, DataProgramare.Value, SelectedHour);

            MessageBox.Show($"Programarea a fost trimisă pentru medicul {NumeMedic}.", "Succes", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Eroare la trimiterea programării: {ex.Message}", "Eroare", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanExecuteTrimiteProgramare(object parameter) => true;

    private List<string> CalculateAvailableHours(TimeSpan intrare, TimeSpan iesire, List<string> oreProgramate)
    {
        var oreDisponibile = new List<string>();

        for (int hour = intrare.Hours; hour < iesire.Hours; hour++)
        {
            var ora = $"{hour}:00";
            if (!oreProgramate.Contains(ora))
            {
                oreDisponibile.Add(ora);
            }
        }

        return oreDisponibile;
    }
}
