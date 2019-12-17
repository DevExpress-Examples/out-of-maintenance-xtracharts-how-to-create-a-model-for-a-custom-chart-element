<!-- default file list -->
*Files to look at*:

* **[MainForm.cs](./CS/CustomChartElementModel/MainForm.cs) (VB: [MainForm.vb](./VB/CustomChartElementModel/MainForm.vb))**
* [CustomColorizerEditor.cs](./CS/CustomChartElementModel/CustomColorizerEditor.cs) (VB: [CustomLegendOptionsControl.vb](./VB/CustomChartElementModel/CustomColorizerEditor.vb))
<!-- default file list end -->

# How to Create a Model for a Custom Chart Element

This example illustrates how to create and register a model (the **CustomPointColorizerModel** class in this example) for a custom chart element (the **CustomPointColorizer** class in this example).

A user configures *chart element model* properties when modifying *chart element* properties in the Chart Designer. Model changes apply to a corresponding chart element when the user clicks **OK**. The Chart Control provides models for existing chart elements. If you create a custom element (a colorizer in this example), you should create and register a model for this element to allow users to edit its options.

See the [Chart Designer for End-Users](https://docs.devexpress.com/WindowsForms/114127/controls-and-libraries/chart-control/end-user-features/chart-designer-for-end-users) help document for more information.
