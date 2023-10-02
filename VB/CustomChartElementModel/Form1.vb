Imports DevExpress.XtraCharts
Imports DevExpress.XtraCharts.Designer
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Drawing.Design
Imports System.Windows.Forms

Namespace CustomChartElementModel

    Public Partial Class Form1
        Inherits Form

        Private Sub OnButtonClick(ByVal sender As Object, ByVal e As EventArgs)
            Dim chartDesigner As ChartDesigner = New ChartDesigner(CType(Me.Controls("MyChart"), ChartControl))
            chartDesigner.RegisterCustomModelType(GetType(CustomPointColorizer), GetType(CustomPointColorizerModel))
            chartDesigner.RegisterCustomModelType(GetType(SideBySideBarSeriesView), GetType(SideBySideBarSeriesViewCustomModel))
            chartDesigner.ShowDialog(True)
        End Sub

#Region "CustomPointColorizerModel"
        Public Class CustomPointColorizerModel
            Inherits ChartColorizerBaseModel

            Private ReadOnly Property MyColorizer As CustomPointColorizer
                Get
                    Return CType(Colorizer, CustomPointColorizer)
                End Get
            End Property

            Public Property Value As Double
                Get
                    Return MyColorizer.Value
                End Get

                Set(ByVal value As Double)
                    SetProperty("Value", value)
                End Set
            End Property

            Public Property LowerValuePointColor As Color
                Get
                    Return MyColorizer.LowerValuePointColor
                End Get

                Set(ByVal value As Color)
                    SetProperty("LowerValuePointColor", value)
                End Set
            End Property

            Public Property UpperValuePointColor As Color
                Get
                    Return MyColorizer.UpperValuePointColor
                End Get

                Set(ByVal value As Color)
                    SetProperty("UpperValuePointColor", value)
                End Set
            End Property

            Public Sub New(ByVal element As ChartColorizerBase, ByVal customModelProvider As CustomModelProvider)
                MyBase.New(element, customModelProvider)
            End Sub
        End Class

#End Region
#Region "SideBySideBarSeriesViewCustomModel"
        Public Class SideBySideBarSeriesViewCustomModel
            Inherits SideBySideBarSeriesViewModel

            <Editor(GetType(CustomColorizerEditor), GetType(UITypeEditor))>
            Public Overloads Property Colorizer As ChartColorizerBaseModel
                Get
                    Return MyBase.Colorizer
                End Get

                Set(ByVal value As ChartColorizerBaseModel)
                    MyBase.Colorizer = value
                End Set
            End Property

            Public Sub New(ByVal element As SideBySideBarSeriesView, ByVal customModelProvider As CustomModelProvider)
                MyBase.New(element, customModelProvider)
            End Sub
        End Class

#End Region
#Region "CustomPointColorizer"
        <TypeConverter(GetType(ExpandableObjectConverter))>
        Public Class CustomPointColorizer
            Inherits ChartColorizerBase

            Private threshold As Double = 60

            Private lower As Color = Color.Red

            Private upper As Color = Color.Green

            Public Property Value As Double
                Get
                    Return threshold
                End Get

                Set(ByVal value As Double)
                    threshold = value
                End Set
            End Property

            Public Property LowerValuePointColor As Color
                Get
                    Return lower
                End Get

                Set(ByVal value As Color)
                    lower = value
                End Set
            End Property

            Public Property UpperValuePointColor As Color
                Get
                    Return upper
                End Get

                Set(ByVal value As Color)
                    upper = value
                End Set
            End Property

            Public Overrides Function GetAggregatedPointColor(ByVal argument As Object, ByVal values As Object(), ByVal points As SeriesPoint(), ByVal palette As Palette) As Color
                If CDbl(values(0)) > Value Then
                    Return UpperValuePointColor
                Else
                    Return LowerValuePointColor
                End If
            End Function

            Public Overrides Function GetPointColor(ByVal argument As Object, ByVal values As Object(), ByVal colorKey As Object, ByVal palette As Palette) As Color
                Throw New NotImplementedException()
            End Function

            Protected Overrides Function CreateObjectForClone() As ChartElement
                Return New CustomPointColorizer()
            End Function

            Public Overrides Sub Assign(ByVal obj As ChartElement)
                MyBase.Assign(obj)
                Dim colorizer As CustomPointColorizer = TryCast(obj, CustomPointColorizer)
                If colorizer IsNot Nothing Then
                    Value = colorizer.Value
                    LowerValuePointColor = colorizer.LowerValuePointColor
                    UpperValuePointColor = colorizer.UpperValuePointColor
                End If
            End Sub

            Public Overrides Function ToString() As String
                Return "(CustomPointColorizer)"
            End Function
        End Class

#End Region
        Public Sub New()
            InitializeComponent()
        End Sub

#Region "ChartConfiguration"
        Private Sub Form1_Load(ByVal sender As Object, ByVal e As EventArgs)
            Dim chart As ChartControl = New ChartControl()
            chart.Name = "MyChart"
            chart.Dock = DockStyle.Fill
            Me.Controls.Add(chart)
            Dim series As Series = New Series("Temperature", ViewType.Bar)
            series.DataSource = DataPoint.GetDataPoints()
            series.ArgumentDataMember = "Date"
            series.ValueDataMembers.AddRange("Value")
            chart.Series.Add(series)
            Dim view As SideBySideBarSeriesView = CType(series.View, SideBySideBarSeriesView)
            view.Colorizer = New CustomPointColorizer() With {.Value = 60, .LowerValuePointColor = Color.Red, .UpperValuePointColor = Color.Green}
            series.LabelsVisibility = DevExpress.Utils.DefaultBoolean.True
            series.Label.ResolveOverlappingMode = ResolveOverlappingMode.HideOverlapped
            series.Label.TextPattern = "{V:.#}"
            Dim chartTitle As ChartTitle = New ChartTitle()
            chartTitle.Text = "Temperature (°F)"
            chart.Titles.Add(chartTitle)
            Dim diagram As XYDiagram = TryCast(chart.Diagram, XYDiagram)
            diagram.AxisX.Label.TextPattern = "{A:MMM, d (HH:mm)}"
            diagram.AxisX.DateTimeScaleOptions.MeasureUnit = DateTimeMeasureUnit.Hour
            diagram.AxisX.DateTimeScaleOptions.GridSpacing = 9
            diagram.AxisX.WholeRange.SideMarginsValue = 1.2
            diagram.AxisY.WholeRange.AlwaysShowZeroLevel = False
            chart.Legend.Visibility = DevExpress.Utils.DefaultBoolean.False
        End Sub

#End Region
        Public Class DataPoint

            Public Property [Date] As Date

            Public Property Value As Double

            Public Sub New(ByVal [date] As Date, ByVal value As Double)
                Me.Date = [date]
                Me.Value = value
            End Sub

            Public Shared Function GetDataPoints() As List(Of DataPoint)
                Dim data As List(Of DataPoint) = New List(Of DataPoint) From {New DataPoint(New DateTime(2019, 6, 1, 0, 0, 0), 56.1226), New DataPoint(New DateTime(2019, 6, 1, 3, 0, 0), 50.18432), New DataPoint(New DateTime(2019, 6, 1, 6, 0, 0), 51.51443), New DataPoint(New DateTime(2019, 6, 1, 9, 0, 0), 60.2624), New DataPoint(New DateTime(2019, 6, 1, 12, 0, 0), 64.04412), New DataPoint(New DateTime(2019, 6, 1, 15, 0, 0), 66.56123), New DataPoint(New DateTime(2019, 6, 1, 18, 0, 0), 65.48127), New DataPoint(New DateTime(2019, 6, 1, 21, 0, 0), 60.4412), New DataPoint(New DateTime(2019, 6, 2, 0, 0, 0), 57.2341), New DataPoint(New DateTime(2019, 6, 2, 3, 0, 0), 52.3469), New DataPoint(New DateTime(2019, 6, 2, 6, 0, 0), 51.82341), New DataPoint(New DateTime(2019, 6, 2, 9, 0, 0), 61.532), New DataPoint(New DateTime(2019, 6, 2, 12, 0, 0), 63.8641), New DataPoint(New DateTime(2019, 6, 2, 15, 0, 0), 65.12374), New DataPoint(New DateTime(2019, 6, 2, 18, 0, 0), 65.6321)}
                Return data
            End Function
        End Class
    End Class
End Namespace
