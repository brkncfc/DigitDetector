using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace DigitDetector
{
    public static class SaveLoad
    {
        private static readonly BinaryFormatter formatter = new BinaryFormatter();

        public static void Load(NeuralNetwork nn)
        {
            MemoryStream MStream = new MemoryStream(Properties.Resources.weights);
            nn.Weights = (Weight)formatter.Deserialize(MStream);
        }

    }
}
