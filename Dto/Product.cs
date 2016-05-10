using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;
using System.ComponentModel;

namespace customerapp.Dto
{
    public class Product : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private void onPropertyChanged(object sender, string propertyName) {
            if (this.PropertyChanged != null) {
                PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
            }
        }

        public String Name{ get; set; }

        public decimal Price { get; set; }

        int amount;

        public int Amount { 
            get {return amount;} 
            set 
            { 
                amount = value;
                onPropertyChanged(this, "Amount");
            
            } 
        }
       
            

    }

}

