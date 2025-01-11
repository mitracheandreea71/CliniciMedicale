using Project.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project.Commands
{
    public class ViewChangeMessage<T>
    {
        public string NewView { get; set; }
        public T Model { get; set; }

        public MediciModel Medic { get; set; }

        public ViewChangeMessage(string newView, T consultatie,MediciModel medic = null)
        {
            NewView = newView;
            Model = consultatie;
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
