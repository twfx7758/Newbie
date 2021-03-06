﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Newbie.RabbitMQ
{
    public class MqConfigComFactory
    {
        static MqConfigDom _configDom = null;
        public static MqConfigDom CreateConfigDomInstance()
        {
            if (_configDom == null)
            {
                _configDom = new MqConfigDom() {
                    MqHost = ConfigurationManager.AppSettings["MqHost"],
                    MqUserName = ConfigurationManager.AppSettings["MqUserName"] ?? "guest",
                    MqPassword = ConfigurationManager.AppSettings["MqPassword"] ?? "guest",
                    MqVirtualHost = ConfigurationManager.AppSettings["MqVirtualHost"] ?? "/"
                };
            }

            return _configDom;
        }
    }
}
