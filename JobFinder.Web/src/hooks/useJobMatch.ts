import { useEffect, useRef, useState } from 'react';
import { jobMatchService } from '../services/jobMatchService';

type Status = 'idle' | 'running' | 'done' | 'cancelled' | 'error';

export function useJobMatch() {
  const [status, setStatus] = useState<Status>('idle');
  const [messages, setMessages] = useState<string[]>([]);
  const eventSourceRef = useRef<EventSource | null>(null);

  useEffect(() => {
    jobMatchService.getStatus().then(({ isRunning }) => {
      if (isRunning) setStatus('running');
    });

    return () => {
      eventSourceRef.current?.close();
    };
  }, []);

  function start() {
    setMessages([]);
    setStatus('running');

    const es = jobMatchService.createEventSource();
    eventSourceRef.current = es;

    es.addEventListener('progress', (e: MessageEvent) => {
      setMessages(prev => [...prev, e.data]);
    });

    es.addEventListener('done', (e: MessageEvent) => {
      setMessages(prev => [...prev, e.data]);
      setStatus('done');
      es.close();
    });

    es.addEventListener('cancelled', (e: MessageEvent) => {
      setMessages(prev => [...prev, e.data]);
      setStatus('cancelled');
      es.close();
    });

    es.addEventListener('error', (e: MessageEvent) => {
      setMessages(prev => [...prev, e.data ?? 'An error occurred.']);
      setStatus('error');
      es.close();
    });
  }

  async function stop() {
    eventSourceRef.current?.close();
    await jobMatchService.stop();
    setStatus('cancelled');
  }

  return { status, messages, start, stop };
}
