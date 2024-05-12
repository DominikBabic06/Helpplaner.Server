using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Routing;

using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.WpfGraphControl;
using Microsoft.Win32;
using Color = Microsoft.Msagl.Drawing.Color;


using ModifierKeys = System.Windows.Input.ModifierKeys;
using Size = System.Windows.Size;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using System.Threading.Tasks;


using System.Windows.Controls.Primitives;






namespace Helpplaner.Client.GUI.Pages
{
    /// <summary>
    /// Interaktionslogik für Graph.xaml
    /// </summary>
    public partial class Graph : Page
    {
        public Graph()
        {
            InitializeComponent();

            DockPanel graphViewerPanel = new DockPanel();


            Microsoft.Msagl.Drawing.Graph graph = new Microsoft.Msagl.Drawing.Graph("graph");

            graph.AddEdge("A", "B");
            graph.AddEdge("B", "C");
            graph.AddEdge("A", "C").Attr.Color = Microsoft.Msagl.Drawing.Color.Green;
            graph.FindNode("A").Attr.FillColor = Microsoft.Msagl.Drawing.Color.Magenta;
            graph.FindNode("B").Attr.FillColor = Microsoft.Msagl.Drawing.Color.MistyRose;
            Microsoft.Msagl.Drawing.Node c = graph.FindNode("C");
            c.Attr.FillColor = Microsoft.Msagl.Drawing.Color.PaleGreen;
            c.Attr.Shape = Microsoft.Msagl.Drawing.Shape.Diamond;
            GraphViewer graphViewer = new GraphViewer();
            graph.Attr.LayerDirection = LayerDirection.LR;
            //graph.LayoutAlgorithmSettings.EdgeRoutingSettings.EdgeRoutingMode = EdgeRoutingMode.Rectilinear;

            var global = (SugiyamaLayoutSettings)graph.LayoutAlgorithmSettings;
            var local = (SugiyamaLayoutSettings)global.Clone();
            local.Transformation = PlaneTransformation.Rotation(Math.PI / 2);
            graph.LayoutAlgorithmSettings = local;


            graphViewer.Graph = graph;
            graphViewer.BindToPanel(graphViewerPanel);  
            Main.Children.Add(graphViewerPanel);  


        }
    }
}
