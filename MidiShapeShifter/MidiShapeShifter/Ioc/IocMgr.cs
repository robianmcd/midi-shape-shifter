using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Ninject;
using MidiShapeShifter.Mss;

namespace MidiShapeShifter.Ioc
{
    public static class IocMgr
    {
        public static IKernel Kernal;

        static IocMgr()
        {
            Kernal = new StandardKernel();

            Kernal.Bind<IMssEvaluator>().To<MssEvaluator>();
            Kernal.Bind<IMssMsgProcessor>().To<MssMsgProcessor>();
        }
    }
}
