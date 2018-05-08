using System;
using System.Threading.Tasks;
using Nito.AsyncEx;
using Ninject;
using System.Globalization;
using OddTwitterStreamer.Classes;
using OddTwitterStreamer.Interfaces;

namespace OddTwitterStreamer
{
    class Program
    {
        static IKernel _kernel;
        static ITweetStreamer _tweetStreamer;

        static void Main(string[] args)
        {
            //async entrypoint
            AsyncContext.Run(() => MainAsync(args));
            Console.ReadLine();
        }

        static async Task MainAsync(string[] args)
        {
            //Safety measure, you mess up decimal annotations only once.
            CultureInfo.DefaultThreadCurrentCulture = new CultureInfo("NO");

            _kernel = new StandardKernel();
            var bindings = new NinjectBindings();
            bindings.Load(_kernel);

            //Main class for the program, as I'm partial to doing as little as possible in Main()
            _tweetStreamer = _kernel.Get<TweetStreamer>();

            //Kicks off the business logic
            await _tweetStreamer.StreamTweets();

        }
    }
}
