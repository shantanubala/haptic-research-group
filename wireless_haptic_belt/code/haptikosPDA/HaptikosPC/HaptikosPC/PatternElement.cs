using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Haptikos
{
    class PatternElement
    {
        internal string name, mtr_time_file, rhythm, magnitude, cycles;

        internal PatternElement() {

            this.name = "";
            this.mtr_time_file = "";
            this.rhythm = "";
            this.magnitude = "";
            this.cycles = "";
        }

        internal PatternElement(string name, string mtr_time_file, string rhythm,
            string magnitude, string cycles) {

            this.name = name;
            this.mtr_time_file = mtr_time_file;
            this.rhythm = rhythm;
            this.magnitude = magnitude;
            this.cycles = cycles;
        }
    }
}
