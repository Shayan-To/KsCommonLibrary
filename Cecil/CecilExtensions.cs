using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;

namespace Ks.Cecil
{
    public static class CecilExtensions
    {
        public static IEnumerable<System.Reflection.Assembly> GetRecursiveReferencedAssembliesUsingCecil(this System.Reflection.Assembly Assembly)
        {
            var Helper = CecilHelper.Instance;
            return Helper.GetRawRecursiveReferencedAssemblies(Helper.Convert(Assembly)).Select(A => Helper.Convert(A));
        }

        public static IEnumerable<System.Reflection.Assembly> GetAllReferencedAssembliesUsingCecil(this System.Reflection.Assembly Assembly)
        {
            var Helper = CecilHelper.Instance;
            return Helper.GetRawReferencedAssemblyNames(Helper.Convert(Assembly)).Select(A => Helper.Convert(A));
        }
    }
}
