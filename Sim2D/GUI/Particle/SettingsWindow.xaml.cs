﻿using System;
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
using System.Windows.Shapes;
using System.ComponentModel;

using Sim2D.Simulations.Particles;

namespace Sim2D.GUI.Particle
{
    public struct SimTime
    {
        public SimTime(double FPS, double TimeInterval)
        {
            this.FPS = FPS;
            this.TimeInterval = TimeInterval;
        }
        public double FPS;
        public double TimeInterval;
    }

    /// <summary>
    /// Interaction logic for SettingsWindow.xaml
    /// </summary>
    public partial class SettingsWindow : Window
    {
        private ParticleSimulator pSimPage;
        private ParticleSim particleSim;

        public SettingsWindow(ParticleSimulator pSimPage, ParticleSim particleSim)
        {
            this.pSimPage = pSimPage;
            this.particleSim = particleSim;

            InitializeComponent();

            // Register events
            Closing += OnClosing;
            TrailsToggleButton.Click += (s, e) =>
            {
                particleSim.ToggleTrails();
                TrailsToggleButton.Content = (TrailsToggleButton.IsChecked ?? false) ? "On" : "Off";
            };

            // Time interval events
            RealTimeCheckBox.Checked += (s, e) => TimeStepUpDown.IsEnabled = false;
            RealTimeCheckBox.Unchecked += (s, e) => TimeStepUpDown.IsEnabled = true;
        }

        private void OnClosing(object sender, CancelEventArgs e)
        {
            Hide();
            e.Cancel = true;
        }

        // FPS
        private DateTime lastFrameTime = DateTime.Now;
        public SimTime GetTime()
        {
            // Find new frame time and delta T
            DateTime currentFrameTime = DateTime.Now;
            double frameDelta = (currentFrameTime - lastFrameTime).TotalSeconds;
            lastFrameTime = currentFrameTime;

            // Find time interval
            double timeInterval;
            if (RealTimeCheckBox.IsChecked ?? true)
            {
                timeInterval = frameDelta;
                TimeStepUpDown.Value = frameDelta * 1000;
            }
            else
            {
                timeInterval = (TimeStepUpDown.Value ?? 0) / 1000;
            }

            return new SimTime(
                FPS: 1 / frameDelta,
                TimeInterval: timeInterval);
        }
    }
}