Imports DevExpress.XtraCharts
Imports DevExpress.XtraCharts.Designer
Imports DevExpress.XtraEditors
Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Windows.Forms.Design
Imports CustomChartElementModel.Form1

Namespace CustomChartElementModel

    Public Class CustomColorizerEditor
        Inherits UITypeEditor

        Private editorService As IWindowsFormsEditorService

        Public Overrides Function EditValue(ByVal context As ITypeDescriptorContext, ByVal provider As IServiceProvider, ByVal value As Object) As Object
            editorService = CType(provider.GetService(GetType(IWindowsFormsEditorService)), IWindowsFormsEditorService)
            Dim seriesView = TryCast(context.Instance, SeriesViewBaseModel)
            Dim colorizers As List(Of ChartColorizerBase) = GetColorizers()
            Dim modelProvider As CustomModelProvider = New CustomModelProvider()
            modelProvider.RegisterCustomModelType(GetType(CustomPointColorizer), GetType(CustomPointColorizerModel))
            Dim listBox = New ListBoxControl()
            AddHandler listBox.Click, AddressOf listBox_Click
            listBox.Items.Add("(None)")
            For Each colorizer As ChartColorizerBase In colorizers
                Dim colorizerModel = ModelHelper.GetModel(Of ChartColorizerBaseModel)(colorizer, modelProvider)
                Dim index As Integer = listBox.Items.Add(colorizerModel)
                If value IsNot Nothing AndAlso colorizerModel.GetType() Is value.GetType() Then
                    listBox.SelectedIndex = index
                End If
            Next

            editorService.DropDownControl(listBox)
            If listBox.SelectedIndex <> 0 Then
                If value Is Nothing OrElse listBox.SelectedItem.GetType() IsNot value.GetType() Then
                    Return listBox.SelectedItem
                Else
                    Return value
                End If
            Else
                Return Nothing
            End If
        End Function

        Private Sub listBox_Click(ByVal sender As Object, ByVal e As EventArgs)
            editorService.CloseDropDown()
        End Sub

        Private Function GetColorizers() As List(Of ChartColorizerBase)
            Return New List(Of ChartColorizerBase)() From {New CustomPointColorizer(), New KeyColorColorizer(), New RangeColorizer()}
        End Function

        Public Overrides Function GetEditStyle(ByVal context As ITypeDescriptorContext) As UITypeEditorEditStyle
            Return UITypeEditorEditStyle.DropDown
        End Function
    End Class
End Namespace
