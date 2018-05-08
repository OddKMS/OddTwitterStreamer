using Ninject;
using OddTwitterStreamer.Classes;
using OddTwitterStreamer.Interfaces;

namespace OddTwitterStreamer
{
    public class NinjectBindings
    {
        public void Load(IKernel kernel)
        {
            kernel.Bind<ITweetStreamer>().To<TweetStreamer>().InSingletonScope();
        }
    }
}
