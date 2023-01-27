using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace lab10
{
    class Renderer
    {
        public void DrawAxis(int[,] DisplayM, System.Drawing.Graphics gfx)
        {
            Pen deepSkyBlue = new Pen(new SolidBrush(Color.DeepSkyBlue), 1);
            Font f = new Font("Comic Sans", 8);
            gfx.DrawLine(deepSkyBlue, DisplayM[0, 0], DisplayM[0, 1], DisplayM[1, 0], DisplayM[1, 1]);
            gfx.DrawLine(deepSkyBlue, DisplayM[2, 0], DisplayM[2, 1], DisplayM[3, 0], DisplayM[3, 1]);
            gfx.DrawLine(deepSkyBlue, DisplayM[4, 0], DisplayM[4, 1], DisplayM[5, 0], DisplayM[5, 1]);
            gfx.DrawString("X", f, Brushes.Black, DisplayM[1, 0], DisplayM[1, 1]);
            gfx.DrawString("Y", f, Brushes.Black, DisplayM[3, 0], DisplayM[3, 1]);
            gfx.DrawString("Z", f, Brushes.Black, DisplayM[5, 0], DisplayM[5, 1]);
        }
    }
}
