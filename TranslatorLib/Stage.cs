using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TranslatorLib
{
    public interface IStage<out E> where E : IStageElement
    {
        E TakeElement();

        E CurrentElement { get; }
    }

    public interface IStageElement
    {
        bool IsLast();
    }
}
