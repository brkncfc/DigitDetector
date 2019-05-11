using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace DigitDetector
{
    [Serializable]
    public class Weight
    {
        public Matrix W1 { get; set; }
        public Matrix W2 { get; set; }
        public Matrix W3 { get; set; }

        public Weight(NeuralNetwork NN)
        {
            W1 = NN.W1;
            W2 = NN.W2;
            W3 = NN.W3;
        }
    }

    public class NeuralNetwork
    {
        private int InputNodeN, Layer1NodeN, Layer2NodeN, OutNodeN;
        public Matrix a1, a2, a3, z1, z2, z3, del3, W1, W2, W3, W1_delta, W2_delta, W3_delta, Err, In, InLabel;
        public Weight Weights;

        public NeuralNetwork(Form1 form, int inputs, int L1Nodes, int L2Nodes, int outNodes)
        {
            InputNodeN = inputs;
            Layer1NodeN = L1Nodes;
            Layer2NodeN = L2Nodes;
            OutNodeN = outNodes;

            W1 = new Matrix(L1Nodes, inputs);
            W1_delta = new Matrix(L1Nodes, inputs);
            W1.Randomize();

            W2 = new Matrix(L2Nodes, L1Nodes);
            W2_delta = new Matrix(L2Nodes, L1Nodes);
            W2.Randomize();

            W3 = new Matrix(outNodes, L2Nodes);
            W3_delta = new Matrix(outNodes, L2Nodes);
            W3.Randomize();

            Err = new Matrix(outNodes, 1);
            InLabel = new Matrix(outNodes, 1);

            SaveLoad.Load(this);
            W1 = Weights.W1;
            W2 = Weights.W2;
            W3 = Weights.W3;
        }

        public int GuessDrawing(float[,] mat)
        {
            Matrix IN1 = Matrix.Mat1D(mat, InputNodeN);
            z1 = W1 * IN1;
            a1 = Sigmoid(z1);
            z2 = W2 * a1;
            a2 = Sigmoid(z2);
            z3 = W3 * a2;
            a3 = Sigmoid(z3);

            return a3.MaxInd();
        }

        Matrix Sigmoid(Matrix a)
        {
            return 1.0f / (1.0f + a.ExpMinus());
        }

    }
}
