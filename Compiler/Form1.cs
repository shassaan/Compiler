using Compiler.CustomClasses;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;

namespace Compiler
{
    public partial class Form1 : Form
    {
        int lineNo;
        //int tookInput;
        public static string input { get; set; }
        List<Errors> errors = new List<Errors>();
        List<string> scopeLines;
        string printable;
        string mathExpression = @"^[ ]{0,}[a-zA-Z]{1,}[ ]{0,}=[ ]{0,}([*+\/-]{0,1}[ ]{0,}[0-9a-zA-z()][ ]{0,})*[ ]{0,}[;]{1,}[ ]{0,}$";
        string IfRegex = "^(if)[ ]{0,}[(]{1}[()a-zA-Z 0-9=<>]{1,}[ ]{0,}[)]{1}$";
        string loopregex = "^(until)[ ]{0,}[(]{1}[a-zA-Z0-9=<> ()]{1,}[ ]{0,}[)]{1,}$";
        string assignVariableValueRegex = "^[ ]{0,}[a-zA-Z]{1,}[ ]{0,}=[ ]{0,}[a-zA-Z]{1,}[ ]{0,}[ ]{0,}[;]{1,}[ ]{0,}$";
        string assiginValueRegex = "^[ ]{0,}[a-zA-Z]{1,}[ ]{0,}=[ ]{0,}[ 0-9a-zA-Z\".']{1,}]{0,};[ ]{0,}$";
        string printValueRegex = "^[ ]{0,}[P]{1}[(]{1}[\"][ 0-9a-zA-Z*&^%$#@!:;.,?~`<>]{0,}[\"]{1}[)]{1}[ ]{0,}[;]{1,}[ ]{0,}$";
        string printVariableRegex = "^[ ]{0,}P\\([a-zA-Z]{0,}\\)[ ]{0,};{1,}[ ]{0,}$";
        string varDecalarationRegex = "^[ ]{0,}(NUME|FRAC|ALPH|SENT)[ ]{1,}[a-zA-Z]{1,}[ ]{0,}[;]{1,}[ ]{0,}$";
        string inputRegex = "^[ ]{0,}[a-zA-Z]{1,}[ ]{0,}=[ ]{0,}G[ ]{0,}[(]{1}[)]{1}[ ]{0,}[;]{1,}[ ]{0,}$";
        List<Variables> variables;
        List<Functions> functions;
        public Form1()
        {
            InitializeComponent();
            variables = new List<Variables>();
            functions = new List<Functions>();
            scopeLines = new List<string>();
            input = "";
        }

        public Form1(int i)
        {
            InitializeComponent();
            variables = new List<Variables>();
            functions = new List<Functions>();
        }
       
        private void btnCompile_Click(object sender, EventArgs e)
        {
            try
            {
                InterpretAndExecute(rtbIde.Lines, "GLOBAL");
            }
            catch (Exception ex)
            {
                //MessageBox.Show(ex.s);
                
            }   
        }

        private void InterpretAndExecute(string[] lines,string scope)
        {
            if (!string.IsNullOrWhiteSpace(rtbIde.Text))
            {
                btnCompile.Enabled = false;
                btnStop.Enabled = true;
            }
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                lineNo++;
                if (line.Equals("{") || line.Equals("}") || line.Equals("else") || line.Equals(""))
                {
                    continue;
                }
                if (Regex.IsMatch(line, varDecalarationRegex))
                {
                    DeclareVariable(line, lineNo , scope);
                }
                else if (Regex.IsMatch(line, printValueRegex))
                {
                    printValue(line, lineNo);
                }
                else if (Regex.IsMatch(line, printVariableRegex))
                {

                    printVariable(line, lineNo,scope);
                }
                else if (Regex.IsMatch(line, assiginValueRegex))
                {
                    if (Regex.IsMatch(line, assignVariableValueRegex) && !line.Contains('"'))
                    {
                        assignVariable(line, lineNo,scope);
                    }
                    else
                    {
                        assignValue(line, lineNo,scope);
                    }

                }
                else if (Regex.IsMatch(line, inputRegex))
                {
                    //tookInput = 1;
                    if (errors.Count > 0)
                    {
                        continue;
                    }
                    takeInput(line, lineNo,scope);
                }
                else if (Regex.IsMatch(line, mathExpression))
                {
                    assignExpression(line, lineNo,scope);
                }
                else if (Regex.IsMatch(line,IfRegex))
                {
                    //var RawCondition = line.Where(c=>c != 'i' && c != 'f' && c != '(' && c != ')');
                    var RawCondition = line.Where(c => c != '(' && c != ')');
                    string condition = "";
                    foreach (var item in RawCondition)
                    {
                        condition += "" + item;
                    }
                    StringBuilder stringBuilder = new StringBuilder(condition);
                    condition = condition.Replace("if", " ");
                    condition = ReadyCondition(condition, scope);
                    StringBuilder stringnew = new StringBuilder(condition);

                    var a = stringnew.Replace("fi", " and ").ToString();

                    i = i + 2 ;
                    if ((bool)new DataTable().Compute(a, null))
                    {
                        while (true)
                        {   
                            if (lines[i].Equals("}"))
                            {
                                break;
                            }
                            scopeLines.Add(lines[i]);
                            i++;
                        }
                        var r = scopeLines.Where(s => !s.Equals("{") && !s.Equals("}")).ToArray<string>();
                        //Application.DoEvents();
                        InterpretAndExecute(r, "if"+lineNo);
                        //scopeLines = new List<string>();
                    }
                    else
                    {
                        int j = 0;
                        bool elseFlag = false;
                        while (true)
                        {
                            if (j == 2)
                            {
                                break;
                            }
                            if (lines[i].Equals("else"))
                            {
                                elseFlag = true;
                            }
                            if (lines[i].Equals("}"))
                            {
                                j++;
                            }
                            if (elseFlag)
                            {
                                scopeLines.Add(lines[i]);
                            }
                            i++;
                        }

                        InterpretAndExecute(scopeLines.Where(s=> !s.Equals("else") && !s.Equals("{") && !s.Equals("}")).ToArray<string>(),"else"+lineNo);
                        //scopeLines = new List<string>();
                    }

                }
                else if (Regex.IsMatch(line,loopregex))
                {
                    i++;
                }
                else if (string.IsNullOrWhiteSpace(line))
                {

                }
                else
                {
                    if (rtbIde.Text != string.Empty || line != string.Empty || line != " ")
                    {
                        errors.Add(new Errors { errCode = 102, Message = "Syntax Error", lineNo = lineNo });
                    }
                }

            }

            promptErrors();
        }

        private string ReadyCondition(string condition,string scope)
        {
            string expression = "";
            foreach (var item in condition)
            {
                if (Regex.IsMatch(item.ToString(), "^[0-9]$"))
                {
                    expression += item;
                }
                
                else if (Regex.IsMatch(item.ToString(), "^[a-zA-Z]$"))
                {
                    
                    if (isDeclared(item.ToString(), scope))
                    {
                        expression += GetValueOf(item.ToString(), scope);
                    }
                    else
                    {
                        expression += item.ToString();
                    }
                }
                else if (Regex.IsMatch(item.ToString(), @"^[<>=]$"))
                {
                    expression += item;
                }
                

            }

            return expression;
        }

        private void assignExpression(string line, int lineNo,string scope)
        {
           // bool isDeclared = false, isDeclaredR = false;
            var rawCharArray = line.Where(c => c != ' ' && c != ';').ToArray();
            string rawString = "";

            foreach (var item in rawCharArray)
            {
                rawString += "" + item;
            }

            string[] arr = rawString.Split('=');

            string varName = arr[0];
            var RhString = arr[1].Where(c=>c != ' ').ToArray();
            string expression = "";
            foreach (var item in RhString)
            {
                if (Regex.IsMatch(item.ToString(),"^[0-9]$"))
                {
                    expression += item;
                }
                else if (Regex.IsMatch(item.ToString(), "^[a-zA-Z]$"))
                {
                    if (isDeclared(item.ToString(),scope))
                    {
                        expression += GetValueOf(item.ToString(),scope);
                    }
                }
                else if (Regex.IsMatch(item.ToString(), @"^[()+-\/*]$"))
                {
                    expression += item;
                }
                else
                {
                    errors.Add(new Errors { errCode = 104,Message = "Invalid Value Assignment",lineNo = lineNo});
                }
                
            }

            DataTable dataTable = new DataTable();
            string result = "";
            try
            {
                 result = dataTable.Compute(expression, null).ToString();
                var Variable = variables.Where(v => v.name.Equals(varName) && (v.scope.Equals(scope) || v.scope.Equals("GLOBAL"))).Single();
                if (Variable.dataType.Equals("NUME") || Variable.dataType.Equals("FRAC"))
                {
                    for (int i = 0; i < variables.Count; i++)
                    {
                        if (variables[i].name.Equals(Variable.name) && (variables[i].scope.Equals(scope) || variables[i].scope.Equals("GLOBAL")))
                        {
                            if (Variable.dataType.Equals("NUME") && result.Contains("."))
                            {
                                var actualResult = result.Split('.')[0];
                                variables[i].value = actualResult + "";
                            }
                            else
                            {
                                variables[i].value = result + "";
                            }
                            break;
                        }
                    }
                }
                else
                {
                    errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
                }
            }
            catch (Exception)
            {

               errors.Add(new Errors { errCode = 107,Message = "Invalid Expression",lineNo = lineNo});

            }
           
        }

        private bool isDeclared(string Varname,string scope)
        {
            bool isdeclared = false;
            foreach (var item in variables)
            {
                if (item.name.Equals(Varname) && (item.scope.Equals(scope) || item.scope.Equals("GLOBAL")))
                {
                    isdeclared = true;
                    break;
                }
            }

            return isdeclared;
        }

        private string GetValueOf(string Varname , string scope)
        {
            string value = "";
            foreach (var item in variables)
            {
                if (item.name.Equals(Varname) && (item.scope.Equals(scope) || item.scope.Equals("GLOBAL")))
                {
                    value = item.value;
                    break;

                }
            }

            return value;
        }

        private void assignVariable(string line, int lineNo,string scope)
        {
            bool isDeclared = false , isDeclaredR = false;
            var rawCharArray = line.Where(c => c != ' ' && c != ';').ToArray();
            string rawString = "";

            foreach (var item in rawCharArray)
            {
                rawString += "" + item;
            }

            string[] arr = rawString.Split('=');

            string varName = arr[0];

            string value = arr[1];

            foreach (var item in variables)
            {
                if (item.name.Equals(varName) && (item.scope.Equals(scope) || item.scope.Equals("GLOBAL")))
                {
                    isDeclared = true;
                }
            }

            foreach (var item in variables)
            {
                if (item.name.Equals(value) && (item.scope.Equals(scope) || item.scope.Equals("GLOBAL")))
                {
                    isDeclaredR = true;
                }
            }

            if (isDeclared && isDeclaredR)
            {
                var Variable = variables.Where(v => v.name.Equals(varName) && (v.scope.Equals(scope) || v.scope.Equals("GLOBAL"))).Single();
                var VariableR = variables.Where(v => v.name.Equals(value) && (v.scope.Equals(scope) || v.scope.Equals("GLOBAL"))).Single();

                if (Variable.dataType.Equals(VariableR.dataType))
                {
                    for (int i = 0; i < variables.Count; i++)
                    {
                        if (variables[i].name.Equals(Variable.name) && (variables[i].scope.Equals(scope) || variables[i].scope.Equals("GLOBAL")))
                        {
                            variables[i].value = VariableR.value;
                            break;
                        }
                    }
                }
                else
                {
                    errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
                }

            }
            else
            {
                
                errors.Add(new Errors { errCode = 103, Message = "Variable not declared", lineNo = lineNo });
            }

        }

        private void takeInput(string line, int lineNo,string scope)
        {
            bool isDeclared = false;
            string[] inputComponents = line.Split('=');
            string VarName = inputComponents[0].Trim();
            foreach (var item in variables)
            {
                if (item.name.Equals(VarName) && (item.scope.Equals(scope) || item.scope.Equals("GLOBAL")))
                {
                    isDeclared = true;
                }
            }
          
            if (isDeclared)
            {
               
                    InputWindow inputWindow = new InputWindow();
                    inputWindow.ShowDialog();
                    var value = input;

                {
                    
                    var Variable = variables.Where(v => v.name.Equals(VarName) && (v.scope.Equals(scope) || v.scope.Equals("GLOBAL"))).Single();
                    if (Variable.dataType.Equals("NUME"))
                    {
                        if (Regex.IsMatch(value, "^[0-9]{1,}$"))
                        {
                            for (int i = 0; i < variables.Count; i++)
                            {
                                if (variables[i].name.Equals(Variable.name) && (variables[i].scope.Equals(scope) || variables[i].scope.Equals("GLOBAL")))
                                {
                                    variables[i].value = value;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
                        }
                    }
                    else if (Variable.dataType.Equals("FRAC"))
                    {
                        if (Regex.IsMatch(value, "^([.]{0,1}[0-9]{1,})*$"))
                        {
                            for (int i = 0; i < variables.Count; i++)
                            {
                                if (variables[i].name.Equals(Variable.name) && (variables[i].scope.Equals(scope) || variables[i].scope.Equals("GLOBAL")))
                                {
                                    variables[i].value = value;
                                    break;
                                }
                            }
                        }
                        else
                        {
                            errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
                        }
                    }
                    else if (Variable.dataType.Equals("ALPH"))
                    {
                        if (Regex.IsMatch(value, "^[']{1}[ a-zA-Z0-9$%#@!^&*()_-]{1}[']{1}$"))
                        {
                            for (int i = 0; i < variables.Count; i++)
                            {
                                if (variables[i].name.Equals(Variable.name) && (variables[i].scope.Equals(scope) || variables[i].scope.Equals("GLOBAL")))
                                {
                                    variables[i].value = value.Trim('\'');
                                    break;
                                }
                            }
                        }
                        else
                        {
                            errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
                        }
                    }
                    else if (Variable.dataType.Equals("SENT"))
                    {
                        if (Regex.IsMatch(value, "^[\"]{1}[ a-zA-Z0-9$%#@!^&*()_-]{0,}[\"]{1}$"))
                        {
                            for (int i = 0; i < variables.Count; i++)
                            {
                                if (variables[i].name.Equals(Variable.name) && (variables[i].scope.Equals(scope) || variables[i].scope.Equals("GLOBAL")))
                                {
                                    var valueWithoutQoutes = value.Where(c => c != '"').ToArray();

                                    foreach (var item in valueWithoutQoutes)
                                    {
                                        variables[i].value += "" + item;
                                    }
                                    break;
                                }
                            }
                        }
                        else
                        {
                            errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
                        }
                    }

                }


            }
            else
            {
                errors.Add(new Errors { errCode = 103, Message = "Variable not declared", lineNo = lineNo });

            }

        }

       //public void SetInputValue(string VarName,string value,List<Variables> lst)
       // {
       //     variables = lst;
       //     var Variable = variables.Where(v => v.name.Equals(VarName)).Single();
       //     if (Variable.dataType.Equals("NUME"))
       //     {
       //         if (Regex.IsMatch(value, "^[0-9]{1,}$"))
       //         {
       //             for (int i = 0; i < variables.Count; i++)
       //             {
       //                 if (variables[i].name.Equals(Variable.name))
       //                 {
       //                     variables[i].value = value;
       //                     break;
       //                 }
       //             }
       //         }
       //         else
       //         {
       //             errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
       //         }
       //     }
       //     else if (Variable.dataType.Equals("FRAC"))
       //     {
       //         if (Regex.IsMatch(value, "^([.]{0,1}[0-9]{1,})*$"))
       //         {
       //             for (int i = 0; i < variables.Count; i++)
       //             {
       //                 if (variables[i].name.Equals(Variable.name))
       //                 {
       //                     variables[i].value = value;
       //                     break;
       //                 }
       //             }
       //         }
       //         else
       //         {
       //             errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
       //         }
       //     }
       //     else if (Variable.dataType.Equals("ALPH"))
       //     {
       //         if (Regex.IsMatch(value, "^[']{1}[ a-zA-Z0-9$%#@!^&*()_-]{1}[']{1}$"))
       //         {
       //             for (int i = 0; i < variables.Count; i++)
       //             {
       //                 if (variables[i].name.Equals(Variable.name))
       //                 {
       //                     variables[i].value = value.Trim('\'');
       //                     break;
       //                 }
       //             }
       //         }
       //         else
       //         {
       //             errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
       //         }
       //     }
       //     else if (Variable.dataType.Equals("SENT"))
       //     {
       //         if (Regex.IsMatch(value, "^[\"]{1}[ a-zA-Z0-9$%#@!^&*()_-]{0,}[\"]{1}$"))
       //         {
       //             for (int i = 0; i < variables.Count; i++)
       //             {
       //                 if (variables[i].name.Equals(Variable.name))
       //                 {
       //                     var valueWithoutQoutes = value.Where(c => c != '"').ToArray();

       //                     foreach (var item in valueWithoutQoutes)
       //                     {
       //                         variables[i].value += "" + item;
       //                     }
       //                     break;
       //                 }
       //             }
       //         }
       //         else
       //         {
       //             errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
       //         }
       //     }

       // }

        private void assignValue(string line, int lineNo,string scope)
        {
            bool isDeclared = false;
            var rawCharArray = line.Where(c=>c!=' ' && c!=';').ToArray();
            string rawString = "";

            foreach (var item in rawCharArray)
            {
                rawString += "" + item;
            }

            string[] arr = rawString.Split('=');

            string varName = arr[0];

            string value = arr[1];

            foreach (var item in variables)
            {
                if (item.name.Equals(varName) && (item.scope.Equals(scope) || item.scope.Equals("GLOBAL")))
                {
                    isDeclared = true;
                }
            }

            if (isDeclared)
            {
                var Variable = variables.Where(v => v.name.Equals(varName) && (v.scope.Equals(scope) || v.scope.Equals("GLOBAL"))).Single();
                if (Variable.dataType.Equals("NUME"))
                {
                    if (Regex.IsMatch(value, "^[0-9]{1,}$"))
                    {
                        for (int i = 0; i < variables.Count; i++)
                        {
                            if (variables[i].name.Equals(Variable.name) && (variables[i].scope.Equals(scope) || variables[i].scope.Equals("GLOBAL")))
                            {
                                variables[i].value = value;
                                break;
                            }
                        }


                    }
                    else
                    {
                        errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
                    }
                }
                else if (Variable.dataType.Equals("FRAC"))
                {
                    if (Regex.IsMatch(value, "^([.]{0,1}[0-9]{1,})*$"))
                    {
                        for (int i = 0; i < variables.Count; i++)
                        {
                            if (variables[i].name.Equals(Variable.name) && (variables[i].scope.Equals(scope) || variables[i].scope.Equals("GLOBAL")))
                            {
                                variables[i].value = value;
                                break;
                            }
                        }
                    }
                    else
                    {
                        errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
                    }
                }
                else if (Variable.dataType.Equals("ALPH"))
                {
                    if (Regex.IsMatch(value, "^[']{1}[ a-zA-Z0-9$%#@!^&*()_-]{1}[']{1}$"))
                    {
                        for (int i = 0; i < variables.Count; i++)
                        {
                            if (variables[i].name.Equals(Variable.name) && (variables[i].scope.Equals(scope) || variables[i].scope.Equals("GLOBAL")))
                            {
                                variables[i].value = value.Trim('\'');
                                break;
                            }
                        }
                    }
                    else
                    {
                        errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
                    }
                }
                else if (Variable.dataType.Equals("SENT"))
                {
                    if (Regex.IsMatch(value, "^[\"]{1}[ a-zA-Z0-9$%#@!^&*()_-]{0,}[\"]{1}$"))
                    {
                        for (int i = 0; i < variables.Count; i++)
                        {
                            if (variables[i].name.Equals(Variable.name) && (variables[i].scope.Equals(scope) || variables[i].scope.Equals("GLOBAL")))
                            {
                                var valueWithoutQoutes = value.Where(c => c != '"').ToArray();

                                foreach (var item in valueWithoutQoutes)
                                {
                                    variables[i].value += "" + item;
                                }
                                break;
                            }
                        }
                    }
                    else
                    {
                        errors.Add(new Errors { errCode = 104, Message = "Invalid Value Assignment", lineNo = lineNo });
                    }
                }


            }
            else
            {
                errors.Add(new Errors { errCode = 103, Message = "Variable not declared", lineNo = lineNo });
            }

        }

        private void printVariable(string line, int lineNo,string scope)
        {
            
                bool isDeclared = false;
                var toBePrinted = line.Where(c => c != ')' && c != '(' && c != ';' && c!='P').ToArray();
                string varName = "";
                foreach (var item in toBePrinted)
                {
                    varName += "" + item;
                }
                foreach (var variable in variables)
                {
                    if (variable.name.Equals(varName) && (variable.scope.Equals(scope) || variable.scope.Equals("GLOBAL")))
                    {
                        isDeclared = true;
                    }
                }

                if (isDeclared)
                {
                    var printableVar = variables.Where(v => v.name.Equals(varName) && (v.scope.Equals(scope) || v.scope.Equals("GLOBAL"))).SingleOrDefault();
                    printable += printableVar.value;
                    rtBOutput.Text += printableVar.value;
                }
                else
                {
                    errors.Add(new Errors { errCode = 103, Message = "Variable not declared", lineNo = lineNo });
                }
        }

        private void printValue(string line, int lineNo)
        {
            var toBePrinted = line.Where(c => c != '"' && c !=')' && c != '('  && c != ';').ToArray();
            string msg = "";
            int i = 0;
            foreach (var item in toBePrinted)
            {
                if (i == 0)
                {
                    i++;
                    continue;
                    
                }
                msg += "" + item;
                i++;
            }
            if (msg.Trim().Equals("*"))
            {
                msg = "\n";
            }
            printable += msg;
            rtBOutput.Text += msg;
        }

        private void DeclareVariable(string Line, int lineNo,string scope)
        {
            bool isDeclared = false;
            var variable = new Variables();
            var newLine = Line.Where(c => c != ' ').ToArray();//remove all spaces from line 
            variable.dataType = "" + newLine[0] + newLine[1] + newLine[2] + newLine[3];
            var name = newLine.Where(c => c != ';').Skip(4).ToArray();
            foreach (var item in name)
            {
                variable.name += "" + item;
            }
            foreach (var item in variables)
            {
                if (item.name.Equals(variable.name) && (item.scope.Equals(scope) || item.scope.Equals("GLOBAL")))
                {
                    isDeclared = true;
                }
            }

            if (!isDeclared)
            {
                variable.scope = scope;
                variables.Add(variable);
                //MessageBox.Show("Variable Declared !!" + variable.dataType + "===>" + variable.name + " at Line # " + lineNo);
            }
            else
            {
                errors.Add(new Errors { errCode = 101,lineNo = lineNo,Message = "Variable Already Declared"});
            }


        }
	


        /*
          this method works with the global list of Errors
          display those errors for user
        */
        private void promptErrors()
        {
            if (errors.Count > 0)
            {
                rtBOutput.Text = string.Empty;
                
                foreach (var error in errors)
                {
                    lstBxErrors.Items.Add($"{error.errCode} ->  {error.Message} Line # {error.lineNo}");
                }

                
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            btnStop.Enabled = false;
            lstBxErrors.Items.Clear();
            btnCompile.Enabled = true;
            variables = new List<Variables>();
            scopeLines = new List<string>();
            lineNo = 0;
            printable = string.Empty;
            rtBOutput.Text = "";
            errors.Clear();
        }

        public void GenerateAssembly(List<Variables> vs,string[] lines)
        {
            StringBuilder assembly = new StringBuilder();
            assembly.Append($@".model small{Environment.NewLine}.stack 100h{Environment.NewLine}.data{Environment.NewLine}");
            foreach (var variable in vs)
            {
                if (variable.dataType.Equals("SENT"))
                {
                    assembly.Append($"{variable.name} DD \"{variable.value}\"$ {Environment.NewLine}");
                    continue;
                }

                assembly.Append($@"{variable.name} DD {variable.value}{Environment.NewLine}");
            }
            
            assembly.Append($@".code{Environment.NewLine}{Environment.NewLine}Main Proc{Environment.NewLine}MOV AX,@data{Environment.NewLine}MOV DS, AX{Environment.NewLine} MOV AH, 4CH{Environment.NewLine}INT 21H{Environment.NewLine}Main endp{Environment.NewLine}end");
            File.WriteAllText($@"{System.IO.Directory.GetCurrentDirectory()}/asm.txt",assembly.ToString());

        }

        private void btnGenerateAssembly_Click(object sender, EventArgs e)
        {
            GenerateAssembly(variables,null);
        }
    }
}
