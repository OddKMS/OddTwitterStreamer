using OddTwitterStreamer.Interfaces;
using System;
using Tweetinvi.Models;

namespace OddTwitterStreamer.Classes
{
    public class TweetToConsole : ITweetHandler
    {
        public void HandleTweet(ITweet tweet)
        {
            Console.WriteLine(tweet.CreatedBy.ScreenName + ": " + tweet);
        }
    }
}
