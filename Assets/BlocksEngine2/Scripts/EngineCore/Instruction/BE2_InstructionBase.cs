using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BE2_InstructionBase : MonoBehaviour, I_BE2_InstructionBase
{
    I_BE2_BlockLayout _blockLayout;
    I_BE2_BlockSection[] _sectionsList;
    I_BE2_BlockSectionHeader _section0header;
    int _lastLocation;

    public I_BE2_Instruction Instruction { get; set; }
    public I_BE2_Block Block { get; set; }
    public I_BE2_BlocksStack BlocksStack { get; set; }
    public I_BE2_TargetObject TargetObject { get; set; }
    public int[] LocationsArray { get; set; }
    protected virtual void OnAwake() { }
    protected virtual void OnStart() { }
    protected virtual void OnButtonPlay() { }
    protected virtual void OnButtonStop() { }
    public virtual void OnPrepareToPlay() { }
    public virtual void OnStackActive() { }


    public BE2_Generator generator;

    void Awake()
    {
        InstructionBase = this;
        Instruction = GetComponent<I_BE2_Instruction>();
        Block = GetComponent<I_BE2_Block>();
        _blockLayout = GetComponent<I_BE2_BlockLayout>();

        if (Block.Type == BlockTypeEnum.trigger)
        {
            BlocksStack = GetComponent<I_BE2_BlocksStack>();
        }

        _section0header = transform.GetChild(0).GetChild(0).GetComponent<I_BE2_BlockSectionHeader>();
        _section0header.UpdateInputsArray();

        OnAwake();
    }

    void Start()
    {
        // ---> add to layout number of bodies and use this number to create the locations array
        I_BE2_BlockSection[] tempSectionsArr = _blockLayout.SectionsArray;
        LocationsArray = new int[
            BE2_ArrayUtils.FindAll(ref tempSectionsArr, (x => x.Body != null)).Length + 1];

        _sectionsList = _blockLayout.SectionsArray;

        BE2_MainEventsManager.Instance.StartListening(BE2EventTypes.OnPlay, OnButtonPlay);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypes.OnStop, OnButtonStop);
        BE2_MainEventsManager.Instance.StartListening(BE2EventTypes.OnPointerUpEnd, GetBlockStack);

        generator = GameObject.FindWithTag("Generator").GetComponent<BE2_Generator>();

        OnStart();
    }

    void OnDisable()
    {
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypes.OnPlay, OnButtonPlay);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypes.OnStop, OnButtonStop);
        BE2_MainEventsManager.Instance.StopListening(BE2EventTypes.OnPointerUpEnd, GetBlockStack);
    }

    void GetBlockStack()
    {
        BlocksStack = GetComponentInParent<I_BE2_BlocksStack>();
        if (BlocksStack == null)
        {
            Block.SetShadowActive(false);
        }
        else if (BlocksStack.IsActive)
        {
            Block.SetShadowActive(true);
        }
    }

    public I_BE2_BlockSectionHeaderInput[] Section0Inputs
    {
        get
        {
            return _section0header?.InputsArray;
        }
    }

    public I_BE2_BlockSectionHeaderInput[] GetSectionInputs(int sectionIndex)
    {
        return _sectionsList[sectionIndex].Header.InputsArray;
    }

    public void PrepareToPlay()
    {
        _lastLocation = LocationsArray[LocationsArray.Length - 1];

        OnPrepareToPlay();
    }

    int _overflowLimit = 100;

    public string UpdateCode(string text, int tabCounter)
    {
        string newCode = "";

        for (int j = 0; j < tabCounter; j++)
            newCode += "\t";

        newCode += text;

        return newCode;
    }

    public string GetSectionCode(int sectionIndex)
    {
        string code = "";

        // variavel para contar a quantidade de tabs dado durante o codigo
        int tabCounter = 0;

        // dicionario para guardar os blocos que possuem mais de uma secao de codigo
        Dictionary<I_BE2_Block, int> blocks_with_sections = new Dictionary<I_BE2_Block, int>();

        if (BlocksStack != null && BlocksStack.InstructionsArray != null && BlocksStack.InstructionsArray.Length >= 0)
        {

            // vare o a pilha de blocos e monta o código
            for (int i = 0; i < BlocksStack.InstructionsArray.Length; i++)
            {
          
                I_BE2_Instruction instruction = BlocksStack.InstructionsArray[i];
                I_BE2_Block currentBlock = instruction.InstructionBase.Block;
                BlockTypeEnum type = currentBlock.Type;

                // verifica se o bloco eh do tipo codition, loop ou trigger, tipos que possuiram seus codigos em novos escopos (tab)
                if (type.ToString() == "condition" || type.ToString() == "loop" || type.ToString() == "trigger")
                {
                    // verifica a quantidade de secoes que o bloco atual tem
                    int sectionNumber = currentBlock.Layout.SectionsArray.Length;

                    // se o bloco nao estiver salvo no dic
                    if (!blocks_with_sections.ContainsKey(currentBlock))
                    {
                        // adiciona o bloco e incrementa seu contador de secao lida
                        blocks_with_sections.Add(currentBlock, 1);

                        // atualiza o codigo
                        code += UpdateCode(instruction.Generator(generator.GetGeneratorLanguage()), tabCounter);

                        // incrementa o tabCounter
                        tabCounter++;
                    }
                    else 
                    {
                        // se estiver salvo, pega o valor do contador de secao
                        int sectionCounter = blocks_with_sections[currentBlock];

                        // se o contador estiver atualizando
                        if (sectionCounter < sectionNumber)
                        {
                            // decrementa para acrescentar a chamada da nova secao
                            tabCounter--;

                            // adiciona o else
                            code += UpdateCode("else:\n", tabCounter);

                            // incrementa para adicionar a secao
                            tabCounter++;

                            // atualiza o contador de secao
                            blocks_with_sections[currentBlock]++;
                        }
                        else
                        {
                            // se o contador parou de atualizar, remove o bloco do dic
                            blocks_with_sections.Remove(currentBlock);
                            
                            // pula linha
                            code += "\n";

                            //decrementa o tabCounter
                            tabCounter--;
                        }
                    }
                }
                else
                {
                    // atualiza o codigo com o generator do bloco
                    code += UpdateCode(instruction.Generator(generator.GetGeneratorLanguage()), tabCounter);
                }
            }

            if (generator.generatorLanguage.Equals(BE2_Generator.programmingLanguages.Python))
            {
                // adiciona o footer do codigo
                code += "\nif __name__ == \'__main__\':\n" +
                   "\tmain()\n";
            }
            else if (generator.generatorLanguage.Equals(BE2_Generator.programmingLanguages.Cpp))
            {
                code = "";
            }
                   
        }
        else
        {
            // caso o bloco esteja se nenhum trigger

            /*if (Block != null)
            {
                Debug.Log(Block.GetType()) ;

                I_BE2_Instruction instruction = Block.Instruction;

                code += instruction.Generator(generator.GetGeneratorLanguage());
            }*/
        }

        return code;
    }

    public void ExecuteSection(int sectionIndex)
    {

        if (BlocksStack.InstructionsArray.Length > LocationsArray[sectionIndex])
        {
            I_BE2_Instruction instruction = BlocksStack.InstructionsArray[LocationsArray[sectionIndex]];
            BlockTypeEnum type = instruction.InstructionBase.Block.Type;

            // v2.1 - Loops are now executed "in frame" instead of mandatorily "in update". Faster loop execution and nested loops without delay
            //if (type != BlockTypeEnum.loop && !instruction.ExecuteInUpdate && BlocksStack.OverflowGuard < _overflowLimit)
            if (!instruction.ExecuteInUpdate && BlocksStack.OverflowGuard < _overflowLimit)
            {
                BlocksStack.OverflowGuard++;
                instruction.Function();
            }
            else
            {
                //if (BlocksStack.OverflowGuard >= _overflowLimit)
                //    Debug.LogWarning("(!) Overflow Guard Alert: Consider using \"ExecuteInUpdate => true\" in the " + instruction.GetType() +
                //    "\n(Refer to the documentation for information about the ExecuteInUpdate)");

                BlocksStack.Pointer = LocationsArray[sectionIndex];
            }
        }
        else
        {
            BlocksStack.Pointer = LocationsArray[sectionIndex];
        }
    }

    public void ExecuteNextInstruction()
    {
        if (BlocksStack.InstructionsArray.Length > _lastLocation)
        {
            I_BE2_Instruction instruction = BlocksStack.InstructionsArray[_lastLocation];
            BlockTypeEnum type = instruction.InstructionBase.Block.Type;

            // v2.1 - Loops are now executed "in frame" instead of mandatorily "in update". Faster loop execution and nested loops without delay
            if (!instruction.ExecuteInUpdate && BlocksStack.OverflowGuard < _overflowLimit)
            {
                BlocksStack.OverflowGuard++;
                instruction.Function();
            }
            else
            {
                //if (BlocksStack.OverflowGuard >= _overflowLimit)
                //    Debug.LogWarning("(!) Overflow Guard Alert: Consider using \"ExecuteInUpdate => true\" in the " + instruction.GetType() +
                //        "\n(Refer to the documentation for information about the ExecuteInUpdate)");

                BlocksStack.Pointer = _lastLocation;
            }
        }
        else
        {
            BlocksStack.Pointer = _lastLocation;
        }

        
    }

    public void Update()
    {
        string code = GetSectionCode(0);

        Debug.Log(generator.GetGeneratorLanguage().ToString() + " Code: \n" + code);

    }

    // ### Instruction ###
    public I_BE2_InstructionBase InstructionBase { get; set; }
    public bool ExecuteInUpdate { get; }

    public string Operation() { return ""; }
    public void Function() { }
}
