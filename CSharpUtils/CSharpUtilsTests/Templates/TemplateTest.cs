﻿using CSharpUtils.Templates;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using CSharpUtils;
using System.Linq;
using CSharpUtils.Templates.Tokenizers;
using CSharpUtils.Templates.TemplateProvider;

namespace CSharpUtilsTests.Templates
{
	[TestClass]
	public class TemplateTest
	{
		[TestMethod]
		public void TestExecVariableSimple()
		{
			Assert.AreEqual("Hello Test", TemplateCodeGen.CompileTemplateByString("Hello {{ User }}").RenderToString(new Dictionary<String, object>() {
				{ "User", "Test" },
			}));
		}

		[TestMethod]
		public void TestExecFor()
		{
			Assert.AreEqual("1234", TemplateCodeGen.CompileTemplateByString("{% for Item in List %}{{ Item }}{% endfor %}").RenderToString(new Dictionary<String, object>() {
				{ "List", new int[] { 1, 2, 3, 4 } },
			}));
		}

		[TestMethod]
		public void TestExecBlock()
		{
			Assert.AreEqual("1", TemplateCodeGen.CompileTemplateByString("{% block Body %}1{% endblock %}").RenderToString());
		}

		[TestMethod]
		public void TestExecForRange()
		{
			Assert.AreEqual("0123456789", TemplateCodeGen.CompileTemplateByString("{% for Item in 0..9 %}{{ Item }}{% endfor %}").RenderToString());
		}

		[TestMethod]
		public void TestExecWithOperator()
		{
			Assert.AreEqual("Hello 3 World", TemplateCodeGen.CompileTemplateByString("Hello {{ 1 + 2 }} World").RenderToString());
		}

		[TestMethod]
		public void TestExecWithOperatorPrecedence()
		{
			Assert.AreEqual("Hello 5 World", TemplateCodeGen.CompileTemplateByString("Hello {{ 1 + 2 * 2 }} World").RenderToString());
		}

		[TestMethod]
		public void TestExecIfElseCond0()
		{
			Assert.AreEqual("B", TemplateCodeGen.CompileTemplateByString("{% if 0 %}A{% else %}B{% endif %}").RenderToString());
		}

		[TestMethod]
		public void TestExecIfElseCond1()
		{
			Assert.AreEqual("A", TemplateCodeGen.CompileTemplateByString("{% if 1 %}A{% else %}B{% endif %}").RenderToString());
		}

		[TestMethod]
		public void TestExecTernaryTrue()
		{
			Assert.AreEqual("A", TemplateCodeGen.CompileTemplateByString("{{ 1 ? 'A' : 'B' }}").RenderToString());
		}

		[TestMethod]
		public void TestExecTernaryFalse()
		{
			Assert.AreEqual("B", TemplateCodeGen.CompileTemplateByString("{{ (1 - 1) ? 'A' : 'B' }}").RenderToString());
		}

		[TestMethod]
		public void ExecUnary()
		{
			Assert.AreEqual("-6", TemplateCodeGen.CompileTemplateByString("{{ -(1 + 2) + -3  }}").RenderToString());
		}

		[TestMethod]
		public void TestExecIfAnd()
		{
			Assert.AreEqual("A", TemplateCodeGen.CompileTemplateByString("{% if 1 && 2 %}A{% endif %}").RenderToString());
		}

		[TestMethod]
		public void TestExecIfOr()
		{
			Assert.AreEqual("A", TemplateCodeGen.CompileTemplateByString("{% if 0 || 2 %}A{% endif %}").RenderToString());
		}

		[TestMethod]
		public void TestExecBasicInheritance()
		{
			TemplateProviderMemory TemplateProvider = new TemplateProviderMemory();
			TemplateFactory TemplateFactory = new TemplateFactory(TemplateProvider);

			TemplateProvider.Add("Base.html", "Test{% block Body %}Base{% endblock %}Test");
			TemplateProvider.Add("Test.html", "{% extends 'Base.html' %}Not{% block Body %}Ex{% endblock %}Rendered");

			Assert.AreEqual("TestExTest", TemplateFactory.GetTemplateByFile("Test.html").RenderToString());
		}

		[TestMethod]
		public void TestExecInheritanceWithParent()
		{
			TemplateProviderMemory TemplateProvider = new TemplateProviderMemory();
			TemplateFactory TemplateFactory = new TemplateFactory(TemplateProvider);

			TemplateProvider.Add("Base.html", "Test{% block Body %}Base{% endblock %}Test");
			TemplateProvider.Add("Test.html", "{% extends 'Base.html' %}Not{% block Body %}1{% parent %}2{% endblock %}Rendered");

			Assert.AreEqual("Test1Base2Test", TemplateFactory.GetTemplateByFile("Test.html").RenderToString());
		}
	}
}