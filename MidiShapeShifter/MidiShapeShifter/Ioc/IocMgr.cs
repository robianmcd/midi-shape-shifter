using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ninject;
using MidiShapeShifter.Mss;
using MidiShapeShifter.Mss.Evaluation;

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
