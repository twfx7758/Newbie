
using System;
using System.Collections;
using System.Collections.Generic;

namespace Newbie.Util
{
    /// <summary>
    /// 球面之间距离辅助类
    /// </summary>
    public class DistanceHelper
    {
        private static double Rad(double d)
        {
            return d * Math.PI / 180.0;
        }

        /// <summary>
        /// 根据两点之间经纬度获取距离
        /// </summary>
        /// <param name="startlat">起点纬度</param>
        /// <param name="startlng">起点经度</param>
        /// <param name="endlat">终点纬度</param>
        /// <param name="endlng">终点经度</param>
        /// <returns>距离(单位：km)</returns>
        public static double GetDistance(double startlat, double startlng, double endlat, double endlng)
        {
            double d_EarthRadius = 6378.137;//地球半径
            double radLat1 = Rad(startlat);
            double radLat2 = Rad(endlat);
            double radLat = Rad(startlat) - Rad(endlat);
            double radLng = Rad(startlng) - Rad(endlng);
            double s = 2 * Math.Asin(Math.Sqrt(Math.Pow(Math.Sin(radLat / 2), 2) +
             Math.Cos(radLat1) * Math.Cos(radLat2) * Math.Pow(Math.Sin(radLng / 2), 2)));
            s = s * d_EarthRadius;
            s = Math.Round(s * 10000) / 10000;
            return s;
        }
    }
}
