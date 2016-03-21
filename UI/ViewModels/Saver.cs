using System.ComponentModel;
using Common;

namespace UI.ViewModels
{
    public interface ISaver
    {
        void Save(string name, decimal value);
    }

    public class Saver : ISaver
    {
        private readonly ISettings settings;

        public Saver(ISettings settings)
        {
            this.settings = settings;
        }

        public void Save(string name, decimal value)
        {
            settings.Save(name, value);
        }
    }
}