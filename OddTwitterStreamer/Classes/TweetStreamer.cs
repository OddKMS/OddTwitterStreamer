using OddTwitterStreamer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Newtonsoft.Json;
using System.IO;

namespace OddTwitterStreamer.Classes
{
    public class TweetStreamer : ITweetStreamer
    {
        

        public async Task StreamTweets()
        {
            //Step 1: Get Tweets
            await Configure();

            //Step 2: Do something with the tweets
            //"Store"/publish to a repo
            //Maybe we can read them with text-to-speech?
        }

        /// <summary>
        /// Sets up credentials and the API key
        /// </summary>
        private async Task Configure()
        {
            using (StreamReader jsonFile = File.OpenText(@"key.secure.json"))
            {
                var jsonString = await jsonFile.ReadToEndAsync();
                var definition = new { Key = "", Secret = "" };

                var secretJsonContent = JsonConvert.DeserializeAnonymousType(jsonString, definition);

                var key = secretJsonContent.Key;
                var secret = secretJsonContent.Secret;

                Auth.SetApplicationOnlyCredentials(key, secret);
            }
        }
    }
}
