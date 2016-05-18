using System;
using System.Collections.Generic;
using Xamarin.Forms.Maps;

namespace customerapp
{
	public class MapWithRoute : Map
	{
		public List<customerapp.Dto.Position> RouteCoordinates { get; set; }


		public MapWithRoute ()
		{
			RouteCoordinates = new List<customerapp.Dto.Position> ();
		}
	}
}

