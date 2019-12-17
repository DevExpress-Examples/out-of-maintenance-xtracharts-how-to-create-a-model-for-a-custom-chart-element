using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Designer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace CustomChartElementModel {
    public partial class Form1 : Form {
        void OnButtonClick(object sender, EventArgs e) {
            ChartDesigner chartDesigner = new ChartDesigner((ChartControl)this.Controls["MyChart"]);
            chartDesigner.RegisterCustomModelType(typeof(CustomPointColorizer), typeof(CustomPointColorizerModel));
            chartDesigner.RegisterCustomModelType(typeof(SideBySideBarSeriesView), typeof(SideBySideBarSeriesViewCustomModel));
            chartDesigner.ShowDialog(true);
        }
        #region CustomPointColorizerModel
        public class CustomPointColorizerModel : ChartColorizerBaseModel {
            CustomPointColorizer MyColorizer { get { return (CustomPointColorizer)Colorizer; } } 
            public double Value {
                get { return MyColorizer.Value; }
                set { SetProperty("Value", value); }
            }
            public Color LowerValuePointColor {
                get { return MyColorizer.LowerValuePointColor; }
                set { SetProperty("LowerValuePointColor", value); }
            }
            public Color UpperValuePointColor {
                get { return MyColorizer.UpperValuePointColor; }
                set { SetProperty("UpperValuePointColor", value); }
            }
            public CustomPointColorizerModel(ChartColorizerBase element, CustomModelProvider customModelProvider) : base(element, customModelProvider) {
            }
        }
        #endregion
        #region SideBySideBarSeriesViewCustomModel
        public class SideBySideBarSeriesViewCustomModel : SideBySideBarSeriesViewModel {
            [Editor(typeof(CustomColorizerEditor), typeof(UITypeEditor))]
            public new ChartColorizerBaseModel Colorizer {
                get { return base.Colorizer; }
                set { base.Colorizer = value; }
            }

            public SideBySideBarSeriesViewCustomModel(SideBySideBarSeriesView element, CustomModelProvider customModelProvider) 
                : base(element, customModelProvider) {
            }
        }
        #endregion
        #region CustomPointColorizer
        [TypeConverter(typeof(ExpandableObjectConverter))]
        public class CustomPointColorizer : ChartColorizerBase {
            double threshold = 60;
            Color lower = Color.Red;
            Color upper = Color.Green;

            public double Value {
                get { return threshold; }
                set { threshold = value; }
            }
            public Color LowerValuePointColor {
                get { return lower; }
                set { lower = value; }
            }
            public Color UpperValuePointColor {
                get { return upper; }
                set { upper = value; }
            }
            public override Color GetAggregatedPointColor(object argument, object[] values, SeriesPoint[] points, Palette palette) {
                if ((double)values[0] > Value)
                    return UpperValuePointColor;
                else
                    return LowerValuePointColor;
            }
            public override Color GetPointColor(object argument, object[] values, object colorKey, Palette palette) {
                throw new NotImplementedException();
            }
            protected override ChartElement CreateObjectForClone() {
                return new CustomPointColorizer();
            }
            public override void Assign(ChartElement obj) {
                base.Assign(obj);
                CustomPointColorizer colorizer = obj as CustomPointColorizer;
                if (colorizer != null) {
                    Value = colorizer.Value;
                    LowerValuePointColor = colorizer.LowerValuePointColor;
                    UpperValuePointColor = colorizer.UpperValuePointColor;
                }
            }
            public override string ToString() {
                return "(CustomPointColorizer)";
            }
        }
        #endregion
        public Form1() {
            InitializeComponent();
        }
        #region ChartConfiguration
        void Form1_Load(object sender, EventArgs e) {
            ChartControl chart = new ChartControl();
            chart.Name = "MyChart";
            chart.Dock = DockStyle.Fill;
            this.Controls.Add(chart);

            Series series = new Series("Temperature", ViewType.Bar);
            series.DataSource = DataPoint.GetDataPoints();
            series.ArgumentDataMember = "Date";
            series.ValueDataMembers.AddRange("Value");
            chart.Series.Add(series);

            SideBySideBarSeriesView view = (SideBySideBarSeriesView)series.View;
            
            view.Colorizer = new CustomPointColorizer() {
                Value = 60,
                LowerValuePointColor = Color.Red,
                UpperValuePointColor = Color.Green
            };

            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True;
            series.Label.ResolveOverlappingMode = ResolveOverlappingMode.HideOverlapped;
            series.Label.TextPattern = "{V:.#}";

            ChartTitle chartTitle = new ChartTitle();
            chartTitle.Text = "Temperature (°F)";
            chart.Titles.Add(chartTitle);

            XYDiagram diagram = chart.Diagram as XYDiagram;
            diagram.AxisX.Label.TextPattern = "{A:MMM, d (HH:mm)}";
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour;
            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 9;
            diagram.AxisX.WholeRange.SideMarginsValue = 1.2;
            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = false;

            chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False;
        }
        #endregion

        public class DataPoint {
            public DateTime Date { get; set; }
            public double Value { get; set; }
            public DataPoint(DateTime date, double value) {
                this.Date = date;
                this.Value = value;
            }
            public static List<DataPoint> GetDataPoints() {
                List<DataPoint> data = new List<DataPoint> {
                new DataPoint(new DateTime(2019, 6, 1, 0, 0, 0), 56.1226),
                new DataPoint(new DateTime(2019, 6, 1, 3, 0, 0), 50.18432),
                new DataPoint(new DateTime(2019, 6, 1, 6, 0, 0), 51.51443),
                new DataPoint(new DateTime(2019, 6, 1, 9, 0, 0), 60.2624),
                new DataPoint(new DateTime(2019, 6, 1, 12, 0, 0), 64.04412),
                new DataPoint(new DateTime(2019, 6, 1, 15, 0, 0), 66.56123),
                new DataPoint(new DateTime(2019, 6, 1, 18, 0, 0), 65.48127),
                new DataPoint(new DateTime(2019, 6, 1, 21, 0, 0), 60.4412),
                new DataPoint(new DateTime(2019, 6, 2, 0, 0, 0), 57.2341),
                new DataPoint(new DateTime(2019, 6, 2, 3, 0, 0), 52.3469),
                new DataPoint(new DateTime(2019, 6, 2, 6, 0, 0), 51.82341),
                new DataPoint(new DateTime(2019, 6, 2, 9, 0, 0), 61.532),
                new DataPoint(new DateTime(2019, 6, 2, 12, 0, 0), 63.8641),
                new DataPoint(new DateTime(2019, 6, 2, 15, 0, 0), 65.12374),
                new DataPoint(new DateTime(2019, 6, 2, 18, 0, 0), 65.6321)};
                return data;
            }
        }
    }
}
