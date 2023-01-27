using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace lab10
{
    public partial class Form1 : Form
    {
        struct VectorXYZ
        {
            public float X, Y, Z;
        }
        struct SurfaceSides
        {
            public int id;
            public VectorXYZ[] p;
        }

        Calculator calculator = new Calculator();
        Renderer renderer = new Renderer();

        private ShapeData AxisXYZ;
        private ShapeData SurfaceXOY;
        private ShapeData SurfaceXOZ;
        private ShapeData SurfaceYOZ;
        private ShapeData PPD;
        int ppdA, ppdB,ppdH;

        private SurfaceSides[] sidesStart;
        private SurfaceSides[] sidesTransformed;
        private float a0, a1, b0, b1;
        private int M, N;
        private float[,] SrfcNormalizedM;
        private int[,] SrfcDisplayM;

        private float[,] TM0;
        private float[,] TM;        
        private float[,] AxisXRotTM;
        private float[,] AxisYRotTM;
        private float[,] ZOrthogTM;
        private float[,] MainTM;

        private int WIDTH, HEIGHT;
        private float IntervalCount, IntervalW, IntervalH, W, H;
        private float rad;
        private int rotX;
        private int rotY;

        private Graphics gfx0;
        private Graphics gfx;
        private Bitmap bmp;
        private Font f;

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            WIDTH = pictureBox1.Width;
            HEIGHT = pictureBox1.Height;
            IntervalCount = 10;
            IntervalW = WIDTH / (2 * (IntervalCount + 1));
            IntervalH = HEIGHT / (2 * (IntervalCount + 1));
            rad = (float)Math.PI / 180;
            rotX = 0; rotY = 0;

            /*
            AxisXYZ = new ShapeData();
            SurfaceXOY = new ShapeData();
            SurfaceXOZ = new ShapeData();
            SurfaceYOZ = new ShapeData();*/

            AxisXYZ.StartM = new float[,]
            {
                { -IntervalCount, 0, 0, 1 },
                { IntervalCount, 0, 0, 1 },
                { 0, -IntervalCount, 0, 1 },
                { 0, IntervalCount, 0, 1 },
                { 0, 0, -IntervalCount, 1 },
                { 0, 0, IntervalCount, 1 }
            };
            AxisXYZ.TransformedM = new float[6, 4];
            AxisXYZ.NormalizedM = new float[6, 4];
            AxisXYZ.DisplayM = new int[6, 4];

            SurfaceXOY.StartM = new float[4,4]
            {
                { -IntervalCount, IntervalCount, 0, 1},
                { IntervalCount, IntervalCount, 0, 1},
                { IntervalCount, -IntervalCount, 0, 1},
                { -IntervalCount, -IntervalCount, 0,1}
            };
            SurfaceXOY.TransformedM = new float[4, 4];
            SurfaceXOY.NormalizedM = new float[4, 4];
            SurfaceXOY.DisplayM = new int[4, 4];

            SurfaceXOZ.StartM = new float[4, 4]
            {
                { -IntervalCount, 0, -IntervalCount, 1},
                { IntervalCount, 0, -IntervalCount, 1},
                { IntervalCount, 0, IntervalCount, 1},
                { -IntervalCount, 0, IntervalCount, 1}
            };
            SurfaceXOZ.TransformedM = new float[4, 4];
            SurfaceXOZ.NormalizedM = new float[4, 4];
            SurfaceXOZ.DisplayM = new int[4, 4];

            SurfaceYOZ.StartM = new float[4, 4]
            {
                { 0, -IntervalCount, -IntervalCount,1},
                { 0, -IntervalCount, IntervalCount, 1},
                { 0, IntervalCount, IntervalCount, 1},
                { 0, IntervalCount,-IntervalCount, 1}
            };
            SurfaceYOZ.TransformedM = new float[4, 4];
            SurfaceYOZ.NormalizedM = new float[4, 4];
            SurfaceYOZ.DisplayM = new int[4, 4];

            ppdA = 2; ppdB  = 2; ppdH = 4;
            PPD.StartM = new float[8, 4]
            {
                { 0, 0, 0, 1}, { 0, 1 * ppdH, 0, 1},
                { 1 * ppdA, 1 * ppdH, 0, 1}, { 1 * ppdA, 0, 0, 1},
                { 0, 0, 1 * ppdB, 1}, { 0, 1 * ppdH, 1 * ppdB, 1},
                { 1 * ppdA, 1 * ppdH, 1 * ppdB, 1}, { 1 * ppdA, 0, 1 * ppdB, 1},
            };
            PPD.TransformedM = new float[8, 4];
            PPD.NormalizedM = new float[8, 4];
            PPD.DisplayM = new int[8, 4];

            AxisXRotTM = new float[,]
            {
                {1,0,0,0},
                {0, (float)Math.Cos(rad * rotX), (float)Math.Sin(rad * rotX), 0},
                {0, (float)-Math.Sin(rad * rotX), (float)Math.Cos(rad * rotX), 0},
                {0,0,0,1}
            };
            AxisYRotTM = new float[,]
            {
                {(float)Math.Cos(rad * rotY), 0, (float)-Math.Sin(rad * rotY), 0},
                {0,1,0,0},
                {(float)Math.Sin(rad * rotY), 0, (float)Math.Cos(rad * rotY), 0},
                {0,0,0,1}
            };
            ZOrthogTM = new float[,]
            {
                {1,0,0,0},
                {0,1,0,0},
                {0,0,0,0},
                {0,0,0,1}
            };
            TM = new float[,]
            {
                {1,0,0,0},
                {0,1,0,0},
                {0,0,1,0},
                {0,0,0,1},
            };
            MainTM = new float[4, 4];

            a0 = 0; a1 = 1;
            b0 = 0; b1 = 1;

            f = new Font("Comic Sans", 8);
            gfx = pictureBox1.CreateGraphics();
            bmp = new Bitmap(WIDTH, HEIGHT);
            gfx0 = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
            Draw();
        }

        private void Draw()
        {
            calculator.MainTransformationM(AxisXRotTM, AxisYRotTM, ZOrthogTM, MainTM);
            calculator.AxisTransformation(AxisXYZ, MainTM);
            calculator.Normalization(AxisXYZ.TransformedM, AxisXYZ.NormalizedM);
            calculator.CalculateDisplayCoord(AxisXYZ.NormalizedM, AxisXYZ.DisplayM, WIDTH, HEIGHT, IntervalW, IntervalH);
            renderer.DrawAxis(AxisXYZ.DisplayM, gfx0);

            if (checkBox1.Checked)
            {
                SurfaceTransformation();
                Normalization(SurfaceXOY.TransformedM, SurfaceXOY.NormalizedM);
                Normalization(SurfaceXOZ.TransformedM, SurfaceXOZ.NormalizedM);
                Normalization(SurfaceYOZ.TransformedM, SurfaceYOZ.NormalizedM);
                CalculateDisplayCoord(SurfaceXOY.NormalizedM, SurfaceXOY.DisplayM);
                CalculateDisplayCoord(SurfaceXOZ.NormalizedM, SurfaceXOZ.DisplayM);
                CalculateDisplayCoord(SurfaceYOZ.NormalizedM, SurfaceYOZ.DisplayM);
                drawSurface(SurfaceXOY.DisplayM);
                drawSurface(SurfaceXOZ.DisplayM);
                drawSurface(SurfaceYOZ.DisplayM);
            }
            if (checkBox2.Checked)
            {
                /*
                CreateSrfcPArr();
                drawSrfc();
                */
                ShapeTransformation(PPD.StartM, PPD.TransformedM);
                Normalization(PPD.TransformedM, PPD.NormalizedM);
                CalculateDisplayCoord(PPD.NormalizedM, PPD.DisplayM);
                drawPPD();
            }
        }
        private void MultiplyMatrix(float[,] m1, float[,] m2, float[,] result)
        {

            if (m1.GetLength(1) != m2.GetLength(0))
            {
                MessageBox.Show("Error! m1.col != m2.row");
                return;
            }
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i,j] = 0;
                }
            }
            for (int i = 0; i < m1.GetLength(0); i++)
            {
                for (int j = 0; j < m2.GetLength(1); j++)
                {
                    for (int k = 0; k < m1.GetLength(1); k++)
                    {
                        result[i, j] += m1[i, k] * m2[k, j]; 
                    }
                }
            }
        }
        private void Normalization(float[,] transformedMx, float[,] resultMx)
        {
            for (int i = 0; i < transformedMx.GetLength(0); i++)
            {
                for (int j = 0; j < transformedMx.GetLength(1); j++)
                {
                    if (transformedMx[i,3] != 1)
                    {
                        if (transformedMx[i,3] == 0)
                        {
                            transformedMx[i, 3] = 0.01f;
                        }
                        else
                        {
                            resultMx[i,j] = transformedMx[i,j] / transformedMx[i,3];
                        }
                    }
                    else
                    {
                        resultMx[i, j] = transformedMx[i, j];
                    }
                }
            }
        }
        private void CalculateDisplayCoord(float[,] DecCoordMx, int[,] DisplayCoordMx)
        {
            for (int i = 0; i < DecCoordMx.GetLength(0); i++)
            {
                for (int j = 0; j < DecCoordMx.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        DisplayCoordMx[i, 0] = (int)(WIDTH / 2 + (IntervalW * DecCoordMx[i, j]));
                    }
                    if (j == 1)
                    {
                        DisplayCoordMx[i, 1] = (int)(HEIGHT / 2 - (IntervalH * DecCoordMx[i, j]));
                    }
                }
            }
        }
        private void ResultTrMx()
        {
            float[,] temp = new float[4, 4];
            MultiplyMatrix(AxisYRotTM, AxisXRotTM, temp);
            MultiplyMatrix(temp, ZOrthogTM, MainTM);
        }
        private void AxisTransformation()
        {
            MultiplyMatrix(AxisXYZ.StartM, MainTM, AxisXYZ.TransformedM);
        }
        private void SurfaceTransformation()
        {
            MultiplyMatrix(SurfaceXOY.StartM, MainTM, SurfaceXOY.TransformedM);
            MultiplyMatrix(SurfaceXOZ.StartM, MainTM, SurfaceXOZ.TransformedM);
            MultiplyMatrix(SurfaceYOZ.StartM, MainTM, SurfaceYOZ.TransformedM);
        }
        private void drawAxis(int[,] screenM)
        {
            Pen deepSkyBlue = new Pen(new SolidBrush(Color.DeepSkyBlue), 1);
            gfx0.DrawLine(deepSkyBlue, screenM[0, 0], screenM[0, 1], screenM[1, 0], screenM[1, 1]);
            gfx0.DrawLine(deepSkyBlue, screenM[2, 0], screenM[2, 1], screenM[3, 0], screenM[3, 1]);
            gfx0.DrawLine(deepSkyBlue, screenM[4, 0], screenM[4, 1], screenM[5, 0], screenM[5, 1]);
            gfx0.DrawString("X", f, Brushes.Black, screenM[1, 0], screenM[1, 1]);
            gfx0.DrawString("Y", f, Brushes.Black, screenM[3, 0], screenM[3, 1]);
            gfx0.DrawString("Z", f, Brushes.Black, screenM[5, 0], screenM[5, 1]);
        }
        private void drawSurface(int[,] screenM)
        {
            SolidBrush ggreen = new SolidBrush(Color.FromArgb(50, 54, 217, 173));
            Point[] p = new Point[screenM.GetLength(0)];
            for (int i = 0; i < p.Length; i++)
            {
                p[i].X = screenM[i, 0]; p[i].Y = screenM[i, 1];
            }
            gfx0.FillClosedCurve(ggreen, p, FillMode.Alternate, 0.5f);
        }
        private void drawSrfc()
        {
            for (int i = 0; i < M * N; i++)
            {
                gfx0.DrawLine(Pens.Green, sidesStart[i].p[0].X, sidesStart[i].p[0].Y,
                                          sidesStart[i].p[1].X, sidesStart[i].p[1].Y);
                gfx0.DrawLine(Pens.Green, sidesStart[i].p[1].X, sidesStart[i].p[1].Y,
                                          sidesStart[i].p[2].X, sidesStart[i].p[2].Y);
                gfx0.DrawLine(Pens.Green, sidesStart[i].p[2].X, sidesStart[i].p[2].Y,
                                          sidesStart[i].p[3].X, sidesStart[i].p[3].Y);
                gfx0.DrawLine(Pens.Green, sidesStart[i].p[3].X, sidesStart[i].p[3].Y,
                                          sidesStart[i].p[0].X, sidesStart[i].p[0].Y);
            }
        }
        private void drawPPD()
        {
            SolidBrush ggreen0 = new SolidBrush(Color.Black);
            SolidBrush ggreen = new SolidBrush(Color.FromArgb(50, 54, 217, 173));
            Pen p = new Pen(ggreen0, 2);
            gfx0.DrawLine(p, PPD.DisplayM[0, 0], PPD.DisplayM[0, 1], PPD.DisplayM[1, 0], PPD.DisplayM[1, 1]);
            gfx0.DrawLine(p, PPD.DisplayM[1, 0], PPD.DisplayM[1, 1], PPD.DisplayM[2, 0], PPD.DisplayM[2, 1]);
            gfx0.DrawLine(p, PPD.DisplayM[2, 0], PPD.DisplayM[2, 1], PPD.DisplayM[3, 0], PPD.DisplayM[3, 1]);
            gfx0.DrawLine(p, PPD.DisplayM[3, 0], PPD.DisplayM[3, 1], PPD.DisplayM[0, 0], PPD.DisplayM[0, 1]);

            gfx0.DrawLine(p, PPD.DisplayM[4, 0], PPD.DisplayM[4, 1], PPD.DisplayM[5, 0], PPD.DisplayM[5, 1]);
            gfx0.DrawLine(p, PPD.DisplayM[5, 0], PPD.DisplayM[5, 1], PPD.DisplayM[6, 0], PPD.DisplayM[6, 1]);
            gfx0.DrawLine(p, PPD.DisplayM[6, 0], PPD.DisplayM[6, 1], PPD.DisplayM[7, 0], PPD.DisplayM[7, 1]);
            gfx0.DrawLine(p, PPD.DisplayM[7, 0], PPD.DisplayM[7, 1], PPD.DisplayM[4, 0], PPD.DisplayM[4, 1]);

            gfx0.DrawLine(p, PPD.DisplayM[0, 0], PPD.DisplayM[0, 1], PPD.DisplayM[4, 0], PPD.DisplayM[4, 1]);
            gfx0.DrawLine(p, PPD.DisplayM[1, 0], PPD.DisplayM[1, 1], PPD.DisplayM[5, 0], PPD.DisplayM[5, 1]);
            gfx0.DrawLine(p, PPD.DisplayM[2, 0], PPD.DisplayM[2, 1], PPD.DisplayM[6, 0], PPD.DisplayM[6, 1]);
            gfx0.DrawLine(p, PPD.DisplayM[3, 0], PPD.DisplayM[3, 1], PPD.DisplayM[7, 0], PPD.DisplayM[7, 1]);
            Point[] pt = new Point[4];
            pt[0].X = PPD.DisplayM[0, 0]; pt[0].Y = PPD.DisplayM[0, 1];
            pt[1].X = PPD.DisplayM[1, 0]; pt[1].Y = PPD.DisplayM[1, 1];
            pt[2].X = PPD.DisplayM[2, 0]; pt[2].Y = PPD.DisplayM[2, 1];
            pt[3].X = PPD.DisplayM[3, 0]; pt[3].Y = PPD.DisplayM[3, 1];
            gfx0.FillClosedCurve(ggreen, pt, FillMode.Alternate, 0.01f);
            pt[0].X = PPD.DisplayM[0, 0]; pt[0].Y = PPD.DisplayM[0, 1];
            pt[1].X = PPD.DisplayM[4, 0]; pt[1].Y = PPD.DisplayM[4, 1];
            pt[2].X = PPD.DisplayM[5, 0]; pt[2].Y = PPD.DisplayM[5, 1];
            pt[3].X = PPD.DisplayM[1, 0]; pt[3].Y = PPD.DisplayM[1, 1];
            gfx0.FillClosedCurve(ggreen, pt, FillMode.Alternate, 0.01f);
            pt[0].X = PPD.DisplayM[4, 0]; pt[0].Y = PPD.DisplayM[4, 1];
            pt[1].X = PPD.DisplayM[5, 0]; pt[1].Y = PPD.DisplayM[5, 1];
            pt[2].X = PPD.DisplayM[6, 0]; pt[2].Y = PPD.DisplayM[6, 1];
            pt[3].X = PPD.DisplayM[7, 0]; pt[3].Y = PPD.DisplayM[7, 1];
            gfx0.FillClosedCurve(ggreen, pt, FillMode.Alternate, 0.01f);
            pt[0].X = PPD.DisplayM[6, 0]; pt[0].Y = PPD.DisplayM[6, 1];
            pt[1].X = PPD.DisplayM[7, 0]; pt[1].Y = PPD.DisplayM[7, 1];
            pt[2].X = PPD.DisplayM[3, 0]; pt[2].Y = PPD.DisplayM[3, 1];
            pt[3].X = PPD.DisplayM[2, 0]; pt[3].Y = PPD.DisplayM[2, 1];
            gfx0.FillClosedCurve(ggreen, pt, FillMode.Alternate, 0.01f);
            pt[0].X = PPD.DisplayM[1, 0]; pt[0].Y = PPD.DisplayM[1, 1];
            pt[1].X = PPD.DisplayM[2, 0]; pt[1].Y = PPD.DisplayM[2, 1];
            pt[2].X = PPD.DisplayM[6, 0]; pt[2].Y = PPD.DisplayM[6, 1];
            pt[3].X = PPD.DisplayM[5, 0]; pt[3].Y = PPD.DisplayM[5, 1];
            gfx0.FillClosedCurve(ggreen, pt, FillMode.Alternate, 0.01f);
            pt[0].X = PPD.DisplayM[0, 0]; pt[0].Y = PPD.DisplayM[0, 1];
            pt[1].X = PPD.DisplayM[3, 0]; pt[1].Y = PPD.DisplayM[3, 1];
            pt[2].X = PPD.DisplayM[7, 0]; pt[2].Y = PPD.DisplayM[7, 1];
            pt[3].X = PPD.DisplayM[4, 0]; pt[3].Y = PPD.DisplayM[4, 1];
            gfx0.FillClosedCurve(ggreen, pt, FillMode.Alternate, 0.01f);
        }
        private void PictureBoxUpdate()
        {
            gfx0.Clear(pictureBox1.BackColor);
            Draw();
            pictureBox1.Image = bmp;
        }
        private void GlobalUpdate()
        {
            WIDTH = pictureBox1.Width;
            HEIGHT = pictureBox1.Height;
            IntervalH = HEIGHT / (2 * (IntervalCount + 1));
            IntervalW = WIDTH / (2 * (IntervalCount + 1));
            gfx = pictureBox1.CreateGraphics();
            bmp = new Bitmap(WIDTH, HEIGHT);
            gfx0 = Graphics.FromImage(bmp);
            pictureBox1.Image = bmp;
            Draw();
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void CreateSrfcPArr()
        {
            N = 1; M = 1;
            int q;
            float u, w, h1, h2;

            h1 = (a1 - a0) / N;
            h2 = (b1 - b0) / M;

            sidesStart = new SurfaceSides[M * N];
            sidesTransformed = new SurfaceSides[M * N];

            float[,] temp = new float[4,4];
            float[,] transformedtemp = new float[4, 4];

            for (int i = 0; i < M * N; i++)
            {
                sidesStart[i].p = new VectorXYZ[4];
                sidesTransformed[i].p = new VectorXYZ[4];
            }
            q = -1;
            for (int j = 0; j < N; j++)
            {
                for (int i = 0; i < M; i++)
                {
                    q++;
                    for (int k = 0; k < 4; k++)
                    {
                        u = 0; w = 0;
                        switch (k)
                        {
                            case 0:
                                u = a0 + h1 * (j + 0);
                                w = b0 + h2 * (i + 0); break;
                            case 1:
                                u = a0 + h1 * (j + 0);
                                w = b0 + h2 * (i + 1); break;
                            case 2:
                                u = a0 + h1 * (j + 1);
                                w = b0 + h2 * (i + 1); break;
                            case 3:
                                u = a0 + h1 * (j + 1);
                                w = b0 + h2 * (i + 0); break;
                        }
                        VectorXYZ tp;
                        tp = CalculateSrfcPt(u, w, 1);
                        temp[k, 0] = tp.X;
                        temp[k, 1] = tp.Y;
                        temp[k, 2] = tp.Z;
                        temp[k, 3] = 1;
                        sidesTransformed[q].p[k] = tp;
                    }
                    SrfcNormalizedM = new float[4, 4];
                    SrfcDisplayM = new int[4, 4];
                    SrfcTransformation(temp, transformedtemp);                    
                    Normalization(transformedtemp, SrfcNormalizedM);
                    CalculateDisplayCoord(SrfcNormalizedM, SrfcDisplayM);
                    sidesStart[q].p[0].X = SrfcDisplayM[0, 0];
                    sidesStart[q].p[0].Y = SrfcDisplayM[0, 1];
                    sidesStart[q].p[0].Z = SrfcDisplayM[0, 2];
                    sidesStart[q].p[1].X = SrfcDisplayM[1, 0];
                    sidesStart[q].p[1].Y = SrfcDisplayM[1, 1];
                    sidesStart[q].p[1].Z = SrfcDisplayM[1, 2];
                    sidesStart[q].p[2].X = SrfcDisplayM[2, 0];
                    sidesStart[q].p[2].Y = SrfcDisplayM[2, 1];
                    sidesStart[q].p[2].Z = SrfcDisplayM[2, 2];
                    sidesStart[q].p[3].X = SrfcDisplayM[3, 0];
                    sidesStart[q].p[3].Y = SrfcDisplayM[3, 1];
                    sidesStart[q].p[3].Z = SrfcDisplayM[3, 2];
                }
            }
        }        
        private VectorXYZ CalculateSrfcPt(float u1, float w1, float v1)
        {
            VectorXYZ P;
            P.X = 3 * u1 + w1;
            P.Y = 2 * u1 + 3 * w1 + u1 * w1;
            P.Z = v1;
            return P;
        }
        private void SrfcTransformation(float[,] m1, float[,] m2)
        {
            MultiplyMatrix(m1, MainTM, m2);
        }
        
        private void ShapeTransformation(float[,] m1, float[,] m2)
        {
            MultiplyMatrix(m1, MainTM, m2);
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
             trackBar1.Value = (int)numericUpDown1.Value;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            trackBar2.Value = (int)numericUpDown2.Value;
        }
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            GlobalUpdate();
        }
        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            PictureBoxUpdate();
            radioButton3.Enabled = checkBox2.Checked;
            radioButton4.Enabled = checkBox2.Checked;
            radioButton5.Enabled = checkBox2.Checked;
            button1.Enabled = checkBox2.Checked;
            button2.Enabled = checkBox2.Checked;
            button3.Enabled = checkBox2.Checked;
        }
        private void trackBar2_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown2.Value = trackBar2.Value;
            if (radioButton2.Checked)
            {
                rotY = trackBar2.Value;
                numericUpDown2.Value = rotY;

                AxisYRotTM[0, 0] = (float)Math.Cos(rad * rotY); AxisYRotTM[0, 1] = 0;
                AxisYRotTM[0, 2] = (float)-Math.Sin(rad * rotY); AxisYRotTM[0, 3] = 0;

                AxisYRotTM[1, 0] = 0; AxisYRotTM[1, 1] = 1; AxisYRotTM[1, 2] = 0; AxisYRotTM[1, 3] = 0;

                AxisYRotTM[2, 0] = (float)Math.Sin(rad * rotY); AxisYRotTM[2, 1] = 0;
                AxisYRotTM[2, 2] = (float)Math.Cos(rad * rotY); AxisYRotTM[2, 3] = 0;

                AxisYRotTM[3, 0] = 0; AxisYRotTM[3, 1] = 0; AxisYRotTM[3, 2] = 0; AxisYRotTM[3, 3] = 1;
                PictureBoxUpdate();
            }
        }
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBar1.Value;
            numericUpDown2.Value = trackBar2.Value;
            if (radioButton1.Checked)
            {
                IntervalCount = trackBar1.Value;
                GlobalUpdate();
            }
            if (radioButton2.Checked)
            {
                rotX = trackBar1.Value;
                numericUpDown1.Value = rotX;

                AxisXRotTM[0, 0] = 1; AxisXRotTM[0, 1] = 0; AxisXRotTM[0, 2] = 0; AxisXRotTM[0, 3] = 0;

                AxisXRotTM[1, 0] = 0; AxisXRotTM[1, 1] = (float)Math.Cos(rad * rotX);
                AxisXRotTM[1, 2] = (float)Math.Sin(rad * rotX); AxisXRotTM[1, 3] = 0;

                AxisXRotTM[2, 0] = 0; AxisXRotTM[2, 1] = (float)-Math.Sin(rad * rotX);
                AxisXRotTM[2, 2] = (float)Math.Cos(rad * rotX); AxisXRotTM[2, 3] = 0;

                AxisXRotTM[3, 0] = 0; AxisXRotTM[3, 1] = 0; AxisXRotTM[3, 2] = 0; AxisXRotTM[3, 3] = 1;
                PictureBoxUpdate();
            }
            if (radioButton3.Checked)
            {
                float[,] temp = new float[8, 4];
                TM[3, 0] = trackBar1.Value;
                MultiplyMatrix(PPD.TransformedM, TM, temp);
                PPD.TransformedM = temp;
                Normalization(PPD.TransformedM, PPD.NormalizedM);
                CalculateDisplayCoord(PPD.NormalizedM, PPD.DisplayM);
                drawPPD();
            }
        }
        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            trackBar1.Enabled = true;
            trackBar2.Enabled = true;
            trackBar3.Enabled = true;
            trackBar1.Minimum = -100; trackBar1.Maximum = 100;
            trackBar2.Minimum = -100; trackBar2.Maximum = 100;
            trackBar3.Minimum = -100; trackBar3.Maximum = 100;
            numericUpDown1.Minimum = -100; numericUpDown1.Maximum = 100;
            numericUpDown2.Minimum = -100; numericUpDown2.Maximum = 100;
            numericUpDown3.Minimum = -100; numericUpDown3.Maximum = 100;
            trackBar1.Value = 0;
            trackBar2.Value = 0;
            trackBar3.Value = 0;
            RadioButton rb = (RadioButton)sender;
            switch (rb.Name.ToString())
            {
                case "radioButton1":
                    trackBar1.Enabled = true;
                    trackBar2.Enabled = false;
                    trackBar3.Enabled = false;
                    trackBar1.Maximum = 20; trackBar1.Minimum = 5;
                    trackBar1.Value = 10;
                    numericUpDown1.Minimum = 5; numericUpDown1.Maximum = 20;
                    break;
                case "radioButton2":
                    trackBar1.Enabled = true;
                    trackBar2.Enabled = true;
                    trackBar3.Enabled = false;
                    trackBar1.Minimum = -360; trackBar1.Maximum = 360;
                    trackBar2.Minimum = -360; trackBar2.Maximum = 360;
                    trackBar1.Value = 0; trackBar2.Value = 0;
                    numericUpDown1.Minimum = -360; numericUpDown1.Maximum = 360;
                    numericUpDown2.Minimum = -360; numericUpDown2.Maximum = 360;
                    break;
                case "radioButton3":
                    trackBar1.Enabled = true;
                    trackBar2.Enabled = true;
                    trackBar3.Enabled = true;
                    trackBar1.Minimum = -20; trackBar1.Maximum = 20;
                    trackBar2.Minimum = -20; trackBar2.Maximum = 20;
                    trackBar3.Minimum = -20; trackBar3.Maximum = 20;
                    trackBar1.Value = 0; trackBar2.Value = 0; trackBar3.Value = 0;
                    numericUpDown1.Minimum = -20; numericUpDown1.Maximum = 20;
                    numericUpDown2.Minimum = -20; numericUpDown2.Maximum = 20;
                    numericUpDown3.Minimum = -20; numericUpDown3.Maximum = 20;
                    break;
                case "radioButton4":
                    trackBar1.Enabled = true;
                    trackBar2.Enabled = false;
                    trackBar3.Enabled = false;
                    trackBar1.Minimum = 0; trackBar1.Maximum = 100;
                    trackBar1.Value = 0;
                    numericUpDown1.Minimum = 0; numericUpDown1.Maximum = 100;
                    break;
                case "radioButton5":
                    trackBar1.Enabled = true;
                    trackBar2.Enabled = true;
                    trackBar3.Enabled = true;
                    trackBar1.Minimum = -360; trackBar1.Maximum = 360;
                    trackBar2.Minimum = -360; trackBar2.Maximum = 360;
                    trackBar2.Minimum = -360; trackBar2.Maximum = 360;
                    trackBar1.Value = 0; trackBar2.Value = 0; trackBar2.Value = 0;
                    numericUpDown1.Minimum = -360; numericUpDown1.Maximum = 360;
                    numericUpDown2.Minimum = -360; numericUpDown2.Maximum = 360;
                    numericUpDown3.Minimum = -360; numericUpDown3.Maximum = 360;
                    break;
            }
        }

    }
}
