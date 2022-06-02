using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aquality.Selenium.Elements.Interfaces;

namespace EuronewsSub.Utils
{
    public static class RandomListSelector
    {
        public static int SelectRandomIndex(int Values)
        {
            Random random = new Random();
            return random.Next(Values);
        }
        
        public static T RandomEnumValue<T>() where T: Enum 
        {
            Type type = typeof(T);
            Array values = type.GetEnumValues();
            int index = new Random().Next(values.Length);
            return (T)values.GetValue(index);
        } 

    }
}
