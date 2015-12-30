using System;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using System.Net;
using System.IO;

namespace _4Puzzle.Generators {
    public class HttpRequestUtils {

        public static string Select(string gameType) {
            var task = Get(gameType);
            task.Wait(); // Blocks current thread until GetFooAsync task completes
            // For pedagogical use only: in general, don't do this!
            string result = task.Result;
            return task.Result;
        }

        static async Task<string> Get(string gameType) {

            string url = "http://puzzle.win2012r2.oasis.dnnsharp.com/DesktopModules/DnnSharp/DnnApiEndpoint/Api.ashx?method=Highscore&GameType=" + gameType;

            try {
                //Create Client 
                var client = new HttpClient();

                //Define URL. Replace current URL with your URL

                //Current URL is not a valid one



                var uri = new Uri(url);

                //Call. Get response by Async
                var Response = await client.GetAsync(uri);

                //Result & Code
                var statusCode = Response.StatusCode;

                //If Response is not Http 200 
                //then EnsureSuccessStatusCode will throw an exception
                Response.EnsureSuccessStatusCode();

                //Read the content of the response.
                //In here expected response is a string.
                //Accroding to Response you can change the Reading method.
                //like ReadAsStreamAsync etc..
                return await Response.Content.ReadAsStringAsync();
            } catch (Exception ex) {
                //...
                return "";
            }
        }

        public static async void Insert(string playerName, string gameType, string playerScore) {

            var url = "http://puzzle.win2012r2.oasis.dnnsharp.com/DesktopModules/DnnSharp/DnnApiEndpoint/Api.ashx?method=Post4Puzzle";
            string[] values = new string[] { playerName, gameType, playerScore };

            System.Uri myUri = new System.Uri(url);
            HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(myUri);
            myRequest.Method = "POST";
            myRequest.ContentType = "application/x-www-form-urlencoded";
            myRequest.BeginGetRequestStream(new AsyncCallback(GetRequestStreamCallback), Tuple.Create(myRequest, values));
        }

        static void GetRequestStreamCallback(IAsyncResult callbackResult) {
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

        static void GetResponsetStreamCallback(IAsyncResult callbackResult) {
            HttpWebRequest request = (HttpWebRequest)callbackResult.AsyncState;
            HttpWebResponse response = (HttpWebResponse)request.EndGetResponse(callbackResult);
            using (StreamReader httpWebStreamReader = new StreamReader(response.GetResponseStream())) {
                string result = httpWebStreamReader.ReadToEnd();
            }
        }

    }
}
