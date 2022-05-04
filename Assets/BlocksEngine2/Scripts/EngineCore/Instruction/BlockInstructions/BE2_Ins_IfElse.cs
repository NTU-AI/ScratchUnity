using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Ins_IfElse : BE2_InstructionBase, I_BE2_Instruction
{
    //protected override void OnAwake()
    //{
    //
    //}

    //protected override void OnStart()
    //{
    //    
    //}

    I_BE2_BlockSectionHeaderInput _input0;
    string _value;
    bool _isFirstPlay = true;

    protected override void OnButtonStop()
    {
        _isFirstPlay = true;
    }

    public override void OnStackActive()
    {
        _isFirstPlay = true;
    }

    public new void Function()
    {
        if (_isFirstPlay)
        {
            _input0 = Section0Inputs[0];
            _value = _input0.StringValue;
            //EndLoop = true;

            if (_value == "1" || _value.ToLower() == "true")
            {
                _isFirstPlay = false;
                ExecuteSection(0);
            }
            else
            {
                _isFirstPlay = false;
                ExecuteSection(1);
            }
        }
        else
        {
            _isFirstPlay = true;
            ExecuteNextInstruction();
        }
    }

    public string Generator(BE2_Generator.programmingLanguages language)
    {
        string code = "";

        _input0 = Section0Inputs[0];
        _value = _input0.StringValue;

        if (language.Equals(BE2_Generator.programmingLanguages.Python))
        {
            code = "if (" + _value + "):\n";
        }
        else if (language.Equals(BE2_Generator.programmingLanguages.Cpp))
            code = "...\n";

        return code;
    }
}
