using System;

namespace AdditionalAttributes
{
    namespace Internal
    {
        public abstract class ExecuteOnEachWhenScriptsReloads : Attribute
        {
            /// <summary>
            /// In which loop of the execution will this script execute.<br>
            /// Accept any kind of number.
            /// </summary>
            public int Loop { get; private set; }

            protected ExecuteOnEachWhenScriptsReloads(int loop = 0) => Loop = loop;
        }
    }
}