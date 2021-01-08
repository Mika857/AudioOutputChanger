using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ChangeAudioOutput
{
    class SaveLoadManager
    {
        private const string dataFolder = "Mika";
        private const string dataFile = "audioDevices.dat";
        public static void WriteObjectToStream(Stream stream, Object data)
        {
            if (data == null)
            {
                return;
            }

            BinaryFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, data);
            stream.Close();
        }

        public static object ReadObjectFromStream(Stream stream)
        {
            BinaryFormatter formatter = new BinaryFormatter();

            object obj = formatter.Deserialize(stream);
            stream.Close();
            return obj;
        }

        public static void Save(Data data)
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + dataFolder;
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            path += "/" + dataFile;

           

            using (FileStream filestream = new FileStream(path, FileMode.Create))
            {
                WriteObjectToStream(filestream, data);
            }
        }

        public static Data Load()
        {
            string path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "/" + dataFolder + "/" + dataFile;

            if (File.Exists(path))
            {
                Data returnData;

                using (FileStream filestream = new FileStream(path, FileMode.Open))
                {
                    returnData = (Data)ReadObjectFromStream(filestream);
                }

                return returnData;
            }

            return null;
        }
    }

    [Serializable]
    public class Data
    {
        public string device1Name;
        public string device2Name;

        public Data(string device1Name, string device2Name)
        {
            this.device1Name = device1Name;
            this.device2Name = device2Name;
        }
    }
}
