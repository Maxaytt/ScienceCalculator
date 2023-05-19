using System.Windows.Controls;

namespace Kalkulator.StateMachines;

public enum TrigonometryState
{
    BaseMode,
    SecondMode,
    HypoMode
}

public class TrigonometryStateMachine
{
    private TrigonometryState _currentState; 
    private readonly Button _sinButton;
    private readonly Button _secButton;
    private readonly Button _cosButton;
    private readonly Button _cscButton;
    private readonly Button _tanButton;
    private readonly Button _cotButton;

    public TrigonometryStateMachine(Button sinButton, Button secButton, Button cosButton,
        Button cscButton, Button tanButton, Button cotButton)
    {
        _sinButton = sinButton;
        _secButton = secButton;
        _cosButton = cosButton;
        _cscButton = cscButton;
        _tanButton = tanButton;
        _cotButton = cotButton;
        _currentState = TrigonometryState.BaseMode;
    }

    public void ChangeState(TrigonometryState state)
    {
        if (state == TrigonometryState.SecondMode)
        {
            if (_currentState != TrigonometryState.BaseMode)
            {
                _currentState = TrigonometryState.BaseMode;
                
                ChangeToBaseButtonsState();
            }
            else
            {
                _currentState = TrigonometryState.SecondMode;
                
                ChangeToSecondButtonsState();
            }
        }
        else if (state == TrigonometryState.HypoMode)
        {
            if (_currentState != TrigonometryState.BaseMode)
            {
                _currentState = TrigonometryState.BaseMode;

                ChangeToBaseButtonsState();
            }
            else
            {
                _currentState = TrigonometryState.HypoMode;
                
                ChangeToHypoButtonsState();
            }
        }
    }

    private void ChangeToBaseButtonsState()
    {
        _sinButton.Name = "BtnSin";
        _sinButton.Content = "sin";
                
        _secButton.Name = "BtnSec";
        _secButton.Content = "sec";
                
        _cosButton.Name = "BtnCos";
        _cosButton.Content = "cos";
                
        _cscButton.Name = "BtnCsc";
        _cscButton.Content = "csc";
                
        _tanButton.Name = "BtnTan";
        _tanButton.Content = "tan";
        
        _cotButton.Name = "BtnCot";
        _cotButton.Content = "cot";
    }
    
    private void ChangeToHypoButtonsState()
    {
        _sinButton.Name = "BtnSinh";
        _sinButton.Content = "sinh";
                
        _secButton.Name = "BtnSech";
        _secButton.Content = "sech";
                
        _cosButton.Name = "BtnCosh";
        _cosButton.Content = "cosh";
                
        _cscButton.Name = "BtnCsch";
        _cscButton.Content = "csch";
                
        _tanButton.Name = "BtnTanh";
        _tanButton.Content = "tanh";
        
        _cotButton.Name = "BtnCoth";
        _cotButton.Content = "coth";
    }
    
    private void ChangeToSecondButtonsState()
    {
        _sinButton.Name = "BtnAsin";
        _sinButton.Content = "Asin";
                
        _secButton.Name = "BtnAsec";
        _secButton.Content = "Asec";
                
        _cosButton.Name = "BtnAcos";
        _cosButton.Content = "Acos";
                
        _cscButton.Name = "BtnAcsc";
        _cscButton.Content = "Acsc";
                
        _tanButton.Name = "BtnAtan";
        _tanButton.Content = "Atan";
        
        _cotButton.Name = "BtnAcot";
        _cotButton.Content = "Acot";
    }
}
