using Nito.AsyncEx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OddTwitterStreamer
{
    class Program
    {
        static void Main(string[] args)
        {
            //async entrypoint
            AsyncContext.Run(() => MainAsync(args));
        }

        static async Task MainAsync(string[] args)
        {

        }
    }
}
