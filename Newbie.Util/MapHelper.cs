using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Newbie.Util
{
    /// <summary>
    /// 计算两地点直线距离
    /// </summary>
    public class MapHelper
    {
        #region 私有变量

        string apiUrl;
        int maxRetryCount;
        List<string> akList;

        #endregion

        #region 构造函数

        private MapHelper() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="akList">百度ak列表</param>
        /// <param name="apiUrl">ip定位Api地址</param>
        /// <param name="maxRetryCount">最大尝试解析次数</param>
        public MapHelper(List<string> akList, string apiUrl, int maxRetryCount)
        {
            this.akList = akList;
            this.apiUrl = apiUrl;
            this.maxRetryCount = maxRetryCount;
        }

        #endregion

        #region 私有方法

        /// <summary>
        /// 随机取ak
        /// </summary>
        /// <returns></returns>
        private string GetMapAk()
        {
            Random r = new Random(10000);
            var akCount = akList.Count;
            return akList[r.Next() % akCount];
        }

        /// <summary>
        /// 获取IP所在地坐标（经纬度）
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public Position GetLatLongByIP(string ip,string refer="")
        {
            var url = string.Format(apiUrl, GetMapAk(), ip);
            var client = new WebClient() { Proxy = null};
            if (string.IsNullOrEmpty(refer))
            {
                client.Headers.Add("Referer", "www.huimaiche.com");
            }
            else
            {
                client.Headers.Add("Referer",refer);
            }

            var data = client.DownloadString(url);
            client.Dispose();

            JObject json = JObject.Parse(data);
            try
            {
                var x = json.SelectToken("content.point.x").ToString();
                var y = json.SelectToken("content.point.y").ToString();
                var noiseChar = new char[] { ' ', '"', '\\', '.' };

                return new Position()
                {
                    Latitude = Double.Parse(y.Trim(noiseChar)),
                    Longitude = Double.Parse(x.Trim(noiseChar))
                };
            }
            catch (Exception e)
            {
                throw new Exception("地理位置获取失败，异常信息：" + data);
            }
        }

        #endregion

        #region 公有方法

        /// <summary>
        /// 计算两地距离
        /// 公式：R*arccos(cos(lat1*π/180 )*cos(lat2*π/180)*cos(lng1*π/180 -lng2*π/180)+sin(lat1*π/180 )*sin(lat2*π/180))，其中地球半径R=6370996.81
        /// </summary>
        /// <param name="fromLongitude">起点经度</param>
        /// <param name="fromLatitude">起点纬度</param>
        /// <param name="toLongitude">终点经度</param>
        /// <param name="toLatitude">终点纬度</param>
        /// <returns></returns>
        public double GetDistance(double fromLongitude, double fromLatitude, double toLongitude, double toLatitude)
        {
            double Radius = 6370996.81; // radius of earth

            // 角度转换为弧度  
            double ew1 = fromLongitude * Math.PI / 180;
            double ns1 = fromLatitude * Math.PI / 180;
            double ew2 = toLongitude * Math.PI / 180;
            double ns2 = toLatitude * Math.PI / 180;

            // 求大圆劣弧与球心所夹的角(弧度)  
            double radian = Math.Sin(ns1) * Math.Sin(ns2) + Math.Cos(ns1) * Math.Cos(ns2) * Math.Cos(ew1 - ew2);
            if (Math.Abs(radian) > 1.0)
            {
                // 安全验证，保证取值在[-1..1]范围内 
                radian = radian / Math.Abs(radian);
            }

            // 取大圆劣弧长度  
            return Radius * Math.Acos(radian);
        }

        /// <summary>
        /// 获取IP到指定地点的距离
        /// </summary>
        /// <param name="fromLongitude">起点经度</param>
        /// <param name="fromLatitude">起点纬度</param>
        /// <param name="ip">终点IP地址</param>
        /// <returns></returns>
        public double GetDistance(double fromLongitude, double fromLatitude, string ip, string refer = "")
        {
            int tryCount = 0;
            double distance = 0;

            while (tryCount++ < maxRetryCount)
            {
                try
                {
                    var point = GetLatLongByIP(ip,refer);
                    distance = GetDistance(fromLongitude, fromLatitude, point.Longitude, point.Latitude);
                    break;
                }
                catch (Exception e)
                {
                    if (tryCount >= maxRetryCount)
                    {
                        throw e;
                    }
                }
            }

            return distance;
        }
        
        /// <summary>
        /// 计算两地点的距离
        /// </summary>
        /// <param name="from">起点坐标（经纬度）</param>
        /// <param name="to">终点坐标（经纬度）</param>
        /// <returns></returns>
        public double GetDistance(Position from, Position to)
        {
            return GetDistance(from.Longitude, from.Latitude, to.Longitude, to.Latitude);
        }

        #endregion
    }

    /// <summary>
    /// 地点坐标，用百度经纬度表示
    /// </summary>
    [Serializable]
    public class Position
    {
        double _longitude;//经度
        double _latitude;//纬度

        /// <summary>
        /// 经度
        /// </summary>
        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        /// <summary>
        /// 纬度
        /// </summary>
        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }
        /// <summary>
        /// 返回格式为：经度,纬度 ,例如：39.944651,116.335042
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Longitude.ToString() + "," + Latitude.ToString();
        }
    }
}
