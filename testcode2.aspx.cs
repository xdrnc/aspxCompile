using System.CodeDom.Compiler;
using System.Diagnostics;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Web;
using System.Linq;
using System.Reflection;
using Sitecore.Caching;

using Sitecore.Data.Items;
using Sitecore.ContentSearch;

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


    private string preset1 = @"//cache sample
using Sitecore.Caching;
using Sitecore.Data;
using Sitecore.Globalization;
using Sitecore.Data.Items;

namespace SitecoreTestCode
{
    public class Program
    {
        public static string Main()
        {
            string output = ""item does not exist in item cache""; 
             
            string databaseName = ""master"";
            ItemCache itemCache = CacheManager.GetItemCache(Sitecore.Configuration.Factory.GetDatabase(databaseName));

            ID itemId = new ID(""0DE95AE4-41AB-4D01-9EB0-67441B7C2450"");
            Language itemLanguage = Language.Parse(""en"");
            Version itemVersion = Version.Parse(1);

            Item item = itemCache.GetItem(itemId, itemLanguage, itemVersion);
            if( item != null)
            {
                output = ""found item ("" + itemId + "", language="" + itemLanguage + "", version="" + itemVersion + "") in the ItemCache, Name=""+ item.Name +"""";

            }
             
            return output;
        }
    }
}


";


    private string preset2 = @"//index sample https://doc.sitecore.com/xp/en/developers/103/sitecore-experience-manager/linq-to-sitecore.html

using System;
using System.Linq;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;

namespace SitecoreTestCode
{
    public class Program
    {
        public static string Main()
        {
            string output = """";

            string indexName = ""sitecore_master_index"";

            using(IProviderSearchContext context = ContentSearchManager.GetIndex(indexName).CreateSearchContext())
            {
                IQueryable<SearchResultItem> searchResult = context.GetQueryable<SearchResultItem>()
                                .Where(r => r.Content.Contains(""abc""));

                foreach(SearchResultItem sr in searchResult)
                {
                    output += sr.ItemId + ""\n"";
                }
            }
             return output;
        }
    }
}

";


    private string preset3 = @"//GetItem sample
using System;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace SitecoreTestCode
{
    public class Program
    {
        public static string Main()
        {
            string output = ""item cannot be found""; 
            string databaseName = ""master""; // name of the database to check

            Database database = Sitecore.Configuration.Factory.GetDatabase(databaseName);

            Sitecore.Data.Items.Item item = database.GetItem(""sitecore/Content/Home""); //please put the item path you're checking...

            if (item != null)
            {
                // put the information from the item to the """"output""""
                output = ""item has the id: "" + item.ID;
            }
            
            return output;
        }
    }
}

";

    //load recent events and last processed timestamp
    private string preset4 = @"//event processing sample
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
            // playground #1
            string output = ""; // for the list of recent events
            string lastProcessedTimestamp = ""; // for the last processsed timestamp from Properties table



            // playground #2


            // check user must have administrator role
            if (Sitecore.Context.User.IsAdministrator)
            {
                string input = Request.Form["TextArea1"];

                TextArea1.InnerText = input;

                CodeDomProvider codeDomProvider = CodeDomProvider.CreateProvider("C#");

                CompilerParameters compParams = new CompilerParameters();

                compParams.GenerateInMemory = true;
                compParams.ReferencedAssemblies.Add("System.Web.dll");
                compParams.ReferencedAssemblies.Add("System.Core.dll");
                compParams.ReferencedAssemblies.Add("System.Configuration.dll");
                compParams.ReferencedAssemblies.Add(AppContext.BaseDirectory + "bin\\Sitecore.Kernel.dll");
                compParams.ReferencedAssemblies.Add(AppContext.BaseDirectory + "bin\\Sitecore.ContentSearch.dll");
                compParams.ReferencedAssemblies.Add(AppContext.BaseDirectory + "bin\\Sitecore.ContentSearch.Linq.dll"); 
                


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