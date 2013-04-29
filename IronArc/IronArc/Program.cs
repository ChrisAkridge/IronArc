using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IronArc.Utilities;

namespace IronArc
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine(TypeConversionUtilities.ToDoubleBinary(45656).ToString());
            Console.ReadLine();
        }
    }
}
