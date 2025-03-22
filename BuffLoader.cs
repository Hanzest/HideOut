using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SplashKitSDK;

namespace HideOut
{
    public class BuffLoader
    {
        private string _path;
        private StreamReader _reader;
        public BuffLoader() 
        {
            _path = "default";
        }
        public void Load(List<Bitmap> bitmaps)
        {
            _reader = new StreamReader("Resource\\BuffSetUp.txt");
            _path = _reader.ReadLine();
            int count = _reader.ReadInteger();
            for (int i = 0; i < count; i++)
            {
                string name = _reader.ReadLine();
                bitmaps.Add(new Bitmap(name, $"{_path}{name}.png"));
            }
        }
    }
}
