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
        public void CalcPPDRotateTM(float[,] RotationXM, float[,] RotationYM, float[,] RotationZM,
                                                                                float[,] PPDRotationTM)
        {
            float[,] temp = new float[4, 4];
            MultiplyMatrix(RotationYM, RotationXM, temp);
            MultiplyMatrix(temp, RotationZM, PPDRotationTM);
        }
        public void CalcPPDReflectTM(float[,] ReflectionXTM, float[,] ReflectionYTM, float[,] ReflectionZTM,
                                                                                float[,] PPDReflectionTM)
        {
            float[,] temp = new float[4, 4];
            MultiplyMatrix(ReflectionXTM, ReflectionYTM, temp);
            MultiplyMatrix(temp, ReflectionZTM, PPDReflectionTM);
        }
        public void AxisTransformation(ShapeData AxisData, float[,] MainTransformationM)
        {
            MultiplyMatrix(AxisData.StartM, MainTransformationM, AxisData.TransformedM);
        }
        public void AxisSrfcTransfotmation(ShapeData XOY, ShapeData XOZ, ShapeData YOZ, float[,] MainTransformationM)
        {
            MultiplyMatrix(XOY.StartM, MainTransformationM, XOY.TransformedM);
            MultiplyMatrix(XOZ.StartM, MainTransformationM, XOZ.TransformedM);
            MultiplyMatrix(YOZ.StartM, MainTransformationM, YOZ.TransformedM);
        }
        public void ShapeTransformation(ShapeData shape, float[,] MainTM, float[,] TranspTM, float[,] ScaleTM, 
                                                         float[,] PPDRotateTM, float[,] PPDReflectionTM, float[,] TM)
        {
            float[,] ScalexTransp = new float[4, 4];
            float[,] ScalexTranspxRotate = new float[4, 4];
            float[,] ScalexTranspxRotatexReflect = new float[4, 4];
            MultiplyMatrix(TranspTM, ScaleTM, ScalexTransp);
            MultiplyMatrix(ScalexTransp, PPDRotateTM, ScalexTranspxRotate);
            MultiplyMatrix(ScalexTranspxRotate, PPDReflectionTM, ScalexTranspxRotatexReflect);
            MultiplyMatrix(ScalexTranspxRotatexReflect, MainTM, TM);
            MultiplyMatrix(shape.StartM, TM, shape.TransformedM);
        }
        public void XYZProjectionTM(float[,] MainTM, float[,] TranspTM, float[,] ScaleTM, 
                                        float[,] PPDRotationTM, float[,] PPDReflectTM,
                                   float[,] OrthX, float[,] OrthY, float[,] OrthZ,
                                   float[,] MainPrXTM, float[,] MainPrYTM, float[,] MainPrZTM)
        {
            float[,] TransxScale = new float[4, 4];
            float[,] TransxScalexRotation = new float[4, 4];
            float[,] TransxScalexRotationxReflect = new float[4, 4];
            float[,] tempXTM = new float[4, 4];
            float[,] tempYTM = new float[4, 4];
            float[,] tempZTM = new float[4, 4];
            MultiplyMatrix(TranspTM, ScaleTM, TransxScale);
            MultiplyMatrix(TransxScale, PPDRotationTM, TransxScalexRotation);
            MultiplyMatrix(TransxScalexRotation, PPDReflectTM, TransxScalexRotationxReflect);
            MultiplyMatrix(TransxScalexRotationxReflect, OrthX, tempXTM);
            MultiplyMatrix(TransxScalexRotationxReflect, OrthY, tempYTM);
            MultiplyMatrix(TransxScalexRotationxReflect, OrthZ, tempZTM);
            MultiplyMatrix(tempXTM, MainTM, MainPrXTM);
            MultiplyMatrix(tempYTM, MainTM, MainPrYTM);
            MultiplyMatrix(tempZTM, MainTM, MainPrZTM);
        }
        public void XYZProjectionTM(float[,] MainTM, float[,] TranspTM, float[,] ScaleTM, 
                                        float[,] PPDRotationTM, float[,] PPDReflectTM,
                                   float[,] OrthX, float[,] OrthY, float[,] OrthZ,
                                   float[,] PrObliqueX, float[,] PrObliqueY, float[,] PrObliqueZ,
                                   float[,] MainPrXTM, float[,] MainPrYTM, float[,] MainPrZTM)
        {
            float[,] TransxScale = new float[4, 4];
            float[,] TransxScalexRotation = new float[4, 4];
            float[,] TransxScalexRotationxReflect = new float[4, 4];
            float[,] temp = new float[4, 4];

            float[,] tempOXTM = new float[4, 4];
            float[,] tempOYTM = new float[4, 4];
            float[,] tempOZTM = new float[4, 4];
            MultiplyMatrix(TranspTM, ScaleTM, TransxScale);
            MultiplyMatrix(TransxScale, PPDRotationTM, TransxScalexRotation);
            MultiplyMatrix(TransxScalexRotation, PPDReflectTM, TransxScalexRotationxReflect);
            MultiplyMatrix(TransxScalexRotationxReflect, PrObliqueX, temp);
            MultiplyMatrix(temp, OrthX, tempOXTM);
            MultiplyMatrix(TransxScalexRotationxReflect, PrObliqueY, temp);
            MultiplyMatrix(temp, OrthY, tempOYTM);
            MultiplyMatrix(TransxScalexRotationxReflect, PrObliqueZ, temp);
            MultiplyMatrix(temp, OrthZ, tempOZTM);
            MultiplyMatrix(tempOXTM, MainTM , MainPrXTM);
            MultiplyMatrix(tempOYTM, MainTM, MainPrYTM);
            MultiplyMatrix(tempOZTM, MainTM, MainPrZTM);
        }
    }
}
