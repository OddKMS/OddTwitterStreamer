using OddTwitterStreamer.Interfaces;
using System;
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
        Subject<TweetAsync> _tweets;

        public async Task StreamSampleTweets()
        {
            await Configure();
            
            //Creates a stream that outputs 1% of all tweets worldwide (made in english) to the console.
            var tweetStream = Tweetinvi.Stream.CreateSampleStream();
            tweetStream.AddTweetLanguageFilter(LanguageFilter.Norwegian);

            tweetStream.TweetReceived += (sender, args) =>
            {
                Console.WriteLine(args.Tweet);
            };

            await tweetStream.StartStreamAsync();

            //TODO: Create separate handler, use Reacive Extensions
            //To do something fun with the tweets
            //Maybe we can read them with text-to-speech?
        }


        public async Task StreamFilteredTweets()
        {
            await Configure();

            var filteredStream = Tweetinvi.Stream.CreateFilteredStream();
            filteredStream.AddTweetLanguageFilter(LanguageFilter.Norwegian);
            filteredStream.AddTrack("Eurovision");

            filteredStream.MatchingTweetReceived += (sender, args) =>
            {
                Console.WriteLine(args.Tweet);
            };

            await filteredStream.StartStreamMatchingAnyConditionAsync();
        }

        private async Task Configure()
        {
            //Get Credentials
            var credentials = await GetCredentialsFromFile();

            //Set Credentials for the thread (and child-threads?)
            Auth.SetCredentials(credentials);

            //NOTE: Mind the rate limit (180 searches pr 15 minutes)
            //Thus we use Tweetinvi's automatic rate limiter
            //Also preferable because it's easy and we're using async methods anyway
            RateLimit.RateLimitTrackerMode = RateLimitTrackerMode.TrackAndAwait;
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
