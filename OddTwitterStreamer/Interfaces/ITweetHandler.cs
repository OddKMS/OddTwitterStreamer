using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tweetinvi;
using Tweetinvi.Models;

namespace OddTwitterStreamer.Interfaces
{
    public interface ITweetHandler
    {
        void HandleTweet(ITweet tweet);
    }
}
