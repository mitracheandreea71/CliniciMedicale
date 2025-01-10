using Project.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Project.View;
using System.Windows.Controls;


namespace Project.ViewModel
{
    internal class AdminViewModel:BaseViewModel
    {
        public ICommand AddEmployeeCommand { get; }
        public ICommand DeleteEmployeeCommand { get; }
        public ICommand ExtendEmployeeContractCommand { get; }
        public ICommand AddDepartmentCommand { get; }
        public ICommand DeleteDepartmentCommand { get; }
        public ICommand AddClinicCommand { get; }
        public ICommand DeleteClinicCommand { get; }
        public ICommand AddPatientCommand { get; }
        public ICommand DeletePatientCommand { get; }

        private UserControl _selectedViewModel;
        public UserControl SelectedViewModel
        {
            get => _selectedViewModel;
            set
            {
                _selectedViewModel = value;
                OnPropertyChanged(nameof(SelectedViewModel));
            }
        }

        public AdminViewModel()
        {
            AddEmployeeCommand = new BaseCommand(_ => SelectedViewModel = new AddEmployeeView());
            DeleteEmployeeCommand = new BaseCommand(_ => SelectedViewModel = new DeleteEmployeeView());
            ExtendEmployeeContractCommand = new BaseCommand(_ => SelectedViewModel = new ExtendEmployeeContractView());
            AddDepartmentCommand = new BaseCommand(_ => SelectedViewModel = new AddDepartmentView());
            DeleteDepartmentCommand = new BaseCommand(_ => SelectedViewModel = new DeleteDepartmentView());
            AddClinicCommand = new BaseCommand(_ => SelectedViewModel = new AddClinicView());
            DeleteClinicCommand = new BaseCommand(_ => SelectedViewModel = new DeleteClinicView());
            DeletePatientCommand = new BaseCommand(_ => SelectedViewModel = new DeletePatientView());
        }
    }
}
