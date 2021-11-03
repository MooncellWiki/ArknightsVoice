using System.Collections.Generic;

namespace ArknightsVoice
{
    class Program
    {
        static void Main(string[] args)
        {
            Dictionary<string, string> dict = VoiceDataReader.ParseVoiceData();
            ExportWavFiles.LoadAbFiles(dict);
        }
    }
}
