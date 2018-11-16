using DocumentationModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DocumentationGenerator
{
    public static class Extensions
    {
        public static ClassDeclarationSyntax hello;
        public const string NESTED_CLASS_DELIMITER = ".";
        public const string NAMESPACE_CLASS_DELIMITER = ".";

        public static string GetFullName(this ClassDeclarationSyntax sourceNode)
        {
            var name = sourceNode.Identifier.Text;
            var parent = sourceNode.Parent;
            while (parent is ClassDeclarationSyntax parentClass)
            {
                name = parentClass.Identifier.Text + NESTED_CLASS_DELIMITER + name;
                parent = parent.Parent;
            }

            var nameSpace = parent as NamespaceDeclarationSyntax;

            name = nameSpace.Name + NAMESPACE_CLASS_DELIMITER + name;

            return name;
        }

        public static string GetFullName(this BaseTypeDeclarationSyntax node)
        {
            if (node is ClassDeclarationSyntax classNode)
            {
                return classNode.GetFullName();
            }

            if (node.Parent is NamespaceDeclarationSyntax  nameSpace)
            {
                return nameSpace.Name + "." + node.Identifier.Text;
            }

            return node.Identifier.Text;
        }

        public static string GetTypeName(this ITypeSymbol s)
        {
            if (s is IArrayTypeSymbol arrayType)
            {
                return arrayType.ElementType.Name + "[]";
            }

            return s.Name;
        }

        public static string GetFullName(this ISymbol s)
        {
            if (s == null || IsRootNamespace(s))
            {
                return string.Empty;
            }
            var sb = new StringBuilder();

            if(s is IArrayTypeSymbol arrayType)
            {
                sb.Insert(0, "[]");
                s = arrayType.ElementType;
            }
            sb.Insert(0, s.Name);

            var last = s;

            s = s.ContainingSymbol;

            while (!IsRootNamespace(s))
            {
                if (s is ITypeSymbol && last is ITypeSymbol)
                {
                    sb.Insert(0, '+');
                }
                else
                {
                    sb.Insert(0, '.');
                }

                //sb.Insert(0, s.OriginalDefinition.ToDisplayString(SymbolDisplayFormat.MinimallyQualifiedFormat));
                sb.Insert(0, s.Name);
                s = s.ContainingSymbol;
            }

            return sb.ToString();
        }

        private static bool IsRootNamespace(ISymbol symbol)
        {
            return symbol is INamespaceSymbol ns && ns.IsGlobalNamespace;
        }

        public static T GetParentSyntax<T>(this SyntaxNode syntaxNode) where T : SyntaxNode
        {

            if (syntaxNode == null) { return null; }

            syntaxNode = syntaxNode.Parent;

            if (syntaxNode == null) { return null; }

            if (syntaxNode.GetType() == typeof(T))
            {
                return syntaxNode as T;
            }

            return GetParentSyntax<T>(syntaxNode);
        }

        public static string TrimMultiLine(this string str) {
            return string.Join(Environment.NewLine, str.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None).Select(l => l.Trim()));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="xmlString"></param>
        /// <returns></returns>
        public static DocumentationComment ParseDocumentationComment(string xmlString)
        {
            if (string.IsNullOrWhiteSpace(xmlString)) { return null; }
            var xml = XElement.Parse(xmlString);
            var comment = new DocumentationComment
            {
                Summary = xml.Descendants("summary").FirstOrDefault()?.Value.Trim('\n').Trim('\r').TrimMultiLine(),
                Returns = xml.Descendants("returns").FirstOrDefault()?.Value,
                Parameters = new List<ParameterComment>()
            };

            var xmlParameters = xml.Descendants("param");
            foreach (var xmlParam in xmlParameters)
            {
                var param = new ParameterComment { Name = xmlParam.Attribute("name").Value, Description = xmlParam.Value };
                comment.Parameters.Add(param);
            }
            

            return comment;
        }

        public static bool IsPublic(this BaseFieldDeclarationSyntax node) { return node.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)); }
        public static bool IsPublic(this BasePropertyDeclarationSyntax node) { return node.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)); }
        public static bool IsPublic(this BaseMethodDeclarationSyntax node) { return node.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)); }
        public static bool IsPublic(this BaseTypeDeclarationSyntax node) { return node.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)); }
    }
}

