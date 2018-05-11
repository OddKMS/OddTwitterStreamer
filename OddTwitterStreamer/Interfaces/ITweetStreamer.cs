using System.Threading.Tasks;

namespace OddTwitterStreamer.Interfaces
{
    public interface ITweetStreamer
    {
        Task StreamSampleTweets();
        Task StreamFilteredTweets();
    }
}
