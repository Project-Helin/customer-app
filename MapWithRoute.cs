using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace customerapp
{

	/**
	 * This will have their own implementation in 
	 * Android and iOS
	 */
	public class MapWithRoute : Map
	{
		public List<customerapp.Dto.Position> CalculatedRoute { get; set; }

		public List<customerapp.Dto.Position> FlownRoute { get; set; }

		public Position CurrentPosition { get; set; }

		public MapWithRoute ()
		{
			CalculatedRoute = new List<customerapp.Dto.Position> ();
			FlownRoute = new List<customerapp.Dto.Position> ();
		}

	}
}

