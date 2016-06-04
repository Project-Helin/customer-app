using System;
using customerapp.Dto;
using System.Collections.Generic;

namespace customerapp
{
	public class GpsState
	{
        public double posLat{ get; set; }

        public double posLon{ get; set; }

        public Position AsPosition {
            get{ 
                return new Position {
                    Lat = posLat,
                    Lon = posLon
                };
            }
        }
    }
}

