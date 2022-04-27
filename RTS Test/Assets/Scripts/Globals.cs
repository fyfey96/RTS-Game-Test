using System.Collections;
using System.Collections.Generic;

public class Globals
{
   public static BuildingData[] BUILDING_DATA = new BuildingData[]
   {
       new BuildingData("Building" , 100)
   };

   public static int TERRAIN_LAYER_MASK = 1 << 8;
}
