using System;
using System.Drawing;
using System.Linq;


namespace PowerLanguage
{
	namespace Function
	{
		public sealed class Ehlers_Optimal_Tracking_Filter_Series : FunctionSeries<System.Double>
		{
			public string EotfPrice { get; set; }
			
			private VariableSeries<Double> ma;
			private VariableSeries<Double> _Price;
        	private VariableSeries<Double> Value1;
			private VariableSeries<Double> Value2;
			private double period;
			
			// Constructors
			public Ehlers_Optimal_Tracking_Filter_Series(CStudyControl _master) : base(_master) { }
			public Ehlers_Optimal_Tracking_Filter_Series(CStudyControl _master, int _ds) : base(_master, _ds) { }
			
			protected override void Create()
			{
				ma = new VariableSeries<Double>(this);
				_Price = new VariableSeries<Double>(this);
				Value1 = new VariableSeries<Double>(this);
				Value2 = new VariableSeries<Double>(this);
			}

			protected override void StartCalc()
			{
				period = 1;
			}
			
			protected override System.Double CalcBar()
			{
				int i = 0;	
				
				////////////////////////////////////////////////////////////////////
				// ~BASED ON USER INPUT PRICE
				if (string.Equals(EotfPrice, "median"))
				{
					_Price.Value = (Bars.High[i] + Bars.Low[i]) / 2.0;
				}
				else if (string.Equals(EotfPrice, "trendbiasedextreme"))
				{
					if (Bars.Close[i] > Bars.Open[i])
						_Price.Value = Bars.High[i];
					else
						_Price.Value = Bars.Low[i];
				}
				else if (string.Equals(EotfPrice, "heikenashibetterformulamedianbody"))
				{
					_Price.Value = (Bars.High[i] + Bars.Low[i]) / 2.0; // !TODO
					// _Price.Value = (Bars.Open[0] + Bars.Close[0]) / 2.0;
				}
				else if (string.Equals(EotfPrice, "heikenashibetterformulatrendbiasedextreme"))
				{
					_Price.Value = (Bars.High[i] + Bars.Low[i]) / 2.0; // !TODO
					// if heiken ashi close > open , equals high
					// else equals low
				}
				else
				{
					_Price.Value = (Bars.High[i] + Bars.Low[i]) / 2.0;
				}
				//
				////////////////////////////////////////////////////////////////////
				
				Value1.Value = .2*(_Price[i] - _Price[i+1]) + .8*Value1[i+1];
				Value2.Value = .1*(Bars.High[i] - Bars.Low[i]) + .8*Value2[i+1];
				
				double lambda = 0;
				if (Value2[i] != 0)
					lambda = Math.Abs(Value1[i] / Value2[i]) / (double)period;
				
				double alpha = (-lambda*lambda + Math.Sqrt(lambda*lambda*lambda*lambda + 16*lambda*lambda)) / 8.0;
				ma.Value = alpha*_Price[i] + (1-alpha)*ma[i+1];
				
				return ma[i];
			}
		}
	}
}