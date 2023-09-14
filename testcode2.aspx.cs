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
using System.Reflection;

public partial class TestCode : System.Web.UI.Page
{
    private string codeSample = @"
using System;

namespace SitecoreTestCode
{
    public class Program
    {
        public static string Main()
        {
             string output = """"; //Sitecore.Context.User.Name;
             
             
             return output;
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
            // alextest Response.Write("Sitecore.Context.User.Name = " + Sitecore.Context.User.Name);

            // check user must have administrator role
            if (Sitecore.Context.User.IsAdministrator)
            {
                string input = Request.Form["TextArea1"];

                TextArea1.InnerText = input;

                CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("C#");

                CompilerParameters compParams = new CompilerParameters();

                compParams.GenerateInMemory = true;
                compParams.ReferencedAssemblies.Add("System.Web.dll");
                compParams.ReferencedAssemblies.Add(AppContext.BaseDirectory + "bin\\Sitecore.Kernel.dll");


                CompilerResults compResults = codeDomProvider.CompileAssemblyFromSource(compParams, input);

                if (compResults.Errors.HasErrors)
                {
                    compResults.Errors.Cast<CompilerError>().ToList().ForEach(er => Response.Write(er + "\n"));
                }
                else
                {
                    Assembly assembly = compResults.CompiledAssembly;
                    Type program = assembly.GetType("SitecoreTestCode.Program");
                    MethodInfo main = program.GetMethod("Main");
                    TextArea2.InnerText = (string) main.Invoke(null, null);
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