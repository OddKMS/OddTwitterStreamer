using OddTwitterStreamer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Newtonsoft.Json;
using System.IO;
using Tweetinvi.Models;
using System.Reactive.Subjects;

namespace OddTwitterStreamer.Classes
{
    public class TweetStreamer : ITweetStreamer
    {
        public async Task StreamTweets()
        {
            //Step 1: 
            //Get Credentials
            var credentials = await GetCredentialsFromFile();

            //Step 2: 
            //Set Credentials for the thread (and child-threads?)
            Auth.SetCredentials(credentials);

            //NOTE: Mind the rate limit (180 searches pr 15 minutes)
            //Thus we use Tweetinvi's automatic rate limiter
            //Also preferable because it's easy and we're using async methods anyway
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            //For the purpose of this program we will not be getting authentication from any user
            //in order to retrieve tweets, but will use my test account's user credentials

            //Gets the user form current thread credentials
            var authenticatedUser = User.GetAuthenticatedUser();

            //Step 3:
            //Creates a stream that outputs 1% of all tweets worldwide (made in english) to the console.
            var stream = Tweetinvi.Stream.CreateSampleStream(credentials);
            stream.AddTweetLanguageFilter(LanguageFilter.English);

            stream.TweetReceived += (sender, args) =>
            {
                Console.WriteLine(args.Tweet);
            };

            await stream.StartStreamAsync();

            //Step 2: Do something with the tweets
            //Maybe we can read them with text-to-speech?
        }

        /// <summary>
        /// Gets credentials from file and returns an ITwitterCredentials object
        /// </summary>
        /// <returns>
        /// ITwitterCredentials object
        /// </returns>
        private async Task<ITwitterCredentials> GetCredentialsFromFile()
        {
            using (StreamReader jsonFile = File.OpenText(@"key.secure.json"))
            {
                var jsonString = await jsonFile.ReadToEndAsync();
                var definition = new { ApiKey = "", ApiSecret = "",
                                        AccessToken = "", AccessTokenSecret ="" };

                var jsonContent = JsonConvert.DeserializeAnonymousType(jsonString, definition);

                var key = jsonContent.ApiKey;
                var secret = jsonContent.ApiSecret;
                var token = jsonContent.AccessToken;
                var tokenSecret = jsonContent.AccessTokenSecret;

                return Auth.CreateCredentials(key, secret, token, tokenSecret);
            }
        }
    }
}
