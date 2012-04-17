using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Serialization;
using System.IO;
using System.Xml;

namespace MidiShapeShifter.CSharpUtil
{
    public static class ContractSerializer
    {
        private static List<Type> knownTypes;

        static ContractSerializer()
        {
            knownTypes = new List<Type>();

            Assembly myAssembly = Assembly.GetExecutingAssembly();
            foreach (Type type in myAssembly.GetTypes())
            {
                if (!type.IsAbstract && type.IsDefined(typeof(DataContractAttribute), true))
                {
                    knownTypes.Add(type);
                }
            }
        }

        public static void Serialize<SerializableType>(Stream outStream, SerializableType objToSerialize)
        {
            DataContractSerializer serializer = new DataContractSerializer(
                    typeof(SerializableType), knownTypes, 0x7FFF, false, true, null);

            XmlWriterSettings writerSettings = new XmlWriterSettings();
            writerSettings.Indent = true;

            using (var writer = XmlWriter.Create(outStream, writerSettings))
            {
                serializer.WriteObject(writer, objToSerialize);
            }
        }

        public static SerializableType Deserialize<SerializableType>(Stream inStream)
        {
            DataContractSerializer serializer = new DataContractSerializer(
                    typeof(SerializableType), knownTypes, 0x7FFF, false, true, null);

            SerializableType deserializedRoot;
            using (var reader = XmlReader.Create(inStream))
            {
                deserializedRoot = (SerializableType)serializer.ReadObject(reader);
            }
            return deserializedRoot;
        }
    }
}
