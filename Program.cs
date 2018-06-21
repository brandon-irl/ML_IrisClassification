using System;
using System.IO;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using Microsoft.ML.Data;

namespace IrisClassification
{
	class Program
	{
		const string path = "./data/iris-data.csv";
		const string testPath = "./data/iris-data_test.csv";
		const string trainingPath = "./data/iris-data_training.csv";

		static void Main(string[] args)
		{
			var url = new Uri("https://archive.ics.uci.edu/ml/machine-learning-databases/iris/iris.data");
			EnsureData(url);

			// Building and evaluating model.
			var modelBuilder = new ModelBuilder(trainingPath);
			var model = modelBuilder.BuildAndTrain();
			var accuracy = modelBuilder.Evaluate(model, testPath);

			Console.WriteLine($"*************************************************");
			Console.WriteLine($"*        Accuracy of the model : {accuracy}     *");
			Console.WriteLine($"*************************************************");

			// Visualising the results
			var testDataObjects = new IrisCsvReader().GetIrisDataFromCsv(testPath);
			foreach (var iris in testDataObjects)
			{
				var prediction = model.Predict(iris);
				Console.WriteLine($"-------------------------------------------------");
				Console.WriteLine($"Predicted type : {prediction.PredictedLabels}");
				Console.WriteLine($"Actual type :    {iris.Label}");
				Console.WriteLine($"-------------------------------------------------");
			}
		}

		static void EnsureData(Uri uri)
		{
			if (!File.Exists(path) || !File.Exists(testPath) || !File.Exists(trainingPath))
			{
				using (var client = new WebClient())
					client.DownloadFile(uri, path);

				using (var streamReader = File.OpenText(path))
				{
					using (var testWriter = File.CreateText(testPath))
					{
						using (var trainingWriter = File.CreateText(trainingPath))
						{
							var i = 1;
							while (!streamReader.EndOfStream)
							{
								var line = streamReader.ReadLine();
								if (i % 5 == 0)
									testWriter.WriteLine(line);
								else
									trainingWriter.WriteLine(line);
								i++;
							}
						}
					}
				}
			}
		}
	}
}

