using DevExpress.XtraCharts;
using DevExpress.XtraCharts.Designer;
using DevExpress.XtraEditors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms.Design;
using static CustomChartElementModel.Form1;

namespace CustomChartElementModel {
    public class CustomColorizerEditor : UITypeEditor {
        IWindowsFormsEditorService editorService;
        public override object EditValue(ITypeDescriptorContext context, IServiceProvider provider, object value) {
            editorService = (IWindowsFormsEditorService)provider.GetService(typeof(IWindowsFormsEditorService));
            var seriesView = context.Instance as SeriesViewBaseModel;

            List<ChartColorizerBase> colorizers = GetColorizers();

            CustomModelProvider modelProvider = new CustomModelProvider();
            modelProvider.RegisterCustomModelType(typeof(CustomPointColorizer), typeof(CustomPointColorizerModel));

            var listBox = new ListBoxControl();
            listBox.Click += listBox_Click;
            listBox.Items.Add("(None)");
            foreach (ChartColorizerBase colorizer in colorizers) {
                var colorizerModel = ModelHelper.GetModel<ChartColorizerBaseModel>(colorizer, modelProvider);
                int index = listBox.Items.Add(colorizerModel);
                if (value != null && colorizerModel.GetType() == value.GetType()) {
                    listBox.SelectedIndex = index;
                }
            }
            editorService.DropDownControl(listBox);
            if (listBox.SelectedIndex != 0)
                if (value == null || listBox.SelectedItem.GetType() != value.GetType())
                    return listBox.SelectedItem;
                else
                    return value;
            else
                return null;
        }
        void listBox_Click(object sender, EventArgs e) {
            this.editorService.CloseDropDown();
        }
        List<ChartColorizerBase> GetColorizers() {
            return new List<ChartColorizerBase>() { new CustomPointColorizer(), new KeyColorColorizer(), new RangeColorizer() };
        }
        public override UITypeEditorEditStyle GetEditStyle(ITypeDescriptorContext context) {
            return UITypeEditorEditStyle.DropDown;
        }
    }
}