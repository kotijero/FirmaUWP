using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FirmaBLL.Models
{
    public class LookupModel
    {
        private int id;
        private string value;
        private object other;
        public int ID
        {
            get { return id; }
            set
            {
                id = value;
                OnPropertyChanged();
            }
        }
        public string Value
        {
            get { return value; }
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }

        public object Other
        {
            get { return other; }
            set
            {
                this.other = value;
                OnPropertyChanged();
            }
        }

        //public event PropertyChangedEventHandler PropertyChanged;

        public override string ToString()
        {
            return Value;
        }

        public void OnPropertyChanged([CallerMemberName]string propertyName = null)
        {
            //this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

    }
}
