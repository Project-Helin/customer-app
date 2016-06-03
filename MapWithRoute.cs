using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;
using System.ComponentModel;

namespace customerapp
{

	/**
	 * This will have their own implementation in 
	 * Android and iOS
	 */
	public class MapWithRoute : Map
	{
		public event PropertyChangedEventHandler PropertyChanged;

		private void onPropertyChanged(object sender, string propertyName) {
			if (this.PropertyChanged != null) {
				PropertyChanged(sender, new PropertyChangedEventArgs(propertyName));
			}
		}

		public List<customerapp.Dto.Position> CalculatedRoute { get; set; }

		public List<customerapp.Dto.Position> FlownRoute { get; set; }

		private customerapp.Dto.Position pos;

		public customerapp.Dto.Position CurrentPosition 
        { 
            get
            { 
                return pos; 
            } 
            set
            { 
                pos = value;
				if (value != null) {
					onPropertyChanged (this, "pos");
				}
            } 
        }

		public MapWithRoute ()
		{
			CalculatedRoute = new List<customerapp.Dto.Position> ();
			FlownRoute = new List<customerapp.Dto.Position> ();
		}

	}
}

