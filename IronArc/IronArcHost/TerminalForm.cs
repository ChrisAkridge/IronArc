using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronArc.Hardware;

namespace IronArcHost
{
    public partial class TerminalForm : Form, ITerminal
    {
        private enum InputWaitingState
        {
            NotWaiting,
            WaitingForChar,
            WaitingForLine,
            InputReceived
        }

        public Guid MachineId { get; set; }
        public bool CanPerformWaitingRead => (SynchronizationContext.Current == null);	// if it's null, code is running on the VM thread

        private InputWaitingState waitingState = InputWaitingState.NotWaiting;
        private ManualResetEvent waitHandle = new ManualResetEvent(false);
        private string receivedInput;

        public TerminalForm()
        {
            InitializeComponent();
        }

        public void Write(string text)
        {
            if (TextTerminalWindow.InvokeRequired)
            {
                TextTerminalWindow.Invoke(new MethodInvoker(() => TextTerminalWindow.AppendText(text)));
            }
            else
            {
                TextTerminalWindow.AppendText(text);
            }
        }

        public void WriteLine(string text) => Write(string.Concat(text, Environment.NewLine));

        public char Read()
        {
            // How we block the VM while waiting for a keypress:
            //	1. Set waitingState to WaitingForInput
            //	2. Since the code calling ReadChar() is running on the VM thread, we can wait on
            //	   input using an AutoResetEvent
            //	3. The TextBoxInput_KeyDown handler runs on the UI thread and can set waitingState
            //	   to InputReceived
            //	4. VM thread sees that input is received and can invoke code to read the input
            //	5. It then submits it to the TerminalDevice, which writes it to the stack

            waitingState = InputWaitingState.WaitingForChar;
            waitHandle.WaitOne();

            // After the above line, the input should be available.

            waitHandle.Reset();
            return receivedInput[0];
        }

        public string ReadLine()
        {
            waitingState = InputWaitingState.WaitingForLine;
            waitHandle.WaitOne();

            waitHandle.Reset();
            return receivedInput;
        }

        public char NonWaitingRead()
        {
            var inputForm = new DebugTerminalInputForm(DebugTerminalInputForm.DebugTerminalInputType.Character,
                MachineId);
            inputForm.ShowDialog();
            return inputForm.Input[0];
        }

        public string NonWaitingReadLine()
        {
            var inputForm = new DebugTerminalInputForm(DebugTerminalInputForm.DebugTerminalInputType.Line,
                MachineId);
            inputForm.ShowDialog();
            return inputForm.Input;
        }

        private void CMISaveOutput_Click(object sender, EventArgs e)
        {
            if (SFDSaveOutput.ShowDialog() == DialogResult.OK)
            {
                string filePath = SFDSaveOutput.FileName;
                File.WriteAllText(filePath, TextTerminalWindow.Text);
            }
        }

        private void TerminalForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Terminals are tied to the VM, so we can't close them with the X
            e.Cancel = true;
            Hide();
        }

        private void TextBoxInput_KeyDown(object sender, KeyEventArgs e)
        {
            if (waitingState == InputWaitingState.WaitingForChar)
            {
                receivedInput = TextBoxInput.Text == "" ? "\r\n" : TextBoxInput.Text[0].ToString();
                TextBoxInput.Text = "";
                waitingState = InputWaitingState.NotWaiting;

                waitHandle.Set();
            }
            else if (e.KeyCode == Keys.Enter && waitingState == InputWaitingState.WaitingForLine)
            {
                receivedInput = TextBoxInput.Text == "" ? "\r\n" : TextBoxInput.Text;
                TextBoxInput.Text = "";
                waitingState = InputWaitingState.NotWaiting;

                waitHandle.Set();
            }
        }
    }
}
