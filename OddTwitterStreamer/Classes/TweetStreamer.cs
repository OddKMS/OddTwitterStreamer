using OddTwitterStreamer.Interfaces;
using System;
using System.Threading.Tasks;
using Tweetinvi;
using Newtonsoft.Json;
using System.IO;
using Tweetinvi.Models;

namespace OddTwitterStreamer.Classes
{
    public class TweetStreamer : ITweetStreamer
    {

        public async Task StreamSampleTweets()
        {
            var credentials = await GetCredentialsFromFile();

            //Set Credentials for the thread (and child-threads?)
            Auth.SetCredentials(credentials);

            //NOTE: Mind the rate limit (180 searches pr 15 minutes)
            //Thus we use Tweetinvi's automatic rate limiter
            //Also preferable because it's easy and we're using async methods anyway
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            //Creates a stream that outputs 1% of all tweets worldwide (made in english) to the console.
            var tweetStream = Tweetinvi.Stream.CreateSampleStream();

            tweetStream.AddTweetLanguageFilter(LanguageFilter.English);

            tweetStream.TweetReceived += (sender, args) =>
            {
                Console.WriteLine(args.Tweet.CreatedBy.ScreenName + ": " + args.Tweet);
            };

            await tweetStream.StartStreamAsync();

            //TODO: Create separate handler, use Reacive Extensions
            //To do something fun with the tweets
            //Maybe we can read them with text-to-speech?
        }


        public async Task StreamFilteredTweets()
        {
            //Get Credentials
            var credentials = await GetCredentialsFromFile();

            //Set Credentials for the thread (and child-threads?)
            Auth.SetCredentials(credentials);

            //NOTE: Mind the rate limit (180 searches pr 15 minutes)
            //Thus we use Tweetinvi's automatic rate limiter
            //Also preferable because it's easy and we're using async methods anyway
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;

            var filteredStream = Tweetinvi.Stream.CreateFilteredStream();

            filteredStream.AddTrack("#Eurovision");
            filteredStream.AddTrack("#ESC2018");

            filteredStream.MatchingTweetReceived += (sender, args) =>
            {
                Console.WriteLine(args.Tweet.CreatedBy.ScreenName + ": " + args.Tweet);
            };

            await filteredStream.StartStreamMatchingAnyConditionAsync();
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
                var definition = new
                {
                    ApiKey = "",
                    ApiSecret = "",
                    AccessToken = "",
                    AccessTokenSecret = ""
                };

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
