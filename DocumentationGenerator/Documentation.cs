﻿using DocumentationModels;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace DocumentationGenerator
{
    /// <summary>
    /// This is a smuuary
    /// this
    /// is not
    /// a 
    /// single
    /// line
    /// </summary>
    /// <remarks>
    /// these are remarks
    /// </remarks>
    /// <example>
    /// sorry no example
    /// </example>
    [Guid("A1ED89B7-4B97-4E63-A380-BC9E0EC09007")]
    public class Documentation
    {
        public List<INodeInspectionHandler> NodeHandlers { get; set; } = new List<INodeInspectionHandler>();

        public Documentation()
        {
            NodeHandlers.Add(new NodeInspectionHandler<ClassDeclarationSyntax>(ClassVisitor));
            NodeHandlers.Add(new NodeInspectionHandler<InterfaceDeclarationSyntax>(InterfaceVisitor));
            NodeHandlers.Add(new NodeInspectionHandler<EnumDeclarationSyntax>(EnumVisitor));
        }

        public Dictionary<string, Namespace> Namespaces { get; set; } = new Dictionary<string, Namespace>();

        private Namespace GetNamespace(string name)
        {
            if (Namespaces.ContainsKey(name))
            {
                return Namespaces[name];
            }
            var newNamespace = new Namespace { Name = name, FullName = name };
            Namespaces.Add(name, newNamespace);
            return newNamespace;
        }

        private void InsertItemDeclarationInformation(Compilation compilation, SyntaxTree tree, SyntaxNode node, ItemDeclaration item)
        {
            var semanticModel = compilation.GetSemanticModel(tree);
            var symbol = semanticModel.GetDeclaredSymbol(node);

            if (node is BaseTypeDeclarationSyntax baseType)
            {
                item.FullName = baseType.GetFullName();
                item.Name = baseType.Identifier.Text;
                item.Modifiers = baseType.Modifiers.ToString();
            }

            item.DocumentationComment = Extensions.ParseDocumentationComment(symbol.GetDocumentationCommentXml());
            //TODO: Add attributes



        }

        private TypeReference GetTypeReference(ITypeSymbol symbol)
        {
            var fullTypeName = symbol.GetFullName();
            var newTypeReference = new TypeReference { Name = symbol.GetTypeName(), FullName = fullTypeName };

            if (symbol is INamedTypeSymbol named && named.Arity != 0)
            {
                foreach (var typeParam in named.TypeArguments)
                {
                    newTypeReference.TypeParameters.Add(GetTypeReference(typeParam));
                }
            }


            return newTypeReference;
        }

        private TypeReference Visit(Compilation compilation, SyntaxTree tree, TypeSyntax typeNode)
        {
            var semanticModel = compilation.GetSemanticModel(tree);
            var symbolInfo = semanticModel.GetSymbolInfo(typeNode);
            return GetTypeReference(symbolInfo.Symbol as ITypeSymbol);
        }

        private List<Field> Visit(Compilation compilation, SyntaxTree tree, FieldDeclarationSyntax fieldDeclaration)
        {

            var typeReference = Visit(compilation, tree, fieldDeclaration.Declaration.Type);

            var fieldList = new List<Field>();

            foreach (var variable in fieldDeclaration.Declaration.Variables)
            {
                fieldList.Add(new Field
                {
                    Name = variable.Identifier.Text,
                    Type = typeReference,
                });

            }

            return fieldList;
        }

        private Property Visit(Compilation compilation, SyntaxTree tree, PropertyDeclarationSyntax propertyDeclaration)
        {
            return new Property
            {
                Name = propertyDeclaration.Identifier.Text,
                Type = Visit(compilation, tree, propertyDeclaration.Type),
                HasGetter = propertyDeclaration.AccessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.GetAccessorDeclaration)) == true,
                HasSetter = propertyDeclaration.AccessorList?.Accessors.Any(a => a.IsKind(SyntaxKind.SetAccessorDeclaration)) == true,
            };

        }

        private Method Visit(Compilation compilation, SyntaxTree tree, MethodDeclarationSyntax methodDeclaration)
        {
            var semanticModel = compilation.GetSemanticModel(tree);
            var symbol = semanticModel.GetDeclaredSymbol(methodDeclaration);

            var newMethod = new Method
            {
                Name = methodDeclaration.Identifier.Text,
                ReturnType = Visit(compilation, tree, methodDeclaration.ReturnType),
            };

            foreach (var param in methodDeclaration.ParameterList.Parameters)
            {
                newMethod.Parameters.Add(new MethodParameters
                {
                    Name = param.Identifier.Text,
                    Type = Visit(compilation, tree, param.Type),
                });
            }

            return newMethod;
        }


        private List<ItemDeclaration> VisitMembers(Compilation compilation, SyntaxTree tree, SyntaxNode node)
        {
            var newItemDeclarations = new List<ItemDeclaration>();

            foreach (var item in node.ChildNodes())
            {
                if (item is FieldDeclarationSyntax fieldDeclaration && fieldDeclaration.IsPublic())
                {
                    newItemDeclarations.AddRange(Visit(compilation, tree, fieldDeclaration));
                }
                else if (item is PropertyDeclarationSyntax propertyDeclaration && propertyDeclaration.IsPublic())
                {
                    newItemDeclarations.Add(Visit(compilation, tree, propertyDeclaration));
                }
                else if (item is MethodDeclarationSyntax methodDeclaration && methodDeclaration.IsPublic())
                {
                    newItemDeclarations.Add(Visit(compilation, tree, methodDeclaration));
                }
            }

            return newItemDeclarations;
        }

        public IterationDirection ClassVisitor(Compilation compilation, SyntaxTree tree, ClassDeclarationSyntax classDeclaration)
        {
            if (!classDeclaration.IsPublic()) { return IterationDirection.IterateChildren; }
            var namespaceName = classDeclaration.GetParentSyntax<NamespaceDeclarationSyntax>()?.Name.ToString();
            var nameSpace = GetNamespace(namespaceName);

            var newClass = new Class();
            InsertItemDeclarationInformation(compilation, tree, classDeclaration, newClass);
            newClass.Declarations = VisitMembers(compilation, tree, classDeclaration);
            nameSpace.Declarations.Add(newClass);

            return IterationDirection.IterateChildren;
        }

        public IterationDirection InterfaceVisitor(Compilation compilation, SyntaxTree tree, InterfaceDeclarationSyntax interfaceDeclaration)
        {
            if (!interfaceDeclaration.IsPublic()) { return IterationDirection.SkipChildren; }

            var namespaceName = interfaceDeclaration.GetParentSyntax<NamespaceDeclarationSyntax>()?.Name.ToString();
            var nameSpace = GetNamespace(namespaceName);

            var newInterface = new Interface();
            InsertItemDeclarationInformation(compilation, tree, interfaceDeclaration, newInterface);
            newInterface.Declarations = VisitMembers(compilation, tree, interfaceDeclaration);

            nameSpace.Declarations.Add(newInterface);

            return IterationDirection.SkipChildren;
        }

        public IterationDirection EnumVisitor(Compilation compilation, SyntaxTree tree, EnumDeclarationSyntax enumDeclaration)
        {
            if (!enumDeclaration.IsPublic()) { return IterationDirection.SkipChildren; }

            var namespaceName = enumDeclaration.GetParentSyntax<NamespaceDeclarationSyntax>()?.Name.ToString();
            var nameSpace = GetNamespace(namespaceName);

            var newEnum = new DocumentationModels.Enum();
            InsertItemDeclarationInformation(compilation, tree, enumDeclaration, newEnum);

            var semanticModel = compilation.GetSemanticModel(tree);

            foreach (var enumChild in enumDeclaration.ChildNodes())
            {
                if (enumChild is EnumMemberDeclarationSyntax enumMember)
                {
                    var symbol = semanticModel.GetDeclaredSymbol(enumChild) as IFieldSymbol;


                    var newEnumValue = new EnumValue
                    {
                        Name = enumMember.Identifier.Text,
                        Value = symbol.ConstantValue.ToString(),
                    };
                    newEnum.Values.Add(newEnumValue);
                }
            }

            nameSpace.Declarations.Add(newEnum);

            return IterationDirection.SkipChildren;
        }
    }
}