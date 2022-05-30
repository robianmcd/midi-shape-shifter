using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
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
            byte[] inBytes = new byte[inStream.Length];
            inStream.Read(inBytes, 0, (int)inStream.Length);

            int lastValidByteIndex = inBytes.Length - 1;
            //Sometimes during serialization other programs will round the buffer size up to the 
            //next power of two and fill in all the extra space with null bytes. The XML reader
            //Cannot handel these null bytes so we need to create a stream that does not include
            //them.
            while (inBytes[lastValidByteIndex] == 0x00 && lastValidByteIndex > 0)
            {
                lastValidByteIndex--;
            }
            Stream inStreamWithoutNull = new MemoryStream(inBytes, 0, lastValidByteIndex + 1);

            DataContractSerializer serializer = new DataContractSerializer(
                    typeof(SerializableType), knownTypes, 0x7FFF, false, true, null);

            SerializableType deserializedRoot;
            using (var reader = XmlReader.Create(inStreamWithoutNull))
            {
                deserializedRoot = (SerializableType)serializer.ReadObject(reader);
            }
            return deserializedRoot;
        }
    }
}
