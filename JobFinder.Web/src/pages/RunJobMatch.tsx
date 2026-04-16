import { useEffect, useRef } from 'react';
import { useJobMatch } from '../hooks';

export default function RunJobMatch() {
  const { status, messages, start, stop } = useJobMatch();
  const logRef = useRef<HTMLDivElement>(null);

  useEffect(() => {
    if (logRef.current)
      logRef.current.scrollTop = logRef.current.scrollHeight;
  }, [messages]);

  const running = status === 'running';

  return (
    <div style={{ maxWidth: 640, margin: '0 auto', padding: '1rem' }}>
      <h1>Run Job Match</h1>

      <div style={{ display: 'flex', gap: '0.5rem', alignItems: 'center', marginBottom: '1rem' }}>
        {!running && (
          <button onClick={start} disabled={running}>
            Run
          </button>
        )}
        {running && (
          <button onClick={stop} style={{ color: 'red' }}>
            Stop
          </button>
        )}
        {status !== 'idle' && (
          <span style={{ opacity: 0.6, fontSize: '0.9em' }}>
            {status === 'running' && 'Running...'}
            {status === 'done' && 'Completed'}
            {status === 'cancelled' && 'Stopped'}
            {status === 'error' && 'Error'}
          </span>
        )}
      </div>

      {messages.length > 0 && (
        <div
          ref={logRef}
          style={{
            background: '#1a1a1a',
            border: '1px solid #444',
            borderRadius: 8,
            padding: '0.75rem 1rem',
            height: 400,
            overflowY: 'auto',
            fontFamily: 'monospace',
            fontSize: '0.85em',
            lineHeight: 1.6,
            color: '#fff'
          }}
        >
          {messages.map((msg, i) => (
            <div key={i}>{msg}</div>
          ))}
        </div>
      )}
    </div>
  );
}
