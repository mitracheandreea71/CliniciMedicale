using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class ViewChangeMessage
    {
        public string NewView { get; set; }
        public ConsultatieModel Consultatie { get; set; }

        public MediciModel Medic { get; set; }

        public ViewChangeMessage(string newView, ConsultatieModel consultatie,MediciModel medic)
        {
            NewView = newView;
            Consultatie = consultatie;
            Medic = medic;
        }
    }

    public class EventAggregator
    {
        private static EventAggregator _instance;
        private readonly Dictionary<Type, List<Action<object>>> _handlers = new Dictionary<Type, List<Action<object>>>();

        public static EventAggregator Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new EventAggregator();
                return _instance;
            }
        }

        public void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_handlers.ContainsKey(type))
                _handlers[type] = new List<Action<object>>();

            _handlers[type].Add(obj => handler((T)obj));
        }

        public void Publish<T>(T message)
        {
            var type = typeof(T);
            if (_handlers.ContainsKey(type))
            {
                foreach (var handler in _handlers[type])
                {
                    handler(message);
                }
            }
        }
    }

}
