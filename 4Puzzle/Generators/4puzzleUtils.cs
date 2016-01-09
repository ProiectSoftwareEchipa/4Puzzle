using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Windows.Security.ExchangeActiveSyncProvisioning;

namespace _4Puzzle.Generators {
    class _4puzzleUtils {

        class Score {
            public string Name { get; set; }
            public string PlayerScore { get; set; }
            public string DateTime { get; set; }
            public string GameType { get; set; }
            public string PhoneGuid { get; set; }
        }

        public static string FilterName(string name) {
            return Regex.Replace(name, "[^A-Za-z0-9 _]", "");
        }

        static List<Score> GetScoreList() {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            object scoresObject = localSettings.Values["PlayerOfflineScores"];

            List<Score> scoreList = new List<Score>();
            if (scoresObject != null) {
                scoreList = JsonConvert.DeserializeObject<List<Score>>(scoresObject.ToString());
            }
            return scoreList;
        }

        static void SaveScoreList(List<Score> scoreList) {
            Windows.Storage.ApplicationDataContainer localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values["PlayerOfflineScores"] = JsonConvert.SerializeObject(scoreList);
        }

        public static void SaveScoreOffline(string name, string score, string gameType) {
            List<Score> scoreList = GetScoreList();

            scoreList.Add(new Score() {
                Name = FilterName(name),
                PlayerScore = score,
                GameType = gameType,
                DateTime = DateTime.Now.ToString(),
                PhoneGuid = new EasClientDeviceInformation().Id.ToString()
            });

            SaveScoreList(scoreList);
        }

        public static void TrySendOfflineScore() {
            if (!(NetworkInterface.GetIsNetworkAvailable()))
                return;

            List<Score> scoreList = GetScoreList();
            foreach (Score score in scoreList) {
                if (score.Name == null)
                    HttpRequestUtils.InsertStatistics(score.PhoneGuid, score.GameType, score.DateTime);
                if (score.Name != null)
                    HttpRequestUtils.InsertHighScore(score.Name.ToString(), score.GameType, score.PlayerScore);
            }
        }
    }
}
