using System;
using System.Drawing;
using System.Linq;
using PowerLanguage.Function;

namespace PowerLanguage.Indicator
{	
	[SameAsSymbol(true)]
	public class Ehlers_Optimal_Tracking_Filter : IndicatorObject 
	{	
		[Input]
		public EotfPrices ui_Price { get; set; }
		
		// Declare PLOT and FUNCTION objects
		private IPlotObject plot_Eotf;
		private Function.Ehlers_Optimal_Tracking_Filter_Series fn_Eotf;
		
		// Constructor
		public Ehlers_Optimal_Tracking_Filter(object _ctx):base(_ctx) {}
		
		protected override void Create() 
		{
			// Instantiate plot objects
			plot_Eotf = AddPlot(new 
				PlotAttributes("Eotf", EPlotShapes.Line, Color.Yellow,
				Color.Transparent, 1, EPlotStyle.Solid, false));
			
			// Instantiate function objects
			fn_Eotf = new Function.Ehlers_Optimal_Tracking_Filter_Series(this);
		}
		protected override void StartCalc() 
		{
			// set function's settings (properties)
			fn_Eotf.EotfPrice = GetEotfPrice(ui_Price);
		}
		protected override void CalcBar()
		{
			// Update plot on every bar
			plot_Eotf.Set(fn_Eotf[0]);
		}
		
		public enum EotfPrices 
		{
			Median,
			TrendBiasedExtreme,
			HeikenAshiBetterFormulaMedianBody,
			HeikenAshiBetterFormulaTrendBiasedExtreme
		}
		
		string GetEotfPrice(EotfPrices price)
		{
			switch (price)
			{
				case EotfPrices.Median:
					return "median";
				case EotfPrices.TrendBiasedExtreme:
					return "trendbiasedextreme";
				case EotfPrices.HeikenAshiBetterFormulaMedianBody:
					return "heikenashibetterformulamedianbody";
				case EotfPrices.HeikenAshiBetterFormulaTrendBiasedExtreme:
					return "heikenashibetterformulatrendbiasedextreme";
				default:
					return "median";
			}
		}
	}
}