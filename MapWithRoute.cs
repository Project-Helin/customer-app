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
		public List<customerapp.Dto.Position> RouteCoordinates { get; set; }

		public MapWithRoute ()
		{
			RouteCoordinates = new List<customerapp.Dto.Position> ();
		}

	}
}

