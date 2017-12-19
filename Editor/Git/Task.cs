using System;
using System.Threading;

namespace Exodrifter.Yggdrasil
{
	public abstract class Task
	{
		private Thread thread;

		/// <summary>
		/// The name of this task.
		/// </summary>
		public string Name { get; private set; }

		/// <summary>
		/// True if execution of this task has started.
		/// </summary>
		public bool HasStarted { get { return thread != null; } }

		/// <summary>
		/// True if the execution of this task has finished.
		/// </summary>
		public bool IsDone { get; private set; }

		/// <summary>
		/// A value, between 0 and 1 inclusive, indicating progress.
		/// </summary>
		public float Progress { get; protected set; }

		/// <summary>
		/// If non-null, the exception thrown by the task.
		/// </summary>
		public Exception Exception { get; private set; }

		/// <summary>
		/// Creates a new task with the specified name.
		/// </summary>
		/// <param name="name">The name of the task.</param>
		public Task(string name)
		{
			Name = name;
		}

		/// <summary>
		/// Starts execution of the task.
		/// </summary>
		public void Start()
		{
			if (!HasStarted)
			{
				thread = new Thread(new ThreadStart(DoTask));
				thread.Start();
			}
		}

		private void DoTask()
		{
			try
			{
				Perform();
			}
			catch (Exception e)
			{
				Exception = e;
			}
			finally
			{
				IsDone = true;
			}
		}

		protected abstract void Perform();
	}
}
