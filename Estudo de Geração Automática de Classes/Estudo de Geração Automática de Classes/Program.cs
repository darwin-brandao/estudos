using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

//Objetivo: Gerar classes automaticamente baseado em inputs do usuário, como Nome da Classe, Atributos, etc...

namespace Estudo_de_Geração_Automática_de_Classes
{
    class Program
    {

        static void Main(string[] args)
        {
            string sNomeClasse = "";
            List<string> lstAtributos = new List<string>();
            
            Output("Insira o Nome da Classe:");

            sNomeClasse = Input();

            Console.Clear();

            Output("Insira os Atributos (digite 'FIM' quando terminar):");

            string sInput = " ";

            while (sInput != "")
            {
                sInput = Input();

                if (sInput != "")
                    lstAtributos.Add(sInput);
            }

            Console.Clear();

            Dictionary<string, Type> dicAtributos = new Dictionary<string, Type>();

            lstAtributos.ForEach(a => dicAtributos.Add(a, typeof(string)));

            Type t = createType(sNomeClasse, dicAtributos, "TesteClasse");

            var o = Activator.CreateInstance(t);

            var oClass = Assembly.GetExecutingAssembly().GetTypes().ToList().Select(c => c.Name == sNomeClasse);

            PropertyInfo p = o.GetType().GetProperty(lstAtributos[0]);

            Output("Insira o valor de " + lstAtributos[0]);

            string valor = Input();

            p.SetValue(o, valor);

            Output("O Atributo " + lstAtributos[0] + " tem como valor: " + o.GetType().GetProperty(lstAtributos[0]).GetValue(o).ToString());

            Console.ReadKey();
            
        }

        private static string Input()
        {
            return Console.ReadLine();
        }

        private static void Output(string sMsg)
        {
            Console.WriteLine(sMsg);
        }

        //
        static Type createType(string name, IDictionary<string, Type> props, string sNomeDLL)
        {
            var csc = new CSharpCodeProvider(new Dictionary<string, string>() { { "CompilerVersion", "v4.0" } });
            var parameters = new CompilerParameters(new[] { "mscorlib.dll", "System.Core.dll" }, sNomeDLL + ".dll", false);
            parameters.GenerateExecutable = false;

            var compileUnit = new CodeCompileUnit();
            var ns = new CodeNamespace("Estudo_de_Geração_Automática_de_Classes");
            compileUnit.Namespaces.Add(ns);
            ns.Imports.Add(new CodeNamespaceImport("System"));

            var classType = new CodeTypeDeclaration(name);
            classType.Attributes = MemberAttributes.Public;
            ns.Types.Add(classType);

            foreach (var prop in props)
            {
                var fieldName = "_" + prop.Key;
                var field = new CodeMemberField(prop.Value, fieldName);
                classType.Members.Add(field);

                var property = new CodeMemberProperty();
                property.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                property.Type = new CodeTypeReference(prop.Value);
                property.Name = prop.Key;
                property.GetStatements.Add(new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName)));
                property.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName), new CodePropertySetValueReferenceExpression()));
                classType.Members.Add(property);
            }
            
            var results = csc.CompileAssemblyFromDom(parameters, compileUnit);
            results.Errors.Cast<CompilerError>().ToList().ForEach(error => Console.WriteLine(error.ErrorText));

            return results.CompiledAssembly.ExportedTypes.ToList()[0];
        }
        //

    }
}
