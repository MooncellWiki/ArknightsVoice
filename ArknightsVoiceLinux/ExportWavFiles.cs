using AssetStudio;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace ArknightsVoice
{
    public class ExportWavFiles
    {
        private static string redirectFolder;
        public static void DumpAbFiles(AssetsManager asManager, Dictionary<string, string> dict, string langPath)
        {
            foreach (var assetsFile in asManager.assetsFileList)
            {
                foreach (var asset in assetsFile.Objects)
                {
                    if (asset.type != ClassIDType.AudioClip)
                    {
                        Console.WriteLine("跳过非AudioClip文件");
                        return;
                    }
                    AudioClip audioClip = (AudioClip)asset;
                    String name = audioClip.m_Name;
                    String temp = audioClip.assetsFile.originalPath;
                    String wordKey = GetFileName(temp);
                    String tempFolderName = wordKey + "_" + name + ".wav";

                    try
                    {
                        redirectFolder = dict[tempFolderName.Replace("_CN", "/CN")];
                    }
                    catch (KeyNotFoundException)
                    {
                        Console.WriteLine("对应Key不存在语音数据列表中，尝试原名输出");
                        redirectFolder = tempFolderName;
                    }
                    ExportAudioClip(audioClip, langPath + wordKey + "/", redirectFolder, langPath);
                    Console.WriteLine("已导出 " + wordKey + "_" + name + ".");
                }
            }
        }
        public static void LoadAbFiles(Dictionary<string, string> dict)
        {
            Directory.CreateDirectory("./Voice");
            Directory.CreateDirectory("./VoiceCN");
            Directory.CreateDirectory("./Export");
            Directory.CreateDirectory("./ExportCN");
            string[] files = GetAbFiles();
            AssetsManager asManager = new AssetsManager();

            asManager.LoadFiles(files);
            Console.WriteLine("Voice下语音文件已加载\n开始导出AudioClip");
            DumpAbFiles(asManager, dict, "./Export/");

            files = GetAbFiles("./VoiceCN");
            if (files.Length == 0)
            {
                Console.WriteLine("跳过VoiceCN加载");
                return;
            }
            asManager = new AssetsManager();

            asManager.LoadFiles(files);
            Console.WriteLine("VoiceCN下语音文件已加载\n开始导出AudioClip");
            DumpAbFiles(asManager, dict, "./ExportCN/");
        }
        public static string GetFileName(string input)
        {
            string fileName = "";
            foreach (Match match in Regex.Matches(input, @"char(.*).ab"))
            {
                fileName = match.Value.Replace(".ab", "");
            }
            return fileName;
        }
        public static string[] GetAbFiles(string path = "./Voice")
        {
            Directory.CreateDirectory(path);
            string[] files = Directory.GetFiles(path, "*.ab");

            return files;
        }
        public static bool ExportAudioClip(object item, string exportPath, string folderName, string langPath)
        {
            var m_AudioClip = (AudioClip)item;
            var m_AudioData = m_AudioClip.m_AudioData.GetData();
            if (m_AudioData == null || m_AudioData.Length == 0)
                return false;
            var converter = new AudioClipConverter(m_AudioClip);
            Directory.CreateDirectory(exportPath);
            var exportFullName = langPath + folderName;
            var buffer = converter.ConvertToWav();
            if (buffer == null)
                return false;
            File.WriteAllBytes(exportFullName, buffer);
            return true;
        }
    }
}