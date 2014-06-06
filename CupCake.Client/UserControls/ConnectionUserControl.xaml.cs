﻿using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Threading;
using CupCake.Protocol;
using Timer = System.Timers.Timer;

namespace CupCake.Client.UserControls
{
    /// <summary>
    ///     Interaction logic for ConnectionUserControl.xaml
    /// </summary>
    public partial class ConnectionUserControl
    {
        private readonly ClientHandle _handle;
        private bool _cancelClose;
        public bool IsDebug { get; set; }

        public ConnectionUserControl(ClientHandle handle)
        {
            this.InitializeComponent();

            this.Loaded += this.ConnectionUserControl_Loaded;

            this._handle = handle;
            this._handle.ReceiveOutput += this.ClientOutput;
            this._handle.ReceiveClose += this._handle_ConnectionClose;
            this._handle.ReceiveTitle += this._handle_ReceiveTitle;
            this._handle.ReceiveWrongAuth += this._handle_ReceiveWrongAuth;
        }

        private void KeepOpenButton_OnClick(object sender, RoutedEventArgs e)
        {
            this.ClosingGrid.Visibility = Visibility.Collapsed;
            this._cancelClose = true;
        }

        public void Close()
        {
            this._handle.DoSendClose();
            this._cancelClose = true;
        }

        public void RemoveTab()
        {
            var parent = (TabItem)this.Parent;
            var parentParent = (TabControl)parent.Parent;
            parentParent.Items.Remove(parent);
        }

        public void Clear()
        {
            this.OutputTextBox.Text = "--- Log Cleared ---";
        }

        private void _handle_ReceiveWrongAuth()
        {
            this.AppendText("ERROR: Wrong authentication data provided.");
        }

        private void _handle_ReceiveTitle(Title title)
        {
            Dispatch.Invoke(() => ((TabItem)this.Parent).Header = title.Text);
        }

        private void _handle_ConnectionClose()
        {
            this.AppendText("--- Connection terminated ---");
            Dispatch.Invoke(() => {
                ((TabItem)this.Parent).Header += " (Disconnected)";

                if (IsDebug)
                {
                    this.ClosingGrid.Visibility = Visibility.Visible;

                    var timer = new DispatcherTimer(DispatcherPriority.Normal, this.Dispatcher)
                    {
                        Interval = new TimeSpan(0, 0, 30)
                    };
                    timer.Tick += (sender, args) =>
                    {
                        if (!this._cancelClose)
                            this.RemoveTab();

                        timer.Stop();
                    };
                    timer.Start();
                }
            });
        }

        private void ConnectionUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
        }

        private void ClientOutput(Output output)
        {
            this.AppendText(output.Text);
        }

        private void AppendText(string text)
        {
            Dispatch.Invoke(() => this.OutputTextBox.AppendText(Environment.NewLine + text));
        }

        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.OutputTextBox.ScrollToEnd();
                this._handle.DoSendInput(this.InputTextBox.Text);
                this.InputTextBox.Clear();
            }
        }

        private void OutputTextBox_OnMouseUp(object sender, MouseButtonEventArgs e)
        {
            if (this.OutputTextBox.SelectedText.Length == 0)
            {
                this.InputTextBox.Focus();
            }
        }
    }
}