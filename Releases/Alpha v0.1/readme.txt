IronArc - Alpha v0.1
Released January 10, 2018

==== Overview ====

IronArc is a virtual processor architecture that runs programs written using a new instruction set. This release includes software to run and debug IronArc virtual machines and to assemble files written in IronArc assembly into binary files that can be executed by the virtual machines.

==== License ====

This software is licensed under the MIT license. Please refer to license.txt at the root of this archive for more information.

==== Installation ====

This release is portable software, meaning it requires no installation. Merely extract this archive to any location you wish.

==== Usage ====

Run IronArcHost.exe to launch the host that can run VMs. New VMs can be started by clicking Add VM. You can select a program to run from the Sample Programs folder included with this archive, or you can use your own (see IronAssembler Usage below). For most purposes, you can leave the other fields as they are. Check the "IronArc.Hardware.TerminalDevice" checkbox in "Initial Hardware Devices" to attach a terminal to receive output from the VM. Click "OK" to start the VM.

The host then starts the VM, which appears in the VM pane. Depending on the program, the VM may run continuously, await input from the terminal, reach a halting state, or halt with an unhandled error. This status is displayed in the "State" column of the VM pane.

VMs can be paused and resumed by clicking "Pause VM"/"Resume VM". If a terminal was attached to the VM, you can see it by clicking "Terminal...". Input can be placed in the blue textbox and sent by pressing Enter if the VM is awaiting input. You can change which hardware devices are active by clicking "Hardware...".

Unfortunately, there is no support for removing or debugging VMs in this version.

==== IronAssembler Usage ====

IronArc runs on binary files called IronArc executables. These can be created using the included IronAssembler program, which converts IronArc assembly files into IronArc executables.

To create your own programs, simply compose a text file containing instructions and a string table. An example of a correct file:

	main:
		push QWORD str:0
		hwcall str:1
		push QWORD eax
		mov *ebx ecx
		addl edx 32 edx
		jmp main
		
	strings:
		0: "Hello, world!"
		1: "Terminal::WriteLine"

You can refer to the complete instruction listing (https://github.com/ChrisAkridge/IronArc/blob/master/Documentation/Complete%20Instruction%20Listing.txt) for the valid instructions that can be used in your assembly files. All assembly files must contain a string table with at least one entry. All memory addresses must be eight bytes long, in hexadecimal ("0x0123456789ABCDEF"). All numeric literals must be decimal numbers with no commas, and all string table entries must be prefixed with "str:". Any memory address or register can be used as a pointer by prefixing it with an asterisk ("*eax").

A full reference to the assembly format is coming soon.

After your file is complete, run "IronAssemblerCLI -i path//to//input.iasm -o path//to//output.iexe". An IronArc executable file will be produced that can be executed by IronArc virtual machines.

==== Disclaimers ====

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.

==== Feedback ====

Comment? Question? Bug report?
Please open an issue on our official GitHub page: https://github.com/ChrisAkridge/IronArc

==== Links ====

IronArc Project Page: https://github.com/ChrisAkridge/IronArc
IronAssembler Project Page: https://github.com/ChrisAkridge/IronAssembler
IronArc Specification: https://github.com/ChrisAkridge/IronArc/blob/master/Documentation/Specification.md

Licensed under the MIT license. Copyright © 2014-2018 Chris Akridge. Uses icons by Devendra Karkar (http://bit.ly/2mnGFuB) and Jozef Krajčovič (http://bit.ly/2mooV2u).