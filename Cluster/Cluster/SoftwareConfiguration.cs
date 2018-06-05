using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
//HI
namespace Cluster
{
    class SoftwareConfiguration
    {
        private static readonly Dictionary<string, Tuple<string, List<string>>> ConfiguratonDictionary = new Dictionary<string, Tuple<string, List<string>>>()
        {
            {"form",new Tuple<string, List<string>>("1",new List<string>{"0","1"})},
            {"parentIP", new Tuple<string, List<string>>("141.218.147.125",new List<string>{"141.218.147.125"}) }
        };

        public static int Form => int.Parse(ConfiguratonDictionary["form"].Item1);
        public static string ParentIp => ConfiguratonDictionary["parentIP"].Item1;
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
