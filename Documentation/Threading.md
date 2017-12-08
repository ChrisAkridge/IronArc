# IronArc: VMs and Threading

The IronArc host lets the user inspect and modify the state of IronArc VMs. But, since an IronArc VM executes instructions until it reaches an `end` instruction, doing this all in a single thread would block the UI and cause it to freeze. In this document, I try to resolve this problem.

## What is Needed

First, we have a main/UI thread. This runs the launcher, terminal windows, and the debugger. These "view" types all tie into a loose "model" layer which owns each actual VM instance.

Any actively running VM should get a separate thread. Any VM with a thread is actively executing instructions. Each VM, as well as the "model" layer, also owns a concurrent message queue. Messages are enqueued on these queues by either the main/UI thread, or by the VM. The owner of the queue can then dequeue the message and process it on its own thread. Cross thread messages contain information about what kind of message it is and data on how to use it.

A VM that is running may need to be stopped before it reaches an `end` instruction. For example, the user may pause the VM or drop into the debugger for it. Cross thread messages can cause the VM to pause between the execution of instructions and terminate the thread its running on. The VM's state is preserved, and the VM is said to be "yielded".

Handling messages on the main/UI thread is fairly simple - just run a `Timer` every so often to poll for messages. It is equally simple to check for messages in the VM layer - every X instructions, check to see if there's anything on the message queue. This is polling, and ordinarily I wouldn't typically want to use this for performance concerns, but we'll start with this and benchmark later.

## Why Threads?

Modern C# does have quite a few useful tools for asynchonous and parallel programming; however, I feel that threads fit best here. Tasks and async methods tend to describe a process, while perhaps long-running, that is built to return a result after a certain amount of time. IronArc VMs don't really follow this principle, as they're arbitrarily-long-running things that may never terminate on their own. As a result, a simple threading system seems more suitable.