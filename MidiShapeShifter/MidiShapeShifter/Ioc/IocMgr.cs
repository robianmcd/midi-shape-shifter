
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Evaluation;
using Ninject;

namespace MidiShapeShifter.Ioc
{
    public static class IocMgr
    {
        public static IKernel Kernel;

        static IocMgr()
        {
            Kernel = new StandardKernel();

            Kernel.Bind<IEvaluator>().To<Evaluator>();
            Kernel.Bind<IMssMsgProcessor>().To<MssMsgProcessor>();
        }
    }
}
