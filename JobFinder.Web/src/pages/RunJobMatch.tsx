import { useEffect, useRef } from 'react';
import { useJobMatch } from '../hooks';

const statusLabel: Record<string, string> = {
  running: 'Running...',
  done: 'Completed',
  cancelled: 'Stopped',
  error: 'Error',
};

export default function RunJobMatch() {
  const { status, messages, start, stop } = useJobMatch();
  const logRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (logRef.current)
      logRef.current.scrollTop = logRef.current.scrollHeight;
  }, [messages]);

  const running = status === 'running';

  return (
    <div className="flex flex-col gap-6">
      <div>
        <h1 className="text-3xl font-bold text-white">Run Job Match</h1>
        <p className="text-slate-400 mt-1">Start a search to find and score job postings against your profile.</p>
      </div>

      <div className="flex items-center gap-4">
        {!running ? (
          <button
            onClick={start}
            className="bg-indigo-600 hover:bg-indigo-500 text-white px-5 py-2 rounded-lg font-medium transition-colors"
          >
            Run
          </button>
        ) : (
          <button
            onClick={stop}
            className="bg-red-600 hover:bg-red-500 text-white px-5 py-2 rounded-lg font-medium transition-colors"
          >
            Stop
          </button>
        )}
        {status !== 'idle' && (
          <span className={`text-sm ${status === 'done' ? 'text-emerald-400' : status === 'error' ? 'text-red-400' : 'text-slate-400'}`}>
            {statusLabel[status]}
          </span>
        )}
      </div>

      {messages.length > 0 && (
        <div
          ref={logRef}
          className="bg-slate-900 border border-slate-700 rounded-xl p-4 h-96 overflow-y-auto font-mono text-sm text-slate-300 leading-relaxed"
        >
          {messages.map((msg, i) => (
            <div key={i}>{msg}</div>
          ))}
        </div>
      )}
    </div>
  );
}
