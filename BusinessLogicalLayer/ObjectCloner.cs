using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicalLayer
{
    public class ObjectCloner <T> where T:class,new()
    {
        public static T Clone(T item)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, item);
            stream.Position = 0;
            return formatter.Deserialize(stream) as T;
        }
    }
}
