using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_Ins_BreakLoop : BE2_InstructionBase, I_BE2_Instruction
{
    //protected override void OnAwake()
    //{
    //    
    //}

    //protected override void OnStart()
    //{
    //    
    //}


    I_BE2_Instruction _parentLoopInstruction;
    I_BE2_Instruction[] _parentConditionInstructions;

    protected override void OnButtonStop()
    {
        _parentLoopInstruction = BE2_BlockUtils.GetParentInstructionOfType(this, BlockTypeEnum.loop);
        _parentConditionInstructions = BE2_BlockUtils.GetParentInstructionOfTypeAll(this, BlockTypeEnum.condition).ToArray();
    }

    public override void OnStackActive()
    {
        _parentLoopInstruction = BE2_BlockUtils.GetParentInstructionOfType(this, BlockTypeEnum.loop);
        _parentConditionInstructions = BE2_BlockUtils.GetParentInstructionOfTypeAll(this, BlockTypeEnum.condition).ToArray();
    }

    public new void Function()
    {
        if (_parentLoopInstruction != null)
        {
            // v2.4 - bugfix: fixed condition blocks not being reset on a loop break
            foreach (I_BE2_Instruction condIns in _parentConditionInstructions)
            {
                condIns.InstructionBase.OnStackActive();
            }

            _parentLoopInstruction.InstructionBase.ExecuteNextInstruction();
        }
        else
        {
            ExecuteNextInstruction();
        }
    }

    public string Generator(BE2_Generator.programmingLanguages language)
    {
        string code = "";

        if (language.Equals(BE2_Generator.programmingLanguages.Python))
            code = "...\n";
        else if (language.Equals(BE2_Generator.programmingLanguages.Cpp))
            code = "...\n";

        return code;
    }
}
