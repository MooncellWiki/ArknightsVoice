using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace ArknightsVoice
{
    public class VoiceDataReader
    {
        public static Dictionary<string, string> ParseVoiceData()
        {
            string character = File.ReadAllText("./character_table.json");
            string charword = File.ReadAllText("./charword_table.json");

            CharwordTable jo = JsonConvert.DeserializeObject<CharwordTable>(charword);
            Dictionary<string, CharacterTable> jo2 = JsonConvert.DeserializeObject<Dictionary<string, CharacterTable>>(character);

            Dictionary<string, string> kv = new Dictionary<string, string>();
            Dictionary<string, string> kv2 = new Dictionary<string, string>();

            foreach (KeyValuePair<string, CharacterTable> kv3 in jo2)
            {
                string name = kv3.Value.name;
                string charId = kv3.Key;
                kv2[charId] = name;
            }

            foreach (KeyValuePair<string, Charwords> kv1 in jo.charWords)
            {
                string wordKey = kv1.Value.wordKey;
                string charName = kv2[kv1.Value.charId];
                string voiceId = kv1.Value.voiceId;
                string voiceTitle = kv1.Value.voiceTitle;
                kv[wordKey + "/" + voiceId + ".wav"] = wordKey + "/" + charName + "_" + voiceTitle + ".wav";
            }
            //Debug.WriteLine(kv);
            Console.WriteLine("语音数据已加载");
            return kv;
        }
    }

    class CharwordTable
    {
        public Dictionary<string, Charwords> charWords { get; set; }
    }
    class Charwords
    {
        public string charWordId { get; set; }
        public string wordKey { get; set; }
        public string charId { get; set; }
        public string voiceId { get; set; }
        public string voiceText { get; set; }
        public string voiceTitle { get; set; }
        public string voiceIndex { get; set; }
        public string voiceType { get; set; }
        public string unlockType { get; set; }
        public List<Object> unlockParam { get; set; }
        public string lockDescription { get; set; }
        public string placeType { get; set; }
        public string voiceAsset { get; set; }
    }
    class CharacterTable
    {
        public string name { get; set; }
        public string description { get; set; }
        public string canUseGeneralPotentialItem { get; set; }
        public string potentialItemId { get; set; }
        public string team { get; set; }
        public string displayNumber { get; set; }
        public string tokenKey { get; set; }
        public string appellation { get; set; }
        public string position { get; set; }
        public List<Object> tagList { get; set; }
        public string displayLogo { get; set; }
        public string itemUsage { get; set; }
        public string itemDesc { get; set; }
        public string itemObtainApproach { get; set; }
        public string maxPotentialLevel { get; set; }
        public string rarity { get; set; }
        public string profession { get; set; }
        public object trait { get; set; }
        public List<Object> phases { get; set; }
        public List<Object> skills { get; set; }
        public List<Object> talents { get; set; }
        public List<Object> potentialRanks { get; set; }
        public List<Object> favorKeyFrames { get; set; }
        public List<Object> allSkillLvlup { get; set; }
    }
}
