using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IsDrawInfinity.Scripts.Data
{
    internal class DataSet
    {
        private List<DataPoint> _dataSet = new List<DataPoint>();

        private static readonly string TestPath = Directory.GetCurrentDirectory() + @"\data.json";

        private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        internal void Add(DataPoint dataPoint)
        {
            _dataSet.Add(dataPoint);
            File.WriteAllText(TestPath, JsonConvert.SerializeObject(_dataSet, Settings));
        }
        internal string GetElement(int id) => _dataSet.Count.ToString();

        internal void LoadFile()
        {
            if (!File.Exists(TestPath))
            {
                _dataSet = new List<DataPoint>();
            }
            else
            {
                _dataSet = JsonConvert.DeserializeObject<List<DataPoint>>(File.ReadAllText(TestPath));
            }
        }

        internal string Predict(double[] vector, Func<double[], double[], double> distanceFunction)
        {
            double smallestDistance = Double.MaxValue;
            int min = int.MaxValue;
            for (int i = 0; i < _dataSet.Count; i++)
            {
                var distance = distanceFunction(_dataSet[i].Point, vector);
                if (distance < smallestDistance)
                {
                    smallestDistance = distance;
                    min = i;
                }
            }
            return _dataSet[min].Label;
        }
    }
}