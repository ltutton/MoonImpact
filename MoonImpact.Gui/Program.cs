﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoonImpact.Gui
{
    using SharpDX.Direct3D;
    using SharpDX.Direct3D11;
    using SharpDX.DXGI;
    using SharpDX.Windows;
    using Device = SharpDX.Direct3D11.Device;

    class Program
    {
        static void Main(string[] args)
        {
            var form = new RenderForm("Moon Impact Simulator");
            form.Width = 1280;
            form.Height = 1024;

            // SwapChain description
            var desc = new SwapChainDescription
            {
                BufferCount = 1,
                ModeDescription =
                    new ModeDescription(form.ClientSize.Width, form.ClientSize.Height,
                        new Rational(60, 1), Format.R8G8B8A8_UNorm),
                IsWindowed = true,
                OutputHandle = form.Handle,
                SampleDescription = new SampleDescription(1, 0),
                SwapEffect = SwapEffect.Discard,
                Usage = Usage.RenderTargetOutput
            };

            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.None, desc, out var device, out var swapChain);
            var context = device.ImmediateContext;

            // Ignore all windows events
            var factory = swapChain.GetParent<Factory>();
            factory.MakeWindowAssociation(form.Handle, WindowAssociationFlags.IgnoreAll);

            var appLoop = new MoonAppLoop(form, device, swapChain);
            appLoop.IsFixedTimeStep = true;

            // Main loop
            RenderLoop.Run(form, () =>
            {
                appLoop.Tick();
            });

            device.Dispose();
            swapChain.Dispose();
            factory.Dispose();
        }
    }
}
