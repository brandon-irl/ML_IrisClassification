using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using Microsoft.ML.Runtime.Api;

namespace IrisClassification
{
	public class IrisFlower
	{
		[Column("0")]
		public float SepalLength;

		[Column("1")]
		public float SepalWidth;

		[Column("2")]
		public float PetalLength;

		[Column("3")]
		public float PetalWidth;

		[Column("4")]
		[ColumnName("Label")]
		public string Label;
	}

	public class IrisPredict
	{
		[ColumnName("PredictedLabel")]
		public string PredictedLabels;
	}

	public class IrisCsvReader
	{
		public IEnumerable<IrisFlower> GetIrisDataFromCsv(string dataLocation)
		{
			return File.ReadAllLines(dataLocation)
				.Skip(1)
				.Select(x => x.Split(','))
				.Select(x => new IrisFlower()
				{
					SepalLength = float.Parse(x[0]),
					SepalWidth = float.Parse(x[1]),
					PetalLength = float.Parse(x[2]),
					PetalWidth = float.Parse(x[3]),
					Label = x[4]
				});
		}
	}
}