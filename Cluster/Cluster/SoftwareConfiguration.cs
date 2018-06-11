using System;
using System.Collections.Generic;
using System.Net;

namespace Cluster
{
    internal class SoftwareConfiguration
    {
        private static readonly Dictionary<string, Tuple<string, List<string>>> ConfiguratonDictionary = new Dictionary<string, Tuple<string, List<string>>>()
        {
            {"form",new Tuple<string, List<string>>("1",new List<string>{"0","1"})},
            {"parentIP", new Tuple<string, List<string>>("10.80.147.211",new List<string>{"141.218.147.125","10.80.147.211"}) },
            {"dchunk", new Tuple<string, List<string>>("1000000",new List<string>{"100000","1000000","10000000","100000000"}) }
        };

        public static int Form => int.Parse(ConfiguratonDictionary["form"].Item1);
        public static IPAddress ParentIp => IPAddress.Parse(ConfiguratonDictionary["parentIP"].Item1);
        public static int DChunk => int.Parse(ConfiguratonDictionary["dchunk"].Item1);
        public static int SetValue(string key, string value)
        {
            //No key found in possible config options
            if (!ConfiguratonDictionary.ContainsKey(key)) return 2;
            ConfiguratonDictionary.TryGetValue(key, out Tuple<string, List<string>> configTuple);

            //Invalid or no value found for key
            if (configTuple == null || !configTuple.Item2.Contains(value)) return 1;
            var newConfigTuple = new Tuple<string, List<string>>(value, configTuple.Item2);

            ConfiguratonDictionary[key] = newConfigTuple;

            return 0;
        }
    }
}
