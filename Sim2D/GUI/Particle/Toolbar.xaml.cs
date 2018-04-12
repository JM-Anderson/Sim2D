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
using System.Windows.Controls.Primitives;

using Sim2D.GUI.Particle.Tools;
using Sim2D.GUI.Particle.Tools.LinearForceTool;
using Sim2D.GUI.Particle.Tools.RulerTool;
using Sim2D.GUI.Particle.Tools.SelectTool;
using Sim2D.Simulations.Particles;

namespace Sim2D.GUI.Particle
{
    /// <summary>
    /// Interaction logic for Toolbar.xaml
    /// </summary>
    public partial class Toolbar : UserControl
    {
        // GUI
        private Canvas simCanvas;
        private StackPanel optionBar;
        private Dictionary<ToolType, ToggleButton> toolButtons = new Dictionary<ToolType, ToggleButton>();

        // Tools
        private Dictionary<ToolType, ToolLogic> userTools = new Dictionary<ToolType, ToolLogic>();
        public ToolType SelectedTool { get; private set; }

        // Intializers
        public Toolbar()
        {
            InitializeComponent();
        }
        public void Setup(Canvas simCanvas, ParticleSim particleSim, StackPanel optionBar)
        {
            this.simCanvas = simCanvas;
            this.optionBar = optionBar;

            // Register events with tools
            simCanvas.MouseLeftButtonDown += SimCanvas_MouseLeftButtonDown;
            simCanvas.MouseMove += SimCanvas_MouseMove;
            simCanvas.MouseLeftButtonUp += SimCanvas_MouseLeftButtonUp;

            // Track buttons in dictionary
            toolButtons.Add(ToolType.Particle, ParticleButton);
            toolButtons.Add(ToolType.Ruler, RulerButton);
            toolButtons.Add(ToolType.Select, SelectButton);
            toolButtons.Add(ToolType.LinearForce, LinearForceButton);

            // Initialize tools
            RegisterTools(
                new ParticleToolLogic(simCanvas, particleSim),
                new LinearForceLogic(simCanvas, particleSim),
                new RulerLogic(simCanvas),
                new SelectLogic(simCanvas, particleSim)
                );

            // Select tool to begin with
            SelectTool(ToolType.Particle);
        }

        // Toolbar setup
        private void RegisterTools(params ToolLogic[] tools)
        {
            foreach (ToolLogic tool in tools)
            {
                userTools.Add(tool.ToolType, tool);
                tool.OptionBar.Visibility = Visibility.Collapsed;
                optionBar.Children.Add(tool.OptionBar);
            }
        }

        // Select Tool
        private void ToolSelected(object sender, RoutedEventArgs e)
        {
            SelectTool((ToggleButton)sender);
        }
        private void SelectTool(ToolType toolType)
        {
            foreach (KeyValuePair<ToolType, ToggleButton> toolItem in toolButtons)
            {
                if (toolItem.Key != toolType)
                {
                    toolItem.Value.IsChecked = false;
                    userTools[toolItem.Key].OptionBar.Visibility = Visibility.Collapsed;
                }
                else
                {
                    toolItem.Value.IsChecked = true;
                    userTools[toolItem.Key].OptionBar.Visibility = Visibility.Visible;
                    SelectedTool = toolItem.Key;
                }
            }
        }
        private void SelectTool(ToggleButton button)
        {
            foreach (KeyValuePair<ToolType, ToggleButton> toolItem in toolButtons)
            {
                if (toolItem.Value != button)
                {
                    if (toolItem.Value.IsChecked == true)
                    {
                        toolItem.Value.IsChecked = false;
                        userTools[toolItem.Key].OptionBar.Visibility = Visibility.Collapsed;
                        userTools[toolItem.Key].Unselected();
                    }
                }
                else
                {
                    toolItem.Value.IsChecked = true;
                    userTools[toolItem.Key].OptionBar.Visibility = Visibility.Visible;
                    userTools[toolItem.Key].Selected();
                    SelectedTool = toolItem.Key;
                }
            }
        }

        // Events for tools to capture
        private void SimCanvas_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            userTools[SelectedTool].LeftMouseDown(sender, e);
        }
        private void SimCanvas_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                userTools[SelectedTool].LeftMouseDragging(sender, e);
            }
        }
        private void SimCanvas_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            userTools[SelectedTool].LeftMouseUp(sender, e);
        }
    }
}
