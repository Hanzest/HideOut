using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class Effect
    {
        private string _name;
        private float _x;
        private float _y;
        private bool _exist;
        private bool _faceLeft;
        private int _tick;
        private int _tickCounter;
        private int _frame;
        public Effect(string name, float x, float y, int tick, bool faceLeft, int frame)
        {
            _name = name;
            _x = x;
            _y = y;
            _tick = tick;
            _tickCounter = 0;
            _faceLeft = faceLeft;
            _exist = true;
            _frame = frame;
        }
        public string Name
        {
            get { return _name; }
        }
        public float X
        {
            get { return _x; }
        }
        public float Y
        {
            get { return _y; }
        }
        public bool Exist
        {
            get { return _exist; }
        }
        public int TickCounter
        {
            get { return _tickCounter; }
        }
        public bool FaceLeft
        {
            get { return _faceLeft; }
        }
        public void Tick()
        {
            _tickCounter++;
            if(_tickCounter == _tick)
            {
                _exist = false;
            }
        }
        public int Index()
        {
            double ratio = (double)TickCounter / _tick;
            return 1 + (int)(ratio * _frame);
        }
    }
}
