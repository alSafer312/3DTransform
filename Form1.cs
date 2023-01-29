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

        private Calculator calculator = new Calculator();
        private Renderer renderer = new Renderer();

        private ShapeData AxisXYZ;
        private ShapeData SurfaceXOY;
        private ShapeData SurfaceXOZ;
        private ShapeData SurfaceYOZ;
        private ShapeData PPD;
        private ShapeData PPDProjX;
        private ShapeData PPDProjY;
        private ShapeData PPDProjZ;
        int ppdA, ppdB,ppdH;

        private float[,] AxisXRotTM;
        private float[,] AxisYRotTM;
        private float[,] XOrthogTM;
        private float[,] YOrthogTM;
        private float[,] ZOrthogTM;
        private float[,] MainTM;
        private float[,] MainPrXTM;
        private float[,] MainPrYTM;
        private float[,] MainPrZTM;
        private float[,] TransposTM;
        private float[,] ScaleTM;
        private float[,] ReflectTM;
        private float[,] TM;

        private int WIDTH, HEIGHT;
        private float IntervalCount, IntervalW, IntervalH, W, H;
        private float rad;
        private int rotX;
        private int rotY;
        private float a, b, c, p,
                      d, e, f, q,
                      h, i, j, r,
                      l, m, n, s;
        private float alfa, betta, kfc;

        private Graphics gfx0;
        private Graphics gfx;
        private Bitmap bmp;
        private Font ComicSans;

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
            l = 0;  m = 0; n = 0; s = 1;
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
            XOrthogTM = new float[,]
            {
                {0,0,0,0},
                {0,1,0,0},
                {0,0,1,0},
                {0,0,0,1}
            };
            YOrthogTM = new float[,]
            {
                {1,0,0,0},
                {0,0,0,0},
                {0,0,1,0},
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
                {l,m,n,s},
            };
            TransposTM = new float[,]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { l, m, n, 1 }
            };
            ScaleTM = new float[,]
            {
                { 1, 0, 0, 0 },
                { 0, 1, 0, 0 },
                { 0, 0, 1, 0 },
                { 0, 0, 0, s }
            };
            ReflectTM = new float[,]
                {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 }
            };
            MainTM = new float[4, 4];
            MainPrXTM = new float[4, 4];
            MainPrYTM = new float[4, 4];
            MainPrZTM = new float[4, 4];

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

            //PPDProjX.StartM = PPD.StartM;
            //PPDProjY.StartM = PPD.StartM;
            //PPDProjZ.StartM = PPD.StartM;
            PPDProjX.TransformedM = new float[8, 4];
            PPDProjY.TransformedM = new float[8, 4];
            PPDProjZ.TransformedM = new float[8, 4];
            PPDProjX.NormalizedM = new float[8, 4];
            PPDProjY.NormalizedM = new float[8, 4];
            PPDProjZ.NormalizedM = new float[8, 4];
            PPDProjX.DisplayM = new int[8, 4];
            PPDProjY.DisplayM = new int[8, 4];
            PPDProjZ.DisplayM = new int[8, 4];                     

            ComicSans = new Font("Comic Sans", 8);
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
                calculator.AxisSrfcTransfotmation(SurfaceXOY, SurfaceXOZ, SurfaceYOZ, MainTM);
                calculator.Normalization(SurfaceXOY.TransformedM, SurfaceXOY.NormalizedM);
                calculator.Normalization(SurfaceXOZ.TransformedM, SurfaceXOZ.NormalizedM);
                calculator.Normalization(SurfaceYOZ.TransformedM, SurfaceYOZ.NormalizedM);
                calculator.CalculateDisplayCoord(SurfaceXOY.NormalizedM, SurfaceXOY.DisplayM, 
                                                            WIDTH, HEIGHT, IntervalW, IntervalH);
                calculator.CalculateDisplayCoord(SurfaceXOZ.NormalizedM, SurfaceXOZ.DisplayM,
                                                            WIDTH, HEIGHT, IntervalW, IntervalH);
                calculator.CalculateDisplayCoord(SurfaceYOZ.NormalizedM, SurfaceYOZ.DisplayM,
                                                            WIDTH, HEIGHT, IntervalW, IntervalH);
                renderer.DrawAxisSrfc(SurfaceXOY, SurfaceXOZ, SurfaceYOZ, gfx0);
            }
            if (checkBox2.Checked)
            {
                /*
                calculator.MultiplyMatrix(PPD.StartM, MainTM, PPD.TransformedM);
                calculator.Normalization(PPD.TransformedM, PPD.NormalizedM);
                calculator.CalculateDisplayCoord(PPD.NormalizedM, PPD.DisplayM,
                                                            WIDTH, HEIGHT, IntervalW, IntervalH);
                renderer.DrawPPD(PPD, gfx0);
                */

                calculator.ShapeTransformation(PPD, MainTM, TransposTM, ScaleTM, TM);
                calculator.Normalization(PPD.TransformedM, PPD.NormalizedM);
                calculator.CalculateDisplayCoord(PPD.NormalizedM, PPD.DisplayM, WIDTH, HEIGHT, IntervalW, IntervalH);
                renderer.DrawPPD(PPD, gfx0);

                calculator.XYZProfectionTM( MainTM, TransposTM, ScaleTM, 
                                           XOrthogTM, YOrthogTM, ZOrthogTM,
                                           MainPrXTM, MainPrYTM, MainPrZTM);
                if (checkBox3.Checked)
                {
                    calculator.MultiplyMatrix(PPD.StartM, MainPrXTM, PPDProjX.TransformedM);
                    calculator.Normalization(PPDProjX.TransformedM, PPDProjX.NormalizedM);
                    calculator.CalculateDisplayCoord(PPDProjX.NormalizedM, PPDProjX.DisplayM, WIDTH, HEIGHT, IntervalW, IntervalH);
                    renderer.DrawPPDProj(PPDProjX, gfx0);
                    renderer.DrawPPDProjRays(PPD, PPDProjX, gfx0);
                }
                if (checkBox4.Checked)
                {
                    calculator.MultiplyMatrix(PPD.StartM, MainPrYTM, PPDProjY.TransformedM);
                    calculator.Normalization(PPDProjY.TransformedM, PPDProjY.NormalizedM);
                    calculator.CalculateDisplayCoord(PPDProjY.NormalizedM, PPDProjY.DisplayM, WIDTH, HEIGHT, IntervalW, IntervalH);
                    renderer.DrawPPDProj(PPDProjY, gfx0);
                    renderer.DrawPPDProjRays(PPD, PPDProjY, gfx0);
                }
                if (checkBox5.Checked)
                {
                    calculator.MultiplyMatrix(PPD.StartM, MainPrZTM, PPDProjZ.TransformedM);
                    calculator.Normalization(PPDProjZ.TransformedM, PPDProjZ.NormalizedM);
                    calculator.CalculateDisplayCoord(PPDProjZ.NormalizedM, PPDProjZ.DisplayM, WIDTH, HEIGHT, IntervalW, IntervalH);
                    renderer.DrawPPDProj(PPDProjZ, gfx0);
                    renderer.DrawPPDProjRays(PPD, PPDProjZ, gfx0);
                }
            }
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


        /*
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
        }*/
        private void button2_Click(object sender, EventArgs e)
        {

        }
        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
             trackBar1.Value = (int)numericUpDown1.Value;
        }
        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            trackBar2.Value = (int)numericUpDown2.Value;
        }
        private void numericUpDown3_ValueChanged(object sender, EventArgs e)
        {
            trackBar3.Value = (int)numericUpDown3.Value;
        }
        
        private void trackBar1_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown1.Value = trackBar1.Value;
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
            if (radioButton3.Checked && checkBox2.Checked)
            {
                l = trackBar1.Value;
                numericUpDown1.Value = (decimal)l;
                TransposTM[3, 0] = l;
                PictureBoxUpdate();
            }
            if (radioButton4.Checked && checkBox2.Checked)
            {
                s = 0.1f * trackBar1.Value;
                numericUpDown1.Value = trackBar1.Value;
                ScaleTM[3, 3] = s;
                PictureBoxUpdate();
            }
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
            if (radioButton3.Checked && checkBox2.Checked)
            {
                m = trackBar2.Value;
                numericUpDown2.Value = (decimal)m;
                TransposTM[3, 1] = m;
                PictureBoxUpdate();
            }
        }
        private void trackBar3_ValueChanged(object sender, EventArgs e)
        {
            numericUpDown3.Value = trackBar3.Value;
            if (radioButton3.Checked && checkBox2.Checked)
            {
                n = trackBar3.Value;
                numericUpDown3.Value = (decimal)n;
                TransposTM[3, 2] = n;
                PictureBoxUpdate();
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            trackBar1.Enabled = true;
            trackBar2.Enabled = true;
            trackBar3.Enabled = true;
            trackBar1.Minimum = -100; trackBar1.Maximum = 100;
            trackBar2.Minimum = -100; trackBar2.Maximum = 100;
            trackBar3.Minimum = -100; trackBar3.Maximum = 100;
            numericUpDown1.Minimum = -100; numericUpDown1.Maximum = 100;
            numericUpDown2.Minimum = -100; numericUpDown2.Maximum = 100;
            numericUpDown3.Minimum = -100; numericUpDown3.Maximum = 100;
            trackBar1.Value = 0; numericUpDown1.Value = 0;
            trackBar2.Value = 0; numericUpDown2.Value = 0;
            trackBar3.Value = 0; numericUpDown3.Value = 0;
            
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
                    trackBar1.Minimum = 1; trackBar1.Maximum = 50;                    
                    numericUpDown1.Minimum = 1; numericUpDown1.Maximum = 50;
                    trackBar1.Value = 10;
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
        private void checkBox3_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                PictureBoxUpdate();
            }
        }
        private void checkBox4_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                PictureBoxUpdate();
            }
        }
        private void checkBox5_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked)
            {
                PictureBoxUpdate();
            }
        }
    }
}
