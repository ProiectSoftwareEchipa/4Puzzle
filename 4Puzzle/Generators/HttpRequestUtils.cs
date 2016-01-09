using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using System.Net;
using System.IO;

namespace _4Puzzle.Generators {
    public class HttpRequestUtils {

        const string postHighScoreUrl = "http://puzzle.win2012r2.oasis.dnnsharp.com/DesktopModules/DnnSharp/DnnApiEndpoint/Api.ashx?method=Post4Puzzle";

        const string postStatsUrl = "http://puzzle.win2012r2.oasis.dnnsharp.com/DesktopModules/DnnSharp/DnnApiEndpoint/Api.ashx?method=PostStats";

        public static string Select(string gameType) {
            string result = Task.Run(() => Get(gameType)).Result;
            return result;
        }

        static async Task<string> Get(string gameType) {
            
            string url = "http://puzzle.win2012r2.oasis.dnnsharp.com/DesktopModules/DnnSharp/DnnApiEndpoint/Api.ashx?method=Highscore&GameType=" + gameType; 
            
            try {
                //Create Client 
                var client = new HttpClient();

                var uri = new Uri(url);

                //Call. Get response by Async
                var Response = await client.GetAsync(uri);

                //Result & Code
                var statusCode = Response.StatusCode;

                //If Response is not Http 200 
                //then EnsureSuccessStatusCode will throw an exception
                Response.EnsureSuccessStatusCode();

                return await Response.Content.ReadAsStringAsync();
            } catch (Exception ex) {
                //...
                return "";
            }
        }

        public static async void InsertHighScore(string playerName, string gameType, string playerScore) {
            string[] values = new string[] { playerName, gameType, playerScore };
            HttpWebRequest myRequest = CreateHttpWebRequest(postHighScoreUrl);
            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallbackHighScore), Tuple.Create(myRequest, values));
        }

        public static async void InsertStatistics(string phoneGuid, string gameType, string dateTime) {
            string[] values = new string[] { dateTime, gameType, phoneGuid };
            HttpWebRequest myRequest = CreateHttpWebRequest(postStatsUrl);
            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallbackStats), Tuple.Create(myRequest, values));
        }

        static void GetRequestStreamCallbackHighScore(IAsyncResult callbackResult) {
            Tuple<HttpWebRequest, string[]> state = (Tuple<HttpWebRequest, string[]>)callbackResult.AsyncState;
            string playerName = state.Item2[0];
            string gameType = state.Item2[1];
            string playerScore = state.Item2[2];

            HttpWebRequest myRequest = state.Item1;
            // End the stream request operation
            Stream postStream = myRequest.EndGetRequestStream(callbackResult);

            // Create the post data
            string postData = "PlayerName=" + playerName + "&GameType=" + gameType + "&PlayerScore=" + playerScore;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Add the post data to the web request
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Dispose();

            // Start the web request
            myRequest.BeginGetResponse(new AsyncCallback(GetResponsetStreamCallback), myRequest);
        }

        static void GetRequestStreamCallbackStats(IAsyncResult callbackResult) {
            Tuple<HttpWebRequest, string[]> state = (Tuple<HttpWebRequest, string[]>)callbackResult.AsyncState;
            string dateTime = state.Item2[0];
            string gameType = state.Item2[1];
            string phoneGuid = state.Item2[2];

            HttpWebRequest myRequest = state.Item1;
            // End the stream request operation
            Stream postStream = myRequest.EndGetRequestStream(callbackResult);

            // Create the post data
            string postData = "DateTime=" + dateTime + "&GameType=" + gameType + "&PhoneGuid=" + phoneGuid;
            byte[] byteArray = Encoding.UTF8.GetBytes(postData);

            // Add the post data to the web request
            postStream.Write(byteArray, 0, byteArray.Length);
            postStream.Dispose();

            // Start the web request
            myRequest.BeginGetResponse(new AsyncCallback(GetResponsetStreamCallback), myRequest);
        }

        static void GetResponsetStreamCallback(IAsyncResult callbackResult) {
            HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
            using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream())) {
                string result = httpWebStreamReader.ReadToEnd();
            }
        }

        static HttpWebRequest CreateHttpWebRequest(string url) {
            System.Uri myUri = new System.Uri(url);
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            return myRequest;
        }
    }
}
