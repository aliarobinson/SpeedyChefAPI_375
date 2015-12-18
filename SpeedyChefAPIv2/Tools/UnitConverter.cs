using System;
using System.Collections.Generic;

namespace SpeedyChef
{
	public class UnitConverter
	{
		private Dictionary<string, int> unitRatioMap;

		public UnitConverter ()
		{
			unitRatioMap = new Dictionary<string, int> ();
			unitRatioMap.Add ("tsp_to_ml", 5);
			unitRatioMap.Add ("cup_to_ml", 237);
			unitRatioMap.Add ("oz_to_ml", 30);
			unitRatioMap.Add ("pint_to_ml", 237);
			unitRatioMap.Add ("quart_to_ml", 950);
			unitRatioMap.Add ("gal_to_ml", 3800);
		}

		private int convertUnit(int amount, string unitFrom, string unitTo) {
			int result;
			string key = unitFrom + "_to_" + unitTo;
			int ratio;
			if (unitRatioMap.ContainsKey (key)) {
				ratio = unitRatioMap [key];
			} else {
				Console.WriteLine ("Conversion not supported.");
				ratio = 0;
			}
			result *= ratio;
			return result;
		}
	}
}

