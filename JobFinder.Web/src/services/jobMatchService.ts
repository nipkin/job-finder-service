const BASE_URL = '/api/jobmatch';

async function stop(): Promise<void> {
  await fetch(`${BASE_URL}/stop`, { method: 'POST' });
}

async function getStatus(): Promise<{ isRunning: boolean }> {
  const response = await fetch(`${BASE_URL}/status`);
  return response.json();
}

function createEventSource(): EventSource {
  return new EventSource(`${BASE_URL}/run`);
}

export const jobMatchService = { stop, getStatus, createEventSource };
