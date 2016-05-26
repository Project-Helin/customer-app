using System;
using System.Collections.Generic;
using customerapp.Dto;

namespace customerapp
{
	public class Route
	{
		public Double DistanceInMeters { set; get; }

		public List<Waypoint> WayPoints { set; get; }

		public Position DropPosition(){
			Waypoint found = WayPoints.Find (o => o.Action.Equals ("DROP"));
			return found.Position;
		}
	}
}

