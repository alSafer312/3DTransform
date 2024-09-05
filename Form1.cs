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
        private float[,] PPDXRotTM;
        private float[,] PPDYRotTM;
        private float[,] PPDZRotTM;
        private float[,] XOrthogTM;
        private float[,] YOrthogTM;
        private float[,] ZOrthogTM;
        private float[,] MainTM;
        private float[,] MainPrXTM;
        private float[,] MainPrYTM;
        private float[,] MainPrZTM;
        private float[,] PrObliqueXTM;
        private float[,] PrObliqueYTM;
        private float[,] PrObliqueZTM;
        private float[,] PrSingleXTM;
        private float[,] PrSingleYTM;
        private float[,] PrSingleZTM;
        private float[,] TransposTM;
        private float[,] ScaleTM;
        private float[,] ReflectionXTM;
        private float[,] ReflectionYTM;
        private float[,] ReflectionZTM;
        private float[,] PPDReflectTM;
        private float[,] PPDRotateTM;
        private float[,] TM;

        private int WIDTH, HEIGHT;
        private Point CamPoint1; bool camRotation;
        private float IntervalCount, IntervalW, IntervalH, W, H;
        private float rad;
        private int rotX, ppdRotX;
        private int rotY, ppdRotY;
        private int ppdRotZ;
        private float a, b, c, p,
                      d, e, f, q,
                      h, i, j, r,
                      l, m, n, s;
        private float alpha, betta, kfc;

        private Graphics gfx0;
        private Graphics gfx;
        private Bitmap bmp;
        private Font ComicSans;

        private void button5_Click(object sender, EventArgs e)
        {
            Application.Restart();
        }

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
            ppdRotX = 0; ppdRotY = 0; ppdRotZ = 0;
            camRotation = false;

            a = 1; b = 0; c = 0; p = 0;
            d = 0; this.e = 1; f = 0; q = 0;
            h = 0; i = 0; j = 1; r = 0;
            l = 0;  m = 0; n = 0; s = 1;

            alpha = 0; betta = 0; kfc = 1;

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
            PPDXRotTM = new float[,]
            {
                {1,0,0,0},
                {0, (float)Math.Cos(rad * ppdRotX), (float)Math.Sin(rad * ppdRotX), 0},
                {0, (float)-Math.Sin(rad * ppdRotX), (float)Math.Cos(rad * ppdRotX), 0},
                {0,0,0,1}
            };
            PPDYRotTM = new float[,]
            {
                {(float)Math.Cos(rad * ppdRotX), 0, (float)-Math.Sin(rad * ppdRotX), 0},
                {0,1,0,0},
                {(float)Math.Sin(rad * ppdRotY), 0, (float)Math.Cos(rad * ppdRotY), 0},
                {0,0,0,1}
            };
            PPDZRotTM = new float[,]
            {
                {(float)Math.Cos(rad * ppdRotZ), (float)Math.Sin(rad * ppdRotZ), 0, 0},
                {(float)-Math.Sin(rad * ppdRotZ), (float)Math.Cos(rad * ppdRotZ), 0, 0},
                {0,0,1,0},
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
            PrObliqueXTM = new float[,]
            {
                {1, kfc * (float) Math.Sin(rad * alpha), kfc * (float) Math.Cos(rad * alpha), 0},
                {0,1,0,0},
                {0,0,1,0},
                {0,0,0,1}
            };
            PrObliqueYTM = new float[,]
            {
                {1,0,0,0},
                {kfc * (float) Math.Sin(rad * alpha), 1, kfc * (float) Math.Cos(rad * alpha), 0},
                {0,0,1,0},
                {0,0,0,1}
            };
            PrObliqueZTM = new float[,]
            {
                {1,0,0,0},
                {0,1,0,0},
                {kfc * (float) Math.Cos(rad * alpha), kfc * (float) Math.Sin(rad * alpha), 1, 0},
                {0,0,0,1}
            };
            PrSingleXTM = new float[,]
            {
                {0,0,0,p},
                {0,1,0,0},
                {0,0,1,0},
                {0,0,0,1}
            };
            PrSingleYTM = new float[,]
            {
                {1,0,0,0},
                {0,0,0,q},
                {0,0,1,0},
                {0,0,0,1}
            };
            PrSingleZTM = new float[,]
            {
                {1,0,0,0},
                {0,1,0,0},
                {0,0,0,r},
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
            ReflectionXTM = new float[,]
            {
                {a,0,0,0},
                {0,1,0,0},
                {0,0,1,0},
                {0,0,0,1}
            };
            ReflectionYTM = new float[,]
            {
                {1,0,0,0},
                {0,this.e,0,0},
                {0,0,1,0},
                {0,0,0,1}
            };
            ReflectionZTM = new float[,]
            {
                {1,0,0,0},
                {0,1,0,0},
                {0,0,j,0},
                {0,0,0,1}
            };
            /*
            ShiftTM = new float[,]
            {
                { 1, b, c, 0 },
                { d, 1, f, 0 },
                { h, i, 1, 0 },
                { 0, 0, 0, 1 }
            };
            */
            PPDRotateTM = new float[4, 4];
            PPDReflectTM = new float[4, 4];
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
                calculator.CalcPPDReflectTM( ReflectionXTM, ReflectionYTM, ReflectionZTM, PPDReflectTM);
                calculator.CalcPPDRotateTM( PPDXRotTM, PPDYRotTM, PPDZRotTM, PPDRotateTM);
                calculator.ShapeTransformation(PPD, MainTM, TransposTM, ScaleTM, PPDRotateTM, PPDReflectTM, TM);
                calculator.Normalization(PPD.TransformedM, PPD.NormalizedM);
                calculator.CalculateDisplayCoord(PPD.NormalizedM, PPD.DisplayM, WIDTH, HEIGHT, IntervalW, IntervalH);
                renderer.DrawPPD(PPD, gfx0);

                if (radioButton6.Checked)
                {
                    calculator.XYZProjectionTM(MainTM, TransposTM, ScaleTM, PPDRotateTM, PPDReflectTM,
                                           XOrthogTM, YOrthogTM, ZOrthogTM,
                                           MainPrXTM, MainPrYTM, MainPrZTM);
                }
                else if (radioButton7.Checked || radioButton8.Checked)
                {
                    calculator.XYZProjectionTM(MainTM, TransposTM, ScaleTM, PPDRotateTM, PPDReflectTM,
                                           XOrthogTM, YOrthogTM, ZOrthogTM,
                                           PrObliqueXTM, PrObliqueYTM, PrObliqueZTM,
                                           MainPrXTM, MainPrYTM, MainPrZTM);
                }
                else if (radioButton9.Checked)
                {
                    calculator.XYZProjectionTM(MainTM, TransposTM, ScaleTM, PPDRotateTM, PPDReflectTM,
                                           XOrthogTM, YOrthogTM, ZOrthogTM,
                                           PrSingleXTM, PrSingleYTM, PrSingleZTM,
                                           MainPrXTM, MainPrYTM, MainPrZTM);
                }

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
        private void button1_Click(object sender, EventArgs e)
        {
            ReflectionXTM[0, 0] = -ReflectionXTM[0, 0];
            PictureBoxUpdate();
        }
        private void button2_Click(object sender, EventArgs e)
        {
            ReflectionYTM[1, 1] = -ReflectionYTM[1, 1];
            PictureBoxUpdate();
        }
        private void button3_Click(object sender, EventArgs e)
        {
            ReflectionXTM[2, 2] = -ReflectionXTM[2, 2];
            PictureBoxUpdate();
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
            if (radioButton5.Checked && checkBox2.Checked)
            {
                ppdRotX = trackBar1.Value;
                numericUpDown1.Value = ppdRotX;

                PPDXRotTM[0, 0] = 1; PPDXRotTM[0, 1] = 0; PPDXRotTM[0, 2] = 0; PPDXRotTM[0, 3] = 0;

                PPDXRotTM[1, 0] = 0; PPDXRotTM[1, 1] = (float)Math.Cos(rad * ppdRotX);
                PPDXRotTM[1, 2] = (float)Math.Sin(rad * ppdRotX); PPDXRotTM[1, 3] = 0;

                PPDXRotTM[2, 0] = 0; PPDXRotTM[2, 1] = (float)-Math.Sin(rad * ppdRotX);
                PPDXRotTM[2, 2] = (float)Math.Cos(rad * ppdRotX); PPDXRotTM[2, 3] = 0;

                PPDXRotTM[3, 0] = 0; PPDXRotTM[3, 1] = 0; PPDXRotTM[3, 2] = 0; PPDXRotTM[3, 3] = 1;
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
            if (radioButton5.Checked && checkBox2.Checked)
            {
                ppdRotY = trackBar2.Value;
                numericUpDown2.Value = ppdRotY;

                PPDYRotTM[0, 0] = (float)Math.Cos(rad * ppdRotY); PPDYRotTM[0, 1] = 0;
                PPDYRotTM[0, 2] = (float)-Math.Sin(rad * ppdRotY); PPDYRotTM[0, 3] = 0;

                PPDYRotTM[1, 0] = 0; PPDYRotTM[1, 1] = 1; PPDYRotTM[1, 2] = 0; PPDYRotTM[1, 3] = 0;

                PPDYRotTM[2, 0] = (float)Math.Sin(rad * ppdRotY); PPDYRotTM[2, 1] = 0;
                PPDYRotTM[2, 2] = (float)Math.Cos(rad * ppdRotY); PPDYRotTM[2, 3] = 0;

                PPDYRotTM[3, 0] = 0; PPDYRotTM[3, 1] = 0; PPDYRotTM[3, 2] = 0; PPDYRotTM[3, 3] = 1;
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
            if (radioButton5.Checked && checkBox2.Checked)
            {
                ppdRotZ = trackBar3.Value;
                numericUpDown3.Value = ppdRotZ;

                PPDZRotTM[0, 0] = (float)Math.Cos(rad * ppdRotZ); PPDZRotTM[0, 1] = (float)Math.Sin(rad * ppdRotZ);
                PPDZRotTM[0, 2] = 0; PPDZRotTM[0, 3] = 0;

                PPDZRotTM[1, 0] = (float)-Math.Sin(rad * ppdRotZ); PPDZRotTM[1, 1] = (float)Math.Cos(rad * ppdRotZ);
                PPDZRotTM[1, 2] = 0; PPDZRotTM[1, 3] = 0;

                PPDZRotTM[2, 0] = 0; PPDZRotTM[2, 1] = 0; PPDZRotTM[2, 2] = 1; PPDZRotTM[2, 3] = 0;

                PPDZRotTM[3, 0] = 0; PPDZRotTM[3, 1] = 0; PPDZRotTM[3, 2] = 0; PPDZRotTM[3, 3] = 1;
                PictureBoxUpdate();
            }
        }
        private void trackBar4_ValueChanged(object sender, EventArgs e)
        {
            if(radioButton7.Checked || radioButton8.Checked)
            {
                alpha = trackBar4.Value;
                textBox3.Text = alpha.ToString();
                if (radioButton7.Checked || radioButton8.Checked)
                {
                    PrObliqueZTM[2, 0] = (float)(kfc * Math.Cos(rad * alpha));
                    PrObliqueZTM[2, 1] = (float)(kfc * Math.Sin(rad * alpha));

                    PrObliqueXTM[0, 1] = (float)(kfc * Math.Sin(rad * alpha));
                    PrObliqueXTM[0, 2] = (float)(kfc * Math.Cos(rad * alpha));

                    PrObliqueYTM[1, 0] = (float)(kfc * Math.Sin(rad * alpha));
                    PrObliqueYTM[1, 2] = (float)(kfc * Math.Cos(rad * alpha));
                    PictureBoxUpdate();
                }
            }
            if (radioButton9.Checked)
            {
                p = (float)-trackBar4.Value / 100;
                q = (float)-trackBar4.Value / 100;
                r = (float)-trackBar4.Value / 100;
                textBox3.Text = p.ToString();
                PrSingleXTM[0, 3] = p;
                PrSingleYTM[1, 3] = q;
                PrSingleZTM[2, 3] = r;
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
                    trackBar3.Minimum = -360; trackBar3.Maximum = 360;
                    trackBar1.Value = 0; trackBar2.Value = 0; trackBar3.Value = 0;
                    numericUpDown1.Minimum = -360; numericUpDown1.Maximum = 360;
                    numericUpDown2.Minimum = -360; numericUpDown2.Maximum = 360;
                    numericUpDown3.Minimum = -360; numericUpDown3.Maximum = 360;
                    break;
            }
        }
        private void radioButton6_CheckedChanged(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            switch (rb.Name.ToString())
            {
                case "radioButton6":
                    PictureBoxUpdate();
                    break;
                case "radioButton7":
                    label3.Text = "Alpha";
                    trackBar4.Minimum = -360;
                    trackBar4.Maximum = 360;
                    trackBar4.Value = 0;
                    PictureBoxUpdate();
                    break;
                case "radioButton8":
                    label3.Text = "Alpha";
                    trackBar4.Minimum = -360;
                    trackBar4.Maximum = 360;
                    trackBar4.Value = 0;
                    PictureBoxUpdate();
                    break;
                case "radioButton9":
                    label3.Text = "{ p, q, r }";
                    trackBar4.Minimum = -50;
                    trackBar4.Maximum = 50;
                    trackBar4.Value = 0;
                    PictureBoxUpdate();
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
        private void button4_Click(object sender, EventArgs e)
        {
            if (radioButton7.Checked)
            {
                betta = Convert.ToSingle(textBox2.Text);
                if (betta == 45)
                {
                    kfc = (float)(1 / Math.Tan(rad * betta));
                    textBox1.Text = kfc.ToString();
                    PrObliqueZTM[2, 0] = (float)(kfc * Math.Cos(rad * alpha));
                    PrObliqueZTM[2, 1] = (float)(kfc * Math.Sin(rad * alpha));

                    PrObliqueXTM[0, 1] = (float)(kfc * Math.Sin(rad * alpha));
                    PrObliqueXTM[0, 2] = (float)(kfc * Math.Cos(rad * alpha));

                    PrObliqueYTM[1, 0] = (float)(kfc * Math.Sin(rad * alpha));
                    PrObliqueYTM[1, 2] = (float)(kfc * Math.Cos(rad * alpha));
                }
                PictureBoxUpdate();
            }
            if (radioButton8.Checked)
            {
                kfc = (float)Convert.ToSingle(textBox1.Text);
                if (kfc == 0.5)
                {
                    betta = (float)((180 / Math.PI) * (Math.PI/2 - (Math.Atan(kfc))));
                    textBox2.Text = betta.ToString();

                    PrObliqueZTM[2, 0] = (float)(kfc * Math.Cos(rad * alpha));
                    PrObliqueZTM[2, 1] = (float)(kfc * Math.Sin(rad * alpha));

                    PrObliqueXTM[0, 1] = (float)(kfc * Math.Sin(rad * alpha));
                    PrObliqueXTM[0, 2] = (float)(kfc * Math.Cos(rad * alpha));

                    PrObliqueYTM[1, 0] = (float)(kfc * Math.Sin(rad * alpha));
                    PrObliqueYTM[1, 2] = (float)(kfc * Math.Cos(rad * alpha));
                }
                PictureBoxUpdate();
            }
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (camRotation && radioButton2.Checked)
            {
                if (-360 < (e.X - CamPoint1.X) && (e.X - CamPoint1.X) < 360 &&
                     -360 < (e.Y - CamPoint1.Y) && (e.X - CamPoint1.Y) < 360)
                {
                    trackBar1.Value = (e.Y - CamPoint1.Y) / 2;
                    trackBar2.Value = (e.X - CamPoint1.X) / 2;
                }
            }
        }
        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            camRotation = false;
        }
        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            camRotation = true;
            CamPoint1.X = e.X;
            CamPoint1.Y = e.Y;
        }

    }
}
