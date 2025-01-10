using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Project.Model;
using System.Windows.Media;

namespace Project.ViewModel
{
    internal class MedicHomeViewModel : BaseViewModel
    {

        private string _juramantMedic;
        private string _welcomeMessage;
        private MediciModel _medic;
        public MedicHomeViewModel(MediciModel medic)
        {
            _juramantMedic = citesteJuramant("../../Images/juramant.txt");
            _welcomeMessage = $"Bine ai venit, \n {medic.Titulatura} {medic.Nume} {medic.Prenume}!";
            _medic = medic;
        }

        public string JuramantMedic
        {
            get => _juramantMedic;
        }

        public string citesteJuramant(string fileName)
        {
            string juramant=File.ReadAllText(fileName);
            return juramant;
        }

        public string WelcomeMessage
        {
            get => _welcomeMessage;
        }
    }
}
