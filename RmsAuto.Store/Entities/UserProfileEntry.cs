using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;

namespace RmsAuto.Store.Entities
{
    public partial class UserProfileEntry
    {
        [Serializable]
        class NameValue
        {
            public string Name { get; set; }
            public object Value { get; set; }
        }

        public IDictionary<string, object> NameObjectMap
        {
            get
            {
                using (var stream = new MemoryStream(ProfileEntryBytes.ToArray()))
                {
                    var nvArray = (NameValue[])new BinaryFormatter().Deserialize(stream);
                    return nvArray.ToDictionary(nv => nv.Name, nv => nv.Value);
                }
            }

            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");

                using (var stream = new MemoryStream())
                {
                    new BinaryFormatter().Serialize(stream, 
                        value.Select(pair => new NameValue
                        {
                            Name = pair.Key,
                            Value = pair.Value
                        }).ToArray());
                    ProfileEntryBytes = stream.GetBuffer();
                }
            }
        }
    }
}
