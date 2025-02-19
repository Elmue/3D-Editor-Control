
/*****************************************************************************

This class has been written by Łukasz Światkowski
Cleaned up spaghetti code and improved by Elmü (elmue@gmx.de)

*****************************************************************************/

using System;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using Microsoft.CSharp;

using delRendererFunction = Plot3D.Editor3D.delRendererFunction;

namespace Plot3D
{
    public delegate double delCompiledFunction(params double[] x);

    public static class FunctionCompiler
    {
        const String EVAL_CLASS  = 
              "using {1};\n"
            + "public class Eval\n"
            + "{{\n"
            + "	public static double e  {{ get {{ return System.Math.E;  }}  }}\n"
            + "	public static double pi {{ get {{ return System.Math.PI; }}  }}\n"
            // -------------------------------------------------------
            + "	public static double abs  (double x)           {{ return System.Math.Abs(x);      }}\n"
            + "	public static double acos (double x)           {{ return System.Math.Acos(x);     }}\n"
            + "	public static double asin (double x)           {{ return System.Math.Asin(x);     }}\n"
            + "	public static double atan (double x)           {{ return System.Math.Atan(x);     }}\n"
            + "	public static double atan2(double x, double y) {{ return System.Math.Atan2(x, y); }}\n"
            + "	public static double ceil (double x)           {{ return System.Math.Ceiling(x);  }}\n"
            + "	public static double cos  (double x)           {{ return System.Math.Cos(x);      }}\n"
            + "	public static double cosh (double x)           {{ return System.Math.Cosh(x);     }}\n"
            + "	public static double exp  (double x)           {{ return System.Math.Exp(x);      }}\n"
            + "	public static double floor(double x)           {{ return System.Math.Floor(x);    }}\n"
            + "	public static double log  (double x)           {{ return System.Math.Log(x);      }}\n"
            + "	public static double log2 (double x)           {{ return System.Math.Log(x, 2.0); }}\n"
            + "	public static double log10(double x)           {{ return System.Math.Log10(x);    }}\n"
            + "	public static double max  (double x, double y) {{ return System.Math.Max(x, y);   }}\n"
            + "	public static double min  (double x, double y) {{ return System.Math.Min(x, y);   }}\n"
            + "	public static double pow  (double x, double y) {{ return System.Math.Pow(x, y);   }}\n"
            + "	public static double round(double x)           {{ return System.Math.Round(x);    }}\n"
            + "	public static double sign (double x)           {{ return System.Math.Sign(x);     }}\n"
            + "	public static double sin  (double x)           {{ return System.Math.Sin(x);      }}\n"
            + "	public static double sinh (double x)           {{ return System.Math.Sinh(x);     }}\n"
            + "	public static double sqrt (double x)           {{ return System.Math.Sqrt(x);     }}\n"
            + "	public static double tan  (double x)           {{ return System.Math.Tan(x);      }}\n"
            + "	public static double tanh (double x)           {{ return System.Math.Tanh(x);     }}\n"
            // -------------------------------------------------------
            + "	public static double __eval(params double[] __X)\n"
            + "	{{\n"
            + "		double x = __X[0];\n"
            + "		double y = __X[1];\n"
            + "		return {0};\n"
            + "	}}\n"
            + "	public static {2} __get()\n"
            + "	{{\n"
            + "		return __eval;\n"
            + "	}}\n"
            + "}}";

        public static delRendererFunction Compile(string functionBody)
        {
            functionBody = functionBody.Trim().ToLower();
            if (functionBody.Contains(";"))
                throw new Exception("Function string cannot contain semicolon");

            string s_Class = string.Format(EVAL_CLASS, functionBody, typeof(delCompiledFunction).Namespace, typeof(delCompiledFunction).Name);

            CSharpCodeProvider i_Provider = new CSharpCodeProvider();
            CompilerParameters i_Params   = new CompilerParameters();
            i_Params.CompilerOptions      = "/t:library";
            i_Params.GenerateInMemory     = true;
            i_Params.ReferencedAssemblies.Add("mscorlib.dll");
            i_Params.ReferencedAssemblies.Add("System.dll");
            i_Params.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);

            CompilerResults i_Result = i_Provider.CompileAssemblyFromSource(i_Params, s_Class);
            if (i_Result.Errors.HasErrors)
            {
                StringBuilder s_Error = new StringBuilder();
                if (i_Result.Errors.Count == 1)
                    s_Error.Append("Compilation error:\n");
                else
                    s_Error.AppendFormat("{0} Compilation errors:\n", i_Result.Errors.Count);

                foreach (CompilerError i_Error in i_Result.Errors)
                {
                    s_Error.Append(i_Error.ErrorText);
                    s_Error.Append("\n");
                }

                s_Error.Append("\nSupported math functions are:\n"
                             + "e, pi, abs(), acos(), asin(), atan(), atan2(), ceil(), cos(), cosh(), "
                             + "exp(), floor(), log(), log2(), log10(), max(), min(), pow(), "
                             + "round(), sign(), sin(), sinh(), sqrt(), tan(), tanh()\n");

                throw new Exception(s_Error.ToString());
            }

            MethodInfo          i_Method   = i_Result.CompiledAssembly.GetType("Eval").GetMethod("__get");
            delCompiledFunction f_Compiled = (delCompiledFunction)i_Method.Invoke(null, null);
            delRendererFunction f_Render   = delegate(double X, double Y)
            {
                return f_Compiled(X, Y);
            };
            return f_Render;
        }
    }
}
