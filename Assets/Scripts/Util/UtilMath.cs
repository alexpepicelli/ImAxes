using UnityEngine;
using System.Collections;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System;
using System.Xml.Linq;
using Staxes;

public class UtilMath {

	//constants
    public static float FL_TO_M = 0.3048f/2f;

	//Unity max vertices count per mesh
	public static int MAXIMUM_VERTICES_COUNT = 65534; 

	// scale data between 2 spaces
	public static float NormaliseValue(float value, float inputMin, float inputMax, float outputMin, float outputMax)
	{
		float inputRange = inputMax - inputMin;
		float outputRange = outputMax - outputMin;
		float normalised = outputRange / inputRange;
		return (outputMin - (normalised * inputMin) + (normalised * value));
	}

	public static float animateSlowInSlowOut(float t)
	{
		if (t <= 0.5f)
			return 2.0f * t * t;

		else
			return 1.0f - 2.0f * (1.0f - t) * (1.0f - t);            
	}

	//format fileName : @"C:\Users\maxc\Documents\Maxime\DATA FOR VISUALISATION\TEST.BIN"
	public static void SerializeVector3(Vector3[] data, string fileName)
	{
		using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
		{
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, data);
		}
	}

	//format fileName : @"C:\Users\maxc\Documents\Maxime\DATA FOR VISUALISATION\TEST.BIN"
	public static Vector3[] DeserializeVector3(string fileName)
	{
		using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
		{

			BinaryFormatter bf = new BinaryFormatter();
			Vector3[] result = (Vector3[])bf.Deserialize(fs);		
			
			return result;
		}
	}

	//format fileName : @"C:\Users\maxc\Documents\Maxime\DATA FOR VISUALISATION\TEST.BIN"
	public static void SerializeInt(int[] data, string fileName)
	{
		using (FileStream fs = new FileStream(fileName, FileMode.Create, FileAccess.ReadWrite))
		{
			BinaryFormatter bf = new BinaryFormatter();
			bf.Serialize(fs, data);
		}
	}
	
	//format fileName : @"C:\Users\maxc\Documents\Maxime\DATA FOR VISUALISATION\TEST.BIN"
	public static int[] DeserializeInt(string fileName)
	{
		using (FileStream fs = new FileStream(fileName, FileMode.Open, FileAccess.Read))
		{
			
			BinaryFormatter bf = new BinaryFormatter();
			int[] result = (int[])bf.Deserialize(fs);		
			
			return result;
		}
	}

	/// <summary>
	/// Projects a point on sphere.
	/// </summary>
	/// <returns>The on sphere.</returns>
	/// <param name="center">Center.</param>
	/// <param name="radius">Radius.</param>
	/// <param name="x">The x coordinate.</param>
	/// <param name="y">The y coordinate.</param>
    /// 
	public static Vector3 projectOnSphere (Vector3 center, float radius, float x, float y)
	{
		float theta = 2f * Mathf.PI * x;
		float phi = Mathf.Acos(2f * y - 1f);
		
		float xS = center.x + (radius * Mathf.Sin(theta) * Mathf.Cos(phi));
		float yS = center.y + (radius * Mathf.Sin(phi) * Mathf.Sin(theta));
		float zS = ( center.z + (radius * Mathf.Cos(theta)));
		
		return new Vector3(xS,yS,zS);
	}

    public static Vector3 GPS_to_Spherical(Vector3 center, float _lat, float _lon, float earthRadius, float altitude)
    {

      //excentricity correction...
      // % WGS84 ellipsoid constants:
      //  a = 6378137;
      // e = 8.1819190842622e-2;

      // % intermediate calculation
      //% (prime vertical radius of curvature)
      //N = a ./ sqrt(1 - e^2 .* sin(lat).^2);

      /*  Vector3 spherical = new Vector3(((_lat) * Mathf.Deg2Rad), (_lon * Mathf.Deg2Rad - Mathf.PI / 2f), earthRadius);
          float xS = (earthRadius + altitude) * Mathf.Sin(spherical.x) * Mathf.Cos(spherical.y);
          float yS = (earthRadius + altitude) * Mathf.Sin(spherical.x) * Mathf.Sin(spherical.y);
          float zS = (earthRadius + altitude) * Mathf.Cos(spherical.x);
      */

        //Vector3 spherical = new Vector3(((_lat) * Mathf.Deg2Rad), (_lon * Mathf.Deg2Rad - Mathf.PI / 2f), earthRadius);
        var lat = Mathf.Deg2Rad*_lat;
        var lon = Mathf.Deg2Rad*_lon;

        float xS = (earthRadius + altitude) * Mathf.Cos(lat) * Mathf.Cos(lon);
        float yS = (earthRadius + altitude) * Mathf.Cos(lat) * Mathf.Sin(lon);
        float zS = (earthRadius + altitude) * Mathf.Sin(lat);

       /* float xS = (earthRadius + altitude) * Mathf.Cos(spherical.y) * Mathf.Sin(spherical.x);
        float yS = (earthRadius + altitude) * Mathf.Sin(spherical.y) * Mathf.Sin(spherical.x);
        float zS = (earthRadius + altitude) * Mathf.Cos(spherical.x);*/

       // Vector3 p;
        
        return new Vector3(xS,yS,zS);

    }

    public static float[] diffArray(float[] from, float[] to )
    {
        List<float> diff_ = new List<float>();

        for (int i = 0; i < from.Length; i++)
            diff_.Add(from[i] - to[i]);

        return diff_.ToArray();
    }

    public static string printPositionCSV(Vector3 position, int precision)
    {
        return position.x.ToString("G" + precision) + "," + position.y.ToString("G" + precision) + "," + position.z.ToString("G" + precision);
    }

    public static string printRotationCSV(Quaternion rotation, int precision)
    {
        return rotation.x.ToString("G" + precision) + "," + rotation.y.ToString("G" + precision) + "," + rotation.z.ToString("G" + precision) + "," + rotation.w.ToString("G" + precision);
    }

    public static float ClosestTo(List<float> collection, float target)
    {
        float closest_value = collection[0];
        float subtract_result = Math.Abs(closest_value - target);
        for (int i = 1; i < collection.Count; i++)
        {
            if (Math.Abs(collection[i] - target) < subtract_result)
            {
                subtract_result = Math.Abs(collection[i] - target);
                closest_value = collection[i];
            }
        }
        return closest_value;
    }

}