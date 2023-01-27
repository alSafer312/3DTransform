using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab10
{
    struct ShapeData
    {
        public float[,] StartM;
        public float[,] TransformedM;
        public float[,] NormalizedM;
        public int[,] DisplayM;
    }
    class Calculator
    {
        public void MultiplyMatrix(float[,] m1, float[,] m2, float[,] result)
        {
            if (m1.GetLength(1) != m2.GetLength(0))
            {
                System.Windows.Forms.MessageBox.Show("Error! m1.col != m2.row");
                return;
            }
            for (int i = 0; i < result.GetLength(0); i++)
            {
                for (int j = 0; j < result.GetLength(1); j++)
                {
                    result[i, j] = 0;
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
        public void Normalization(float[,] transformedMx, float[,] resultMx)
        {
            for (int i = 0; i < transformedMx.GetLength(0); i++)
            {
                for (int j = 0; j < transformedMx.GetLength(1); j++)
                {
                    if (transformedMx[i, 3] != 1)
                    {
                        if (transformedMx[i, 3] == 0)
                        {
                            transformedMx[i, 3] = 0.01f;
                        }
                        else
                        {
                            resultMx[i, j] = transformedMx[i, j] / transformedMx[i, 3];
                        }
                    }
                    else
                    {
                        resultMx[i, j] = transformedMx[i, j];
                    }
                }
            }
        }
        public void CalculateDisplayCoord(float[,] NormalMx, int[,] DisplayMx, int W, int H, float iw, float ih)
        {
            for (int i = 0; i < NormalMx.GetLength(0); i++)
            {
                for (int j = 0; j < NormalMx.GetLength(1); j++)
                {
                    if (j == 0)
                    {
                        DisplayMx[i, 0] = (int)(W / 2 + (iw * NormalMx[i, j]));
                    }
                    if (j == 1)
                    {
                        DisplayMx[i, 1] = (int)(H / 2 - (ih * NormalMx[i, j]));
                    }
                }
            }
        }
        public void MainTransformationM(float[,] RotationXM, float[,] RotationYM, float[,] OrthogonalZM,
                                                                                float[,] MainTransformationM)
        {
            float[,] temp = new float[4, 4];
            MultiplyMatrix(RotationYM, RotationXM, temp);
            MultiplyMatrix(temp, OrthogonalZM, MainTransformationM);
        }
        public void AxisTransformation(ShapeData AxisData, float[,] MainTransformationM)
        {
            MultiplyMatrix(AxisData.StartM, MainTransformationM, AxisData.TransformedM);
        }
    }
}
