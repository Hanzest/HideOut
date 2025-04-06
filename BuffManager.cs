using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideOut
{
    public class BuffManager
    {
        private int[] _buffArray;
        private string[] _buffString;
        public BuffManager()
        {
            _buffArray = new int[7];
            _buffString = new string[7];
            for (int i = 0; i < 7; i ++)
            {
                _buffArray[i] = 0;
                _buffString[i] = "";
                switch(i){
                    case 0:
                        _buffString[i] = "Increase max HP";
                        break;
                    case 1:
                        _buffString[i] = "Increase max Energy";
                        break;
                    case 2:
                        _buffString[i] = "Increase max Armor";
                        break;
                    case 3:
                        _buffString[i] = "Increase max Speed";
                        break;
                    case 4:
                        _buffString[i] = "Decrease HP";
                        break;
                    case 5:
                        _buffString[i] = "Decrease Speed";
                        break;
                    case 6:
                        _buffString[i] = "Decrease Damage";
                        break;
                    default:
                        break;
                }
            }
        }
        public void ResetBuff()
        {
            for (int i = 0; i < 7; i++)
            {
                _buffArray[i] = 0;
            }
        }
        public void UpgradeBuff(int index)
        {
            _buffArray[index]++;
        }
        public int GetBuffIndex(int index)
        {
            return _buffArray[index];
        }
        public void SetBuffIndex(int index, int value)
        {
            _buffArray[index] = value;
        }
        public string GetBuffString(int index)
        {
            return _buffString[index];
        }
    }
}
