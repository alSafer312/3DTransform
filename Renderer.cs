using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace lab10
{
    class Renderer
    {
        public void DrawAxis(int[,] DisplayM, Graphics gfx)
        {
            Pen deepSkyBlue = new Pen(new SolidBrush(Color.DeepSkyBlue), 2);
            Font f = new Font("Comic Sans", 8);
            gfx.DrawLine(deepSkyBlue, DisplayM[0, 0], DisplayM[0, 1], DisplayM[1, 0], DisplayM[1, 1]);
            gfx.DrawLine(deepSkyBlue, DisplayM[2, 0], DisplayM[2, 1], DisplayM[3, 0], DisplayM[3, 1]);
            gfx.DrawLine(deepSkyBlue, DisplayM[4, 0], DisplayM[4, 1], DisplayM[5, 0], DisplayM[5, 1]);
            gfx.DrawString("X", f, Brushes.Black, DisplayM[1, 0], DisplayM[1, 1]);
            gfx.DrawString("Y", f, Brushes.Black, DisplayM[3, 0], DisplayM[3, 1]);
            gfx.DrawString("Z", f, Brushes.Black, DisplayM[5, 0], DisplayM[5, 1]);
        }
        public void DrawAxisSrfc(ShapeData XOY, ShapeData XOZ, ShapeData YOZ, Graphics gfx)
        {
            SolidBrush ggreen = new SolidBrush(Color.FromArgb(50, 54, 217, 173));

            Point[] p = new Point[XOY.DisplayM.GetLength(0)];
            for (int i = 0; i < p.Length; i++)
            {
                p[i].X = XOY.DisplayM[i, 0]; p[i].Y = XOY.DisplayM[i, 1];
            }
            gfx.FillClosedCurve(ggreen, p, FillMode.Alternate, 0.5f);
            for (int i = 0; i < p.Length; i++)
            {
                p[i].X = XOZ.DisplayM[i, 0]; p[i].Y = XOZ.DisplayM[i, 1];
            }
            gfx.FillClosedCurve(ggreen, p, FillMode.Alternate, 0.5f);
            for (int i = 0; i < p.Length; i++)
            {
                p[i].X = YOZ.DisplayM[i, 0]; p[i].Y = YOZ.DisplayM[i, 1];
            }
            gfx.FillClosedCurve(ggreen, p, FillMode.Alternate, 0.5f);
        }
        public void DrawPPD(ShapeData PPD, Graphics gfx)
        {
            SolidBrush purple = new SolidBrush(Color.FromArgb(84, 195, 19, 214));
            Pen p = new Pen(new SolidBrush(Color.Black), 2);
            gfx.DrawLine(p, PPD.DisplayM[0, 0], PPD.DisplayM[0, 1], PPD.DisplayM[1, 0], PPD.DisplayM[1, 1]);
            gfx.DrawLine(p, PPD.DisplayM[1, 0], PPD.DisplayM[1, 1], PPD.DisplayM[2, 0], PPD.DisplayM[2, 1]);
            gfx.DrawLine(p, PPD.DisplayM[2, 0], PPD.DisplayM[2, 1], PPD.DisplayM[3, 0], PPD.DisplayM[3, 1]);
            gfx.DrawLine(p, PPD.DisplayM[3, 0], PPD.DisplayM[3, 1], PPD.DisplayM[0, 0], PPD.DisplayM[0, 1]);

            gfx.DrawLine(p, PPD.DisplayM[4, 0], PPD.DisplayM[4, 1], PPD.DisplayM[5, 0], PPD.DisplayM[5, 1]);
            gfx.DrawLine(p, PPD.DisplayM[5, 0], PPD.DisplayM[5, 1], PPD.DisplayM[6, 0], PPD.DisplayM[6, 1]);
            gfx.DrawLine(p, PPD.DisplayM[6, 0], PPD.DisplayM[6, 1], PPD.DisplayM[7, 0], PPD.DisplayM[7, 1]);
            gfx.DrawLine(p, PPD.DisplayM[7, 0], PPD.DisplayM[7, 1], PPD.DisplayM[4, 0], PPD.DisplayM[4, 1]);

            gfx.DrawLine(p, PPD.DisplayM[0, 0], PPD.DisplayM[0, 1], PPD.DisplayM[4, 0], PPD.DisplayM[4, 1]);
            gfx.DrawLine(p, PPD.DisplayM[1, 0], PPD.DisplayM[1, 1], PPD.DisplayM[5, 0], PPD.DisplayM[5, 1]);
            gfx.DrawLine(p, PPD.DisplayM[2, 0], PPD.DisplayM[2, 1], PPD.DisplayM[6, 0], PPD.DisplayM[6, 1]);
            gfx.DrawLine(p, PPD.DisplayM[3, 0], PPD.DisplayM[3, 1], PPD.DisplayM[7, 0], PPD.DisplayM[7, 1]);
            Point[] pt = new Point[4];
            pt[0].X = PPD.DisplayM[0, 0]; pt[0].Y = PPD.DisplayM[0, 1];
            pt[1].X = PPD.DisplayM[1, 0]; pt[1].Y = PPD.DisplayM[1, 1];
            pt[2].X = PPD.DisplayM[2, 0]; pt[2].Y = PPD.DisplayM[2, 1];
            pt[3].X = PPD.DisplayM[3, 0]; pt[3].Y = PPD.DisplayM[3, 1];
            gfx.FillClosedCurve(purple, pt, FillMode.Alternate, 0.01f);
            pt[0].X = PPD.DisplayM[0, 0]; pt[0].Y = PPD.DisplayM[0, 1];
            pt[1].X = PPD.DisplayM[4, 0]; pt[1].Y = PPD.DisplayM[4, 1];
            pt[2].X = PPD.DisplayM[5, 0]; pt[2].Y = PPD.DisplayM[5, 1];
            pt[3].X = PPD.DisplayM[1, 0]; pt[3].Y = PPD.DisplayM[1, 1];
            gfx.FillClosedCurve(purple, pt, FillMode.Alternate, 0.01f);
            pt[0].X = PPD.DisplayM[4, 0]; pt[0].Y = PPD.DisplayM[4, 1];
            pt[1].X = PPD.DisplayM[5, 0]; pt[1].Y = PPD.DisplayM[5, 1];
            pt[2].X = PPD.DisplayM[6, 0]; pt[2].Y = PPD.DisplayM[6, 1];
            pt[3].X = PPD.DisplayM[7, 0]; pt[3].Y = PPD.DisplayM[7, 1];
            gfx.FillClosedCurve(purple, pt, FillMode.Alternate, 0.01f);
            pt[0].X = PPD.DisplayM[6, 0]; pt[0].Y = PPD.DisplayM[6, 1];
            pt[1].X = PPD.DisplayM[7, 0]; pt[1].Y = PPD.DisplayM[7, 1];
            pt[2].X = PPD.DisplayM[3, 0]; pt[2].Y = PPD.DisplayM[3, 1];
            pt[3].X = PPD.DisplayM[2, 0]; pt[3].Y = PPD.DisplayM[2, 1];
            gfx.FillClosedCurve(purple, pt, FillMode.Alternate, 0.01f);
            pt[0].X = PPD.DisplayM[1, 0]; pt[0].Y = PPD.DisplayM[1, 1];
            pt[1].X = PPD.DisplayM[2, 0]; pt[1].Y = PPD.DisplayM[2, 1];
            pt[2].X = PPD.DisplayM[6, 0]; pt[2].Y = PPD.DisplayM[6, 1];
            pt[3].X = PPD.DisplayM[5, 0]; pt[3].Y = PPD.DisplayM[5, 1];
            gfx.FillClosedCurve(purple, pt, FillMode.Alternate, 0.01f);
            pt[0].X = PPD.DisplayM[0, 0]; pt[0].Y = PPD.DisplayM[0, 1];
            pt[1].X = PPD.DisplayM[3, 0]; pt[1].Y = PPD.DisplayM[3, 1];
            pt[2].X = PPD.DisplayM[7, 0]; pt[2].Y = PPD.DisplayM[7, 1];
            pt[3].X = PPD.DisplayM[4, 0]; pt[3].Y = PPD.DisplayM[4, 1];
            gfx.FillClosedCurve(purple, pt, FillMode.Alternate, 0.01f);
        }
        public void DrawPPDProj(ShapeData PPDProj, Graphics gfx)
        {
            Pen p = new Pen(new SolidBrush(Color.DarkGray), 2);
            gfx.DrawLine(p, PPDProj.DisplayM[0, 0], PPDProj.DisplayM[0, 1], PPDProj.DisplayM[1, 0], PPDProj.DisplayM[1, 1]);
            gfx.DrawLine(p, PPDProj.DisplayM[1, 0], PPDProj.DisplayM[1, 1], PPDProj.DisplayM[2, 0], PPDProj.DisplayM[2, 1]);
            gfx.DrawLine(p, PPDProj.DisplayM[2, 0], PPDProj.DisplayM[2, 1], PPDProj.DisplayM[3, 0], PPDProj.DisplayM[3, 1]);
            gfx.DrawLine(p, PPDProj.DisplayM[3, 0], PPDProj.DisplayM[3, 1], PPDProj.DisplayM[0, 0], PPDProj.DisplayM[0, 1]);

            gfx.DrawLine(p, PPDProj.DisplayM[4, 0], PPDProj.DisplayM[4, 1], PPDProj.DisplayM[5, 0], PPDProj.DisplayM[5, 1]);
            gfx.DrawLine(p, PPDProj.DisplayM[5, 0], PPDProj.DisplayM[5, 1], PPDProj.DisplayM[6, 0], PPDProj.DisplayM[6, 1]);
            gfx.DrawLine(p, PPDProj.DisplayM[6, 0], PPDProj.DisplayM[6, 1], PPDProj.DisplayM[7, 0], PPDProj.DisplayM[7, 1]);
            gfx.DrawLine(p, PPDProj.DisplayM[7, 0], PPDProj.DisplayM[7, 1], PPDProj.DisplayM[4, 0], PPDProj.DisplayM[4, 1]);

            gfx.DrawLine(p, PPDProj.DisplayM[0, 0], PPDProj.DisplayM[0, 1], PPDProj.DisplayM[4, 0], PPDProj.DisplayM[4, 1]);
            gfx.DrawLine(p, PPDProj.DisplayM[1, 0], PPDProj.DisplayM[1, 1], PPDProj.DisplayM[5, 0], PPDProj.DisplayM[5, 1]);
            gfx.DrawLine(p, PPDProj.DisplayM[2, 0], PPDProj.DisplayM[2, 1], PPDProj.DisplayM[6, 0], PPDProj.DisplayM[6, 1]);
            gfx.DrawLine(p, PPDProj.DisplayM[3, 0], PPDProj.DisplayM[3, 1], PPDProj.DisplayM[7, 0], PPDProj.DisplayM[7, 1]);
        }
        public void DrawPPDProjRays(ShapeData PPD, ShapeData PPDProj, Graphics gfx)
        {
            Pen p = new Pen(new SolidBrush(Color.Yellow), 1);
            p.DashStyle = DashStyle.Dash;
            if (PPD.DisplayM.GetLength(0) != PPDProj.DisplayM.GetLength(0))
            {
                System.Windows.Forms.MessageBox.Show("Error! m1.col != m2.row");
                return;
            }
            for (int i = 0; i < PPD.DisplayM.GetLength(0); i++)
            {
                gfx.DrawLine(p, PPD.DisplayM[i, 0], PPD.DisplayM[i, 1],
                            PPDProj.DisplayM[i, 0], PPDProj.DisplayM[i, 1]);
            }
        }

    }
}
