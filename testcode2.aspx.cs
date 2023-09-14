using System.CodeDom.Compiler;
using System.Diagnostics;
using Microsoft.CSharp;
using Sitecore.Data;
using Sitecore.Diagnostics;
using Sitecore.Web.UI.Controls.Common.TextAreas;
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;

public partial class TestCode : System.Web.UI.Page
{
    private string codeSample = @"
using System;

namespace SitecoreTestCode
{
    public class Program
    {
        public static void Main()
        {
            Console.WriteLine(""sample code"");
        }
    }
}
    ";

    //search in cache values (e.g. problem like content not updated)
    private string preset1 = @"//cache sample";

    //search in content index
    private string preset2 = @"//index sample";

    //get item request
    private string preset3 = @"//GetItem sample";

    //load recent events and last processed timestamp
    private string preset4 = @"//event processing sample";

    protected void Page_Load(object sender, EventArgs e)
    {
        TextArea1.InnerText = codeSample;
    }

    protected void preset1_Click(object sender, EventArgs e)
    {
        TextArea1.InnerText = preset1;
    }

    protected void preset2_Click(object sender, EventArgs e)
    {
        TextArea1.InnerText = preset2;
    }

    protected void preset3_Click(object sender, EventArgs e)
    {
        TextArea1.InnerText = preset3;
    }

    protected void preset4_Click(object sender, EventArgs e)
    {
        TextArea1.InnerText = preset4;
    }

    //    protected string Result { get; set; }


    protected void execute_Click(object sender, EventArgs e)
    {
        try
        {
            // check user must have administrator role
            if (Sitecore.Context.User.IsAdministrator)
            {
                string input = Request.Form["TextArea1"];

                TextArea1.InnerText = input;

                string result = "";

                CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("C#");

//                CSharpCodeProvider csp = new CSharpCodeProvider();

//                ICodeCompiler icc = csp.CreateCompiler();

                CompilerParameters compParams = new CompilerParameters();
//                compParams.GenerateExecutable = true;
                compParams.GenerateInMemory = true;
                compParams.ReferencedAssemblies.Add("System.Web.dll");

                //                string outputLocation = AppContext.BaseDirectory + "out.exe";
                //                compParams.OutputAssembly = outputLocation;
                //compParams.ReferencedAssemblies.Add(AppContext.BaseDirectory + "bin\\Sitecore.Kernela.dll");

                //Response.Write(AppContext.BaseDirectory + @"bin\Sitecore.Kernel.dll");


                //CompilerResults compResults = icc.CompileAssemblyFromSource(compParams, input);
                CompilerResults compResults = codeDomProvider.CompileAssemblyFromSource(compParams, input);

                if (compResults.Errors.HasErrors)
                {
                    compResults.Errors.Cast<CompilerError>().ToList().ForEach(er => Response.Write(er + "\n"));
                }
                else
                {
                    /*                    var process = new Process
                                        {
                                            StartInfo = new ProcessStartInfo
                                            {
                                                FileName = outputLocation,
                                                Arguments = "command line arguments to your executable",
                                                UseShellExecute = false,
                                                RedirectStandardOutput = true,
                                                CreateNoWindow = true
                                            }
                                        };

                                        process.Start();
                                        while (!process.StandardOutput.EndOfStream)
                                        {
                                            result = process.StandardOutput.ReadLine();
                                        };
                    */
                    //TextArea2.InnerText = result;

                    foreach (string stroutput in compResults.Output)
                    {
                        result += stroutput;
                    }

                    TextArea2.InnerText = result;

                }
            }
            else
            {
                Response.Write("Only administrator user can execute this");
            }
        }
        catch(Exception ex) 
        {
           Response.Write(ex);
        }
    }
}